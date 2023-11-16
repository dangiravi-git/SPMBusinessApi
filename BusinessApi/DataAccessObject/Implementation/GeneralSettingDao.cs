using BusinessApi.DataAccessObject.Interface;
using BusinessApi.Models;
using BusinessApi.Utils;
using System.Data;
using System.Text;


namespace BusinessApi.DataAccessObject.Implementation
{
    public class GeneralSettingDao: IGeneralSettingDao
    {
        private readonly IDbUtility _dbUtility;
        private readonly ILogger<GeneralSettingDao> _logger;
        public GeneralSettingDao(IDbUtility dbUtility, ILogger<GeneralSettingDao> logger)
        {
            try
            {
                _dbUtility = dbUtility;
                _logger = logger;
                _logger.LogInformation($"Initiate {nameof(GeneralSettingDao)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }
        public async Task<DataTable> GetCountries()
        {
            try
            {
                var sqlSelect = "select  C_NAZIONE as Code , C_NAZIONE + '-' + S_NAZIONE as [Name] from NAZIONE_T104 where C_AZD = 2 order by S_NAZIONE;";
                var Countries = await _dbUtility.ExecuteQuery(sqlSelect);
                return Countries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }
        public async Task<DataTable> GetCurrencies()
        {
            try
            {
                var sqlSelect = "SELECT C_VL as Code, S_VL as Name  FROM VL_T011 WHERE C_AZD = 2 ORDER BY C_VL";
                var Currencies = await _dbUtility.ExecuteQuery(sqlSelect);
                return Currencies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }
        public async Task<DataTable> GetHFS()
        {
            try
            {
                var sqlSelect = "SELECT C_STRU as Code,C_STRU +' - '+ S_STRU as Name FROM DEFN_STRU_SIST_T032 WHERE T_STRU = 'D' AND C_AZD = 2 ORDER BY S_STRU;";
                var HFS = await _dbUtility.ExecuteQuery(sqlSelect);
                return HFS;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

    }
}
