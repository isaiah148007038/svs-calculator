using System.Text.Json;
using SvsWebApp.Models;

namespace SvsWebApp.Services;

public class PlayerEntryStore
{
    private readonly IWebHostEnvironment _env;
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
    private readonly object _lock = new();

    public PlayerEntryStore(IWebHostEnvironment env)
    {
        _env = env;
    }

    private string DataDirectory => Path.Combine(_env.ContentRootPath, "Data");
    private string DataFile => Path.Combine(DataDirectory, "players.json");
    private string UploadDirectory => Path.Combine(_env.WebRootPath, "uploads");

    public List<SavedPlayerEntry> GetAll()
    {
        lock (_lock)
        {
            EnsureStorage();
            var json = File.ReadAllText(DataFile);
            return JsonSerializer.Deserialize<List<SavedPlayerEntry>>(json) ?? new List<SavedPlayerEntry>();
        }
    }

    public SavedPlayerEntry? Get(Guid id) => GetAll().FirstOrDefault(x => x.Id == id);

    public void Save(SavedPlayerEntry entry)
    {
        lock (_lock)
        {
            EnsureStorage();
            var items = GetAll();
            items.Insert(0, entry);
            File.WriteAllText(DataFile, JsonSerializer.Serialize(items, _jsonOptions));
        }
    }

    public void Delete(Guid id)
    {
        lock (_lock)
        {
            EnsureStorage();
            var items = GetAll();
            var match = items.FirstOrDefault(x => x.Id == id);
            if (match is not null)
            {
                if (!string.IsNullOrWhiteSpace(match.ScreenshotPath))
                {
                    var physicalPath = Path.Combine(_env.WebRootPath, match.ScreenshotPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(physicalPath))
                    {
                        File.Delete(physicalPath);
                    }
                }

                items.Remove(match);
                File.WriteAllText(DataFile, JsonSerializer.Serialize(items, _jsonOptions));
            }
        }
    }

    public void DeleteAll()
    {
        lock (_lock)
        {
            EnsureStorage();
            var items = GetAll();
            foreach (var item in items)
            {
                if (!string.IsNullOrWhiteSpace(item.ScreenshotPath))
                {
                    var physicalPath = Path.Combine(_env.WebRootPath, item.ScreenshotPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(physicalPath))
                    {
                        File.Delete(physicalPath);
                    }
                }
            }
            File.WriteAllText(DataFile, "[]");
        }
    }

    public async Task<string> SaveScreenshotAsync(IFormFile? file)
    {
        EnsureStorage();

        if (file is null || file.Length == 0)
            return string.Empty;

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        if (!allowed.Contains(ext))
            throw new InvalidOperationException("Only JPG, PNG, and WEBP screenshots are allowed.");

        var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}{ext}";
        var physicalPath = Path.Combine(UploadDirectory, fileName);
        using var stream = File.Create(physicalPath);
        await file.CopyToAsync(stream);
        return $"/uploads/{fileName}";
    }

    private void EnsureStorage()
    {
        Directory.CreateDirectory(DataDirectory);
        Directory.CreateDirectory(UploadDirectory);
        if (!File.Exists(DataFile))
        {
            File.WriteAllText(DataFile, "[]");
        }
    }
}
