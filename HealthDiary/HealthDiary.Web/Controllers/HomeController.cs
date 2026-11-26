using HealthDiary.Data.Interfaces;
using HealthDiary.Data.Models;
using HealthDiary.Web.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace HealthDiary.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHealthDiaryRepository _repository;
        private readonly AppConfiguration _config;

        public HomeController(
            ILogger<HomeController> logger,
            IHealthDiaryRepository repository,
            AppConfiguration config)
        {
            _logger = logger;
            _repository = repository;
            _config = config;
        }

        public IActionResult Index()
        {
            var users = _repository.All<ApplicationUser>().ToList();

            // Передача параметрів конфігурації у ViewData для Footer
            ViewData["ApplicationName"] = _config.ApplicationName;
            ViewData["PrinterModel"] = _config.StepLabSettings.DefaultPrinterModel;

            return View(users);
        }

        public IActionResult Privacy()
        {
            ViewData["ApplicationName"] = _config.ApplicationName;
            ViewData["PrinterModel"] = _config.StepLabSettings.DefaultPrinterModel;
            return View();
        }
    }
}