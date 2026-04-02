namespace AdminService.Domain.Entities;

/// <summary>A promotional rewards campaign (bonus points for a period).</summary>
public class Campaign
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TriggerType { get; set; } = string.Empty;   // TopUp | Transfer | All
    public decimal BonusMultiplier { get; set; } = 1m;         // e.g. 2x points
    public decimal? MinimumAmount { get; set; }                // optional minimum transaction ₹
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
