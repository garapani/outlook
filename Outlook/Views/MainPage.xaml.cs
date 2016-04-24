using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Outlook.Services;
using Outlook.ViewModel;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace Outlook.Views
{
    public partial class MainPage
    {
        #region Constructor

        public MainPage()
        {
            try
            {
                InitializeComponent();
                FeedbackOverlay.VisibilityChanged += FeedbackOverlay_VisibilityChanged;
                //SetupNavigationTransitions();
            }
            catch (Exception)
            {
            }
        }

        #endregion Constructor

        private void FeedbackOverlay_VisibilityChanged(object sender, EventArgs e)
        {
            //ApplicationBar.IsVisible = (FeedbackOverlay.Visibility != Visibility.Visible);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                string id, category;
                NavigationContext.QueryString.TryGetValue("Id", out id);
                NavigationContext.QueryString.TryGetValue("Category", out category);
                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(category))
                {
                    NavigationService.Navigate(new Uri(string.Format("/Views/ArticlePage.xaml?Id={0}&Category={1}", id, category), UriKind.RelativeOrAbsolute));
                }
            }
        }

        private void Pivot_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                ApplicationBar.IsVisible = true;
                ApplicationBar.Mode = ApplicationBarMode.Minimized;
                ViewModelLocator viewModelLocator = App.Current.Resources["Locator"] as ViewModelLocator;
                if (viewModelLocator != null && viewModelLocator.MainViewModel != null)
                {
                    switch ((sender as Pivot).SelectedIndex)
                    {
                        case 0:
                            {
                                viewModelLocator.MainViewModel.ActiveCategory = DataService.LATESTNEWS;
                            }
                            break;
                        case 1:
                            {
                                viewModelLocator.MainViewModel.ActiveCategory = DataService.PHOTOS;
                            }
                            break;

                        default:
                            {
                                viewModelLocator.MainViewModel.ActiveCategory = null;
                                //adStackPanel.Visibility = Visibility.Collapsed;
                                ApplicationBar.IsVisible = false;
                                break;
                            }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void RemoveAds_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/RemoveAds.xaml", UriKind.Relative));
        }

        private void MyAdRotatorControl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SettingsClass settings = App.Current.Resources["settings"] as SettingsClass;
            if (settings != null)
            {
                settings.LastTimeAdClicked = DateTime.Now;
            }
        }

        private void MyAdRotatorControl_AdRotatorReady()
        {
        }

        //private void MyAdRotatorControl_AdRotatorReady()
        //{
        //    SettingsClass settings = App.Current.Resources["settings"] as SettingsClass;
        //    if (settings != null)
        //    {
        //        var diff = DateTime.Now - settings.LastTimeAdClicked;
        //        if (diff.TotalHours > 12)
        //        {
        //            MyAdRotatorControl.Visibility = System.Windows.Visibility.Visible;
        //        }
        //        else
        //        {
        //            MyAdRotatorControl.Visibility = System.Windows.Visibility.Collapsed;
        //        }
        //    }
        //}
    }
}