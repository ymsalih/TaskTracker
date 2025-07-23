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
        // Gerekirse dropdown hazırlığı yapılabilir
    }

    public IActionResult OnPost()
    {
        // 👀 Tarih doğrulaması – geçmişe izin verilmez
        if (Project.StartDate < DateTime.Today)
        {
            ModelState.AddModelError("Project.StartDate", "Başlangıç tarihi bugünden önce olamaz.");
        }

        if (Project.EndDate < DateTime.Today)
        {
            ModelState.AddModelError("Project.EndDate", "Bitiş tarihi bugünden önce olamaz.");
        }

        // 📅 Ek kontrol: Başlangıç tarihi bitiş tarihinden sonra olmasın
        if (Project.EndDate < Project.StartDate)
        {
            ModelState.AddModelError(string.Empty, "Bitiş tarihi, başlangıç tarihinden önce olamaz.");
        }

        // 🔍 ModelState hataları varsa detayları göster
        if (!ModelState.IsValid)
        {
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    Console.WriteLine($"Alan: {entry.Key} - Hata: {error.ErrorMessage}");
                }
            }

            return Page();
        }

        // ✅ Her şey yolundaysa kaydet
        _context.Projects.Add(Project);
        _context.SaveChanges();

        return RedirectToPage("List");
    }
}