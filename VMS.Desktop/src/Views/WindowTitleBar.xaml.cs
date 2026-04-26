using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VMS.Views
{
    public partial class WindowTitleBar : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(WindowTitleBar),
                new PropertyMetadata(string.Empty, OnTitleChanged));

        public static readonly DependencyProperty ShowMaximizeProperty =
            DependencyProperty.Register(nameof(ShowMaximize), typeof(bool), typeof(WindowTitleBar),
                new PropertyMetadata(true, OnShowMaximizeChanged));

        public static readonly DependencyProperty ShowMinimizeProperty =
            DependencyProperty.Register(nameof(ShowMinimize), typeof(bool), typeof(WindowTitleBar),
                new PropertyMetadata(true, OnShowMinimizeChanged));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public bool ShowMaximize
        {
            get => (bool)GetValue(ShowMaximizeProperty);
            set => SetValue(ShowMaximizeProperty, value);
        }

        public bool ShowMinimize
        {
            get => (bool)GetValue(ShowMinimizeProperty);
            set => SetValue(ShowMinimizeProperty, value);
        }

        public WindowTitleBar()
        {
            InitializeComponent();
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowTitleBar ctrl)
                ctrl.SubTitle.Text = e.NewValue as string ?? string.Empty;
        }

        private static void OnShowMaximizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowTitleBar ctrl)
                ctrl.MaxBtn.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void OnShowMinimizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowTitleBar ctrl)
                ctrl.MinBtn.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            => Window.GetWindow(this)?.DragMove();

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            var w = Window.GetWindow(this);
            if (w != null) w.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            var w = Window.GetWindow(this);
            if (w != null) w.WindowState = w.WindowState == WindowState.Maximized
                ? WindowState.Normal : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
            => Window.GetWindow(this)?.Close();
    }
}
