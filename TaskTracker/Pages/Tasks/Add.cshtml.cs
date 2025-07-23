using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskTracker.Core;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Pages.Tasks;

public class AddModel : PageModel
{
    private readonly AppDbContext _context;

    [BindProperty]
    public TaskItem TaskItem { get; set; } = new();

    public List<SelectListItem> ProjectOptions { get; set; } = new();
    public List<SelectListItem> UserOptions { get; set; } = new();

    public AddModel(AppDbContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        LoadProjects();
        LoadUsers();
    }

    public IActionResult OnPost()
    {
        // 🎯 Görev başlığı zorunlu
        if (string.IsNullOrWhiteSpace(TaskItem.Title))
            ModelState.AddModelError("TaskItem.Title", "Görev başlığı zorunludur.");

        // 🎯 Proje seçimi zorunlu
        if (TaskItem.ProjectId <= 0)
            ModelState.AddModelError("TaskItem.ProjectId", "Lütfen bir proje seçin.");

        // 🎯 Durum & Öncelik kontrolü
        if (string.IsNullOrWhiteSpace(TaskItem.Status))
            ModelState.AddModelError("TaskItem.Status", "Görev durumu belirtilmelidir.");

        if (string.IsNullOrWhiteSpace(TaskItem.Priority))
            ModelState.AddModelError("TaskItem.Priority", "Öncelik belirtilmelidir.");

        // 🎯 Kullanıcı seçimi opsiyonel ama FK geçerli olmalı (nullable olduğu için kontrol gerekmez)

        if (!ModelState.IsValid)
        {
            LoadProjects();
            LoadUsers();
            return Page();
        }

        _context.Tasks.Add(TaskItem);
        _context.SaveChanges();

        return RedirectToPage("List");
    }

    private void LoadProjects()
    {
        ProjectOptions = _context.Projects
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Title
            }).ToList();
    }

    private void LoadUsers()
    {
        UserOptions = _context.Users
            .Where(u => u.UserType == "Kullanıcı")
            .Select(u=> new SelectListItem 
            {
                Value = u.Id.ToString(),
                Text = u.Name // veya Email, FullName alanı varsa tercih edebilirsin
            }).ToList();
    }
}