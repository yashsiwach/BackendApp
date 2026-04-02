using AdminService.Domain.Entities;

namespace AdminService.Application.Interfaces.Repositories;

public interface IActivityLogRepository
{
    Task AddAsync(AdminActivityLog log);
    Task<int> CountPendingKYCAsync();
    Task<int> CountKYCByStatusTodayAsync(string status);
    Task<int> CountActiveCampaignsAsync();
    Task<int> CountAllCampaignsAsync();
    Task<int> CountAdminActionsTodayAsync();
    Task SaveAsync();
}
