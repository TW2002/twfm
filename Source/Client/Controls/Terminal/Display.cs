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
        public Display()
        {
            DefaultStyleKey = typeof(Display);

        }

        protected override void OnApplyTemplate()
        {
            var terminalDisplay = GetTemplateChild("TerminalDisplay") as RichTextBlock;
            if (terminalDisplay == null) return;

            Run run = new Run();

            // Customize some properties on the RichTextBlock.
            terminalDisplay.IsTextSelectionEnabled = true;
            terminalDisplay.TextWrapping = TextWrapping.Wrap;
            run.Text = "This is some sample text to show the wrapping behavior.";
            //richTextBlock.Width = 200;
            run.Foreground = new SolidColorBrush(Colors.LimeGreen);

            // Add the Run to the Paragraph, the Paragraph to the RichTextBlock.
            Paragraph paragraph = new();
            paragraph.Inlines.Add(run);
            terminalDisplay.Blocks.Add(paragraph);





        }
    }
}
