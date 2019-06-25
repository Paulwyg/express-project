using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Ux.Wj1090Module.Partol.Services;
using Wlst.client;

namespace Wlst.Ux.Wj1090Module.Partol.ViewModel
{
    [Export(typeof (IIPartolView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class PartolViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IIPartolView
    {
        public PartolViewModel()
        {
            this.InitAction();
        }


        private ObservableCollection<LduLineItem> _collectionWj1090;

        public ObservableCollection<LduLineItem> Items
        {
            get { return _collectionWj1090 ?? (_collectionWj1090 = new ObservableCollection<LduLineItem>()); }
            set
            {
                if (value == _collectionWj1090) return;
                _collectionWj1090 = value;
                RaisePropertyChanged(() => Items);
            }
        }

        private Dictionary<Tuple<int, int>, LduLineItem> info = new Dictionary<Tuple<int, int>, LduLineItem>();

        private void LoadNode()
        {
            info.Clear();
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems)//.EquipmentInfoDictionary)
            {
                if (t.Value.RtuModel != EnumRtuModel.Wj1090 && t.Value.RtuModel != EnumRtuModel.Wj30920 && t.Value.RtuModel != EnumRtuModel.Wj30910) continue;
                if (t.Value.RtuFid == 0) continue;

                var tmps = LduLineItem.GetLduLineItem(t.Key);
                foreach (var g in tmps)
                {
                    var kessss = new Tuple<int, int>(g.ConId, g.LineId);
                    if (!info.ContainsKey(kessss)) info.Add(kessss, g);
                }
            }

            Items.Clear();
            var ordlst = (from t in info
                          orderby t.Value.PhyId
                              ascending
                          //orderby t.Value.ConId
                          //    ascending
                          //orderby t.Value.LineId
                          //    ascending
                          select t.Value).ToList();
            foreach (var g in ordlst) Items.Add(g);

        }

        public void NavOnLoad(params object[] parsObjects)
        {
            // Load();
            this.LoadNode();
            AnsCount = 0;
        }

        public void OnUserHideOrClosing()
        {
            this.Items.Clear();
            AnsCount = 0;
        }

        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "线路检测巡测"; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion

        #region attri

        private int _loCount;

        public int Count
        {
            get { return _loCount; }
            set
            {
                if (_loCount == value) return;
                _loCount = value;
                RaisePropertyChanged("Count");
            }
        }

        private int _looAnsCount;

        public int AnsCount
        {
            get { return _looAnsCount; }
            set
            {
                if (_looAnsCount == value) return;
                _looAnsCount = value;
                RaisePropertyChanged("AnsCount");
            }
        }

        #endregion


        #region  Measure

        private DateTime _dtMeasureBtn;
        private ICommand _cmdMeasureBtn;

        public ICommand CmdMeasureBtn
        {
            get { return _cmdMeasureBtn ?? (_cmdMeasureBtn = new RelayCommand(ExCmdMeasureBtn, CanCmdMeasureBtn, true)); }
        }

        private bool CanCmdMeasureBtn()
        {
            return DateTime.Now.Ticks - _dtMeasureBtn.Ticks > 300000000;
        }

        private void ExCmdMeasureBtn()
        {
            Count = Items.Count;
            _dtMeasureBtn = DateTime.Now;
            SndOrderQuery(false);
        }

        #endregion


        #region  Measure

        private DateTime _dtMeasureBtnOne;
        private ICommand _cmdMeasureBtnOne;

        public ICommand CmdMeasureBtnOne
        {
            get
            {
                return _cmdMeasureBtnOne ??
                       (_cmdMeasureBtnOne = new RelayCommand(ExCmdMeasureBtnOne, CanCmdMeasureBtnOne, true));
            }
        }

        private bool CanCmdMeasureBtnOne()
        {
            return DateTime.Now.Ticks - _dtMeasureBtnOne.Ticks > 300000000;
        }

        private void ExCmdMeasureBtnOne()
        {
            _dtMeasureBtnOne = DateTime.Now;
            SndOrderQuery(true);
        }

        #endregion
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class PartolViewModel
    {


        public void InitAction()
        {
            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxLdu.wst_svr_ans_ldu_orders,// .wlst_svr_ans_cnt_wj1090_order_measure ,//.ClientPart.wlst_Wj1090_server_ans_clinet_order_Measure,
                                          GetRecMeasureData, typeof (PartolViewModel), this);
        }

        //选测数据
        private void GetRecMeasureData(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            var data = infos.WstLduSvrAnsOrders;
            if (data == null) return;


            foreach (LduLineData g in data.ItemsData )
            {
                var kessss = new Tuple<int, int>(g.LduId , g.LineId );

                // SeleteDataItems.Add(new SelectDataModel(item));
                if (info.ContainsKey(kessss))
                {
                    if (info[kessss].IsDataBack == false) AnsCount++;
                    info[kessss].SetLduLineItemInfo(g);
                }
            }
        }


        private void SndOrderQuery(bool ano)
        {
            var nt = Sr.ProtocolPhone .LxLdu .wst_ldu_orders ;// .wlst_cnt_wj1090_order_measure ;//.ServerPart.wlst_Wj1090_clinet_order_Measure;
            _dtMeasureBtnOne = DateTime.Now;
            nt.WstLduOrders.Op = 1;


            if (ano == false)
            {
                AnsCount = 0;
                Count = this.Items.Count;

                foreach (var g in this.Items)
                {
                    g.DateCreate = "--";
                    g.IsDataBack = false;
                    g.IsAlarm = "--";
                }
                var lst = new List<int>();
                foreach (var g in info) if (!lst.Contains(g.Key.Item1)) lst.Add(g.Key.Item1);
                foreach (var g in lst )
                {
                    nt.WstLduOrders.LduPartolIds .Add(g);
                    nt.WstLduOrders.LduPartolLines .Add(0);
                }

                SndOrderServer.OrderSnd(nt, 10, 2);

            }
            else
            {
                foreach (var g in this.Items)
                {
                    if (g.IsDataBack) continue;
                    nt.WstLduOrders.LduPartolIds.Add(g.ConId);
                    nt.WstLduOrders.LduPartolLines.Add(g.LineId);
                }
                SndOrderServer.OrderSnd(nt, 10, 2);
            }
        }
    }
}
