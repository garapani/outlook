using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace Outlook.Views
{
    public partial class NetworkError : PhoneApplicationPage
    {
        public NetworkError()
        {
            InitializeComponent();
            //SetupNavigationTransitions();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            base.OnNavigatedFrom(e);
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