namespace AdminService.Application.DTOs;

// ── KYC Review DTOs ──

public record KYCReviewDto
{
    public Guid Id { get; init; }
    public Guid DocumentId { get; init; }
    public Guid UserId { get; init; }
    public string DocType { get; init; } = string.Empty;
    public string FileUrl { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? ReviewNotes { get; init; }
    public DateTime SubmittedAt { get; init; }
    public DateTime? ReviewedAt { get; init; }
}

public record KYCApproveRequest
{
    public string Notes { get; init; } = string.Empty;
}

public record KYCRejectRequest
{
    public string Reason { get; init; } = string.Empty;
}

// ── Campaign DTOs ──

public record CampaignDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string TriggerType { get; init; } = string.Empty;
    public decimal BonusMultiplier { get; init; }
    public decimal? MinimumAmount { get; init; }
    public DateTime StartsAt { get; init; }
    public DateTime EndsAt { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record CreateCampaignRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string TriggerType { get; init; } = string.Empty;   // TopUp | Transfer | All
    public decimal BonusMultiplier { get; init; } = 2m;
    public decimal? MinimumAmount { get; init; }
    public DateTime StartsAt { get; init; }
    public DateTime EndsAt { get; init; }
}

public record UpdateCampaignRequest
{
    public string Description { get; init; } = string.Empty;
    public decimal BonusMultiplier { get; init; }
    public decimal? MinimumAmount { get; init; }
    public DateTime StartsAt { get; init; }
    public DateTime EndsAt { get; init; }
    public bool IsActive { get; init; }
}

// ── Dashboard DTOs ──

public record DashboardStatsDto
{
    public int PendingKYCCount { get; init; }
    public int ApprovedKYCToday { get; init; }
    public int RejectedKYCToday { get; init; }
    public int ActiveCampaigns { get; init; }
    public int TotalCampaigns { get; init; }
    public int AdminActionsToday { get; init; }
}
