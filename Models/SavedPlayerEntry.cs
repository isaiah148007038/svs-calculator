namespace SvsWebApp.Models;

public class SavedPlayerEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public string SubmittedBy { get; set; } = string.Empty;
    public CalculatorInput Input { get; set; } = new();
    public CalculatorResult Result { get; set; } = new();
    public string ScreenshotPath { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
