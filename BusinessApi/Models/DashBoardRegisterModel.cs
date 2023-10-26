namespace BusinessApi.Models
{
    public class DashBoardRegisterModel
    {
        public List<DashBoardRegisterViewTypeViewModel> DashBoardRegisterViewType { get; set; } = new List<DashBoardRegisterViewTypeViewModel>();
    }
    public class DashBoardRegisterViewTypeViewModel
    {
        public string DB_ID { get; set; }
        public string DB_CODE { get; set; }
        public string DB_DESCRIPTION { get; set; }
        public string CreatedBy { get; set; }
        public string DB_CREATION_DATE { get; set; }
        public string db_type { get; set; }
        public string IS_WF { get; set; }
    }
}
