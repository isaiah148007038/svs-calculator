namespace SvsWebApp.Services;

public class AuthService
{
    private readonly IConfiguration _config;
    public AuthService(IConfiguration config) => _config = config;

    public bool IsAllianceCode(string? code) =>
        !string.IsNullOrWhiteSpace(code) &&
        code == _config["Codes:Alliance"];

    public bool IsAdminCode(string? code) =>
        !string.IsNullOrWhiteSpace(code) &&
        code == _config["Codes:Admin"];
}
