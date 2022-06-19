using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using System;

namespace Controls;

[ContentProperty(Name = nameof(Items))]
public sealed class Ribbon : ItemsControl
{
    //private ItemsRepeater tabItems;
    ItemsRepeater groupItems;

    public Ribbon()
    {
        this.DefaultStyleKey = typeof(Ribbon);
    }


    protected override void OnApplyTemplate()
    {
        // Get the items repeater for the tab items, and verify it is not null.
        var tabItems = GetTemplateChild("TabItemsRepeater") as ItemsRepeater;
        if (tabItems == null) return;

        // Bind the tab items list to the repeater itemsource.
        tabItems.ItemsSource = Items;

        foreach(RibbonTab item in Items)
        {
            item.TabChecked += RibbonTabChecked;
        }

        //Items.FirstOrDefault()
        RibbonTab tab = Items[0] as RibbonTab;
        tab.IsSelected = true;

        // Get the items repeater for the group items, and verify it is not null.
        groupItems = GetTemplateChild("GroupItemsRepeater") as ItemsRepeater;
        if (groupItems == null) return;

        // Get the group items list, and bind it to the repeater itemsource.
        var groupItemsControl = Items[0] as ItemsControl;
        groupItems.ItemsSource = groupItemsControl.Items;

    }

    private void RibbonTabChecked(object sender, RoutedEventArgs e)
    {
        var tab = sender as RibbonTab;

        // Deselect all other tabs.
        foreach (RibbonTab item in Items)
        {
            if (item != tab) item.IsSelected = false;
        }

        // Verify items repeater for the group items is not null.
        if (groupItems == null) return;

        // Get the group items list, and bind it to the repeater itemsource.
        groupItems.ItemsSource = tab.Items;
    }
}

[ContentProperty(Name = nameof(Items))]
public sealed class RibbonTab : ItemsControl
{
    public event EventHandler<RoutedEventArgs> TabChecked;

    //private Grid rootGrid;
    private ToggleButton ribbonTab;
    public string Header { get; set; }
    public string Icon { get; set; }
    public bool IsChecked { get; set; }

    public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
        nameof(IsSelected),
        typeof(object),
        typeof(RibbonTab),
        new PropertyMetadata(string.Empty));

    public object IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }


    public Visibility IconVisibility
    {
        get
        {
            return String.IsNullOrEmpty(Icon) ?
                Visibility.Collapsed : Visibility.Visible;
        }
    }


    public RibbonTab()
    {
        this.DefaultStyleKey = typeof(RibbonTab);

    }


    protected override void OnApplyTemplate()
    {
        ribbonTab = GetTemplateChild("RibbonTab") as ToggleButton;
        if (ribbonTab == null) return;

        ribbonTab.Checked += RibbonTabChecked;

    }

    private void RibbonTabChecked(object sender, RoutedEventArgs e)
    {
        if (TabChecked != null)
        {
            TabChecked.Invoke(this, new RoutedEventArgs());
        }
    }
}

[ContentProperty(Name = nameof(Items))]
public sealed class RibbonGroup : ItemsControl
{
    //private Grid rootGrid;
    public string Header { get; set; }


    public RibbonGroup()
    {
        this.DefaultStyleKey = typeof(RibbonGroup);

    }


    protected override void OnApplyTemplate()
    {
        // Get the items repeater for the command items, and verify it is not null.
        var commandItems = GetTemplateChild("CommandItemsRepeater") as ItemsRepeater;
        if (commandItems == null) return;

        // Bind the command items list to the repeater itemsource.
        commandItems.ItemsSource = Items;
    }
}

