using BusinessApi.Models;

namespace BusinessApi.Repositories.Interface
{
    public interface IDashBoardRegisterRepository
    {
        Task<DashBoardRegisterModel> GetProject();
    }
}
