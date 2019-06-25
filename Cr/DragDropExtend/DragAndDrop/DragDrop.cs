using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using DragDropExtend.ExtensionsHelper;

namespace DragDropExtend.DragAndDrop
{
    public static class DragDrop
    {
        private class DragInfo
        {
            public DragInfo(object sender, MouseEventArgs e)
            {
                DragStartPosition = e.GetPosition(null);
                Effects = DragDropEffects.None;
                VisualSource = sender as UIElement;
            }

            public object Data { get; set; }
            public Point DragStartPosition { get; private set; }
            public DragDropEffects Effects { get; set; }
            public UIElement VisualSource { get; private set; }
        };

        public static bool GetIsDragSource(UIElement target)
        {
            return (bool)target.GetValue(IsDragSourceProperty);
        }

        public static void SetIsDragSource(UIElement target, bool value)
        {
            target.SetValue(IsDragSourceProperty, value);
        }

        public static bool GetIsDropTarget(UIElement target)
        {
            return (bool)target.GetValue(IsDropTargetProperty);
        }

        public static void SetIsDropTarget(UIElement target, bool value)
        {
            target.SetValue(IsDropTargetProperty, value);
        }

        public static IDragSource GetDragHandler(UIElement target)
        {
            return (IDragSource)target.GetValue(DragHandlerProperty);
        }

        public static void SetDragHandler(UIElement target, IDragSource value)
        {
            target.SetValue(DragHandlerProperty, value);
        }

        public static IDropTarget GetDropHandler(UIElement target)
        {
            return (IDropTarget)target.GetValue(DropHandlerProperty);
        }

        public static void SetDropHandler(UIElement target, IDropTarget value)
        {
            target.SetValue(DropHandlerProperty, value);
        }



        public static readonly DependencyProperty DragHandlerProperty =
            DependencyProperty.RegisterAttached("DragHandler", typeof(IDragSource), typeof(DragDrop));

        public static readonly DependencyProperty DropHandlerProperty =
            DependencyProperty.RegisterAttached("DropHandler", typeof(IDropTarget), typeof(DragDrop));

        public static readonly DependencyProperty IsDragSourceProperty =
            DependencyProperty.RegisterAttached("IsDragSource", typeof(bool), typeof(DragDrop),
                new UIPropertyMetadata(false, IsDragSourceChanged));

        public static readonly DependencyProperty IsDropTargetProperty =
            DependencyProperty.RegisterAttached("IsDropTarget", typeof(bool), typeof(DragDrop),
                new UIPropertyMetadata(false, IsDropTargetChanged));

        static void IsDragSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement uiElement = (UIElement)d;

            if ((bool)e.NewValue == true)
            {
                uiElement.PreviewMouseLeftButtonDown += DragSource_PreviewMouseLeftButtonDown;
                uiElement.PreviewMouseLeftButtonUp += DragSource_PreviewMouseLeftButtonUp;
                uiElement.PreviewMouseMove += DragSource_PreviewMouseMove;
            }
            else
            {
                uiElement.PreviewMouseLeftButtonDown -= DragSource_PreviewMouseLeftButtonDown;
                uiElement.PreviewMouseLeftButtonUp -= DragSource_PreviewMouseLeftButtonUp;
                uiElement.PreviewMouseMove -= DragSource_PreviewMouseMove;
            }
        }

        static void IsDropTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement uiElement = (UIElement)d;

