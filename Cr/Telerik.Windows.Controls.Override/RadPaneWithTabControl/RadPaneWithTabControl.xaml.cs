using System;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Controls.Override.RadPaneWithTabControl
{
    /// <summary>
    /// RadPaneWithTabControl.xaml 的交互逻辑
    /// </summary>
    public partial class RadPaneWithTabControl : RadPane
    {
        public RadPaneWithTabControl()
        {
            InitializeComponent();

            this.DataContext = this;
            this.TabControl.OnItemsChangeded += new System.EventHandler(TabControl_OnItemsChangeded);
            this.TabControl.Pined += new EventHandler(TabControl_Pined);
            CheckVisibility();
            this.PaneHeaderVisibility = Visibility.Collapsed;
            

        }



        void TabControl_Pined(object sender, EventArgs e)
        {
            this.IsPinned = !this.IsPinned;
            
            //if (this.IsPinned)
            //{
            //    this.PaneHeaderVisibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    //this.PaneHeaderVisibility = Visibility.Visible;
            //}
        }

        private void TabControl_OnItemsChangeded(object sender, System.EventArgs e)
        {
            CheckVisibility();
        }


        private bool _laststates;
        private bool _canEx;
        private void CheckVisibility()
        {
            if (this.TabControl.Items.Count == 0)
            {
                //this.Visibility = Visibility.Collapsed;
                _laststates = this.IsPinned;
                _canEx = true;
                this.IsPinned = false ;
                this.SetVisiable(Visibility.Hidden);
            }
            else
            {
                if (_laststates && _canEx ) this.IsPinned = true ;
                _canEx = false;
                //this.Visibility = Visibility.Visible;
                this.SetVisiable(Visibility.Visible);
            }
        }

        void SetVisiable(Visibility visi)
        {
            this.Visibility = visi;
            var g = this.GetVisualAncestor(this, typeof (RadPaneGroup));
            if (g == null) return;
            var gg = g as RadPaneGroup;
            if (gg == null) return;
            gg.Visibility = visi;

        }

        /// <summary>
        /// 获取指定DependencyObject对象的满足类型为type的父类  搜索到第一个满足条件时  返回
        /// </summary>
        /// <param name="d">指定DependencyObject对象</param>
        /// <param name="type">目标类型</param>
        /// <returns>指定DependencyObject对象的满足类型为type的第一个父类</returns>
          DependencyObject GetVisualAncestor(DependencyObject d, Type type)
        {
            DependencyObject item = VisualTreeHelper.GetParent(d);

            while (item != null)
            {
                if (item.GetType() == type) return item;
                item = VisualTreeHelper.GetParent(item);
            }

            return null;
        }



        public static readonly DependencyProperty PropertyPrismRegionName =
            DependencyProperty.Register("PrismRegionName", typeof(string),
                                        typeof(RadPaneWithTabControl), new PropertyMetadata("null"));

        public string PrismRegionName
        {
            get { return (string)GetValue(PropertyPrismRegionName); }
            set { SetValue(PropertyPrismRegionName, value); }
        }
    }
}
