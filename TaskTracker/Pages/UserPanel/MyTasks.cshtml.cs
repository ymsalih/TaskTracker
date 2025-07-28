using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Infrastructure.Data;
using TaskTracker.Models.ViewModels;

public class MyTasksModel : PageModel
{
    private readonly AppDbContext _context;
    public List<UserTaskViewModel> MyTasks { get; set; } = new();

    public MyTasksModel(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync()
    {


        var role = HttpContext.Session.GetString("UserType");
        var userId = HttpContext.Session.GetInt32("UserId");

        if (role != "Kullanıcı" || userId == null)
            return RedirectToPage("/UserPanel/MyTasks");

        MyTasks = await _context.Tasks
            .Include(t => t.Project)
            .Include(t => t.TaskUsers)
            .Where(t =>
                t.AssignedUserId == userId ||              // Tekil görevler
                t.TaskUsers.Any(tu => tu.UserId == userId) // Çoklu görevler
            )
            .Select(t => new UserTaskViewModel
            {
                TaskTitle = t.Title,
                TaskDetail = t.Description,
                TaskStatus = t.Status,
                DueDate = t.Project.EndDate,
                ProjectName = t.Project.Title,
                ProjectDescription = t.Project.Description,
                ProjectFeature = t.Project.Feature
            })
            .ToListAsync();

        return Page();
    }
}