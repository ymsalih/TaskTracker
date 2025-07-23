using Microsoft.EntityFrameworkCore; // kütüphaneyi dahil etmek için bu olmadam DbContext ve DbSet gibi yapılara erişemem 
using System.Collections.Generic; // ileride kullanılacak list<> türleri çin gerekli
using TaskTracker.Core; // veritabanı bağlayacağımız model sınıflarını içeren namespace

namespace TaskTracker.Infrastructure.Data // proje yapısındaki mantıksal ayrımı temsil eder 
{
    // veritabanı işlemlerini yöneten klasördür 
    // bu dosya modelleri veritabanına bağlayan bir köprü gibi çalışır 
    // AppSbContext Entity framework Core ile veritabanı bağlantısını sağlar 
  // tüm modelleri fiziksel veritabanı tablolarıyla eşleştiren yerlerdir 
    public class AppDbContext : DbContext // DbContext ten miras alıyoruz bu sınıf ile veritabanında işlemler yapabiliyoruz
    {
        
        // bağlantı bilgilerini ve konfigrasyonu startup.cs veya program.cs gibi yerlerden alıp EF Cora ileten kurucu method 
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { } // options nesnesi appstetting.json da belirttiğimiz connection string ile veritabanına bağlanır 
       // Bu yapı Dependency Injection sayesinde otomatik olarak yapılandırılır ve yönetilir.
        // veritabanında ki tabloları temsil eder 
        // her biri veritabanında bir tabloya denk gelir 
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        
        // bu tanuımlar sayesinde veritabanında tablolar oluşturur ve bu tablolara sorgular gönderebiliriz  
    }
}
// Dependency Injection ise bir sınıfın ihtiyaç duyduğu bağımlılıkları kendi içinde oluşturmadan dışarıdan alması anlamına gelir  