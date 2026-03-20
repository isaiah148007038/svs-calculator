using Microsoft.AspNetCore.Mvc.RazorPages;
using SvsWebApp.Models;
using SvsWebApp.Services;

namespace SvsWebApp.Pages;

public class CompareModel : PageModel
{
    private readonly PlayerEntryStore _store;

    public CompareModel(PlayerEntryStore store)
    {
        _store = store;
    }

    public List<SavedPlayerEntry> Entries { get; set; } = new();

    public void OnGet()
    {
        Entries = _store.GetAll()
            .OrderByDescending(x => x.Result.DangerScore)
            .ThenBy(x => x.Result.PlayerName)
            .ToList();
    }
}
