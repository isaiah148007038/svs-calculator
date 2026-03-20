using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SvsWebApp.Data;
using SvsWebApp.Models;
using SvsWebApp.Services;

namespace SvsWebApp.Pages;

public class IndexModel : PageModel
{
    private readonly CalculatorService _calc;
    private readonly AppDbContext _db;
    private readonly AuditService _audit;
    private readonly BackupService _backup;

    public IndexModel(CalculatorService calc, AppDbContext db, AuditService audit, BackupService backup)
    {
        _calc = calc; _db = db; _audit = audit; _backup = backup;
    }

    [BindProperty] public PlayerInputModel Input { get; set; } = new();
    public string Message { get; set; } = "";
    public string Error { get; set; } = "";
    public PlayerEntry? LastSaved { get; set; }

    public void OnGet()
    {
        if (!IsLoggedIn()) Response.Redirect("/Login");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!IsLoggedIn()) return RedirectToPage("/Login");
        try
        {
            var totalPower = _calc.ParseValue(Input.TotalPower);
            var totalKills = _calc.ParseValue(Input.TotalKills);
            var techPower = _calc.ParseValue(Input.TechPower);
            var heroPower = _calc.ParseValue(Input.HeroPower);
            var troopPower = _calc.ParseValue(Input.TroopPower);
            var structurePower = _calc.ParseValue(Input.StructurePower);
            var modVehiclePower = _calc.ParseValue(Input.ModVehiclePower);
            var result = _calc.Compute(totalPower, totalKills, techPower, heroPower, troopPower, structurePower, modVehiclePower);

            string path = "";
            if (Input.Screenshot is { Length: > 0 })
            {
                var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(Input.Screenshot.FileName)}";
                var fullPath = Path.Combine("/data/uploads", fileName);
                using var stream = System.IO.File.Create(fullPath);
                await Input.Screenshot.CopyToAsync(stream);
                path = $"/uploads/{fileName}";
            }

            _backup.CreateBackup();
            var entry = new PlayerEntry
            {
                PlayerName = Input.PlayerName,
                TotalPower = totalPower,
                TotalKills = totalKills,
                TechPower = techPower,
                HeroPower = heroPower,
                TroopPower = troopPower,
                StructurePower = structurePower,
                ModVehiclePower = modVehiclePower,
                Notes = Input.Notes ?? "",
                ScreenshotPath = path ?? "",
                CombatDensity = result.CombatDensity,
                KillEfficiency = result.KillEfficiency,
                TechReadiness = result.TechReadiness,
                HeroLethality = result.HeroLethality,
                TroopBias = result.TroopBias,
                FakePower = result.FakePower,
                DangerScore = result.DangerScore,
                ThreatLabel = result.ThreatLabel
            };
            _db.PlayerEntries.Add(entry);
            await _db.SaveChangesAsync();
            await _audit.LogAsync("save", $"saved {entry.PlayerName}");
            LastSaved = entry;
            Message = "Saved successfully.";
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        return Page();
    }

    private bool IsLoggedIn() => HttpContext.Session.GetString("role") is "alliance" or "admin";
}
