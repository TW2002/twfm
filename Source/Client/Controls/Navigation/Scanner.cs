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
    public sealed class Scanner : Control
    {
        public Scanner()
        {
            DefaultStyleKey = typeof(Scanner);

        }

        protected override void OnApplyTemplate()
        {
            var rootGrid = GetTemplateChild("RootGrid") as RichTextBlock;
            if (rootGrid == null) return;

            //TextCommandBarFlyout flyout = new();
            //rootGrid.ContextFlyout = new TextCommandBarFlyout();


            var terminalDisplay = GetTemplateChild("TerminalDisplay") as RichTextBlock;

            Run run = new Run();

        




        }
    }
}
