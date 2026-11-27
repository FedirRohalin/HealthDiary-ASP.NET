using HealthDiary.Data.Models;
using HealthDiary.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HealthDiary.Data.Interfaces;
using HealthDiary.Data.Repositories;
using HealthDiary.Web.Configurations;
using System.Threading.RateLimiting;
using HealthDiary.Web.Authorization;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Налаштування Partitioned Rate Limiting (Завдання 6)
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var isAuthenticated = httpContext.User.Identity?.IsAuthenticated == true;

        if (isAuthenticated)
        {
            var userName = httpContext.User.Identity!.Name ?? "authenticated_user";
            return RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: userName,
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 20,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0
                });
        }

        var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: clientIp,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });
});


// Налаштування джерел конфігурації (від базового до найвищого пріоритету)
builder.Configuration.Sources.Clear();
builder.Configuration
    .AddJsonFile("sharedSettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Прив'язка до строго типізованої моделі та реєстрація Singleton у DI
var appConfig = builder.Configuration.Get<AppConfiguration>()
    ?? throw new InvalidOperationException("AppConfiguration could not be bound.");

builder.Services.AddSingleton(appConfig);

// Add services to the container.
var connectionString = appConfig.ConnectionStrings.DefaultConnection
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."); 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IHealthDiaryRepository, HealthDiaryRepository>();
// Налаштування локалізації (Завдання 2, 4)
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var supportedCultures = new[] { "uk-UA", "en-US", "fr-FR" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("uk-UA")
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();
// Шляхи для перенаправлення при відсутності автентифікації / доступу
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Налаштування політик авторизації (Завдання 1–5)
builder.Services.AddAuthorization(options =>
{
    // Завдання 1: глобальна вимога автентифікації для всіх сторінок (крім [AllowAnonymous])
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    // Завдання 2: Архів матеріалів лише для IsVerifiedClient
    options.AddPolicy("VerifiedClientOnly", policy =>
        policy.RequireClaim("IsVerifiedClient", "true"));

    // Завдання 3: Ресурсна авторизація (редагування лише автором)
    options.AddPolicy("CanEditMaterial", policy =>
        policy.AddRequirements(new IsMaterialOwnerRequirement()));

    // Завдання 4: Преміум (WorkingHours >= 100)
    options.AddPolicy("PremiumAccess", policy =>
        policy.AddRequirements(new MinimumWorkingHoursRequirement(100)));

    // Завдання 5: Форум (хоча б одне з 3 тверджень)
    options.AddPolicy("ForumAccess", policy =>
        policy.AddRequirements(new ForumAccessRequirement()));
});

// Реєстрація обробників авторизації у DI
builder.Services.AddScoped<IAuthorizationHandler, IsMaterialOwnerHandler>();
builder.Services.AddScoped<IAuthorizationHandler, MinimumWorkingHoursHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ForumAccessHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRequestLocalization();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization(); 
app.UseRateLimiter();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
