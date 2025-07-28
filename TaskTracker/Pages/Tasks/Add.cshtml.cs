using Microsoft.AspNetCore.Mvc;                     // Razor Pages ve model binding için gerekli
using Microsoft.AspNetCore.Mvc.Rendering;           // SelectListItem yapısı için gerekli
using Microsoft.AspNetCore.Mvc.RazorPages;          // PageModel taban sınıfı için gerekli
using TaskTracker.Core;                             // TaskItem, User, TaskUser modelleri için namespace
using TaskTracker.Infrastructure.Data;              // EF Core veritabanı bağlantısı

namespace TaskTracker.Pages.Tasks;

public class AddModel : PageModel
{
    private readonly AppDbContext _context;         // Veritabanı bağlantısını yöneten EF Core sınıfı

    // 📌 Görev bilgileri: formdan gelen verilerle dolacak
    [BindProperty]
    public TaskItem TaskItem { get; set; } = new();

    // 📌 Çoklu kullanıcı seçimi: checkbox’larla gönderilen kullanıcı ID’leri burada tutulur
    [BindProperty]
    public List<int> SelectedUserIds { get; set; } = new();

    // 📌 Proje ve kullanıcı dropdown/checkbox’ları için seçenek listeleri
    public List<SelectListItem> ProjectOptions { get; set; } = new();
    public List<SelectListItem> UserOptions { get; set; } = new();

    public AddModel(AppDbContext context)
    {
        _context = context;
    }

    // Sayfa yüklendiğinde proje ve kullanıcı listelerini hazırlar
    public void OnGet()
    {
        LoadProjects();
        LoadUsers();
    }

    // Form gönderildiğinde işlem yapılır
    public IActionResult OnPost()
    {
        // 🎯 Form doğrulama: eksik veya hatalı alanları kontrol ediyoruz

        if (string.IsNullOrWhiteSpace(TaskItem.Title))
            ModelState.AddModelError("TaskItem.Title", "Görev başlığı zorunludur.");

        if (TaskItem.ProjectId <= 0)
            ModelState.AddModelError("TaskItem.ProjectId", "Lütfen bir proje seçin.");

        if (string.IsNullOrWhiteSpace(TaskItem.Status))
            ModelState.AddModelError("TaskItem.Status", "Görev durumu belirtilmelidir.");

        if (string.IsNullOrWhiteSpace(TaskItem.Priority))
            ModelState.AddModelError("TaskItem.Priority", "Öncelik belirtilmelidir.");

        // Eğer form geçersizse tekrar liste yükle ve sayfayı geri döndür
        if (!ModelState.IsValid)
        {
            LoadProjects();
            LoadUsers();
            return Page();
        }

        // ✅ Görev ekleniyor
        _context.Tasks.Add(TaskItem);
        _context.SaveChanges(); // ID gibi değerler burada veritabanından alınır

        // ✅ Görev–Kullanıcı ilişkisi kuruluyor (checkbox ile seçilen kullanıcılar)
        foreach (var userId in SelectedUserIds)
        {
            _context.TaskUsers.Add(new TaskUser
            {
                TaskId = TaskItem.Id,
                UserId = userId
            });
        }

        _context.SaveChanges(); // Görev–Kullanıcı ilişkileri kaydediliyor

        return RedirectToPage("List"); // Görev listesine yönlendirme
    }

    // 📥 Proje listesi hazırlanıyor
    private void LoadProjects()
    {
        ProjectOptions = _context.Projects
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Title
            }).ToList();
    }

    // 📥 Kullanıcı listesi hazırlanıyor — sadece "Kullanıcı" rolünde olanlar
    private void LoadUsers()
    {
        UserOptions = _context.Users
            .Where(u => u.UserType == "Kullanıcı")
            .Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Name
            }).ToList();
    }
}