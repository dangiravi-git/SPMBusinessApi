using BusinessApi.DataAccessObject.Interface;
using BusinessApi.Models;
using BusinessApi.Utils;
using System.Data;
using System.Text;

namespace BusinessApi.DataAccessObject.Implementation
{
    public class DashBoardRegisterDao : IDashBoardRegisterDao
    {
        private readonly IDbUtility _dbUtility;
        private readonly ILogger<DashBoardRegisterDao> _logger;
        public DashBoardRegisterDao(IDbUtility dbUtility, ILogger<DashBoardRegisterDao> logger)
        {
            try
            {
                _dbUtility = dbUtility;
                _logger = logger;
                _logger.LogInformation($"Initiate {nameof(DashBoardRegisterDao)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
          
        }
        public async Task<DataTable> GetProjects()
        {
            try
            {
                List<DashBoardRegisterViewTypeViewModel> modelProjectListViewType = new();
                var sqlSelect = "Select DB_ID, DB_CODE, DB_DESCRIPTION, case when CreatedBy= 80125112841950 then 'Global System Administrator' end as CreatedBy , DB_CREATION_DATE, CASE WHEN db_type='P' THEN 'Initiative Dashboard' WHEN db_type='M' THEN 'Program Dashboard'  WHEN db_type='H' THEN 'Analytics Dashboard'  WHEN db_type='S' THEN 'Strategy Dashboard' WHEN db_type='F' THEN 'Portfolio Dashboard' ELSE ''  END AS  db_type, IS_WF from tab_publish_dashboards db_type order by db_creation_date desc,DB_CODE";
                var dashbords = await _dbUtility.ExecuteQuery(sqlSelect);
                return dashbords;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
           
        }
        public async Task<int> CreateNewDashboard(string? code, string? type, string? description, double createdBy, string isWf)
        {
            try
            {
                StringBuilder sql = new();
                sql.Append("INSERT INTO TAB_PUBLISH_DASHBOARDS ( [DB_ID],[DB_CODE] ,[DB_DESCRIPTION] ,[CreatedBy] ,[DB_CREATION_DATE] ,[DB_TYPE] , [IS_WF])");
                sql.Append(" Values((select (ISNULL(MAX(DB_ID),0) + 1) as id from TAB_PUBLISH_DASHBOARDS),'" + code + "','" + description + "','" + createdBy + "',getdate(),'" + type + "','" + isWf + "')");
                int records = await _dbUtility.QueryExec(sql.ToString());
                return records;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task CreateLayoutAssociationWithDashboard(int dashboardId, int layoutId, int layoutSeq)
        {
            try
            {
                StringBuilder sql = new();
                sql.Append("INSERT INTO ASCN_PUB_LAYOUT_DASHBOARDS(DB_ID, P_LAYOUT_ID, LAYOUT_SEQ)");
                sql.Append("Values(" + dashboardId + "," + layoutId + "," + layoutSeq + ")");
                int records = await _dbUtility.QueryExec(sql.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<DataTable> IsDashboardCodeAlreadyExists(string code)
        {
            try
            {
                StringBuilder sql = new();
                sql.Append("select [DB_ID] DB_ID from TAB_PUBLISH_DASHBOARDS where DB_CODE ='" + code + "'");
                var dt = await _dbUtility.ExecuteQuery(sql.ToString());
                return dt;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<DataTable> GetBindData(string dashboardId, string dashboardType)
        {
            try
            {
                List<DashboardTypeModel> modelProjectListViewTypenew = new();
                string sql = "SELECT 'G' AS DashBoardType, CONVERT(varchar, g.C_GRP) + '$G' AS C_UTEN, S_GRP_NOM AS S_NOM " +
                             "FROM GRP_T019 g " +
                             "JOIN ASCN_GRP_AZD_T204 a ON a.C_GRP = g.C_GRP AND a.C_AZD = " + 2 + " " +
                             "WHERE ISNULL(" + dashboardType + "_DASHBOARD, -1) = " + dashboardId;
                var result = await _dbUtility.ExecuteQuery(sql);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<DataTable> GetBindAvailableGroupDataWithId(string dashboardId, string dashboardType, string funzlPermission)
        {
            try
            {
               
                string sql = "SELECT CONVERT(VARCHAR, C_GRP) + '$G' AS C_UTEN, S_GRP_NOM AS S_NOM " +
                      " FROM GRP_T019 " +
                      "WHERE C_GRP IN (SELECT C_GRP FROM ASCN_GRP_AZD_T204 " +
                      "WHERE C_AZD = 2 AND C_GRP IN (SELECT C_GRP FROM ASCN_GRP_FUNZL_T018 WHERE C_FUNZL IN (" + funzlPermission + "))) " +
                      "AND php_dashboard NOT IN (" + dashboardId + ") AND ISNULL(" + dashboardType + "_DASHBOARD, -1) = -1";
                var result = await _dbUtility.ExecuteQuery(sql);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<DataTable> GetBindAvailableGroupDataWithType(string dashboardType, string funzlPermission)
        {
            try
            {
                string sql = "SELECT CONVERT(VARCHAR, C_GRP) + '$G' AS C_UTEN, S_GRP_NOM AS S_NOM " +
                    "FROM GRP_T019 " +
                    "WHERE C_GRP IN (SELECT C_GRP FROM ASCN_GRP_AZD_T204 " +
                    "WHERE C_AZD = 2 AND C_GRP IN (SELECT C_GRP FROM ASCN_GRP_FUNZL_T018 WHERE C_FUNZL IN (" + funzlPermission + "))) AND ISNULL(" + dashboardType + "_DASHBOARD, -1) = -1";
                var result = await _dbUtility.ExecuteQuery(sql);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<DataRow> GetTypeVal(string item)
        {
            try
            {
                string sql = "SELECT DB_TYPE, db_id FROM TAB_PUBLISH_DASHBOARDS WHERE DB_CODE = '" + item + "'";
                var result = await _dbUtility.ExecuteQuery(sql);
                return result.Rows.Count > 0 ? result.Rows[0] : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<object> GetGroupVal(string colName, object v)
        {
            try
            {
                string sql = "SELECT Stuff(list, 1, 1, '')" + Environment.NewLine;
                sql += "FROM   (SELECT DISTINCT ',' + Cast(c_grp AS NVARCHAR(50)) + '' AS [text()]" + Environment.NewLine;
                sql += "        FROM   grp_t019" + Environment.NewLine;
                sql += "        WHERE  " + colName + " = " + v + "" + Environment.NewLine;
                sql += "        FOR xml path('')) AS Sub(list)" + Environment.NewLine;
                object groups = await _dbUtility.ExecuteQuery(sql);
                return groups;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task updatethedata(string groups, string checkColName, string colName, string grpColName)
        {
            try
            {
                string updateSql = $"UPDATE GRP_T019 SET {colName} = -1 WHERE C_GRP IN ({groups}); ";
                updateSql += $"UPDATE ASCN_GROUP_USER_DASHBOARD SET {checkColName} = -1 WHERE {grpColName} IN ({groups}); ";
                await _dbUtility.ExecuteQuery(updateSql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<DataTable> deletethedata(object v, string item)
        {
            try
            {
                string deleteSql = $"DELETE FROM ASCN_PUB_LAYOUT_DASHBOARDS WHERE DB_ID = {v}; ";
                deleteSql += $"DELETE FROM tab_publish_dashboards WHERE DB_CODE = '{item.Trim()}'";

                DataTable dataTable = await _dbUtility.ExecuteQuery(deleteSql);
                return dataTable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<DataTable> GetLayoutData(string selectedValue)
        {
            try
            {
                string sql = "select P_LAYOUT_ID as ID, LAYOUT_NAME + ' - ' + Revision as des from TAB_PUBLISH_LAYOUTS where LAYOUT_TYPE = '" + selectedValue + "'";
                DataTable dataTable = await _dbUtility.ExecuteQuery(sql);
                return dataTable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<DataTable> GetWidgetData(string selectedValue, int dashboardType)
        {
            try
            {
                string dtWidgetsSql = "select distinct L.P_LAYOUT_ID, layout_name, M.WIDGET_ID, WIDGET_NAME" +
                                      " from TAB_PUBLISH_LAYOUTS L join TAB_MAPPING_PUBLISH_LAYOUTS M on L.P_LAYOUT_ID = M.P_LAYOUT_ID " +
                                      " left join tab_gen_widgets W on W.WIDGET_ID = M.WIDGET_ID and L.LAYOUT_TYPE = '" + selectedValue + "'" +
                                      " and w.widget_type = " + dashboardType;
                DataTable dataTable = await _dbUtility.ExecuteQuery(dtWidgetsSql);
                return dataTable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<DataTable> GetMenuList()
        {
            try
            {
                string dtMenuSql = "select id, F_ID, MENU_NAME from BPM_MENU where C_AZD = 2";
                DataTable dataTable = await _dbUtility.ExecuteQuery(dtMenuSql);
                return dataTable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<DataTable> GetSelectedLayoutData(string layoutType, Int64 Id)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT t.p_layout_id AS id, ");
                sql.AppendLine("layout_name + ' - ' + revision AS des ");
                sql.AppendLine("FROM tab_publish_layouts t ");
                sql.AppendLine("INNER JOIN ascn_pub_layout_dashboards a ");
                sql.AppendLine("ON t.p_layout_id = a.p_layout_id ");
                sql.AppendLine("AND LAYOUT_TYPE = '" + layoutType + "' and db_id = " + Id);
                sql.AppendLine(" ORDER BY a.layout_seq ");
                DataTable dataTable = await _dbUtility.ExecuteQuery(sql.ToString());
                return dataTable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<DataRow> GetDataRowByID(Int64 ID)
        {
            try
            {
                string sql = $"SELECT UPPER(DB_TYPE), DB_CODE, DB_DESCRIPTION FROM TAB_PUBLISH_DASHBOARDS WHERE DB_ID = {ID}";
                var result = await _dbUtility.ExecuteQuery(sql);
                return  result.Rows[0] ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task RestAllAssociation(string dashboardId, string dashboardType)
        {
            try
            {
                string sql = "UPDATE grp_t019 SET " + dashboardType + "_DASHBOARD = -1 WHERE " + dashboardType + "_DASHBOARD = " + dashboardId;
                await _dbUtility.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task UpdateAssociationData(string dashboardId, string dashboardType, string c_grp)
        {
            try
            {
                string sql = $"UPDATE grp_t019 SET {dashboardType}_DASHBOARD = {dashboardId} WHERE c_grp = {c_grp}";
                await _dbUtility.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task DeleteFromGrpUserDashboard()
        {
            try
            {
                string deleteSql = "DELETE FROM ASCN_GROUP_USER_DASHBOARD";
                await _dbUtility.ExecuteQuery(deleteSql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task InsertIntoGrpUserDashboard()
        {
            try
            {
                string insertSql = "INSERT INTO ASCN_GROUP_USER_DASHBOARD " +
                                   "SELECT DISTINCT A.C_UTEN, A.GroupID, " +
                                   "CASE G.PHP_DASHBOARD WHEN NULL THEN 0 WHEN 0 THEN 0 WHEN -1 THEN 0 ELSE 1 END, " +
                                   "A.GroupID, " +
                                   "CASE G.HP_DASHBOARD WHEN NULL THEN 0 WHEN 0 THEN 0 WHEN -1 THEN 0 ELSE 1 END, " +
                                   "A.P_ELMNT, A.GroupID, " +
                                   "CASE G.SP_DASHBOARD WHEN NULL THEN 0 WHEN 0 THEN 0 WHEN -1 THEN 0 ELSE 1 END, " +
                                   "A.GroupID, " +
                                   "CASE G.PFP_DASHBOARD WHEN NULL THEN 0 WHEN 0 THEN 0 WHEN -1 THEN 0 ELSE 1 END, " +
                                   "A.GroupID, " +
                                   "CASE G.PRG_DASHBOARD WHEN NULL THEN 0 WHEN 0 THEN 0 WHEN -1 THEN 0 ELSE 1 END " +
                                   "FROM ASCN_Group_OBS A LEFT JOIN GRP_T019 G ON G.C_GRP = A.GroupID";
                await _dbUtility.ExecuteQuery(insertSql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task UpdateDescription(string Description, Int64 DashboardId)
        {
            try
            {
                string descriptionParam = await _dbUtility.QS(Description, true);
                string sql = $"UPDATE TAB_PUBLISH_DASHBOARDS SET [DB_DESCRIPTION] = {descriptionParam} WHERE DB_ID = {DashboardId}";
                await _dbUtility.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task DeleteLayoutDashboard(Int64 DashboardId)
        {
            try
            {
                string sql = $"DELETE FROM ASCN_PUB_LAYOUT_DASHBOARDS WHERE DB_ID = {DashboardId}";
                await _dbUtility.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task InsertIntoDashboard(Int64 DashboardId, string itm, int i)
        {
            try
            {
                string sql = $"INSERT INTO ASCN_PUB_LAYOUT_DASHBOARDS (DB_ID, P_LAYOUT_ID, LAYOUT_SEQ) VALUES ({DashboardId}, {itm}, {i})";
                await _dbUtility.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


    }
}