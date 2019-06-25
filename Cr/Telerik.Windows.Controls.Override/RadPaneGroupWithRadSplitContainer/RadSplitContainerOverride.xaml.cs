using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Telerik.Windows.Controls.Override.RadPaneGroupWithRadSplitContainer
{
    /// <summary>
    /// RadSplitContainerOverride.xaml 的交互逻辑
    /// </summary>
    public partial class RadSplitContainerOverride : RadSplitContainer
    {
        public RadSplitContainerOverride()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty PropertyPrismRegionName =
            DependencyProperty.Register("PrismRegionName", typeof(string),
                                        typeof(RadSplitContainerOverride), new PropertyMetadata("null"));

        public string PrismRegionName
        {
            get { return (string)GetValue(PropertyPrismRegionName); }
            set { SetValue(PropertyPrismRegionName, value); }
        }

        private void RadPaneGroupOverride_OnItemsChangeded(object sender, System.EventArgs e)
        {
            if (OnItemsChangeded != null) this.OnItemsChangeded(this, e);
        }

        /// <summary>
        /// 当内容发生变化的时候呈现
        /// </summary>
        public event EventHandler OnItemsChangeded;

        /// <summary>
        /// RadPaneGroup
        /// </summary>
        public RadPaneGroup RadPaneGroupOverride
        {
            get { return this.PaneGroup; }
        }

    }
}
