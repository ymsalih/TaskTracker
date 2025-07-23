using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using TaskTracker.Core;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public LoginModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnPost()
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == Username && u.Password == Password);

            if (user == null)
            {
                ErrorMessage = "Kullanıcı adı veya şifre hatalı.";
                return Page();
            }

            // ✅ Oturum başlat
            HttpContext.Session.SetString("UserType", user.UserType);
            HttpContext.Session.SetInt32("UserId", user.Id);

            // Role göre yönlendirme
            if (user.UserType == "Yönetici")
                return RedirectToPage("/Admin/Dashboard");

            return RedirectToPage("/UserPanel/MyTasks");
        }
    }
}