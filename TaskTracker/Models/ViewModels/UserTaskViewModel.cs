namespace TaskTracker.Models.ViewModels
{// bu sınıf razor page için veri taşıyıxı görevi görür 
    // ef core ile ilişkili değildir 
    public class UserTaskViewModel
    {
        public string TaskTitle { get; set; }
        public string TaskDetail { get; set; }
        public DateTime? DueDate { get; set; }
      

        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectFeature { get; set; }

    }
}
