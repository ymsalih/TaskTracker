using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace TaskTracker.Core
{
    public class TaskItem
    {
        public int Id { get; set; }

        // 🔗 Görev ait olduğu proje
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        [Required(ErrorMessage = "Görev Başlığı zorunludur.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Görev açıklaması zorunludur.")]
        [Display(Name = "Görev Açıklaması")]
        public string Description { get; set; }

        [Display(Name = "Görev Önceliği")]
        public string Priority { get; set; }

        [Display(Name = "Görev Durumu")]
        public string Status { get; set; }

        // 🆕 Yeni Alan: Görev Kime Atandı
        [Display(Name = "Atanan Kullanıcı ID")]
        public int? AssignedUserId { get; set; } // bu şekilde null olabilir de diyoruz aksi takdirde not null olarak geçiyor bu da hata sebebiyeti verebiliyor 

        [Display(Name ="Teslim Tarihi")]
        public DateTime? DueDate { get; set; } 
        // Bu şekilde ekledik çünkü kullanıcı proje bitiş tarihini de görsün diye
        // ayrıca hata almamak için eklendi 

        // (Opsiyonel) Navigation Property
        public User? AssignedUser { get; set; }
       
    }
}