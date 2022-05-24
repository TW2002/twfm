using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Controls
{

    [ContentProperty(Name = nameof(Items))]
    public class Ribbon : ItemsControl
    {
        /// <summary>
        /// Event raised when user click on a ribbon button.
        /// </summary>
        public event TypedEventHandler<Ribbon, ButtonClickEventArgs> ButtonClick;

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

            navigationView.SelectionChanged += NavigationView_SelectionChanged;

        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs e)
        {
            ButtonClickEventArgs args = new();
            args.SelectedItem = e.SelectedItem;

            if (ButtonClick != null)
            {
                ButtonClick.Invoke(this, args);
            }

        }

        private Grid rootGrid;

        private NavigationView navigationView;
        private List<NavigationViewItem> navigationViewItems;

        private RibbonGroup ribbonGroup;

        private ItemsControl itemsControl;
    }
}
