using Microsoft.AspNetCore.Mvc;
using Authentication.Response;


namespace Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        [HttpGet("GetAuthenticate")]
        public ActionResult<bool> GetAuthenticate(string key)
        {
            ApiResponse response;
            try
            {
                if (key == "abc")
                {
                    response = new ApiResponse
                    {
                        IsSuccess = true,
                        Message = "Authenticated"

                    };
                }
                else
                {
                    response = new ApiResponse
                    {
                        IsSuccess = false,
                        Message = "Not Authenticated"
                    };
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Error in Authenticated"
                };
                return Ok(response);
            }
            
        }
    }
}
