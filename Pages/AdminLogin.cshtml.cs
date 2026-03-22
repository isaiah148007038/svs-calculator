using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SvsWebApp.Pages;

public class AdminLoginModel : PageModel
{
    private const string AdminCode = "Elmora1984";

    [BindProperty]
    public string AccessCode { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("AdminAccess") == "granted")
            return RedirectToPage("/Admin");

        return Page();
    }

    public IActionResult OnPost()
    {
        if (string.Equals(AccessCode?.Trim(), AdminCode, StringComparison.Ordinal))
        {
            HttpContext.Session.SetString("AdminAccess", "granted");
            HttpContext.Session.SetString("AllianceAccess", "granted");
            return RedirectToPage("/Admin");
        }

        ErrorMessage = "Wrong admin code.";
        return Page();
    }
}
}
