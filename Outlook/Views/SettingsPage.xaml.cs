using Microsoft.Phone.Controls;
using Outlook.ViewModel;
using System;
using System.Windows;

namespace Outlook.Views
{
    public enum enumListOfThemes
    {
        Dark,
        Light
    }

    internal enum enumFontFamily
    {
        Segoe_WP,
        Pescadero,
        arial,
        Georgia,
        Calibri,
        verdana,
    }

    public partial class SettingsPage
    {
        private const double FontSmall = 18.667;// = 18.667,
        private const double Fontnormal = 20;// = 20,
        private const double Fontmedium = 22.667;// = 22.667,
        private const double Fontlarge = 32;// = 32

        //private SettingsViewModel _settingViewModel;
        private SettingsClass _settings;

        #region Constructor

        public SettingsPage()
        {
            InitializeComponent();
            _settings = App.Current.Resources["settings"] as SettingsClass;
            //SetupNavigationTransitions();
            //if (_settingViewModel != null)
            //{
            //    if (_settings != null && !string.IsNullOrEmpty(_settings.SelectedTheme))
            //    {
            //        if (_settings.SelectedTheme == enumListOfThemes.Dark.ToString())
            //        {
            //            SelectedDarkTheme.IsChecked = true;
            //            SelectedLightTheme.IsChecked = false;
            //        }
            //        else
            //        {
            //            SelectedDarkTheme.IsChecked = false;
            //            SelectedLightTheme.IsChecked = true;
            //        }
            //    }
            //    else
            //    {
            //        SelectedLightTheme.IsChecked = true;
            //        SelectedDarkTheme.IsChecked = false;
            //    }
            //}
        }

        #endregion Constructor

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //RadioButton li = (sender as RadioButton);
            //if (li.Content.ToString() == enumListOfThemes.Dark.ToString())
            //{
            //    _settings.SelectedTheme = enumListOfThemes.Dark.ToString();

            //    ThemeManager.ToDarkTheme();
            //}
            //else
            //{
            //    _settings.SelectedTheme = enumListOfThemes.Light.ToString();
            //    ThemeManager.ToLightTheme();
            //}
        }

        private void IsDataSavingEnabled_Checked(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("You must relaunch this app to apply changes", "Data Saving Mode", MessageBoxButton.OK);
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            startTime.Value = DateTime.Now;
            endTime.Value = DateTime.Now;
        }
    }
}