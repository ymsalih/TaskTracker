using Microsoft.EntityFrameworkCore; // kütüphaneyi dahil etmek için bu olmadam DbContext ve DbSet gibi yapılara erişemem 
using System.Collections.Generic; // ileride kullanılacak list<> türleri çin gerekli
using TaskTracker.Core; // veritabanı bağlayacağımız model sınıflarını içeren namespace

// veritabanı işlemlerini yöneten klasördür 
// bu dosya modelleri veritabanına bağlayan bir köprü gibi çalışır 
// AppSbContext Entity framework Core ile veritabanı bağlantısını sağlar 
// tüm modelleri fiziksel veritabanı tablolarıyla eşleştiren yerlerdir 

namespace TaskTracker.Infrastructure.Data // proje yapısındaki mantıksal ayrımı temsil eder 
{
 
    public class AppDbContext : DbContext // DbContext ten miras alıyoruz bu sınıf ile veritabanında işlemler yapabiliyoruz
    {

        // bağlantı bilgilerini ve konfigrasyonu startup.cs veya program.cs gibi yerlerden alıp EF Cora ileten kurucu method 
        // Bu yapı Dependency Injection sayesinde otomatik olarak yapılandırılır ve yönetilir.
        // veritabanında ki tabloları temsil eder 
        // her biri veritabanında bir tabloya denk gelir 
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { } // options nesnesi appstetting.json da belirttiğimiz connection string ile veritabanına bağlanır 

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        // bu tanuımlar sayesinde veritabanında tablolar oluşturur ve bu tablolara sorgular gönderebiliriz  
        protected override void OnModelCreating(ModelBuilder modelBuilder) // veritabanı modellerini özelleştirmek için 
        {
            base.OnModelCreating(modelBuilder); // bağlantıları nasıl yöneteceğini söylediğimiz yerdir 
            // bunu ekleme amacımız ise EF Corun varsayılan kurallarını korumak için aksi takdirde hata alırız 
            modelBuilder.Entity<TaskItem>() // görevlerle ilgili konuşma olacak 
                .HasOne(t => t.AssignedUser) // bire bir ilişki vardır yani görev bir kullanıcıya atanmış 
                .WithMany() //kullanıcının birden fazla görevi olabilir 
                .HasForeignKey(t => t.AssignedUserId) // veritabanı bağlantısı olarak kullanır 
                .OnDelete(DeleteBehavior.SetNull); // 🔥 Anahtar davranışı burada artık kullanıcı silinirse görevdeki AssignedUserId otomatik olarak null kalır 
       // eğer bir kullanıcı silinirse ve ona bağlı görev varsa o görevi silme sadece referansını boşalt 
        // daha sonrasında migration ile bu güncel bilgileri veritabanı için uyguladık 
        // böylece ilişkiyi SetNull davranışıyla güncelledi 
        // oluşturulma nedeni normalde modelin sadece veriyi tanımlaması ama ilişki davranışlarını tanımlamaması olduğundan ekledik 
        }
    }

}
// Bu dosya EF Corenin ilişkileri yönettiği merkezdir
// Dependency Injection ise bir sınıfın ihtiyaç duyduğu bağımlılıkları kendi içinde oluşturmadan dışarıdan alması anlamına gelir  
// kısacası veritabanı kurallara göre çalışır biz de bu şekilde kuralları tanımlamış olduk 
// EF Core veritabanını oluştururken ve sorgu hazırlanırken bu dosyadaki kurallara göre davranır 