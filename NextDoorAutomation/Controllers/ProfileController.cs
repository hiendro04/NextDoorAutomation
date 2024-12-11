using Business.Dao;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

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
        public async Task<JsonResult> GetDataCity()
        {
            try
            {
                var stateInfo = StateDao.GetInstance().GetAll().FirstOrDefault();
                var options = new ChromeOptions();
                string profilePath = @"D:\ChromeProfile\UserData4";
                options.AddArgument($"--user-data-dir={profilePath}");
                using (var driver = new ChromeDriver(options))
                {
                    driver.Navigate().GoToUrl(stateInfo.ReferenceLink);

                    await Task.Delay(5000);

                    // Tìm tất cả thẻ <a> trong div.child_links
                    var links = driver.FindElements(By.CssSelector("div#child_links a"));
                    var cityList = new List<CityInfo>();
                    var cityCount = 0;
                    foreach(var link in links)
                    {
                        try
                        {
                            var cityInfo = new CityInfo();
                            cityInfo.Name = link.Text;
                            cityInfo.ReferenceLink = link.GetAttribute("href");
                            cityInfo.StateId = stateInfo._id;
                            cityList.Add(cityInfo);
                            cityCount++;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    stateInfo.CityCount = cityCount;
                    CityDao.GetInstance().InsertRange(cityList);
                    StateDao.GetInstance().Replace(stateInfo);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return Json(new
            {
            });
        }

        [HttpPost]
        public async Task<JsonResult> GetDataNeighborhood()
        {
            try
            {
                var stateInfo = StateDao.GetInstance().GetAll().FirstOrDefault();
                var cityList = CityDao.GetInstance().GetByStateId(stateInfo._id);
                var options = new ChromeOptions();
                string profilePath = @"D:\ChromeProfile\UserData4";
                options.AddArgument($"--user-data-dir={profilePath}");
                using (var driver = new ChromeDriver(options))
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10000));

                    foreach (var city in cityList)
                    {
                        if(city.NeighborhoodCount > 0) continue;

                        var nbhList = new List<NeighborhoodInfo>();
                        var nbhCount = 0;
                        driver.Navigate().GoToUrl(city.ReferenceLink);

                        await Task.Delay(5000);

                        wait.Until((x) =>
                        {
                            return ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete");

                        });

                        // Lấy chiều cao của trang (scrollHeight) bằng JavaScript
                        IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
                        var scrollHeight = (long)jsExecutor.ExecuteScript("return document.body.scrollHeight");
                        // Cuộn xuống 30% trang
                        jsExecutor.ExecuteScript($"window.scrollTo(0, {scrollHeight * 0.5});");
                        await Task.Delay(5000);
                        scrollHeight = (long)jsExecutor.ExecuteScript("return document.body.scrollHeight");
                        // Cuộn xuống 50% trang
                        jsExecutor.ExecuteScript($"window.scrollTo(0, {scrollHeight * 0.5});");
                        await Task.Delay(5000);
                        scrollHeight = (long)jsExecutor.ExecuteScript("return document.body.scrollHeight");
                        // Cuộn xuống 80% trang
                        jsExecutor.ExecuteScript($"window.scrollTo(0, {scrollHeight * 0.6});");
                        await Task.Delay(5000);
                        scrollHeight = (long)jsExecutor.ExecuteScript("return document.body.scrollHeight");
                        // Cuộn xuống 100% trang
                        jsExecutor.ExecuteScript($"window.scrollTo(0, {scrollHeight * 0.8});");
                        // CSS Selector cho div với class dài
                        var cssSelector = "div.Styled_display-sm__zpop7k53.Grid__n0b9cgg.Grid_gridTemplateColumns-xs__n0b9cg5.Grid__n0b9cgh.reset__1m5uu6e0";
                        await Task.Delay(5000);

                        // Lấy tất cả thẻ <a> bên trong div đó
                        var links = driver.FindElements(By.CssSelector($"{cssSelector} > div.reset__1m5uu6e0 > a"));
                        int timeW = 0;
                        while ((links == null || links.Count == 0) && timeW < 5)
                        {
                            links = driver.FindElements(By.CssSelector($"{cssSelector} a"));
                            await Task.Delay(3000);
                            timeW++;
                        }

                        if (links.Count == 0)
                        {
                            continue;
                        }

                        foreach (var link in links)
                        {
                            try
                            {
                                var nbhInfo = new NeighborhoodInfo();
                                nbhInfo.Name = link.Text;
                                nbhInfo.ReferenceLink = link.GetAttribute("href");
                                nbhInfo.StateId = stateInfo._id;
                                nbhInfo.CityId = city._id;
                                nbhList.Add(nbhInfo);
                                nbhCount++;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        city.NeighborhoodCount = nbhCount;
                        NeighborhoodDao.GetInstance().InsertRange(nbhList);
                        CityDao.GetInstance().Replace(city);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return Json(new
            {
            });
        }
    }
}
