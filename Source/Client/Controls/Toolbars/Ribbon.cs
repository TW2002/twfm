using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using System;

namespace Controls;

[ContentProperty(Name = nameof(Items))]
public sealed class Ribbon : ItemsControl
{
    private Grid rootGrid;
    private ItemsRepeater tabItems;

    public Ribbon()
    {
        this.DefaultStyleKey = typeof(Ribbon);
    }


    protected override void OnApplyTemplate()
    {
        // Get the items repeater for the tab items, and verify it is not null.
        tabItems = GetTemplateChild("TabItemsRepeater") as ItemsRepeater;
        if (tabItems == null) return;

        // Bind the tab items list to the repeater itemsource.
        tabItems.ItemsSource = Items;

        // Get the items repeater for the group items, and verify it is not null.
        var groupItems = GetTemplateChild("GroupItemsRepeater") as ItemsRepeater;
        if (groupItems == null) return;

        // Get the group items list, and bind it to the repeater itemsource.
        var groupItemsControl = Items[0] as ItemsControl;
        groupItems.ItemsSource = groupItemsControl.Items;

    }
}

[ContentProperty(Name = nameof(Items))]
public sealed class RibbonTab : ItemsControl
{
    //private Grid rootGrid;
    private AppBarButton ribbonTab;
    public string Header { get; set; }
    public string Icon { get; set; }

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

