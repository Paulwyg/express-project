using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj2090Module.ZOperatorLarge.OrdersGrp.Services;

namespace Wlst.Ux.Wj2090Module.ZOperatorLarge.OrdersGrp.Views
{
    /// <summary>
    /// OrdersLargeGrpView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wj2090Module.Services.ViewIdAssign.OrderLargeGrpViewId)]
    public partial class OrdersLargeGrpView : UserControl
    {
        public OrdersLargeGrpView()
        {
            InitializeComponent();
        }


        [Import]
        public IIOrdersGrp Model
        {
            get { return DataContext as IIOrdersGrp; }
            set { DataContext = value; }
        }

        private void FindText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Model.Query();
            }
        }

        private Dictionary<int, string> dic = new Dictionary<int, string>();
        private void GridViewDataColumn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (dic.Count == 0)
            {
                if (dic.ContainsKey(1) == false) dic.Add(1, "全部");
                if (dic.ContainsKey(2) == false) dic.Add(2, "单数");
                if (dic.ContainsKey(3) == false) dic.Add(3, "双数");
                if (dic.ContainsKey(4) == false) dic.Add(4, "隔二亮一");
            }

            var listView = sender as Telerik.Windows.Controls.RadTreeListView;
            if (listView == null) return;
            var ggg = listView.CurrentCellInfo;
        
            var rx = e.OriginalSource as TextBlock;
            if (rx == null) return;

            var para = rx.Parent as Telerik.Windows.Controls.GridView.GridViewHeaderCell;
            if (para == null) return;
            var con = para.ToString();
            if (con == null) return;
            var keys = (from t in dic where con.Contains(t.Value) select t.Key).ToList();
            if (keys.Count == 0) return;
            int soutid = keys.Max();
            if (soutid == 0) return;
            Model.SelectAllSwitchOut(soutid);


        }
    }
}
