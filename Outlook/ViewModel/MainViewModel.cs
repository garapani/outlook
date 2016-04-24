using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Outlook.Model;
using Outlook.Services;
using OutlookIndia.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using Windows.System.Threading;

namespace Outlook.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private readonly DataService _dataService;

        #endregion Fields

        #region Properties

        public bool HasInternet { get; private set; }
        public bool IsCachedModeMessageDisplayed { get; set; }

        public bool IsRefreshingArticles { get; set; }
        public bool IsRefreshingLatestNews { get; set; }
        public bool IsRefreshingPhotos { get; set; }
        public bool IsProblem { get; set; }
        public bool IsMyAppsLoading { get; set; }

        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<MyApp> ListOfMyApps { get; set; }
        public MyApp MySelectedApp { get; set; }

        public ObservableCollection<Article> ActiveCategoryArticles { get; set; }
        //public ObservableCollection<Article> LatestNews { get; set; }
        //public ObservableCollection<Article> Photos { get; set; }

        public Category SelectedCategory { get; set; }
        public bool IsRefreshingActiveCategory { get; set; }
        
        public Article CurrentArticle { get; set; }

        private ObservableCollection<Article> _latestNews;
        public ObservableCollection<Article> LatestNews
        {
            get { return _latestNews; }
            set { _latestNews = value; RaisePropertyChanged("LatestNews"); }
        }

        private ObservableCollection<Article> _photos;
        public ObservableCollection<Article> Photos
        {
            get { return _photos; }
            set { _photos = value; RaisePropertyChanged("Photos"); }
        }

        public RelayCommand LoadedCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand ShowSettingsCommand { get; private set; }
        public RelayCommand ShowRateTheAppCommand { get; private set; }
        public RelayCommand ShowShareTheAppCommand { get; private set; }
        public RelayCommand ShowAboutCommand { get; private set; }
        public RelayCommand ReadCurrentArticleCommand { get; private set; }
        public RelayCommand ReadSelectedCategoryCommand { get; private set; }
        public RelayCommand ShowMyAppsCommand { get; private set; }
        public RelayCommand ReadMyAppCommand { get; private set; }
        public RelayCommand MyAppsLoadedCommand { get; private set; }

        public string ActiveCategory = DataService.LATESTNEWS;

        #endregion Properties


        #region Constructor

        public MainViewModel(DataService dataService)
        {
            try
            {
                if (!DesignerProperties.IsInDesignTool)
                {   
                    ActiveCategoryArticles = new ObservableCollection<Article>();
                    Categories = new ObservableCollection<Category>();
                    _dataService = dataService;
                    _dataService.ListOfCategories.ForEach(o => Categories.Add(o));

                    SelectedCategory = new Category();

                    if (CurrentArticle == null)
                    {
                        CurrentArticle = new Article();
                    }
                    
                    RefreshCommand = new RelayCommand(ForceRefreshData);
                    ShowSettingsCommand = new RelayCommand(ShowSettings);
                    ShowRateTheAppCommand = new RelayCommand(ShowRateTheApp);
                    ShowShareTheAppCommand = new RelayCommand(ShowShareTheApp);
                    ShowAboutCommand = new RelayCommand(ShowAbout);
                    ReadCurrentArticleCommand = new RelayCommand(ReadCurrentArticle);
                    ReadSelectedCategoryCommand = new RelayCommand(ReadSelectedCategory);
                    ShowMyAppsCommand = new RelayCommand(ShowMyApps);
                    MyAppsLoadedCommand = new RelayCommand(GetMyApps);
                    ReadMyAppCommand = new RelayCommand(OpenMyApp);
                    HasInternet = true;
                    MySelectedApp = new MyApp();
                    Task.Run(async () =>
                    {
                        await DatabaseOperations.GetInstance().DeleteExpiredArticlesAsync();
                        await UpdateLatestNewsAsync();
                        await UpdatePhotosAsync();
                    });
                }
            }
            catch (Exception)
            {
            }
        }

        private void ShowMyApps()
        {
            App.RootFrame.Navigate(new Uri("/Views/MyAppsPage.xaml", UriKind.Relative));
        }

        private void OpenMyApp()
        {
            if(MySelectedApp != null)
            {
                var wbt = new WebBrowserTask();
                wbt.Uri = new Uri(MySelectedApp.appUrl, UriKind.Absolute);
                wbt.Show();
            }
        }

        #endregion Constructor


        #region Event Handlers

        private async void GetMyApps()
        {
            if (_dataService.NetWorkAvailable())
            {
                IsMyAppsLoading = true;
                if (ListOfMyApps == null)
                    ListOfMyApps = new ObservableCollection<MyApp>();
                if (ListOfMyApps.Count == 0)
                {
                    var myApps = await _dataService.LoadMyAppsAsync();
                    if (myApps != null)
                    {
                        foreach (var myapp in myApps)
                        {
                            ListOfMyApps.Add(myapp);
                        }
                    }
                }
                IsMyAppsLoading = false;
            }
            else
            {
                App.RootFrame.Navigate(new Uri("/Views/NetworkError.xaml", UriKind.Relative));
            }
        }

        #endregion Event Handlers

        #region Command

        public async void GetCategoryArticlesAsync(string category, bool force = false)
        {
            try
            {
                if (_dataService.NetWorkAvailable())
                {
                    if (category.ToLower() == DataService.LATESTNEWS.ToLower())
                    {
                        await UpdateLatestNewsAsync(force);
                    }
                    else if (category.ToLower() == DataService.PHOTOS.ToLower())
                    {
                        await UpdatePhotosAsync(force);
                    }
                    else
                    {
                        await UpdateArticlesAsync(category, force);
                    }
                }
                else
                {
                    DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                    {
                        App.RootFrame.Navigate(new Uri("/Views/NetworkError.xaml", UriKind.Relative));
                    });
                }
            }
            catch (Exception)
            {
            }
        }

        private void ForceRefreshData()
        {
            if (string.IsNullOrEmpty(ActiveCategory))
            {
                GetCategoryArticlesAsync(SelectedCategory.CategoryName, true);
            }
            else
            {
                GetCategoryArticlesAsync(ActiveCategory, true);
            }
        }

        private void ShowSettings()
        {
            try
            {
                App.RootFrame.Navigate(new Uri("/Views/SettingsPage.xaml", UriKind.Relative));
            }
            catch (Exception)
            {
            }
        }

        private void ShowShareTheApp()
        {
            try
            {
                App.RootFrame.Navigate(new Uri("/Views/SharePage.xaml", UriKind.Relative));
            }
            catch (Exception)
            {
            }
        }

        private void ShowRateTheApp()
        {
            try
            {
                var mp = new MarketplaceReviewTask();
                mp.Show();
            }
            catch (Exception)
            {
            }
        }

        private void ShowAbout()
        {
            try
            {
                App.RootFrame.Navigate(new Uri("/Views/AboutPage.xaml", UriKind.Relative));
            }
            catch (Exception)
            {
            }
        }

        public void ReadSelectedCategory()
        {
            try
            {
                if (SelectedCategory != null && !string.IsNullOrEmpty(SelectedCategory.CategoryName))
                {
                    App.RootFrame.Navigate(new Uri("/Views/CategoryPage.xaml?category=" + SelectedCategory.CategoryName, UriKind.Relative));
                }
            }
            catch (Exception)
            {
            }
        }

        private async void ReadCurrentArticle()
        {
            try
            {
                if (CurrentArticle != null && !string.IsNullOrEmpty(CurrentArticle.HeadLine))
                {
                    await ThreadPool.RunAsync((workItem) => { _dataService.SetCurrentArticle(CurrentArticle); });
                    App.RootFrame.Navigate(new Uri(string.Format("/Views/ArticlePage.xaml?Id={0}&Category={1}", CurrentArticle.HeadLine, CurrentArticle.Category), UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Command

        #region Private Methods

        private void UpdateNotifications(Article newArticle)
        {
            try
            {
                if (newArticle != null)
                {
                    ShellTile appTile = ShellTile.ActiveTiles.First();
                    if (appTile != null)
                    {
                        if (string.IsNullOrEmpty(newArticle.Thumb))
                        {
                            appTile.Update(new FlipTileData
                            {
                                BackContent = newArticle.HeadLine,
                                WideBackContent = newArticle.HeadLine,
                                BackTitle = "Outlook India"
                            });
                        }
                        else
                        {
                            //var uri = new Uri("isostore:/Shared/ShellContent/" + "liveTile.jpg", UriKind.Absolute);
                            appTile.Update(new FlipTileData
                            {
                                BackContent = newArticle.HeadLine,
                                WideBackContent = newArticle.HeadLine,
                                BackgroundImage = new Uri(newArticle.Thumb),
                                WideBackgroundImage = new Uri(newArticle.Thumb),
                                BackTitle = "Outlook India"
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void UpdateLiveTile(Article article)
        {
            UpdateNotifications(article);
            //string tempJpeg = "liveTile.jpg";
            //try
            //{
            //    var webClient = new WebClient();
            //    webClient.OpenReadCompleted += (object sender, OpenReadCompletedEventArgs e) =>
            //    {
            //        var streamResourceInfo = new StreamResourceInfo(e.Result, null);

            //        var userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
            //        if (userStoreForApplication.FileExists("Shared/ShellContent/" + tempJpeg))
            //        {
            //            userStoreForApplication.DeleteFile("Shared/ShellContent/" + tempJpeg);
            //        }

            //        var isolatedStorageFileStream = userStoreForApplication.CreateFile("Shared/ShellContent/" + tempJpeg);

            //        var bitmapImage = new BitmapImage { CreateOptions = BitmapCreateOptions.None };
            //        bitmapImage.SetSource(streamResourceInfo.Stream);

            //        var writeableBitmap = new WriteableBitmap(bitmapImage);
            //        writeableBitmap.SaveJpeg(isolatedStorageFileStream, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight, 0, 85);
            //        isolatedStorageFileStream.Close();
            //        DispatcherHelper.UIDispatcher.BeginInvoke(
            //            delegate
            //            {
            //                UpdateNotifications(article);
            //            });
            //    };
            //    webClient.OpenReadAsync(new Uri(article.Thumb, UriKind.Absolute));
            //}
            //catch (Exception)
            //{
            //}
        }

        private async Task UpdateArticlesAsync(string category, bool force = false)
        {
            IsProblem = false;
            try
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(
                    delegate
                    {
                        IsRefreshingActiveCategory = true;

                        if (ActiveCategoryArticles == null)
                        {
                            ActiveCategoryArticles = new ObservableCollection<Article>();
                        }
                        ActiveCategoryArticles.Clear();
                    });
                var tempArticles = await _dataService.GetArticlesAsync(category,force);
                if (tempArticles != null && tempArticles.Count > 0)
                {
                    DispatcherHelper.UIDispatcher.BeginInvoke(
                     delegate
                     {
                         if (Categories == null)
                         {
                             Categories = new ObservableCollection<Category>();
                             _dataService.ListOfCategories.ForEach(o => Categories.Add(o));
                         }
                         Category categoryDetails = Categories.FirstOrDefault(o => o.CategoryName.ToLower() == category.ToLower());

                         foreach (var article in tempArticles.ToList())
                         {
                             ActiveCategoryArticles.Add(article);
                         }
                         IsRefreshingActiveCategory = false;
                     });
                }
                else
                {
                    IsProblem = true;
                    IsRefreshingActiveCategory = false;
                }
            }
            catch (Exception ex)
            {
            }
        }


        private async Task UpdatePhotosAsync(bool isForce = false)
        {
            IsProblem = false;
            try
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(
                    delegate
                    {
                        IsRefreshingPhotos = true;

                        if (Photos == null)
                        {
                            Photos = new ObservableCollection<Article>();
                        }
                        Photos.Clear();

                    });
                var tempArticles = await _dataService.GetArticlesAsync(DataService.PHOTOS, isForce);
                if (tempArticles != null && tempArticles.Count > 0)
                {
                    DispatcherHelper.UIDispatcher.BeginInvoke(
                     delegate
                     {
                         if (Categories == null)
                         {
                             Categories = new ObservableCollection<Category>();
                             _dataService.ListOfCategories.ForEach(o => Categories.Add(o));
                         }
                         Category categoryDetails = Categories.FirstOrDefault(o => o.CategoryName.ToLower() == DataService.PHOTOS.ToLower());

                         foreach (var article in tempArticles.ToList())
                         {
                             Photos.Add(article);
                         }
                         IsRefreshingPhotos = false;
                     });
                }
                else
                {
                    IsProblem = true;
                    IsRefreshingPhotos = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async Task UpdateLatestNewsAsync(bool isForce = false)
        {
            IsProblem = false;
            try
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(
                    delegate
                    {
                        IsRefreshingLatestNews = true;

                        if (LatestNews == null)
                        {
                            LatestNews = new ObservableCollection<Article>();
                        }
                        LatestNews.Clear();

                    });
                var tempArticles = await _dataService.GetArticlesAsync(DataService.LATESTNEWS,isForce);
                if (tempArticles != null && tempArticles.Count > 0)
                {
                    DispatcherHelper.UIDispatcher.BeginInvoke(
                     delegate
                     {
                         if (Categories == null)
                         {
                             Categories = new ObservableCollection<Category>();
                             _dataService.ListOfCategories.ForEach(o => Categories.Add(o));
                         }
                         Category categoryDetails = Categories.FirstOrDefault(o => o.CategoryName.ToLower() == DataService.LATESTNEWS.ToLower());

                         foreach (var article in tempArticles.ToList())
                         {
                             LatestNews.Add(article);
                         }
                         IsRefreshingLatestNews = false;
                     });
                }
                else
                {
                    IsProblem = true;
                    IsRefreshingLatestNews = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion Private Methods
    }
}