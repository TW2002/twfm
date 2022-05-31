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
using Microsoft.UI.Xaml.Input;

namespace Controls
{

    [ContentProperty(Name = nameof(Items))]
    public class Ribbon : ItemsControl
    {
        /// <summary>
        /// Event raised when user click on a ribbon button.
        /// </summary>
        public event TypedEventHandler<Ribbon, ButtonClickEventArgs> ButtonClick;

        public event EventHandler<RoutedEventArgs> SettingsSelected;


        private NavigationViewItem lastSelecteItem;


        public Ribbon()
        {
            tabItems = new();

            DependencyProperty MenuItemsProperty = DependencyProperty.Register(
            nameof(Items), typeof(NavigationViewItem), typeof(Ribbon),
            new PropertyMetadata(default(ItemsControl)));

            this.DefaultStyleKey = typeof(Ribbon);
        }


        /// <summary>
        /// Reset the selected item back to the previously selected item.
        /// </summary>
        public void ResetSelectedTab()
        {
            if (tabView == null) return;

            // Get the currently selected Tab Item.
            var item = tabView.SelectedItem as NavigationViewItem;

            // Item will be null if Settings is selected.
            if(item.Content == null)
            {
                // Restore the previously selected tab from SelcetionChanged event.
                tabView.SelectedItem = lastSelecteItem;
            }
        }


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //rootGrid = GetTemplateChild("RootGrid") as Grid;
            //foreach(var child in rootGrid.Children)
            //{
            //    if (child is NavigationView)
            //    {
            //        navigationView = child as NavigationView;
            //    }
            //    if (child is ItemsControl)
            //    {
            //        itemsControl = child as ItemsControl;
            //    }
            //}

            //tabView = GetTemplateChild("TabView") as NavigationViewItem;

            //tabView = rootGrid.Children.Where(c => c.GetType() == typeof(NavigationViewItem)).FirstOrDefault() as NavigationViewItem;
            //Button button = new() { Content = "Hello Universe..." };
            //rootGrid.Children.Add(button);


            tabView = GetTemplateChild("TabView") as NavigationView;
            if (tabView == null) return;

            tabItems.Clear();
            foreach (RibbonTab tab in Items)
            {
                tabItems.Add(new NavigationViewItem()
                {
                    Content = tab.Header,
                    AccessKey = tab.AccessKey,
                    KeyTipTarget = tab.KeyTipTarget,
                    Icon = tab.Icon

                });

                groupView = GetTemplateChild("GroupView") as StackPanel;
                if (groupView == null) return;

                foreach (RibbonGroup group in tab.Items)
                {
                    Button btn = new Button();
                    btn.Content = group.Header;
                    groupView.Children.Add(btn);
                }

                //UIElement uiElement =
                //        (UIElement)ItemContainerGenerator.ContainerFromItem(item);
                //ribbonGroup = ItemContainerGenerator.ContainerFromItem(item) as RibbonGroup;
            }

            tabView.MenuItemsSource = tabItems;
            tabView.SelectedItem = tabItems.FirstOrDefault();
            lastSelecteItem = tabItems.FirstOrDefault();

            tabView.SelectionChanged += NavigationView_SelectionChanged;

        }



        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs e)
        {
            if (e.IsSettingsSelected)
            {
                if (SettingsSelected != null)
                {
                    SettingsSelected.Invoke(this, new RoutedEventArgs());

                    //TODO - Select previously selected item.
                    //sender.SelectedItem = tabItems.FirstOrDefault();
                }
            }
            else
            {
                ButtonClickEventArgs args = new();
                args.SelectedItem = e.SelectedItem;

                if (ButtonClick != null)
                {
                    ButtonClick.Invoke(this, args);
                }
            }

            // Get the currently selected item.
            var item = sender.SelectedItem as NavigationViewItem;

            // Get the tab item for the selected tab.
            var sourceitem = tabItems.Where(n => n == item).FirstOrDefault();

            // Item will not be found if settigns button was seleted.
            if (sourceitem != null)
            {
                // Save the item to be restore by ResetSelectedTab
                lastSelecteItem = sourceitem;
            }
        }

        private Grid rootGrid;

        private NavigationView tabView;
        private List<NavigationViewItem> tabItems;

        //private RibbonGroup ribbonGroup;
        private StackPanel groupView;

        private ItemsControl itemsControl;
    }
}
