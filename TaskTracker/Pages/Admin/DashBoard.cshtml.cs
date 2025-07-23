using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class DashboardModel : PageModel
{
    public IActionResult OnGet()
    {
        var role = HttpContext.Session.GetString("UserType");
        if (role != "Yönetici")
            return RedirectToPage("/AccessDenied");

        return Page();
    }
}