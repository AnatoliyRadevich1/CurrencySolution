using System;
using CurrencyTask.Interfaces;
using CurrencyTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTask.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<IActionResult> FilteredCurrencies() //если я добавлю к методу Async и внесу нужные изменения в других местах, то это не пропустит фреймворк
        {
            var filteredRates = await _currencyService.GetFilteredCurrencyRatesAsync();
            //по сути асинхронно возвращаются отфильтрованные валюты (см. метод GetFilteredCurrencyRatesAsync() класса CurrencyService)
            return View(filteredRates);//логика фильтрации будет на стороне КЛИЕНТА
        }
        public async Task<IActionResult> AllCurrencies()
        {
            var allRates = await _currencyService.GetAllCurrencyRatesAsync();
            return View(allRates);
        }

        [HttpGet]
        public async Task<IActionResult> ConvertCurrencies()
        {
            var allCurrencies = await _currencyService.GetAllCurrencyRatesAsync();
            var abbreviations = allCurrencies.Select(elem => elem.Cur_Abbreviation).ToList();
            return View(abbreviations);//правильнее делать логику на стороне КЛИЕНТА, но тут ради разнообразия сделаю логику на стороне СЕРВЕРА
        }

        [HttpPost]
        public async Task<IActionResult> ConvertCurrencies(string amountStr, string fromCurrency, string toCurrency, string toCurrency2)
        {
            var allCurrencies = await _currencyService.GetAllCurrencyRatesAsync();

            List<string> abbreviations; //объявляем одну переменную для списка валют, чтобы проработать исключения

            // Проверка на пустые поля
            if (string.IsNullOrEmpty(amountStr) || string.IsNullOrEmpty(fromCurrency) || string.IsNullOrEmpty(toCurrency) || string.IsNullOrEmpty(toCurrency2))
            {
                ViewBag.Error = "Пожалуйста, заполните все поля.";
                abbreviations = allCurrencies.Select(elem => elem.Cur_Abbreviation).ToList()!;
                return View(abbreviations);
            }

            // Проверка корректности числа
            if (decimal.TryParse(amountStr, out decimal amount) == false)//читаю как "если набранное пользователем число разряда decimal не преобразовалось в число"
            {
                ViewBag.Error = "Некорректное введённое значение.";
                abbreviations = allCurrencies.Select(elem => elem.Cur_Abbreviation).ToList()!;
                return View(abbreviations);
            }

            // Поиск валют
            var fromCurr = allCurrencies.FirstOrDefault(elem => elem.Cur_Abbreviation == fromCurrency);
            var toCurr = allCurrencies.FirstOrDefault(elem => elem.Cur_Abbreviation == toCurrency);
            var toCurr2 = allCurrencies.FirstOrDefault(elem => elem.Cur_Abbreviation == toCurrency2);

            if (fromCurr == null || toCurr == null)
            {
                ViewBag.Error = "Одна из валют не найдена.";
                abbreviations = allCurrencies.Select(elem => elem.Cur_Abbreviation).ToList()!;
                return View(abbreviations);
            }

            decimal rateFrom = fromCurr.Cur_OfficialRate / fromCurr.Cur_Scale; //приведение величины исходной валюты к единице

            decimal rateTo1 = toCurr.Cur_OfficialRate / toCurr.Cur_Scale; //приведение величины целевой валюты №1 к единице
            decimal rateTo2 = toCurr2!.Cur_OfficialRate / toCurr2.Cur_Scale; //приведение величины целевой валюты №2 к единице

            decimal amountInBase = amount * rateFrom;//считаем количество исходной валюты 

            decimal resultAmount = amountInBase / rateTo1;//конвериуем в валюту №1 (делим количесво базовой валюты на курс валюы №1)
            decimal resultAmount2 = amountInBase / rateTo2;//конвериуем в валюту №2 (делим количесво базовой валюты на курс валюы №20

            // Собираем числа для передачи результата во View()
            ViewBag.OriginalAmount = amount;
            ViewBag.FromCurrency = fromCurrency;
            ViewBag.ToCurrency = toCurrency;
            ViewBag.ToCurrency2 = toCurrency2;
            ViewBag.ResultAmount1 = resultAmount;
            ViewBag.ResultAmount2 = resultAmount2;

            // Передача списка валют для View
            abbreviations = allCurrencies.Select(elem => elem.Cur_Abbreviation).ToList()!;
            return View(abbreviations);
        }
    }
}
