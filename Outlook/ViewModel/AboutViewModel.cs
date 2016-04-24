using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Tasks;
using System.ComponentModel;

namespace Outlook.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        #region Constructor

        public AboutViewModel()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                WriteEmailCommand = new RelayCommand(WriteEmail);
                WriteReviewCommand = new RelayCommand(WriteReview);
            }
        }

        #endregion Constructor

        #region Properties

        public RelayCommand WriteEmailCommand { get; private set; }

        public RelayCommand WriteReviewCommand { get; private set; }

        #endregion Properties

        #region Command

        private void WriteEmail()
        {
            try
            {
                EmailComposeTask emailComposeTask = new EmailComposeTask
                {
                    To = "rob@Outlook.com",
                    Subject = "Outlook App",
                };
                emailComposeTask.Show();
            }
            catch
            {
            }
        }

        private void WriteReview()
        {
            try
            {
                MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
                marketplaceReviewTask.Show();
            }
            catch
            {
            }
        }

        #endregion Command
    }
}