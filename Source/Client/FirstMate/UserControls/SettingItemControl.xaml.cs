using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FirstMate.UserControls
{
    public sealed partial class Setting : UserControl
    {
        public string Header { get; set; }
        public string Value { get; set; }

        public Setting()
        {
            this.InitializeComponent();
        }
    }
}
