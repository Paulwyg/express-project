using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.Wj2096Module.ZcInfo.ZcConLocalArgs.Services;
using Wlst.Cr.CoreMims.Services;
using System.Collections.ObjectModel;
using Wlst.Sr.EquipmentInfoHolding.Services;
 

namespace Wlst.Ux.Wj2096Module.ZcInfo.ZcConLocalArgs.ViewModels
{
    [Export(typeof(IIXcConnLocalArgs))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class XcConnLocalArgs : Wlst.Cr.Core.CoreServices.ObservableObject, IIXcConnLocalArgs 
    {
        public XcConnLocalArgs()
        {
            isViewActive = false;
            this.InitAciton();
        }    
        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public bool CanClose
        {
            get { return true; }
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
            get { return true; }
        }

        public string Title
        {
            get { return "本地操作参数"; }
        }

        #endregion 

        private bool isViewActive = false;

        public void NavOnLoad(params object[] parsObjects)
        {
             if (parsObjects.Count()<2) return;

            isViewActive = true;
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+" 召测指令已发送...";
            LocalInfo.Clear();
            
            var ctrid = (int)parsObjects[1];
            this.CtrlId = ctrid;
            var sluid = (int) parsObjects[0];
            this.SluId = sluid;

            var t = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(CtrlId);
            if (t == null) return;
            this.CtrlPhyId = t.CtrlId;
            this.CtrlName = t.CtrlName;//(sluId, ctrlId);// t.CtrlName;
            
        }

        public void OnUserHideOrClosing()
        {
            isViewActive = false;
            LocalInfo.Clear();
            //this.Items.Clear();

        }
    }
    public partial class XcConnLocalArgs
    { 

        #region CtrlId
        private int _ctrlId;

        /// <summary>
        /// 控制器逻辑地址
        /// </summary>
        public int CtrlId
        {
            get { return _ctrlId; }
            set
            {
                if (_ctrlId == value) return;
                _ctrlId = value;
                RaisePropertyChanged(() => CtrlId);
                //if (CtrlId > 0)
                //{
                //    //if (!EquipmentDataInfoHold.InfoItems.ContainsKey(SluId))
                //    //    return;
                //    //var t = EquipmentDataInfoHold.InfoItems[SluId] as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;

                //    //if (t == null)
                //    //    return;
                //    //if (!t.WjSluCtrls.ContainsKey(CtrlId))
                //    //    return;
                //    //this.CtrlPhyId = t.WjSluCtrls[CtrlId].CtrlPhyId;
                //    //this.CtrlName = t.WjSluCtrls[CtrlId].RtuName;

                //    //lvf 2018年5月31日16:48:35   物联网单灯 信息方式不同于往常控制器
                //    var t = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(CtrlId);
                //    if (t == null) return;
                //    this.CtrlPhyId = t.CtrlId;
                //    this.CtrlName = t.CtrlName;
                //    this.SluId = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(CtrlId);

                //    //var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(SluId).CtrlLst;
                //    //foreach (var t in para)
                //    //{
                //    //    if (t.CtrlId == CtrlId)
                //    //    {
                //    //        this.CtrlName = t.CtrlName;
                //    //        break;
                //    //    }

                //    //}

                //}
                //else
                //{
                //    //this.CtrlPhyId = 0;
                //    this.CtrlName = "";
                //}
            }
        }

        #endregion


        #region CtrlName
        private string _ctrlName;

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string CtrlName
        {
            get { return _ctrlName; }
            set
            {
                if (_ctrlName == value) return;
                _ctrlName = value;
                RaisePropertyChanged(() => CtrlName);
            }
        }

        #endregion


        #region CtrlPhyId
        private int _ctrlPhyId;

        /// <summary>
        /// 控制器物理地址
        /// </summary>
        public int CtrlPhyId
        {
            get { return _ctrlPhyId; }
            set
            {
                if (_ctrlPhyId == value) return;
                _ctrlPhyId = value;
                RaisePropertyChanged(() => CtrlPhyId);
            }
        }

        #endregion


        #region SluName
        private string _sluName;

        /// <summary>
        /// 集中器名称
        /// </summary>
        public string SluName
        {
            get { return _sluName; }
            set
            {
                if (_sluName == value) return;
                _sluName = value;
                RaisePropertyChanged(() => SluName);
            }
        }

        #endregion


        private int _sluId;

        /// <summary>
        /// 集中器逻辑地址
        /// </summary>
        public int SluId
        {
            get { return _sluId; }
            set
            {
                if (value == _sluId) return;
                _sluId = value;
                this.RaisePropertyChanged(() => this.SluId);

                //var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value);
                //if (info != null)
                //{
                //    SluPhyId = info.RtuPhyId;
                //    SluName = info.RtuName;
                //}
                //else
                //{
                //    SluPhyId = value;
                //    SluName = "" + value;
                //}

                var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(value);
                if (para == null)
                {
                    SluPhyId = value;
                    SluName = "" + value;
                }
                else
                {
                    SluPhyId = para.PhyId;
                    SluName = para.FieldName;
                }
            }
        }


        #region SluPhyId
        private int _sluPhyId;

        /// <summary>
        /// 集中器物理地址 
        /// </summary>
        public int SluPhyId
        {
            get { return _sluPhyId; }
            set
            {
                if (_sluPhyId == value) return;
                _sluPhyId = value;
                RaisePropertyChanged(() => SluPhyId);
            }
        }

        #endregion 

        

        private ObservableCollection<LocalArgs> _localInfo;

        /// <summary>
        /// 本地参数
        /// </summary>
        public ObservableCollection<LocalArgs> LocalInfo
        {
            get
            {
                if (_localInfo == null)
                    _localInfo = new ObservableCollection<LocalArgs>();
                return _localInfo;
            }

        }

        private string _msg;

        public string Msg
        {
            get { return _msg; }
            set
            {
                if (value != _msg)
                {
                    _msg = value;
                    this.RaisePropertyChanged(() => this.Msg);
                }

            }
        }
    }

    public partial class XcConnLocalArgs
    {
        #region CmdClearCtrlTime

        private DateTime _dtCmdClearCtrlTime;
        private ICommand _cmdClearCtrlTime;

        public ICommand CmdClearCtrlTime
        {
            get { return _cmdClearCtrlTime ?? (_cmdClearCtrlTime = new RelayCommand(ExCmdClearCtrlTime, CanCmdClearCtrlTime, false)); }
        }



        private void ExCmdClearCtrlTime()
        {
            _dtCmdClearCtrlTime = DateTime.Now;
            //this.Items.Clear();
            var info = Sr.ProtocolPhone.LxSluSgl.wst_slusgl_read_time_plan_in_ctrl;
            // .wlst_cnt_wj3090_order_open_close_light_center ;//.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLightCenter;
            // info.WstCntOrderWj3090OpenClsoeCenter  = data;
            info.WstVsluTimeRead.CtrlId = CtrlId;
            info.WstVsluTimeRead.SluId = 1;
            SndOrderServer.OrderSnd(info, 10, 2);
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "已下发....");
        }


        private bool CanCmdClearCtrlTime()
        {
            return DateTime.Now.Ticks - _dtCmdClearCtrlTime.Ticks > 30000000;
        }

        #endregion
    }

