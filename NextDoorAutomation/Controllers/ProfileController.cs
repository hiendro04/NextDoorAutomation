using Microsoft.AspNetCore.Mvc;

namespace NextDoorAutomation.Controllers
{
    public class ProfileController : Controller
    {
        public ProfileController() { }
        public async Task<IActionResult> Index()
        {

            var test = new Test();
            await test.GetData();
            Console.WriteLine("---");
            return View();
        }
    }
}
