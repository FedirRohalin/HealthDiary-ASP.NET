using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HealthDiary.Web.Controllers
{
    [AllowAnonymous]
    public class SelectLanguageController : Controller
    {
        private readonly IOptions<RequestLocalizationOptions> _locOptions;

        public SelectLanguageController(IOptions<RequestLocalizationOptions> locOptions)
        {
            _locOptions = locOptions;
        }

        [HttpGet]
        public IActionResult Index(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var cultureItems = _locOptions.Value.SupportedUICultures?.ToList();
            return View(cultureItems);
        }

        [HttpPost]
        public IActionResult SetLanguage(string cultureName, string? returnUrl = null)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cultureName)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);
        }
    }
}