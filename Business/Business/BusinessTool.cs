using Business.Dao;
using Business.Helpers;
using Business.Models;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Business
{
    public class BusinessTool
    {
        private static BusinessTool _instance;
        public static BusinessTool GetInstance()
        {
            if(_instance == null)
            {
                _instance = new BusinessTool();
            }
            return _instance;
        }

        string apiToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI2NmVlZGMwMWRiNjZjYjExZWVjYTgzYWUiLCJ0eXBlIjoiZGV2Iiwiand0aWQiOiI2NzUxY2U1ZGNkNGMzMDA2ZmUxZmE0NWEifQ.Lbph_t43G49qUbXW4rLVyOkfDPPYQPQ-uLIiNVjyeys";
        string profileId = "673c93c52a10ef033f803be7";
        public async Task GetData()
        {
            try
            {
                var gologinInfo = new GoLoginApiHelper(apiToken);
                //string wsUrl = "ws://127.0.0.1:24620/devtools/browser/03d13bcf-ea96-4116-99ad-a84152af0e53";
                string wsUrl = await gologinInfo.StartProfileAsync(profileId);

                //todo-tạm thời do mạng chậm để load trang full
                Task.Delay(10000);

                #region test 2 - ok
                await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
                {
                    BrowserWSEndpoint = wsUrl,
                });

                //var page = await browser.NewPageAsync();
                var pages = await browser.PagesAsync();
                var page = pages.FirstOrDefault();
                //await page.GoToAsync("https://nextdoor.com/news_feed/", null, [WaitUntilNavigation.DOMContentLoaded]);

                // Chờ cho container chính của newsfeed hiển thị
                await page.WaitForSelectorAsync("div[data-testid='feed-container']", new WaitForSelectorOptions
                {
                    Timeout = 50000
                });
                Task.Delay(15000);
                // Số lần cuộn
                int scrollTimes = 5;

                // Khoảng thời gian chờ sau mỗi lần cuộn (để tải bài viết mới)
                int scrollDelay = 3000; // 3 giây

                // Cuộn xuống nhiều lần
                for (int i = 0; i < scrollTimes; i++)
                {
                    Console.WriteLine($"Đang cuộn xuống lần {i + 1}...");

                    // Cuộn xuống cuối trang bằng JavaScript
                    await page.EvaluateExpressionAsync(@"window.scrollTo(0, document.body.scrollHeight)");

                    // Chờ một khoảng thời gian để trang tải thêm nội dung
                    await Task.Delay(scrollDelay);
                }

                // Tìm các bài viết trực tiếp từ PuppeteerSharp
                var postElements = await page.QuerySelectorAllAsync("div[data-testid='feed-container'] div[data-v3-view-type='V3Wrapper']");

                var posts = new List<PostInfo>();

                // Lấy nội dung từng bài viết
                foreach (var postElement in postElements)
                {
                    try
                    {
                        // Lấy tên khu vực
                        var neighborhoodName = await postElement.QuerySelectorAsync("div[data-testid='author-children-test'] a.post-byline-redesign.post-byline-truncated")
                            ?.EvaluateFunctionAsync<string>("node => node.innerText");

                        // Lấy tên khách hàng
                        var customerName = await postElement.QuerySelectorAsync("a._3I7vNNNM.E7NPJ3WK")
                            ?.EvaluateFunctionAsync<string>("node => node.innerText");

                        // Lấy URL profile của khách hàng
                        var customerProfileUrl = await postElement.QuerySelectorAsync("a._3I7vNNNM.E7NPJ3WK")
                            ?.EvaluateFunctionAsync<string>("node => node.href");

                        // Lấy thời gian đăng bài
                        var TimePosted = await postElement.QuerySelectorAsync("div[data-testid='author-children-test'] div.post-byline-redesign.blocks-1wvf4u1")
                            ?.EvaluateFunctionAsync<string>("node => node.innerText");

                        // Lấy URL avatar khách hàng
                        var customerAvatarUrl = await postElement.QuerySelectorAsync("div[data-testid='mini-profile-wrapper'] img.Image__1yysfuh2.reset__1m5uu6e0")
                            ?.EvaluateFunctionAsync<string>("node => node.src");

                        // Lấy Content
                        var Content = await postElement.QuerySelectorAsync("div[data-testid='post-body'] span.postTextBodySpan span.Linkify")
                            ?.EvaluateFunctionAsync<string>("node => node.innerText");

                        // Thêm vào danh sách bài post
                        posts.Add(new PostInfo
                        {
                            CustomerName = customerName,
                            CustomerProfileUrl = customerProfileUrl,
                            NeighborhoodName = neighborhoodName,
                            CustomerAvatarUrl = customerAvatarUrl,
                            TimePosted = TimePosted,
                            Content = Content,
                            CreatedDate = DateTime.UtcNow,
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                PostDao.GetInstance().InsertRange(posts);
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }

        }

        public async Task Inbox(PostInfo info)
        {
            var gologinInfo = new GoLoginApiHelper(apiToken);
            var wsUrl = await gologinInfo.StartProfileAsync(profileId);

            #region test 2 - ok
            await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
            {
                BrowserWSEndpoint = wsUrl
            });

            var pages = await browser.PagesAsync();
            var page = pages.FirstOrDefault();
            await page.GoToAsync($"https://nextdoor.com/{info.CustomerProfileUrl}");

            // Chờ đến khi phần tử xuất hiện
            await page.WaitForSelectorAsync("[data-testid='open-registration-form-button']");

            await page.ClickAsync("[data-testid='open-registration-form-button']");

            #endregion
        }
        public async Task<string> GetProfileAsync()
        {
            try
            {
                var gologinInfo = new GoLoginApiHelper(apiToken);
                //string wsUrl = "ws://127.0.0.1:24620/devtools/browser/03d13bcf-ea96-4116-99ad-a84152af0e53";
                string rs = await gologinInfo.GetBrowserProfile(profileId);
                return rs;
            }
            catch (Exception ex)
            {
                return "";
                Console.WriteLine($"Error: {ex}");
            }
        }
    }
}
