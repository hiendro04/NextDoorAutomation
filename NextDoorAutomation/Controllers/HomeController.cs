using Business.Business;
using Microsoft.AspNetCore.Mvc;
using NextDoorAutomation.Models;
using System.Diagnostics;

namespace NextDoorAutomation.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() { }

        public async Task<IActionResult> Index()
        {
            //BusinessTool.GetInstance().GetData();
            return View();
        }
    }
}
