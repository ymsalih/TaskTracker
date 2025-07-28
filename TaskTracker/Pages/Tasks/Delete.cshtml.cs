using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Core;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Pages.Tasks;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;

    [BindProperty]
    public TaskItem TaskItem { get; set; } = new();

    public List<User> AssociatedUsers { get; set; } = new();

    public DeleteModel(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        TaskItem = await _context.Tasks
            .Include(t => t.AssignedUser)
            .Include(t => t.TaskUsers)
                .ThenInclude(tu => tu.User)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (TaskItem == null)
            return NotFound();

        AssociatedUsers = TaskItem.TaskUsers.Select(tu => tu.User).ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var task = await _context.Tasks
            .Include(t => t.TaskUsers)
            .FirstOrDefaultAsync(t => t.Id == TaskItem.Id);

        if (task == null)
            return NotFound();

        // 🔄 Görev–Kullanıcı ilişkilerini sil
        _context.TaskUsers.RemoveRange(task.TaskUsers);

        // 🔴 Görevi sil
        _context.Tasks.Remove(task);

        await _context.SaveChangesAsync();

        return RedirectToPage("List");
    }
}