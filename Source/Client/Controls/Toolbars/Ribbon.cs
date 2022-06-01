using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Controls.Toolbars;

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
        //base.OnApplyTemplate();

        //rootGrid = GetTemplateChild("RootGrid") as Grid;
        //if (rootGrid == null) return;

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
        //base.OnApplyTemplate();

        ribbonTab = GetTemplateChild("RibbonTab") as AppBarButton;
        if (ribbonTab == null) return;

        //StackPanel tabPanel = new();

        //tabPanel.Children.Add(new SymbolIcon()
        //{
        //    Symbol = new SymbolIcon(Icon)
        //});

        //ribbonTab.Content = Header;
        //ribbonTab.Icon = Icon;
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
        base.OnApplyTemplate();

        var rootGrid = GetTemplateChild("RootGrid") as Grid;
        if (rootGrid == null) return;

        //StackPanel tabPanel = new();

        //tabPanel.Children.Add(new SymbolIcon()
        //{
        //    Symbol = new SymbolIcon(Icon)
        //});

        //ribbonTab.Content = Header;
        //ribbonTab.Icon = Icon;
    }
}

