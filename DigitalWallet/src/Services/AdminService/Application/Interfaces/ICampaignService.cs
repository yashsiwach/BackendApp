using AdminService.Application.DTOs;
using SharedContracts.DTOs;

namespace AdminService.Application.Interfaces;

public interface ICampaignService
{
    Task<PaginatedResult<CampaignDto>> GetAllAsync(int page, int size, bool? activeOnly = null);
    Task<CampaignDto> GetByIdAsync(Guid id);
    Task<CampaignDto> CreateAsync(Guid adminId, CreateCampaignRequest request);
    Task<CampaignDto> UpdateAsync(Guid id, Guid adminId, UpdateCampaignRequest request);
    Task DeleteAsync(Guid id, Guid adminId);
}
