using Microsoft.AspNetCore.Mvc;
using NextDoorAutomation.Models;
using System.Diagnostics;

namespace NextDoorAutomation.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ILogger<HomeController> logger)
        {
        }

        public async Task<IActionResult> Index()
        {

            var test = new Test();
            await test.GetData();
            Console.WriteLine("---");
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
