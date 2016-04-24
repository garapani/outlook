using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Outlook.Model;
using Outlook.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Outlook.ViewModel
{
    public class CategoryPageViewModel : ViewModelBase
    {
        private readonly DataService _dataService;

        //private readonly NavigationService _navigationService;
        private string _selectedCategory;

        public RelayCommand LoadCategoryArticles { get; private set; }

        public Article CurrentArticle { get; private set; }

        public RelayCommand ReadCurrentArticleCommand { get; private set; }

        public RelayCommand RefreshCommand { get; private set; }

        public RelayCommand ShowSettingsCommand { get; private set; }

        public CategoryPageViewModel(DataService dataService)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                _dataService = dataService;
                IsRefreshingArticles = true;
            }
        }

      
        private void ShowSettingsPage()
        {
            App.RootFrame.Navigate(new Uri("/Views/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
        }

       
        public List<Article> Articles { get; private set; }

        public bool HasInternet { get; private set; }

        public bool IsCachedModeMessageDisplayed { get; set; }

        public bool IsRefreshingArticles { get; set; }


           }
}