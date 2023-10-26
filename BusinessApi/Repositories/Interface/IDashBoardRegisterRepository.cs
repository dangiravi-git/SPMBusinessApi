using BusinessApi.Models;

namespace BusinessApi.Repositories.Interface
{
    public interface IDashBoardRegisterRepository
    {
        DashBoardRegisterModel GetProject(DashBoardRegisterModel parameters);
    }
}
