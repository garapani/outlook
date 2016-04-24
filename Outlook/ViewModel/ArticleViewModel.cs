using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Phone.Tasks;
using Outlook.Helper;
using Outlook.Model;
using Outlook.Services;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Outlook.ViewModel
{
    public class ArticleViewModel : ViewModelBase
    {
        #region Fields

        private readonly DataService _dataService;
        #endregion Fields

        #region Constructor

        public ArticleViewModel(DataService dataService)
        {
            try
            {
                if (!DesignerProperties.IsInDesignTool)
                {
                    _dataService = dataService;
                    OpenInIeCommand = new RelayCommand(OpenInIe);
                    ShareEmailArticleCommand = new RelayCommand(ShareEmailArticle);
                    ShareArticleCommand = new RelayCommand(ShareArticle);
                    ShowSettingsCommand = new RelayCommand(ShowSettings);
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Constructor

        #region Properties

        private Article _article;
        public Article Article
        {
            get
            {
                return _article;
            }
            set
            {
                _article = value;
                RaisePropertyChanged("Article");
            }
        }

        private Article _previousArticle;
        public Article PreviousArticle
        {
            get
            {
                return _previousArticle;
            }
            set
            {
                _previousArticle = value;
                RaisePropertyChanged("PreviousArticle");
            }
        }

        private Article _nextArticle;
        public Article NextArticle
        {
            get
            {
                return _nextArticle;
            }
            set
            {
                _nextArticle = value;
                RaisePropertyChanged("NextArticle");
            }
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }
            set
            {
                _selectedCategory = value;
                RaisePropertyChanged("SelectedCategory");
            }
        }

        private bool _isFetchingArticle = true;
        public bool IsFetchingArticle
        {
            get
            {
                return _isFetchingArticle;
            }
            set
            {
                _isFetchingArticle = value;
                RaisePropertyChanged("IsFetchingArticle");
            }
        }

        public Settings Settings
        {
            get
            {
                return _dataService.Settings;
            }
            private set
            {
            }
        }

        public RelayCommand OpenInIeCommand { get; private set; }
        public RelayCommand ShareEmailArticleCommand { get; private set; }
        public RelayCommand ShareArticleCommand { get; private set; }
        public RelayCommand ShowSettingsCommand { get; private set; }

        #endregion Properties

        #region Commands

        private void ShareEmailArticle()
        {
            try
            {
                var storeURI = DeepLinkHelper.BuildApplicationDeepLink();
                EmailComposeTask emailComposeTask = new EmailComposeTask {
                    Subject = string.Format("Outlook India: {0}", Article.HeadLine),
                    Body = String.Format("Hi,\n\nThis article will interest you: {0}\n\n{1}\n\nSent by the \"Outlook India\" for Windows Phone 8.For the App you can click this link. " + storeURI, Article.HeadLine, Article.WebURL)};
                emailComposeTask.Show();
            }
            catch
            {
            }
        }

        private void ShowSettings()
        {
            try
            {
                App.RootFrame.Navigate(new Uri("/Views/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception)
            {
            }
        }

        private void ShareArticle()
        {
            try
            {
                ShareLinkTask shareLinkTask = new ShareLinkTask {
                    Title = "Read this article!",
                    LinkUri = new Uri(Article.WebURL, UriKind.Absolute),
                    Message = Article.HeadLine};
                shareLinkTask.Show();
            }
            catch (Exception)
            {
            }
        }

        private void OpenInIe()
        {
            if (!string.IsNullOrEmpty(Article.WebURL))
            {
                try
                {
                    var wbt = new WebBrowserTask();
                    string weburl = Article.WebURL.Replace("Outlook", "m.Outlook");
                    wbt.Uri = new Uri(Article.WebURL, UriKind.Absolute);
                    wbt.Show();
                }
                catch (Exception)
                {
                }
            }
        }

        public void SetCurrentPreviousandNextArticle(Article article)
        {
            if (article != null)
            {
                IsFetchingArticle = true;
                Article = article;
                _dataService.SetCurrentArticle(article);
                NextArticle = _dataService.GetNextArticle(article.Category);
                PreviousArticle = _dataService.GetPreviousArticle(article.Category);
                IsFetchingArticle = false;
            }
        }

        public async Task<Article> SetCurrentArticle(string headline, string category)
        {
            IsFetchingArticle = true;
            var temp = await _dataService.GetArticleAsync(headline, category);
            if (temp != null)
            {
                Article = temp;
                _dataService.SetCurrentArticle(temp);
                NextArticle = _dataService.GetNextArticle(category);
                PreviousArticle = _dataService.GetPreviousArticle(category);
            }
            IsFetchingArticle = false;
            return Article;
        }

        #endregion Commands

        #region Event Handlers

        public void ClearArticle()
        {
            Article = null;
        }

        #endregion Event Handlers
    }
}