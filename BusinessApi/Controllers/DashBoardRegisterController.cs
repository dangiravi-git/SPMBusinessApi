using Azure;
using BusinessApi.Models;
using BusinessApi.Repositories.Interface;
using BusinessApi.Utils.Response;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;

namespace BusinessApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashBoardRegisterController : ControllerBase
    {
        private readonly IDashBoardRegisterRepository _repository;
        public DashBoardRegisterController(IDashBoardRegisterRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<DashBoardRegisterModel>>> GetDashBoardRegister()
        {
            ApiResponse<List<DashBoardRegisterViewTypeViewModel>> response;
            try
            {
                var dataList = await _repository.GetProject();
                response = new ApiResponse<List<DashBoardRegisterViewTypeViewModel>>
                {
                    IsSuccess = true,
                    Message = "",
                    Item  = dataList
                };
            }
            catch(Exception ex)
            {
                response = new ApiResponse<List<DashBoardRegisterViewTypeViewModel>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Item = new List<DashBoardRegisterViewTypeViewModel>()
                };
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse<DashboardDto>>> CreateNewDashboard([FromBody] DashboardDto dashboardDto)
        {
            ApiResponse<DashboardDto> response;
            try
            {
                var dataList = await _repository.CreateNewDashboard(dashboardDto);
                response = new ApiResponse<DashboardDto>
                {
                    IsSuccess = true,
                    Message = "",
                    Item = dataList
                };
            }
            catch (Exception ex)
            {
                response = new ApiResponse<DashboardDto>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Item = new DashboardDto()
                };
            }
            return Ok(response);
        }

        [HttpGet("BindSelectedRolesDataPopup")]
        public async Task<ActionResult<ApiResponse<DashboardTypeModel>>> BindSelectedRolesDataPopup(string dashboardId, string dashboardType)
        {
            ApiResponse<List<DashboardTypeModel>> response;
            try
            {
                var type = _repository.SetDashboardType(dashboardType);
                var dataList = await _repository.GetBindData(dashboardId, type);
                response = new ApiResponse<List<DashboardTypeModel>>
                {
                    IsSuccess = true,
                    Message = "",
                    Item = dataList
                };
            }
            catch (Exception ex)
            {
                response = new ApiResponse<List<DashboardTypeModel>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Item = new List<DashboardTypeModel>()
                };
            }
            return Ok(response);
        }
       
    }
}
