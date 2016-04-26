using GalaSoft.MvvmLight;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using Outlook.Client;
using Outlook.Model;
using OutlookIndia.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;

namespace Outlook.Services
{
    public class DataService : ViewModelBase
    {
        #region Constants

        private const int _expairyDays = 1;
        private string categoryUrlTemplate = "http://www.outlook.thevillagesoftware.com/API/Articles?categoryName=";
        public const string LATESTNEWS = "LATEST NEWS";
        public const string PHOTOS = "PHOTOS";
        public const string BLOGS = "BLOGS";
        public const string BOOKS = "BOOKS";
        public const string ARTSANDENETERTAINEMNT = "ARTS-ENTERTAINMENT";
        public const string SPORTS = "SPORTS";
        public const string NATIONAL = "NATIONAL";
        public const string INTERNATIONAL = "INTERNATIONAL";
        public const string BUSINESS = "BUSINESS";

        private const string AgentName = "Outlook.Agent";

        #endregion Constants

        #region Fields

        private readonly OutlookClient _OutlookClient = new OutlookClient();

        #endregion Fields

        #region Properties

        public Settings Settings { get; private set; }

        private List<Category> _listofCategories;
        public List<Category> ListOfCategories
        {
            get
            {
                return _listofCategories;
            }
        }

        private Article _currentArticle;
        public Article CurrentArticle
        {
            get { return _currentArticle; }
            set
            { this.Set<Article>("CurrentArticle", ref _currentArticle, value); }
        }

        public bool CanMoveToPreviousArticle { get; private set; }
        public bool CanMoveToNextArticle { get; private set; }

        #endregion Properties

        #region Constructor

        public DataService()
        {
            if (_listofCategories == null)
                _listofCategories = new List<Category>();

            InitializeCategories();
            InitializeLiveTile();
        }

        #endregion Constructor

        #region Methods

        private void InitializeCategories()
        {
            Category tempCategory = new Category();
            tempCategory.CategoryName = LATESTNEWS;
            tempCategory.CategoryUrl = categoryUrlTemplate + LATESTNEWS;
            tempCategory.LastUpdated = DateTime.Now.AddDays(-1);
            tempCategory.CanShow = false;
            _listofCategories.Add(tempCategory);
            tempCategory = null;
            tempCategory = new Category();
            tempCategory.CategoryName = PHOTOS;
            tempCategory.CanShow = false;
            tempCategory.LastUpdated = DateTime.Now.AddDays(-1);
            tempCategory.CategoryUrl = categoryUrlTemplate + PHOTOS;
            _listofCategories.Add(tempCategory);
            tempCategory = null;
            tempCategory = new Category();
            tempCategory.CategoryName = SPORTS;
            tempCategory.CanShow = true;
            tempCategory.LastUpdated = DateTime.Now.AddDays(-1);
            tempCategory.CategoryUrl = categoryUrlTemplate + SPORTS;
            _listofCategories.Add(tempCategory);
            tempCategory = null;
            tempCategory = new Category();
            tempCategory.CategoryName = NATIONAL;
            tempCategory.CanShow = true;
            tempCategory.LastUpdated = DateTime.Now.AddDays(-1);
            tempCategory.CategoryUrl = categoryUrlTemplate + NATIONAL;
            _listofCategories.Add(tempCategory);
            tempCategory = null;
            tempCategory = new Category();
            tempCategory.CanShow = true;
            tempCategory.CategoryName = INTERNATIONAL;
            tempCategory.LastUpdated = DateTime.Now.AddDays(-1);
            tempCategory.CategoryUrl = categoryUrlTemplate + INTERNATIONAL;
            _listofCategories.Add(tempCategory);
            tempCategory = null;
            tempCategory = new Category();
            tempCategory.CategoryName = BUSINESS;
            tempCategory.LastUpdated = DateTime.Now.AddDays(-1);
            tempCategory.CategoryUrl = categoryUrlTemplate + BUSINESS;
            tempCategory.CanShow = true;
            _listofCategories.Add(tempCategory);
            tempCategory = null;
            tempCategory = new Category();
            tempCategory.CategoryName = BOOKS;
            tempCategory.LastUpdated = DateTime.Now.AddDays(-1);
            tempCategory.CanShow = true;
            tempCategory.CategoryUrl = categoryUrlTemplate + BOOKS;
            _listofCategories.Add(tempCategory);
            tempCategory = null;
            tempCategory = new Category();
            tempCategory.CategoryName = BLOGS;
            tempCategory.LastUpdated = DateTime.Now.AddDays(-1);
            tempCategory.CategoryUrl = categoryUrlTemplate + BLOGS;
            tempCategory.CanShow = true;
            _listofCategories.Add(tempCategory);
            tempCategory = null;
            tempCategory = new Category();
            tempCategory.CategoryName = ARTSANDENETERTAINEMNT;
            tempCategory.LastUpdated = DateTime.Now.AddDays(-1);
            tempCategory.CategoryUrl = categoryUrlTemplate + ARTSANDENETERTAINEMNT;
            tempCategory.CanShow = true;
            _listofCategories.Add(tempCategory);

            foreach (var category in _listofCategories)
            {
                Task.Run(async () =>
                {
                    await DatabaseOperations.GetInstance().AddCategoryAsync(category);
                });
            }
        }

