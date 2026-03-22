using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SvsWebApp.Services;

namespace SvsWebApp.Pages;

public class LoginModel : PageModel
{
    private readonly AuthService _auth;
    private readonly AuditService _audit;
    public LoginModel(AuthService auth, AuditService audit) { _auth = auth; _audit = audit; }

    [BindProperty] public string Code { get; set; } = "";
    public string Message { get; set; } = "";

    public void OnGet()
    {
        if (Request.Query.ContainsKey("expired"))
        {
            Message = "Your session expired. Please sign in again.";
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (_auth.IsAdminCode(Code))
        {
            HttpContext.Session.SetString("role", "admin");
            await _audit.LogAsync("login", "admin login");
            return RedirectToPage("/Admin");
        }
        if (_auth.IsAllianceCode(Code))
        {
            HttpContext.Session.SetString("role", "alliance");
            await _audit.LogAsync("login", "alliance login");
            return RedirectToPage("/Index");
        }
        Message = "Invalid code.";
        return Page();
    }
}
