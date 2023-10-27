using BusinessApi.Models;

namespace BusinessApi.Repositories.Interface
{
    public interface IDashBoardRegisterRepository
    {
        Task<List<DashBoardRegisterViewTypeViewModel>> GetProject();
        Task<DashboardDto> CreateNewDashboard(DashboardDto dashboardDto);
    }
}
