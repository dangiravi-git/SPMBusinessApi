using BusinessApi.Controllers;
using BusinessApi.DataAccessObject.Interface;
using BusinessApi.Models;
using BusinessApi.Repositories.Interface;
using BusinessApi.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Resources;
using System.Text;


namespace BusinessApi.Repositories.Implementation
{
    public class GeneralSettingRepository : IGeneralSettingRepository

    {
        private IGeneralSettingDao _projectListDao;
        private readonly IDbUtility _dbUtility;
        private readonly ILogger<GeneralSettingRepository> _logger;

        public GeneralSettingRepository(IGeneralSettingDao projectListDao, ILogger<GeneralSettingRepository> logger)
        {
            try
            {
                _logger = logger;
                _projectListDao = projectListDao;
                _logger.LogInformation($"Initiate {nameof(GeneralSettingRepository)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }
        public async Task<List<GeneralSettingModel>> GetDropDownList()
        {
            try
            {

                DataTable countryTable = await _projectListDao.GetCountries();
                DataTable currencyTable = await _projectListDao.GetCurrencies();
                DataTable hfsTable = await _projectListDao.GetHFS();
                GeneralSettingModel generalSettings = new GeneralSettingModel
                {
                    CountryList = MapDataTableToList<Country>(countryTable, "Code", "Name", "Country"),
                    CurrencyList = MapDataTableToList<Currency>(currencyTable, "Code", "Name", "Currency"),
                    HFSList = MapDataTableToList<HFS>(hfsTable, "Code", "Name", "HFS")
                };
                List<GeneralSettingModel> result = new List<GeneralSettingModel> { generalSettings };
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        private List<T> MapDataTableToList<T>(DataTable table, string codeColumnName, string nameColumnName, string tableName) where T : new()
        {
            try
            {
                List<T> result = new List<T>();

                foreach (DataRow row in table.Rows)
                {
                    T item = new T();
                    var codeProperty = typeof(T).GetProperty("Code");
                    var nameProperty = typeof(T).GetProperty("Name");

                    foreach (DataColumn column in table.Columns)
                    {
                        string propertyName = column.ColumnName;
                        var property = typeof(T).GetProperty(propertyName);
                        if (property != null)
                        {
                            property.SetValue(item, row[column], null);
                            if (propertyName == codeColumnName && codeProperty != null)
                            {
                                codeProperty.SetValue(item, row[column], null);
                            }
                            else if (propertyName == nameColumnName && nameProperty != null)
                            {
                                nameProperty.SetValue(item, row[column], null);
                            }
                        }
                    }
                    var tableNameProperty = typeof(T).GetProperty("TableName");
                    if (tableNameProperty != null)
                    {
                        tableNameProperty.SetValue(item, tableName, null);
                    }
                    result.Add(item);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

    }
}
