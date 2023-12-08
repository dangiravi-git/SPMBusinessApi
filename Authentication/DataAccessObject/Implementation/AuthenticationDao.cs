using Authentication.DataAccessObject.Interface;
using Authentication.Models;
using Authentication.Utils;
using Microsoft.Identity.Client;
using System.Data;
using System.Text;

namespace Authentication.DataAccessObject.Implementation
{
    public class AuthenticationDao : IAuthenticationDao
    {
        private readonly IDbUtility _dbUtility;
        private readonly ILogger<AuthenticationDao> _logger;
        public AuthenticationDao(IDbUtility dbUtility, ILogger<AuthenticationDao> logger)
        {
            try
            {
                _dbUtility = dbUtility;
                _logger = logger;
                _logger.LogInformation($"Initiate {nameof(AuthenticationDao)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
          
        }
        public async Task<bool> IsUserAuthentic(string key)
        {
            try
            {
                string sql = "select count(1) as auth from SpmAngularUserAuthontication where RequestToken = '" + key + "' and IsActive = 1";
                var result = await _dbUtility.QueryValue(sql);
                if( result != null && Convert.ToInt32(result) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}