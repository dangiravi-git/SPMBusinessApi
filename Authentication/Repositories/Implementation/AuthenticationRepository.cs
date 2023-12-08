using Authentication.DataAccessObject.Implementation;
using Authentication.DataAccessObject.Interface;
using Authentication.Models;
using Authentication.Repositories.Interface;
using Authentication.Utils;
using Authentication.Utils.Response;
using Azure;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Resources;
using System.Text;

namespace Authentication.Repositories.Implementation
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private IAuthenticationDao _authenticationDao;
        private readonly IDbUtility _dbUtility;
        private readonly ILogger<AuthenticationRepository> _logger;
        public AuthenticationRepository(IAuthenticationDao authenticationDao, ILogger<AuthenticationRepository> logger)
        {
            try
            {
                _logger = logger;
                _authenticationDao = authenticationDao;
                //_logger.LogInformation($"Initiate {nameof(AuthenticationRepository)}");
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                throw;
            }

        }
        public async Task<ApiResponse> AuthenticateUser(string Key)
        {
            ApiResponse response;
            try
            {
                var isAuthenticUser = await _authenticationDao.IsUserAuthentic(Key);
                if (isAuthenticUser)
                {
                    response = new ApiResponse
                    {
                        IsSuccess = true,
                        Message = "Authenticated User"
                    };
                }
                else
                {
                    response = new ApiResponse
                    {
                        IsSuccess = false,
                        Message = "Unauthenticated User"
                    };
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
