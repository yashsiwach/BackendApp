using AdminService.Application.DTOs;
using AdminService.Application.Interfaces;
using AdminService.Application.Interfaces.Repositories;
using AdminService.Application.Mappers;
using AdminService.Domain.Entities;
using SharedContracts.DTOs;

namespace AdminService.Application.Services;

public class CampaignServiceImpl : ICampaignService
{
    private readonly ICampaignRepository _campaigns;
    private readonly IActivityLogRepository _logs;
    private readonly ILogger<CampaignServiceImpl> _logger;

    /// <summary>Initializes campaign management dependencies for repository access, audit logging, and diagnostics.</summary>
    public CampaignServiceImpl(ICampaignRepository campaigns, IActivityLogRepository logs, ILogger<CampaignServiceImpl> logger)
    {
        _campaigns = campaigns;
        _logs      = logs;
        _logger    = logger;
    }

    /// <summary>Returns paginated campaigns with optional active-status filtering.</summary>
    public Task<PaginatedResult<CampaignDto>> GetAllAsync(int page, int size, bool? activeOnly = null) =>
        _campaigns.GetPagedAsync(page, size, activeOnly);

    /// <summary>Loads a campaign by id and maps it to a DTO or throws when not found.</summary>
    public async Task<CampaignDto> GetByIdAsync(Guid id)
    {
        var c = await _campaigns.FindByIdAsync(id)
            ?? throw new KeyNotFoundException("Campaign not found.");
        return AdminMapper.ToDto(c);
    }

    /// <summary>Validates, creates, audits, and persists a new campaign, then returns its DTO.</summary>
    public async Task<CampaignDto> CreateAsync(Guid adminId, CreateCampaignRequest request)
    {
        if (request.EndsAt <= request.StartsAt)
            throw new InvalidOperationException("End date must be after start date.");

        if (await _campaigns.ExistsByNameAsync(request.Name))
            throw new InvalidOperationException($"A campaign named '{request.Name}' already exists.");

        var campaign = new Campaign
        {
            Name            = request.Name,
            Description     = request.Description,
            TriggerType     = request.TriggerType,
            BonusMultiplier = request.BonusMultiplier,
            MinimumAmount   = request.MinimumAmount,
            StartsAt        = request.StartsAt,
            EndsAt          = request.EndsAt,
            IsActive        = true,
            CreatedBy       = adminId
        };

        await _campaigns.AddAsync(campaign);
        await _logs.AddAsync(new AdminActivityLog { AdminUserId = adminId, Action = "CampaignCreated", TargetType = "Campaign", TargetId = campaign.Id, Details = $"Created '{campaign.Name}'" });
        await _campaigns.SaveAsync();

        _logger.LogInformation("Campaign created: {CampaignId} '{Name}' by Admin {AdminId}", campaign.Id, campaign.Name, adminId);
        return AdminMapper.ToDto(campaign);
    }

    /// <summary>Validates and updates an existing campaign, records admin activity, and returns the updated DTO.</summary>
    public async Task<CampaignDto> UpdateAsync(Guid id, Guid adminId, UpdateCampaignRequest request)
    {
        var campaign = await _campaigns.FindByIdAsync(id)
            ?? throw new KeyNotFoundException("Campaign not found.");

        if (request.EndsAt <= request.StartsAt)
            throw new InvalidOperationException("End date must be after start date.");

        campaign.Description     = request.Description;
        campaign.BonusMultiplier = request.BonusMultiplier;
        campaign.MinimumAmount   = request.MinimumAmount;
        campaign.StartsAt        = request.StartsAt;
        campaign.EndsAt          = request.EndsAt;
        campaign.IsActive        = request.IsActive;
        campaign.UpdatedAt       = DateTime.UtcNow;

        await _logs.AddAsync(new AdminActivityLog { AdminUserId = adminId, Action = "CampaignUpdated", TargetType = "Campaign", TargetId = campaign.Id, Details = $"Updated '{campaign.Name}'" });
        await _campaigns.SaveAsync();
        return AdminMapper.ToDto(campaign);
    }

    /// <summary>Deletes an existing campaign, records the admin audit entry, and persists the removal.</summary>
    public async Task DeleteAsync(Guid id, Guid adminId)
    {
        var campaign = await _campaigns.FindByIdAsync(id)
            ?? throw new KeyNotFoundException("Campaign not found.");

        await _logs.AddAsync(new AdminActivityLog { AdminUserId = adminId, Action = "CampaignDeleted", TargetType = "Campaign", TargetId = campaign.Id, Details = $"Deleted '{campaign.Name}'" });
        await _campaigns.RemoveAsync(campaign);
        await _campaigns.SaveAsync();

        _logger.LogInformation("Campaign deleted: {CampaignId} by Admin {AdminId}", id, adminId);
    }

}
