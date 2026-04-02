using AdminService.Application.DTOs;
using AdminService.Domain.Entities;
using SharedContracts.DTOs;

namespace AdminService.Application.Interfaces.Repositories;

public interface ICampaignRepository
{
    Task<bool> ExistsByNameAsync(string name);
    Task AddAsync(Campaign campaign);
    Task<Campaign?> FindByIdAsync(Guid id);
    Task<PaginatedResult<CampaignDto>> GetPagedAsync(int page, int size, bool? activeOnly);
    Task RemoveAsync(Campaign campaign);
    Task SaveAsync();
}
