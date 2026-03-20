using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SvsWebApp.Pages;

public class LogoutModel : PageModel
{
    public void OnGet() => HttpContext.Session.Clear();
}
