using HealthDiary.Data.Interfaces;
using HealthDiary.Data.Models;
using HealthDiary.Web.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace HealthDiary.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHealthDiaryRepository _repository;
        private readonly AppConfiguration _config;
        private readonly IStringLocalizer<HomeController> _localizer; 

        public HomeController(
            ILogger<HomeController> logger,
            IHealthDiaryRepository repository,
            AppConfiguration config,                                   
            IStringLocalizer<HomeController> localizer)                
        {
            _logger = logger;
            _repository = repository;
            _config = config;
            _localizer = localizer;                                    
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var users = _repository.All<ApplicationUser>().ToList();

            // Передача параметрів конфігурації у ViewData для Footer
            ViewData["ApplicationName"] = _config.ApplicationName;
            ViewData["PrinterModel"] = _config.StepLabSettings.DefaultPrinterModel;

            // Передача локалізованого повідомлення з HomeController.resx
            ViewData["WelcomeMessage"] = _localizer["WelcomeMessage"]; 

            return View(users);
        }

        public IActionResult Privacy()
        {
            ViewData["ApplicationName"] = _config.ApplicationName;
            ViewData["PrinterModel"] = _config.StepLabSettings.DefaultPrinterModel;
            return View();
        }

        // Завдання 4: Преміум (WorkingHours >= 100)
        [Authorize(Policy = "PremiumAccess")]
        public IActionResult Premium()
        {
            return View();
        }

        // Завдання 5: Форум (IsMentor, IsVerifiedUser або HasForumAccess)
        [Authorize(Policy = "ForumAccess")]
        public IActionResult Forum()
        {
            return View();
        }
    }
}