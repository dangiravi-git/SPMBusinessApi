using Authentication.Models;
using System.Data;

namespace Authentication.DataAccessObject.Interface
{
    public interface IAuthenticationDao
    {
        Task<bool> IsUserAuthentic(string Key);
    }
}
