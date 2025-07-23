using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskTracker.Core;
using TaskTracker.Infrastructure.Data;

public class MyTasksModel : PageModel
{
    private readonly AppDbContext _context;
    public List<TaskItem> MyTasks { get; set; }

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
            .Where(t => t.AssignedUserId == userId)
            .ToList();

        return Page();
    }
}