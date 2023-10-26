using BusinessApi.Repositories.Interface;
using BusinessApi.Utils.Response;
using System.Web.Http;

namespace BusinessApi.Controllers
{
    public class DashBoardRegisterController : ApiController
    {
        private IDashBoardRegisterRepository _dashBoardRegisterRepository;
        public DashBoardRegisterController(IDashBoardRegisterRepository dashBoardRegisterRepository)
        {
            _dashBoardRegisterRepository = dashBoardRegisterRepository;

        }
        [System.Web.Http.HttpPost]
        public IHttpActionResult GetDashBoardRegisterList()
        {
            ApiResponse response;
            response = new ApiResponse
            {
                success = true,
                message = "",
                data = ""
            };
            return Ok(response);
        }
    }
}
