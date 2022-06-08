using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Controls
{
    public sealed class TransformAdorner : ContentControl
    {
        private Grid itemDecoratorElement;
        private Thumb moveThumbElement;
        private Grid resizeThumbElement;

        public Visibility DecoratorVisibility
        {
            set 
            {
                if (itemDecoratorElement == null) return;
                itemDecoratorElement.Visibility = value; 
            }
        }


        public TransformAdorner()
        {
            this.DefaultStyleKey = typeof(TransformAdorner);

            LostFocus += Aoorner_LostFocus;
        }

        private void Aoorner_LostFocus(object sender, RoutedEventArgs e)
        {
            // Restore the cursor to an arrow.
            ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);

            if (itemDecoratorElement == null) return;

            itemDecoratorElement.Visibility = Visibility.Collapsed;
        }

        protected override void OnApplyTemplate()
        {
            itemDecoratorElement = GetTemplateChild("ItemDecorator") as Grid;


            moveThumbElement = GetTemplateChild("MoveThumb") as Thumb;
            //moveThumbElement.DragStarted += MoveThumbDragStarted;
            moveThumbElement.DragDelta += MoveThumbDragDelta;
            //moveThumbElement.PointerMoved += MoveThumbPointerMoved;
            //            moveThumbElement.PointerEntered
            moveThumbElement.PointerEntered += MoveThumb_PointerEntered;

            resizeThumbElement = GetTemplateChild("ResizeThumbs") as Grid;
            //resizeThumbElement.PointerEntered += ResizeThumb_PointerEntered;
            foreach (Thumb thumb in resizeThumbElement.Children)
            {
                thumb.PointerEntered += ResizeThumb_PointerEntered;
                thumb.DragDelta += ResizeThumb_DragDelta;
            }

        }


        /// <summary>
        /// Handle click event from Move / Resize context menu.
        /// </summary>
        private void MoveResize_Clicked(object sender, RoutedEventArgs e)
        {
            if (itemDecoratorElement == null) return;

            // Make the Item Decorator visable, and ensure it has focus.
            itemDecoratorElement.Visibility = Visibility.Visible;
            Focus(FocusState.Pointer);
        }


        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            Thickness margin = Margin;

            switch (thumb.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    if (Height - e.VerticalChange > 50)
                    {
                        margin.Top += e.VerticalChange;
                        Height -= e.VerticalChange;
                    }
                    break;
                case VerticalAlignment.Bottom:
                    if (Height + e.VerticalChange > 50)
                    {
                        Height += e.VerticalChange;
                    }
                    break;
            }

            switch (thumb.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    if (Width - e.HorizontalChange > 50)
                    {
                        margin.Left += e.HorizontalChange;
                        Width -= e.HorizontalChange;
                    }
                    break;
                case HorizontalAlignment.Right:
                    if (Width + e.HorizontalChange > 50)
                    {
                        Width += e.HorizontalChange;
                    }
                    break;
            }
            this.Margin = margin;
        }

        private void ResizeThumb_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            switch (thumb.Name)
            {
                case "TopLeftThumb":
                case "BottomRightThumb":
                    ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.SizeNorthwestSoutheast);
                    break;
                case "TopRightThumb":
                case "BottomLeftThumb":
                    ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.SizeNortheastSouthwest);
                    break;
                case "TopThumb":
                case "BottomThumb":
                    ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.SizeNorthSouth);
                    break;
                case "LeftThumb":
                case "RightThumb":
                    ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.SizeWestEast);
                    break;

            }

        }

        private void MoveThumb_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.SizeAll);
        }


        private void MoveThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            this.Margin = new()
            {
                Left = Margin.Left + e.HorizontalChange,
                Top = Margin.Top + e.VerticalChange
            };
        }


    }
}
