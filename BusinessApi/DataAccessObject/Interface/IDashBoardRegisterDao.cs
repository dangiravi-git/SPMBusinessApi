using BusinessApi.Models;

namespace BusinessApi.DataAccessObject.Interface
{
    public interface IDashBoardRegisterDao<T, in PK>
    {
        DashBoardRegisterModel getProjects(DashBoardRegisterModel parameters);
    }
}
