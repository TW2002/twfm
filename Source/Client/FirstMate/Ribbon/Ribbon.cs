using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System.Linq;
using Windows.UI.Xaml.Markup;
//using NavigationView = Microsoft.UI.Xaml.Controls.NavigationView;
//using NavigationViewSelectionChangedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs;

namespace FirstMate.Ribbon
{
    /// <summary>
    /// A basic Ribbon control that houses <see cref="RibbonItem"/>s
    /// </summary>
    [ContentProperty(Name = nameof(MenuItems))]
    [TemplatePart(Name = "PART_RibbonContent", Type = typeof(ContentControl))]
    [TemplatePart(Name = "PART_RibbonContentBorder", Type = typeof(Border))]
    [TemplatePart(Name = "PART_TabChangedStoryboard", Type = typeof(Storyboard))]
    public class Ribbon : NavigationView
    {
        private ContentControl _RibbonContent = null;
        private Border _RibbonContentBorder = null;
        private Storyboard _tabChangedStoryboard = null;

        /// <summary>
        /// The last selected <see cref="RibbonItem"/>.
        /// </summary>
        private RibbonItem _previousSelectedItem = null;
        private long _visibilityChangedToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ribbon"/> class.
        /// </summary>
        public Ribbon()
        {
            DefaultStyleKey = typeof(Ribbon);
            //DefaultStyleResourceUri = new System.Uri("ms-appx:///Microsoft.Toolkit.Uwp.UI.Controls.Core/Themes/Generic.xaml");

            SelectionChanged += SelectedItemChanged;
            Loaded += Ribbon_Loaded;
        }


        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_RibbonContent != null)
            {
                _RibbonContent.Content = null;
            }

            // Get RibbonContent first, since setting SelectedItem requires it
            _RibbonContent = GetTemplateChild("PART_RibbonContent") as ContentControl;
            _RibbonContentBorder = GetTemplateChild("PART_RibbonContentBorder") as Border;
            _tabChangedStoryboard = GetTemplateChild("TabChangedStoryboard") as Storyboard;

            // TODO: We could maybe optimize and use a lower-level Loaded event for what's hosting the MenuItems
            // to set SelectedItem, but then we may have to pull in another template part, so think we're OK
            // to do the Loaded event at the top level.
        }

        private void Ribbon_Loaded(object sender, RoutedEventArgs e)
        {
            // We need to select the item after the template is realized, otherwise the SelectedItem's
            // DataTemplate bindings don't properly navigate the visual tree.
            SelectedItem = MenuItems.FirstOrDefault();
        }

        private void SelectedItemChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = sender.SelectedItem as RibbonItem;
            if (item == null || item.Visibility == Visibility.Collapsed)
            {
                // If the item is now hidden, select the first item instead.
                // I can't think of any way that the visibiltiy would be null
                // and still be selectable, but let's handle it just in case.
                sender.SelectedItem = sender.MenuItems.FirstOrDefault();
                return;
            }

            // Remove the visibility PropertyChanged handler from the
            // previously selected item
            if (_previousSelectedItem != null)
            {
                _previousSelectedItem.UnregisterPropertyChangedCallback(RibbonItem.VisibilityProperty, _visibilityChangedToken);
            }

            // Register a new visibility PropertyChangedcallback for the
            // currently selected item
            _previousSelectedItem = item;
            _visibilityChangedToken =
            _previousSelectedItem.RegisterPropertyChangedCallback(RibbonItem.VisibilityProperty, SelectedItemVisibilityChanged);

            // Set the Ribbon background and start the transition animation
            _tabChangedStoryboard?.Begin();
        }

        private void SelectedItemVisibilityChanged(DependencyObject sender, DependencyProperty dp)
        {
            // If the item is not visible, default to the first tab
            if (sender.GetValue(dp) is Visibility vis && vis == Visibility.Collapsed)
            {
                // FIXME: This will cause WinUI to throw an exception if run
                // when the tabs overflow
                SelectedItem = MenuItems.FirstOrDefault();
            }
        }
    }
}