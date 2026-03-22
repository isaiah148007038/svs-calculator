using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using SvsWebApp.Data;
using SvsWebApp.Services;
using System.IO;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.Configure<FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = 10 * 1024 * 1024;
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Data Source=/data/players.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddSingleton<AuthService>();
builder.Services.AddScoped<CalculatorService>();
builder.Services.AddScoped<AuditService>();
builder.Services.AddScoped<BackupService>();
builder.Services.AddHttpContextAccessor();


Directory.CreateDirectory("/data/keys");

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/data/keys"));






builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(8);
});

var app = builder.Build();

Directory.CreateDirectory("/data");
Directory.CreateDirectory("/data/uploads");
Directory.CreateDirectory("/data/backups");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider("/data/uploads"),
    RequestPath = "/uploads"
});
app.UseRouting();
app.UseSession();
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Microsoft.AspNetCore.Antiforgery.AntiforgeryValidationException)
    {
        context.Session.Clear();

        context.Response.Cookies.Delete(".AspNetCore.Session");
        context.Response.Cookies.Delete(".AspNetCore.Antiforgery");
        context.Response.Cookies.Delete("svs_antiforgery");

        context.Response.Redirect("/Login?expired=1");
    }
    catch (System.Security.Cryptography.CryptographicException)
    {
        context.Session.Clear();

        context.Response.Cookies.Delete(".AspNetCore.Session");
        context.Response.Cookies.Delete(".AspNetCore.Antiforgery");
        context.Response.Cookies.Delete("svs_antiforgery");

        context.Response.Redirect("/Login?expired=1");
    }
});
app.MapRazorPages();

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://*:{port}");

app.Run();
