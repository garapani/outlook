using AdvancedREI.Net.Http.Compression;
using Newtonsoft.Json;
using Outlook.Model;
using OutlookIndia.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Outlook.Client
{
    public class OutlookClient
    {
        #region Methods
        private const string LATESTNEWS_URL = "http://outlook.thevillagesoftware.com/API/Articles?category=Latest News";
        private const string PHOTOS_URL = "http://outlook.thevillagesoftware.com/API/Articles?category=photos";
        private const string SPORTS_URL = "http://outlook.thevillagesoftware.com/API/Articles?category=sports";
        private const string BUSINESS_URL = "http://outlook.thevillagesoftware.com/API/Articles?category=business";
        private const string NATIONAL_URL = "http://outlook.thevillagesoftware.com/API/Articles?category=national";
        private const string INTERNATIONAL_URL = "http://outlook.thevillagesoftware.com/API/Articles?category=international";
        private const string ARTSANDENTERTAINMENT_URL = "http://outlook.thevillagesoftware.com/API/Articles?category=Arts-Entertainment";
        private const string BOOKS_URL = "http://outlook.thevillagesoftware.com/API/Articles?category=books";
        private const string BLOGS_URL = "http://outlook.thevillagesoftware.com/API/Articles?category=blogs";


        public async Task<List<Article>> GetArticlesAsync(string category)
        {
            List<Article> articles = new List<Article>();
            try
            {
                System.Net.Http.HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                System.Net.Http.HttpClient httpClient = new HttpClient(httpClientHandler);
                var categoryUrl = "";
                switch (category.ToLower())
                {
                    case "latest news":
                        categoryUrl = LATESTNEWS_URL;
                        break;
                    case "photos":
                        categoryUrl = PHOTOS_URL;
                        break;
                    case "sports":
                        categoryUrl = SPORTS_URL;
                        break;
                    case "business":
                        categoryUrl = BUSINESS_URL;
                        break;
                    case "national":
                        categoryUrl = NATIONAL_URL;
                        break;
                    case "international":
                        categoryUrl = INTERNATIONAL_URL;
                        break;
                    case "arts-entertainment":
                        categoryUrl = ARTSANDENTERTAINMENT_URL;
                        break;
                    case "books":
                        categoryUrl = BOOKS_URL;
                        break;
                    case "blogs":
                        categoryUrl = BLOGS_URL;
                        break;
                }
                var tempString = await httpClient.GetStringAsync(categoryUrl);
                if (!string.IsNullOrEmpty(tempString))
                {
                    articles = JsonConvert.DeserializeObject<List<Article>>(tempString);
                }
            }
            catch (Exception ex)
            {

            }
            return articles;
        }

        public async Task<string> GetArticles(string url)
        {
            try
            {
                AdvancedREI.Net.Http.Compression.CompressedHttpClientHandler handler = new CompressedHttpClientHandler();
                System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient(handler);
                hc.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                hc.DefaultRequestHeaders.ConnectionClose = true;
                //hc.DefaultRequestHeaders.CacheControl.NoCache = true;
                return await hc.GetStringAsync(new Uri(url));
            }
            catch (Exception)
            {
                return "";
            }
        }

        public async Task<List<MyApp>> GetMyAppsAsync()
        {
            List<MyApp> listOfApps = new List<MyApp>();
            var myAppsUrl = string.Format("https://thehinduadrotator.blob.core.windows.net/myapps/myapps.json");
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip |
                                                 DecompressionMethods.Deflate;
            }

            try
            {
                var httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (iPhone; CPU iPhone OS 8_0 like Mac OS X) AppleWebKit/600.1.3 (KHTML, like Gecko) Version/8.0 Mobile/12A4345d Safari/600.1.4");
                var tempString = await httpClient.GetStringAsync(myAppsUrl);
                var myApps = JsonConvert.DeserializeObject<MyApps>(tempString);
                if (myApps != null)
                    listOfApps = myApps.myapps;
            }
            catch (Exception exception)
            {
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine("WinSourceClient:" + exception);
                }
            }
            return listOfApps;
        }

        #endregion Methods
    }
}