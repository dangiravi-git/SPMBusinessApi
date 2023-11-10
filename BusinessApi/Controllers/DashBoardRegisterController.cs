using Azure;
using BusinessApi.Models;
using BusinessApi.Repositories.Interface;
using BusinessApi.Utils.Response;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;
using Serilog;
//using ILogger = Serilog.ILogger;

namespace BusinessApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashBoardRegisterController : ControllerBase
    {
        private readonly IDashBoardRegisterRepository _repository;
        private readonly ILogger<DashBoardRegisterController> _logger;
        public DashBoardRegisterController(IDashBoardRegisterRepository repository, ILogger<DashBoardRegisterController> logger)
        {
            try
            {
                _logger = logger;
                _repository = repository;
                _logger.LogInformation($"Initiate {nameof(DashBoardRegisterController)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

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
                    Item = dataList
                };
            }
            catch (Exception ex)
            {
                response = new ApiResponse<List<DashBoardRegisterViewTypeViewModel>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Item = new List<DashBoardRegisterViewTypeViewModel>()
                };
                _logger.LogError(ex.Message);
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
                _logger.LogError(ex.Message);
            }
            return Ok(response);
        }

        [HttpGet("SelectedRoles")]
        public async Task<ActionResult<ApiResponse<DashboardTypeModel>>> SelectedRoles(string dashboardId, string dashboardType)
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
                _logger.LogError(ex.Message);
            }
            return Ok(response);
        }
        [HttpGet("AvailableRoles")]
        public async Task<ActionResult<ApiResponse<DashboardTypeModel>>> AvailableRoles(string dashboardId, string dashboardType, string typename, string typeValues, string reTransferValue, string checkIsTransferButtonClick)
        {
            ApiResponse<List<DashboardTypeModel>> response;
            try
            {
                var type = _repository.SetDashboardType(dashboardType);
                var dataList = await _repository.GetBindAvailableGroupData(dashboardId, type, typename, typeValues, reTransferValue, checkIsTransferButtonClick);
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
                _logger.LogError(ex.Message);
            }
            return Ok(response);
        }
        [HttpDelete("DeleteRecords")]
        public async Task<IActionResult> DeleteRecords(string val)
        {
            ApiResponse<string> response;
            try
            {
                string resultMessage = await _repository.DeleteRecords(val);
                response = new ApiResponse<string>
                {
                    IsSuccess = true,
                    Message = "",
                    Item = resultMessage
                };
            }
            catch (Exception ex)
            {
                response = new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Item = null
                };
                _logger.LogError(ex.Message);
            }
            return Ok(response);
        }


        [HttpGet("GetLayoutsWidgetAssociation")]
        public async Task<ActionResult<ApiResponse<DashboardLayoutDto>>> GetLayoutsWidgetAssociation(string SelectedDasboardType)
        {
            ApiResponse<List<DashboardLayoutDto>> response;
            try
            {
                List<DashboardLayoutDto> dataList = await _repository.GetLayoutsWidgetAssociation(SelectedDasboardType);
                response = new ApiResponse<List<DashboardLayoutDto>>
                {
                    IsSuccess = true,
                    Message = "",
                    Item = dataList
                };
            }
            catch (Exception ex)
            {
                response = new ApiResponse<List<DashboardLayoutDto>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Item = new List<DashboardLayoutDto>()
                };
                _logger.LogError(ex.Message);
            }
            return Ok(response);
        }
        [HttpPut("EditLayoutsWidgetAssociation")]
        public async Task<ActionResult<ApiResponse<Dashboardeditdata>>> EditLayoutsWidgetAssociation(Int64 Id)
        {
            ApiResponse<List<Dashboardeditdata>> response;
            try
            {
                List<Dashboardeditdata> dataList = await _repository.EditLayoutsWidgetAssociation(Id);
                response = new ApiResponse<List<Dashboardeditdata>>
                {
                    IsSuccess = true,
                    Message = "",
                    Item = dataList,
                };
            }
            catch (Exception ex)
            {
                response = new ApiResponse<List<Dashboardeditdata>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Item = new List<Dashboardeditdata>(),
                };
                _logger.LogError(ex.Message);
            }
            return Ok(response);
        }

        [HttpPost("SaveDashboardAssociation")]
        public async Task<IActionResult> SaveDashboardAssociation([FromBody] Dashboardassociatedata dashboardDto)
        {
            ApiResponse<string> response;
            try
            {
                string resultMessage = await _repository.SaveDashboardAssociationData(dashboardDto.selectedval, dashboardDto.dashboardId, dashboardDto.dashboardType);
                response = new ApiResponse<string>
                {
                    IsSuccess = true,
                    Message = "",
                    Item = resultMessage
                };
            }
            catch (Exception ex)
            {
                response = new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Item = null
                };
                _logger.LogError(ex.Message);
            }
            return Ok(response);
        }

        [HttpPut("UpdateDashboardDataByID")]
        public async Task<IActionResult> UpdateDashboardDataByID([FromBody] SaveDashboard dashboardDto)
        {
            ApiResponse<string> response;
            try
            {
                string resultMessage = await _repository.UpdateDashboardData(dashboardDto.DashboardId, dashboardDto.DashboardLayoutAssoList, dashboardDto.Description);
                response = new ApiResponse<string>
                {
                    IsSuccess = true,
                    Message = "",
                    Item = resultMessage
                };
            }
            catch (Exception ex)
            {
                response = new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Item = null
                };
                _logger.LogError(ex.Message);
            }
            return Ok(response);
        }
    }
}
