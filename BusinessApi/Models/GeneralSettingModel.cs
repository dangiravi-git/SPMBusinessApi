using System.Diagnostics.Metrics;

namespace BusinessApi.Models
{
    public class GeneralSettingModel
    {
         public List<Country> CountryList { get; set; } = new List<Country>();
        public List<Currency> CurrencyList { get; set; } = new List<Currency>();
        public List<HFS> HFSList { get; set; } = new List<HFS>();

    }
    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }

    }
    public class Currency
    {
        public string Code { get; set; }
        public string Name { get; set; }

    }
    public class HFS
    {
        public string Code { get; set; }
        public string Name { get; set; }

    }

}
