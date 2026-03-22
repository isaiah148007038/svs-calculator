using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SvsWebApp.Data;
using SvsWebApp.Models;

namespace SvsWebApp.Pages;

public class CompareModel : PageModel
{
    private readonly AppDbContext _db;
    public CompareModel(AppDbContext db) => _db = db;
    public List<PlayerEntry> Entries { get; set; } = new();

    public async Task OnGetAsync()
    {
       if (!IsLoggedIn())
{
    Response.Redirect("/Login");
    return;
}
        Entries = (await _db.PlayerEntries
        .Where(x => !x.IsDeleted)
        .ToListAsync())
        .OrderByDescending(x => x.DangerScore)
        .ThenBy(x => x.PlayerName)
        .ToList();
    }
    private bool IsLoggedIn()
    {
        try
        {
            return HttpContext.Session.GetString("role") is "alliance" or "admin";
        }
        catch
        {
            return false;
        }
    }
}
}
