using BusinessApi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace BusinessApi.Repositories.Interface
{
    public interface IDashBoardRegisterRepository
    {
        Task<List<DashBoardRegisterViewTypeViewModel>> GetProject();
        Task<DashboardDto> CreateNewDashboard(DashboardDto dashboardDto);
        Task<List<DashboardTypeModel>> GetBindData(string dashboardId, string dashboardType);
        string SetDashboardType(string type);
        Task<List<DashboardTypeModel>> GetBindAvailableGroupData(string dashboardId, string dashboardType, string typename, string typeValues, string reTransferValue, string checkIsTransferButtonClick);
    }
}