        public void Load()
        {
            try
            {
                LoadSettings();
            }
            catch (Exception)
            {
            }
        }

        public void Save()
        {
            try
            {
                SaveSettings();
                UpdateAgent();
            }
            catch (Exception)
            {
            }
        }

        public async Task<bool> CanRefreshCategory(string categoryName)
        {
            var categoryDetails = await DatabaseOperations.GetInstance().GetCategoryDetailAsync(categoryName);
            if (categoryDetails != null)
            {
                return !categoryDetails.isRrefreshing && !categoryDetails.isLoading;
            }
            return false;
        }

        private static object GetApplicationSettingValue(string propertyName, object defaultValue = null)
        {
            object result = null;

            try
            {
                if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(propertyName, out result))
                {
                    result = defaultValue;
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public static void SaveSettingValueImmediately(string propertyName, object value)
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings[propertyName] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            catch (Exception)
            {
            }
        }

        public async Task<List<Article>> GetArticlesAsync(string categoryName, bool isForce = false)
        {
            List<Article> articles = new List<Article>();
            try
            {
                DateTime presentTime = DateTime.Now;
                bool refresh = false;
                Category category = await DatabaseOperations.GetInstance().GetCategoryDetailAsync(categoryName);
                if (category != null)
                {
                    var diff = DateTime.Now - category.LastUpdated;
                    if (diff.TotalMinutes > 15)
                    {
                        refresh = true;
                    }
                }
                else
                {
                    category = new Category();
                    category.CategoryName = categoryName;
                    category.CategoryUrl = categoryUrlTemplate + categoryName;
                    await DatabaseOperations.GetInstance().AddCategoryAsync(category);
                }

                var tempCategory = _listofCategories.Find(o => o.CategoryName.ToLower() == categoryName.ToLower());
                if (tempCategory != null)
                {
                    if (refresh || isForce)
                    {
                        if (NetworkInterface.GetIsNetworkAvailable())
                        {
                            var oldArticles = await DatabaseOperations.GetInstance().GetCategoryArticlesAsync(categoryName);
                            if (oldArticles != null && oldArticles.Count > 0)
                            {
                                articles.AddRange(oldArticles);
                            }
                            _listofCategories.Find(o => o.CategoryName.ToLower() == categoryName.ToLower()).isRrefreshing = true;
                            category.isRrefreshing = true;
                            OutlookClient client = new OutlookClient();
                            var latestArticles = await client.GetArticlesAsync(categoryName);
                            if (latestArticles != null && latestArticles.Count > 0)
                            {
                                //articles.AddRange(latestArticles.ToList());
                                foreach (var article in latestArticles)
                                {
                                    if (articles.Find(o => o.HeadLine == article.HeadLine) == null)
                                        articles.Add(article);
                                    await DatabaseOperations.GetInstance().AddOrUpdateArticleAsync(article);
                                }
                                _listofCategories.Find(o => o.CategoryName.ToLower() == categoryName.ToLower()).LastUpdated = DateTime.Now;
                                category.LastUpdated = DateTime.Now;
                            }
                            category.isRrefreshing = false;
                            await DatabaseOperations.GetInstance().UpdateCategoryAsync(category);
                        }
                    }
                    else
                    {
                        var tempArticles = await DatabaseOperations.GetInstance().GetCategoryArticlesAsync(categoryName);
                        if (tempArticles != null && tempArticles.Count > 0)
                        {
                            articles.AddRange(tempArticles);
                        }
                    }
                    if (_listofCategories.Find(o => o.CategoryName.ToLower() == categoryName.ToLower()).Articles == null)
                    {
                        _listofCategories.Find(o => o.CategoryName.ToLower() == categoryName.ToLower()).Articles = new ObservableCollection<Article>();
                    }
                    else
                    {
                        _listofCategories.Find(o => o.CategoryName.ToLower() == categoryName.ToLower()).Articles.Clear();
                    }
                    articles.OrderByDescending(o => o.ArticleDate).ToList().ForEach(o => _listofCategories.Find(l => l.CategoryName.ToLower() == categoryName.ToLower()).Articles.Add(o));
                }
            }
            catch (Exception)
            {
            }
            return articles.OrderByDescending(o => o.ArticleDate).ToList();
        }

        public bool NetWorkAvailable()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                return true;
            }
            return false;
        }

        public async Task<List<MyApp>> LoadMyAppsAsync()
        {
            return await _OutlookClient.GetMyAppsAsync();
        }

        #endregion Methods

        #region Private Methods

