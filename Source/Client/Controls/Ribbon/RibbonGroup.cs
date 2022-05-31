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
        private Grid rootGrid;
        private ItemsRepeater groupItems;

        public RibbonGroup()
        {
            DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header), typeof(string), typeof(RibbonGroup),
            new PropertyMetadata(default(ItemsControl)));


            this.DefaultStyleKey = typeof(RibbonGroup);

            Loaded += Control_Loaded;
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            rootGrid = GetTemplateChild("RootGrid") as Grid;

            groupItems = GetTemplateChild("GroupItemsRepeater") as ItemsRepeater;
            if (groupItems == null) return;

            groupItems.ItemsSource = Items;

            var groupHeader = GetTemplateChild("GroupHeader") as TextBlock;
            if (groupHeader == null) return;

            //TODO: Set header with a binding instead.
            groupHeader.Text = Header;
        }

        public string Header { get; set; }
    }
}
