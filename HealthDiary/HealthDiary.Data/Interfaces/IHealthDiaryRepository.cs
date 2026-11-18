using HealthDiary.Data.Models;

namespace HealthDiary.Data.Interfaces
{
    public interface IHealthDiaryRepository : IRepository
    {
        // Кастомний метод із Завдання 5
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
    }
}