using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            Request.Cookies.ToList().ForEach(cookie => Response.Cookies.Delete(cookie.Key));
            return RedirectToPage("/Login");
        }
    }
}
