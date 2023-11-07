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
        Task<string> DeleteRecords(string val);
        Task<List<DashboardLayoutDto>> GetLayoutsWidgetAssociation(string SelectedDasboardType);
        Task<List<Dashboardeditdata>> EditLayoutsWidgetAssociation(Int64 Id);
        Task<string> SaveDashboardAssociationData(string selectedval, string dashboardId, string dashboardType);
        Task<string> UpdateDashboardData(string DashboardId, List<MutipleIds> Values, string Description);
    }
}
