using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Outlook.ViewModel;
using System.Windows.Navigation;

namespace Outlook.Views
{
    public partial class CategoryPage : PhoneApplicationPage
    {
        private MainViewModel _mainViewModel;

        public CategoryPage()
        {
            InitializeComponent();
            _mainViewModel = DataContext as MainViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string category = NavigationContext.QueryString["category"];
            this.PageName.Text = category;
            _mainViewModel.GetCategoryArticlesAsync(category);
            base.OnNavigatedTo(e);
        }
    }
}