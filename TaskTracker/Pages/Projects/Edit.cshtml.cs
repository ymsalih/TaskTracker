using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Core;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Pages.Projects;

public class EditModel : PageModel
{
    private readonly AppDbContext _context;

    [BindProperty]
    public Project Project { get; set; } = new();

    public EditModel(AppDbContext context)
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
        if (!ModelState.IsValid)
            return Page();

        _context.Attach(Project).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Projects.Any(p => p.Id == Project.Id))
                return NotFound();

            throw;
        }

        return RedirectToPage("List");
    }
}