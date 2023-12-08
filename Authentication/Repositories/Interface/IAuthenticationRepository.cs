using Authentication.Utils.Response;
using System.Data;

namespace Authentication.Repositories.Interface
{
    public interface IAuthenticationRepository
    {
        Task<ApiResponse> AuthenticateUser(string Key);
    }
}
