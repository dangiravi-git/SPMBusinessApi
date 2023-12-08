using Microsoft.AspNetCore.Mvc;
using Authentication.Utils.Response;
using Authentication.Repositories.Interface;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _repository;
        private readonly ILogger<AuthenticationController> _logger;
        public AuthenticationController(IAuthenticationRepository repository, ILogger<AuthenticationController> logger)
        {
            try
            {
                _logger = logger;
                _repository = repository;
                _logger.LogInformation($"Initiate {nameof(AuthenticationController)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        [HttpGet("GetAuthenticate")]
        public async Task<ActionResult<ApiResponse>> GetAuthenticate(string key)
        {
            ApiResponse response;
            try
            {
                if(key != "" && key != null)
                {
                    response = await _repository.AuthenticateUser(key);
                }
                else
                {
                    response = new ApiResponse
                    {
                        IsSuccess = false,
                        Message = "Fail to access key"
                    };
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Error while authenticating"
                };
                return Ok(response);
            }
            
        }
    }
}
