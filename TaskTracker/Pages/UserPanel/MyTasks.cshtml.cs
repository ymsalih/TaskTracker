using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Infrastructure.Data;
using TaskTracker.Models.ViewModels; // ViewModel�i burada tan�mlad���n� varsay�yoruz

public class MyTasksModel : PageModel
{
    private readonly AppDbContext _context;
    public List<UserTaskViewModel> MyTasks { get; set; } // art�k farkl� yerden verileri viewmodel ile filtrelemi� halini veriyoruz 

    public MyTasksModel(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        var role = HttpContext.Session.GetString("UserType");
        var userId = HttpContext.Session.GetInt32("UserId");

        if (role != "Kullan�c�" || userId == null)
            return RedirectToPage("/AccessDenied");

        MyTasks = _context.Tasks
            .Include(t => t.Project) // Proje verilerini �ekiyoruz
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