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
    public sealed class TransformAdorner : ContentControl
    {
        public TransformAdorner()
        {
            this.DefaultStyleKey = typeof(TransformAdorner);

            Loaded += Control_Loaded;
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {


        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button button = new() { Content = "Hello Universe..." };
            rootGrid = GetTemplateChild("rootGrid") as Grid;
            rootGrid.Children.Add(button);
        }

        private Grid rootGrid;

    }
}
