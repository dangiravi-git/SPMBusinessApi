﻿using BusinessApi.DataAccessObject.Interface;
using BusinessApi.Models;
using BusinessApi.Utils;
using System.Data;
using System.Text;

namespace BusinessApi.DataAccessObject.Implementation
{
    public class DashBoardRegisterDao : IDashBoardRegisterDao
    {
        private readonly IDbUtility _dbUtility;
        public DashBoardRegisterDao(IDbUtility dbUtility)
        {
            _dbUtility = dbUtility;
        }
        public async Task<DataTable> GetProjects()
        {
            List<DashBoardRegisterViewTypeViewModel> modelProjectListViewType = new();
            var sqlSelect = "Select DB_ID, DB_CODE, DB_DESCRIPTION, case when CreatedBy= 80125112841950 then 'Global System Administrator' end as CreatedBy , DB_CREATION_DATE, CASE WHEN db_type='P' THEN 'Initiative Dashboard' WHEN db_type='M' THEN 'Program Dashboard'  WHEN db_type='H' THEN 'Analytics Dashboard'  WHEN db_type='S' THEN 'Strategy Dashboard' WHEN db_type='F' THEN 'Portfolio Dashboard' ELSE ''  END AS  db_type, IS_WF from tab_publish_dashboards db_type order by db_creation_date desc,DB_CODE";
            var dashbords = await _dbUtility.ExecuteQuery(sqlSelect);
            return dashbords;
        }
        public async Task<int> CreateNewDashboard(string? code1, string? code2, string? description)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO TAB_PUBLISH_DASHBOARDS ( [DB_ID],[DB_CODE] ,[DB_DESCRIPTION] ,[CreatedBy] ,[DB_CREATION_DATE] ,[DB_TYPE] , [IS_WF])");
            sql.Append(" Values((select (ISNULL(MAX(DB_ID),0) + 1) as id from TAB_PUBLISH_DASHBOARDS ),)");

        }
        public Task CreateLayoutAssociationWithDashboard(int dashboardId, int layoutId, int layoutSeq)
        {
            throw new NotImplementedException();
        }
        public Task<bool> IsDashboardCodeAlreadyExists(string? code)
        {
            throw new NotImplementedException();
        }
    }
}
