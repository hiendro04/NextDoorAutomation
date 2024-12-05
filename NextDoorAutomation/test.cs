using PuppeteerSharp;

namespace NextDoorAutomation
{
    public class test
    {
        public async Task testF()
        {

        string apiToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI2NzRlY2I1MGNiY2JlZWFlYWExMTBmOGYiLCJ0eXBlIjoiZGV2Iiwiand0aWQiOiI2NzUwMjY0YjA0MzM0NWI5ZTkxMmI0YmMifQ.lS_PGRldsPF3QLp34BYwhb2xD7v0lsZbntr5Z9z9HpM";
        string profileId = "674f0da6c82e63b31f0dcf00";

        #region test
        //try
        //{
        //    // Bắt đầu profile trên GoLogin
        //    var gologinApi = new GoLoginApiHelper(apiToken);
        //    string webSocketDebuggerUrl = await gologinApi.StartProfileAsync(profileId);

        //    // Khởi tạo Selenium WebDriver
        //    var seleniumHelper = new SeleniumHelper();
        //    IWebDriver driver = seleniumHelper.CreateWebDriver(webSocketDebuggerUrl);
        //    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));

        //    // Điều khiển trình duyệt qua Selenium
        //    driver.Navigate().GoToUrl("https://nextdoor.com/");

        //    var btnLoginSelector = "#root > div > div > div._1yixros4 > div:nth-child(3) > div > a";

        //    var loginButton = wait.Until(driver =>
        //    {
        //        var element = driver.FindElement(By.ClassName(btnLoginSelector));
        //        return (element.Displayed && element.Enabled) ? element : null;
        //    });

        //    loginButton.Click();

        //    Console.WriteLine(driver.Title);
        //}
        //catch(Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}

        #endregion

        #region test 1
        //try
        //{
        //    var gologinApi = new GoLoginApiHelper(apiToken);
        //    var browserPath = await gologinApi.GetBrowserProfile(profileId);
        //    // Cấu hình Selenium với trình duyệt GoLogin
        //    ChromeOptions options = new ChromeOptions();
        //    options.BinaryLocation = browserPath;

        //    // Tạo driver và khởi chạy trình duyệt
        //    IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver(options);

        //    // Mở trang web thử nghiệm
        //    driver.Navigate().GoToUrl("https://example.com");

        //    Console.WriteLine($"Title: {driver.Title}");

        //    // Đóng trình duyệt sau khi hoàn thành
        //    driver.Quit();
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}
        #endregion

        #region test 2 - ok
        string wsUrl = "ws://127.0.0.1:26236/devtools/browser/f0b70ab9-d3a1-42bc-936b-836d3e40b4f4";
        await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
        {
            BrowserWSEndpoint = wsUrl
        });

        var pages = await browser.PagesAsync();
        var page = pages.FirstOrDefault();
        await page.GoToAsync("https://nextdoor.com/");

        var title = await page.GetTitleAsync();

        // Chờ đến khi phần tử xuất hiện
        await page.WaitForSelectorAsync("[data-testid='nux-top-bar-login-button']");

        await page.ClickAsync("[data-testid='nux-top-bar-login-button']");

        #endregion
        }

    }
}
