using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Core
{
    public class User
    {
        // kullanıcı modelidir kullanıcı ile ilgili bilgileri tutar veritabanında ki users tabosuna denk gelir 
        public int Id {  get; set; } // kullanıcının benzersiz id 
       
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
         [Display(Name="Ad")] 
        public string Name { get; set; }

        [Required(ErrorMessage = "Rol alanı zorunludur.")]
        [Display(Name="Rol")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Kullanıcı Adı Girilmesi Zounludur.")]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre Girilmesi Zorunludur")]
        [Display(Name = "Şifre ")]
        public string Password { get; set; }

        [Required(ErrorMessage ="Kullanıcı Tipi")]
        [Display(Name="Kullanıcı Tipi girilmeli ")]
        public string UserType { get; set; }
    }
}
