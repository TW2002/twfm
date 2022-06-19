using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Controls;
using FirstMate.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FirstMate.UserControls
{
    public sealed partial class RibbonControl : UserControl
    {
        public RibbonControl()
        {
            InitializeComponent();
            InitializeTeamsTab();
        }

        private void InitializeTeamsTab()
        {
            RibbonTab rt = new() {
                Header = "Team"
            };

            RibbonGroup rg = new() {
                Header = "Corporation"
            };
            rg.Items.Add(new AppBarButton() {
                Icon = new SymbolIcon(Symbol.People),
                Label = "Join",
                Margin = new Thickness(0)
            });
            
            rt.Items.Add(rg);
            MainRibbon.Items.Insert(3, rt);
        }

        //private void Ribbon_ButtonClick(Ribbon sender, ButtonClickEventArgs args)
        //{
        //    //string s = args.SelectedItem.ToString();
        //}

        //private void Ribbon_SettingsSelected(object sender, RoutedEventArgs e)
        //{
        //    Window window = WindowManager.GetWindowForElement(this);
        //    TabViewPage page = window.Content as TabViewPage;
        //    page.ShowSettingsTab();
        //}

        private void Game_Selected(object sender, SelectionChangedEventArgs e)
        {
            var listItem = e.AddedItems[0] as ListBoxItem;
            GameButton.Content = listItem.Content.ToString();
            GameButton.Flyout.Hide();
        }

        private void Proxy_Selected(object sender, SelectionChangedEventArgs e)
        {
            var listItem = e.AddedItems[0] as ListBoxItem;
            GameButton.Content = listItem.Content.ToString();
            GameButton.Flyout.Hide();
        }

        private void Bot_Selected(object sender, SelectionChangedEventArgs e)
        {
            var listItem = e.AddedItems[0] as ListBoxItem;
            GameButton.Content = listItem.Content.ToString();
            GameButton.Flyout.Hide();
        }
    }
}
