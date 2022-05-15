using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Controls
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : UserControl
    {
        public Game()
        {
            InitializeComponent();
        }

        private void DisplayControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                foreach (Control child in MainCanvas.Children)
                {
                    if (child != (Control)sender)
                    {
                        Selector.SetIsSelected(child, false);
                    }
                }

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

        private void ScannerControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                foreach (Control child in MainCanvas.Children)
                {
                    if (child != (Control)sender)
                    {
                        Selector.SetIsSelected(child, false);
                    }
                }

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
