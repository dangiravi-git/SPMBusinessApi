namespace BusinessApi.Models
{
    public class DashBoardRegisterModel
    {
        public List<DashBoardRegisterViewTypeViewModel> DashBoardRegisterViewType { get; set; } = new List<DashBoardRegisterViewTypeViewModel>();
    }
    public class DashBoardRegisterViewTypeViewModel : DashboardDto
    {
        public string? DashboardCreatedBy { get; set; }
    }

    public class DashboardDto : BaseDto 
    {
        public int DashboardId { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? DashboardType { get; set; }
        public string? IsWf { get; set; }
        public List<DashboardLayoutAssoDto>? DashboardLayoutAssoList { get; set; } 
    }
    public class DashboardLayoutAssoDto
    {
        public int LayoutId { get; set; }
        public int LayoutSeq { get; set; }

    }
    public class DashboardTypeModel
    {
        public string? DashBoardType { get; set; }
        public string? Value { get; set; }
        public string? Name { get; set; }
    }

        public class DashboardLayoutDto
        {
            public int LayoutId { get; set; }
            public string LayoutName { get; set; }
            public Int64 IsAvailable { get; set; }
            public string layoutType { get; set; }
            public List<WidgetDto> Widgets { get; set; }

        }
        public class WidgetDto
        {
            public int WidgetId { get; set; }
            public string WidgetName { get; set; }

        }
    public class Dashboardassociatedata 
    {
        public string dashboardType { get; set; }
        public string selectedval { get; set; }
        public string dashboardId { get; set; }

    }
    public class Dashboardeditdata
    {
        public string dashboard_code { get; set; }
        public string description { get; set; }
        public string dashboard_type { get; set; }
        public Int64 Id { get; set; }                   

        public List<DashboardLayoutDto> data { get; set; }
    }

    public class Savedashboardthroughid {
        public Int64 DashboardId { get; set; }
        public List<MutipleIds> Values { get; set; }
        public string Description { get; set; }
    }
    public class MutipleIds {
        public Int64 Id { get; set; }
    }

}
