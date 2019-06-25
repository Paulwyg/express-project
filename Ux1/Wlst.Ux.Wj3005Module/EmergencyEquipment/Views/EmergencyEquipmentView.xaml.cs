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
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.ViewModel;
using Wlst.Ux.WJ3005Module.EmergencyEquipment.Services;

namespace Wlst.Ux.WJ3005Module.EmergencyEquipment.Views
{
    /// <summary>
    /// EmergencyEquipmentView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(WJ3005Module.Services.ViewIdAssign.NavToEmergencyEquipmentViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EmergencyEquipmentView : UserControl
    {
        public EmergencyEquipmentView()
        {
            InitializeComponent();
        }

        [Import]
        private IIEmergencyEquipment Model
        {
            get { return DataContext as IIEmergencyEquipment; }
            set { DataContext = value; }
        }

        private Dictionary<int, string> dic = new Dictionary<int, string>();
        private void GridViewDataColumn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (dic.Count == 0)
            {
                for (int i = 1; i < 32; i++)
                {
                    if (dic.ContainsKey(i) == false) dic.Add(i, "K" + i);
                }
            }

            var listView = sender as Telerik.Windows.Controls.RadGridView;
            if (listView == null) return;
            var ggg = listView.CurrentCellInfo;
            if (ggg != null)
            {
                var mvvm = ggg.Item as TreeTmlNode;
                if (mvvm != null)
                {


                    //发布事件  选中当前节点
                    var args = new PublishEventArgs
                    {
                        EventType = PublishEventType.Core,
                        EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                    };
                    args.AddParams(mvvm.NodeId);
                    EventPublish.PublishEvent(args);

                }
            }

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
