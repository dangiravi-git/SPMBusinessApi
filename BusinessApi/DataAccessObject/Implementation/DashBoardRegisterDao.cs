using BusinessApi.DataAccessObject.Interface;
using BusinessApi.Models;

namespace BusinessApi.DataAccessObject.Implementation
{
    public class DashBoardRegisterDao : IDashBoardRegisterDao<DashBoardRegisterModel, int>
    {
        public DashBoardRegisterModel getProjects(DashBoardRegisterModel parameter)
        {

            DashBoardRegisterModel model = new DashBoardRegisterModel();
            
            List<DashBoardRegisterViewTypeViewModel> modelProjectListViewType = new List<DashBoardRegisterViewTypeViewModel>();
            modelProjectListViewType.Add(new DashBoardRegisterViewTypeViewModel { DB_ID = "", DB_CODE = "", DB_DESCRIPTION = "", CreatedBy = "", DB_CREATION_DATE = "", db_type = "", IS_WF = "" });
            model.DashBoardRegisterViewType = modelProjectListViewType;
            return model;
        }
    }
}
