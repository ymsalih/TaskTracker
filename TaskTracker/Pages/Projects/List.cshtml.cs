using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskTracker.Core;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Pages.Projects;

public class ListModel : PageModel
{
    private readonly AppDbContext _context;

    public List<Project> Projects { get; set; } = new();

    public ListModel(AppDbContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        Projects = _context.Projects.ToList();
    }
}