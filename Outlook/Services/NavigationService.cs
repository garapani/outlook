using Microsoft.Phone.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;

namespace Outlook.Services
{
    public class NavigationService
    {
        #region Fields

        private PhoneApplicationFrame _phoneApplicationFrame;

        #endregion Fields

        #region Methods

        public void NavigateTo(Uri pageUri)
        {
            EnsurePhotoApplicationFrame();

            _phoneApplicationFrame.Navigate(pageUri);
        }

        public void GoBack()
        {
            EnsurePhotoApplicationFrame();

            if (_phoneApplicationFrame.CanGoBack)
            {
                _phoneApplicationFrame.GoBack();
            }
        }

        public void RemoveBackEntry()
        {
            _phoneApplicationFrame.RemoveBackEntry();
        }

        public string GetCurrentPage()
        {
            return _phoneApplicationFrame.BackStack.First().Source.OriginalString;
        }

        #endregion Methods

        #region Events

        public event NavigatingCancelEventHandler Navigating;

        #endregion Events

        #region Private Methods

        private void EnsurePhotoApplicationFrame()
        {
            if (_phoneApplicationFrame == null)
            {
                _phoneApplicationFrame = Application.Current.RootVisual as PhoneApplicationFrame;

                if (_phoneApplicationFrame != null)
                {
                    // Could be null if the app runs inside a design tool
                    _phoneApplicationFrame.Navigating += (s, e) =>
                                                             {
                                                                 if (Navigating != null)
                                                                 {
                                                                     Navigating(s, e);
                                                                 }
                                                             };
                }
            }
        }

        #endregion Private Methods
    }
}