using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskTracker.Core;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Pages.Projects;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;

    [BindProperty]
    public Project Project { get; set; } = new();

    public DeleteModel(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Project? project = await _context.Projects.FindAsync(id);
        if (project == null)
            return NotFound();

        Project = project;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Project? project = await _context.Projects.FindAsync(Project.Id);
        if (project == null)
            return NotFound();

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return RedirectToPage("List");
    }
}