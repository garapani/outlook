using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using Outlook.Client;
using Outlook.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Outlook.Agent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        #region Constants

        private const string LastUpdateVerificationPropertyName = "LastUpdateVerification";
        private const string _latestNews = "LATEST NEWS";

        #endregion Constants

        #region Fields

        private static volatile bool _classInitialized;

        #endregion Fields

        #region Constructor

        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;

                Deployment.Current.Dispatcher.BeginInvoke(delegate { Application.Current.UnhandledException += ScheduledAgent_UnhandledException; });
            }
        }

        #endregion Constructor

        #region Event Handlers

        protected override async void OnInvoke(ScheduledTask task)
        {
            try
            {
                var lastCheck = (DateTime)GetApplicationSettingValue(LastUpdateVerificationPropertyName, new DateTime(1, 1, 1));
                if (lastCheck != null)
                {
                    if (lastCheck.Year == 1)
                    {
                        SaveSettingValueImmediately(LastUpdateVerificationPropertyName, DateTime.Now);
                    }
                    else
                    {
                        if (IsUpdateRequired())
                        {
                            SaveSettingValueImmediately(LastUpdateVerificationPropertyName, DateTime.Now);
                            await UpdateAsync();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        #endregion Event Handlers

        private bool IsUpdateRequired()
        {
            var lastCheck = (DateTime)GetApplicationSettingValue(LastUpdateVerificationPropertyName, DateTime.Now);
            if (lastCheck != null)
            {
                return (DateTime.Now - lastCheck).TotalHours >= 1;
            }
            else
            {
                return false;
            }
        }

        private async Task UpdateAsync()
        {
            try
            {
                //BackgroundWorker bwLatestNews = new BackgroundWorker();
                //bwLatestNews.DoWork += bwLatestNews_DoWork;
                //bwLatestNews.RunWorkerAsync();
                try
                {
                    OutlookClient client = new OutlookClient();
                    var articles = await client.GetArticlesAsync(_latestNews);
                    if (articles != null && articles.Count > 0)
                    {
                        articles = articles.OrderByDescending(o => o.ArticleDate).ToList();
                        SaveLatestNews(articles);
                        UpdateNotifications(articles.Count, articles[0]);
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        private async void bwLatestNews_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                OutlookClient client = new OutlookClient();
                var articles = await client.GetArticlesAsync(_latestNews);
                if (articles != null && articles.Count > 0)
                {
                    articles = articles.OrderByDescending(o => o.ArticleDate).ToList();
                    SaveLatestNews(articles);
                    UpdateNotifications(articles.Count, articles[0]);
                }
            }
            catch (Exception)
            {
            }
        }

        private void UpdateNotifications(int newArticlesCount, Article newArticle)
        {
            try
            {
                if (newArticle != null)
                {
                    Settings settings = (Settings)GetApplicationSettingValue("Settings", new Settings());
                    if (settings == null)
                    {
                        settings = new Settings();
                    }

                    string lastNotifiedArticle = (string)GetApplicationSettingValue("LastNotifiedArticle", "");
                    if (newArticle.HeadLine != lastNotifiedArticle)
                    {
                        newArticlesCount = (int)GetApplicationSettingValue("LastNewArticlesCount", 0) + newArticlesCount;
                        SaveSettingValueImmediately("LastNotifiedArticle", newArticle.HeadLine);

                        try
                        {
                            UpdateLiveTile(newArticle);
                        }
                        catch (Exception)
                        {
                        }

                        SaveSettingValueImmediately("LastNewArticlesCount", newArticlesCount);
                        if (settings.IsToastNotificationUsed == true)
                        {
                            bool canNotify = true;
                            if (settings.IsQuietHoursUsed == true)
                            {
                                if ((settings.QuietHoursStartTime.TimeOfDay < DateTime.Now.TimeOfDay) && (DateTime.Now.TimeOfDay < settings.QuietHoursEndTime.TimeOfDay))
                                {
                                    canNotify = false;
                                }
                            }

                            if (canNotify)
                            {
                                try
                                {
                                    ShellToast shellToast = new ShellToast();
                                    shellToast.Content = newArticle.HeadLine;
                                    shellToast.Title = "Outlook India";
                                    shellToast.NavigationUri = new Uri(string.Format("/Views/MainPage.xaml?Id={0}&Category={1}&Toast={2}", newArticle.HeadLine, _latestNews, true), UriKind.Relative);
                                    shellToast.Show();
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                NotifyComplete();
            }
        }

        private void UpdateLiveTile(Article newArticle)
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
            catch
            {
            }

            return result;
        }

        private static void SaveSettingValueImmediately(string propertyName, object value)
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings[propertyName] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            catch
            {
            }
        }

        private async void SaveLatestNews(List<Article> articles)
        {
            if (articles != null && articles.Count > 0)
            {
                foreach (var article in articles)
                {
                    await DatabaseOperations.GetInstance().AddOrUpdateArticleAsync(article);
                }
            }
        }
    }
}