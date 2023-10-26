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
        private IDashBoardRegisterRepository _dashBoardRegisterRepository;
        public DashBoardRegisterController(IDashBoardRegisterRepository dashBoardRegisterRepository)
        {
            _dashBoardRegisterRepository = dashBoardRegisterRepository;

        }
        [HttpGet]
        public ActionResult GetDashBoardRegisterList([FromBody] DashBoardRegisterModel parameter)
        {
            ApiResponse response;
            try
            {
                DashBoardRegisterModel dataList = _dashBoardRegisterRepository.GetProject(parameter);
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
