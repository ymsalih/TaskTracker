using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskTracker.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Clear(); // 🔓 Tüm oturum bilgilerini temizler
            return RedirectToPage("/Auth/Login"); // 🔁 Login ekranına döner

        }
    }
}
