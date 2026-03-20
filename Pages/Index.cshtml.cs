using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SvsWebApp.Models;

namespace SvsWebApp.Pages;

public class IndexModel : PageModel
{
    private const string AllianceCode = "BRG225";

    [BindProperty]
    public CalculatorInput Input { get; set; } = new();

    public string ErrorMessage { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("AdminAccess") == "granted")
            return RedirectToPage("/Admin");

        if (HttpContext.Session.GetString("AllianceAccess") == "granted")
            return RedirectToPage("/Calculator");

        return Page();
    }

    public IActionResult OnPost()
    {
        if (string.Equals(Input.AccessCode?.Trim(), AllianceCode, StringComparison.Ordinal))
        {
            HttpContext.Session.SetString("AllianceAccess", "granted");
            return RedirectToPage("/Calculator");
        }

        ErrorMessage = "Wrong alliance code.";
        return Page();
    }
}
