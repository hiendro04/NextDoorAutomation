using Microsoft.AspNetCore.Mvc;

namespace NextDoorAutomation.Controllers
{
    public class GologinController : Controller
    {
        public GologinController() { }
        public async Task<IActionResult> Index()
        {

            var test = new Test();
            await test.GetData();
            Console.WriteLine("---");
            return View();
        }
    }
}
