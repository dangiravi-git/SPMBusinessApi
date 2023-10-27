using BusinessApi.DataAccessObject.Interface;
using BusinessApi.Models;
using BusinessApi.Repositories.Interface;
using System;
using System.Data;

namespace BusinessApi.Repositories.Implementation
{
    public class DashBoardRegisterRepository : IDashBoardRegisterRepository
    {
        private IDashBoardRegisterDao _projectListDao;
        public DashBoardRegisterRepository(IDashBoardRegisterDao projectListDao)
        {
            _projectListDao = projectListDao;
        }
        public async Task<List<DashBoardRegisterViewTypeViewModel>> GetProject()
        {
            DataTable dashbords = await _projectListDao.GetProjects();
            List<DashBoardRegisterViewTypeViewModel> dashBoardRegisterViewTypeViewModel = new();
            if (dashbords != null)
            {
                foreach (DataRow row in dashbords.Rows)
                {
                    var viewModel = new DashBoardRegisterViewTypeViewModel
                    {
                        DashboardId = Convert.ToInt32(row["DB_ID"]),
                        Code = row["DB_CODE"].ToString(),
                        Description = row["DB_DESCRIPTION"].ToString(),
                        DashboardCreatedBy = Convert.ToString(row["CreatedBy"]),
                        CreatedDate = Convert.ToDateTime(row["DB_CREATION_DATE"]),
                        DashboardType = Convert.ToString(row["db_type"]),
                        IsWf = Convert.ToString(row["IS_WF"])
                    };
                    dashBoardRegisterViewTypeViewModel.Add(viewModel);
                }
            }
            return dashBoardRegisterViewTypeViewModel;
        }
        public async Task<DashboardDto> CreateNewDashboard(DashboardDto dashboard)
        {
            bool isCodeExists = await _projectListDao.IsDashboardCodeAlreadyExists(dashboard.Code);
            if (isCodeExists)
            {
                int dashboardId = await _projectListDao.CreateNewDashboard(dashboard.Code, dashboard.Code, dashboard.Description);
                if (dashboardId != 0)
                {
                    foreach (var item in dashboard.DashboardLayoutAssoList)
                    {
                        await _projectListDao.CreateLayoutAssociationWithDashboard(dashboardId, item.LayoutId, item.LayoutSeq);
                    }
                }
                dashboard.DashboardId = dashboardId;
            }
          
            return dashboard;
        }
    }
}
