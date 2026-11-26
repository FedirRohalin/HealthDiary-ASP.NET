using HealthDiary.Data.Models;
using HealthDiary.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HealthDiary.Data.Interfaces;
using HealthDiary.Data.Repositories;
using HealthDiary.Web.Configurations;
using System.Threading.RateLimiting;

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

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IHealthDiaryRepository, HealthDiaryRepository>();
builder.Services.AddControllersWithViews();

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

app.UseRouting();

app.UseAuthorization();

app.UseAuthorization();
app.UseRateLimiter();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
