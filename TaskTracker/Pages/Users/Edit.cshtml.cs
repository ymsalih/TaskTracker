using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskTracker.Core;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; }

        public IActionResult OnGet(int id)
        {
            User = _context.Users.Find(id);
            if (User == null)
                return RedirectToPage("List");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var userInDb = _context.Users.Find(User.Id);
            if (userInDb == null)
                return RedirectToPage("List");

            // Tüm alanları güncelle
            userInDb.Name = User.Name;
            userInDb.Role = User.Role?.Trim();
            userInDb.Username = User.Username?.Trim();
            userInDb.Password = User.Password; // 🔒 Şimdilik düz metin, hashleme istersen ekleyebiliriz
            userInDb.UserType = User.UserType;

            _context.SaveChanges();

            return RedirectToPage("List");

        }
    }
}