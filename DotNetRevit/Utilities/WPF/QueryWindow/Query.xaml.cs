using Autodesk.Revit.UI;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DotNetRevit.Utilities.WPF
{
    /// <summary>
    /// Interaction logic for Query.xaml
    /// </summary>
    public partial class Query : Window
    {
        #region SystemMenu

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y,
           int nReserved, IntPtr hWnd, IntPtr prcRect);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        private struct RECT
        { public int left, top, right, bottom; }

        #endregion SystemMenu

        private WindowPinned state = WindowPinned.notPinned;

        private bool expanded = false;
        private double height;

        private Size normalWidth;
        private Size expandedWidth;

        public Query(string title)
        {
            InitializeComponent();

            this.ContentRendered += onFinishedRendering;

            this.pluginVersion.Text = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            this.userName.Text = $"{Environment.UserDomainName}/{Environment.UserName}";
        }

        private async Task ShowToolTip(string message, Button sender)
        {
            ToolTip tt = new ToolTip();
            tt.Content = message;

            tt.PlacementTarget = sender;
            tt.IsOpen = true;

            await Task.Delay(1000);

            tt.IsOpen = false;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MaxButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                this.maxButtonImage.Source = new BitmapImage(new Uri("pack://application:,,,/DotNetRevit;component/Utilities/WPF/QueryWindow/UI/Maximise.png"));
                this.MaxButton.ToolTip = "Maximise";
                this.TitleBar.Height = 24;
                this.TitleBar.Margin = new Thickness(0, 0, 0, 6);
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.maxButtonImage.Source = new BitmapImage(new Uri("pack://application:,,,/DotNetRevit;component/Utilities/WPF/QueryWindow/UI/Normalise.png"));
                this.MaxButton.ToolTip = "Restore";
                this.TitleBar.Height = 32;
                this.TitleBar.Margin = new Thickness(0);
            }
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void userNameButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.userName.Text);
            ShowToolTip("Copied to Clipboard", sender as Button);
        }

        private void pluginVersionButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.pluginVersion.Text);
            ShowToolTip("Copied to Clipboard", sender as Button);
        }

        private void Extend_Click(object sender, RoutedEventArgs e)
        {
            if (expanded)
            {
                extendImage.RenderTransform = new RotateTransform(180, 0, 0);

                expanded = false;
                this.Width = normalWidth.Width;
                this.Height = normalWidth.Height;
            }
            else
            {
                expanded = true;
                this.Width = expandedWidth.Width;
                this.Height = expandedWidth.Height;
            }
        }

        private void Resized(object sender, SizeChangedEventArgs e)
        {
            if (expanded)
            {
                expandedWidth = e.NewSize;
            }
            else
            {
                normalWidth = e.NewSize;
            }
        }

        private void onFinishedRendering(object sender, EventArgs e)
        {
            normalWidth = this.RenderSize;
            expandedWidth = new Size(normalWidth.Width + 300, normalWidth.Height + 150);
        }

        private void openHtml_Click(object sender, RoutedEventArgs e)
        {
        }

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void lockWindow_Click(object sender, RoutedEventArgs e)
        {
            if (state == WindowPinned.notPinned)
            {
                state = WindowPinned.alwaysTop;
                this.lockWindowImage.Source = this.maxButtonImage.Source = new BitmapImage(new Uri("pack://application:,,,/DotNetRevit;component/Utilities/WPF/QueryWindow/UI/Lock.png"));
                this.Topmost = true;
            }
            else if (state == WindowPinned.alwaysTop)
            {
                state = WindowPinned.collapsedTop;
                this.lockWindowImage.Source = this.maxButtonImage.Source = new BitmapImage(new Uri("pack://application:,,,/DotNetRevit;component/Utilities/WPF/QueryWindow/UI/UnPin.png"));
                this.Topmost = true;
                this.info.Height = 0;
            }
            else if (state == WindowPinned.collapsedTop)
            {
                state = WindowPinned.collapsedTop;
                this.lockWindowImage.Source = this.maxButtonImage.Source = new BitmapImage(new Uri("pack://application:,,,/DotNetRevit;component/Utilities/WPF/QueryWindow/UI/UnPin.png"));
                this.Topmost = true;
                this.Height = 40;
            }
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            IntPtr hWnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;

            RECT pos;
            GetWindowRect(hWnd, out pos);
            IntPtr hMenu = GetSystemMenu(hWnd, false);
            int cmd = TrackPopupMenu(hMenu, 0x100, pos.left, pos.top, 0, hWnd, IntPtr.Zero);
            if (cmd > 0) SendMessage(hWnd, 0x112, (IntPtr)cmd, IntPtr.Zero);
        }

        private enum WindowPinned
        {
            notPinned,
            alwaysTop,
            collapsedTop
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}