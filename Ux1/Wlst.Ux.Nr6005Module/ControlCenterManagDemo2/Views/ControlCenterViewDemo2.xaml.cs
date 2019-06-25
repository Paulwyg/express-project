using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.Nr6005Module.ControlCenterManagDemo2.Services;
using Wlst.Ux.Nr6005Module.ControlCenterManagDemo2.ViewModel;

namespace Wlst.Ux.Nr6005Module.ControlCenterManagDemo2.Views
{
    /// <summary>
    /// ControlCenterView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Nr6005Module.Services.ViewIdAssign.NavToControlCenterManagDemo2Id)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ControlCenterView : UserControl
    {
        public ControlCenterView()
        {
            InitializeComponent();



            Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();

            Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd HH:mm:ss";
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
        }
       [Import]
        private IIControlCenterManagDemo2 Model
        {
            get { return DataContext as IIControlCenterManagDemo2; }
            set
            {
                DataContext = value;
                value.OnNavOnLoadSelectdRtus += new EventHandler(value_OnNavOnLoadSelectdRtus);
            }
        }

       void value_OnNavOnLoadSelectdRtus(object sender, EventArgs e)
       {
           //rtl.AutoExpandItems = true;
           //rtl.ExpandAllGroups();
           if (sender == null)
           {
                 rtl.ExpandAllHierarchyItems();
           }
           else
           {
               rtl.CollapseAllHierarchyItems();
           }
           //throw new NotImplementedException();
       }

        private void SndDeleteAlarmTime()
        {
            if (Model!=null)
            {
                Model.SndDeleteAlarmTime();
            }
        }

        private void Label_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var info = WlstMessageBox.Show("确认", "是否解除屏蔽报警！", WlstMessageBoxType.YesNo);
            if (info == WlstMessageBoxResults.Yes) SndDeleteAlarmTime();
        }

        private void RadTreeListView_ColumnReordered(object sender, Telerik.Windows.Controls.GridViewColumnEventArgs e)
        {
            rtl.AutoExpandItems = true;
            rtl .ExpandAllGroups();
         
        }

        private int x = 0;
        private void RadTreeListView_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            x = 1;
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

            var listView = sender as Telerik.Windows.Controls.RadTreeListView;
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
