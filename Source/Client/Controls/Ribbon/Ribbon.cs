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

            groupViews = GetTemplateChild("GroupViews") as Grid;
            if (groupViews == null) return;


            tabItems.Clear();
            foreach (RibbonTab tab in Items)
            {
                tabView.MenuItems.Add(new NavigationViewItem()
                {
                    Content = tab.Header,
                    AccessKey = tab.AccessKey,
                    KeyTipTarget = tab.KeyTipTarget,
                    Icon = tab.Icon

                });

                //tabItems.Add(new NavigationViewItem()
                //{
                //    Content = tab.Header,
                //    AccessKey = tab.AccessKey,
                //    KeyTipTarget = tab.KeyTipTarget,
                //    Icon = tab.Icon

                //});

                StackPanel stackPanel = new()
                {
                    Orientation = Orientation.Horizontal,
                    Visibility = Visibility.Collapsed
                };

                foreach (RibbonGroup group in tab.Items)
                {
                    //Button btn = new Button();
                    //btn.Content = group.Header;
                    stackPanel.Children.Add(group);
                }

                groupViews.Children.Add(stackPanel);

                //UIElement uiElement =
                //        (UIElement)ItemContainerGenerator.ContainerFromItem(item);
                //ribbonGroup = ItemContainerGenerator.ContainerFromItem(item) as RibbonGroup;
            }

            groupViews.Children[0].Visibility = Visibility.Visible;

            tabView.SelectedItem = tabView.MenuItems[0];
            lastSelecteItem = tabView.SelectedItem as NavigationViewItem;
            //tabView.MenuItemsSource = tabItems;
            //tabView.SelectedItem = tabItems.FirstOrDefault();
            //lastSelecteItem = tabItems.FirstOrDefault();

            tabView.SelectionChanged += TabView_SelectionChangedAsync;

        }



        private void TabView_SelectionChangedAsync(NavigationView sender, NavigationViewSelectionChangedEventArgs e)
        {
            int selectedIndex = 0;

            for (int i = 0; i < tabView.MenuItems.Count; i++)
            {
                var navItem = tabView.MenuItems[i] as NavigationViewItem;

                if (navItem == lastSelecteItem)
                { 
                    selectedIndex = i;
                    break;
                }
            }

            if (e.IsSettingsSelected)
            {
                if (SettingsSelected != null)
                {
                    SettingsSelected.Invoke(this, new RoutedEventArgs());

                    // Select previously selected item using dispatcher.
                    tabView.DispatcherQueue.TryEnqueue(() =>
                    {
                        tabView.SelectedItem = tabView.MenuItems[selectedIndex];
                    });
                }
            }
            else
            {
                lastSelecteItem = e.SelectedItem as NavigationViewItem;

                if (ButtonClick != null)
                {
                    ButtonClick.Invoke(this, new ButtonClickEventArgs()
                    {
                        SelectedItem = lastSelecteItem
                    });
                }

                for (int i = 0; i < tabView.MenuItems.Count; i++)
                {
                    var navItem = tabView.MenuItems[i] as NavigationViewItem;

                    groupViews.Children[i].Visibility =
                        (navItem == lastSelecteItem) ? Visibility.Visible : Visibility.Collapsed;


                    //if (navItem == lastSelecteItem)
                    //{
                    //    groupViews.Children[i].Visibility = Visibility.Visible;
                    //}
                    //else
                    //{
                    //    groupViews.Children[i].Visibility = Visibility.Collapsed;
                    //}
                }


            }

            //// Get the currently selected item.
            //var item = sender.SelectedItem as NavigationViewItem;

            //// Get the tab item for the selected tab.
            //var sourceitem = tabItems.Where(n => n == item).FirstOrDefault();

            //// Item will not be found if settigns button was seleted.
            //if (sourceitem != null)
            //{
            //    // Save the item to be restore by ResetSelectedTab
            //    lastSelecteItem = sourceitem;
            //}
        }

        private Grid rootGrid;

        private NavigationView tabView;
        private List<NavigationViewItem> tabItems;

        //private RibbonGroup ribbonGroup;
        private Grid groupViews;

        private ItemsControl itemsControl;
    }
}
