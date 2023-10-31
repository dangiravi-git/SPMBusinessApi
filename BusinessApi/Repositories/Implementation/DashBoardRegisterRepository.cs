using BusinessApi.DataAccessObject.Interface;
using BusinessApi.Models;
using BusinessApi.Repositories.Interface;
using BusinessApi.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Data;
using System.Net;
using System.Resources;
using System.Text;

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
        public async Task<string> DeleteRecords(string val)
        {
            DataTable result = null;
            string[] str = val.Split(',');
            

            foreach (string item in str)
            {
                DataRow type = await _projectListDao.GetTypeVal(item);
                StringBuilder sqlString = new StringBuilder();

                var columnname = GetColumnsValues(type[0].ToString());
                string colName = columnname.columnname;
                string grpColName = columnname.grpColumnname;
                string checkColName = columnname.ischeckColumnName;

                object groups = await _projectListDao.GetGroupVal(colName, type[1]);
                string groupsAsString = ConvertGroupsToString(groups);
                if (groups != null && groups != DBNull.Value && groupsAsString != "" && groupsAsString != null)
                {
                    await _projectListDao.updatethedata(groupsAsString, checkColName, colName, grpColName);
                }
                result = await _projectListDao.deletethedata(type[1], item);
            }
            var msg = "error";
            if (result == null)
            {
                msg = "Data Deleted Sucessfully";
            }
            return msg;
        }
        public (string columnname, string grpColumnname, string ischeckColumnName) GetColumnsValues(string val)
        {
            string columnname = "";
            string grpColumnname = "";
            string ischeckColumnName = "";
            switch (val)
            {
                case "H":
                    columnname = "HP_Dashboard";
                    grpColumnname = "HP_C_GRP";
                    ischeckColumnName = "isChecked_HP";
                    break;
                case "P":
                    columnname = "PHP_Dashboard";
                    grpColumnname = "PHP_C_GRP";
                    ischeckColumnName = "isChecked_PHP";
                    break;
                case "S":
                    columnname = "SP_Dashboard";
                    grpColumnname = "SP_C_GRP";
                    ischeckColumnName = "isChecked_SP";
                    break;
                case "F":
                    columnname = "PFP_Dashboard";
                    grpColumnname = "PFP_C_GRP";
                    ischeckColumnName = "isChecked_PFP";
                    break;
                case "M":
                    columnname = "PRG_Dashboard";
                    grpColumnname = "PRG_C_GRP";
                    ischeckColumnName = "isChecked_PRG";
                    break;
            }
            return (columnname, grpColumnname, ischeckColumnName);
        }
        private string ConvertGroupsToString(object groups)
        {
            if (groups is DataTable dt)
            {
                var values = dt.AsEnumerable()
                              .Select(row => row.Field<string>("Column1"))
                              .Where(value => !string.IsNullOrEmpty(value))
                              .ToArray();
                return string.Join(",", values);
            }
            else if (groups is string groupsString)
            {
                
                return groupsString;
            }
            else
            {
                return null;
            }
        }
    }
}
