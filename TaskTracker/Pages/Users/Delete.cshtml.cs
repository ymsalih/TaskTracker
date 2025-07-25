using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskTracker.Core;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context; // users tablosuna eri�mek i�in 
        }

        [BindProperty] // Bu �zellik, User nesnesinin Razor taraf� ile C# taraf� aras�nda veri al��veri�ini sa�lar

        public User User { get; set; }

        public IActionResult OnGet(int id)
        {
            User = _context.Users.Find(id);
            if (User == null)
                return RedirectToPage("List");

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToPage("List");
        }
    }
}