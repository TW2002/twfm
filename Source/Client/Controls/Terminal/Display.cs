using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
    public sealed class Display : Control
    {
        public event EventHandler<RoutedEventArgs> MoveResizeClick;


        public Display()
        {
            DefaultStyleKey = typeof(Display);
        }

        protected override void OnApplyTemplate()
        {
            var terminalDisplay = GetTemplateChild("TerminalDisplay") as RichTextBlock;
            if (terminalDisplay == null) return;

            //terminalDisplay.ContextFlyout.Opening += Menu_Opening;
            //terminalDisplay.SelectionFlyout.Opening += Menu_Opening;



            // Customize some properties on the RichTextBlock.
            Run run = new()
            {
                Text = "Ready for Combat!",
                Foreground = new SolidColorBrush(Colors.LimeGreen)
            };

            // Add the Run to the Paragraph, the Paragraph to the RichTextBlock.
            Paragraph paragraph = new();
            paragraph.Inlines.Add(run);
            terminalDisplay.Blocks.Add(paragraph);





        }

        //private void Menu_Opening(object sender, object e)
        //{
        //    AppBarButton item = new()
        //    {
        //        //Command = new StandardUICommand(StandardUICommandKind.Share)
        //        Icon = new SymbolIcon(Symbol.FullScreen),
        //        Label = "Move / Resize"
        //    };
        //    item.Click += MoveResize_Clicked;

        //    //terminalDisplay.ContextFlyout.Items.Add(myButton);
        //    var flyout = sender as TextCommandBarFlyout;
        //    flyout.SecondaryCommands.Add(item);
        //}

        //private void MoveResize_Clicked(object sender, RoutedEventArgs e)
        //{
        //    if (MoveResizeClick != null)
        //    {
        //        MoveResizeClick.Invoke(this, new RoutedEventArgs());
        //    }
        //}
    }
}
