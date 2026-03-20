using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SvsWebApp.Models;
using SvsWebApp.Services;

namespace SvsWebApp.Pages;

public class AdminModel : PageModel
{
    private readonly PlayerEntryStore _store;

    public AdminModel(PlayerEntryStore store)
    {
        _store = store;
    }

    public List<SavedPlayerEntry> Entries { get; set; } = new();
    public int TotalPlayers { get; set; }
    public int ExtremeCount { get; set; }
    public int HighCount { get; set; }
    public int UploadCount { get; set; }
    public string Message { get; set; } = string.Empty;

    public void OnGet() => Load();

    public IActionResult OnPostDeleteAll()
    {
        _store.DeleteAll();
        TempData["Message"] = "All saved data was deleted.";
        return RedirectToPage();
    }

    public IActionResult OnPostDeleteOne(Guid id)
    {
        _store.Delete(id);
        TempData["Message"] = "Saved entry was deleted.";
        return RedirectToPage();
    }

    private void Load()
    {
        Entries = _store.GetAll().OrderByDescending(x => x.CreatedUtc).ToList();
        TotalPlayers = Entries.Count;
        ExtremeCount = Entries.Count(x => x.Result.ThreatLabel == "Extreme");
        HighCount = Entries.Count(x => x.Result.ThreatLabel == "High");
        UploadCount = Entries.Count(x => !string.IsNullOrWhiteSpace(x.ScreenshotPath));
        Message = TempData["Message"]?.ToString() ?? string.Empty;
    }
}
