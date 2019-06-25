using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Services;
using Wlst.Ux.Wj1080Module.Wj1080InfoSet.ViewModel;
using Wlst.client;

namespace Wlst.Ux.Wj1080Module.LuxOnTab
{
    [Export(typeof (IILuxOnTabView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class LuxOnTabViewModel : IILuxOnTabView
    {
        #region tab
        public int Index
        {
            get { return 1; }
        }
        public bool CanClose
        {
            get { return false; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return false; }
        }

        public string Title
        {
            get { return "分布式光控"; }
        }

        #endregion


        public void NavOnLoad(params object[] parsObjects)
        {
            //throw new NotImplementedException();

        }

        public LuxOnTabViewModel()
        {
            InitAction();
        }

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxAls .wst_svr_ans_lux_orders ,// .wlst_svr_ans_cnt_lux_measure ,//.ClientPart.wlst_Wj1080_server_ans_clinet_order_Measure,
                LuxDataMeasureEvent,
                typeof (LuxOnTabViewModel), this);
        }

        public void LuxDataMeasureEvent(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            var info = infos.WstAlsSvrAnsOrderZcOrSet  ;
            if (info == null) return;
            if (info.Op == 1)
            {
                if (Wj1080Module.Services.Common.IsLuxOnTabShowd == false) return;

                UpdateLuxData(info.SuggestedLuxId, info.LuxValue);
            }


        }

        private void UpdateLuxData(int luxid, double data)
        {

            bool find = false;
            foreach (var g in Records)
            {
                if (g == null) continue;
                if (g.RtuId == luxid)
                {
                    if (g.RtuName.Contains("Resvrve"))
                    {
                        var luxequip =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(luxid);//.GetEquipmentInfo(luxid);
                        if (luxequip != null)
                        {
                            g.RtuName = luxequip.RtuName;
                            g.PhyId = luxequip.RtuPhyId;

                        }
                        var tmps = luxequip as Wlst.Sr.EquipmentInfoHolding.Model.Wj1080Lux;//Wlst.Cr.Core..TerminalInfomationInterface.IILux;
                        if (tmps != null)
                        {
                            if (tmps.WjLux.LuxWorkMode == 0) g.RtuWork = "主报";
                            if (tmps.WjLux.LuxWorkMode == 1) g.RtuName = "选测应答";
                            if (tmps.WjLux.LuxWorkMode == 2) g.RtuWork = "GPRS主报";
                            if (tmps.WjLux.LuxWorkMode == 3) g.RtuWork = "485主报";
                        }
                    }
                    g.LuxData2 = g.LuxData1;
                    g.LuxData1 = g.LuxData0;
                    g.DateCreate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    g.LuxData0 = data.ToString("f2");
                    find = true;
                    break;

                }
            }
            if (find == false)
            {
                LuxDataViewModel tps = new LuxDataViewModel()
                {
                    DateCreate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Id = 0,
                    LuxData0 = data.ToString("f2"),
                    PhyId = 1,
                    RtuId = luxid,
                    RtuName = "Resvrve",
                    RtuWork = "",
                    LuxData1 = "--",
                    LuxData2 = "--"

                };

                var luxequip =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(luxid);
                if (luxequip != null)
                {  tps.RtuName = luxequip.RtuName;
                   tps.PhyId = luxequip.RtuPhyId;

                    if (luxequip.RtuFid  != 0)
                    {
                        var fiinfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(luxequip.RtuFid);//.GetEquipmentInfo(luxequip.AttachRtuId);
                        if (fiinfo != null)
                        {
                            tps.PhyId = fiinfo.RtuFid;
                        }
                    }

                }
                var tmps = luxequip as Wlst.Sr.EquipmentInfoHolding.Model.Wj1080Lux;//.TerminalInfomationInterface.IILux;
                if (tmps != null)
                {
                    if (tmps.WjLux.LuxWorkMode == 0) tps.RtuWork = "主报";
                    if (tmps.WjLux.LuxWorkMode == 1) tps.RtuWork = "选测应答";
                    if (tmps.WjLux.LuxWorkMode == 2) tps.RtuWork = "GPRS主报";
                    if (tmps.WjLux.LuxWorkMode == 3) tps.RtuWork = "485主报";
                }
                int index = 0;
                for (int i = 0; i < Records.Count; i++)
                {
                    if (Records[i] == null) continue;
                    if (Records[i].RtuId < luxid) index++;
                    else break;
                }
                Records.Insert(index, tps);
            }
    
        }



        private ObservableCollection<LuxDataViewModel> _record;

        public ObservableCollection<LuxDataViewModel> Records
        {
            get
            {
                if (_record == null)
                    _record = new ObservableCollection<LuxDataViewModel>();
                return _record;
            }
        }
    }


    public class LuxDataViewModel : LuxRecoredViewModel
    {

        private int _phyid;

        /// <summary>
        /// 记录序号
        /// </summary>

        public int PhyId
        {
            get { return _phyid; }
            set
            {
                if (value != _phyid)
                {
                    _phyid = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }

        private  string LuxDataa0;
        public   string  LuxData0
        {
            get { return LuxDataa0; }
            set
            {
                if (value != LuxDataa0)
                {
                    LuxDataa0 = value;
                    this.RaisePropertyChanged(() => this.LuxData0);
                }
            }
        }

        private  string LuxDataa1;
        public  string LuxData1
        {
            get { return LuxDataa1; }
            set
            {
                if (value != LuxDataa1)
                {
                    LuxDataa1 = value;
                    this.RaisePropertyChanged(() => this.LuxData1);
                }
            }
        }

        private  string LuxDataa2;
        public  string LuxData2
        {
            get { return LuxDataa2; }
            set
            {
                if (value != LuxDataa2)
                {
                    LuxDataa2 = value;
                    this.RaisePropertyChanged(() => this.LuxData2);
                }
            }
        }

        private string  _rtuName;

        /// <summary>
        /// 终端名称
        /// </summary>

        public string  RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }



        private string _rtsdf;

        /// <summary>
        /// 终端名称
        /// </summary>

        public string RtuWork
        {
            get { return _rtsdf; }
            set
            {
                if (value != _rtsdf)
                {
                    _rtsdf = value;
                    this.RaisePropertyChanged(() => this.RtuWork);
                }
            }
        }
    }
}
