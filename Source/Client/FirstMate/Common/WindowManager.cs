using FirstMate.Pages;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using Windows.Graphics;

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
                    Content = new UserControls.PlayGameControl()
                    {
                        DataContext = $"NewTab"
                    }

                };
            }

            Window newWindow = new()
            {
                Content = new Pages.TabViewPage(tab)
               
            };

            TrackWindow(newWindow);

            // Get the AppWindow so we can resize it.
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(newWindow);
            WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

            // TODO: Size window on first run from screen size, and store
            //       open new windows in cascxading position.
            appWindow.Resize(new SizeInt32 { Width = 1230, Height = 700 });
            appWindow.Move(new PointInt32 { X = 20, Y = 20 });

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