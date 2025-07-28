using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskTracker.Core;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Pages.Projects;

public class AddModel : PageModel
{
    private readonly AppDbContext _context;

    [BindProperty]
    public Project Project { get; set; } = new();

    public AddModel(AppDbContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        // Örneğin: kategori, kullanıcı vb. dropdown listeler burada hazırlanabilir
    }

    public IActionResult OnPost()
    {
        // 📅 Tarih doğrulaması
        if (Project.StartDate < DateTime.Today)
        {
            ModelState.AddModelError(nameof(Project.StartDate), "Başlangıç tarihi bugünden önce olamaz.");
        }

        if (Project.EndDate < DateTime.Today)
        {
            ModelState.AddModelError(nameof(Project.EndDate), "Bitiş tarihi bugünden önce olamaz.");
        }

        if (Project.EndDate < Project.StartDate)
        {
            ModelState.AddModelError(nameof(Project.EndDate), "Bitiş tarihi, başlangıç tarihinden önce olamaz.");
        }

        // ⛔ Model geçerli değilse geri dön
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // ✅ Kayıt işlemi
        _context.Projects.Add(Project);
        _context.SaveChanges();

        return RedirectToPage("List");
    }
}