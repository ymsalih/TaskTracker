using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Core;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Pages.Tasks;

public class ListModel : PageModel
{
    private readonly AppDbContext _context;

    public List<TaskItem> Tasks { get; set; } = new();
    public List<SelectListItem> ProjectOptions { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int? ProjectId { get; set; }

    public string? SelectedProjectId => ProjectId?.ToString();

    public ListModel(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userType = HttpContext.Session.GetString("UserType");
        var userId = HttpContext.Session.GetInt32("UserId");

        if (string.IsNullOrEmpty(userType))
            return RedirectToPage("/Auth/Login");

        // 🔽 Dropdown verisi: Projeler listesi
        ProjectOptions = await _context.Projects
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Title
            }).ToListAsync();

        // 📦 Görevleri sorgula (proje, atayan kullanıcı, görev–kullanıcı ilişkileri)
        var query = _context.Tasks
            .Include(t => t.Project)
            .Include(t => t.AssignedUser)
            .Include(t => t.TaskUsers)
                .ThenInclude(tu => tu.User)
            .AsQueryable();

        if (userType == "Yönetici")
        {
            if (ProjectId.HasValue)
                query = query.Where(t => t.ProjectId == ProjectId.Value);
        }
        else if (userType == "Kullanıcı" && userId != null)
        {
            // Kullanıcının bireysel atanmış görevleri + ekip görevleri
            query = query.Where(t =>
                t.AssignedUserId == userId ||
                t.TaskUsers.Any(tu => tu.UserId == userId));

            if (ProjectId.HasValue)
                query = query.Where(t => t.ProjectId == ProjectId.Value);
        }
        else
        {
            return RedirectToPage("/AccessDenied");
        }

        Tasks = await query.ToListAsync();
        return Page();
    }
}