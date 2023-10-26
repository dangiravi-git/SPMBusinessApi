using BusinessApi.Models;

namespace BusinessApi.DataAccessObject.Interface
{
    public interface IDashBoardRegisterDao
    {
        Task<DashBoardRegisterModel> GetProjects();
    }
}
