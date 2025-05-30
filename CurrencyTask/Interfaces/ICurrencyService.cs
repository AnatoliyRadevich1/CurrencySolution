using CurrencyTask.Models;

namespace CurrencyTask.Interfaces
{
        public interface ICurrencyService
        {
            Task<List<CurrencyModel>> GetFilteredCurrencyRatesAsync();
            Task<List<CurrencyModel>> GetAllCurrencyRatesAsync();
        }
}
