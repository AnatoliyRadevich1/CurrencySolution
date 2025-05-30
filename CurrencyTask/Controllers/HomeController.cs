using System.Diagnostics;
using CurrencyTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        [Route("/StatusCodeError/{enteredStatusCode}")]
        public IActionResult Error(int enteredStatusCode)
        {
            if (enteredStatusCode == 404)
            {
                ViewBag.AnyErrorMessage = "������ 404. �� ���������� ����� �� �������������� ��������";
                return View();
            }
            else
            {
                ViewBag.AnyErrorMessage = $"������ {enteredStatusCode}. �� ���������� ����� �� �������������� ��������";
                return View();
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
