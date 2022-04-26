using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FirstMate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            ResourceDictionary appTheme = new();
            appTheme.Source = new Uri("/Themes/Dark.xaml", UriKind.Relative);

            Resources.MergedDictionaries.Add(appTheme);
        }


    }
}
