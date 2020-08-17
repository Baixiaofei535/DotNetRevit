using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        #region Variables and Constructor

        private WindowPinned state = WindowPinned.notPinned;

        private bool expanded = false;
        private double height;

        private System.Windows.Size normalSize;
        private System.Windows.Size expandedSize;

        public Query(string title)
        {
            InitializeComponent();

            this.ContentRendered += onFinishedRendering;
            this.Activated += Query_Activated;
            this.Deactivated += Query_Deactivated;

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

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #endregion Variables and Constructor

        #region Min/Max/Close

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

        #endregion Min/Max/Close

        #region Information Buttons

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

        #endregion Information Buttons

        #region Resize Button

        private void Extend_Click(object sender, RoutedEventArgs e)
        {
            if (expanded)
            {
                extendImage.RenderTransform = new RotateTransform(0, extendImage.ActualHeight / 2, extendImage.ActualWidth / 2);

                expanded = false;
                this.Width = normalSize.Width;
                this.Height = normalSize.Height;
            }
            else
            {
                extendImage.RenderTransform = new RotateTransform(180, extendImage.ActualHeight / 2, extendImage.ActualWidth / 2);

                expanded = true;
                this.Width = expandedSize.Width;
                this.Height = expandedSize.Height;
            }
        }

        private void Resized(object sender, SizeChangedEventArgs e)
        {
            if (expanded)
            {
                expandedSize = e.NewSize;
            }
            else
            {
                normalSize = e.NewSize;
            }
        }

        private void onFinishedRendering(object sender, EventArgs e)
        {
            normalSize = this.RenderSize;
            expandedSize = new System.Windows.Size(normalSize.Width + 300, normalSize.Height + 150);
        }

        #endregion Resize Button

        #region Saving opening Printing

        private void openHtml_Click(object sender, RoutedEventArgs e)
        {
            string fileName = Path.GetTempFileName().Replace(".tmp", ".html");

            SaveHtml(fileName, false);

            Process.Start(fileName);
        }

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            string fileName = Path.GetTempFileName().Replace(".tmp", ".html");

            SaveHtml(fileName, true);

            Process.Start(fileName);
        }

        private void Document_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font font = new Font("Arial", 12, System.Drawing.FontStyle.Regular);

            e.Graphics.DrawString(InfoBox.Text, font, System.Drawing.Brushes.Black, 0, 0);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();
            saveDialog.Title = "Save to";
            saveDialog.AddExtension = true;
            saveDialog.DefaultExt = "html";
            saveDialog.Filter = "html file (*.html)|*.html";

            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveHtml(saveDialog.FileName, false);
            }
        }

        private void SaveHtml(string fileName, bool print)
        {
            //Add some CSS Later
            string newString = String.Empty;
            string printCommand = String.Empty;
            if (print)
            {
                printCommand = "onload=\"window.print()\"";
            }

            newString += $"<html>\n<body {printCommand}>\n<div class=\"content\"\n";
            string[] lines = InfoBox.Text.Split('\n');
            foreach (var line in lines)
            {
                newString += $"<p>{line}</p>\n";
            }

            newString += "</div></body>\n";

            newString += "</html>";
            File.WriteAllText(fileName, newString);
        }

        #endregion Saving opening Printing

        #region Locking Window

        private void lockWindow_Click(object sender, RoutedEventArgs e)
        {
            if (state == WindowPinned.notPinned)
            {
                state = WindowPinned.alwaysTop;
                this.lockWindowImage.Source = new BitmapImage(new Uri("pack://application:,,,/DotNetRevit;component/Utilities/WPF/QueryWindow/UI/Lock.png"));
                this.Topmost = true;
            }
            else if (state == WindowPinned.alwaysTop)
            {
                state = WindowPinned.collapsedTop;
                this.lockWindowImage.Source = new BitmapImage(new Uri("pack://application:,,,/DotNetRevit;component/Utilities/WPF/QueryWindow/UI/UnPin.png"));
                this.Topmost = true;
            }
            else if (state == WindowPinned.collapsedTop)
            {
                state = WindowPinned.notPinned;
                this.lockWindowImage.Source = new BitmapImage(new Uri("pack://application:,,,/DotNetRevit;component/Utilities/WPF/QueryWindow/UI/Pin.png"));
                this.Topmost = false;
            }
        }

        private void Query_Deactivated(object sender, EventArgs e)
        {
            if (state == WindowPinned.collapsedTop)
            {
                height = this.Height;

                this.Height = 40;
            }
        }

        private void Query_Activated(object sender, EventArgs e)
        {
            if (state == WindowPinned.collapsedTop)
            {
                this.Height = height;
            }
        }

        private enum WindowPinned
        {
            notPinned,
            alwaysTop,
            collapsedTop
        }

        #endregion Locking Window

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.InfoBox.Text);
            ShowToolTip("Copied to Clipboard", sender as Button);
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
    }
}