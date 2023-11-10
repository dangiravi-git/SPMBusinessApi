using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpGet("GetAuthenticate")]
        public async Task<ActionResult<bool>> GetAuthenticate(string key)
        {
            if (key == "abc")
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }
    }
}
