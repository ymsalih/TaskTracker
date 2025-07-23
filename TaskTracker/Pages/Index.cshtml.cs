using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskTracker.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("UserType");

            if (string.IsNullOrEmpty(role))
                return RedirectToPage("/Auth/Login");

            if (role == "Yönetici")
                return RedirectToPage("/Admin/Dashboard");

            if (role == "Kullanýcý")
                return RedirectToPage("/UserPanel/MyTasks");

            return Page(); // Bilinmeyen role varsa sayfada kal
        }
    }
}