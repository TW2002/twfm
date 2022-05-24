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
    public class Ribbon : ItemsControl
    {
        //public List<Button> Items { get; set; }

        public Ribbon()
        {
            navigationViewItems = new();

            DependencyProperty MenuItemsProperty = DependencyProperty.Register(
            nameof(Items), typeof(NavigationViewItem), typeof(Ribbon),
            new PropertyMetadata(default(ItemsControl)));

            //DependencyProperty NavigationViewItemsProperty = DependencyProperty.Register(
            //nameof(navigationViewItems), typeof(NavigationViewItem), typeof(Ribbon),
            //new PropertyMetadata(default(ItemsControl)));


            this.DefaultStyleKey = typeof(Ribbon);



            //Items = new();

            Loaded += Control_Loaded;
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            rootGrid = GetTemplateChild("RootGrid") as Grid;
            foreach(var child in rootGrid.Children)
            {
                if (child is NavigationView)
                {
                    navigationView = child as NavigationView;
                }
                if (child is ItemsControl)
                {
                    itemsControl = child as ItemsControl;
                }
            }

            //tabView = GetTemplateChild("TabView") as NavigationViewItem;

            //tabView = rootGrid.Children.Where(c => c.GetType() == typeof(NavigationViewItem)).FirstOrDefault() as NavigationViewItem;
            //Button button = new() { Content = "Hello Universe..." };
            //rootGrid.Children.Add(button);

            if (navigationView == null) return;

            navigationViewItems.Clear();
            foreach (RibbonTab tab in Items)
            {
                navigationViewItems.Add(new NavigationViewItem()
                {
                    Content = tab.Header,
                    AccessKey = tab.AccessKey,
                    KeyTipTarget = tab.KeyTipTarget,
                    Icon = tab.Icon

                });

                foreach (RibbonGroup group in tab.Items)
                {

                }

                //UIElement uiElement =
                //        (UIElement)ItemContainerGenerator.ContainerFromItem(item);
                //ribbonGroup = ItemContainerGenerator.ContainerFromItem(item) as RibbonGroup;
            }


            navigationView.MenuItemsSource = navigationViewItems;
            navigationView.SelectedItem = navigationViewItems.FirstOrDefault();



        }

        private Grid rootGrid;

        private NavigationView navigationView;
        private List<NavigationViewItem> navigationViewItems;

        private RibbonGroup ribbonGroup;

        private ItemsControl itemsControl;
    }
}
