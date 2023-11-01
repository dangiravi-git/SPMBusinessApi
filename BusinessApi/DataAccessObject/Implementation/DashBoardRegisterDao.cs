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
            sql.Append("select [DB_ID] DB_ID from TAB_PUBLISH_DASHBOARDS where DB_CODE ='" + code+"'");
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
        public async Task<DataRow> GetTypeVal(string item)
        {
            string sql = "SELECT DB_TYPE, db_id FROM TAB_PUBLISH_DASHBOARDS WHERE DB_CODE = '" + item + "'";
            var result = await _dbUtility.ExecuteQuery(sql);
            if (result.Rows.Count > 0)
            {
                return result.Rows[0];
            }

            return null; 
        }
        public async Task<object> GetGroupVal(string colName, object v)
        {
            string sql = "SELECT Stuff(list, 1, 1, '')" + Environment.NewLine;
            sql += "FROM   (SELECT DISTINCT ',' + Cast(c_grp AS NVARCHAR(50)) + '' AS [text()]" + Environment.NewLine;
            sql += "        FROM   grp_t019" + Environment.NewLine;
            sql += "        WHERE  " + colName + " = "+ v + "" + Environment.NewLine; 
            sql += "        FOR xml path('')) AS Sub(list)" + Environment.NewLine;
            object groups = await _dbUtility.ExecuteQuery(sql);
            return groups;
        }
        public async Task updatethedata(string groups, string checkColName, string colName, string grpColName)
        {
            if (groups != null)
            {
                string updateSql1 = $"UPDATE GRP_T019 SET {colName} = -1 WHERE C_GRP IN ({groups})";
                string updateSql2 = $"UPDATE ASCN_GROUP_USER_DASHBOARD SET {checkColName} = -1 WHERE {grpColName} IN ({groups})";

                await _dbUtility.ExecuteQuery(updateSql1);
                await _dbUtility.ExecuteQuery(updateSql2);
            }
        }
        public async Task<DataTable> deletethedata(object v, string item)
        {
            string deleteSql1 = $"DELETE FROM ASCN_PUB_LAYOUT_DASHBOARDS WHERE DB_ID = {v}";
            await _dbUtility.ExecuteQuery(deleteSql1);

            string deleteSql2 = $"DELETE FROM tab_publish_dashboards WHERE DB_CODE = '{item.Trim()}'";
            DataTable dataTable = await _dbUtility.ExecuteQuery(deleteSql2);
             return dataTable;
        }
        public async Task<DataTable> GetLayoutData(string selectedValue)
        {
            string sql = "select P_LAYOUT_ID as ID, LAYOUT_NAME + ' - ' + Revision as des from TAB_PUBLISH_LAYOUTS where LAYOUT_TYPE = '" + selectedValue + "'";
            DataTable dataTable = await _dbUtility.ExecuteQuery(sql);
            return dataTable;
        }
        public async Task<DataTable> GetWidgetData(string selectedValue, int dashboardType)
        {
            string dtWidgetsSql = "select distinct L.P_LAYOUT_ID, layout_name, M.WIDGET_ID, WIDGET_NAME" +
                                  " from TAB_PUBLISH_LAYOUTS L join TAB_MAPPING_PUBLISH_LAYOUTS M on L.P_LAYOUT_ID = M.P_LAYOUT_ID " +
                                  " left join tab_gen_widgets W on W.WIDGET_ID = M.WIDGET_ID and L.LAYOUT_TYPE = '" + selectedValue + "'" +
                                  " and w.widget_type = " + dashboardType;
            DataTable dataTable = await _dbUtility.ExecuteQuery(dtWidgetsSql);
            return dataTable;
        }
        public async Task<DataTable> GetMenuList()
        {
            string dtMenuSql = "select id,F_ID,MENU_NAME from BPM_MENU where C_AZD = 2";
            DataTable dataTable = await _dbUtility.ExecuteQuery(dtMenuSql);
            return dataTable;
        }
    }
}