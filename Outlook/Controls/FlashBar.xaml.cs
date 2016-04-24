using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Outlook.Controls
{
    public partial class FlashBar : UserControl
    {
        private Storyboard fadeInSB = new Storyboard();
        private Storyboard fadeOutSB = new Storyboard();

        private bool IsReallyAnimating;
        private bool HasBeenToggled;

        public bool DoNotShowOnFirstToggleOfIsAnimating { get; set; }

        public bool IsAnimating
        {
            get { return (bool)GetValue(IsAnimatingProperty); }
            set { SetValue(IsAnimatingProperty, value); }
        }

        public static readonly DependencyProperty IsAnimatingProperty =
            DependencyProperty.Register("IsAnimating", typeof(bool), typeof(FlashBar), new PropertyMetadata(false, IsAnimatingChanged));

        private static void IsAnimatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fb = d as FlashBar;
            if (true.Equals(e.NewValue))
                fb.Show();
        }

        public FlashBar()
        {
            InitializeComponent();
            InitStoryBoard();
        }

        private void InitStoryBoard()
        {
            SetupSB(0.5D, 1D, TimeSpan.FromMilliseconds(200D), fadeInSB);
            SetupSB(1D, 0, TimeSpan.FromMilliseconds(750D), fadeOutSB);

            fadeInSB.Completed += (s, e) => { fadeOutSB.Begin(); };
            fadeOutSB.Completed += (s, e) =>
            {
                theRect.Visibility = Visibility.Collapsed;
                IsReallyAnimating = false;
            };
        }

        private void SetupSB(double from, double to, TimeSpan duration, Storyboard sb)
        {
            var animation = new DoubleAnimation { To = to, From = from, Duration = duration };
            sb.Children.Add(animation);
            Storyboard.SetTarget(animation, theRect);
            Storyboard.SetTargetProperty(animation, new PropertyPath(FrameworkElement.OpacityProperty));
        }

        public void Show()
        {
            if (IsReallyAnimating) return;

            if (!HasBeenToggled)
            {
                HasBeenToggled = true;
                if (DoNotShowOnFirstToggleOfIsAnimating) return;
            }

            theRect.Visibility = Visibility.Visible;
            IsReallyAnimating = true;
            fadeInSB.Begin();
        }
    }
}