using BusinessApi.Models;
using System.Data;

namespace BusinessApi.DataAccessObject.Interface
{
    public interface IDashBoardRegisterDao
    {
        Task<DataTable> GetProjects();
        Task<int> CreateNewDashboard(string? code1, string? code2, string? description);
        Task CreateLayoutAssociationWithDashboard(int dashboardId, int layoutId, int layoutSeq);
        Task<bool> IsDashboardCodeAlreadyExists(string? code);
    }
}
