using InfoProtection.Models;
using Microsoft.EntityFrameworkCore;

namespace InfoProtection.Servises
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<EncryptedMessage> EncryptedMessages { get; set; }
        public DbSet<NfaCode> NfaCodes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка внешних ключей и связей
            modelBuilder.Entity<EncryptedMessage>()
                .HasOne(e => e.User)               // Связь с пользователем
                .WithMany(u => u.EncryptedMessages) // Один пользователь - много сообщений
                .HasForeignKey(e => e.UserId)       // Указываем внешний ключ
                .OnDelete(DeleteBehavior.Cascade);  // Каскадное удаление

            modelBuilder.Entity<NfaCode>()
                .HasOne(n => n.User)               // Связь с пользователем
                .WithMany(u => u.NfaCodes)         // Один пользователь - много кодов
                .HasForeignKey(n => n.UserId)      // Указываем внешний ключ
                .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление
        }

    }
}
