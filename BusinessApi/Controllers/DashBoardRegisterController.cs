using BusinessApi.Models;
using BusinessApi.Repositories.Interface;
using BusinessApi.Utils.Response;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<DashBoardRegisterModel>> GetDashBoardRegister()
        {
            ApiResponse response;
            try
            {
                var dataList = await _repository.GetProject();
                response = new ApiResponse
                {
                    success = true,
                    message = "",
                    data = dataList
                };
            }
            catch
            {
                response = new ApiResponse
                {
                    success = false,
                    message = "",
                    data = new DashBoardRegisterModel()
                };
            }
            return Ok(response);
        }
    }
}
