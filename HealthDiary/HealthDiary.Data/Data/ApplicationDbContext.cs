using HealthDiary.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Колекції сутностей, які стануть таблицями в базі
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<GaitAnalysis> GaitAnalyses { get; set; }
        public DbSet<InsoleOrder> InsoleOrders { get; set; }
        public DbSet<Material> Materials { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Fluent API: Відношення 1:N (Один Лікар -> Багато Аналізів)
            builder.Entity<GaitAnalysis>()
                .HasOne(g => g.Doctor)
                .WithMany(d => d.GaitAnalyses)
                .HasForeignKey(g => g.DoctorId)
                .OnDelete(DeleteBehavior.Restrict); // Запобігає каскадному видаленню

            // Fluent API: Відношення 1:N (Один Пацієнт -> Багато Аналізів)
            builder.Entity<GaitAnalysis>()
                .HasOne(g => g.Patient)
                .WithMany(p => p.GaitAnalyses)
                .HasForeignKey(g => g.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Fluent API: Відношення 1:N (Один Аналіз -> Багато Замовлень)
            builder.Entity<InsoleOrder>()
                .HasOne(o => o.GaitAnalysis)
                .WithMany(g => g.InsoleOrders)
                .HasForeignKey(o => o.GaitAnalysisId);

            // Fluent API: Відношення M:N (Багато-до-багатьох: Замовлення <-> Матеріали)
            builder.Entity<InsoleOrder>()
                .HasMany(o => o.Materials)
                .WithMany(m => m.InsoleOrders)
                .UsingEntity(j => j.ToTable("InsoleOrderMaterials")); // Створення проміжної таблиці
        }
    }
}   