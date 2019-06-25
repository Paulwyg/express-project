using System.Diagnostics.CodeAnalysis;
using System.Windows;

using Elysium.Extensions;

using JetBrains.Annotations;

namespace Elysium.Parameters
{
    public static class Bullet
    {
        [PublicAPI]
        public static readonly DependencyProperty DecoratorSizeProperty =
            DependencyProperty.RegisterAttached("DecoratorSize", typeof(double), typeof(Bullet),
                                                new FrameworkPropertyMetadata(16d, FrameworkPropertyMetadataOptions.AffectsMeasure,
                                                                              null, DoubleUtil.CoerceNonNegative));

        [PublicAPI]
        [AttachedPropertyBrowsableForType(typeof(System.Windows.Controls.CheckBox))]
        [AttachedPropertyBrowsableForType(typeof(System.Windows.Controls.RadioButton))]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static double GetDecoratorSize([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ValidationHelper.OfTypes(obj, "obj", typeof(System.Windows.Controls.CheckBox), typeof(System.Windows.Controls.RadioButton));
            DoubleUtil.EnsureNonNegative();
            return BoxingHelper<double>.Unbox(obj.GetValue(DecoratorSizeProperty));
        }

        [PublicAPI]
        public static void SetDecoratorSize([NotNull] DependencyObject obj, double value)
        {
            ValidationHelper.NotNull(obj, "obj");
            ValidationHelper.OfTypes(obj, "obj", typeof(System.Windows.Controls.CheckBox), typeof(System.Windows.Controls.RadioButton));
            obj.SetValue(DecoratorSizeProperty, value);
        }

        [PublicAPI]
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.RegisterAttached("Size", typeof(double), typeof(Bullet),
                                                new FrameworkPropertyMetadata(8d, FrameworkPropertyMetadataOptions.AffectsMeasure,
                                                                              null, DoubleUtil.CoerceNonNegative));

        [PublicAPI]
        [AttachedPropertyBrowsableForType(typeof(System.Windows.Controls.CheckBox))]
        [AttachedPropertyBrowsableForType(typeof(System.Windows.Controls.RadioButton))]
        [SuppressMessage("Microsoft.Contracts", "Ensures", Justification = "Can't be proven.")]
        public static double GetSize([NotNull] DependencyObject obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            ValidationHelper.OfTypes(obj, "obj", typeof(System.Windows.Controls.CheckBox), typeof(System.Windows.Controls.RadioButton));
            DoubleUtil.EnsureNonNegative();
            return BoxingHelper<double>.Unbox(obj.GetValue(SizeProperty));
        }

        [PublicAPI]
        public static void SetSize([NotNull] DependencyObject obj, double value)
        {
            ValidationHelper.NotNull(obj, "obj");
            ValidationHelper.OfTypes(obj, "obj", typeof(System.Windows.Controls.CheckBox), typeof(System.Windows.Controls.RadioButton));
            obj.SetValue(SizeProperty, value);
        }
    }
}