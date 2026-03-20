using SvsWebApp.Data;
using SvsWebApp.Models;

namespace SvsWebApp.Services;

public class AuditService
{
    private readonly AppDbContext _db;
    private readonly IHttpContextAccessor _http;

    public AuditService(AppDbContext db, IHttpContextAccessor http)
    {
        _db = db;
        _http = http;
    }

    public async Task LogAsync(string action, string details)
    {
        var ctx = _http.HttpContext;
        var role = ctx?.Session.GetString("role") ?? "guest";
        var ip = ctx?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        _db.AuditLogs.Add(new AuditLog
        {
            Action = action,
            Details = details,
            ActorRole = role,
            ActorIp = ip
        });
        await _db.SaveChangesAsync();
    }
}
