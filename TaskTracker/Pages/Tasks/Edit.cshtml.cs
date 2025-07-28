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

    [BindProperty]
    public List<int> SelectedUserIds { get; set; } = new();

    public List<SelectListItem> ProjectOptions { get; set; } = new();
    public List<SelectListItem> UserOptions { get; set; } = new();

    public EditModel(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        TaskItem = await _context.Tasks
            .Include(t => t.TaskUsers)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (TaskItem == null)
            return NotFound();

        // Önceden atanmış kullanıcıları SelectedUserIds'e aktar
        SelectedUserIds = TaskItem.TaskUsers.Select(tu => tu.UserId).ToList();

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

        var existingTask = await _context.Tasks
            .Include(t => t.TaskUsers)
            .FirstOrDefaultAsync(t => t.Id == TaskItem.Id);

        if (existingTask == null)
            return NotFound();

        // 🎯 Görev alanlarını güncelle
        existingTask.Title = TaskItem.Title;
        existingTask.Description = TaskItem.Description;
        existingTask.Status = TaskItem.Status;
        existingTask.Priority = TaskItem.Priority;
        existingTask.ProjectId = TaskItem.ProjectId;

        // 🔀 Hibrit kullanıcı atama
        existingTask.AssignedUserId = (SelectedUserIds.Count == 1)
            ? SelectedUserIds.First()
            : null;

        // 🔄 TaskUser ilişkilerini güncelle
        _context.TaskUsers.RemoveRange(existingTask.TaskUsers);

        foreach (var userId in SelectedUserIds)
        {
            _context.TaskUsers.Add(new TaskUser
            {
                TaskId = TaskItem.Id,
                UserId = userId
            });
        }

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