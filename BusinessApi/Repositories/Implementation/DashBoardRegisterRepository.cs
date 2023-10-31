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
            int newDbId = 0;
            var isCodeExists = await _projectListDao.IsDashboardCodeAlreadyExists(dashboard.Code);
            if (isCodeExists != null && isCodeExists.Rows.Count>0 && Convert.ToBoolean(isCodeExists.Rows[0][0]))
            {
                int created = await _projectListDao.CreateNewDashboard(dashboard.Code, dashboard.DashboardType, dashboard.Description,dashboard.CreatedBy,dashboard.IsWf);
                if (created != 0)
                {
                    var newDashboardId = await _projectListDao.IsDashboardCodeAlreadyExists(dashboard.Code);
                    if(newDashboardId != null && newDashboardId.Rows.Count>0)
                    {
                        int.TryParse( newDashboardId.Rows[0][0].ToString(),out newDbId);
                        if(newDbId != 0)
                        {
                            foreach (var item in dashboard.DashboardLayoutAssoList)
                            {
                                await _projectListDao.CreateLayoutAssociationWithDashboard(newDbId, item.LayoutId, item.LayoutSeq);
                            }
                        }
                    }
                   
                }
                dashboard.DashboardId = newDbId;
            }
            return dashboard;
        }
        public async Task<List<DashboardTypeModel>> GetBindData(string dashboardId, string dashboardType)
        {
            DataTable dashbordsdata = await _projectListDao.GetBindData(dashboardId, dashboardType);
            List<DashboardTypeModel> DashboardTypeModel = new();
            if (dashbordsdata != null)
            {
                foreach (DataRow row in dashbordsdata.Rows)
                {
                    var AddviewModel = new DashboardTypeModel
                    {

                        DashBoardType = row["DashBoardType"].ToString(),
                        C_UTEN = row["C_UTEN"].ToString(),
                        S_NOM = row["S_NOM"].ToString(),
                        
                    };
                    DashboardTypeModel.Add(AddviewModel);
                }
            }
            return DashboardTypeModel;
        }
    }
}
