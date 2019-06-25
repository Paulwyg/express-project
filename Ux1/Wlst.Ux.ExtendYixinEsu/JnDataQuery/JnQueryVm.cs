using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.ExtendYixinEsu.JnDataQuery
{
    [Export(typeof (IIJnQuery))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class JnQueryVm : Wlst.Cr.Core.CoreServices.ObservableObject, IIJnQuery
    {



        public static JnQueryVm _myself;

        public JnQueryVm()
        {
            _myself = this;
            WicthIsK = 5;
            InitAction();
           
            DtStart = DateTime.Now.AddDays(-2);
            DtEnd = DateTime.Now;
        }

        internal void OnRtuSetOver()
        {
            InitNodes();
            Remark = "节能设备修改，执行更新.";
        }

        internal void OnNodeSelected(int rtuId)
        {
            SelectedRtuId = rtuId;

            var xg = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( rtuId);
            if (xg != null)
            {
                SelectedRtuName = xg.RtuPhyId .ToString("D4") + "-" + xg.RtuName ;
            }
            if (IsSelectedThenRequestData)
            {
                RequestData();
            }
        }

        private bool _isViewShow = false;

        public void NavOnLoad(params object[] parsObjects)
        {
            WicthIsK = 5;
            _isViewShow = true;
            DtEnd = DateTime.Now;
            DtStart = DateTime.Now.AddDays(-1);
            Records.Clear();
            InitNodes();
        }

        public void OnUserHideOrClosing()
        {
            Records.Clear();
            Nodes.Clear();
            InitNodes();
            _isViewShow = false;
        }

        #region IITab

        public string Title
        {
            get { return "节能数据"; }
        }

        public int Index
        {
            get { return 1; }
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


        #region

        private string newName;

        public string Remark
        {
            get { return newName; }
            set
            {
                if (value == newName) return;
                newName = value;
                this.RaisePropertyChanged(() => this.Remark);
            }
        }

        private DateTime dtx1;

        public DateTime DtStart
        {
            get { return dtx1; }
            set
            {
                if (value == dtx1) return;
                dtx1 = value;
                this.RaisePropertyChanged(() => this.DtStart);
            }
        }

        private DateTime dtx2;

        public DateTime DtEnd
        {
            get { return dtx2; }
            set
            {
                if (value == dtx2) return;
                dtx2 = value;
                this.RaisePropertyChanged(() => this.DtEnd);
            }
        }

        private string sdfsdfsd;

        public string SelectedRtuName
        {
            get { return sdfsdfsd; }
            set
            {
                if (value == sdfsdfsd) return;
                sdfsdfsd = value;
                this.RaisePropertyChanged(() => this.SelectedRtuName);
            }
        }


        private int ssdfsddfsdfsd;

        public int SelectedRtuId
        {
            get { return ssdfsddfsdfsd; }
            set
            {
                if (value == ssdfsddfsdfsd) return;
                ssdfsddfsdfsd = value;
                this.RaisePropertyChanged(() => this.SelectedRtuId);
            }
        }


        private bool ssdfsddfsdfssdfsd;

        public bool IsSelectedThenRequestData
        {
            get { return ssdfsddfsdfssdfsd; }
            set
            {
                if (value == ssdfsddfsdfssdfsd) return;
                ssdfsddfsdfssdfsd = value;
                this.RaisePropertyChanged(() => this.IsSelectedThenRequestData);
            }
        }


        private int wichIsK;

        public int WicthIsK
        {
            get { return wichIsK; }
            set
            {
                if (value == wichIsK) return;
                wichIsK = value;
                this.RaisePropertyChanged(() => this.WicthIsK);
            }
        }

        #endregion
    }

    public partial class JnQueryVm
    {
        private ObservableCollection<NodeInfo> _itemItemsRuless;

        public ObservableCollection<NodeInfo> Nodes
        {
            get
            {
                if (_itemItemsRuless == null) _itemItemsRuless = new ObservableCollection<NodeInfo>();
                return _itemItemsRuless;
            }
        }


        public void InitNodes()
        {
            Nodes.Clear();
            foreach (var f in Sr.RtuBelongDataHold.MySlef.Info)
            {
                var tmp = new NodeInfo() {NodeId = 0, NodeName = f.Key};
                foreach (
                    var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems.Values  )
                {
                    if (t.EquipmentType !=WjParaBase.EquType.Rtu )
                        continue;
 
                    if (!f.Value.Contains(t.RtuPhyId )) continue;
                    tmp.Nodes.Add(new NodeInfo()
                                      {
                                          NodeName = t.RtuPhyId .ToString("D4")+"-"+ t.RtuName,
                                          NodeId = t.RtuId 
                                      });

                }
                tmp.NodeName = f.Key + " (" + tmp.Nodes.Count + ")";
                if (tmp.Nodes.Count > 0) Nodes.Add(tmp);
            }
        }
    }


    public partial class JnQueryVm
    {
        private ObservableCollection<JnItem> _record;

        public ObservableCollection<JnItem> Records
        {
            get
            {

                if (_record == null)
                    _record = new ObservableCollection<JnItem>();
                return _record;
            }
            set
            {
                if (_record == value) return;
                _record = value;
                this.RaisePropertyChanged(() => this.Records);
            }
        }

        private void RequestData()
        {

            var dts = new DateTime(DtStart.Year, DtStart.Month, DtStart.Day, 0, 0, 1);
            var dte = new DateTime(DtEnd.Year, DtEnd.Month, DtEnd.Day, 23, 59, 59);
            if (dte.Ticks < dts.Ticks) return;

            var info = Wlst.Sr.ProtocolPhone .LxRtu .wst_rtu_data ;// .wlst_cnt_request_wj3090_measure_data ;//.ProtocolCnt.ServerPart.wlst_RecordEvent_clinet_request_data;
            info.WstRtuData .DtEndTime  = dte.Ticks ;
            info.WstRtuData.DtStartTime  = dts.Ticks;
            info.WstRtuData.RtuId  = SelectedRtuId;
            info.WstRtuData.Op = 2;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  请求数据命令已发送...";

        }


        #region CmdDeleteCatalog

        private ICommand _cCmdUpdate;

        public ICommand CmdQuery
        {
            get { return _cCmdUpdate ?? (_cCmdUpdate = new RelayCommand(ExCmdUpdate, CanCmdUpdate, true)); }
        }

        private DateTime _dtUpdate = DateTime.Now.AddDays(-1);

        private void ExCmdUpdate()
        {
            _dtUpdate = DateTime.Now;

            this.RequestData( );
        }

        private bool CanCmdUpdate()
        {
            return  DtEnd .Ticks >DtStart .Ticks && DateTime.Now.Ticks - _dtUpdate.Ticks > 20000000 && SelectedRtuId >0 ;
        }

        #endregion



        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxRtu .wst_rtu_data ,// .wlst_svr_ans_cnt_request_wj3090_measure_data  ,//ProtocolCnt.ClientPart.wlst_RecordEvent_server_ans_clinet_request_data,
                RecordDataRequest,
                typeof (JnQueryVm), this);
        }

        public void RecordDataRequest(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            if (_isViewShow == false) return;
            var info = infos.WstRtuData   ;
            if (info == null) return;
            //   this.Records.Clear();
            if (this.SelectedRtuId != info.RtuId ) return;

            var qeu = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( SelectedRtuId);
            if (qeu == null||qeu .EquipmentType !=WjParaBase.EquType.Rtu ) return;
            var amps = qeu as Wlst .Sr .EquipmentInfoHolding .Model .Wj3005Rtu ;
            if (amps == null ||amps .WjLoops ==null ) return;

            int[] xr = new int[6];
            foreach (var f in amps.WjLoops .Values )
            {
                if (f.VectorMoniliang < 1) continue;
                if (f.SwitchOutputId == WicthIsK) //节电输出
                {
                    if (xr[3 + (int )f.VoltagePhaseCode ] == 0)
                        xr[3 + (int)f.VoltagePhaseCode] = f.LoopId;
                }
                else
                {
                    if (xr[(int)f.VoltagePhaseCode] == 0)
                        xr[(int)f.VoltagePhaseCode] = f.LoopId;
                }
            }

            this.Records.Clear();
            var tmpitems = new ObservableCollection<JnItem>();
            int index = 1;
            foreach (var t in info.Items )
            {

                var direx = new Dictionary<int, double>();
                foreach (var f in t.LstNewLoopsData)
                {
                    if (direx.ContainsKey(f.LoopId)) continue;
                    direx.Add(f.LoopId, f.V);
                }
                double[] xg = new double[6];
                for (int i = 0; i < xr.Count(); i++)
                {
                    xg[i] = direx.ContainsKey(xr[i]) ? direx[xr[i]] : -1;
                }
                tmpitems.Add(new JnItem(index++,new DateTime(  t.DateCreate), xg[0], xg[1], xg[2], xg[3], xg[4], xg[5]));
            }
            this.Records = tmpitems;
            Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--终端数据查询成功，共计" + info.Items .Count + " 条数据.";
        }
    }

    public class NodeInfo : Wlst.Cr.CoreOne.TreeNodeBase.TreeNodeBaseViewModel
    {

        private ObservableCollection<NodeInfo> _itemItemsRuless;

        public ObservableCollection<NodeInfo> Nodes
        {
            get
            {
                if (_itemItemsRuless == null) _itemItemsRuless = new ObservableCollection<NodeInfo>();
                return _itemItemsRuless;
            }
        }

        public override void OnNodeSelectActive()
        {
            base.OnNodeSelectActive();
            if (NodeId > 0 && JnQueryVm._myself != null) JnQueryVm._myself.OnNodeSelected(NodeId);
        }
    }

    public class JnItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        public JnItem(int index, DateTime dt, double ina, double inb, double inc, double oua, double oub, double ouc)
        {
            Index = index;
            DtTime = dt.ToString("yyyy-MM-dd HH:mm:ss");
            InA = ina < 0 ? "--" : ina.ToString("f2");
            InB = inb < 0 ? "--" : inb.ToString("f2");
            InC = inc < 0 ? "--" : inc.ToString("f2");

            OutA = oua < 0 ? "--" : oua.ToString("f2");
            OutB = oub < 0 ? "--" : oub.ToString("f2");
            OutC = ouc < 0 ? "--" : ouc.ToString("f2");
        }

        private int xxIndex;

        public int Index
        {
            get { return xxIndex; }
            set
            {
                if (value == xxIndex) return;
                xxIndex = value;
                this.RaisePropertyChanged(() => this.Index);
            }
        }

        private string dtTime;

        public string DtTime
        {
            get { return dtTime; }
            set
            {
                if (value == dtTime) return;
                dtTime = value;
                this.RaisePropertyChanged(() => this.DtTime);
            }
        }

        private string ina;

        public string InA
        {
            get { return ina; }
            set
            {
                if (value == ina) return;
                ina = value;
                this.RaisePropertyChanged(() => this.InA);
            }
        }

        private string inb;

        public string InB
        {
            get { return inb; }
            set
            {
                if (value == inb) return;
                inb = value;
                this.RaisePropertyChanged(() => this.InB);
            }
        }

        private string inc;

        public string InC
        {
            get { return inc; }
            set
            {
                if (value == inc) return;
                inc = value;
                this.RaisePropertyChanged(() => this.InC);
            }
        }



        private string outc;

        public string OutC
        {
            get { return outc; }
            set
            {
                if (value == outc) return;
                outc = value;
                this.RaisePropertyChanged(() => this.OutC);
            }
        }


        private string outb;

        public string OutB
        {
            get { return outb; }
            set
            {
                if (value == outb) return;
                outb = value;
                this.RaisePropertyChanged(() => this.OutB);
            }
        }

        private string outa;

        public string OutA
        {
            get { return outa; }
            set
            {
                if (value == outa) return;
                outa = value;
                this.RaisePropertyChanged(() => this.OutA);
            }
        }
    }
}
