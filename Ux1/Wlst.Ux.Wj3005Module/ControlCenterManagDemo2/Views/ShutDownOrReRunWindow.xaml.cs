using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WindowForWlst;
using Wlst.Cr.CoreMims.Services;
using Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.Services;
using Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.ViewModel;

namespace Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.Views
{
    /// <summary>
    /// ShutDownOrReRunWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ShutDownOrReRunWindow : CustomChromeWindow
    {
        private bool _isShutDownOrReRun;
        private bool _isViewShow;

        public ShutDownOrReRunWindow(bool isShutDownOrReRun)
        {
            InitializeComponent();



            _isShutDownOrReRun = isShutDownOrReRun;
            Items.Clear();
            if (_isShutDownOrReRun)
            {

                var nodes = new List<TreeNodeBase>();
                var dic = new Dictionary<int, TreeNodeBase>();
                foreach (var f in TreeTmlNode.RegisterTmlNode)
                {
                    foreach (var l in f.Value)
                    {
                        if (l.CheckStopTml && dic.ContainsKey(l.NodeId) == false) dic.Add(l.NodeId, l);
                    }
                }

                nodes = (from t in dic select t.Value).ToList();
                foreach (var tt in nodes)
                {
                    Items.Add(new ShutDownOrReRunModel(tt));
                }

                Title = "停运界面";
            }
            else
            {
                Title = "启运界面";
                var nodes = new List<TreeNodeBase>();
                var dic = new Dictionary<int, TreeNodeBase>();
                foreach (var f in TreeTmlNode.RegisterTmlNode)
                {
                    foreach (var l in f.Value)
                    {
                        if (l.CheckStopTml && dic.ContainsKey(l.NodeId) == false) dic.Add(l.NodeId, l);
                    }
                }

                nodes = (from t in dic select t.Value).ToList();
                foreach (var tt in nodes)
                {
                    Items.Add(new ShutDownOrReRunModel(tt));
                }
            }

            DataContext = this;
            _isViewShow = true;
        }

        private ObservableCollection<ShutDownOrReRunModel> _items;

        public ObservableCollection<ShutDownOrReRunModel> Items
        {
            get { return _items ?? (_items = new ObservableCollection<ShutDownOrReRunModel>()); }
        }

        private void CmdRemove(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (!Items[i].IsCheckOut) continue;
                Items.Remove(Items[i]);
                i--;
            }
        }

        private void CmdCancel(object sender, RoutedEventArgs e)
        {
            Items.Clear();
            Close();
            _isViewShow = false;
        }

        private void CmdSave(object sender, RoutedEventArgs e)
        {
            if (_isShutDownOrReRun)
            {
                var info = Sr.ProtocolPhone.LxRtu
                    .wst_rtu_orders; // .wlst_cnt_wj3090_order_snd_paras ;//.ServerPart.wlst_rtuargsupdate_clinet_order_stopruning;
                info.Args.Addr.AddRange((from t in Items select t.NodeId).ToList());
                info.WstRtuOrders.Op = 6;
                info.WstRtuOrders.RtuIds.AddRange((from t in Items select t.NodeId).ToList());
                //info.WstCntOrderWj3090UpdateParas.OrderIs = OrderUpdateRtuParas.OrderUpdateRtuParasItem.StopRunning;
                //info.WstCntOrderWj3090UpdateParas.RtuIds.AddRange((from t in Items select t.NodeId).ToList());
                SndOrderServer.OrderSnd(info, 10, 6);
            }
            else
            {
                var info = Sr.ProtocolPhone.LxRtu
                    .wst_rtu_orders; //.ServerPart.wlst_rtuargsupdate_clinet_order_stopruning;
                //info.Args.Addr.AddRange((from t in Items select t.NodeId).ToList());
                //info.WstCntOrderWj3090UpdateParas.OrderIs = OrderUpdateRtuParas.OrderUpdateRtuParasItem.StartRunning;
                //info.WstCntOrderWj3090UpdateParas.RtuIds.AddRange((from t in Items select t.NodeId).ToList());

                info.Args.Addr.AddRange((from t in Items select t.NodeId).ToList());
                info.WstRtuOrders.Op = 6;
                info.WstRtuOrders.RtuIds.AddRange((from t in Items select t.NodeId).ToList());
                SndOrderServer.OrderSnd(info, 10, 6);
            }

            Close();
        }





        public class ShutDownOrReRunModel : TreeTmlNode
        {
            private bool _isCheckOut;

            public bool IsCheckOut
            {
                get { return _isCheckOut; }
                set
                {
                    if (_isCheckOut == value) return;
                    _isCheckOut = value;
                    RaisePropertyChanged(() => IsCheckOut);
                }
            }

            public ShutDownOrReRunModel(TreeNodeBase model)
            {
                NodeId = model.NodeId;
                NodeName = model.NodeName;
                IsCheckOut = false;
            }
        }
    }
}
