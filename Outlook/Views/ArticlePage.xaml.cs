using GalaSoft.MvvmLight.Threading;
using Microsoft.Phone.Controls;
using Outlook.Model;
using Outlook.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Outlook.Views
{
    public partial class ArticlePage
    {
        #region Fields

        private readonly ArticleViewModel _articleViewModel;

        private System.Windows.Threading.Dispatcher dispatcher { get; set; }

        private SettingsClass settings = null;

        #endregion Fields

        #region Constructor

        public ArticlePage()
        {
            InitializeComponent();
            _articleViewModel = DataContext as ArticleViewModel;

            if (App.Current.Resources.Contains("settings"))
            {
                settings = App.Current.Resources["settings"] as SettingsClass;
                if (settings == null)
                    settings = new SettingsClass();
            }
            else
            {
                settings = new SettingsClass();
            }
        }

        #endregion Constructor

        #region PhoneApplicationPage Overrides

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            bool isToastNotificationHandle = false;
            if (_articleViewModel != null)
            {
                if (e.NavigationMode == NavigationMode.New)
                {
                    string headline, category, toast;
                    NavigationContext.QueryString.TryGetValue("Id", out headline);
                    NavigationContext.QueryString.TryGetValue("Category", out category);
                    NavigationContext.QueryString.TryGetValue("Toast", out toast);
                    Article article = null;
                    if (!string.IsNullOrEmpty(headline) && !string.IsNullOrEmpty(category))
                    {
                        onePivot.DataContext = _articleViewModel.Article = await _articleViewModel.SetCurrentArticle(headline, category);
                        UpdateStory();
                        UpdateFontStyleButtons();
                        if (_articleViewModel.Article == null)
                            NavigationService.GoBack();
                    }
                }
                else
                {
                    if (_articleViewModel.Article != null)
                    {
                        string id = _articleViewModel.Article.HeadLine;
                        string category = _articleViewModel.Article.Category;
                        onePivot.DataContext = _articleViewModel.Article = await _articleViewModel.SetCurrentArticle(id, category);
                        UpdateStory();
                        UpdateFontStyleButtons();
                    }
                    else
                    {
                        NavigationService.GoBack();
                    }
                }
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            previousIndex = 0;
            base.OnNavigatingFrom(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            previousIndex = 0;
            base.OnNavigatedFrom(e);
            _articleViewModel.PreviousArticle = null;
            _articleViewModel.NextArticle = null;
        }

        #endregion PhoneApplicationPage Overrides

        #region Private Methods

        private void UpdateAppBar()
        {
        }

        private void InitializePage()
        {
        }

        #endregion Private Methods

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            fontStyleSelection.Visibility = System.Windows.Visibility.Visible;
            ApplicationBar.IsVisible = false;
        }

        private void ContentPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            fontStyleSelection.Visibility = System.Windows.Visibility.Collapsed;
            ApplicationBar.IsVisible = true;
        }

        private void btnSmallFont_Click(object sender, RoutedEventArgs e)
        {
            settings.FontSize = 18.667;
            UpdateAllStories();
            btnSmallFont.Background = new SolidColorBrush(Colors.LightGray);
            btnNormalFont.Background = new SolidColorBrush(Colors.Transparent);
            btnMediumFont.Background = new SolidColorBrush(Colors.Transparent);
            btnMediumLargeFont.Background = new SolidColorBrush(Colors.Transparent);
            btnLargeFont.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void btnNormalFont_Click(object sender, RoutedEventArgs e)
        {
            settings.FontSize = 20;
            UpdateAllStories();
            btnSmallFont.Background = new SolidColorBrush(Colors.Transparent);
            btnNormalFont.Background = new SolidColorBrush(Colors.LightGray);
            btnMediumFont.Background = new SolidColorBrush(Colors.Transparent);
            btnMediumLargeFont.Background = new SolidColorBrush(Colors.Transparent);
            btnLargeFont.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void btnMediumFont_Click(object sender, RoutedEventArgs e)
        {
            settings.FontSize = 22.667;
            UpdateAllStories();
            btnSmallFont.Background = new SolidColorBrush(Colors.Transparent);
            btnNormalFont.Background = new SolidColorBrush(Colors.Transparent);
            btnMediumFont.Background = new SolidColorBrush(Colors.LightGray);
            btnMediumLargeFont.Background = new SolidColorBrush(Colors.Transparent);
            btnLargeFont.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void btnMediumLargeFont_Click(object sender, RoutedEventArgs e)
        {
            settings.FontSize = 25.333;
            UpdateAllStories();
            btnSmallFont.Background = new SolidColorBrush(Colors.Transparent);
            btnNormalFont.Background = new SolidColorBrush(Colors.Transparent);
            btnMediumFont.Background = new SolidColorBrush(Colors.Transparent);
            btnMediumLargeFont.Background = new SolidColorBrush(Colors.LightGray);
            btnLargeFont.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void btnLargeFont_Click(object sender, RoutedEventArgs e)
        {
            settings.FontSize = 32;
            UpdateAllStories();
            btnSmallFont.Background = new SolidColorBrush(Colors.Transparent);
            btnNormalFont.Background = new SolidColorBrush(Colors.Transparent);
            btnMediumFont.Background = new SolidColorBrush(Colors.Transparent);
            btnMediumLargeFont.Background = new SolidColorBrush(Colors.Transparent);
            btnLargeFont.Background = new SolidColorBrush(Colors.LightGray);
        }

        private void UpdateFontStyleButtons()
        {
            btnSmallFont.Background = new SolidColorBrush(Colors.Transparent);
            btnNormalFont.Background = new SolidColorBrush(Colors.Transparent);
            btnMediumFont.Background = new SolidColorBrush(Colors.Transparent);
            btnMediumLargeFont.Background = new SolidColorBrush(Colors.Transparent);
            btnLargeFont.Background = new SolidColorBrush(Colors.Transparent);

            if (settings.FontSize == 18.667)
            {
                btnSmallFont.Background = new SolidColorBrush(Colors.LightGray);
            }
            else if (settings.FontSize == 20)
            {
                btnNormalFont.Background = new SolidColorBrush(Colors.LightGray);
            }
            else if (settings.FontSize == 22.667)
            {
                btnMediumFont.Background = new SolidColorBrush(Colors.LightGray);
            }
            else if (settings.FontSize == 25.333)
            {
                btnMediumLargeFont.Background = new SolidColorBrush(Colors.LightGray);
            }
            else if (settings.FontSize == 32)
            {
                btnLargeFont.Background = new SolidColorBrush(Colors.LightGray);
            }
            else
            {
                settings.FontSize = 22.667;
                btnMediumFont.Background = new SolidColorBrush(Colors.LightGray);
            }

            btnCalibriFont.Background = new SolidColorBrush(Colors.Transparent);
            btnSegoeWPFont.Background = new SolidColorBrush(Colors.Transparent);
            btnOpenSans.Background = new SolidColorBrush(Colors.Transparent);

            btnCalibriFont.FontFamily = new System.Windows.Media.FontFamily("/Assets/Fonts/ArbutusSlab-regular.ttf#Arbutus Slab");
            btnSegoeWPFont.FontFamily = new FontFamily("Segoe WP");
            btnOpenSans.FontFamily = new FontFamily("/Assets/Fonts/roboto.ttf#roboto");
            if (settings.FontFamily == "Segoe WP")
            {
                btnSegoeWPFont.Background = new SolidColorBrush(Colors.LightGray);
            }
            else if (settings.FontFamily == "roboto")
            {
                btnOpenSans.Background = new SolidColorBrush(Colors.LightGray);
            }
            else
            {
                btnCalibriFont.Background = new SolidColorBrush(Colors.LightGray);
            }

            btnBlack.Background = new SolidColorBrush(Colors.Transparent);
            btnWhite.Background = new SolidColorBrush(Colors.Transparent);
            if (settings.SelectedBackground == "White")
            {
                btnWhite.Background = new SolidColorBrush(Colors.LightGray);
            }
            else
            {
                btnBlack.Background = new SolidColorBrush(Colors.LightGray);
            }
        }

        private void btnSegoeWPFont_Click(object sender, RoutedEventArgs e)
        {
            settings.FontFamily = "Segoe WP";
            btnSegoeWPFont.Background = new SolidColorBrush(Colors.LightGray);
            btnCalibriFont.Background = new SolidColorBrush(Colors.Transparent);
            btnOpenSans.Background = new SolidColorBrush(Colors.Transparent);
            UpdateAllStories();
        }

        private void btnCalibriFont_Click(object sender, RoutedEventArgs e)
        {
            settings.FontFamily = "Calibri";
            btnSegoeWPFont.Background = new SolidColorBrush(Colors.Transparent);
            btnCalibriFont.Background = new SolidColorBrush(Colors.LightGray);
            btnOpenSans.Background = new SolidColorBrush(Colors.Transparent);
            UpdateAllStories();
        }

        private void btnOpenSans_Click(object sender, RoutedEventArgs e)
        {
            settings.FontFamily = "roboto";
            btnSegoeWPFont.Background = new SolidColorBrush(Colors.Transparent);
            btnCalibriFont.Background = new SolidColorBrush(Colors.Transparent);
            btnOpenSans.Background = new SolidColorBrush(Colors.LightGray);
            UpdateAllStories();
        }

        private void btnBlack_Click(object sender, RoutedEventArgs e)
        {
            btnWhite.Background = new SolidColorBrush(Colors.Transparent);
            btnBlack.Background = new SolidColorBrush(Colors.LightGray);

            settings.SelectedForeground = "White";
            settings.SelectedBackground = "Black";
            UpdateAllStories();
        }

        private void btnWhite_Click(object sender, RoutedEventArgs e)
        {
            btnWhite.Background = new SolidColorBrush(Colors.LightGray);
            btnBlack.Background = new SolidColorBrush(Colors.Transparent);

            settings.SelectedForeground = "Black";
            settings.SelectedBackground = "White";
            UpdateAllStories();
        }

        //private async void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        //{
        //    if (_articleViewModel != null && _articleViewModel.Article != null && !string.IsNullOrEmpty(_articleViewModel.Article.Story))
        //    {
        //        string ssmlPrompt = "<speak version=\"1.0\" ";
        //        ssmlPrompt += "xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\">";
        //        ssmlPrompt += _articleViewModel.Article.HeadLine;
        //        ssmlPrompt += "    ";
        //        ssmlPrompt += _articleViewModel.Article.Story;
        //        ssmlPrompt += "</speak>";
        //        await synth.SpeakSsmlAsync(ssmlPrompt);
        //    }
        //}

        private static int previousIndex = 0;

        private void Pivot_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Pivot pivotcontrol = sender as Pivot;
            switch (pivotcontrol.SelectedIndex)
            {
                case 0:
                    {
                        if (_articleViewModel != null)
                        {
                            ApplicationBar.IsVisible = false;
                            secondScrollBar.Visibility = Visibility.Collapsed;
                            thirdScrollBar.Visibility = Visibility.Collapsed;
                            _articleViewModel.IsFetchingArticle = true;

                            scrollViewer.Visibility = Visibility.Visible;
                            if (previousIndex == 2)
                            {
                                if (_articleViewModel.NextArticle != null)
                                {
                                    onePivot.DataContext = _articleViewModel.NextArticle;
                                    _articleViewModel.SetCurrentPreviousandNextArticle(_articleViewModel.NextArticle);
                                    UpdateStory();
                                }
                            }
                            else if (previousIndex == 1)
                            {
                                if (_articleViewModel.PreviousArticle != null)
                                {
                                    onePivot.DataContext = _articleViewModel.PreviousArticle;
                                    _articleViewModel.SetCurrentPreviousandNextArticle(_articleViewModel.PreviousArticle);
                                    UpdateStory();
                                }
                            }
                            else
                            {
                                if (_articleViewModel.Article != null)
                                {
                                    onePivot.DataContext = _articleViewModel.Article;
                                    _articleViewModel.SetCurrentPreviousandNextArticle(_articleViewModel.Article);
                                    UpdateStory();
                                }
                            }
                            _articleViewModel.IsFetchingArticle = false;
                        }
                        previousIndex = 0;
                        ApplicationBar.IsVisible = true;
                    }
                    break;

                case 1:
                    {
                        if (_articleViewModel != null)
                        {
                            ApplicationBar.IsVisible = false;
                            scrollViewer.Visibility = Visibility.Collapsed;
                            secondScrollBar.Visibility = Visibility.Visible;
                            thirdScrollBar.Visibility = Visibility.Collapsed;

                            secondPivot.DataContext = null;
                            _articleViewModel.IsFetchingArticle = true;
                            if (previousIndex == 0)
                            {
                                if (_articleViewModel.NextArticle != null)
                                {
                                    secondPivot.DataContext = _articleViewModel.NextArticle;
                                    _articleViewModel.SetCurrentPreviousandNextArticle(_articleViewModel.NextArticle);
                                    UpdateSecondPivot();
                                }
                            }
                            else if (previousIndex == 2)
                            {
                                if (_articleViewModel.PreviousArticle != null)
                                {
                                    secondPivot.DataContext = _articleViewModel.PreviousArticle;
                                    _articleViewModel.SetCurrentPreviousandNextArticle(_articleViewModel.PreviousArticle);
                                    UpdateSecondPivot();
                                }
                            }
                            _articleViewModel.IsFetchingArticle = false;
                        }
                        previousIndex = 1;
                        ApplicationBar.IsVisible = true;
                    }
                    break;

                case 2:
                    {
                        if (_articleViewModel != null)
                        {
                            ApplicationBar.IsVisible = false;
                            scrollViewer.Visibility = Visibility.Collapsed;
                            secondScrollBar.Visibility = Visibility.Collapsed;
                            thirdScrollBar.Visibility = Visibility.Visible;

                            thirdPivot.DataContext = null;
                            _articleViewModel.IsFetchingArticle = true;
                            if (previousIndex == 1)
                            {
                                if (_articleViewModel.NextArticle != null)
                                {
                                    thirdPivot.DataContext = _articleViewModel.NextArticle;
                                    _articleViewModel.SetCurrentPreviousandNextArticle(_articleViewModel.NextArticle);
                                    UpdateThirdPivot();
                                }
                            }
                            else if (previousIndex == 0)
                            {
                                if (_articleViewModel.PreviousArticle != null)
                                {
                                    thirdPivot.DataContext = _articleViewModel.PreviousArticle;
                                    _articleViewModel.SetCurrentPreviousandNextArticle(_articleViewModel.PreviousArticle);
                                    UpdateThirdPivot();
                                }
                            }
                            _articleViewModel.IsFetchingArticle = false;
                        }
                        previousIndex = 2;
                        ApplicationBar.IsVisible = true;
                    }
                    break;
            }
        }

        private void UpdateAllStories()
        {
            UpdateStory();
            UpdateSecondPivot();
            UpdateThirdPivot();
        }

        private void UpdateStory()
        {
            scrollViewer.ScrollToVerticalOffset(0);
            scrollViewer.ScrollToHorizontalOffset(0);
            story.Html = "";
            if (settings != null)
            {
                if (settings.FontFamily == "roboto")
                {
                    headLine.FontFamily = new FontFamily("/Assets/Fonts/Roboto.ttf#Roboto");
                    story.FontFamily = new FontFamily("/Assets/Fonts/Roboto.ttf#Roboto");
                    photoCaption.FontFamily = new FontFamily("/Assets/Fonts/Roboto.ttf#Roboto");
                }
                else if (settings.FontFamily.ToLower() == "calibri")
                {
                    headLine.FontFamily = new FontFamily("/Assets/Fonts/ArbutusSlab-regular.ttf#Arbutus Slab");
                    story.FontFamily = new FontFamily("/Assets/Fonts/ArbutusSlab-regular.ttf#Arbutus Slab");
                    photoCaption.FontFamily = new FontFamily("/Assets/Fonts/ArbutusSlab-regular.ttf#Arbutus Slab");
                }
                else
                {
                    headLine.FontFamily = new FontFamily(settings.FontFamily);
                    story.FontFamily = new FontFamily(settings.FontFamily);
                    photoCaption.FontFamily = new FontFamily(settings.FontFamily);
                }
                story.FontSize = settings.FontSize;
                if (settings.SelectedForeground == "White")
                {
                    story.Foreground = new SolidColorBrush(Colors.White);
                }
                else
                {
                    story.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
            Article temp = scrollViewer.DataContext as Article;
            if (temp != null)
            {
                story.Html = "";
                story.Html = temp.Story;
            }
            ApplicationBar.IsVisible = true;
            fontStyleSelection.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void UpdateSecondPivot()
        {
            secondScrollBar.ScrollToVerticalOffset(0);
            secondScrollBar.ScrollToHorizontalOffset(0);

            if (settings != null)
            {
                if (settings.FontFamily == "roboto")
                {
                    secondPivotHeadLine.FontFamily = new FontFamily("/Assets/Fonts/Roboto.ttf#Roboto");
                    secondPivotStory.FontFamily = new FontFamily("/Assets/Fonts/Roboto.ttf#Roboto");
                    secondPivotPhotoCaption.FontFamily = new FontFamily("/Assets/Fonts/Roboto.ttf#Roboto");
                }
                else if (settings.FontFamily.ToLower() == "calibri")
                {
                    secondPivotHeadLine.FontFamily = new FontFamily("/Assets/Fonts/ArbutusSlab-regular.ttf#Arbutus Slab");
                    secondPivotStory.FontFamily = new FontFamily("/Assets/Fonts/ArbutusSlab-regular.ttf#Arbutus Slab");
                    secondPivotPhotoCaption.FontFamily = new FontFamily("/Assets/Fonts/ArbutusSlab-regular.ttf#Arbutus Slab");
                }
                else
                {
                    secondPivotHeadLine.FontFamily = new FontFamily(settings.FontFamily);
                    secondPivotStory.FontFamily = new FontFamily(settings.FontFamily);
                    secondPivotPhotoCaption.FontFamily = new FontFamily(settings.FontFamily);
                }
                secondPivotStory.FontSize = settings.FontSize;
                if (settings != null && settings.SelectedForeground == "White")
                {
                    secondPivotStory.Foreground = new SolidColorBrush(Colors.White);
                }
                else
                {
                    secondPivotStory.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
            Article temp = secondScrollBar.DataContext as Article;
            if (temp != null)
            {
                secondPivotStory.Html = "";
                secondPivotStory.Html = temp.Story;
            }
            ApplicationBar.IsVisible = true;
            fontStyleSelection.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void UpdateThirdPivot()
        {
            thirdScrollBar.ScrollToVerticalOffset(0);
            thirdScrollBar.ScrollToHorizontalOffset(0);
            if (settings != null)
            {
                if (settings.FontFamily == "roboto")
                {
                    thirdPivotHeadLine.FontFamily = new FontFamily("/Assets/Fonts/Roboto.ttf#Roboto");
                    thirdPivotStory.FontFamily = new FontFamily("/Assets/Fonts/Roboto.ttf#Roboto");
                    thirdPivotPhotoCaption.FontFamily = new FontFamily("/Assets/Fonts/Roboto.ttf#Roboto");
                }
                else if (settings.FontFamily.ToLower() == "calibri")
                {
                    thirdPivotHeadLine.FontFamily = new FontFamily("/Assets/Fonts/ArbutusSlab-regular.ttf#Arbutus Slab");
                    thirdPivotStory.FontFamily = new FontFamily("/Assets/Fonts/ArbutusSlab-regular.ttf#Arbutus Slab");
                    thirdPivotPhotoCaption.FontFamily = new FontFamily("/Assets/Fonts/ArbutusSlab-regular.ttf#Arbutus Slab");
                }
                else
                {
                    thirdPivotHeadLine.FontFamily = new FontFamily(settings.FontFamily);
                    thirdPivotStory.FontFamily = new FontFamily(settings.FontFamily);
                    thirdPivotPhotoCaption.FontFamily = new FontFamily(settings.FontFamily);
                }
                thirdPivotStory.FontSize = settings.FontSize;
                if (settings.SelectedForeground == "White")
                {
                    thirdPivotStory.Foreground = new SolidColorBrush(Colors.White);
                }
                else
                {
                    thirdPivotStory.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
            Article temp = thirdScrollBar.DataContext as Article;
            if (temp != null)
            {
                thirdPivotStory.Html = "";
                thirdPivotStory.Html = temp.Story;
            }
            ApplicationBar.IsVisible = true;
            fontStyleSelection.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (_articleViewModel != null && _articleViewModel.Article != null && !string.IsNullOrEmpty(_articleViewModel.Article.Thumb))
            {
                App.RootFrame.Navigate(new Uri(string.Format("/Views/ImageViewer.xaml?Id={0}", _articleViewModel.Article.Thumb), UriKind.Relative));
            }
        }
    }
}