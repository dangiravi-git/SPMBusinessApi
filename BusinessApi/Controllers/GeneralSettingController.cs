using BusinessApi.Models;
using BusinessApi.Repositories.Interface;
using BusinessApi.Utils.Response;
using Microsoft.AspNetCore.Mvc;

namespace BusinessApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeneralSettingController : ControllerBase
    {
        private readonly IGeneralSettingRepository _repository;
        private readonly ILogger<GeneralSettingController> _logger;
        public GeneralSettingController(IGeneralSettingRepository repository, ILogger<GeneralSettingController> logger)
        {
            try
            {
                _logger = logger;
                _repository = repository;
                _logger.LogInformation($"Initiate {nameof(GeneralSettingController)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }
        [HttpGet("GetAllDropDownList")]
        public async Task<ActionResult<ApiResponse<GeneralSettingModel>>> GetAllDropDownList()
        {
            ApiResponse<List<GeneralSettingModel>> response;
            try
            {
                var dataList = await _repository.GetDropDownList();
                response = new ApiResponse<List<GeneralSettingModel>>
                {
                    IsSuccess = true,
                    Message = "",
                    Item = dataList
                };
            }
            catch (Exception ex)
            {
                response = new ApiResponse<List<GeneralSettingModel>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Item = new List<GeneralSettingModel>()
                };
                _logger.LogError(ex.Message);
            }
            return Ok(response);
        }
    }
}
