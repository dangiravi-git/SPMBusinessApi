using BusinessApi.Models;
using System.Data;

namespace BusinessApi.Repositories.Interface
{
    public interface IDashBoardRegisterRepository
    {
        Task<List<DashBoardRegisterViewTypeViewModel>> GetProject();
        Task<DashboardDto> CreateNewDashboard(DashboardDto dashboardDto);
        Task<List<DashboardTypeModel>> GetBindData(string dashboardId, string dashboardType);
    }
}
