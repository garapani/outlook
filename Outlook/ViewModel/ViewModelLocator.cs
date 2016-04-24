//using DotNetApp.Toolkit.Services;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Outlook.Services;
using System.ComponentModel;

namespace Outlook.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
                SimpleIoc.Default.Register<SettingsViewModel>();
                SimpleIoc.Default.Register<DataService>();
                SimpleIoc.Default.Register<NavigationService>();
                SimpleIoc.Default.Register<MainViewModel>();
                SimpleIoc.Default.Register<ArticleViewModel>(true);
                
                SimpleIoc.Default.Register<AboutViewModel>();
                //SimpleIoc.Default.Register<CategorySelectionViewModel>();
                SimpleIoc.Default.Register<CategoryPageViewModel>();
            }
        }

        #region Methods

        public DataService DataService
        {
            get { return ServiceLocator.Current.GetInstance<DataService>(); }
        }

        public MainViewModel MainViewModel
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public ArticleViewModel ArticleViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ArticleViewModel>(); }
        }

        public SettingsViewModel SettingsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<SettingsViewModel>(); }
        }

        public AboutViewModel AboutViewModel
        {
            get { return ServiceLocator.Current.GetInstance<AboutViewModel>(); }
        }

        //public CategorySelectionViewModel CategorySelectionViewModel
        //{
        //    get { return ServiceLocator.Current.GetInstance<CategorySelectionViewModel>(); }
        //}
        public CategoryPageViewModel CategoryPageViewModel
        {
            get { return ServiceLocator.Current.GetInstance<CategoryPageViewModel>(); }
        }

        public NavigationService NavigationService
        {
            get { return ServiceLocator.Current.GetInstance<NavigationService>(); }
        }

        public void Cleanup()
        {
            DataService.Save();
            _isSaved = true;
        }

        private bool _isSaved = false;

        public bool IsSaved()
        {
            return _isSaved;
        }

        #endregion Methods
    }
}