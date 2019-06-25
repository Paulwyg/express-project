using System.Windows;
using System.Windows.Media;

using Elysium.Extensions;

using JetBrains.Annotations;

namespace Elysium.Parameters
{
    public static class Manager
    {
        [PublicAPI]
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.RegisterAttached("Theme", typeof(Theme?), typeof(Manager),
                                                new FrameworkPropertyMetadata(null,
                                                                              FrameworkPropertyMetadataOptions.AffectsRender, OnThemeChanged));

        [PublicAPI]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static Theme? GetTheme([NotNull] FrameworkElement obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return NullableBoxingHelper<Theme>.Unbox(obj.GetValue(ThemeProperty));
        }

        [PublicAPI]
        public static void SetTheme([NotNull] FrameworkElement obj, Theme? value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ThemeProperty, NullableBoxingHelper<Theme>.Box(value));
        }

        private static void OnThemeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as FrameworkElement;
            if (control != null)
            {
                var theme = NullableBoxingHelper<Theme>.Unbox(e.NewValue);
                control.Apply(theme, GetAccentBrush(control), GetContrastBrush(control));
                TryRemove(control);
            }
        }

        [PublicAPI]
        public static readonly DependencyProperty AccentBrushProperty =
            DependencyProperty.RegisterAttached("AccentBrush", typeof(SolidColorBrush), typeof(Manager),
                                                new FrameworkPropertyMetadata(null,
                                                                              FrameworkPropertyMetadataOptions.AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
                                                                              OnAccentBrushChanged));

        [PublicAPI]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static SolidColorBrush GetAccentBrush([NotNull] FrameworkElement obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(AccentBrushProperty);
        }

        [PublicAPI]
        public static void SetAccentBrush([NotNull] FrameworkElement obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(AccentBrushProperty, value);
        }

        private static void OnAccentBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as FrameworkElement;
            if (control != null)
            {
                var accentBrush = (SolidColorBrush)e.NewValue;
                control.Apply(GetTheme(control), accentBrush, GetAccentBrush(control));
                TryRemove(control);
            }
        }

        [PublicAPI]
        public static readonly DependencyProperty ContrastBrushProperty =
            DependencyProperty.RegisterAttached("ContrastBrush", typeof(SolidColorBrush), typeof(Manager),
                                                new FrameworkPropertyMetadata(null,
                                                                              FrameworkPropertyMetadataOptions.AffectsRender |
                                                                              FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
                                                                              OnContrastBrushChanged));

        [PublicAPI]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static SolidColorBrush GetContrastBrush([NotNull] FrameworkElement obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (SolidColorBrush)obj.GetValue(ContrastBrushProperty);
        }

        [PublicAPI]
        public static void SetContrastBrush([NotNull] FrameworkElement obj, SolidColorBrush value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(ContrastBrushProperty, value);
        }

        private static void OnContrastBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as FrameworkElement;
            if (control != null)
            {
                var contrastBrush = (SolidColorBrush)e.NewValue;
                control.Apply(GetTheme(control), GetAccentBrush(control), contrastBrush);
                TryRemove(control);
            }
        }

        private static void TryRemove(FrameworkElement control)
        {
            if (GetTheme(control) == null && GetAccentBrush(control) == null && GetContrastBrush(control) == null)
            {
                control.Remove();
            }
        }
    }
}