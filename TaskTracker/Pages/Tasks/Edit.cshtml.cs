using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Core;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Pages.Tasks;

public class EditModel : PageModel
{
    private readonly AppDbContext _context;

    [BindProperty]
    public TaskItem TaskItem { get; set; } = new();

    public List<SelectListItem> ProjectOptions { get; set; } = new();
    public List<SelectListItem> UserOptions { get; set; } = new();

    public EditModel(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        TaskItem = await _context.Tasks.FindAsync(id);
        if (TaskItem == null)
            return NotFound();

        await LoadDropdowns();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdowns();
            return Page();
        }

        var existingTask = await _context.Tasks.FindAsync(TaskItem.Id);
        if (existingTask == null)
            return NotFound();

        // 🎯 Güncellenecek alanlar
        existingTask.Title = TaskItem.Title;
        existingTask.Description = TaskItem.Description;
        existingTask.Status = TaskItem.Status;
        existingTask.Priority = TaskItem.Priority;
        existingTask.ProjectId = TaskItem.ProjectId;
        existingTask.AssignedUserId = TaskItem.AssignedUserId;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Tasks.Any(t => t.Id == TaskItem.Id))
                return NotFound();

            throw;
        }

        return RedirectToPage("List");
    }

    private async Task LoadDropdowns()
    {
        ProjectOptions = await _context.Projects
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Title
            }).ToListAsync();

        UserOptions = await _context.Users
            .Where(u => u.UserType == "Kullanıcı")
            .Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Name
            }).ToListAsync();
    }
}