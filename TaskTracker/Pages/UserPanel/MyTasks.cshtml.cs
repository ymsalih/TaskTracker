using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Infrastructure.Data;
using TaskTracker.Models.ViewModels; // ViewModel’i burada tanýmladýðýný varsayýyoruz

public class MyTasksModel : PageModel
{
    private readonly AppDbContext _context;
    public List<UserTaskViewModel> MyTasks { get; set; } // artýk farklý yerden verileri viewmodel ile filtrelemiþ halini veriyoruz 

    public MyTasksModel(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        var role = HttpContext.Session.GetString("UserType");
        var userId = HttpContext.Session.GetInt32("UserId");

        if (role != "Kullanýcý" || userId == null)
            return RedirectToPage("/AccessDenied");

        MyTasks = _context.Tasks
            .Include(t => t.Project) // Proje verilerini çekiyoruz
            .Where(t => t.AssignedUserId == userId)
            .Select(t => new UserTaskViewModel
            {
                TaskTitle = t.Title,
                TaskDetail = t.Description,
                DueDate =t.DueDate,  
                ProjectName = t.Project.Title,
                ProjectDescription = t.Project.Description,
                ProjectFeature = t.Project.Feature
            })
            .ToList();

        return Page();
    }
}