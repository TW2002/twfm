
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls
{

    [ContentProperty(Name = nameof(Items))]
    public class RibbonGroup : ItemsControl
    {
        //public List<Button> Items { get; set; }

        public RibbonGroup()
        {
            this.DefaultStyleKey = typeof(Ribbon);

            Loaded += Control_Loaded;
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

   
        }
        
        public string Header { get; set; }
    }
}
