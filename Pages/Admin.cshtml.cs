using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SvsWebApp.Data;
using SvsWebApp.Models;
using SvsWebApp.Services;

namespace SvsWebApp.Pages;

public class AdminModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly AuditService _audit;
    private readonly BackupService _backup;
    public AdminModel(AppDbContext db, AuditService audit, BackupService backup)
    { _db = db; _audit = audit; _backup = backup; }

    public List<PlayerEntry> Entries { get; set; } = new();
    public List<AuditLog> Logs { get; set; } = new();
    public string Message { get; set; } = "";
    public string Error { get; set; } = "";

    public async Task<IActionResult> OnGetAsync()
    {
        if (!IsAdmin()) return RedirectToPage("/Login");
        await LoadAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        if (!IsAdmin()) return RedirectToPage("/Login");
        _backup.CreateBackup();
        var item = await _db.PlayerEntries.FindAsync(id);
        if (item != null)
        {
            item.IsDeleted = true;
            item.DeletedUtc = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            await _audit.LogAsync("soft-delete", $"deleted {item.PlayerName}");
        }
        await LoadAsync();
        Message = "Entry soft deleted.";
        return Page();
    }

    public async Task<IActionResult> OnPostRestoreOneAsync(int id)
    {
        if (!IsAdmin()) return RedirectToPage("/Login");
        var item = await _db.PlayerEntries.FindAsync(id);
        if (item != null)
        {
            item.IsDeleted = false;
            item.DeletedUtc = null;
            await _db.SaveChangesAsync();
            await _audit.LogAsync("restore-one", $"restored {item.PlayerName}");
        }
        await LoadAsync();
        Message = "Entry restored.";
        return Page();
    }

    public async Task<IActionResult> OnPostBackupAsync()
    {
        if (!IsAdmin()) return RedirectToPage("/Login");
        _backup.CreateBackup();
        await _audit.LogAsync("backup", "manual backup");
        await LoadAsync();
        Message = "Backup created.";
        return Page();
    }

    public async Task<IActionResult> OnPostRestoreAsync()
    {
        if (!IsAdmin()) return RedirectToPage("/Login");
        var ok = _backup.RestoreLatest();
        if (ok) await _audit.LogAsync("restore-latest", "restored latest backup");
        return RedirectToPage();
    }

    public async Task<FileResult> OnPostExportAsync()
    {
        if (!IsAdmin()) throw new UnauthorizedAccessException();
        var rows = await _db.PlayerEntries.Where(x => !x.IsDeleted).OrderByDescending(x => x.DangerScore).ToListAsync();
        var sb = new StringBuilder();
        sb.AppendLine("PlayerName,DangerScore,ThreatLabel,TotalPower,TotalKills,Notes");
        foreach (var r in rows)
            sb.AppendLine($"\"{r.PlayerName}\",{r.DangerScore:F2},\"{r.ThreatLabel}\",{r.TotalPower}, {r.TotalKills}, \"{r.Notes.Replace("\"", "''")}\"");
        await _audit.LogAsync("export", "exported csv");
        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "players-export.csv");
    }

    private async Task LoadAsync()
    {
        Entries = await _db.PlayerEntries.OrderByDescending(x => x.CreatedUtc).ToListAsync();
        Logs = await _db.AuditLogs.OrderByDescending(x => x.CreatedUtc).Take(100).ToListAsync();
    }

    private bool IsAdmin() 
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

