using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Core
{
    // Domain katmanıdır uygulamanın temel modellerini barındıran klasördür 
    // burası genellikle veri nedir? sorusunun cevabıdır 
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Proje adı zorunludur.")]
        [Display(Name = "Proje Adı")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Proje açıklaması zorunludur.")]

        [Display(Name = "Projenin Açıklaması")]
        public string Description { get; set; } = string.Empty; // başlangıç değeri null atanmasını engeller "" ile aynıdır çünkü kontrol yapıyoruz ya null ise diye hiç proje yoksa hata çıkmasın diye 

        [Required(ErrorMessage ="Proje Özellikleri Girilmesi Zorunludur.")]
        [Display(Name ="Projenin Özellikleri ")]
        public string Feature { get; set; }

        [Required(ErrorMessage = "Başlangıç tarihi gereklidir.")] // required ise boş bir kutucuğun kalmaması için uyarı verir 
        [Display(Name = "Projenin Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Bitiş tarihi gereklidir.")] // [Required] bu özellik boş bırakılamaz demektir 
        [Display(Name = "Projenin Bitiş Tarihi")]
        public DateTime EndDate { get; set; }

        public List<User> Members { get; set; } = new();
    }
}