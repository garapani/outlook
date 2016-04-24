using GalaSoft.MvvmLight;
using Outlook.Model;
using System.IO.IsolatedStorage;

namespace Outlook.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Constructor

        //public SettingsViewModel(DataService dataService)
        //{
        //    if (!DesignerProperties.IsInDesignTool)
        //    {
        //        Settings = dataService.Settings;
        //    }
        //}
        public SettingsViewModel()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("Settings"))
            {
                Settings = (Settings)IsolatedStorageSettings.ApplicationSettings["Settings"];
            }
            else
            {
                Settings = new Settings();
            }
        }

        #endregion Constructor

        #region Properties

        public Settings Settings { get; private set; }

        #endregion Properties
    }
}