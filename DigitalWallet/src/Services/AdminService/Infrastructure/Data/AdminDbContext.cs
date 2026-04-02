using AdminService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Infrastructure.Data;

public class AdminDbContext : DbContext
{
    public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options) { }

    public DbSet<KYCReview> KYCReviews => Set<KYCReview>();
    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<AdminActivityLog> ActivityLogs => Set<AdminActivityLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── KYCReview ──
        modelBuilder.Entity<KYCReview>(e =>
        {
            e.HasKey(k => k.Id);
            e.HasIndex(k => k.DocumentId).IsUnique();
            e.HasIndex(k => new { k.UserId, k.Status });
            e.Property(k => k.DocType).HasMaxLength(50).IsRequired();
            e.Property(k => k.FileUrl).HasMaxLength(500).IsRequired();
            e.Property(k => k.Status).HasMaxLength(20).HasDefaultValue("Pending");
            e.Property(k => k.ReviewNotes).HasMaxLength(1000);
        });

        // ── Campaign ──
        modelBuilder.Entity<Campaign>(e =>
        {
            e.HasKey(c => c.Id);
            e.HasIndex(c => c.Name).IsUnique();
            e.Property(c => c.Name).HasMaxLength(200).IsRequired();
            e.Property(c => c.Description).HasMaxLength(1000);
            e.Property(c => c.TriggerType).HasMaxLength(50).IsRequired();
            e.Property(c => c.BonusMultiplier).HasColumnType("decimal(5,2)");
            e.Property(c => c.MinimumAmount).HasColumnType("decimal(18,2)");
            e.HasIndex(c => new { c.IsActive, c.StartsAt, c.EndsAt });
        });

        // ── AdminActivityLog ──
        modelBuilder.Entity<AdminActivityLog>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.Action).HasMaxLength(100).IsRequired();
            e.Property(a => a.TargetType).HasMaxLength(50).IsRequired();
            e.Property(a => a.Details).HasMaxLength(2000);
            e.HasIndex(a => new { a.AdminUserId, a.CreatedAt });
        });
    }
}
