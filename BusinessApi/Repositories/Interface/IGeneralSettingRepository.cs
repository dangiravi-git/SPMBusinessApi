
using BusinessApi.Models;

namespace BusinessApi.Repositories.Interface
{
    public interface IGeneralSettingRepository
    {
        Task<List<GeneralSettingModel>> GetDropDownList();
    }
}
