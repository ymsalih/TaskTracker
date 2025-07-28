namespace TaskTracker.Models.ViewModels
{
    // Bu sınıf Razor Page için veri taşıyıcıdır (EF Core ile doğrudan ilişkili değil)
    // Görev detaylarını ve proje bilgilerini taşır, ayrıca işlem yapmak için TaskId eklendi
    public class UserTaskViewModel
    {
       

        public string TaskTitle { get; set; }
        public string TaskDetail { get; set; }
        public DateTime? DueDate { get; set; }
        public string TaskStatus { get; set; }

        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectFeature { get; set; }
    }
}