            if ((bool)e.NewValue == true)
            {
                uiElement.AllowDrop = true;
                uiElement.PreviewDragEnter += DropTarget_PreviewDragEnter;
                uiElement.PreviewDragLeave += DropTarget_PreviewDragLeave;
                uiElement.PreviewDragOver += DropTarget_PreviewDragOver;
                uiElement.PreviewDrop += DropTarget_PreviewDrop;
                uiElement.MouseUp += DragSource_PreviewMouseLeftButtonDownUp;

            }
            else
            {
                uiElement.AllowDrop = false;
                uiElement.PreviewDragEnter -= DropTarget_PreviewDragEnter;
                uiElement.PreviewDragLeave -= DropTarget_PreviewDragLeave;
                uiElement.PreviewDragOver -= DropTarget_PreviewDragOver;
                uiElement.PreviewDrop -= DropTarget_PreviewDrop;

            }
        }

        static void DragSource_PreviewMouseLeftButtonDownUp(object sender, MouseButtonEventArgs e)
        {
            _mBolPreviewMouseLeftButtonDown = false;
        }

        static bool HitTestScrollBar(object sender, MouseEventArgs e)
        {
            HitTestResult hit = VisualTreeHelper.HitTest((Visual)sender, e.GetPosition((IInputElement)sender));
            if (hit == null) return true;
            return hit.VisualHit.GetVisualAncestor<System.Windows.Controls.Primitives.ScrollBar>() != null;
        }

        static void Scroll(DependencyObject o, DragEventArgs e)
        {
            ScrollViewer scrollViewer = o.GetVisualDescendent<ScrollViewer>();

            if (scrollViewer == null) return;
            Point position = e.GetPosition(scrollViewer);
            double scrollMargin = Math.Min(scrollViewer.FontSize * 2, scrollViewer.ActualHeight / 2);

            if (position.X >= scrollViewer.ActualWidth - scrollMargin &&
                scrollViewer.HorizontalOffset < scrollViewer.ExtentWidth - scrollViewer.ViewportWidth)
            {
                scrollViewer.LineRight();
            }
            else if (position.X < scrollMargin && scrollViewer.HorizontalOffset > 0)
            {
                scrollViewer.LineLeft();
            }
            else if (position.Y >= scrollViewer.ActualHeight - scrollMargin &&
                     scrollViewer.VerticalOffset < scrollViewer.ExtentHeight - scrollViewer.ViewportHeight)
            {
                scrollViewer.LineDown();
            }
            else if (position.Y < scrollMargin && scrollViewer.VerticalOffset > 0)
            {
                scrollViewer.LineUp();
            }
        }

        static void DragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mBolPreviewMouseLeftButtonDown = true;
        }

        static void DragSource_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _mBolPreviewMouseLeftButtonDown = false;
            _mDragInfo = null;

        }

        static void DragSource_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_mDragInfo == null && _mBolPreviewMouseLeftButtonDown)
            {
                _mBolPreviewMouseLeftButtonDown = false;
                if (HitTestScrollBar(sender, e))
                {
                    _mDragInfo = null;
                    return;
                }
                _mDragInfo = new DragInfo(sender, e);
                IDragSource dragHandler = GetDragHandler(_mDragInfo.VisualSource);

                if (dragHandler != null)
                {
                    _mDragInfo.Data = dragHandler.DragData();
                }
            }

            if (_mDragInfo != null)
            {
                Point dragStart = _mDragInfo.DragStartPosition;
                Point position = e.GetPosition(null);

                if (Math.Abs(position.X - dragStart.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - dragStart.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    var dragHandler = GetDragHandler(_mDragInfo.VisualSource);

                    if (dragHandler != null)
                    {
                        _mDragInfo.Effects = dragHandler.StartDrag(_mDragInfo.Data);

                        if (_mDragInfo.Effects != DragDropEffects.None && _mDragInfo.Data != null)
                        {
                            var data = new DataObject(MFormat.Name, _mDragInfo.Data);
                            System.Windows.DragDrop.DoDragDrop(_mDragInfo.VisualSource, data, _mDragInfo.Effects);
                            _mDragInfo = null;
                        }
                    }
                }
            }
        }

        static void DropTarget_PreviewDragEnter(object sender, DragEventArgs e)
        {
            DropTarget_PreviewDragOver(sender, e);
        }

        static void DropTarget_PreviewDragLeave(object sender, DragEventArgs e)
        {
        }

        static void DropTarget_PreviewDragOver(object sender, DragEventArgs e)
        {
            var data = (e.Data.GetDataPresent(MFormat.Name)) ? e.Data.GetData(MFormat.Name) : e.Data;
            var dropHandler = GetDropHandler((UIElement)sender);
            var effect = DragDropEffects.None;
            if (dropHandler != null)
            {
                effect = dropHandler.DragOver(data);
            }

            e.Effects = effect;
            e.Handled = true;

            Scroll((DependencyObject)sender, e);
        }

        static void DropTarget_PreviewDrop(object sender, DragEventArgs e)
        {

            var data = (e.Data.GetDataPresent(MFormat.Name)) ? e.Data.GetData(MFormat.Name) : e.Data;
            var dropHandler = GetDropHandler((UIElement)sender);

            if (dropHandler != null)
            {
                dropHandler.Drop(sender, e, data);
            }

            e.Handled = true;
        }

        static DragInfo _mDragInfo;
        static readonly DataFormat MFormat = DataFormats.GetDataFormat("Infrastructure.DragAndDrop");
        static bool _mBolPreviewMouseLeftButtonDown = false;
    }
}
