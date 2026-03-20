namespace SvsWebApp.Models;

public class CalcResult
{
    public decimal CombatDensity { get; set; }
    public decimal KillEfficiency { get; set; }
    public decimal TechReadiness { get; set; }
    public decimal HeroLethality { get; set; }
    public decimal TroopBias { get; set; }
    public decimal FakePower { get; set; }
    public decimal DangerScore { get; set; }
    public string ThreatLabel { get; set; } = "Low";
}
