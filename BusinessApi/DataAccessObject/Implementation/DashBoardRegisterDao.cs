using BusinessApi.DataAccessObject.Interface;
using BusinessApi.Models;
using BusinessApi.Utils;
using System.Data;

namespace BusinessApi.DataAccessObject.Implementation
{
    public class DashBoardRegisterDao : IDashBoardRegisterDao
    {
        private readonly IDbUtility _dbUtility;
        public DashBoardRegisterDao(IDbUtility dbUtility)
        {
            _dbUtility = dbUtility;
        }
        public async Task<DashBoardRegisterModel> GetProjects()
        {
            List<DashBoardRegisterViewTypeViewModel> modelProjectListViewType = new();
            var sqlSelect = "Select DB_ID, DB_CODE, DB_DESCRIPTION, case when CreatedBy= 80125112841950 then 'Global System Administrator' end as CreatedBy , DB_CREATION_DATE, CASE WHEN db_type='P' THEN 'Initiative Dashboard' WHEN db_type='M' THEN 'Program Dashboard'  WHEN db_type='H' THEN 'Analytics Dashboard'  WHEN db_type='S' THEN 'Strategy Dashboard' WHEN db_type='F' THEN 'Portfolio Dashboard' ELSE ''  END AS  db_type, IS_WF from tab_publish_dashboards db_type order by db_creation_date desc,DB_CODE";
            DashBoardRegisterModel model = new();
            var dashbords = await _dbUtility.ExecuteQuery(sqlSelect);
            if (dashbords != null)
            {
                foreach (DataRow row in dashbords.Rows)
                {
                    var viewModel = new DashBoardRegisterViewTypeViewModel
                    {
                        DB_ID = row["DB_ID"].ToString(),
                        DB_CODE = row["DB_CODE"].ToString(),
                        DB_DESCRIPTION = row["DB_DESCRIPTION"].ToString(),
                        CreatedBy = row["CreatedBy"].ToString(),
                        DB_CREATION_DATE = row["DB_CREATION_DATE"].ToString(),
                        db_type = row["db_type"].ToString(),
                        IS_WF = row["IS_WF"].ToString()
                    };
                    modelProjectListViewType.Add(viewModel);
                }
            }
            model.DashBoardRegisterViewType = modelProjectListViewType;
            return model;
        }
    }
}
