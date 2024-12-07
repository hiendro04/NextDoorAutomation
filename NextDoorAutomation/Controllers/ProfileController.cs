using Microsoft.AspNetCore.Mvc;

namespace NextDoorAutomation.Controllers
{
    public class ProfileController : Controller
    {
        public ProfileController() { }
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
