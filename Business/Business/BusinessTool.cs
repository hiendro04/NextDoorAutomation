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
                //string wsUrl = await gologinInfo.StartProfileAsync(profileId);
                var wsUrlInfo = WsUrlDao.GetInstance().GetLastInfo(profileId);
                string wsUrl = wsUrlInfo.WsUrl;

                //todo-tạm thời do mạng chậm để load trang full
                #region test 2 - ok
                await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
                {
                    BrowserWSEndpoint = wsUrl,
                });

                await delayTime(wsUrlInfo.DelayTime);

                //var page = await browser.NewPageAsync();
                var pages = await browser.PagesAsync();
                var page = pages.FirstOrDefault();
                await page.GoToAsync("https://nextdoor.com/news_feed/", null, [WaitUntilNavigation.DOMContentLoaded]);

                await delayTime(wsUrlInfo.DelayTime);

                // Chờ cho container chính của newsfeed hiển thị
                await page.WaitForSelectorAsync("div[data-testid='feed-container']");

                await delayTime(wsUrlInfo.DelayTime);

                // Số lần cuộn
                int scrollTimes = 10;

                // Cuộn xuống nhiều lần
                for (int i = 0; i < scrollTimes; i++)
                {
                    Console.WriteLine($"Đang cuộn xuống lần {i + 1}...");

                    // Cuộn xuống cuối trang bằng JavaScript
                    await page.EvaluateExpressionAsync(@"window.scrollTo(0, document.body.scrollHeight)");

                    // Chờ một khoảng thời gian để trang tải thêm nội dung
                    await delayTime(wsUrlInfo.DelayTime);
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

                        var info = new PostInfo
                        {
                            CustomerName = customerName.Trim(),
                            CustomerProfileUrl = customerProfileUrl.Trim(),
                            NeighborhoodName = neighborhoodName.Trim(),
                            CustomerAvatarUrl = customerAvatarUrl.Trim(),
                            TimePosted = TimePosted.Trim(),
                            Content = Content.Trim(),
                            CreatedDate = DateTime.UtcNow,
                            PostedTime = getPostedTime(TimePosted.Trim())
                        };

                        //check
                        if (PostDao.GetInstance().IsExist(info)) continue;

                        var oldInfo = posts.Find(p => p.Content.ToLower() == info.Content.ToLower()
                                                    && p.NeighborhoodName.ToLower() == info.NeighborhoodName.ToLower()
                                                    && p.CustomerName.ToLower() == info.CustomerName.ToLower());
                        if (oldInfo != null) continue;

                        // Thêm vào danh sách bài post
                        posts.Add(info);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                browser.Disconnect();

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
            try
            {
                var gologinInfo = new GoLoginApiHelper(apiToken);
                //var wsUrl = await gologinInfo.StartProfileAsync(profileId);
                var wsUrlInfo = WsUrlDao.GetInstance().GetLastInfo(profileId);
                string wsUrl = wsUrlInfo.WsUrl;

                #region test 2 - ok
                await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions
                {
                    BrowserWSEndpoint = wsUrl
                });

                await delayTime(wsUrlInfo.DelayTime);

                var pages = await browser.PagesAsync();
                var page = pages.FirstOrDefault();
                await page.GoToAsync(info.CustomerProfileUrl);

                await delayTime(wsUrlInfo.DelayTime);

                // Chờ đến khi phần tử xuất hiện
                await page.WaitForSelectorAsync("button.blocks-1g1g3ih");

                await page.ClickAsync("button.blocks-1g1g3ih");

                browser.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            #endregion
        }
        public async Task StartProfileAsync()
        {
            try
            {
                var gologinInfo = new GoLoginApiHelper(apiToken);
                var wsUrl = await gologinInfo.StartProfileAsync(profileId);

                var info = new WsUrlInfo()
                {
                    Name = "alex10013",
                    ProfileId = profileId,
                    WsUrl = wsUrl,
                    CreatedDate = DateTime.Now,
                    DelayTime = 10
                };
                WsUrlDao.GetInstance().Insert(info);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }

        private DateTime? getPostedTime(string agoTime)
        {
            var now = DateTime.UtcNow;
            if (int.TryParse(agoTime.Split(' ')[0], out int c))
            {
                if (agoTime.Contains("min ago"))
                {
                    return now.AddMinutes(-c);
                }
                if (agoTime.Contains("hr ago"))
                {
                    return now.AddHours(-c);
                }
                if (agoTime.Contains("day ago") || agoTime.Contains("days ago"))
                {
                    return now.AddDays(-c);
                }
            }
            return now;
        }

        private async Task delayTime(int time)
        {
            if (time > 0)
            {
                await Task.Delay(time * 1000);
            }
            else
            {
                await Task.Delay(15000);
            }
        }

    }
}
