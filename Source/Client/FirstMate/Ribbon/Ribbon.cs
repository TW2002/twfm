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
    /// A basic Ribbon control.
    /// </summary>

    [ContentProperty(Name = nameof(MenuItems))]
    public class Ribbon : NavigationView
    {

        /// <summary>
        /// Initializes a new instance of the Ribbon class.
        /// </summary>
        public Ribbon()
        {
            DefaultStyleKey = typeof(Ribbon);
            //DefaultStyleResourceUri = new System.Uri("ms-appx:///Microsoft.Toolkit.Uwp.UI.Controls.Core/Themes/Generic.xaml");

            Loaded += Ribbon_Loaded;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();



            // Get RibbonContent
            ribbonContent = GetTemplateChild("RibbonContent") as ContentControl;
            rootGrid = GetTemplateChild("rootGrid") as Grid;
        }

        private void Ribbon_Loaded(object sender, RoutedEventArgs e)
        {


        }

        //public static DependencyProperty ContentProperty { get; }

        private ContentControl ribbonContent;
        private Grid rootGrid;

    }
}