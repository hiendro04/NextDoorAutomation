using Business.Helpers;
using PuppeteerSharp;

namespace NextDoorAutomation
{
    public class Test
    {
        string apiToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI2NmVlZGMwMWRiNjZjYjExZWVjYTgzYWUiLCJ0eXBlIjoiZGV2Iiwiand0aWQiOiI2NzUxY2U1ZGNkNGMzMDA2ZmUxZmE0NWEifQ.Lbph_t43G49qUbXW4rLVyOkfDPPYQPQ-uLIiNVjyeys";
        string profileId = "673c912fecbebacfed635da1";
        public async Task GetData()
        {
            try
            {
                var gologinInfo = new GoLoginApiHelper(apiToken);
                //string wsUrl = "ws://127.0.0.1:24620/devtools/browser/03d13bcf-ea96-4116-99ad-a84152af0e53";
                string wsUrl = await gologinInfo.StartProfileAsync(profileId);

                #region test 2 - ok
                await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
                {
                    BrowserWSEndpoint = wsUrl,
                });

                //var page = await browser.NewPageAsync();
                var pages = await browser.PagesAsync();
                var page = pages.FirstOrDefault();
                await page.GoToAsync("https://nextdoor.com/");

                // Chờ cho container chính của newsfeed hiển thị
                await page.WaitForSelectorAsync("div[data-testid='feed-container']", new WaitForSelectorOptions
                {
                    Timeout = 999999
                });

                // Tìm các bài viết trực tiếp từ PuppeteerSharp
                var postElements = await page.QuerySelectorAllAsync("div[data-testid='feed-container'] div[data-v3-view-type='V3Wrapper']");

                var posts = new List<string>();

                // Lấy nội dung từng bài viết
                foreach (var postElement in postElements)
                {
                    // Trích xuất nội dung văn bản của bài post
                    var content = await postElement.EvaluateFunctionAsync<string>("node => node.innerText");
                    posts.Add(content);
                }

                // In danh sách bài viết
                Console.WriteLine("Danh sách bài viết:");
                foreach (var post in posts)
                {
                    Console.WriteLine("-----------");
                    Console.WriteLine(post);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }

        }

        public async Task Inbox()
        {
            string wsUrl = "ws://127.0.0.1:26236/devtools/browser/f0b70ab9-d3a1-42bc-936b-836d3e40b4f4";

            var gologinInfo = new GoLoginApiHelper(apiToken);
            wsUrl = await gologinInfo.StartProfileAsync(profileId);

            #region test 2 - ok
            await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
            {
                BrowserWSEndpoint = wsUrl
            });

            var pages = await browser.PagesAsync();
            var page = pages.FirstOrDefault();
            await page.GoToAsync("https://www.facebook.com/");

            var title = await page.GetTitleAsync();

            // Chờ đến khi phần tử xuất hiện
            await page.WaitForSelectorAsync("[data-testid='open-registration-form-button']");

            await page.ClickAsync("[data-testid='open-registration-form-button']");

            #endregion
        }

    }
}
