using FgQuota.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FgQuota.Api.Data;

public sealed class QuotaDbContext : DbContext
{
    public QuotaDbContext(DbContextOptions<QuotaDbContext> options) : base(options)
    {
    }

    public DbSet<Empvl> Applications => Set<Empvl>();

    public DbSet<EmplQuota> QuotaDetails => Set<EmplQuota>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empvl>(entity =>
        {
            entity.ToTable("EMPVL");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(64);
            entity.Property(e => e.Empid).HasMaxLength(50);
            entity.Property(e => e.ActivityName).HasMaxLength(100);
            entity.Property(e => e.ActivityDay).HasMaxLength(10);
            entity.Property(e => e.RegionId).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.CreateEmpid).HasMaxLength(50);
            entity.Property(e => e.CreateEmpname).HasMaxLength(50);
            entity.Property(e => e.ApplyType).HasMaxLength(10);
            entity.HasIndex(e => e.ApplicationId).IsUnique();
            entity.HasIndex(e => new { e.Empid, e.ActivityDay, e.ApplyType });
        });

        modelBuilder.Entity<EmplQuota>(entity =>
        {
            entity.ToTable("EMPL_QUOTA");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.RegionId).HasMaxLength(20);
            entity.Property(e => e.EmplId).HasMaxLength(50);
            entity.Property(e => e.Year).HasMaxLength(10);
            entity.Property(e => e.ApplicationType).HasMaxLength(10);
            entity.HasIndex(e => new { e.EmplId, e.Year, e.ApplicationType });
            entity.HasIndex(e => e.ApplicationId);
        });
    }
}
