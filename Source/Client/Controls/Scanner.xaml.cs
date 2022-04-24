using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Threading;

namespace Controls
{
    /// <summary>
    /// Interaction logic for Scanner.xaml
    /// </summary>
    public partial class Scanner : UserControl
    {
        public static readonly RoutedEvent ButtonClickEvent =
            EventManager.RegisterRoutedEvent("ButtonClick", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Scanner));

        public event RoutedEventHandler ButtonClick
        {
            add { AddHandler(ButtonClickEvent, value); }
            remove { RemoveHandler(ButtonClickEvent, value); }
        }


        public static DependencyProperty SectorsProperty =
            DependencyProperty.Register("SectorData", typeof(XElement), typeof(Scanner));

        public XElement SectorData
        {
            get { return (XElement)GetValue(SectorsProperty); }
            set { SetValue(SectorsProperty, value); }
        }

        public static DependencyProperty GameProperty =
            DependencyProperty.Register("Game", typeof(XElement), typeof(Scanner));

        public XElement Game
        {
            get { return (XElement)GetValue(GameProperty); }
            set { SetValue(GameProperty, value); }
        }

        private string _CurrentSector;

        private string _SectorClicked;
        public string SectorClicked
        {
            get { return _SectorClicked; }
        }

        public Scanner()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _SectorClicked = (string)((Button)sender).Content;
            RaiseEvent(new RoutedEventArgs(ButtonClickEvent));
        }

        public void Update(string CurrentSector)
        {
            XElement sector = null;

            _CurrentSector = CurrentSector;
            SectorLabel.Text = "Sector : " + CurrentSector;

            ButtonQ.Template = (ControlTemplate)FindResource("Hexagon1");
            ButtonW.Template = (ControlTemplate)FindResource("Hexagon1");
            ButtonE.Template = (ControlTemplate)FindResource("Hexagon1");
            ButtonA.Template = (ControlTemplate)FindResource("Hexagon1");
            ButtonS.Template = (ControlTemplate)FindResource("Hexagon1");
            ButtonD.Template = (ControlTemplate)FindResource("Hexagon1");
            ButtonX.Template = (ControlTemplate)FindResource("Hexagon1");

            // Search for an existing sector matching sector Number attribute
            try
            {
                sector = (from s in SectorData.Elements("Sector")
                          where (string)s.Attribute("Number") == CurrentSector
                          select s).First();
                //textBlock2.Text = (string)sector.Element("Warps");
                foreach (XElement w in sector.Element("Warps").Elements("Warp"))
                {
                    switch (((string)w.Attribute("Nav")).Substring(0, 1))
                    {
                        case "Q":
                            ButtonQ.Content = (string)w;
                            ButtonQ.Template = (ControlTemplate)FindResource("Hexagon3");
                            if ((string)w.Attribute("Unexplored") == "True") ButtonQ.Template = (ControlTemplate)FindResource("Hexagon2");
                            break;

                        case "W":
                            ButtonW.Content = (string)w;
                            ButtonW.Template = (ControlTemplate)FindResource("Hexagon3");
                            if ((string)w.Attribute("Unexplored") == "True") ButtonW.Template = (ControlTemplate)FindResource("Hexagon2");
                            break;

                        case "E":
                            ButtonE.Content = (string)w;
                            ButtonE.Template = (ControlTemplate)FindResource("Hexagon3");
                            if ((string)w.Attribute("Unexplored") == "True") ButtonE.Template = (ControlTemplate)FindResource("Hexagon2");
                            break;

                        case "A":
                            ButtonA.Content = (string)w;
                            ButtonA.Template = (ControlTemplate)FindResource("Hexagon3");
                            if ((string)w.Attribute("Unexplored") == "True") ButtonA.Template = (ControlTemplate)FindResource("Hexagon2");
                            break;

                        case "S":
                            ButtonS.Content = (string)w;
                            ButtonS.Template = (ControlTemplate)FindResource("Hexagon3");
                            if ((string)w.Attribute("Unexplored") == "True") ButtonS.Template = (ControlTemplate)FindResource("Hexagon2");
                            break;

                        case "D":
                            ButtonD.Content = (string)w;
                            ButtonD.Template = (ControlTemplate)FindResource("Hexagon3");
                            if ((string)w.Attribute("Unexplored") == "True") ButtonD.Template = (ControlTemplate)FindResource("Hexagon2");
                            break;

                        case "X":
                            ButtonX.Content = (string)w;
                            ButtonX.Template = (ControlTemplate)FindResource("Hexagon3");
                            if ((string)w.Attribute("Unexplored") == "True") ButtonX.Template = (ControlTemplate)FindResource("Hexagon2");
                            break;

                    }

                }
            }
            catch
            { }



        }

        public void Scan()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Spin;
            worker.RunWorkerAsync();

        }
        public void Spin(object sender, DoWorkEventArgs e)
        {
            SolidColorBrush GreenBrush = new SolidColorBrush(Color.FromArgb(50, 0, 255, 0));
            ControlTemplate g = new ControlTemplate();

            int Delay = 50;

            ButtonX.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            { ButtonX.Template = (ControlTemplate)FindResource("Hexagon0"); }));
            System.Threading.Thread.Sleep(Delay);
            ButtonX.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            { ButtonX.Template = (ControlTemplate)FindResource("Hexagon1"); }));
            for (int i = 0; i < 1; i++)
            {
                ButtonE.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                { ButtonE.Template = (ControlTemplate)FindResource("Hexagon0"); }));
                System.Threading.Thread.Sleep(Delay);
                ButtonE.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                { ButtonE.Template = (ControlTemplate)FindResource("Hexagon3"); }));

                ButtonD.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                { ButtonD.Template = (ControlTemplate)FindResource("Hexagon0"); }));
                System.Threading.Thread.Sleep(Delay);
                ButtonD.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                { ButtonD.Template = (ControlTemplate)FindResource("Hexagon1"); }));

                ButtonS.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                { ButtonS.Template = (ControlTemplate)FindResource("Hexagon0"); }));
                System.Threading.Thread.Sleep(Delay);
                ButtonS.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                { ButtonS.Template = (ControlTemplate)FindResource("Hexagon1"); }));

                ButtonA.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                { ButtonA.Template = (ControlTemplate)FindResource("Hexagon0"); }));
                System.Threading.Thread.Sleep(Delay);
                ButtonA.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                { ButtonA.Template = (ControlTemplate)FindResource("Hexagon3"); }));

                ButtonQ.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                { ButtonQ.Template = (ControlTemplate)FindResource("Hexagon0"); }));
                System.Threading.Thread.Sleep(Delay);
                ButtonQ.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                { ButtonQ.Template = (ControlTemplate)FindResource("Hexagon2"); }));

            }
            ButtonW.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            { ButtonW.Template = (ControlTemplate)FindResource("Hexagon0"); }));
            System.Threading.Thread.Sleep(Delay);
            ButtonW.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            { ButtonW.Template = (ControlTemplate)FindResource("Hexagon1"); }));
            System.Threading.Thread.Sleep(Delay);

            for (int i = 0; i < 3; i++)
            {

                ButtonD.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                { ButtonD.Template = (ControlTemplate)FindResource("Hexagon5"); }));
                System.Threading.Thread.Sleep(Delay);

                ButtonD.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                { ButtonD.Template = (ControlTemplate)FindResource("Hexagon4"); }));
                System.Threading.Thread.Sleep(Delay);
            }


        }


        bool AutoDensityScan;

        private void image2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            if (e.ChangedButton == MouseButton.Right)
            {
                if (AutoDensityScan)
                {
                    AutoDensityScan = false;
                    RefreshImage.Source = new BitmapImage(new Uri(@"/Controls;component/Images/refresh2.png", UriKind.Relative));
                }
                else
                {
                    AutoDensityScan = true;
                    RefreshImage.Source = new BitmapImage(new Uri(@"/Controls;component/Images/refresh3.png", UriKind.Relative));
                }
            }
            else
            {
                Scan();
            }


        }


    }
}
