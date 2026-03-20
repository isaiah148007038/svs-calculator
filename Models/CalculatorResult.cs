namespace SvsWebApp.Models;

public class CalculatorResult
{
    public string PlayerName { get; set; } = string.Empty;
    public double CombatDensity { get; set; }
    public double KillEfficiency { get; set; }
    public double TechReadiness { get; set; }
    public double HeroLethality { get; set; }
    public double TroopBias { get; set; }
    public double FakePower { get; set; }
    public double DangerScore { get; set; }
    public string ThreatLabel { get; set; } = string.Empty;
}
