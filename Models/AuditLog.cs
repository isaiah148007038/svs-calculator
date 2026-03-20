namespace SvsWebApp.Models;

public class AuditLog
{
    public int Id { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public string Action { get; set; } = "";
    public string ActorRole { get; set; } = "";
    public string ActorIp { get; set; } = "";
    public string Details { get; set; } = "";
}
