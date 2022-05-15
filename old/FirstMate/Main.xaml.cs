using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml;
using System.Timers;
using System.Windows.Controls;

namespace FirstMate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Main : Window
    {

        #region Globals

        Controls.Game gameControl;
        Control currentGame;


        //About aboutWindow;

        // Lock Object for Threaded list changes
        //private object lockActivity = new object();
        //private object lockGame = new object();
        //private object lockTrader = new object();

        // Background Worker
        //BackgroundWorker logWorker;
        //BackgroundWorker fraudWorker;

        // Refresh Timer
        System.Timers.Timer RefreshTimer;
        //DateTime LastRefresh;

        // Store the mouse state for title bar drag, snap, and maximize events.
        bool UnSnap = false;
        Point MousePosition;

        #endregion
        #region System

        private enum ResizeDirection
        {
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8,
        }

        const int WM_SYSCOMMAND = 0x112;
        HwndSource hwndSource;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        #endregion
        #region Initialization

        public Main()
        {
            InitializeComponent();

            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            Background = Brushes.Transparent;
            innerBorder.Visibility = Visibility.Visible;
            outerBorder.Visibility = Visibility.Visible;

            // Event hander to initialize window source.
            SourceInitialized += new EventHandler(InitializeWindowSource);

            gameControl = new();
            currentGame = gameControl;
            currentGame.Width = this.Width;
            currentGame.Height = this.Height - 50;
            currentGame.Margin = new Thickness(0, 150, 0, 0);

            mainGrid.Children.Add(currentGame);

            LoadSettings();

            //OverviewControl.MainWindow = this;
            //GamesControl.MainWindow = this;
            //TradersControl.MainWindow = this;

            //fraudWorker = new BackgroundWorker();
            //fraudWorker.WorkerSupportsCancellation = true;
            //fraudWorker.WorkerReportsProgress = true;
            //fraudWorker.ProgressChanged += fraudWorkerProgress;
            //fraudWorker.RunWorkerCompleted += fraudWorkerCompleted;
            //fraudWorker.DoWork += fraudWorkerDoWorkAsync;

            //logWorker = new BackgroundWorker();
            //logWorker.WorkerSupportsCancellation = true;
            //logWorker.WorkerReportsProgress = true;
            //logWorker.ProgressChanged += logWorkerProgress;
            //logWorker.RunWorkerCompleted += logWorkerCompleted;
            //logWorker.DoWork += logWorkerDoWork;


            //BindingOperations.EnableCollectionSynchronization(ActivityList, lockActivity);
            //BindingOperations.EnableCollectionSynchronization(TraderList, lockTrader);

            //GamesControl.gamesDataGrid.ItemsSource = GameList;
            //TradersControl.tradersDataGrid.ItemsSource = TraderList;

            // TODO: add resresh on startup to config
            BeginRefresh();

            // TODO: add autorefresh and refresh time to config
            //RefreshTimer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            RefreshTimer = new System.Timers.Timer(300000); // 5 Min
            RefreshTimer.Elapsed += RefreshTimerElapsed;
            RefreshTimer.AutoReset = true;
            RefreshTimer.Enabled = true;


        }

        private void RefreshTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => BeginRefresh());
        }

        private void onWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.Maximized)
            {
                SwitchState();
            }


        }

        private void LoadSettings()
        {
            // Get the file modified date, and generate appVersion from this date.
            //DateTime CompiledDate = File.GetLastWriteTimeUtc(Environment.CurrentDirectory + "\\dw.exe");
            //String appVersion = String.Format("{0:yy}.{1}.{2:0000}", CompiledDate, CompiledDate.DayOfYear / 7, (CompiledDate.DayOfYear * 24) + CompiledDate.Hour);
            //versionLabel.Content = "Version " + appVersion;
            versionLabel.Visibility = Visibility.Hidden;

            if (Properties.Settings.Default.FirstRun)
            {
                Properties.Settings.Default.FirstRun = false;
            }
            else
            {
                // Apply window position.
                Top = Properties.Settings.Default.Top;
                Left = Properties.Settings.Default.Left;
                Width = Properties.Settings.Default.Width;
                Height = Properties.Settings.Default.Height;
            }


        }


        #endregion
        #region Event Handlers

        private void InitializeWindowSource(object sender, EventArgs e)
        {
            // Get a handle to the window source so we can re-size it.
            hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            // Save window position.
            if (WindowState == WindowState.Maximized)
            {
                Properties.Settings.Default.Maximized = true;
            }
            else
            {
                Properties.Settings.Default.Maximized = false;
                Properties.Settings.Default.Top = Top;
                Properties.Settings.Default.Left = Left;
                Properties.Settings.Default.Width = Width;
                Properties.Settings.Default.Height = Height;
            }


            Properties.Settings.Default.Save();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                //TODO: Restore down if already max height

                this.Top = 0;
                this.Height = 1024;

                //TODO: Get/Apply actual screen height
            }
            windowResizing(true);
        }


        private void onMouseMove(object sender, MouseEventArgs e)
        {
            windowResizing(false);
        }

        private void onMouseUp(object sender, MouseButtonEventArgs e)
        {

            // resize the background to fit the new windows size
            //MainTheme.backgroundImage.Height = this.Height - 64;
            //MainTheme.backgroundRect.Height = this.Height - 64;
        }

        private void onMouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void windowResizing(bool resizing)
        {
            const int BORDER_WIDTH = 8;

            //if (window == null) return;

            ResizeDirection d = 0;
            Point mousePosition = Mouse.GetPosition(this);
            //_textBox1.Text = mousePosition.X + ":" + mousePosition.Y + "\n" + _window.Width + "\n" + this.Width;

            if (mousePosition.X < BORDER_WIDTH)  // Left
                d = ResizeDirection.Left;

            if (mousePosition.X > (this.Width - BORDER_WIDTH)) // Right
                d = ResizeDirection.Right;


            if (mousePosition.Y < BORDER_WIDTH) //Top
            {
                d = ResizeDirection.Top;

                if (mousePosition.X < BORDER_WIDTH) // Top Left
                    d = ResizeDirection.TopLeft;

                if (mousePosition.X > (this.Width - BORDER_WIDTH)) // Top Right
                    d = ResizeDirection.TopRight;
            }

            if (mousePosition.Y > (this.Height - BORDER_WIDTH)) // Bottom
            {
                d = ResizeDirection.Bottom;

                if (mousePosition.X < BORDER_WIDTH) // Bottom Left
                    d = ResizeDirection.BottomLeft;

                if (mousePosition.X > (this.Width - BORDER_WIDTH)) // Bottom Right
                    d = ResizeDirection.BottomRight;
            }

            // Send a windows system message to resize the window if the left mouse button is down.
            if (resizing)
            {
                SendMessage(hwndSource.Handle, WM_SYSCOMMAND, (IntPtr)(61440 + d), IntPtr.Zero);
            }
            else
            {
                switch (d)
                {
                    case ResizeDirection.Left:
                    case ResizeDirection.Right:
                        this.Cursor = Cursors.SizeWE;
                        break;

                    case ResizeDirection.Top:
                    case ResizeDirection.Bottom:
                        this.Cursor = Cursors.SizeNS;
                        break;

                    case ResizeDirection.TopLeft:
                    case ResizeDirection.BottomRight:
                        this.Cursor = Cursors.SizeNWSE;
                        break;

                    case ResizeDirection.TopRight:
                    case ResizeDirection.BottomLeft:
                        this.Cursor = Cursors.SizeNESW;
                        break;
                }
            }
        }

        private void onTitleClick(object sender, MouseButtonEventArgs e)
        {
            // Change window state if double clicked
            if (e.ClickCount == 2)
            {
                if (e.GetPosition(this).Y < 60)
                {
                    SwitchState();
                }
            }
            else
            {
                if (this.WindowState == System.Windows.WindowState.Maximized)
                {
                    UnSnap = true;
                    MousePosition = e.GetPosition(this);
                }
            }
        }

        private void onTitleRelease(object sender, MouseButtonEventArgs e)
        {
            UnSnap = false;
        }

        private void onTitleDrag(object sender, MouseEventArgs e)
        {
            if (UnSnap)
            {
                Point p = e.GetPosition(this);
                if (p.X - MousePosition.X > 5 ||
                    p.X - MousePosition.X < -5 ||
                    p.Y - MousePosition.Y > 5 ||
                    p.Y - MousePosition.Y < -5)
                {

                    double X = p.X - RestoreBounds.Width / 2;
                    if (X < 0) X = 0;
                    if (X + RestoreBounds.Width > SystemParameters.PrimaryScreenWidth) X = SystemParameters.PrimaryScreenWidth - RestoreBounds.Width;

                    //re-position on second monitor
                    if (Left > SystemParameters.PrimaryScreenWidth) X += SystemParameters.PrimaryScreenWidth;

                    UnSnap = false;
                    SwitchState();
                    Top = 0;
                    Left = X;
                }
            }
            else
            {
                // Allow dragging by the title bar.
                if (e.LeftButton == MouseButtonState.Pressed && e.GetPosition(this).Y < 60)
                {
                    DragMove();
                }
            }
        }

        private void onAppClose(object sender, RoutedEventArgs e)
        {
            //Close();
            Application.Current.Shutdown();
        }

        private void onAppMinimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void onAppMaximize(object sender, RoutedEventArgs e)
        {
            SwitchState();
        }

        private void onAppRestore(object sender, RoutedEventArgs e)
        {
            SwitchState();
        }

        private void SwitchState()
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
                cmdMaximizeApp.Visibility = Visibility.Hidden;
                cmdRestoreApp.Visibility = Visibility.Visible;

                innerBorder.Visibility = Visibility.Hidden;
                outerBorder.Visibility = Visibility.Hidden;

                //This doesn't work, because you can't modify the height of a window this way.
                //this.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;

                // TODO: Get working area of active monitor, and re-adjust when working area changes.
                System.Drawing.Rectangle workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;

                Thickness maximizedMargin = new Thickness
                {
                    Top = workingArea.Top + 6,
                    Left = workingArea.Left + 6,
                    Bottom = this.ActualHeight - workingArea.Height - workingArea.Top - 7,
                    Right = this.ActualWidth - workingArea.Width - workingArea.Left - 7
                };

                mainGrid.Margin = maximizedMargin;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
                cmdMaximizeApp.Visibility = Visibility.Visible;
                cmdRestoreApp.Visibility = Visibility.Hidden;

                innerBorder.Visibility = Visibility.Visible;
                outerBorder.Visibility = Visibility.Visible;

                mainGrid.Margin = new Thickness(4);
            }
        }

        private void onCustomizeClick(object sender, RoutedEventArgs e)
        {
            // Show Customization Menu
            if (customizeMenu.Visibility == Visibility.Hidden)
            {
                customizeMenu.Visibility = Visibility.Visible;
                customizeMenu.IsOpen = true;

                customizeMenu.PlacementTarget = customize;
                customizeMenu.Placement = PlacementMode.RelativePoint;
                customizeMenu.HorizontalOffset = -230;
                customizeMenu.VerticalOffset = 25;
            }
            else
            {
                customizeMenu.Visibility = Visibility.Hidden;
            }
        }

        private void onCustomizeClosed(object sender, RoutedEventArgs e)
        {
            customizeMenu.Visibility = Visibility.Hidden;

        }

        private void onCustomizeLostFocus(object sender, RoutedEventArgs e)
        {
            // TODO: Close customization menu when focus is lost
            //customizeMenu.Visibility = Visibility.Hidden;
        }

        private void onRefreshMouseEnter(object sender, MouseEventArgs e)
        {
            if (ProgressBorder.Visibility == Visibility.Hidden)
            {
                RefreshImage.Opacity = .5;
            }
        }

        private void onRefreshMouseLeave(object sender, MouseEventArgs e)
        {
            if (ProgressBorder.Visibility == Visibility.Hidden)
            {
                RefreshImage.Opacity = .3;
            }
            else
            {
                RefreshImage.Opacity = .1;
            }
        }

        private void onRefreshClick(object sender, MouseButtonEventArgs e)
        {
            BeginRefresh();
        }

        private void OverviewMouseEnter(object sender, MouseEventArgs e)
        {
            OverviewHighlight.Visibility = Visibility.Visible;
        }

        private void OverviewMouseLeave(object sender, MouseEventArgs e)
        {
            OverviewHighlight.Visibility = Visibility.Hidden;
        }

        private void OverviewClick(object sender, MouseEventArgs e)
        {
            //OverviewControl.Visibility = Visibility.Visible;
            //GamesControl.Visibility = Visibility.Hidden;
            //TradersControl.Visibility = Visibility.Hidden;

            OverviewImage.Opacity = 1;
            GamesImage.Opacity = .5;
            TradersImage.Opacity = .5;

            OverviewMenuItem.IsChecked = true;
            GamesMenuItem.IsChecked = false;
            TradersMenuItem.IsChecked = false;
        }

        private void GamesMouseEnter(object sender, MouseEventArgs e)
        {
            GamesHighlight.Visibility = Visibility.Visible;
        }

        private void GamesMouseLeave(object sender, MouseEventArgs e)
        {
            GamesHighlight.Visibility = Visibility.Hidden;
        }

        private void GamesClick(object sender, MouseEventArgs e)
        {
            //OverviewControl.Visibility = Visibility.Hidden;
            //GamesControl.Visibility = Visibility.Visible;
            //TradersControl.Visibility = Visibility.Hidden;

            OverviewImage.Opacity = .5;
            GamesImage.Opacity = 1;
            TradersImage.Opacity = .5;

            OverviewMenuItem.IsChecked = false;
            GamesMenuItem.IsChecked = true;
            TradersMenuItem.IsChecked = false;
        }

        private void TradersMouseEnter(object sender, MouseEventArgs e)
        {
            TradersHighlight.Visibility = Visibility.Visible;
        }

        private void TradersMouseLeave(object sender, MouseEventArgs e)
        {
            TradersHighlight.Visibility = Visibility.Hidden;
        }

        private void TradersClick(object sender, MouseEventArgs e)
        {
            //OverviewControl.Visibility = Visibility.Hidden;
            //GamesControl.Visibility = Visibility.Hidden;
            //TradersControl.Visibility = Visibility.Visible;

            OverviewImage.Opacity = .5;
            GamesImage.Opacity = .5;
            TradersImage.Opacity = 1;

            OverviewMenuItem.IsChecked = false;
            GamesMenuItem.IsChecked = false;
            TradersMenuItem.IsChecked = true;
        }


        private void onProcessStarted(object sender, RoutedEventArgs e)
        {
            OverviewImage.IsEnabled = false;
            TradersImage.IsEnabled = false;

            customize.IsEnabled = false;
        }

        private void onProcessFinished(object sender, RoutedEventArgs e)
        {
            OverviewImage.IsEnabled = true;
            TradersImage.IsEnabled = true;

            customize.IsEnabled = true;
        }


        #endregion
        #region Menu Commands

        private void cmdShowOverview(object sender, RoutedEventArgs e)
        {
            //OverviewControl.Visibility = Visibility.Visible;
            //GamesControl.Visibility = Visibility.Hidden;
            //TradersControl.Visibility = Visibility.Hidden;

            OverviewImage.Opacity = 1;
            GamesImage.Opacity = .5;
            TradersImage.Opacity = .5;

            OverviewMenuItem.IsChecked = true;
            GamesMenuItem.IsChecked = false;
            TradersMenuItem.IsChecked = false;
        }

        private void cmdShowGames(object sender, RoutedEventArgs e)
        {
            //OverviewControl.Visibility = Visibility.Hidden;
            //GamesControl.Visibility = Visibility.Visible;
            //TradersControl.Visibility = Visibility.Hidden;

            OverviewImage.Opacity = .5;
            GamesImage.Opacity = 1;
            TradersImage.Opacity = .5;

            OverviewMenuItem.IsChecked = false;
            GamesMenuItem.IsChecked = true;
            TradersMenuItem.IsChecked = false;
        }

        private void cmdShowTraders(object sender, RoutedEventArgs e)
        {
            //OverviewControl.Visibility = Visibility.Hidden;
            //GamesControl.Visibility = Visibility.Hidden;
            //TradersControl.Visibility = Visibility.Visible;

            OverviewImage.Opacity = .5;
            GamesImage.Opacity = .5;
            TradersImage.Opacity = 1;

            OverviewMenuItem.IsChecked = false;
            GamesMenuItem.IsChecked = false;
            TradersMenuItem.IsChecked = true;
        }

        private void cmdShowAbout(object sender, RoutedEventArgs e)
        {
            customizeMenu.Visibility = Visibility.Hidden;
            //aboutWindow.Left = this.Left + 55;
            //aboutWindow.Top = this.Top + 35;

            //aboutWindow.ShowDialog();
        }

        #endregion
        #region Data

        public class Activity
        {
            public bool Bannable { get; set; }
            public DateTime TimeStamp { get; set; }
            public string Value { get; set; }
            public string Background { get; set; }
            public string Address { get; set; }

            public Activity() { }
        }

        public class Game
        {
            public bool Active, Scheduled, Deleted;
            public int Traders { get; set; }
            public int DupeCount { get; set; }
            public int FraudCount { get; set; }
            public int ProxyCount { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Directory { get; set; }
            public string Notes { get; set; }

            public Game()
            {
                Active = true;
                Scheduled = false;
                Deleted = false;
                Traders = 0;
                DupeCount = 0;
                FraudCount = 0;
            }
        }

        public class Trader
        {
            public DateTime TimeStamp { get; set; }
            public bool Active { get; set; }
            public bool Banned { get; set; }
            public bool IsDupe { get; set; }
            public bool IsFraud { get; set; }
            public bool IsDynamic { get; set; }  // TODO: Test for Static / Dynamic IPs
            public int ProxyType { get; set; }
            public int AddressCount { get; set; }
            public int UserID { get; set; }
            public int IpqFraudScore { get; set; }
            public int IpiFraudScore { get; set; }
            public string DisplayName { get; set; }
            public string DisplayAddress { get; set; }
            public string LastIP { get; set; }
            public string Game { get; set; }
            public string Logon { get; set; }
            public string Alias { get; set; }
            public string Location { get; set; }
            public string Provider { get; set; }
            public string Note { get; set; }
            public string LastError { get; set; }

            public Trader()
            {
                Active = true;
            }
        }

        public class Provider
        {
            public DateTime TimeStamp { get; set; }
            public bool Active { get; set; }
            public bool IsProxy { get; set; }
            public string Name { get; set; }
            public string IP { get; set; }
            public string Location { get; set; }

            public Provider(string name)
            {
                Name = name;
            }
        }

        public class Address
        {
            public bool Active { get; set; }
            public DateTime TimeStamp { get; set; }
            public string Logon { get; set; }
            public string Game { get; set; }
            public string IP { get; set; }

            public Address()
            {
                Active = true;
            }
        }

        #endregion
        #region Background Workers

        private void BeginRefresh()
        {
            if (ProgressBorder.Visibility == Visibility.Hidden)
            {
                ProgressBar.Value = 0;
                ProgressLabel.Content = "Scanning...";
                ProgressBorder.Visibility = Visibility.Visible;
                RefreshImage.Opacity = .1;

                //AddressList.Clear();
                //ActivityList.Clear();

                //logWorker.RunWorkerAsync();
            }
        }

        private void RefrehCompleted()
        {
            ProgressBorder.Visibility = Visibility.Hidden;
            RefreshImage.Opacity = .3;
        }


        private void logWorkerProgress(object sender, ProgressChangedEventArgs e)
        {
            //ProgressLabel.Content = (string)e.UserState;
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void logWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void logWorkerDoWork(object sender, DoWorkEventArgs e)
        {
        }



        #endregion

    }
}
