using HealthDiary.Data.Interfaces;
using HealthDiary.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HealthDiary.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHealthDiaryRepository _repository;

        // Впровадження залежностей (Dependency Injection) через конструктор
        public HomeController(ILogger<HomeController> logger, IHealthDiaryRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index()
        {
            // Використання репозиторію для отримання всіх користувачів
            var users = _repository.All<ApplicationUser>().ToList();

            return View(users);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}