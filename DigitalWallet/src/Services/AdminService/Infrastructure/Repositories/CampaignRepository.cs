using AdminService.Application.DTOs;
using AdminService.Application.Interfaces.Repositories;
using AdminService.Domain.Entities;
using AdminService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SharedContracts.DTOs;

namespace AdminService.Infrastructure.Repositories;

public class CampaignRepository : ICampaignRepository
{
    private readonly AdminDbContext _db;
    public CampaignRepository(AdminDbContext db) => _db = db;

    public Task<bool> ExistsByNameAsync(string name) =>
        _db.Campaigns.AnyAsync(c => c.Name == name);

    public async Task AddAsync(Campaign campaign) =>
        await _db.Campaigns.AddAsync(campaign);

    public Task<Campaign?> FindByIdAsync(Guid id) =>
        _db.Campaigns.FindAsync(id).AsTask();

    public async Task<PaginatedResult<CampaignDto>> GetPagedAsync(int page, int size, bool? activeOnly)
    {
        var query = _db.Campaigns.AsQueryable();
        if (activeOnly.HasValue) query = query.Where(c => c.IsActive == activeOnly.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .Select(c => new CampaignDto
            {
                Id              = c.Id,
                Name            = c.Name,
                Description     = c.Description,
                TriggerType     = c.TriggerType,
                BonusMultiplier = c.BonusMultiplier,
                MinimumAmount   = c.MinimumAmount,
                StartsAt        = c.StartsAt,
                EndsAt          = c.EndsAt,
                IsActive        = c.IsActive,
                CreatedAt       = c.CreatedAt
            })
            .ToListAsync();

        return new PaginatedResult<CampaignDto> { Items = items, Page = page, PageSize = size, TotalCount = total };
    }

    public Task RemoveAsync(Campaign campaign)
    {
        _db.Campaigns.Remove(campaign);
        return Task.CompletedTask;
    }

    public Task SaveAsync() => _db.SaveChangesAsync();
}
