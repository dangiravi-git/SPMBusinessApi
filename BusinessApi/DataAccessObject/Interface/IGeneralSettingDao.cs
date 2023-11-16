using BusinessApi.Models;
using System.Data;

namespace BusinessApi.DataAccessObject.Interface
{
    public interface IGeneralSettingDao
    {
        Task<DataTable> GetCountries();   
        Task<DataTable> GetCurrencies();  
        Task<DataTable> GetHFS();
    }
}
