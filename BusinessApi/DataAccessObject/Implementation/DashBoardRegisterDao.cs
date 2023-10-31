using BusinessApi.DataAccessObject.Interface;
using BusinessApi.Models;
using BusinessApi.Utils;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

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
        public async Task<int> CreateNewDashboard(string? code, string? type, string? description, double createdBy, string isWf)
        {
            StringBuilder sql = new();
            sql.Append("INSERT INTO TAB_PUBLISH_DASHBOARDS ( [DB_ID],[DB_CODE] ,[DB_DESCRIPTION] ,[CreatedBy] ,[DB_CREATION_DATE] ,[DB_TYPE] , [IS_WF])");
            sql.Append(" Values((select (ISNULL(MAX(DB_ID),0) + 1) as id from TAB_PUBLISH_DASHBOARDS),'" + code + "','" + description + "','" + createdBy + "',getdate(),'" + type + "','" + isWf + "')");
            int records = await _dbUtility.QueryExec(sql.ToString());
            return records;
        }
        public async Task CreateLayoutAssociationWithDashboard(int dashboardId, int layoutId, int layoutSeq)
        {
            StringBuilder sql = new();
            sql.Append("INSERT INTO ASCN_PUB_LAYOUT_DASHBOARDS(DB_ID, P_LAYOUT_ID, LAYOUT_SEQ)");
            sql.Append("Values(" + dashboardId + "," + layoutId + "," + layoutSeq + ")");
            int records = await _dbUtility.QueryExec(sql.ToString());
        }
        public async Task<DataTable> IsDashboardCodeAlreadyExists(string code)
        {
            StringBuilder sql = new();
            sql.Append("select [DB_ID] DB_ID from TAB_PUBLISH_DASHBOARDS where DB_CODE =" + code);
            var dt = await _dbUtility.ExecuteQuery(sql.ToString());
            return dt;
        }
        public async Task<DataTable> GetBindData(string dashboardId, string dashboardType)
        {
            List<DashboardTypeModel> modelProjectListViewTypenew = new();
            string sql = "SELECT 'G' AS DashBoardType, CONVERT(varchar, g.C_GRP) + '$G' AS C_UTEN, S_GRP_NOM AS S_NOM " +
                         "FROM GRP_T019 g " +
                         "JOIN ASCN_GRP_AZD_T204 a ON a.C_GRP = g.C_GRP AND a.C_AZD = " + 2 + " " +
                         "WHERE ISNULL(" + dashboardType + "_DASHBOARD, -1) = " + dashboardId;
            var result = await _dbUtility.ExecuteQuery(sql);
            return result;
        }
        public async Task<DataTable> GetBindAvailableGroupDataWithId(string dashboardId, string dashboardType, string funzlPermission)
        {
            string sql;
                sql = "SELECT CONVERT(VARCHAR, C_GRP) + '$G' AS C_UTEN, S_GRP_NOM AS S_NOM " +
                      " FROM GRP_T019 " +
                      "WHERE C_GRP IN (SELECT C_GRP FROM ASCN_GRP_AZD_T204 " +
                      "WHERE C_AZD = 2 AND C_GRP IN (SELECT C_GRP FROM ASCN_GRP_FUNZL_T018 WHERE C_FUNZL IN (" + funzlPermission + "))) " +
                      "AND php_dashboard NOT IN (" + dashboardId + ") AND ISNULL(" + dashboardType + "_DASHBOARD, -1) = -1";
            var result = await _dbUtility.ExecuteQuery(sql);
            return result;
        }
        public async Task<DataTable> GetBindAvailableGroupDataWithType(string dashboardType, string funzlPermission)
        {
            string sql;
            sql = "SELECT CONVERT(VARCHAR, C_GRP) + '$G' AS C_UTEN, S_GRP_NOM AS S_NOM " +
                      "FROM GRP_T019 " +
                      "WHERE C_GRP IN (SELECT C_GRP FROM ASCN_GRP_AZD_T204 " +
                      "WHERE C_AZD = 2 AND C_GRP IN (SELECT C_GRP FROM ASCN_GRP_FUNZL_T018 WHERE C_FUNZL IN (" + funzlPermission + "))) AND ISNULL(" + dashboardType + "_DASHBOARD, -1) = -1";
            var result = await _dbUtility.ExecuteQuery(sql);
            return result;
        }
    }
}