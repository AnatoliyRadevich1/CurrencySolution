using System.Text.Json;
using CurrencyTask.Interfaces;
using CurrencyTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTask.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CurrencyModel>> GetFilteredCurrencyRatesAsync()
        {
            var response = await _httpClient.GetStringAsync("https://api.nbrb.by/exrates/rates?periodicity=0");
            var allCurrencies = JsonSerializer.Deserialize<List<CurrencyModel>>(response)!;
            var filteredCurrencies = allCurrencies.Where(elem => elem.Cur_Abbreviation == "USD" || elem.Cur_Abbreviation == "EUR" || elem.Cur_Abbreviation == "RUB").ToList();
            
            return filteredCurrencies;
        }

        public async Task<List<CurrencyModel>> GetAllCurrencyRatesAsync()
        {
            var response = await _httpClient.GetStringAsync("https://api.nbrb.by/exrates/rates?periodicity=0");
            var allCurrencies = JsonSerializer.Deserialize<List<CurrencyModel>>(response)!;

            return allCurrencies;
        }
    }
}
