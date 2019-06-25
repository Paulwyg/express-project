using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using Elysium.Extensions;

using JetBrains.Annotations;

namespace Elysium.Parameters
{
    [PublicAPI]
    public static class DropDown
    {
        [PublicAPI]
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.RegisterAttached("IsOpen", typeof(bool), typeof(DropDown),
                                                new FrameworkPropertyMetadata(BooleanBoxingHelper.FalseBox, FrameworkPropertyMetadataOptions.None,
                                                                              OnIsOpenChanged));

        [PublicAPI]
        [AttachedPropertyBrowsableForType(typeof(Popup))]
        public static bool GetIsOpen(Popup obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return BooleanBoxingHelper.Unbox(obj.GetValue(IsOpenProperty));
        }

        [PublicAPI]
        public static void SetIsOpen(Popup obj, bool value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(IsOpenProperty, BooleanBoxingHelper.Box(value));
        }

        private static void OnIsOpenChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ValidationHelper.NotNull(obj, "obj");
            ValidationHelper.OfType(obj, "obj", typeof(Popup));

            var popup = (Popup)obj;
            var oldIsOpen = BooleanBoxingHelper.Unbox(e.OldValue);
            var newIsOpen = BooleanBoxingHelper.Unbox(e.NewValue);
            if (!oldIsOpen && newIsOpen)
            {
                popup.Opened += OnOpened;
            }
            if (oldIsOpen && !newIsOpen)
            {
                popup.Opened -= OnOpened;
            }
        }

        private static void OnOpened(object sender, EventArgs e)
        {
            var popup = sender as Popup;
            if (popup != null)
            {
                var target = popup.PlacementTarget as FrameworkElement;
                if (target != null)
                {
                    if (popup.Child != null)
                    {
                        var root = VisualTreeHelperExtensions.FindTopLevelParent(popup.Child) as Visual;
                        if (root != null)
                        {
                            var realPosition = root.PointToScreen(new Point(0d, 0d));
                            var calculatedPosition = target.PointToScreen(new Point(0d + popup.HorizontalOffset, target.ActualHeight - popup.VerticalOffset));
                            var isDefaultHorizontalPosition = Math.Abs(realPosition.X - calculatedPosition.X) < double.Epsilon;
                            var isDefaultVerticalPosition = Math.Abs(realPosition.Y - calculatedPosition.Y) < double.Epsilon;
                            SetIsDefaultHorizontalPosition(popup, isDefaultHorizontalPosition);
                            SetIsDefaultVerticalPosition(popup, isDefaultVerticalPosition);
                        }
                    }
                }
            }
        }

        private static readonly DependencyPropertyKey IsDefaultHorizontalPositionPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("IsDefaultHorizontalPosition", typeof(bool), typeof(DropDown),
                                                        new FrameworkPropertyMetadata(BooleanBoxingHelper.TrueBox, FrameworkPropertyMetadataOptions.None));

        [PublicAPI]
        public static readonly DependencyProperty IsDefaultHorizontalPositionProperty = IsDefaultHorizontalPositionPropertyKey.DependencyProperty;

        [PublicAPI]
        [AttachedPropertyBrowsableForType(typeof(Popup))]
        public static bool GetIsDefaultHorizontalPosition(Popup obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return BooleanBoxingHelper.Unbox(obj.GetValue(IsDefaultHorizontalPositionProperty));
        }

        private static void SetIsDefaultHorizontalPosition(Popup obj, bool value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(IsDefaultHorizontalPositionPropertyKey, BooleanBoxingHelper.Box(value));
        }

        private static readonly DependencyPropertyKey IsDefaultVerticalPositionPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("IsDefaultVerticalPosition", typeof(bool), typeof(DropDown),
                                                        new FrameworkPropertyMetadata(BooleanBoxingHelper.TrueBox, FrameworkPropertyMetadataOptions.None));

        [PublicAPI]
        public static readonly DependencyProperty IsDefaultVerticalPositionProperty = IsDefaultVerticalPositionPropertyKey.DependencyProperty;

        [PublicAPI]
        [AttachedPropertyBrowsableForType(typeof(Popup))]
        public static bool GetIsDefaultVerticalPosition(Popup obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return BooleanBoxingHelper.Unbox(obj.GetValue(IsDefaultVerticalPositionProperty));
        }

        private static void SetIsDefaultVerticalPosition(Popup obj, bool value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(IsDefaultVerticalPositionPropertyKey, BooleanBoxingHelper.Box(value));
        }

        [PublicAPI]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Suppression is OK here.")]
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible", Justification = "This field used as callback")]
        public static CustomPopupPlacementCallback PopupPlacementCallback = (popupSize, targetSize, offset) =>
        {
            var mainPlace = new CustomPopupPlacement(new Point(0d, targetSize.Height - offset.Y * 2), PopupPrimaryAxis.None);
            var topPlace = new CustomPopupPlacement(new Point(0d, -popupSize.Height), PopupPrimaryAxis.Vertical);
            var leftPlace = new CustomPopupPlacement(new Point(-popupSize.Width + targetSize.Width, targetSize.Height - offset.Y * 2),
                                                     PopupPrimaryAxis.Horizontal);
            return new[] { mainPlace, topPlace, leftPlace };
        };
    }
}