using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Outlook.Helper;
using System;

namespace Outlook
{
    public partial class SharePage : PhoneApplicationPage
    {
        public SharePage()
        {
            InitializeComponent();
            //SetupNavigationTransitions();
        }

        private void ShareViaSocialNetwork_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.Title = "Outlook India";
            var storeURI = DeepLinkHelper.BuildApplicationDeepLink();
            //await Windows.System.Launcher.LaunchUriAsync(storeURI);

            //shareLinkTask.LinkUri = new Uri("http://msdn.microsoft.com/en-us/library/windowsphone/develop/ff431744(v=vs.92).aspx", UriKind.Absolute);
            shareLinkTask.LinkUri = new Uri(storeURI);
            shareLinkTask.Message = "\"Outlook India App\" for Windows phone 8";
            shareLinkTask.Show();
        }

        private void ShareViaMail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var storeURI = DeepLinkHelper.BuildApplicationDeepLink();
            EmailComposeTask emailComposeTask = new EmailComposeTask()
            {
                Subject = "Try \"Outlook India App\" for windows phone 8",
                Body = "\"Outlook India App\" is helps you in reading news from outlook india magazine.Please click on this link " + storeURI,
            };

            emailComposeTask.Show();
        }

        private void ShareViaSMS_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var storeURI = DeepLinkHelper.BuildApplicationDeepLink();
            SmsComposeTask smsComposeTask = new SmsComposeTask()
            {
                Body = "Try \"Outlook India App\" for windows phone 8. It's great!. " + storeURI,
            };

            smsComposeTask.Show();
        }

        private void SetupNavigationTransitions()
        {
            NavigationInTransition navigateInTransition = new NavigationInTransition();
            navigateInTransition.Backward = new SlideTransition { Mode = SlideTransitionMode.SlideRightFadeIn };
            navigateInTransition.Forward = new SlideTransition { Mode = SlideTransitionMode.SlideLeftFadeIn };

            NavigationOutTransition navigateOutTransition = new NavigationOutTransition();
            navigateOutTransition.Backward = new SlideTransition { Mode = SlideTransitionMode.SlideRightFadeOut };
            navigateOutTransition.Forward = new SlideTransition { Mode = SlideTransitionMode.SlideLeftFadeOut };
            TransitionService.SetNavigationInTransition(this, navigateInTransition);
            TransitionService.SetNavigationOutTransition(this, navigateOutTransition);
        }
    }
}