using System.Security.Claims;
using HealthDiary.Data.Interfaces;
using HealthDiary.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthDiary.Web.Controllers
{
    // Завдання 2: доступ до «Архіву матеріалів» лише для IsVerifiedClient
    [Authorize(Policy = "VerifiedClientOnly")]
    public class MaterialsController : Controller
    {
        private readonly IHealthDiaryRepository _repository;
        private readonly IAuthorizationService _authorizationService;

        public MaterialsController(
            IHealthDiaryRepository repository,
            IAuthorizationService authorizationService)
        {
            _repository = repository;
            _authorizationService = authorizationService;
        }

        public IActionResult Index()
        {
            var materials = _repository.ReadAll<Material>().ToList();
            return View(materials);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Material material)
        {
            material.AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _repository.AddAsync(material);
            return RedirectToAction(nameof(Index));
        }

        // Завдання 3: імперативна ресурсна авторизація (редагувати може лише автор)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var material = await _repository.ReadSingleAsync<Material>(m => m.Id == id);
            if (material == null) return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, material, "CanEditMaterial");
            if (!authResult.Succeeded)
                return Forbid();

            return View(material);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Material updated)
        {
            var existing = await _repository.ReadSingleAsync<Material>(m => m.Id == updated.Id);
            if (existing == null) return NotFound();

            var authResult = await _authorizationService.AuthorizeAsync(User, existing, "CanEditMaterial");
            if (!authResult.Succeeded)
                return Forbid();

            updated.AuthorId = existing.AuthorId;
            await _repository.UpdateAsync(updated);
            return RedirectToAction(nameof(Index));
        }
    }
}