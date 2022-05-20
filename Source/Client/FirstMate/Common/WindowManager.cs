using FirstMate.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace FirstMate
{
    // Helper class to allow the app to find the Window that contains an
    // arbitrary UIElement (GetWindowForElement).  To do this, we keep track
    // of all active Windows.  The app code must call WindowHelper.CreateWindow
    // rather than "new Window" so we can keep track of all the relevant
    // windows.  In the future, we would like to support this in platform APIs.
    public class WindowManager
    {
        static public List<Window> ActiveWindows { get; private set; }

        static public Window CreateWindow(TabViewItem tab = null)
        {
            if (tab == null)
            {
                tab = new TabViewItem()
                {
                    Header = $"Welcome",
                    IconSource = new SymbolIconSource()
                    {
                        Symbol = Symbol.Globe
                    },
                    Content = new Controls.PlayGameControl()
                    {
                        DataContext = $"NewTab"
                    }
                };
            }

            Window newWindow = new()
            {
                Content = new Pages.TabViewPage(tab),
            };

            TrackWindow(newWindow);
            return newWindow;
        }

        static public void CloseWindow(UIElement element)
        {
            Window window = GetWindowForElement(element);

            if (window != null) window.Close();
        }


        static public void TrackWindow(Window window)
        {
            if (ActiveWindows == null) ActiveWindows = new();

            window.Closed += (sender, args) => {
                ActiveWindows.Remove(window);
            };
            ActiveWindows.Add(window);
        }

        static public Window GetWindowForElement(UIElement element)
        {
            if (element.XamlRoot != null)
            {
                foreach (Window window in ActiveWindows)
                {
                    if (element.XamlRoot == window.Content.XamlRoot)
                    {
                        return window;
                    }
                }
            }
            return null;
        }

    }
}