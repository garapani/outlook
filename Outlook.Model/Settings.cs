using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Outlook.Model
{
    public enum enumListOfThemes
    {
        Dark,
        Light
    }

    internal enum enumFontSize
    {
        small, //= 18.667,
        normal,// = 20,
        medium,// = 22.667,
        large,// = 32
    }

    internal enum enumFontFamily
    {
        Segoe_WP,
        Times_New_Roman,
        arial,
        Georgia,
        Calibri,
        verdana,
    }

    public class Settings : INotifyPropertyChanged
    {
        #region Fields

        private bool _isRefreshAutomatic;
        private bool _isDataSavingEnabled = false;
        private bool _isDownloadingArticleOffline = true;
        private bool _isToastNotificationUsed = true;
        private string _selectedTheme;

        #endregion Fields

        #region Properties

        public string SelectedTheme
        {
            get
            {
                return _selectedTheme;
            }
            set
            {
                if (_selectedTheme != value)
                {
                    _selectedTheme = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("SelectedTheme"));
                    }
                }
            }
        }

        private bool _isPhoneTheme;

        public bool IsPhoneTheme
        {
            get
            {
                return _isPhoneTheme;
            }
            set
            {
                if (_isPhoneTheme != value)
                {
                    _isPhoneTheme = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsPhoneTheme"));
                    }
                }
            }
        }

        private double _fontSize = 18;

        public double FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FontSize"));
                    }
                }
            }
        }

        private string _fontfamily = enumFontFamily.Calibri.ToString();

        public string FontFamily
        {
            get
            {
                return _fontfamily;
            }
            set
            {
                if (_fontfamily != value)
                {
                    _fontfamily = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FontFamily"));
                    }
                }
            }
        }

        private List<string> _themes;

        public List<string> Themes
        {
            get
            {
                if (_themes == null)
                    _themes = new List<string>();
                _themes.Clear();
                _themes.Add(enumListOfThemes.Dark.ToString());
                _themes.Add(enumListOfThemes.Light.ToString());
                return _themes;
            }
            private set
            {
                //if (PropertyChanged != null)
                //{
                //    PropertyChanged(this, new PropertyChangedEventArgs("Themes"));
                //}
            }
        }

        public bool IsRefreshAutomatic
        {
            get { return _isRefreshAutomatic; }

            set
            {
                if (_isRefreshAutomatic != value)
                {
                    _isRefreshAutomatic = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsRefreshAutomatic"));
                }
            }
        }

        public bool IsDataSavingEnabled
        {
            get { return _isDataSavingEnabled; }

            set
            {
                if (_isDataSavingEnabled != value)
                {
                    _isDataSavingEnabled = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsDataSavingEnabled"));
                }
            }
        }

        public bool IsDownloadingArticlesOffline
        {
            get { return _isDownloadingArticleOffline; }

            set
            {
                if (_isDownloadingArticleOffline != value)
                {
                    _isDownloadingArticleOffline = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsDownloadingArticleOffline"));
                }
            }
        }

        public bool IsToastNotificationUsed
        {
            get { return _isToastNotificationUsed; }

            set
            {
                if (_isToastNotificationUsed != value)
                {
                    _isToastNotificationUsed = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsToastNotificationUsed"));
                }
            }
        }

        private bool _isQuietHoursUsed = false;

        public bool IsQuietHoursUsed
        {
            get
            {
                return _isQuietHoursUsed;
            }
            set
            {
                if (_isQuietHoursUsed != value)
                {
                    _isQuietHoursUsed = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsQuietHoursUsed"));
                }
            }
        }

        private DateTime _quietHoursStartTime = DateTime.Now;

        public DateTime QuietHoursStartTime
        {
            get
            {
                return _quietHoursStartTime;
            }
            set
            {
                if (_quietHoursStartTime != value)
                {
                    _quietHoursStartTime = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("QuietHoursStartTime"));
                    }
                }
            }
        }

        private DateTime _quietHoursEndTime = DateTime.Now;

        public DateTime QuietHoursEndTime
        {
            get
            {
                return _quietHoursEndTime;
            }
            set
            {
                if (_quietHoursEndTime != value)
                {
                    _quietHoursEndTime = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("QuietHoursEndTime"));
                    }
                }
            }
        }

        private DateTime _lastRefreshedPhotosTime;

        public DateTime LastRefreshedPhotosTime
        {
            get
            {
                return _lastRefreshedPhotosTime;
            }
            set
            {
                if (_lastRefreshedPhotosTime != value)
                {
                    _lastRefreshedPhotosTime = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LastRefreshedPhotosTime"));
                    }
                }
            }
        }

        private DateTime _lastRefreshedLatestNewsTime;

        public DateTime LastRefreshedLatestNewsTime
        {
            get
            {
                return _lastRefreshedLatestNewsTime;
            }
            set
            {
                if (_lastRefreshedLatestNewsTime != value)
                {
                    _lastRefreshedLatestNewsTime = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LastRefreshedLatestNewsTime"));
                    }
                }
            }
        }

        private DateTime _lastRefreshedHeadLinesTime;

        public DateTime LastRefreshedHeadLinesTime
        {
            get
            {
                return _lastRefreshedHeadLinesTime;
            }
            set
            {
                if (_lastRefreshedHeadLinesTime != value)
                {
                    _lastRefreshedHeadLinesTime = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LastRefreshedHeadLinesTime"));
                    }
                }
            }
        }

        #endregion Properties

        #region Event Handlers

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Event Handlers
    }
}