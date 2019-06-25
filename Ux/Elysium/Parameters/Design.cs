using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;

using Elysium.Extensions;

using JetBrains.Annotations;

namespace Elysium.Parameters
{
    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Justification = "Do not change type name.")]
    [PublicAPI]
    public static class Design
    {
        [PublicAPI]
        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.RegisterAttached("Theme", typeof(Theme?), typeof(Design),
                                                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnThemeChanged));

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
                if (DesignerProperties.GetIsInDesignMode(control))
                {
                    Manager.SetTheme(control, NullableBoxingHelper<Theme>.Unbox(e.NewValue));
                }
            }
        }

        [PublicAPI]
        public static readonly DependencyProperty AccentBrushProperty =
            DependencyProperty.RegisterAttached("AccentBrush", typeof(SolidColorBrush), typeof(Design),
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
                if (DesignerProperties.GetIsInDesignMode(control))
                {
                    Manager.SetAccentBrush(control, (SolidColorBrush)e.NewValue);
                }
            }
        }

        [PublicAPI]
        public static readonly DependencyProperty ContrastBrushProperty =
            DependencyProperty.RegisterAttached("ContrastBrush", typeof(SolidColorBrush), typeof(Design),
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
                if (DesignerProperties.GetIsInDesignMode(control))
                {
                    Manager.SetContrastBrush(control, (SolidColorBrush)e.NewValue);
                }
            }
        }
    }
}
