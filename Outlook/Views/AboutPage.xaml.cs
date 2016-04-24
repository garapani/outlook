using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Windows;

namespace Outlook.Views
{
    public partial class AboutUs : PhoneApplicationPage
    {
        //private NavigationService _navigationService;
        public AboutUs()
        {
            //_navigationService = navigateService;
            InitializeComponent();
        }

        private void Rate_Click(object sender, RoutedEventArgs e)
        {
            var mp = new MarketplaceReviewTask();
            mp.Show();
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SharePage.xaml", UriKind.Relative));
        }

        private void Feedback_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmailComposeTask emailComposeTask = new EmailComposeTask
                {
                    To = "venkatachalapathi.g@outlook.com",
                    Subject = "Feedback on Outlook India App",
                };
                emailComposeTask.Show();
            }
            catch
            {
            }
        }

        private void MoreApps_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var search = new MarketplaceSearchTask();
                search.ContentType = MarketplaceContentType.Applications;
                search.SearchTerms = "The Village Software";
                search.Show();
            }
            catch (Exception)
            {
            }
        }

        private void MyApps_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/MyAppsPage.xaml", UriKind.Relative));
        }
    }
}