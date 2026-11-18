using HealthDiary.Data.Data;
using HealthDiary.Data.Interfaces;
using HealthDiary.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.Data.Repositories
{
    public class HealthDiaryRepository : BaseSqlServerRepository<ApplicationDbContext>, IHealthDiaryRepository
    {
        public HealthDiaryRepository(ApplicationDbContext db) : base(db)
        {
        }

        // Кастомний метод із Завдання 5: Пошук користувача за email
        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await Db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}