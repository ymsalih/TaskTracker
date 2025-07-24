using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskTracker.Core;
using TaskTracker.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;

namespace TaskTracker.Pages.Users
{
    public class ListModel : PageModel
    {
        private readonly AppDbContext _context;

        public ListModel(AppDbContext context)
        {
            _context = context;
        }

        public List<User> Users { get; set; }

        public void OnGet()
        {
            Users = _context.Users.ToList();
            // sayfa yüklendiðinde tüm kullanýcýlarý çeker 
         
        }
    }
}