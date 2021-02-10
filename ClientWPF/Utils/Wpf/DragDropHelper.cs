using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ClientWPF.Utils.Wpf
{
    public class DragDropHelper
    {
        public static void SetDragSource(DependencyObject obj, object source)
        {
            obj.SetValue(DragSourceProperty, source);
        }

        public static object GetDragSource(DependencyObject obj)
        {
            return obj.GetValue(DragSourceProperty);
        }

        // Using a DependencyProperty as the backing store for DragSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DragSourceProperty =
            DependencyProperty.RegisterAttached("DragSource", typeof(object), typeof(DragDropHelper), new FrameworkPropertyMetadata(DragSourceChanged));

        

        private static void DragSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (UIElement)d;
            element.PreviewMouseMove += StartDrag;
        }

        private static void StartDrag(object sender, MouseEventArgs e)
        {
            var element = (UIElement)sender;
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(element, element.GetValue(DragSourceProperty), DragDropEffects.Move);
            }
        }
    }
}
