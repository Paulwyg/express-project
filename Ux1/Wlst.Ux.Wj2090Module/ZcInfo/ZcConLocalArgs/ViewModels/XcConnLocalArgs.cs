using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Wlst.Ux.Wj2090Module.ZcInfo.ZcConLocalArgs.Services;
using Wlst.Cr.CoreMims.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Sr.EquipmentInfoHolding.Services;
 

namespace Wlst.Ux.Wj2090Module.ZcInfo.ZcConLocalArgs.ViewModels
{
    [Export(typeof(IIXcConnLocalArgs))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class XcConnLocalArgs : Wlst.Cr.Core.CoreServices.ObservableObject
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
            isViewActive = true;

            if (parsObjects.Length > 0)
            {
                try
                {
                    if (parsObjects.Count() > 0)
                    {
                        var rtulinfo = parsObjects[0] as Tuple<int, int>;
                        var sluid = rtulinfo.Item1;
                        var ctrlid = rtulinfo.Item2;
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(sluid))
                        {

                            SluId = sluid;
                            if (ctrlid > 0) CtrlId = ctrlid;
                        }

                        LocalInfo.Clear();
                        Remind ="";
                    }

                }
                catch (Exception ex)
                {
                }
            }
        }

        public void OnUserHideOrClosing()
        {
            isViewActive = false;
            //this.Items.Clear();

        }
    }
    public partial class XcConnLocalArgs : Wlst.Cr.Core.CoreServices.ObservableObject, IIXcConnLocalArgs 
    { 
        /// <summary>
        /// 控制器逻辑地址
        /// </summary>
        #region CtrlId
        private int _ctrlId;
        public int CtrlId
        {
            get { return _ctrlId; }
            set
            {
                if (_ctrlId == value) return;
                _ctrlId = value;
                RaisePropertyChanged(() => CtrlId);
                if (CtrlId > 0)
                {
                    if (!EquipmentDataInfoHold.InfoItems .ContainsKey(SluId))
                        return;
                    var t = EquipmentDataInfoHold.InfoItems [SluId] as Wlst .Sr .EquipmentInfoHolding .Model .Wj2090Slu ;

                    if (t == null)
                        return;
                    if (!t.WjSluCtrls .ContainsKey(CtrlId))
                        return;
                    this.CtrlPhyId = t.WjSluCtrls [CtrlId].CtrlPhyId;
                    this.CtrlName = t.WjSluCtrls [CtrlId].RtuName;
                }
                else
                {
                    this.CtrlPhyId = 0;
                    this.CtrlName = "";
                }
            }
        }

        #endregion

        /// <summary>
        /// 控制器名称
        /// </summary>
        #region CtrlName
        private string _ctrlName;
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

        /// <summary>
        /// 控制器物理地址
        /// </summary>
        #region CtrlPhyId
        private int _ctrlPhyId;
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

        /// <summary>
        /// 集中器名称
        /// </summary>
        #region SluName
        private string _sluName;
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

        /// <summary>
        /// 集中器逻辑地址
        /// </summary>
        private int _sluId;
        public int SluId
        {
            get { return _sluId; }
            set
            {
                if (value == _sluId) return;
                _sluId = value;
                this.RaisePropertyChanged(() => this.SluId);

                var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( value);
                if (info != null)
                {
                    SluPhyId = info.RtuPhyId ;
                    SluName = info.RtuName;
                }
                else
                {
                    SluPhyId = value;
                    SluName = "" + value;
                }
            }
        }

        /// <summary>
        /// 集中器物理地址 
        /// </summary>
        #region SluPhyId
        private int _sluPhyId;
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

        
        /// <summary>
        /// 本地参数
        /// </summary>
        private ObservableCollection<LocalArgs> _localInfo;

        public ObservableCollection<LocalArgs> LocalInfo
        {
            get
            {
                if (_localInfo == null)
                    _localInfo = new ObservableCollection<LocalArgs>();
                return _localInfo;
            }

        }

        private string _remind;
        /// <summary>
        /// 提示
        /// </summary>
        public string Remind
        {
            get { return _remind; }
            set
            {
                if (value == _remind) return;
                _remind = value;
                this.RaisePropertyChanged(() => this.Remind);
            }
        }


    }
    /// <summary>
    /// Action
    /// </summary>
    public partial class XcConnLocalArgs
    {

        private void InitAciton()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxSlu .wst_svr_ans_read_ctrl_args ,// .wlst_svr_ans_cnt_wj2090_order_xc_conn_args ,//.ClientPart.wlst_Wj2090_svr_to_clinet_xc_conn_args, 
                OnZcOrSetBack,
                typeof(XcConnLocalArgs), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSlu.wst_slu_snd_order,// .wlst_svr_ans_cnt_wj2090_order_xc_conn_args ,//.ClientPart.wlst_Wj2090_svr_to_clinet_xc_conn_args, 
                RequestSluOrderBack,
                typeof(XcConnLocalArgs), this);
        }


        private void OnZcOrSetBack(string sessionid,Wlst .mobile .MsgWithMobile  info)
        {
            if (info == null || info.WstSluSvrAnsReadCtrlArgs == null || info.WstSluSvrAnsReadCtrlArgs.CtrlRuntimeField == null || info.WstSluSvrAnsReadCtrlArgs.CtrlSunrisesetField == null) return;

            if (info.WstSluSvrAnsReadCtrlArgs != null && info.WstSluSvrAnsReadCtrlArgs.CtrlParaField != null && info.WstSluSvrAnsReadCtrlArgs.DataMarkField.ReadCtrlParaField == true) return;
            if (info.WstSluSvrAnsReadCtrlArgs != null && info.WstSluSvrAnsReadCtrlArgs.CtrlVerField != null && info.WstSluSvrAnsReadCtrlArgs.DataMarkField.ReadVer == true) return;
            if (info.WstSluSvrAnsReadCtrlArgs != null && info.WstSluSvrAnsReadCtrlArgs.CtrlVerField != null && info.WstSluSvrAnsReadCtrlArgs.DataMarkField.ReadCtrlRuntimeField == false) return;

            SluId = info.WstSluSvrAnsReadCtrlArgs.SluId;
            CtrlId = info.WstSluSvrAnsReadCtrlArgs.CtrPhyId;
            LocalInfo.Clear();

            var t = info.WstSluSvrAnsReadCtrlArgs.CtrlSunrisesetField;
            int index0 = 1;;
            foreach (var f in info.WstSluSvrAnsReadCtrlArgs.CtrlRuntimeField)
            {
                var la = new LocalArgs()
                {
                    Index = index0,
                    DataMode = f.DataType == 0 ? "基本型" : "扩展型",
                    OperateMode = f.OperateType == 1 ? "定时" : "经纬度",
                    OperateArgs = f.OperateType == 1 ?
                                                    (f.OperateTime / 60).ToString("D2") + ":" + (f.OperateTime % 60).ToString("D2") 
                                                    : "偏移:" + f.OperateOff,
                    Sunrise_sunset = (t.Sun  / 60).ToString("D2") + ":" + (t.Sun  % 60).ToString("D2") + "-" + (t.Sunrise / 60).ToString("D2") + ":" + (t.Sunrise % 60).ToString("D2"),
                    OutputMode = f.OutputType == 0 ? "混合控制" : "Pwm控制",
                };

                la.ValidDate = "";
                string[] bt = new string[7] { "日", "一", "二", "三", "四", "五", "六" };
                for (int i = 0; i < 7; i++)la.ValidDate += f.DateEnable[i] == 1 ? bt[i]+"、" : ""; 

                if (la.ValidDate.Length > 1) la.ValidDate = la.ValidDate.Substring(0, la.ValidDate.Length - 1);    

                if (f.OutputType == 1)
                {
                    for (int i = 0; i < f.PwmLoop.Count; i++)
                    {
                        if (f.PwmLoop[i] == 1) la.LampOperate[i].Name = f.PwmPower * 10 + "%";
                        else la.LampOperate[i].Name = "无";
                    }
                }
                else
                {
                    //     继电器操作回路1-4 0-不操作，3-开灯，5-一档节能，a-二档节能，c-关灯
                    for (int i = 0; i < f.RelayOperate.Count; i++)
                    {
                        switch (f.RelayOperate[i])
                        {
                            case 0:
                                la.LampOperate[i].Name = "不操作";
                                break;
                            case 3:
                                la.LampOperate[i].Name = "开灯";
                                break;
                            case 5:
                                la.LampOperate[i].Name = "调档节能";
                                break;
                            case 10:
                                la.LampOperate[i].Name = "调光节能";
                                break;
                            case 12:
                                la.LampOperate[i].Name = "关灯";
                                break;
                            default:
                                la.LampOperate[i].Name = "无";
                                break;
                        }
                    }
                }

                index0++;
                LocalInfo.Add(la);
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 数据已返回 ...";
            }


            if (isViewActive == false)
            {
                this.ActiveView();
            }


        }



        private void RequestSluOrderBack(string sessionid, Wlst.mobile.MsgWithMobile info)
        {
            if (info == null || info.WstSluSndOrder == null ) return;

            if (info.WstSluSndOrder.Op != 21) return;

            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 已下发 ...";


        }


        private void ActiveView()
        {
            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
                Wj2090Module.Services.ViewIdAssign.ZcConnLocalArgsViewId, true);
        }
    }



    public partial class XcConnLocalArgs
    {
        private DateTime _dtCmdSendTimeAgain;
        private ICommand _cmdCmdSendTimeAgain;

        /// <summary>
        /// 重发方案
        /// </summary>
        public ICommand CmdSendTimeAgain
        {
            get
            {
                if (_cmdCmdSendTimeAgain == null)
                    _cmdCmdSendTimeAgain = new RelayCommand(ExCmdSendTimeAgain, CanExCmdSendTimeAgain, false);
                return _cmdCmdSendTimeAgain;
            }
        }

        private void ExCmdSendTimeAgain()
        {
            _dtCmdSendTimeAgain = DateTime.Now;

            var info = Wlst.Sr.ProtocolPhone.LxSlu.wst_slu_snd_order;// .wlst_cnt_wj2090_order_xc_conn_args ;//.ServerPart.wlst_Wj2090_clinet_xc_conn_args;
            info.Args.Addr.Add(1);
            info.WstSluSndOrder.Op = 21;
            var ctrllst = new List<int>();
            ctrllst.Add(CtrlId);
            info.WstSluSndOrder.CtrlIds = ctrllst;
            info.WstSluSndOrder.SluId = SluId;

            info.Head.Gid += 1;
            SndOrderServer.OrderSnd(info);

            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在下发 ...";
        


        }

        private bool CanExCmdSendTimeAgain()
        {
            return DateTime.Now.Ticks - _dtCmdSendTimeAgain.Ticks > 30000000;
            return false;
        }
    }


}
