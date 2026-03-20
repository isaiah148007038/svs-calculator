using Microsoft.AspNetCore.Http;
using SvsWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(12);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<PlayerEntryStore>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapGet("/logout", (HttpContext context) =>
{
    context.Session.Clear();
    return Results.Redirect("/");
});

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;

    var publicPaths = new[]
    {
        "/", "/index", "/adminlogin", "/error"
    };

    if (publicPaths.Contains(path) ||
        path.StartsWith("/css") ||
        path.StartsWith("/js") ||
        path.StartsWith("/lib") ||
        path.StartsWith("/favicon") ||
        path.StartsWith("/uploads") ||
        path.StartsWith("/_framework"))
    {
        await next();
        return;
    }

    if (path.StartsWith("/admin"))
    {
        var isAdmin = context.Session.GetString("AdminAccess") == "granted";
        if (!isAdmin)
        {
            context.Response.Redirect("/AdminLogin");
            return;
        }
    }
    else
    {
        var isAuthenticated = context.Session.GetString("AllianceAccess") == "granted" || context.Session.GetString("AdminAccess") == "granted";
        if (!isAuthenticated)
        {
            context.Response.Redirect("/");
            return;
        }
    }

    await next();
});

app.MapRazorPages();
app.Run();
