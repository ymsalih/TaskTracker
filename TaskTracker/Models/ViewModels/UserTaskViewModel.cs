using Microsoft.VisualBasic;

namespace TaskTracker.Models.ViewModels
{// bu sınıf razor page için veri taşıyıxı görevi görür 
    // ef core ile ilişkili değildir 
    // amaç zaten ef ile almayıp çünkü o direk razora bağlı olmasın diye bu şekilde güncelledik 
    public class UserTaskViewModel
    {
        public string TaskTitle { get; set; }
        public string TaskDetail { get; set; }
        public DateTime? DueDate { get; set; }
        public string TaskStatus {  get; set; }
        
      
        

        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectFeature { get; set; }

    }
}
