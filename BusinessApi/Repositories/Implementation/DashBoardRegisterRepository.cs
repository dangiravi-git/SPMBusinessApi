using BusinessApi.DataAccessObject.Interface;
using BusinessApi.Models;
using BusinessApi.Repositories.Interface;
using BusinessApi.Utils;
using System;
using System.Data;
using System.Net;

namespace BusinessApi.Repositories.Implementation
{
    public class DashBoardRegisterRepository : IDashBoardRegisterRepository
    {
        private IDashBoardRegisterDao _projectListDao;
        private readonly IDbUtility _dbUtility;
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
                        Value = row["C_UTEN"].ToString(),
                        Name = row["S_NOM"].ToString(  ),
                        
                    };
                    DashboardTypeModel.Add(AddviewModel);           
                }
            }
            return DashboardTypeModel;
        }
        public async Task<List<DashboardTypeModel>> GetBindAvailableGroupData(string dashboardId, string dashboardType, string typename, string typeValues, string reTransferValue, string checkIsTransferButtonClick)
        {
            string funzlPermission;
            string groupReTransferId = "0";
           
            if (string.IsNullOrEmpty(typeValues))
            {
                typeValues = "0";
            }
            groupReTransferId = await ProcessReTransferValue(reTransferValue, groupReTransferId);
            funzlPermission = GetFunzlPermissionValue(dashboardType);
            DataTable dashbordsdata = null;
            if (typename == "G")
            {
                if ((groupReTransferId == "0" || string.IsNullOrEmpty(groupReTransferId)) && checkIsTransferButtonClick == "1")
                {
                    dashbordsdata = await _projectListDao.GetBindAvailableGroupDataWithId(dashboardId, dashboardType,funzlPermission);
                }
                else
                {
                    dashbordsdata = await _projectListDao.GetBindAvailableGroupDataWithType(dashboardType,funzlPermission);
                }
                    
            }
           
            List<DashboardTypeModel> DashboardTypeModel = new();
            if (dashbordsdata != null)
            {
                foreach (DataRow row in dashbordsdata.Rows)
                {
                    var AddviewModel = new DashboardTypeModel
                    {

                        DashBoardType = "U",
                        Value = row["C_UTEN"].ToString(),
                        Name = row["S_NOM"].ToString(),

                    };
                    DashboardTypeModel.Add(AddviewModel);
                }
            }
            return DashboardTypeModel;
        }
        public string SetDashboardType(string type)
        {
            string hdnDashboardType;

            if (type == "Initiative Dashboard")
            {
                hdnDashboardType = "PHP";
            }
            else if (type == "Portfolio Dashboard")
            {
                hdnDashboardType = "PFP";
            }
            else if (type == "Strategy Dashboard")
            {
                hdnDashboardType = "SP";
            }
            else if (type == "Program Dashboard")
            {
                hdnDashboardType = "PRG";
            }
            else
            {
                hdnDashboardType = "HP";
            }

            return hdnDashboardType;
        }
        public string GetFunzlPermissionValue(string dashboardType)
        {
            string funzlPermission;
            switch (dashboardType)
            {
                case "SP":
                    funzlPermission = "1000,1001";
                    break;
                case "PHP":
                    funzlPermission = "1036,1037,1038";
                    break;
                case "PFP":
                    funzlPermission = "1005,1006";
                    break;
                case "HP":
                    funzlPermission = "1030,1029,1031,1032";
                    break;
                case "PRG":
                    funzlPermission = "1010,1011";
                    break;
                default:
                    funzlPermission = "";
                    break;
            }
            return funzlPermission;
        }
        private async Task<string> ProcessReTransferValue(string reTransferValue, string groupReTransferId)
        {
            if (!string.IsNullOrEmpty(reTransferValue))
            {
                string[] finalSelectedUsersRoleGroups = reTransferValue.Split(',');
                foreach (string finalSelectedUsersRoleGroup in finalSelectedUsersRoleGroups)
                {
                    string[] userRoleGroupId = finalSelectedUsersRoleGroup.Split('$');
                    if (userRoleGroupId.Length == 2 && userRoleGroupId[1] == "G")
                    {
                        if (groupReTransferId == "0")
                        {
                            groupReTransferId = await _dbUtility.QN(userRoleGroupId[0]);
                        }
                        else
                        {
                            groupReTransferId += "," + await _dbUtility.QN(userRoleGroupId[0]);
                        }
                    }
                }
            }
            return groupReTransferId;
        }

    }
}
