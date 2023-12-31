﻿using BusinessApi.Models;
using System.Data;

namespace BusinessApi.DataAccessObject.Interface
{
    public interface IDashBoardRegisterDao
    {
        Task<DataTable> GetProjects();
        Task<int> CreateNewDashboard(string? code, string? type, string? description, double createdBy, string isWf);
        Task CreateLayoutAssociationWithDashboard(int dashboardId, int layoutId, int layoutSeq);
        Task<DataTable> IsDashboardCodeAlreadyExists(string code);
        Task<DataTable> GetBindData(string dashboardId, string dashboardType);
        Task<DataTable> GetBindAvailableGroupDataWithId(string dashboardId, string dashboardType,string funzlPermission);
        Task<DataTable> GetBindAvailableGroupDataWithType(string dashboardType,string funzlPermission);
        Task<DataRow> GetTypeVal(string item);
        Task<object> GetGroupVal(string colName, object v);
        Task updatethedata(string groups, string checkColName, string colName, string grpColName);
        Task<DataTable> deletethedata(object v, string item);
        Task<DataTable> GetLayoutData(string selectedValue);
        Task<DataTable> GetWidgetData(string selectedValue, int dashboardType);
        Task<DataTable> GetMenuList();
    }
}
