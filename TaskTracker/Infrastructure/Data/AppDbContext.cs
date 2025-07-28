using Microsoft.EntityFrameworkCore; // EF Core özelliklerini kullanmak için gerekli
using System.Collections.Generic; // List<T> gibi koleksiyonlar için gerekli
using TaskTracker.Core; // Veritabanı modellerine erişim sağlar

namespace TaskTracker.Infrastructure.Data // Mantıksal proje organizasyonu
{
    public class AppDbContext : DbContext // EF Core'un temel veri yöneticisi sınıfı
    {
        // Kurucu metod, DI üzerinden bağlantı ayarlarını alır
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { } // connection string ile veritabanı bağlantısı sağlanır

        // Tablolara karşılık gelen DbSet tanımları
        public DbSet<User> Users { get; set; } // Kullanıcılar tablosu
        public DbSet<Project> Projects { get; set; } // Projeler tablosu
        public DbSet<TaskItem> Tasks { get; set; } // Görevler tablosu
        public DbSet<TaskUser> TaskUsers { get; set; } // Görev–Kullanıcı eşleşme tablosu (many-to-many)

        // Model yapılandırma metodu
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // EF Core'un varsayılan kurallarını koru

            // 👤 Görev – Tek Kullanıcı (AssignedUserId ile birebir ilişkili)
            modelBuilder.Entity<TaskItem>() // Görev modeli üzerinde çalışıyoruz
                .HasOne(t => t.AssignedUser) // Görev, bir kullanıcıya atanmış olabilir
                .WithMany() // Bir kullanıcı birden fazla görevde çalışabilir
                .HasForeignKey(t => t.AssignedUserId) // ForeignKey tanımı
                .OnDelete(DeleteBehavior.SetNull); // Kullanıcı silinirse görev boşa düşsün (silinmesin)

            // 👥 Görev – Çoklu Kullanıcı İlişkisi (many-to-many ara tablosu)
            modelBuilder.Entity<TaskUser>() // Ara tablo: görev ile kullanıcı arasında eşleşme tutar
                .HasKey(tu => new { tu.TaskId, tu.UserId }); // Composite primary key: her görev-kullanıcı eşleşmesi benzersiz olmalı

            modelBuilder.Entity<TaskUser>() // Görev bağlantısı
                .HasOne(tu => tu.Task) // Her eşleşme bir görevle bağlıdır
                .WithMany(t => t.TaskUsers) // Bir görev birden fazla kullanıcı ile eşleşebilir
                .HasForeignKey(tu => tu.TaskId); // Görev FK’si

            modelBuilder.Entity<TaskUser>() // Kullanıcı bağlantısı
                .HasOne(tu => tu.User) // Her eşleşme bir kullanıcı ile bağlıdır
                .WithMany(u => u.TaskUsers) // Bir kullanıcı birçok görevle ilişkilendirilebilir
                .HasForeignKey(tu => tu.UserId); // Kullanıcı FK’si
        }
    }
}