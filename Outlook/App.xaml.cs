using BugSense;
using BugSense.Core.Model;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Outlook.Services;
using Outlook.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Xml;

namespace Outlook
{
    public partial class App
    {
        #region Fields

        private bool _isPhoneApplicationInitialized;
        private static string _version;

        #endregion Fields

        #region Constructors

        public App()
        {
            try
            {
                BugSenseHandler.Instance.InitAndStartSession(new ExceptionManager(Current), RootFrame, "33c9a891");
                UnhandledException += App_UnhandledException;
            }
            catch (Exception)
            {
            }
            //BugSenseHandler.Instance.Init(this, "d17bb886");
            UnhandledException += ApplicationUnhandledException;

            InitializeComponent();
            InitializePhoneApplication();

            TiltEffect.SetIsTiltEnabled(RootFrame, true);
            DispatcherHelper.Initialize();
        }

        private async void App_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                LimitedCrashExtraDataList extrasExtraDataList = new LimitedCrashExtraDataList
                {
                    new CrashExtraData("Outlook India", e.ExceptionObject.Message),
                    new CrashExtraData("Outlook India", e.ExceptionObject.StackTrace),
                };

                BugSenseResponseResult sendResult = await BugSenseHandler.Instance.SendExceptionAsync(e.ExceptionObject, extrasExtraDataList);
            }
            catch (Exception)
            {
            }
        }

        #endregion Constructors

        #region Properties

        public static PhoneApplicationFrame RootFrame { get; private set; }

        public static string Version
        {
            get
            {
                if (_version == null)
                {
                    try
                    {
                        string assemblyName = typeof(App).Assembly.ToString();

                        // ReSharper disable StringIndexOfIsCultureSpecific.1
                        if (assemblyName.IndexOf("Version=") >= 0)
                        {
                            assemblyName = assemblyName.Substring(assemblyName.IndexOf("Version=") + "Version=".Length);
                            assemblyName = assemblyName.Substring(0, assemblyName.IndexOf(","));

                            string[] versions = assemblyName.Split('.');

                            _version = string.Format("{0}.{1}", versions[0], versions[1]);
                        }
                        // ReSharper restore StringIndexOfIsCultureSpecific.1
                    }
                    catch
                    {
                    }
                }

                return _version;
            }
        }

        #endregion Properties

        #region Event Handlers

        private void ApplicationLaunching(object sender, LaunchingEventArgs e)
        {
            try
            {
                GoogleAnalytics.EasyTracker.GetTracker().SendView("Launch");
            }
            catch (Exception)
            {
            }

            ViewModelLocator viewModelLocator = Current.Resources["Locator"] as ViewModelLocator;

            try
            {
                if (viewModelLocator != null)
                {
                    //viewModelLocator.DataService.LoadArticles(DataService._latestNews);
                }
            }
            catch (Exception)
            {
            }

            try
            {
                if (viewModelLocator != null)
                {
                    viewModelLocator.DataService.Load();
                }
            }
            catch (Exception)
            {
            }
        }

        private void ApplicationActivated(object sender, ActivatedEventArgs e)
        {
            if (!e.IsApplicationInstancePreserved)
            {
                ViewModelLocator viewModelLocator = Current.Resources["Locator"] as ViewModelLocator;

                try
                {
                    if (viewModelLocator != null)
                    {
                        //viewModelLocator.DataService.LoadArticles(DataService._latestNews);
                    }
                }
                catch (Exception)
                {
                }

                try
                {
                    if (viewModelLocator != null)
                    {
                        viewModelLocator.DataService.Load();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void ApplicationDeactivated(object sender, DeactivatedEventArgs e)
        {
            ViewModelLocator viewModelLocator = Current.Resources["Locator"] as ViewModelLocator;

            if (viewModelLocator != null && !viewModelLocator.IsSaved())
            {
                viewModelLocator.Cleanup();
            }
        }

        private void ApplicationClosing(object sender, ClosingEventArgs e)
        {
            ViewModelLocator viewModelLocator = Current.Resources["Locator"] as ViewModelLocator;

            if (viewModelLocator != null && !viewModelLocator.IsSaved())
            {
                viewModelLocator.Cleanup();
            }
        }

        private void OnRootFrameNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        private void OnCompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            RootVisual = RootFrame;

            RootFrame.Navigated -= OnCompleteInitializePhoneApplication;
        }

        private static void ApplicationUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (e != null)
            {
                Exception exception = e.ExceptionObject;

                if ((exception is XmlException || exception is NullReferenceException) && exception.ToString().ToUpper().Contains("INNERACTIVE"))
                {
                    Debug.WriteLine("Handled Inneractive exception {0}", exception);
                    e.Handled = true;
                    return;
                }
                else if (exception is NullReferenceException && exception.ToString().ToUpper().Contains("SOMA"))
                {
                    Debug.WriteLine("Handled Smaato null reference exception {0}", exception);
                    e.Handled = true;
                    return;
                }
                else if ((exception is System.IO.IOException || exception is NullReferenceException) && exception.ToString().ToUpper().Contains("GOOGLE"))
                {
                    Debug.WriteLine("Handled Google exception {0}", exception);
                    e.Handled = true;
                    return;
                }
                else if (exception is ObjectDisposedException && exception.ToString().ToUpper().Contains("MOBFOX"))
                {
                    e.Handled = true;
                    return;
                }
                else if ((exception is NullReferenceException) && exception.ToString().ToUpper().Contains("MICROSOFT.ADVERTISING"))
                {
                    e.Handled = true;
                    return;
                }
                else
                {
                    try
                    {
                        GoogleAnalytics.EasyTracker.GetTracker().SendException(e.ExceptionObject.Message + e.ExceptionObject.StackTrace, false);
                    }
                    catch (Exception ex)
                    {
                        if (Debugger.IsAttached)
                        {
                            Debug.WriteLine("App.xaml.cs:" + ex);
                        }
                    }
                }
            }
        }

        #endregion Event Handlers

        #region Private Methods

        private void InitializePhoneApplication()
        {
            if (!_isPhoneApplicationInitialized)
            {
                RootFrame = new PhoneApplicationFrame();
                //ImageBrush bkgnd = new ImageBrush();
                //bkgnd.ImageSource = new BitmapImage(new Uri(@"/Assets/Icons/Mainpagebackground.png", UriKind.RelativeOrAbsolute));
                RootFrame.Background = new SolidColorBrush(Colors.White);
                RootFrame.Navigated += OnCompleteInitializePhoneApplication;
                RootFrame.NavigationFailed += OnRootFrameNavigationFailed;

                _isPhoneApplicationInitialized = true;
            }
        }

        #endregion Private Methods
    }
}