﻿namespace BusinessApi.Models
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
        public int IsAvailable { get; set; }
        public List<WidgetDto> Widgets { get; set; }

    }
    public class WidgetDto
    {
        public int WidgetId { get; set; }
        public string WidgetName { get; set; }

    } 

}
