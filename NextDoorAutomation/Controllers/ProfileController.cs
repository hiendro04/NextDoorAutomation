using Microsoft.AspNetCore.Mvc;

namespace NextDoorAutomation.Controllers
{
    public class ProfileController : Controller
    {
        public ProfileController() { }
        public async Task<IActionResult> Index()
        {
            //var state = new StateInfo();
            //state.ReferenceLink = "https://nextdoor.com/find-neighborhood/va/";
            //state.Name = "Virginia";
            //state.Acronym = "VA";
            //StateDao.GetInstance().Insert(state);
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetDataNeighborhood()
        {
            var data = new List<string>();
            return Json(new
            {
                Data = data,
            });
        }
    }
}
