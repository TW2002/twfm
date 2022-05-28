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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FirstMate.UserControls
{
    public sealed partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            this.InitializeComponent();

            //SettingsNavigation.SelectedItem = SettingsNavigation.MenuItems.ElementAt(1);
            //AccountSettings.MenuItems.First()

            //SettingsNavigation.SelectedItem = SettingsNavigation.MenuItems
            //    .OfType<NavigationViewItem>().First();
            SettingsNavigation.SelectedItem = AccountSettings;
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = args.SelectedItem as NavigationViewItem;
            if (selectedItem != null)
            {
                if (selectedItem.Tag == null)
                {
                    if (!selectedItem.IsExpanded)
                    {
                        foreach (NavigationViewItem item in sender.MenuItems)
                        {
                            if (item.Content != selectedItem.Content)
                            {
                                item.IsExpanded = false;
                            }
                        }

                        //if (!selectedItem.IsChildSelected)
                        if (true)
                        {
                            switch (selectedItem.Content.ToString())
                            {
                                case "Account":
                                    SettingsNavigation.SelectedItem = AccountSettings;
                                    break;
                                case "Database":
                                    SettingsNavigation.SelectedItem = GameSettings;
                                    break;
                                case "Terminal":
                                    SettingsNavigation.SelectedItem = ProxySettings;
                                    break;
                                case "Appearance":
                                    SettingsNavigation.SelectedItem = ThemeSettings;
                                    break;
                            }
                        }
                    }//sender.SelectedItem = sender.MenuItems[1];
                }
                else
                {
                    //string selectedItemTag = selectedItem.Tag.ToString();
                    //sender.Header = "Sample Page " + selectedItemTag.Substring(selectedItemTag.Length - 1);
                    //string pageName = "FirstMate.Pages." + selectedItem.Tag + "Settings";
                    Type pageType = Type.GetType("FirstMate.Pages.SettingsPage");
                    //contentFrame.Navigate(pageType);
                    contentFrame.Navigate(pageType, selectedItem.Tag as string);
                }
            }
        }

        private void AutoSuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }
    }
}
