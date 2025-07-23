using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskTracker.Core;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Pages.Tasks;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;

    [BindProperty]
    public TaskItem TaskItem { get; set; } = new();

    public DeleteModel(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return NotFound();

        TaskItem = task;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var task = await _context.Tasks.FindAsync(TaskItem.Id);
        if (task == null)
            return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return RedirectToPage("List");
    }
}