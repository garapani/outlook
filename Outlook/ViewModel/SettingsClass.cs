using System;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using Utilities;

namespace Outlook.ViewModel
{
    public enum enumListOfThemes
    {
        Dark,
        Light
    }

    internal enum enumFontSize
    {
        small = 19,
        normal = 22,
        medium = 25,
        large = 32,
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

    public class SettingsClass : INotifyPropertyChanged
    {
        private IsolatedStorageSettings _isolatedStorageSettings;

        public SettingsClass()
        {
            _isolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings;
        }

        //private static Settings _instance;
        //public static Settings GetInstance()
        //{
        //    if(_instance == null)
        //    {
        //        _instance = new Settings();
        //    }
        //    return _instance;
        //}

        private readonly object _readLock = new object();

        public void AddOrUpdateValue(string Key, Object value)
        {
            IsolatedStorageHelper.SaveSettingValueImmediately(Key, value);
        }

        public void Save()
        {
            _isolatedStorageSettings.Save();
        }

        #region Properties

        public string SelectedTheme
        {
            get
            {
                return IsolatedStorageHelper.GetString("SelectedTheme", enumListOfThemes.Dark.ToString());
            }
            set
            {
                AddOrUpdateValue("SelectedTheme", value);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedTheme"));
                }
            }
        }

        public bool IsPhoneTheme
        {
            get
            {
                return IsolatedStorageHelper.GetBool("IsPhoneTheme", true);
            }
            set
            {
                AddOrUpdateValue("IsPhoneTheme", value);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsPhoneTheme"));
                }
            }
        }

        public double FontSize
        {
            get
            {
                return IsolatedStorageHelper.GetFloat("FontSize", 22.667F);
            }
            set
            {
                AddOrUpdateValue("FontSize", value);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FontSize"));
                }
            }
        }

        public string FontFamily
        {
            get
            {
                return IsolatedStorageHelper.GetString("FontFamily", enumFontFamily.Calibri.ToString());
            }
            set
            {
                AddOrUpdateValue("FontFamily", value);

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FontFamily"));
                }
            }
        }

        //private List<string> _themes;
        //public List<string> Themes
        //{
        //    get
        //    {
        //        if (_themes == null)
        //            _themes = new List<string>();
        //        _themes.Clear();
        //        _themes.Add(enumListOfThemes.Dark.ToString());
        //        _themes.Add(enumListOfThemes.Light.ToString());
        //        return _themes;
        //    }
        //   private set
        //    {
        //        //if (PropertyChanged != null)
        //        //{
        //        //    PropertyChanged(this, new PropertyChangedEventArgs("Themes"));
        //        //}
        //    }
        //}

        public bool IsRefreshAutomatic
        {
            get
            {
                return IsolatedStorageHelper.GetBool("IsRefreshAutomatic", false);
            }
            set
            {
                AddOrUpdateValue("IsRefreshAutomatic", value);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsRefreshAutomatic"));
                }
            }
        }

        public bool IsDataSavingEnabled
        {
            get
            {
                return IsolatedStorageHelper.GetBool("IsDataSavingEnabled", false);
            }
            set
            {
                AddOrUpdateValue("IsDataSavingEnabled", value);

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsDataSavingEnabled"));
                }
            }
        }

        public bool IsDownloadingArticlesOffline
        {
            get
            {
                return IsolatedStorageHelper.GetBool("IsDownloadingArticlesOffline", true);
            }
            set
            {
                AddOrUpdateValue("IsDownloadingArticlesOffline", value);

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsDownloadingArticlesOffline"));
                }
            }
        }

        public bool IsToastNotificationUsed
        {
            get
            {
                return IsolatedStorageHelper.GetBool("IsToastNotificationUsed", true);
            }
            set
            {
                AddOrUpdateValue("IsToastNotificationUsed", value);

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsToastNotificationUsed"));
                }
            }
        }

        public DateTime LastTimeAdClicked
        {
            get
            {
                return IsolatedStorageHelper.GetDateTime("LastTimeAdClicked", DateTime.Now.AddDays(-2));
            }
            set
            {
                AddOrUpdateValue("LastTimeAdClicked", value);

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("LastTimeAdClicked"));
                }
            }
        }

        public DateTime LastRefreshedPhotosTime
        {
            get
            {
                return IsolatedStorageHelper.GetDateTime("LastRefreshedPhotosTime", DateTime.Now);
            }
            set
            {
                AddOrUpdateValue("LastRefreshedPhotosTime", value);

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("LastRefreshedPhotosTime"));
                }
            }
        }

        public DateTime LastRefreshedLatestNewsTime
        {
            get
            {
                return IsolatedStorageHelper.GetDateTime("LastRefreshedLatestNewsTime", DateTime.Now);
            }
            set
            {
                AddOrUpdateValue("LastRefreshedLatestNewsTime", value);

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("LastRefreshedLatestNewsTime"));
                }
            }
        }

        public DateTime LastRefreshedHeadLinesTime
        {
            get
            {
                return IsolatedStorageHelper.GetDateTime("LastRefreshedHeadLinesTime", DateTime.Now);
            }
            set
            {
                AddOrUpdateValue("LastRefreshedHeadLinesTime", value);

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("LastRefreshedHeadLinesTime"));
                }
            }
        }

        public string SelectedBackground
        {
            get
            {
                return IsolatedStorageHelper.GetString("SelectedBackground", "White");
            }
            set
            {
                AddOrUpdateValue("SelectedBackground", value);

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedBackground"));
                }
            }
        }

        public string SelectedForeground
        {
            get
            {
                return IsolatedStorageHelper.GetString("SelectedForeground", "Black");
            }
            set
            {
                AddOrUpdateValue("SelectedForeground", value);

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedForeground"));
                }
            }
        }

        #endregion Properties

        #region Event Handlers

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Event Handlers
    }
}