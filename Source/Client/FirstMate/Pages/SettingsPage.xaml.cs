using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Linq;

namespace FirstMate.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        InitializeComponent();
    }


    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        StackPanel panel = new();

        string tag = e.Parameter as string;

        foreach(StackPanel p in RootPannel.Children)
        {
            if (p.Name == tag)
            {
                p.Visibility = Visibility.Visible;
            }
            else 
            {
                p.Visibility = Visibility.Collapsed;
            }
        }

    }

}