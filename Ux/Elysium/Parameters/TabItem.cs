using System.Windows;

using Elysium.Extensions;

using JetBrains.Annotations;

namespace Elysium.Parameters
{
    [PublicAPI]
    public static class TabItem
    {
        [PublicAPI]
        public static readonly DependencyProperty HeaderStyleProperty =
            DependencyProperty.RegisterAttached("HeaderStyle", typeof(Style), typeof(TabItem),
                                                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        [PublicAPI]
        [AttachedPropertyBrowsableForType(typeof(System.Windows.Controls.TabItem))]
        public static Style GetHeaderStyle([NotNull] System.Windows.Controls.TabItem obj)
        {
            ValidationHelper.NotNull(obj, "obj");
            return (Style)obj.GetValue(HeaderStyleProperty);
        }

        [PublicAPI]
        public static void SetHeaderStyle([NotNull] System.Windows.Controls.TabItem obj, Style value)
        {
            ValidationHelper.NotNull(obj, "obj");
            obj.SetValue(HeaderStyleProperty, value);
        }
    }
}
