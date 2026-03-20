namespace SvsWebApp.Models;

public class PlayerEntry
{
    public int Id { get; set; }
    public string PlayerName { get; set; } = "";
    public decimal TotalPower { get; set; }
    public decimal TotalKills { get; set; }
    public decimal TechPower { get; set; }
    public decimal HeroPower { get; set; }
    public decimal TroopPower { get; set; }
    public decimal StructurePower { get; set; }
    public decimal ModVehiclePower { get; set; }
    public string Notes { get; set; } = "";
    public string ScreenshotPath { get; set; } = "";
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedUtc { get; set; }

    public decimal CombatDensity { get; set; }
    public decimal KillEfficiency { get; set; }
    public decimal TechReadiness { get; set; }
    public decimal HeroLethality { get; set; }
    public decimal TroopBias { get; set; }
    public decimal FakePower { get; set; }
    public decimal DangerScore { get; set; }
    public string ThreatLabel { get; set; } = "Low";
}
