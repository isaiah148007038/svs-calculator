using SvsWebApp.Models;

namespace SvsWebApp.Services;

public class CalculatorService
{
    public decimal ParseValue(string input)
    {
        input = (input ?? "").Trim().ToUpper().Replace(",", "");
        if (string.IsNullOrWhiteSpace(input)) return 0m;
        if (input.EndsWith("K")) return decimal.Parse(input[..^1]) * 1_000m;
        if (input.EndsWith("M")) return decimal.Parse(input[..^1]) * 1_000_000m;
        if (input.EndsWith("B")) return decimal.Parse(input[..^1]) * 1_000_000_000m;
        return decimal.Parse(input);
    }

    public CalcResult Compute(decimal totalPower, decimal totalKills, decimal techPower, decimal heroPower,
        decimal troopPower, decimal structurePower, decimal modVehiclePower)
    {
        decimal combatDensity = totalPower == 0 ? 0 : totalKills / totalPower;
        decimal killEfficiency = troopPower == 0 ? 0 : totalKills / troopPower;
        decimal techReadiness = totalPower == 0 ? 0 : techPower / totalPower;
        decimal heroLethality = totalPower == 0 ? 0 : heroPower / totalPower;
        decimal troopBias = totalPower == 0 ? 0 : troopPower / totalPower;
        decimal fakePower = totalPower == 0 ? 0 : (structurePower + modVehiclePower) / totalPower;

        decimal dangerScore =
            combatDensity * 40m +
            killEfficiency * 20m +
            techReadiness * 15m +
            heroLethality * 10m +
            troopBias * 10m -
            fakePower * 15m;

        string threat = dangerScore >= 20 ? "Extreme" :
                        dangerScore >= 10 ? "High" :
                        dangerScore >= 5 ? "Moderate" : "Low";

        return new CalcResult
        {
            CombatDensity = combatDensity,
            KillEfficiency = killEfficiency,
            TechReadiness = techReadiness,
            HeroLethality = heroLethality,
            TroopBias = troopBias,
            FakePower = fakePower,
            DangerScore = dangerScore,
            ThreatLabel = threat
        };
    }
}
