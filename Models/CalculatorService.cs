using System.Globalization;

namespace SvsWebApp.Models;

public static class CalculatorService
{
    public static CalculatorResult Calculate(CalculatorInput input)
    {
        double totalPower = ParseValue(input.TotalPower);
        double totalKills = ParseValue(input.TotalKills);
        double techPower = ParseValue(input.TechPower);
        double heroPower = ParseValue(input.HeroPower);
        double troopPower = ParseValue(input.TroopPower);
        double structurePower = ParseValue(input.StructurePower);
        double modVehiclePower = string.IsNullOrWhiteSpace(input.ModVehiclePower)
            ? 0
            : ParseValue(input.ModVehiclePower);

        double combatDensity = totalPower == 0 ? 0 : totalKills / totalPower;
        double killEfficiency = troopPower == 0 ? 0 : totalKills / troopPower;
        double techReadiness = totalPower == 0 ? 0 : techPower / totalPower;
        double heroLethality = totalPower == 0 ? 0 : heroPower / totalPower;
        double troopBias = totalPower == 0 ? 0 : troopPower / totalPower;
        double fakePower = totalPower == 0 ? 0 : (structurePower + modVehiclePower) / totalPower;

        double dangerScore =
            combatDensity * 40 +
            killEfficiency * 20 +
            techReadiness * 15 +
            heroLethality * 10 +
            troopBias * 10 -
            fakePower * 15;

        string threatLabel = dangerScore >= 20 ? "Extreme" :
                             dangerScore >= 10 ? "High" :
                             dangerScore >= 5 ? "Moderate" : "Low";

        return new CalculatorResult
        {
            PlayerName = input.PlayerName,
            CombatDensity = combatDensity,
            KillEfficiency = killEfficiency,
            TechReadiness = techReadiness,
            HeroLethality = heroLethality,
            TroopBias = troopBias,
            FakePower = fakePower,
            DangerScore = dangerScore,
            ThreatLabel = threatLabel
        };
    }

    public static double ParseValue(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return 0;

        input = input.Trim().ToUpperInvariant().Replace(",", "");

        if (input.EndsWith("K"))
            return double.Parse(input[..^1], CultureInfo.InvariantCulture) * 1_000;

        if (input.EndsWith("M"))
            return double.Parse(input[..^1], CultureInfo.InvariantCulture) * 1_000_000;

        if (input.EndsWith("B"))
            return double.Parse(input[..^1], CultureInfo.InvariantCulture) * 1_000_000_000;

        return double.Parse(input, CultureInfo.InvariantCulture);
    }
}