    /// <summary>
    /// Action
    /// </summary>
    public partial class XcConnLocalArgs
    {

        private void InitAciton()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_read_time_plan_in_ctrl, // .wlst_svr_ans_cnt_wj2090_order_xc_conn_args ,//.ClientPart.wlst_Wj2090_svr_to_clinet_xc_conn_args, 
                OnZcOrSetBackSgl,
                typeof(XcConnLocalArgs), this,true);
        }

        private long ggggid = 0;
        private void OnZcOrSetBackSgl(string sessionid,Wlst .mobile .MsgWithMobile  info)
        {
            if (isViewActive == false) return;
            if (info == null || info.WstVsluTimeRead == null)
            {
                Msg = "";
                return;
            }
            if (info.WstVsluTimeRead.CtrlId != CtrlId) return;


            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "召测成功");
            SluId = info.WstVsluTimeRead.SluId;
            CtrlId = info.WstVsluTimeRead.CtrlId;
            var t = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(CtrlId);
            if (t == null) return;
            this.CtrlPhyId = t.CtrlId;
            this.CtrlName = t.CtrlName; //(sluId, ctrlId);// t.CtrlName;
            //if (ggggid != info.Head.Gid)
            //{

            //    ggggid = info.Head.Gid;
            //    LocalInfo.Clear();
            //}


            foreach (var f in info.WstVsluTimeRead.Iems)
            {
                if (f.Index ==1 ) LocalInfo.Clear();
                var la = new LocalArgs
                {
                    Index = f.Index,
                    OperateMode = f.OperationType == 1 ? "定时" : "经纬度",
                    OperateArgs = f.OperationType == 1 ?
                                                    (f.TimerOrOff / 60).ToString("D2") + ":" + (f.TimerOrOff % 60).ToString("D2")
                                                    : "偏移:" + f.TimerOrOff,
                    OutputMode = f.CmdType == 4 ? "混合控制" : "调光控制", //Pwm
                };
                
                la.ValidDate = "";
                string[] bt = new string[7] { "日", "一", "二", "三", "四", "五", "六" };
                for (int i = 0; i < 7; i++)la.ValidDate += f.WeekSet[i]  ? bt[i]+"、" : ""; 

                if (la.ValidDate.Length > 1) la.ValidDate = la.ValidDate.Substring(0, la.ValidDate.Length - 1);    

                if (f.CmdType == 5)
                {
                    for (int i = 0; i < f.CmdMix.Count; i++)
                    {
                        if (f.CmdMix[i].Handle >= 100 && f.CmdMix[i].Handle <= 200)
                        {
                            la.LampOperate[i].Name = (f.CmdMix[i].Handle - 100).ToString() + "%" + "调光";
                        }
                        else
                            la.LampOperate[i].Name = "不操作";
                    }
                }
                else
                {
                    //     继电器操作回路1-4 0-不操作，3-开灯，5-一档节能，a-二档节能，c-关灯
                    for (int i = 0; i < f.CmdMix.Count; i++)
                    {
                        switch (f.CmdMix[i].Handle)
                        {
                            case -1:
                                la.LampOperate[i].Name = "不操作";
                                break;
                            case 0:
                                la.LampOperate[i].Name = "开灯";
                                break;
                            case 3:
                                la.LampOperate[i].Name = "关灯";
                                break;
                            default:
                                la.LampOperate[i].Name = "不操作";
                                break;
                        }
                    }
                }

                LocalInfo.Add(la);
            }


            if (isViewActive == false)
            {
                this.ActiveView();
            }


        }

        private void ActiveView()
        {
            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
                Wj2096Module.Services.ViewIdAssign.ZcConnLocalArgsViewId, true);
        }
    }
}
