using BusinessApi.Models;
using BusinessApi.Repositories.Interface;
using BusinessApi.DataAccessObject.Interface;

namespace BusinessApi.Repositories.Implementation
{
    public class DashBoardRegisterRepository : IDashBoardRegisterRepository
    {
        private IDashBoardRegisterDao<DashBoardRegisterModel, int> _projectListDao;
        public DashBoardRegisterRepository(IDashBoardRegisterDao<DashBoardRegisterModel, int> projectListDao)
        {
            _projectListDao = projectListDao;
        }

        public DashBoardRegisterModel GetProject(DashBoardRegisterModel parameters)
        {
            return _projectListDao.getProjects(parameters);
        }
    }
}
