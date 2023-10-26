using BusinessApi.Models;
using BusinessApi.Repositories.Interface;
using BusinessApi.DataAccessObject.Interface;
using System;

namespace BusinessApi.Repositories.Implementation
{
    public class DashBoardRegisterRepository : IDashBoardRegisterRepository
    {
        private IDashBoardRegisterDao _projectListDao;
        public DashBoardRegisterRepository(IDashBoardRegisterDao projectListDao)
        {
            _projectListDao = projectListDao;
        }

        public async Task<DashBoardRegisterModel> GetProject()
        {
            return await  _projectListDao.GetProjects();
        }
    }
}
