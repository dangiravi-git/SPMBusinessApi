using BusinessApi.Models;
using System.Data;

namespace BusinessApi.DataAccessObject.Interface
{
    public interface IDashBoardRegisterDao
    {
        Task<DataTable> GetProjects();
        Task<int> CreateNewDashboard(string? code, string? type, string? description, double createdBy, string isWf);
        Task CreateLayoutAssociationWithDashboard(int dashboardId, int layoutId, int layoutSeq);
        Task<DataTable> IsDashboardCodeAlreadyExists(string code);
        Task<DataTable> GetBindData(string dashboardId, string dashboardType);
        Task<DataTable> GetBindAvailableGroupDataWithId(string dashboardId, string dashboardType,string funzlPermission);
        Task<DataTable> GetBindAvailableGroupDataWithType(string dashboardType,string funzlPermission);
    }
}
