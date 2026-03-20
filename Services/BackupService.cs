namespace SvsWebApp.Services;

public class BackupService
{
    private readonly string _dbPath = "/data/players.db";
    private readonly string _backupDir = "/data/backups";

    public void CreateBackup()
    {
        Directory.CreateDirectory(_backupDir);
        if (!File.Exists(_dbPath)) return;
        var file = Path.Combine(_backupDir, $"players-{DateTime.UtcNow:yyyyMMdd-HHmmss}.db");
        File.Copy(_dbPath, file, true);
    }

    public string? LatestBackup()
    {
        Directory.CreateDirectory(_backupDir);
        return Directory.GetFiles(_backupDir, "*.db").OrderByDescending(x => x).FirstOrDefault();
    }

    public bool RestoreLatest()
    {
        var latest = LatestBackup();
        if (latest is null) return false;
        File.Copy(latest, _dbPath, true);
        return true;
    }
}
