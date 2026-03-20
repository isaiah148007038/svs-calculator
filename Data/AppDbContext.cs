using Microsoft.EntityFrameworkCore;
using SvsWebApp.Models;

namespace SvsWebApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PlayerEntry> PlayerEntries => Set<PlayerEntry>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerEntry>().Property(x => x.TotalPower).HasPrecision(18, 2);
        modelBuilder.Entity<PlayerEntry>().Property(x => x.TotalKills).HasPrecision(18, 2);
        modelBuilder.Entity<PlayerEntry>().Property(x => x.TechPower).HasPrecision(18, 2);
        modelBuilder.Entity<PlayerEntry>().Property(x => x.HeroPower).HasPrecision(18, 2);
        modelBuilder.Entity<PlayerEntry>().Property(x => x.TroopPower).HasPrecision(18, 2);
        modelBuilder.Entity<PlayerEntry>().Property(x => x.StructurePower).HasPrecision(18, 2);
        modelBuilder.Entity<PlayerEntry>().Property(x => x.ModVehiclePower).HasPrecision(18, 2);
        modelBuilder.Entity<PlayerEntry>().Property(x => x.CombatDensity).HasPrecision(18, 6);
        modelBuilder.Entity<PlayerEntry>().Property(x => x.KillEfficiency).HasPrecision(18, 6);
        modelBuilder.Entity<PlayerEntry>().Property(x => x.TechReadiness).HasPrecision(18, 6);
        modelBuilder.Entity<PlayerEntry>().Property(x => x.HeroLethality).HasPrecision(18, 6);
        modelBuilder.Entity<PlayerEntry>().Property(x => x.TroopBias).HasPrecision(18, 6);
        modelBuilder.Entity<PlayerEntry>().Property(x => x.FakePower).HasPrecision(18, 6);
        modelBuilder.Entity<PlayerEntry>().Property(x => x.DangerScore).HasPrecision(18, 6);
    }
}
