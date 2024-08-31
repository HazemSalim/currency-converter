using System.Threading.Tasks;

namespace CurrencyConverterAPI.Services
{
    public interface ICurrencyService
    {
        Task<string> GetLatestRates(string baseCurrency);
        Task<string> GetHistoricalRates(string baseCurrency, string startDate, string endDate);
        Task<string> ConvertCurrency(string from, string to, decimal amount);
    }
}
