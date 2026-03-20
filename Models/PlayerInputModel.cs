using System.ComponentModel.DataAnnotations;

namespace SvsWebApp.Models;

public class PlayerInputModel
{
    [Required]
    public string PlayerName { get; set; } = "";
    [Required]
    public string TotalPower { get; set; } = "";
    [Required]
    public string TotalKills { get; set; } = "";
    [Required]
    public string TechPower { get; set; } = "";
    [Required]
    public string HeroPower { get; set; } = "";
    [Required]
    public string TroopPower { get; set; } = "";
    [Required]
    public string StructurePower { get; set; } = "";
    public string ModVehiclePower { get; set; } = "";
    public string Notes { get; set; } = "";
    public IFormFile? Screenshot { get; set; }
}
