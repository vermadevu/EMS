using API.DTOs.Dashboard;

namespace API.Interfaces.Service
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardAsync();
    }
}
