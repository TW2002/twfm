using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System.Linq;
using Windows.UI.Xaml.Markup;


namespace FirstMate.Ribbon;

/// <summary>
/// A <see cref="CommandBar"/> to be displayed in a <see cref="Ribbon"/>
/// </summary>
[TemplatePart(Name = "PrimaryItemsControl", Type = typeof(ItemsControl))]
[TemplatePart(Name = "MoreButton", Type = typeof(Button))]
public class RibbonItem : CommandBar
{
    private ItemsControl _primaryItemsControl;
    private Button _moreButton;

    /// <summary>
    /// Initializes a new instance of the <see cref="RibbonItem"/> class.
    /// </summary>
    public RibbonItem()
    {
        DefaultStyleKey = typeof(RibbonItem);
    }

    /// <summary>
    /// Identifies the <see cref="Header"/> property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(object),
        typeof(RibbonItem),
        new PropertyMetadata(string.Empty));

    /// <summary>
    /// Gets or sets the text or <see cref="UIElement"/> to display in the header of this Ribbon tab.
    /// </summary>
    public object Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="IsContextual"/> property.
    /// </summary>
    public static readonly DependencyProperty IsContextualProperty = DependencyProperty.Register(
        nameof(IsContextual),
        typeof(bool),
        typeof(RibbonItem),
        new PropertyMetadata(false));

    /// <summary>
    /// Gets or sets a value indicating whether this tab is contextual.
    /// </summary>
    public bool IsContextual
    {
        get => (bool)GetValue(IsContextualProperty);
        set => SetValue(IsContextualProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="OverflowButtonAlignment"/> property.
    /// </summary>
    public static readonly DependencyProperty OverflowButtonAlignmentProperty = DependencyProperty.Register(
        nameof(OverflowButtonAlignment),
        typeof(HorizontalAlignment),
        typeof(RibbonItem),
        new PropertyMetadata(HorizontalAlignment.Left));

    /// <summary>
    /// Gets or sets a value indicating the alignment of the command overflow button.
    /// </summary>
    public HorizontalAlignment OverflowButtonAlignment
    {
        get => (HorizontalAlignment)GetValue(OverflowButtonAlignmentProperty);
        set => SetValue(OverflowButtonAlignmentProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="CommandAlignment"/> property.
    /// </summary>
    public static readonly DependencyProperty CommandAlignmentProperty = DependencyProperty.Register(
        nameof(CommandAlignment),
        typeof(HorizontalAlignment),
        typeof(RibbonItem),
        new PropertyMetadata(HorizontalAlignment.Stretch));

    /// <summary>
    /// Gets or sets a value indicating the alignment of the commands in the <see cref="RibbonItem"/>.
    /// </summary>
    public HorizontalAlignment CommandAlignment
    {
        get => (HorizontalAlignment)GetValue(CommandAlignmentProperty);
        set => SetValue(CommandAlignmentProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _primaryItemsControl = GetTemplateChild("PrimaryItemsControl") as ItemsControl;
        if (_primaryItemsControl != null)
        {
            _primaryItemsControl.HorizontalAlignment = CommandAlignment;
            RegisterPropertyChangedCallback(CommandAlignmentProperty, (sender, dp) =>
            {
                _primaryItemsControl.HorizontalAlignment = (HorizontalAlignment)sender.GetValue(dp);
            });
        }

        _moreButton = GetTemplateChild("MoreButton") as Button;
        if (_moreButton != null)
        {
            _moreButton.HorizontalAlignment = OverflowButtonAlignment;
            RegisterPropertyChangedCallback(OverflowButtonAlignmentProperty, (sender, dp) =>
            {
                _moreButton.HorizontalAlignment = (HorizontalAlignment)sender.GetValue(dp);
            });
        }
    }
}