using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Controls
{
    public sealed class Scanner : Control
    {
        TransformAdorner rootAdorner;

        public Scanner()
        {
            this.DefaultStyleKey = typeof(Scanner);
        }

        protected override void OnApplyTemplate()
        {
            rootAdorner = GetTemplateChild("RootAdorner") as TransformAdorner;

            var rootGrid = GetTemplateChild("RootGrid") as Grid;
            if (rootGrid == null) return;

            // Add handlers for Context Flyout.
            rootGrid.ContextFlyout = new TextCommandBarFlyout();
            rootGrid.ContextFlyout.Opening += Menu_Opening;
            rootGrid.ContextFlyout.Placement = FlyoutPlacementMode.RightEdgeAlignedTop;
        }

        private void Menu_Opening(object sender, object e)
        {
            AppBarButton item = new()
            {
                //Command = new StandardUICommand(StandardUICommandKind.Share)
                Icon = new SymbolIcon(Symbol.FullScreen),
                Label = "Move / Resize"
            };
            item.Click += MoveResize_Clicked;

            //terminalDisplay.ContextFlyout.Items.Add(myButton);
            var flyout = sender as TextCommandBarFlyout;
            flyout.SecondaryCommands.Add(item);
        }

        private void MoveResize_Clicked(object sender, RoutedEventArgs e)
        {
            // Make the Item Decorator visable, and ensure it has focus.
            if (rootAdorner == null) return;
            rootAdorner.DecoratorVisibility = Visibility.Visible;
            rootAdorner.Focus(FocusState.Pointer);
        }
    }
}
