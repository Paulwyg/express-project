using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.Wj2096Module.ZcInfo.ZcConArgs.Services;
using Wlst.Cr.CoreMims.Services;
using Wlst.Sr.EquipmentInfoHolding.Services;
using System.Collections.ObjectModel;

namespace Wlst.Ux.Wj2096Module.ZcInfo.ZcConArgs.ViewModels
{
    [Export(typeof(IIXcConnArgs))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class XcConnArgs : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public XcConnArgs()
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
            get { return "控制器基本参数"; }
        }

        #endregion

        private bool isViewActive = false;

        public void NavOnLoad(params object[] parsObjects)
        {
            isViewActive = true;
        }

        public void OnUserHideOrClosing()
        {
            isViewActive = false;
            //this.Items.Clear();

        }

    }


    public partial class XcConnArgs : Wlst.Cr.Core.CoreServices.ObservableObject, IIXcConnArgs
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
                //if (CtrlId > 0)
                //{
                //    //lvf 2018年5月31日16:48:35   物联网单灯 信息方式不同于往常控制器
                 
                //    var t = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(CtrlId);
                //    if (t == null) return;
                //    this.CtrlPhyId = t.CtrlId;
                //    this.CtrlName = t.CtrlName;//(sluId, ctrlId);// t.CtrlName;
                //    this.SluId = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(CtrlId);

                //    //if (!EquipmentDataInfoHold .InfoItems .ContainsKey(SluId))
                //    //    return;
                //    //var t = EquipmentDataInfoHold.InfoItems [SluId] as Wlst .Sr .EquipmentInfoHolding .Model .Wj2090Slu ;

                //    //if (t == null ||t.WjSluCtrls ==null )
                //    //    return;
                //    //if (!t.WjSluCtrls.ContainsKey(CtrlId))
                //    //    return;
                //    //this.CtrlPhyId = t.WjSluCtrls[CtrlId].CtrlPhyId;
                //    ////this.CtrlName = t.WjSluCtrls [CtrlId].RtuName;
                //    //if (string.IsNullOrEmpty(t.WjSluCtrls[CtrlId].LampCode))
                //    //    this.CtrlName = t.WjSluCtrls[CtrlId].RtuName;
                //    //else this.CtrlName = t.WjSluCtrls[CtrlId].LampCode;
                //}
                //else
                //{
                //    this.CtrlPhyId = 0;
                //    this.CtrlName = "";
                //}
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

                //var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( value);
                if (Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Info.ContainsKey(value)==false) return;
                var info = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Info[value];
                if (info != null)
                {

                    SluPhyId = info.PhyId;// info.FieldId;
                    SluName = info.FieldName;
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
        /// 日出日落
        /// </summary>
        #region Sunrise_sunset
        private string _sunrise_sunset;
        public string Sunrise_sunset
        {
            get { return _sunrise_sunset; }
            set
            {
                if (_sunrise_sunset == value) return;
                _sunrise_sunset = value;
                RaisePropertyChanged(() => Sunrise_sunset);
            }
        }
        #endregion

        /// <summary>
        /// 节能方式 
        /// </summary>
        #region PowerSavingMode
        private string _powerSavingMode;
        public string PowerSavingMode
        {
            get { return _powerSavingMode; }
            set
            {
                if (_powerSavingMode == value) return;
                _powerSavingMode = value;
                RaisePropertyChanged(() => PowerSavingMode);
            }
        }
        #endregion

        /// <summary>
        /// 软件版本
        /// </summary>
        #region SfwVer
        private string _sfwVer;
        public string SfwVer
        {
            get { return _sfwVer; }
            set
            {
                if (_sfwVer == value) return;
                _sfwVer = value;
                RaisePropertyChanged(() => SfwVer);
            }
        }
        #endregion

        /// <summary>
        /// 型号
        /// </summary>
        #region CtrlModel
        private string _ctrlModel;
        public string CtrlModel
        {
            get { return _ctrlModel; }
            set
            {
                if (_ctrlModel == value) return;
                _ctrlModel = value;
                RaisePropertyChanged(() => CtrlModel);
            }
        }
        #endregion

        /// <summary>
        /// 漏电模块
        /// </summary>
        #region LeakModule
        private string _leakModule;
        public string LeakModule
        {
            get { return _leakModule; }
            set
            {
                if (_leakModule == value) return;
                _leakModule = value;
                RaisePropertyChanged(() => LeakModule);
            }
        }
        #endregion


        /// <summary>
        /// 温度检测模块
        /// </summary>
        #region TemperatureModule
        private string _temperatureModule;
        public string TemperatureModule
        {
            get { return _temperatureModule; }
            set
            {
                if (_temperatureModule == value) return;
                _temperatureModule = value;
                RaisePropertyChanged(() => TemperatureModule);
            }
        }
        #endregion


        /// <summary>
        /// 时钟模块
        /// </summary>
        #region TimerModule
        private string _timerModule;
        public string TimerModule
        {
            get { return _timerModule; }
            set
            {
                if (_timerModule == value) return;
                _timerModule = value;
                RaisePropertyChanged(() => TimerModule);
            }
        }
        #endregion

        /// <summary>
        /// 控制器时钟 
        /// </summary>
        #region DateTimeRecevie
        private string _dateTimeRecevie;
        public string DateTimeRecevie
        {
            get { return _dateTimeRecevie; }
            set
            {
                if (_dateTimeRecevie == value) return;
                _dateTimeRecevie = value;
                RaisePropertyChanged(() => DateTimeRecevie);
            }
        }
        #endregion

        /// <summary>
        /// 回路数量 
        /// </summary>
        #region LoopCount
        private int _loopCount;
        public int LoopCount
        {
            get { return _loopCount; }
            set
            {
                if (_loopCount == value) return;
                _loopCount = value;
                RaisePropertyChanged(() => LoopCount);
            }
        }
        #endregion

        /// <summary>
        /// 所属分组 
        /// </summary>
        #region GrpName
        private string _grpName;
        public string GrpName
        {
            get { return _grpName; }
            set
            {
                if (_grpName == value) return;
                _grpName = value;
                RaisePropertyChanged(() => GrpName);
            }
        }
        #endregion

        /// <summary>
        /// 运行参数
        /// </summary>
        private ObservableCollection<RunArgs> _runInfo;

        public ObservableCollection<RunArgs> RunInfo
        {
            get
            {
                if (_runInfo == null)
                    _runInfo = new ObservableCollection<RunArgs>();
                return _runInfo;
            }

        }




        /// <summary>
        /// 域名 
        /// </summary>
        #region Domain
        private int _domain;
        public int DomainName
        {
            get { return _domain; }
            set
            {
                if (_domain == value) return;
                _domain = value;
                RaisePropertyChanged(() => DomainName);
            }
        }
        #endregion

        /// <summary>
        /// 经纬度
        /// </summary>
        #region LatitudeLatitudelongitude
        private double LatitudeLatitudelongitude;
        public double Latitude
        {
            get { return LatitudeLatitudelongitude; }
            set
            {
                if (LatitudeLatitudelongitude == value) return;
                LatitudeLatitudelongitude = value;
                RaisePropertyChanged(() => Latitude);
            }
        }
        #endregion


        /// <summary>
        /// 经纬度
        /// </summary>
        #region LatitudeLongitudelongitude
        private double LatitudeLongitudelongitude;
        public double Longitude
        {
            get { return LatitudeLongitudelongitude; }
            set
            {
                if (LatitudeLongitudelongitude == value) return;
                LatitudeLongitudelongitude = value;
                RaisePropertyChanged(() => Longitude);
            }
        }
        #endregion

        /// <summary>
        /// 投运
        /// </summary>
        #region IsRun
        private string  _isRun;
        public string CtrlStatus
        {
            get { return _isRun; }
            set
            {
                if (_isRun == value) return;
                _isRun = value;
                RaisePropertyChanged(() => CtrlStatus);
            }
        }
        #endregion

        /// <summary>
        /// 主报
        /// </summary>
        #region IsActiveAlarm
        private string  _isActiveAlarm;
        public string  CtrlEnableAlarm
        {
            get { return _isActiveAlarm; }
            set
            {
                if (_isActiveAlarm == value) return;
                _isActiveAlarm = value;
                RaisePropertyChanged(() => CtrlEnableAlarm);
            }
        }
        #endregion

        /// <summary>
        /// 主报周期
        /// </summary>
        #region UplinkTimer
        private int _uplinkTimer;
        public int UplinkTimer
        {
            get { return _uplinkTimer; }
            set
            {
                if (_uplinkTimer == value) return;
                _uplinkTimer = value;
                RaisePropertyChanged(() => UplinkTimer);
            }
        }
        #endregion

        /// <summary>
        /// 是否应答
        /// </summary>
        #region UplinkReply
        private string _uplinkReply;
        public string UplinkReply
        {
            get { return _uplinkReply; }
            set
            {
                if (_uplinkReply == value) return;
                _uplinkReply = value;
                RaisePropertyChanged(() => UplinkReply);
            }
        }
        #endregion

        #region IsActiveCtrlPowerTurnonAlarm
        private string IsActiveCtrlPowerTurnonAlarm;
        public string CtrlPowerTurnon
        {
            get { return IsActiveCtrlPowerTurnonAlarm; }
            set
            {
                if (IsActiveCtrlPowerTurnonAlarm == value) return;
                IsActiveCtrlPowerTurnonAlarm = value;
                RaisePropertyChanged(() => CtrlPowerTurnon);
            }
        }
        #endregion

        #region IsActiveCtrlPoRatedPowerwerTurnonAlarm
        private string IsActiveCtrlPoRatedPowerwerTurnonAlarm;
        public string RatedPower
        {
            get { return IsActiveCtrlPoRatedPowerwerTurnonAlarm; }
            set
            {
                if (IsActiveCtrlPoRatedPowerwerTurnonAlarm == value) return;
                IsActiveCtrlPoRatedPowerwerTurnonAlarm = value;
                RaisePropertyChanged(() => RatedPower);
            }
        }
        #endregion

        #region IsActiveCtrlPoRatedPowerwCtrlVectorerTurnonAlarm
        private string IsActiveCtrlPoRatedPowerwCtrlVectorerTurnonAlarm;
        public string CtrlVector
        {
            get { return IsActiveCtrlPoRatedPowerwCtrlVectorerTurnonAlarm; }
            set
            {
                if (IsActiveCtrlPoRatedPowerwCtrlVectorerTurnonAlarm == value) return;
                IsActiveCtrlPoRatedPowerwCtrlVectorerTurnonAlarm = value;
                RaisePropertyChanged(() => CtrlVector);
            }
        }
        #endregion


        #region IsActiveCtrlPEfoRatedPowerwCtrlVectorerTurnonAlarm
        private string IsActiveCtrlPEfoRatedPowerwCtrlVectorerTurnonAlarm;
        public string Ef
        {
            get { return IsActiveCtrlPEfoRatedPowerwCtrlVectorerTurnonAlarm; }
            set
            {
                if (IsActiveCtrlPEfoRatedPowerwCtrlVectorerTurnonAlarm == value) return;
                IsActiveCtrlPEfoRatedPowerwCtrlVectorerTurnonAlarm = value;
                RaisePropertyChanged(() => Ef);
            }
        }
        #endregion

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
    public partial class XcConnArgs
    {

        private void InitAciton()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_read_ctrl_args,//wst_svr_ans_read_ctrl_args  ,// .wlst_svr_ans_cnt_wj2090_order_xc_conn_args ,//.ClientPart.wlst_Wj2090_svr_to_clinet_xc_conn_args, 
                
                OnZcOrSetBack,
                typeof(XcConnArgs), this);
        }


        private int ctrlidx = 0;
        private int sluidx = 0;
        private void OnZcOrSetBack(string sessionid,Wlst .mobile .MsgWithMobile  info)
        {
            Remind = "";
            //if (info.Args.Cid==2) Remind = "召测失败";

            if (info != null && info.WstSluSvrAnsReadCtrlArgs != null && info.WstSluSvrAnsReadCtrlArgs.CtrlVerField != null && info.WstSluSvrAnsReadCtrlArgs.DataMarkField.ReadVer == true)
            {
                SluId = info.WstSluSvrAnsReadCtrlArgs.SluId;
                CtrlId = info.WstSluSvrAnsReadCtrlArgs.CtrlId;
                var t = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(CtrlId);
                if (t == null) return;
                this.CtrlPhyId = t.CtrlId;
                this.CtrlName = t.CtrlName;//(sluId, ctrlId);// t.CtrlName;



                //lvf 如果不是本客户端操作的 不处理 2018年8月9日09:34:11
                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.ContainsKey(SluId) == false) return;
                System.TimeSpan ts = DateTime.Now - Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus[SluId];
                if (ts.Minutes > 5)
                {
                    //可以不清除,但可能占用内存
                    Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.Remove(SluId);
                    return;
                }


                var tt = info.WstSluSvrAnsReadCtrlArgs.CtrlVerField;

                ctrlidx = info.WstSluSvrAnsReadCtrlArgs.CtrlId;
                sluidx = info.WstSluSvrAnsReadCtrlArgs.SluId;

                LoopCount = tt.CtrlLoop;
                switch (tt.EnergySaving)
                {
                    case 0:
                        PowerSavingMode = "无控制";
                        break;
                    case 1:
                        PowerSavingMode = "只有开关灯";
                        break;
                    case 2:
                        PowerSavingMode = "调档节能";
                        break;
                    case 3:
                        PowerSavingMode = "调光节能";
                        break;
                    case 4:
                        PowerSavingMode = "RS485节能";
                        break;
                    case 5:
                        PowerSavingMode = "调光节能";
                        break;
                    case 6:
                        PowerSavingMode = "0~10V节能";
                        break;
                    default:
                        PowerSavingMode = "未知";
                        break;
                }
                SfwVer = tt.Ver;
                CtrlModel = tt.CtrlType;
                LeakModule = tt.ElectricLeakageModule == true ? "有" : "无";
                TemperatureModule = tt.TemperatureModule == true ? "有" : "无";
                TimerModule = tt.TimerModule == true ? "有" : "无";
                DateTimeRecevie =
                    new DateTime(info.WstSluSvrAnsReadCtrlArgs.CtrlTime).ToString("yyyy-MM-dd HH:mm:ss");
                GrpName = "";

                var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(SluId);
                if (para != null)
                {
                    var slupara = para as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
                    if (slupara != null)
                    {
                        //var grpInfo = SrInfo.CtrGrpInfo.MySelf.GetGrpInfoBySluId(SluId);
                        foreach (var ff in slupara.WjSluCtrlGrps.Values)
                        {
                            if (info.WstSluSvrAnsReadCtrlArgs.CtrlGroup.Contains(ff.GrpId))
                                GrpName = GrpName + "  " + ff.GrpName;
                        }
                    }
                }


                CtrlStatus = "";
                CtrlEnableAlarm = "";
                UplinkReply = "";
                UplinkTimer = 0;
                DomainName = 0;
                Latitude = 0;
                Longitude = 0;

                CtrlVector = "";
             
                CtrlPowerTurnon = "";
            

                RatedPower = "";


                Ef = "";


                if (isViewActive == false)
                {
                    this.ActiveView();
                }
                Remind = "召测成功..." + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (info != null && info.WstSluSvrAnsReadCtrlArgs != null && info.WstSluSvrAnsReadCtrlArgs.CtrlParaField != null && info.WstSluSvrAnsReadCtrlArgs.DataMarkField.ReadCtrlParaField == true)
            {
                if (info.WstSluSvrAnsReadCtrlArgs.CtrlId != ctrlidx) return;
                if (info.WstSluSvrAnsReadCtrlArgs.SluId != sluidx) return;

                var tmpx = info.WstSluSvrAnsReadCtrlArgs.CtrlParaField;
                CtrlStatus = tmpx.CtrlStatus ? "投运" : "停运";
                CtrlEnableAlarm = tmpx.CtrlEnableAlarm ? "是" : "否";
                UplinkReply = tmpx.UplinkReply ? "是" : "否";
                UplinkTimer = tmpx.UplinkTimer;
                DomainName = tmpx.DomainName;
                Latitude = tmpx.Latitude;
                Longitude = tmpx.Longitude;

                CtrlVector = "";
                foreach (var g in tmpx .CtrlVector )
                {
                    CtrlVector += g + "-";
                }
                CtrlPowerTurnon = "";
                foreach (var g in tmpx.CtrlPowerTurnon)
                {
                    CtrlPowerTurnon += g?"是-":"否-";
                }
 

                RatedPower = "";

                //lvf 2018年4月2日11:28:29  额定功率 对应关系 只呈现上限数字
                var dir = new Dictionary<int, string>();
                dir.Add(0, "未设置");
                dir.Add(1, "20");
                dir.Add(2, "100");
                dir.Add(3, "120");
                dir.Add(4, "150");
                dir.Add(5, "200");
                dir.Add(6, "250");
                dir.Add(7, "300");
                dir.Add(8, "400");
                dir.Add(9, "600");
                dir.Add(10, "800");
                dir.Add(11, "1000");
                dir.Add(12, "1500");
                dir.Add(13, "2000");
                dir.Add(14, "50");
                dir.Add(15, "75");

                foreach (var g in tmpx.RatedPower)
                {
                    RatedPower += dir[g] + "-";

                }

                if (CtrlVector.Length > 1)
                    CtrlVector = CtrlVector.Substring(0, CtrlVector.Length - 1);
                if (CtrlPowerTurnon.Length > 1)
                    CtrlPowerTurnon = CtrlPowerTurnon.Substring(0, CtrlPowerTurnon.Length - 1);
                if (RatedPower.Length > 1)
                    RatedPower = RatedPower.Substring(0, RatedPower.Length - 1);

                Remind = "召测成功..." + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //Ef = "0-无;1- 20;14- 50;15- 75;2- 100;3- 120;4- 150;5- 200;6- 250;";
                //Ef += Environment.NewLine;
                //Ef += "7- 300;8- 400;9- 600;10- 800;11- 1000;12- 1500;13- 2000;";

            }


            //var gf = new RunArgs()
            //{
            //    Domain = g.DomainName,
            //    Latitude_longitude = g.Latitude + "," + g.Longitude,
            //    Sunrise_sunset =
            //        t.Sunset / 60 + ":" + t.Sunset % 60 + "-" + t.Sunrise / 60 + ":" + t.Sunrise % 60,
            //    IsRun = g.CtrlStatus,
            //    IsActiveAlarm = g.CtrlEnableAlarm,
            //    LoopCount = tt.CtrlLoop,


            //};
            //for (int i = 0; i < g.CtrlPowerTurnon.Count; i++)
            //{
            //    gf.IsPowerOnLight[i].IsSelected = g.CtrlPowerTurnon[i];
            //}

            //for (int i = 0; i < g.CtrlVector.Count; i++)
            //{
            //    gf.LoopVector[i].Value = g.CtrlVector[i];
            //}

            //for (int i = 0; i < g.RatedPower.Count; i++)
            //{
            //    switch (g.RatedPower[i])
            //    {
            //        case 0:
            //            gf.LoopRatePower[i].Name = "无额定功率";
            //            break;
            //        case 1:
            //            gf.LoopRatePower[i].Name = "0-20";
            //            break;
            //        case 2:
            //            gf.LoopRatePower[i].Name = "21-100";
            //            break;
            //        case 3:
            //            gf.LoopRatePower[i].Name = "101-120";
            //            break;
            //        case 4:
            //            gf.LoopRatePower[i].Name = "121-150";
            //            break;
            //        case 5:
            //            gf.LoopRatePower[i].Name = "151-200";
            //            break;
            //        case 6:
            //            gf.LoopRatePower[i].Name = "201-250";
            //            break;
            //        case 7:
            //            gf.LoopRatePower[i].Name = "251-300";
            //            break;
            //        case 8:
            //            gf.LoopRatePower[i].Name = "301-400";
            //            break;
            //        case 9:
            //            gf.LoopRatePower[i].Name = "401-600";
            //            break;
            //        case 10:
            //            gf.LoopRatePower[i].Name = "601-800";
            //            break;
            //        case 11:
            //            gf.LoopRatePower[i].Name = "801-1000";
            //            break;
            //        case 12:
            //            gf.LoopRatePower[i].Name = "1001-1500";
            //            break;
            //        case 13:
            //            gf.LoopRatePower[i].Name = "1501-2000";
            //            break;
            //        default:
            //            gf.LoopRatePower[i].Name = "";
            //            break;
            //    }
            //}
            //this.RunInfo.Add(gf);
        


        }

        private void ActiveView()
        {
            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
                Wj2096Module.Services.ViewIdAssign.ZcConnArgsViewId, true);
        }
        #region CmdReadArgs
        private DateTime _dtCmdReadArgs;
        private ICommand _cmdCmdReadArgs;

            public ICommand CmdReadArgs
            {
                get
                {
                    if (_cmdCmdReadArgs == null)
                        _cmdCmdReadArgs = new RelayCommand(ExCmdReadArgs, CanExCmdReadArgs, false);
                    return _cmdCmdReadArgs;
                }
            }

            private void ExCmdReadArgs()
            {
                _dtCmdReadArgs = DateTime.Now;

                //var info = Wlst.Sr.ProtocolPhone.LxSlu.wst_read_ctrl_args;// .wlst_cnt_wj2090_order_xc_conn_args ;//.ServerPart.wlst_Wj2090_clinet_xc_conn_args;
                //info.Args.Addr.Add(ctrlidx);
                //info.WstSluReadCtrlArgs.CtrlId = ctrlidx;
                //info.WstSluReadCtrlArgs.SluId = sluidx;
                //info.WstSluReadCtrlArgs.ReadArgs = true;
                //info.WstSluReadCtrlArgs.ReadSunrise = true;
                //info.WstSluReadCtrlArgs.ReadTimer = false;
                //info.WstSluReadCtrlArgs.ReadVer = false;
                //info.WstSluReadCtrlArgs.ReadGroup = false;
                //info.Head.Gid += 1;
                //SndOrderServer.OrderSnd(info);


                var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slu_sgl_ctrl_measure;
                info.Args.Addr.Add(ctrlidx);
                //cid 0:普通选测  1：召测基本参数  2：召测基本参数1
                info.Args.Cid = 2;
                SndOrderServer.OrderSnd(info, 0, 0, true);



                //lvf 记录 召测终端 2018年8月13日15:00:04
                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.ContainsKey(sluidx) == false)
                {
                    Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.Add(sluidx, DateTime.Now);
                }
                else
                {
                    Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus[sluidx] = DateTime.Now;
                }


                Remind = "正在召测..." + DateTime.Now;
            }

            private bool CanExCmdReadArgs()
            {
                return DateTime.Now.Ticks - _dtCmdReadArgs.Ticks > 30000000;
                return false;
            }
        #endregion


            #region CmdSetTime
            private DateTime _dtCmdSetTime;
            private ICommand _cmdCmdSetTime;

            public ICommand CmdSetTime
            {
                get
                {
                    if (_cmdCmdSetTime == null)
                        _cmdCmdSetTime = new RelayCommand(ExCmdSetTime, CanExCmdSetTime, false);
                    return _cmdCmdSetTime;
                }
            }

            private void ExCmdSetTime()
            {
                _dtCmdSetTime = DateTime.Now;

                //var info = Wlst.Sr.ProtocolPhone.LxSlu.wst_read_ctrl_args;// .wlst_cnt_wj2090_order_xc_conn_args ;//.ServerPart.wlst_Wj2090_clinet_xc_conn_args;
                //info.Args.Addr.Add(ctrlidx);
                //info.WstSluReadCtrlArgs.CtrlId = ctrlidx;
                //info.WstSluReadCtrlArgs.SluId = sluidx;
                //info.WstSluReadCtrlArgs.ReadArgs = true;
                //info.WstSluReadCtrlArgs.ReadSunrise = true;
                //info.WstSluReadCtrlArgs.ReadTimer = false;
                //info.WstSluReadCtrlArgs.ReadVer = false;
                //info.WstSluReadCtrlArgs.ReadGroup = false;
                //info.Head.Gid += 1;
                //SndOrderServer.OrderSnd(info);


                var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_zc_or_set;
                info.Args.Addr.Add(ctrlidx);
                //Cid == 0 发送控制器参数，==1 发送控制器经纬度，==2 发送控制器时间
                info.Args.Cid = 2;
                SndOrderServer.OrderSnd(info, 0, 0, true);
                Remind = "正在发送对时..." + DateTime.Now;

            }

            private bool CanExCmdSetTime()
            {
                return DateTime.Now.Ticks - _dtCmdSetTime.Ticks > 30000000;
                return false;
            }
            #endregion


            #region CmdSetXY
            private DateTime _dtCmdSetXY;
            private ICommand _cmdCmdSetXY;

            public ICommand CmdSetXY
            {
                get
                {
                    if (_cmdCmdSetXY == null)
                        _cmdCmdSetXY = new RelayCommand(ExCmdSetXY, CanExCmdSetXY, false);
                    return _cmdCmdSetXY;
                }
            }

            private void ExCmdSetXY()
            {
                _dtCmdSetXY = DateTime.Now;

                //var info = Wlst.Sr.ProtocolPhone.LxSlu.wst_read_ctrl_args;// .wlst_cnt_wj2090_order_xc_conn_args ;//.ServerPart.wlst_Wj2090_clinet_xc_conn_args;
                //info.Args.Addr.Add(ctrlidx);
                //info.WstSluReadCtrlArgs.CtrlId = ctrlidx;
                //info.WstSluReadCtrlArgs.SluId = sluidx;
                //info.WstSluReadCtrlArgs.ReadArgs = true;
                //info.WstSluReadCtrlArgs.ReadSunrise = true;
                //info.WstSluReadCtrlArgs.ReadTimer = false;
                //info.WstSluReadCtrlArgs.ReadVer = false;
                //info.WstSluReadCtrlArgs.ReadGroup = false;
                //info.Head.Gid += 1;
                //SndOrderServer.OrderSnd(info);


                var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_zc_or_set;
                info.Args.Addr.Add(ctrlidx);
                //Cid == 0 发送控制器参数，==1 发送控制器经纬度，==2 发送控制器时间
                info.Args.Cid = 1;
                SndOrderServer.OrderSnd(info, 0, 0, true);
                Remind = "正在发送经纬度..." + DateTime.Now;
            }

            private bool CanExCmdSetXY()
            {
                return DateTime.Now.Ticks - _dtCmdSetXY.Ticks > 30000000;
                return false;
            }
            #endregion
    }
}
