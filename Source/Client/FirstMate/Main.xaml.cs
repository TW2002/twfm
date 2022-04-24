using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FirstMate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Main: Window
    {
        public Main()
        {
            InitializeComponent();
        }

        private void DisplayControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                if (Selector.GetIsSelected((Control)sender))
                {
                    Selector.SetIsSelected((Control)sender, false);
                }
                else
                {
                    Selector.SetIsSelected((Control)sender, true);
                }

            }

        }
    }
}