        private void InitializeLiveTile()
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings["LastNewArticlesCount"] = 0;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            catch (Exception)
            {
            }

            ShellTile appTile = ShellTile.ActiveTiles.First();

            if (appTile != null)
            {
                appTile.Update(new StandardTileData { Count = 0 });
            }
        }

        private void UpdateAgent()
        {
            if (Settings.IsDownloadingArticlesOffline == true)
            {
                StartAgent();
            }
            else
            {
                StopAgentIfStarted();
            }
        }

        private static void StartAgent()
        {
            StopAgentIfStarted();
            try
            {
                ScheduledActionService.Add(new PeriodicTask(AgentName) { Description = "Periodically download articles in the background if the Internet connection is Wi-Fi or Ethernet." });
#if DEBUG
                ScheduledActionService.LaunchForTest(AgentName, new TimeSpan(0, 0, 0, 3));
#endif
            }
            catch (Exception)
            {
            }
        }

        private static void StopAgentIfStarted()
        {
            if (ScheduledActionService.Find(AgentName) != null)
            {
                ScheduledActionService.Remove(AgentName);
            }
        }

        private void LoadSettings()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("Settings"))
            {
                Settings = (Settings)IsolatedStorageSettings.ApplicationSettings["Settings"];
            }
            else
            {
                Settings = new Settings();
            }
        }

        private void SaveSettings()
        {
            IsolatedStorageSettings.ApplicationSettings["Settings"] = Settings;

            if (CurrentArticle != null)
            {
                IsolatedStorageSettings.ApplicationSettings["CurrentArticle"] = CurrentArticle;
            }
        }
        #endregion

        internal void SetCurrentArticle(Article article)
        {
            CurrentArticle = article;
        }

        internal async Task<Article> GetArticleAsync(string headLine, string category)
        {
            Article article = new Article();
            bool foundArticle = false;
            if (_listofCategories.Count > 0)
            {
                var categoryDetails = _listofCategories.FirstOrDefault(o => o.CategoryName.ToLower() == category.ToLower());
                if (categoryDetails != null)
                {
                    if (categoryDetails.Articles != null && categoryDetails.Articles.Count > 0)
                    {
                        CurrentArticle = article = categoryDetails.Articles.FirstOrDefault(o => o.HeadLine == headLine);
                        foundArticle = true;
                    }
                }
            }
            else if (_listofCategories == null)
            {
                List<Category> categories = await DatabaseOperations.GetInstance().GetCategoriesAsync();
                if (categories != null && categories.Count > 0)
                {
                    _listofCategories.AddRange(categories);
                }
                else
                {
                    InitializeCategories();
                }
            }
            if (foundArticle == false || CurrentArticle == null)
            {
                var categoryArticles = await DatabaseOperations.GetInstance().GetCategoryArticlesAsync(category);
                if (categoryArticles != null && categoryArticles.Count > 0)
                {
                    if (_listofCategories.FirstOrDefault(o => o.CategoryName.ToLower() == category.ToLower()).Articles == null)
                        _listofCategories.FirstOrDefault(o => o.CategoryName.ToLower() == category.ToLower()).Articles = new ObservableCollection<Article>();

                    categoryArticles.ForEach(o =>
                    {
                        _listofCategories.FirstOrDefault(l => l.CategoryName.ToLower() == category.ToLower()).Articles.Add(o);
                    });

                    CurrentArticle = article = categoryArticles.FirstOrDefault(o => o.HeadLine == headLine);
                }
            }
            return article;
        }

        internal Article GetNextArticle(string category)
        {
            Article article = new Article();
            try
            {
                if (_listofCategories != null)
                {
                    var categoryDetails = _listofCategories.FirstOrDefault(o => o.CategoryName.ToLower() == category.ToLower());
                    if (categoryDetails != null)
                    {
                        var index = categoryDetails.Articles.ToList().FindIndex(o => o.HeadLine == CurrentArticle.HeadLine);
                        article = index == categoryDetails.Articles.Count - 1 ? categoryDetails.Articles[0] : categoryDetails.Articles[index + 1];
                    }
                }
            }
            catch (Exception exception)
            {
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine("DataService:" + exception);
                }
            }
            return article;
        }

        internal Article GetPreviousArticle(string category)
        {
            Article article = new Article();
            try
            {
                if (_listofCategories != null)
                {
                    var categoryDetails = _listofCategories.FirstOrDefault(o => o.CategoryName.ToLower() == category.ToLower());
                    if (categoryDetails != null)
                    {
                        var index = categoryDetails.Articles.ToList().FindIndex(o => o.HeadLine == CurrentArticle.HeadLine);
                        article = index == 0
                        ? categoryDetails.Articles[categoryDetails.Articles.Count - 1]
                        : categoryDetails.Articles[index - 1];
                    }
                }
            }
            catch (Exception exception)
            {
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine("DataService:" + exception);
                }
            }
            return article;
        }
    }
}