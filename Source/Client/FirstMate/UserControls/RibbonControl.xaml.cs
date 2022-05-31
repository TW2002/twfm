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
            this.InitializeComponent();
        }

        private void Ribbon_ButtonClick(Ribbon sender, ButtonClickEventArgs args)
        {
            //string s = args.SelectedItem.ToString();
        }

        private void Ribbon_SettingsSelected(object sender, RoutedEventArgs e)
        {
            Window window = WindowManager.GetWindowForElement(this);
            TabViewPage page = window.Content as TabViewPage;
            page.ShowSettingsTab();
        }
    }
}
