using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DragDropExtend.DragAndDrop;
using Microsoft.Win32;
using Wlst.Cr.Core.ComponentHold;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.ComponentHold;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Cr.CoreOne.Services;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.WJ3005Module.Wj3005TmlInfoSetViewModel.Services;
using System.Diagnostics;
using Wlst.client;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.iif;

namespace Wlst.Ux.WJ3005Module.Wj3005TmlInfoSetViewModel.ViewModel
{
    [Export(typeof (IITmlInformationViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TmlInformationViewModel : EventHandlerHelperExtendNotifyProperyChanged, IITmlInformationViewModel
    {
        public int CountSwitchIn = 16;
        public int CountSwitchOut = 6;
        public int CountAmpLoops = 36;
        public int CountVectorSample = 36;
        public int AreaId;

        public TmlInformationViewModel()
        {
           EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);

           this.AddEventFilterInfo(
               Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate);

            this.InitAction();
        }

      
        #region  define

       // private Wj3005TerminalInformation _wj3005TerminalInformation;


        //
        private ObservableCollection<RtuParaSwitchOutViewModel> _rtuParaSwitchOutViewModels;

        /// <summary>
        /// 开关量输出参数
        /// </summary>
        public ObservableCollection<RtuParaSwitchOutViewModel> RtuParaSwitchOutViewModels
        {
            get
            {
                if (_rtuParaSwitchOutViewModels == null)
                    _rtuParaSwitchOutViewModels = new ObservableCollection<RtuParaSwitchOutViewModel>();
                return _rtuParaSwitchOutViewModels;
            }
            set
            {
                if (value == _rtuParaSwitchOutViewModels) return;
                _rtuParaSwitchOutViewModels = value;
                this.RaisePropertyChanged(() => RtuParaSwitchOutViewModels);
            }
        }

        //
        private ObservableCollection<RtuLoopInfoVm> _rtuParaAnalogueAmpViewModels;

        /// <summary>
        /// 回路参数
        /// </summary>
        public ObservableCollection<RtuLoopInfoVm> RtuParaAnalogueAmpViewModels
        {
            get
            {
                if (_rtuParaAnalogueAmpViewModels == null)
                    _rtuParaAnalogueAmpViewModels = new ObservableCollection<RtuLoopInfoVm>();
                return _rtuParaAnalogueAmpViewModels;
            }
            set
            {
                if (_rtuParaAnalogueAmpViewModels == value) return;
                _rtuParaAnalogueAmpViewModels = value;
                this.RaisePropertyChanged(() => this.RtuParaAnalogueAmpViewModels);
            }
        }

        
          

        private object _SelectedObject;

        /// <summary>
        /// 自动绑定对象，当选择的终端变化时 需要this.RaisePropertyChanged(() => this.SelectedObject);  
        /// 自动基本属性绑定显示
        /// </summary>
        public object SelectedObject
        {
            get
            {
                if (_SelectedObject == null)
                    _SelectedObject = new object();
                return _SelectedObject;
            }
        }

        #endregion

        #region NavOnLoad

        private bool _isViewShow = false;
        public void NavOnLoad(params object[] parsObjects)
        {


        //    int rid = 1000015;
         //   var rxt =
        //     Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rid]
        //     as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
         //   if (rxt == null) return;

            _isViewShow = true;
            RemarkName = Wlst.Sr.EquipmentInfoHolding.Services.Others.RemarkName + ":";


            //读取地区 lvf 2019年5月6日08:23:21
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.RegionItems.Count != 0)
            {
                this.RtuRegion.Clear();
                this.RtuRegion.Add(new NameValueInt() { Name = "无", Value = 0 });
                foreach (var g in Wlst.Sr.EquipmentInfoHolding.Services.Others.RegionItems)
                {
                    this.RtuRegion.Add(new NameValueInt() { Name = g.Item2, Value = g.Item1 });
                }
            }
            //intRtuRegion = 0;


            var tmlId = (int) parsObjects[0];
            if (tmlId > 0)
            {
                SelectedTmlChange(tmlId);
                NavOnLoadInThisViewModelThree();
            }

            if (parsObjects.Length == 2)
            {
                if ((string)parsObjects[1] == "newAdd3005")
                {
                    Read_Para_from_xml(Wlst.Sr.EquipmentInfoHolding.Services.Others.LoadParaPath);
                }
            }

            for (int i = 0; i < _dtSndCheck.Count(); i++) _dtSndCheck[i] = DateTime.Now;


            ShowInfo = "提示:所有发送参数都以服务器端数据为准，请保存数据成功后再下发参数。";
            ShowInfos.Clear();
            Msg = "";
            IsChecked = false;
            DataCreateZcSjEnd = DateTime.Now;
            DataCreateZcSjStart = DateTime.Now.AddHours(-6);





            InitEvent();
            //Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestHttpData,3);
            if (parsObjects.Length==1)
            {
                RequestHttpData();
            }

            //lvf 2019年6月5日14:53:04 海门门控实现线控日夜报警
            if (Wlst.Cr.CoreMims.SystemOption.GetOptionIsDefaults(2001, 0) != true)
            {
                IsShowLineHM = Visibility.Visible;
            }
            else
            {
                IsShowLineHM = Visibility.Collapsed;
            }
        }

        private void LoadRtuTypeXml()
        {


            var info = Elysium.ThemesSet.Common.ReadSave.Read("LocalRtuType", "SystemXmlConfig");
            if( info.Count==0)return;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType.Clear();
            for (int i = 0; i < 6;i++ )
            {
                if (info.ContainsKey(i+""))
                {
                    var typeName = "";
                    var typeColor = "Black";
                    var keyWord = info[i + ""];
                    if (keyWord.Contains(","))
                    {
                        string[] keyLst = keyWord.Split(',');
                        //如果格式不对，直接退出
                        if (keyLst.Count() < 2) return;
                        if (string.IsNullOrEmpty(keyLst[0]) == false) typeName = keyLst[0].Trim();
                        if (string.IsNullOrEmpty(keyLst[1]) == false) typeColor = keyLst[1].Trim();
                        Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType.Add(i, new Tuple<string, string>(typeName,typeColor));
                    }
                    
                }
            }
                
            //if (info.ContainsKey("1"))
            //{
            //    Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType.Add(1, info["1"]);
            //}
            //if (info.ContainsKey("1"))
            //{
            //    Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType.Add(1, info["1"]);
            //}
            //if (info.ContainsKey("1"))
            //{
            //    Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType.Add(1, info["1"]);
            //}
            //if (info.ContainsKey("1"))
            //{
            //    Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType.Add(1, info["1"]);
            //}
        }

        private void LoadRtuRemarkXml()
        {

            //Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverHttpPort = "0";
            var info = Elysium.ThemesSet.Common.ReadSave.Read("LocalRtuRemarks", "SystemXmlConfig");
            if (info.Count == 0) return;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuRemarks.Clear();
            for (int i = 0; i <8; i++)
            {
                if (info.ContainsKey(i + ""))
                {
                    var remarkName = "";

                    remarkName = info[i + ""];
                    if (string.IsNullOrEmpty(remarkName.Trim())) continue;

                    Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuRemarks.Add(i, remarkName);
                }

            }
            //lvf 2019年5月7日10:00:13 采用系统常规选项 里的参数
            //if (info.ContainsKey("port"))
            //{
            //    Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverHttpPort = info["port"];
            //}




            //if (info.ContainsKey("1"))
            //{
            //    Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType.Add(1, info["1"]);
            //}
            //if (info.ContainsKey("1"))
            //{
            //    Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType.Add(1, info["1"]);
            //}
            //if (info.ContainsKey("1"))
            //{
            //    Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType.Add(1, info["1"]);
            //}
            //if (info.ContainsKey("1"))
            //{
            //    Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType.Add(1, info["1"]);
            //}
        }

        public void RequestHttpData()
        {

            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverHttpPort == "0") return;
            for (int i = 0; i < 7; i++)
            {
                RtuRemarks[i]="";
            }

           
            //请求数据协议格式，详见Sr\wlst.sr.iif\GoogleBuf\proto
            var nt = new Wlst.iif.EquimentRemarkRead();
            nt.RtuId= new List<int>();
            nt.RtuId.Add(RtuId);

            //序列化，请求数据结构
            var base64data = System.Convert.ToBase64String(EquimentRemarkRead.SerializeToBytes(nt));

            //http get
            var url ="http://"+ Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverIpAddr + ":" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverHttpPort + "/mims/get10010";//"http://10.3.9.8:18080/mims/get10010"
            var data = wlst.sr.iif.HttpGetPost.HttpGet(url, "?pb2=" + base64data);
            if (data == null) return;
            // 反序列化get到的数据
            var databk = Wlst.iif.EquimentRemarkReturn.Deserialize(System.Convert.FromBase64String(data));
            if (databk == null || databk.EquRemark == null) return;
            RtuRemarks.Clear();
            foreach (var g in databk.EquRemark)
            {
                RtuRemarks.Add(g.Remark1);
                RtuRemarks.Add(g.Remark2);
                RtuRemarks.Add(g.Remark3);
                RtuRemarks.Add(g.Remark4);
                RtuRemarks.Add(g.Remark5);
                RtuRemarks.Add(g.Remark6);
                RtuRemarks.Add(g.Remark7);
            }

        }

        public void PostHttpData(int rtuid)
        {

            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverHttpPort == "0") return;
            //请求数据协议格式，详见Sr\wlst.sr.iif\GoogleBuf\proto
            var nt = new Wlst.iif.EquimentRemarkSave();
            var datat = new List<EquimentRemarkSave.SingleEquimentRemark>();
            var item = new EquimentRemarkSave.SingleEquimentRemark()
                           {
                               Remark1 = RtuRemarks[0],
                               Remark2 = RtuRemarks[1],
                               Remark3 = RtuRemarks[2],
                               Remark4 = RtuRemarks[3],
                               Remark5 = RtuRemarks[4],
                               Remark6 = RtuRemarks[5],
                               Remark7 = RtuRemarks[6],
                               RtuId = rtuid
                           };
            datat.Add(item);
            nt.EquRemark = datat;



            //序列化，请求数据结构
            var base64data = System.Convert.ToBase64String(EquimentRemarkSave.SerializeToBytes(nt));

            //http get
            var url ="http://"+ Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverIpAddr + ":" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverHttpPort + "/mims/post10011";//"http://10.3.9.8:18080/mims/get10010"

            var data = wlst.sr.iif.HttpGetPost.HttpPost(url, "?pb2=" + base64data);
            if (data == null) return;
            // 反序列化get到的数据
            var databk = Wlst.iif.EquimentRemarkReturn.Deserialize(System.Convert.FromBase64String(data));
            if (databk == null || databk.EquRemark == null) return;
            if ( databk.Head.IfSt ==0)  //post 失败
            {
                Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + RtuName + " post失败...";
            }
            //this.RtuRemarks = new ObservableCollection<string>();
            //foreach (var g in databk.EquRemark)
            //{
            //    RtuRemarks.Add(g.Remark1);
            //    RtuRemarks.Add(g.Remark2);
            //    RtuRemarks.Add(g.Remark3);
            //    RtuRemarks.Add(g.Remark4);
            //    RtuRemarks.Add(g.Remark5);
            //    RtuRemarks.Add(g.Remark6);
            //    RtuRemarks.Add(g.Remark7);
            //}

        }


        public void OnUserHideOrClosing()
        {
            //throw new NotImplementedException();
           // _wj3005TerminalInformation = null;
            _isViewShow = false;

            _SelectedObject = null;
            RtuParaAnalogueAmpViewModels = new ObservableCollection<RtuLoopInfoVm>();
            RtuParaSwitchOutViewModels = new ObservableCollection<RtuParaSwitchOutViewModel>();

        }

        /// <summary>
        /// 提供外界更改终端
        /// </summary>
        /// <param name="rtuId">终端地址</param>
        public void SelectedTmlChange(int rtuId)
        {
         

            if (
                !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .
                     ContainsKey(rtuId))
                return;
            var t =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems [rtuId]
                as Wlst .Sr .EquipmentInfoHolding .Model .Wj3005Rtu ;

            if (t == null)
                return;


            //设备型号确认  
            if ((int)t.RtuModel == 3005 )
            {
                CountSwitchIn = 16;
                CountSwitchOut = 6;
                CountAmpLoops = 36;
                CountVectorSample = 36;
                Visi=Visibility.Collapsed;

                CanChangeModuel = true;
                IntRtuModelModify = 1;
            }
            else if ((int)t.RtuModel == 3090)
            {
                CountSwitchIn = 6;
                CountSwitchOut = 4;
                CountAmpLoops = 16;
                CountVectorSample = 16;
                CanChangeModuel = true;
                IntRtuModelModify = 0;
            }
            else if ((int)t.RtuModel == 3006)
            {
                CountSwitchIn = 16;
                CountSwitchOut = 8;
                CountAmpLoops = 36;
                CountVectorSample = 36;
                Visi =Visibility.Visible;
                CanChangeModuel = false;
                IntRtuModelModify = 2;


            }
            else if ((int)t.RtuModel == 6005)
            {
                CountSwitchIn = 16;
                CountSwitchOut = 6;
                CountAmpLoops = 36;
                CountVectorSample = 36;
                Visi = Visibility.Collapsed;

                CanChangeModuel = true;
                IntRtuModelModify = 3;
            }
            else return;
 
            this.TabOneTitle = t.RtuId + "_" + t.RtuName;
            this.RaisePropertyChanged(() => this.SelectedObject);

          

            this.SetTerminalInformationVm(t);
         

            InitViewModel(t);

           
        }

        /// <summary>
        /// 初始化 需要显示的回路、输出、输入信息
        /// </summary>
        private void InitViewModel(Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu info)
        {
            if (info  == null)
                return;

            if (info .WjSwitchOuts .Count   != CountSwitchOut)
            {

                info.WjSwitchOuts .Clear();

                for (byte i = 1; i < CountSwitchOut + 1; i++)
                    info.WjSwitchOuts .Add(  i,
                        new RtuSwitchOutputParameter() 
                            {
                                RtuId = info .RtuId,
                                SwitchName  = "K" + i,
                                SwitchVecotr  = i,
                                SwitchId =i
                            });
            }

            Visi = info.RtuModel == EnumRtuModel.Wj3006 ? Visibility.Visible : Visibility.Collapsed;
            CanChangeModuel = info.RtuModel == EnumRtuModel.Wj3006 ? false : true;

            var tmps = new ObservableCollection<RtuLoopInfoVm>();
            //  RtuParaAnalogueAmpViewModels.Clear();
            var xxx = new Dictionary<int, int>();
            var ntg = (from t in info.WjLoops.Values orderby t.LoopId ascending select t).ToList();
            foreach (var t in ntg)
            {
                var rtuParaAnalogueAmpViewModel = new RtuLoopInfoVm(t, t.SwitchOutputId   > 0);
                tmps.Add(rtuParaAnalogueAmpViewModel);
                if (!xxx.ContainsKey(t.SwitchOutputId)) xxx.Add(t.SwitchOutputId, 0);
                xxx[t.SwitchOutputId] = xxx[t.SwitchOutputId] + 1;
            }
            RtuParaAnalogueAmpViewModels = tmps;

            SumSwitchInLoops = RtuParaAnalogueAmpViewModels.Count;



            RtuParaSwitchOutViewModels.Clear();
            var ntgg = (from t in info.WjSwitchOuts orderby t.Value .SwitchId  ascending select t.Value ).ToList();
            foreach (var t in ntgg)
            {
                var rtuParaSwitchOutViewModel = new RtuParaSwitchOutViewModel(t);

                rtuParaSwitchOutViewModel.LoopSum = xxx.ContainsKey(rtuParaSwitchOutViewModel.SwitchOutId)
                                                        ? xxx[rtuParaSwitchOutViewModel.SwitchOutId]
                                                        : 0;

                RtuParaSwitchOutViewModels.Add(rtuParaSwitchOutViewModel);
            }


        }

        /// <summary>
        /// 将回路信息、输入信、输出信息还原为 终端信息
        /// </summary>
        /// <returns></returns>
        private Wlst .Sr .EquipmentInfoHolding .Model .Wj3005Rtu  BackViewModelToTerminalInformation()
        {
            var loops = new List<Wlst.client.RtuAnalogParameter>();
            var swout = new List<Wlst.client.RtuSwitchOutputParameter>();
            var swint = new List<Wlst.client.RtuSwitchInputParameter>();

            foreach (var t in RtuParaSwitchOutViewModels)
            {
                swout.Add(t.BackRtuParaSwitchOut());
            }


            foreach (var t in RtuParaAnalogueAmpViewModels)
            {
                loops.Add(t.BackRtuParaAnalogueAmp());
            }

        

            return new Wj3005Rtu(BackVmToTerminalInfomationBasePara(), BackVmToTerminalInfomationVoltage(),
                                 BackVmToTerminalInfomationGprs(), loops,  swout);


        }

        private Wlst .client .EquipmentParameter   BackVmToTerminalInfomationBasePara()
        {
            var tmp = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(this.RtuId);
            if (tmp == null) return null;
            var rtuModeltmp = EnumRtuModel.Wj3005;
            if (RtuModelModify[IntRtuModelModify].Value == 3005) rtuModeltmp = EnumRtuModel.Wj3005;
            if (RtuModelModify[IntRtuModelModify].Value == 3006) rtuModeltmp = EnumRtuModel.Wj3006;
            if (RtuModelModify[IntRtuModelModify].Value == 3090) rtuModeltmp = EnumRtuModel.Wj3090;
            if (RtuModelModify[IntRtuModelModify].Value == 6005) rtuModeltmp = EnumRtuModel.Gz6005;

            return new EquipmentParameter()
                       {
                           //AreaId = tmp.AreaId,
                           DateCreate = this.DataCreate.Ticks,
                           DateUpdate = tmp.DateUpdate,
                           RtuId = this.RtuId,
                           RtuName = RtuName,
                           RtuPhyId = this.PhyId,
                           //  .RtuPhyId ,
                           RtuFid = tmp.RtuFid,
                           RtuGisX = tmp.RtuGisX,
                           RtuGisY = tmp.RtuGisY,
                           RtuArgu = tmp.RtuArgu,
                           RtuInstallAddr = InstallAddr,
                           RtuMapX = Xmap,
                           RtuMapY = Ymap,
                           RtuModel = rtuModeltmp,//tmp.RtuModel,
                           RtuRemark = Remark,
                           RtuStateCode = RtuState > 2 ? 0 : RtuState,
                           RtuRealState = RtuRealState > 2 ? 0 : RtuRealState,
                           Idf = RtuIdf,
                           RtuRegion = this.intRtuRegion,
                       };

        }

        private Wlst.client.RtuVoltageParameter BackVmToTerminalInfomationVoltage()
        {
            return new RtuVoltageParameter()
                       {
                           //AShield = this.AShield,
                           //IsShieldLittleA = this.IsShieldLittleA,
                           IsSwitchinputJudgebyA = this.IsSwitchinputJudgebyA,
                           RtuId = this.RtuId,
                           VoltageAlarmLowerlimit = this.LowerLimit,
                           VoltageAlarmUpperlimit = this.UpperLimit,
                           VoltageRange = this.Range,
                           RtuUsedType = this.intRtuType+1,
                           Aoverload = this.Aoverload,
                           AoverAlittle = this.AoverAlittle,
                           IsHasElec = this.IsHasElec,
                           AphaseRadio = this.AphaseRadio,
                           BphaseRadio = this.BphaseRadio,
                           CphaseRadio = this.CphaseRadio
                       };

        }

        private Wlst.client.RtuGprsParameter BackVmToTerminalInfomationGprs()
        {
            var info = new RtuGprsParameter();
            info.IsAlarm = this.Alarm;
            info.IsBoot = this.Boot;
            info.IsCall = this.Call;
            //  info.CommType = this.CommType;
            info.IsDisplay = this.Display;
            info.RtuAlarmDelay = this.ErrorDelays;
            info.RtuHeartbeatCycle = this.HeartBeatPeriod;

            try
            {
                info.StaticIp = BitConverter.ToInt32(
                    System.Net.IPAddress.Parse(Ip).GetAddressBytes(), 0); // Convert.ToInt32(this.Ip);
            }catch (Exception ex)
            {
                
            }
            info.RtuCommPort = this.Port;


            info.IsReport = this.Report;
            info.RtuReportCycle = this.ReportDataPeriod;
            info.IsRoute = this.Route;
            info.RtuId = this.RtuId;



            info.IsSelfcheck = this.Selfcheck;

            info.MobileNo = this.SimNumber;
            info.IsSound = this.Sound;
             
            return info;
        }

        #endregion


        private string _TabOneTitle;

        public string TabOneTitle
        {
            get { return _TabOneTitle; }
            set
            {
                if (_TabOneTitle == value) return;
                _TabOneTitle = value;
                this.RaisePropertyChanged(() => this.TabOneTitle);
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

        private DateTime dtSnd;


        private int _SumSwitchInLoops;

        /// <summary>
        /// 总的开关量路数
        /// </summary>
        public int SumSwitchInLoops
        {
            get { return _SumSwitchInLoops; }
            set
            {
                if (value != _SumSwitchInLoops)
                {
                    _SumSwitchInLoops = value;
                    this.RaisePropertyChanged(() => this.SumSwitchInLoops);
                }
            }
        }

        private Visibility _txtVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility Visi
        {
            get { return _txtVisi; }
            set
            {
                if (value != _txtVisi)
                {
                    _txtVisi = value;
                    this.RaisePropertyChanged(() => this.Visi);
                }
            }
        }

        private bool _canChangeModuel;

        /// <summary>
        /// 3006不能切换 设备类型
        /// </summary>
        public bool CanChangeModuel
        {
            get { return _canChangeModuel; }
            set
            {
                if (value != _canChangeModuel)
                {
                    _canChangeModuel = value;
                    this.RaisePropertyChanged(() => this.CanChangeModuel);
                }
            }
        }

        #region  回路信息修改

        private void ExAmpChanged()
        {
            #region 开关量名称更新

            //var tmp2 = new Dictionary<int, RtuSwitchOutputParameter>();
            //foreach (var t in RtuParaSwitchOutViewModels)
            //{
            //    if (!tmp2.ContainsKey(t.SwitchOutId)) tmp2.Add(t.SwitchOutId, new RtuSwitchOutputParameter(){RtuId = t.RtuId ,SwitchId = t.SwitchOutId ,SwitchName = t.SwichtOutName ,SwitchVecotr = t.Vector });
            //}

            #endregion
            var dir = new Dictionary<int, List<RtuLoopInfoVm>>();
            int oldloopcount = this.RtuParaAnalogueAmpViewModels.Count;

            int xcount = 0;
            foreach (var t in RtuParaSwitchOutViewModels) xcount += t.LoopSum;
            if (xcount > 48)  //36
            {
                UMessageBox.Show("超电流矢量总数...",
                                 "电流矢量总数不得超过48路，请确认!!!",
                                 UMessageBoxButton.Yes);
                return;
            }
            if (SumSwitchInLoops > 52)//40
            {
                UMessageBox.Show("超开关量总数...",
                                 "开关量总数不得超过52路，请确认!!!",
                                 UMessageBoxButton.Yes);
                return;
            }

            foreach (var t in RtuParaAnalogueAmpViewModels)
            {
                if (!dir.ContainsKey(t.SwitchOutId)) dir.Add(t.SwitchOutId, new List<RtuLoopInfoVm>());
                dir[t.SwitchOutId].Add(t);
            }
            if (dir.ContainsKey(0))
            {
                if (xcount + dir[0].Count > SumSwitchInLoops)
                {
                    var infso = UMessageBox.Show("请确认...",
                                                 "开关量回路是否正确？ 继续设置？",
                                                 UMessageBoxButton.YesNo);
                    if (infso == false) return;
                }
            }

            int monicount = 0;
            foreach (var t in RtuParaSwitchOutViewModels)
            {
                int outloopid = t.SwitchOutId;
                
                if (outloopid < 1 || outloopid > 8) continue;
                monicount += t.LoopSum;

                if (!dir.ContainsKey(outloopid)) dir.Add(outloopid, new List<RtuLoopInfoVm>());
                int x = dir[outloopid].Count - t.LoopSum;

                if (x > 0) //-
                {
                    var lst = (from g in dir[outloopid] orderby g.VectorMoniliang descending select g).ToList();
                    int k = 0;
                    for (k = 0; k < x; k++)
                    {
                        if (lst.Count > k && dir[outloopid].Contains(lst[k])) dir[outloopid].Remove(lst[k]);
                    }
                }
                if (x < 0) //+
                {
                    RtuLoopInfoVm vm = null;

                    //开关量地址 找寻
                    int vsin = 1;
                    var tmpsssssdf = (from g in dir from f in g.Value select f.VectorSwitchIn).ToList();

                    int phase = -1;

                    if (dir[outloopid].Count > 0)
                    {
                        phase = dir[outloopid][dir[outloopid].Count - 1].Phase;
                        vm = dir[outloopid][0];
                    }
                    else
                    {
                        for (int i = 1; i < 17; i++)
                        {
                            if (!tmpsssssdf.Contains(i))
                            {
                                vsin = i;
                                break;
                            }
                        }
                    }



                    for (int k = x; k < 0; k++)
                    {
                        phase += 1;
                        phase = phase % 3;

                        //模拟量地址 找寻
                        int vsmoni = 1;
                        var tmpsggggssdf = (from g in dir from f in g.Value select f.VectorMoniliang).ToList();

                        for (int i = 1; i < 49; i++)
                        {
                            if (!tmpsggggssdf.Contains(i))
                            {
                                vsmoni = i;
                                break;
                            }
                        }
                        RtuLoopInfoVm tmp = null;
                        if (vm != null) tmp = new RtuLoopInfoVm(vm);
                        else
                        {
                            tmp = new RtuLoopInfoVm(RtuId, outloopid, true) {VectorSwitchIn = vsin};
                        }

                        tmp.Phase = phase;

                        tmp.VectorMoniliang = vsmoni;
                        dir[outloopid].Add(tmp);


                    }
                }

            }
            if (!dir.ContainsKey(0)) dir.Add(0, new List<RtuLoopInfoVm>());
            if (monicount + dir[0].Count < SumSwitchInLoops)
            {
                int ggs = monicount + dir[0].Count;
                for (int x = ggs; x < SumSwitchInLoops; x++)
                {
                    //开关量地址 找寻
                    int vsin = 1;
                    var tmpsssssdf = (from g in dir from f in g.Value select f.VectorSwitchIn).ToList();
                    for (int i = 1; i < 17; i++)
                    {
                        if (!tmpsssssdf.Contains(i))
                        {
                            vsin = i;
                            break;
                        }
                    }
                    dir[0].Add(new RtuLoopInfoVm(RtuId, 0, false) {VectorSwitchIn = vsin});
                }
            }
            if (monicount + dir[0].Count > SumSwitchInLoops)
            {
                if (SumSwitchInLoops < monicount)
                {
                    SumSwitchInLoops = monicount;
                }
                int del = monicount + dir[0].Count - SumSwitchInLoops;
                var lst = (from gggg in dir[0] orderby gggg.VectorSwitchIn descending select gggg).ToList();


                int tmpxxx = monicount + dir[0].Count - SumSwitchInLoops;
                for (int x = 0; x < tmpxxx; x++)
                {
                    if (lst.Count > x && dir[0].Contains(lst[x]))
                        dir[0].Remove(lst[x]);
                }
            }

            int index = 0;
            //    RtuParaAnalogueAmpViewModels.Clear();
            var tmpsg = new ObservableCollection<RtuLoopInfoVm>();
            var tmpssss = (from t in dir where t.Key > 0 orderby t.Key ascending select t.Value).ToList();
            foreach (var tmps in tmpssss)
            {

                foreach (var g in tmps)
                {
                    index++;
                    g.LoopId = index;
                    if (g.LoopName == "新回路") g.LoopName = "新回路" + index;
                    tmpsg.Add(g);

                }
            }
            foreach (var g in dir[0])
            {
                index++;
                g.LoopId = index;
                tmpsg.Add(g);
            }
            //  RtuParaAnalogueAmpViewModels.Clear();
            RtuParaAnalogueAmpViewModels = tmpsg;
            if (oldloopcount == 0)
            {
                int OutId = 0;
                int ampabc = 0;
                for (int i = 0; i < this.RtuParaAnalogueAmpViewModels.Count; i++)
                {
                    if (RtuParaAnalogueAmpViewModels[i].SwitchOutId != OutId)
                    {
                        OutId = RtuParaAnalogueAmpViewModels[i].SwitchOutId;
                        ampabc = 0;
                        RtuParaAnalogueAmpViewModels[i].Phase = ampabc;
                        ampabc++;
                        if (ampabc > 2) ampabc = 0;
                    }
                    else
                    {
                        RtuParaAnalogueAmpViewModels[i].Phase = ampabc;
                        ampabc++;
                        if (ampabc > 2) ampabc = 0;
                    }
                    RtuParaAnalogueAmpViewModels[i].LoopName = "新回路" + RtuParaAnalogueAmpViewModels[i].LoopId;
                }
            }

        }

        private RelayCommand _cmdCmdAmpChanged;

        /// <summary>
        /// 确认回路信息修改
        /// </summary>
        public ICommand CmdAmpChanged
        {
            get { return _cmdCmdAmpChanged ?? (_cmdCmdAmpChanged = new RelayCommand(ExAmpChanged, CanCmdAmpChanged, true)); }
        }

        private bool CanCmdAmpChanged()
        {
            return true;
        }

        #endregion

        #region  回路名称修改

        private void ExLoopNameChanged()
        {
            int[] loopNumber = new int[6];

            foreach (var t in this.RtuParaAnalogueAmpViewModels)
            {
                if (t.SwitchOutId == 1)
                {
                    loopNumber[0]++;
                }
                else if (t.SwitchOutId == 2)
                {
                    loopNumber[1]++;
                }
                else if (t.SwitchOutId == 3)
                {
                    loopNumber[2]++;
                }
                else if (t.SwitchOutId == 4)
                {
                    loopNumber[3]++;
                }
                else if (t.SwitchOutId == 5)
                {
                    loopNumber[4]++;
                }
                else if (t.SwitchOutId == 6)
                {
                    loopNumber[5]++;
                }
            }

            int[] loopNumberIndex = new int[6];

            foreach (var t in this.RtuParaAnalogueAmpViewModels)
            {
                if ((t.SwitchOutId >= 1) && (t.SwitchOutId <= 6))
                {
                    if ((loopNumber[t.SwitchOutId - 1]) == 2)
                    {
                        if (loopNumberIndex[t.SwitchOutId - 1] == 0)
                        {
                            t.LoopName = "回路" + t.SwitchOutId + "火线";
                        }
                        else if (loopNumberIndex[t.SwitchOutId - 1] == 1)
                        {
                            t.LoopName = "回路" + t.SwitchOutId + "零线";
                        }

                        loopNumberIndex[t.SwitchOutId - 1]++;
                    }
                }
            }
        }

        private RelayCommand _cmdLoopNameChanged;

        /// <summary>
        /// 回路名称修改
        /// </summary>
        public ICommand CmdLoopNameChanged
        {
            get { return _cmdLoopNameChanged ?? (_cmdLoopNameChanged = new RelayCommand(ExLoopNameChanged, CanLoopNameChanged, true)); }
        }

        private bool CanLoopNameChanged()
        {
            return true;
        }

        #endregion

        #region  保存终端信息

        private Wj3005Rtu Get_Wj3005Rtu_Para()
        {
            var tmp = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(this.RtuId);
            if (tmp == null) return null;
            if (tmp.RtuPhyId != PhyId)
            {
                bool existphyid = false;
                //bool existidf = false;
                int rtuIdExist = 0;
                foreach (
                    var tf in
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
                {
                    if (tf.Value.EquipmentType == Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase.EquType.Rtu)
                    {
                        if (tf.Value.RtuPhyId == PhyId)
                        {
                            existphyid = true;
                            rtuIdExist = tf.Value.RtuId;
                            break;
                        }
                        //if (tf.Value.Idf == RtuIdf && tf.Value.Idf != "")
                        //{
                        //    existidf = true;
                        //    rtuIdExist = tf.Value.RtuId;
                        //    break;
                        //}
                    }
                }
                if (existphyid)
                {
                    Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("该物理地址已经存在",
                                                                                   "该地址已被设备:" + rtuIdExist +
                                                                                   " 使用，请重新设置...",
                                                                                   UMessageBoxButton.Ok);
                    return null;

                }
                //if (existidf)
                //{
                //    Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("该终端识别码已经存在",
                //                                                                   "该地址已被设备:" + rtuIdExist +
                //                                                                   " 使用，请重新设置...",
                //                                                                   UMessageBoxButton.Ok);
                //    return;

                //}

            }
            if (tmp.Idf != RtuIdf)
            {

                bool existidf = false;
                int rtuIdExist = 0;
                foreach (
                    var tf in
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
                {
                    if (tf.Value.EquipmentType == Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase.EquType.Rtu)
                    {

                        if (tf.Value.Idf == RtuIdf && tf.Value.Idf != "")
                        {
                            existidf = true;
                            rtuIdExist = tf.Value.RtuId;
                            break;
                        }
                    }
                }

                if (existidf)
                {
                    Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("该终端识别码已经存在",
                                                                                   "该识别码已被设备:" + rtuIdExist +
                                                                                   " 使用，请重新设置...",
                                                                                   UMessageBoxButton.Ok);
                    return null;

                }

            }
            if (RtuState == 1)
            {
                var info = Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("确认", "终端设置为 停运 ，是否确定停运该终端？\r\n请到最后界面发送停运指令",
                                                                                          UMessageBoxButton.YesNo);
                if (info != true)
                {
                    return null;
                }
            }
            if (RtuState == 3)
            {
                var info = Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("确认", "终端设置为 不用 ，是否确定不用该终端？",
                                                                                          UMessageBoxButton.YesNo);
                if (info != true)
                {
                    return null;
                }
            }

            if ((int)tmp.RtuModel != RtuModelModify[IntRtuModelModify].Value )
            {
                if ((int)tmp.RtuModel == 3090)
                {
                    var info = Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("确认",
                                                                                              (int)tmp.RtuModel +
                                                                                              "型设备切换至" +
                                                                                              RtuModelModify[
                                                                                                  IntRtuModelModify].
                                                                                                  Value +
                                                                                              "型设备，\n3090自带线路防盗功能将会丢失，是否继续",
                                                                                              UMessageBoxButton.YesNo);
                    if (info != true)
                    {
                        return null;
                    }
                }
                else
                {
                    var info = Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("确认",
                                                                                              (int)tmp.RtuModel +
                                                                                              "型设备切换至" +
                                                                                              RtuModelModify[
                                                                                                  IntRtuModelModify].
                                                                                                  Value + "型设备，是否继续",
                                                                                              UMessageBoxButton.YesNo);
                    if (info != true)
                    {
                        return null;
                    }
                }

            }

            if ((int)tmp.RtuModel != RtuModelModify[IntRtuModelModify].Value)
            {
                bool needAlarm = false;
                var loops = new List<Wlst.client.RtuAnalogParameter>();
                foreach (var t in RtuParaAnalogueAmpViewModels)
                {
                    loops.Add(t.BackRtuParaAnalogueAmp());
                }

                if ((int)RtuModelModify[IntRtuModelModify].Value == 3090)
                {
                    foreach (var g in loops)
                    {
                        if (g.SwitchOutputId > 4)
                        {
                            needAlarm = true;
                            break;
                        }
                    }
                }
                if ((int)RtuModelModify[IntRtuModelModify].Value == 3005)
                {
                    foreach (var g in loops)
                    {
                        if (g.SwitchOutputId > 6)
                        {
                            needAlarm = true;
                            break;
                        }
                    }
                }

                if (needAlarm)
                {
                    Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show(" 请重新设置...", "回路参数不符合" + RtuModelModify[IntRtuModelModify].Value + "型设备，\n请手动修改", UMessageBoxButton.Ok);
                    return null;

                }
            }

            if (!CheckLoopCanBeSave()) return null;

            return BackViewModelToTerminalInformation();

        }

        private void SubmitExecute()
        {
           var ins = Get_Wj3005Rtu_Para();

            if (ins != null)
            {
                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.UpdateEquipmentInfo(ins);
                Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + RtuName + " 终端参数已经提交服务器，请等待...";
                dtSnd = DateTime.Now;


                PostHttpData(this.RtuId);
            }
            //new LoadUpdateDeleteTerminalInfo().AddOrUpdateTmlInfomation(ins);
        }
        /// <summary>
        /// 执行终端回路参数 严格检测
        /// </summary>
        /// <returns></returns>
        private bool CheckLoopCanBeSave()
        {
            Dictionary<int, int> outloo = new Dictionary<int, int>();
            foreach (var t in this.RtuParaSwitchOutViewModels)
            {
                if (t.Vector == 0)
                {
                    UMessageBox.Show("开关量输出矢量配置错误", "开关量输出矢量起始值为1，不得为0.请修改..." + t.Vector, UMessageBoxButton.Ok);
                    return false;
                }
                //if (outloo.ContainsKey(t.Vector))
                //{
                //    UMessageBox.Show("开关量输出矢量配置错误", "存在相同的开关量输出矢量：" + t.Vector, UMessageBoxButton.Ok);
                //    return false;
                //}
                //else
                {
                    outloo.Add( t.SwitchOutId,t.Vector);
                }
            }
            //var lst = (from t in outloo.Values orderby t ascending select t).ToList();
            //for (int i = 1; i < lst.Count + 1; i++)
            //{
            //    if (lst[i - 1] != i)
            //    {
            //        UMessageBox.Show("开关量输出矢量配置错误", "开关量输出矢量地址: " + i + " 丢失...", UMessageBoxButton.Ok);
            //        return false;
            //    }
            //}

            var monil = new Dictionary<int, int>();
            var kaiguanl = new Dictionary<int, int>();

            foreach (var t in this.RtuParaAnalogueAmpViewModels)
            {
                if (t.SwitchOutId < 1) continue;

                if (t.VectorMoniliang < 1)
                {
                    UMessageBox.Show("电流矢量配置错误", "电流矢量起始值为1，不得小于1.\n请修改回路 " + t.LoopId + "电流矢量。",
                                     UMessageBoxButton.Ok);
                    return false;
                }
                if (t.VectorMoniliang > 48) //36
                {
                    UMessageBox.Show("电流矢量配置错误", "电流矢量最大值为48.\n请修改回路 " + t.LoopId + "电流矢量。", UMessageBoxButton.Ok);
                    return false;
                }

                //if (monil.ContainsKey(t.VectorMoniliang))
                //{
                //    UMessageBox.Show("模拟量矢量配置错误",
                //                     "回路" + t.LoopId + "与回路" + monil[t.VectorMoniliang] + " 模拟量矢量相同，不允许,请修改.",
                //                     UMessageBoxButton.Ok);
                //    return false;
                //}
                //monil.Add(t.VectorMoniliang, t.LoopId);
                //if (!kaiguanl.ContainsKey(t.VectorSwitchIn)) kaiguanl.Add(t.VectorSwitchIn, t.LoopId);
                
                if (t.UpperLimit < t.LowerLimit ) 
                {
                    UMessageBox.Show("电流上下限配置错误", "电流上限小于电流下限.\n请修改回路 " + t.LoopId + "电流上下限参数。", UMessageBoxButton.Ok);
                    return false;
                }

                if (t.UpperLimit > t.AmRange && t.AmRange>0)
                {
                    UMessageBox.Show("电流上限或量程配置错误", "电流上限大于电流量程.\n请修改回路 " + t.LoopId + "电流上限或量程参数。", UMessageBoxButton.Ok);
                    return false;
                }

                if (t.AmRange > 1275)
                {
                    UMessageBox.Show("电流量程配置错误", "电流量程在0-1275之间.\n请修改回路 " + t.LoopId + "电流量程参数。", UMessageBoxButton.Ok);
                    return false;
                }
                if (t.Range > 1275)
                {
                    UMessageBox.Show("互感器比配置错误", "互感器比在0-1275之间.\n请修改回路 " + t.LoopId + "互感器比参数。", UMessageBoxButton.Ok);
                    return false;
                }

                double isshieldlittlea; 
                if (t.IsShieldLittleA == "不屏蔽")
                    isshieldlittlea = 0;
                else
                {
                    isshieldlittlea = double.Parse(t.IsShieldLittleA);
                }

                if (isshieldlittlea > t.AmRange)
                {
                    UMessageBox.Show("屏蔽小电流或量程配置错误", "\n屏蔽小电流数值大于电流量程.请修改回路 " + t.LoopId + "屏蔽小电流或量程参数。", UMessageBoxButton.Ok);
                    return false;
                }
            }



            foreach (var t in this.RtuParaAnalogueAmpViewModels)
            {
                if (t.SwitchOutId > 0) continue;

                if (t.VectorSwitchIn < 1)
                {
                    UMessageBox.Show("开关量矢量配置错误", "\n开关量矢量起始值为1，不得小于1.请修改回路 " + t.LoopId + "开关量。",
                                     UMessageBoxButton.Ok);
                    return false;
                }
                if (t.VectorSwitchIn > 16)
                {
                    UMessageBox.Show("开关量矢量配置错误", "\n开关量矢量最大值为16.请修改回路 " + t.LoopId + "开关量。", UMessageBoxButton.Ok);
                    return false;
                }

                //if (kaiguanl.ContainsKey(t.VectorSwitchIn))
                //{
                //    UMessageBox.Show("开关量矢量配置错误",
                //                     "回路" + t.LoopId + "与回路" + kaiguanl[t.VectorSwitchIn] + " 开关量矢量相同，不允许,请修改.",
                //                     UMessageBoxButton.Ok);
                //    return false;
                //}
                //kaiguanl.Add(t.VectorMoniliang, t.LoopId);
            }

            return true;
        }

        private RelayCommand _relayCommand;

        /// <summary>
        /// 提交更新  保存终端信息你
        /// </summary>
        public ICommand SaveAllCommand
        {
            get { return _relayCommand ?? (_relayCommand = new RelayCommand(SubmitExecute, CanExSaveAllCommand, true)); }
        }

        private bool CanExSaveAllCommand()
        {
            return true;
        }

        #endregion

        #region SaveParatoXml
        private RelayCommand _saveParatoXml;

        /// <summary>
        /// 保存预存参数
        /// </summary>
        public ICommand SaveParatoXml
        {
            get { return _saveParatoXml ?? (_saveParatoXml = new RelayCommand(ExSaveParatoXml, CanSaveParatoXml, true)); }
        }

        private bool CanSaveParatoXml()
        {
            return true;
        }

        private void Read_Para_from_xml(string path)
        {
            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(path);


            if (info.ContainsKey("IsAlarm"))
            {
                Alarm = info["IsAlarm"].Contains("True");
            }

            if (info.ContainsKey("IsDisplay"))
            {
                Display = info["IsDisplay"].Contains("True");
            }

            if (info.ContainsKey("IsBoot"))
            {
                Boot = info["IsBoot"].Contains("True");
            }

            if (info.ContainsKey("IsSound"))
            {
                Sound = info["IsSound"].Contains("True");
            }

            if (info.ContainsKey("IsReport"))
            {
                Report = info["IsReport"].Contains("True");
            }

            if (info.ContainsKey("IsCall"))
            {
                Call = info["IsCall"].Contains("True");
            }

            if (info.ContainsKey("IsRoute"))
            {
                Route = info["IsRoute"].Contains("True");
            }

            if (info.ContainsKey("IsSelfcheck"))
            {
                Selfcheck = info["IsSelfcheck"].Contains("True");
            }

            if (info.ContainsKey("MobileNo"))
            {
                SimNumber = info["MobileNo"];
            }

            if (info.ContainsKey("RtuAlarmDelay"))
            {
                ErrorDelays = Convert.ToInt32(info["RtuAlarmDelay"]);
            }

            if (info.ContainsKey("RtuCommPort"))
            {
                Port = Convert.ToInt32(info["RtuCommPort"]);
            }

            if (info.ContainsKey("RtuHeartbeatCycle"))
            {
                HeartBeatPeriod = Convert.ToInt32(info["RtuHeartbeatCycle"]);
            }

            if (info.ContainsKey("RtuReportCycle"))
            {
                ReportDataPeriod = Convert.ToInt32(info["RtuReportCycle"]);
            }

            if (info.ContainsKey("AoverAlittle"))
            {
                AoverAlittle = Convert.ToDouble(info["AoverAlittle"]);
            }

            if (info.ContainsKey("IsHasElec"))
            {
                IsHasElec = info["IsHasElec"].Contains("True"); 
            }


            if (info.ContainsKey("AphaseRadio"))
            {
                AphaseRadio = Convert.ToInt32(info["AphaseRadio"]);
            }

            if (info.ContainsKey("BphaseRadio"))
            {
                BphaseRadio = Convert.ToInt32(info["BphaseRadio"]);
            }

            if (info.ContainsKey("CphaseRadio"))
            {
                CphaseRadio = Convert.ToInt32(info["CphaseRadio"]);
            }

            if (info.ContainsKey("Aoverload"))
            {
                Aoverload = Convert.ToDouble(info["Aoverload"]);
            }

            if (info.ContainsKey("IsSwitchinputJudgebyA"))
            {
                IsSwitchinputJudgebyA = info["IsSwitchinputJudgebyA"].Contains("True"); ;
            }

            if (info.ContainsKey("VoltageAlarmLowerlimit"))
            {
                LowerLimit = Convert.ToInt32(info["VoltageAlarmLowerlimit"]);
            }

            if (info.ContainsKey("VoltageAlarmUpperlimit"))
            {
                UpperLimit = Convert.ToInt32(info["VoltageAlarmUpperlimit"]);
            }

            if (info.ContainsKey("VoltageRange"))
            {
                Range = Convert.ToInt32(info["VoltageRange"]);
            }

            int switchOutCount = 0;

            if (info.ContainsKey("SwitchOutCount"))
            {
                switchOutCount = Convert.ToInt32(info["SwitchOutCount"]);
            }

            int index = 1;

            foreach (var t in RtuParaSwitchOutViewModels)
            {
                if (info.ContainsKey("SwitchId" + index))
                {
                    t.SwitchOutId = Convert.ToInt32(info["SwitchId" + index]);
                }

                if (info.ContainsKey("SwitchName" + index))
                {
                    t.SwichtOutName = info["SwitchName" + index];
                }

                if (info.ContainsKey("SwitchVecotr" + index))
                {
                    t.Vector = Convert.ToInt32(info["SwitchVecotr" + index]);
                }

                if (info.ContainsKey("LoopSum" + index))
                {
                    t.LoopSum = Convert.ToInt32(info["LoopSum" + index]);
                }

                index++;
            }

            if (switchOutCount > 6)
            {
                Visi = Visibility.Visible;
                CanChangeModuel = false;
            }

            if (info.ContainsKey("SumSwitchInLoops"))
            {
                SumSwitchInLoops = Convert.ToInt32(info["SumSwitchInLoops"]);
            }

            ExAmpChanged();

            index = 1;

            foreach (var t in RtuParaAnalogueAmpViewModels)
            {
                if (info.ContainsKey("CurrentAlarmLowerlimit" + index))
                {
                    t.LowerLimit = Convert.ToInt32(info["CurrentAlarmLowerlimit" + index]);
                }

                if (info.ContainsKey("CurrentAlarmUpperlimit" + index))
                {
                    t.UpperLimit = Convert.ToInt32(info["CurrentAlarmUpperlimit" + index]);
                }

                if (info.ContainsKey("CurrentRange" + index))
                {
                    t.AmRange = Convert.ToInt32(info["CurrentRange" + index]);
                }

                if (info.ContainsKey("IsAlarmHop" + index))
                {
                    t.IsAlarmSwitch = info["IsAlarmHop" + index].Contains("True");
                }

                if (info.ContainsKey("IsShieldLoop" + index))
                {
                    t.IsShieldLoop = Convert.ToInt32(info["IsShieldLoop" + index]);
                }

                if (info.ContainsKey("IsSwitchStateClose" + index))
                {
                    t.IsSwitchStateClose = info["IsSwitchStateClose" + index].Contains("True");
                }

                if (info.ContainsKey("LoopId" + index))
                {
                    t.LoopId = Convert.ToInt32(info["LoopId" + index]);
                }

                if (info.ContainsKey("LoopName" + index))
                {
                    t.LoopName = info["LoopName" + index];
                }

                if (info.ContainsKey("MutualInductorRatio" + index))
                {
                    t.Range = Convert.ToInt32(info["MutualInductorRatio" + index]);
                }

                if (info.ContainsKey("ShieldLittleA" + index))
                {
                    t.IsShieldLittleA = Convert.ToString(info["ShieldLittleA" + index]);
                }

                if (info.ContainsKey("SwitchOutputId" + index))
                {
                    t.SwitchOutId = Convert.ToInt32(info["SwitchOutputId" + index]);
                }

                if (info.ContainsKey("VectorMoniliang" + index))
                {
                    t.VectorMoniliang = Convert.ToInt32(info["VectorMoniliang" + index]);
                }


                if (info.ContainsKey("VectorSwitchIn" + index))
                {
                    t.VectorSwitchIn = Convert.ToInt32(info["VectorSwitchIn" + index]);
                }

                if (info.ContainsKey("VoltagePhaseCode" + index))
                {
                    t.Phase = Convert.ToInt32(info["VoltagePhaseCode" + index]);
                }

                index++;

            }

            ExLoopNameChanged();


        }

        private void Read_Para_from_xml()
        {
            string dir = Directory.GetCurrentDirectory() + "\\SystemXmlConfig\\WJ3005Para";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            string extension = "xml";
            var dialog = new OpenFileDialog()
            {
                InitialDirectory = dir,
                DefaultExt = extension,
                Filter =
                    String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*",
                                  extension,
                                  "Xml File"),
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == true)
            {
                Read_Para_from_xml("WJ3005Para\\" + dialog.SafeFileName);

            }
        }

        private void ExSaveParatoXml()
        {
            var ins = Get_Wj3005Rtu_Para();

            if (ins != null)
            {
                var dicDesc = new Dictionary<string, string>();

                dicDesc.Add("IsAlarm", Convert.ToString(ins.WjGprs.IsAlarm));
                dicDesc.Add("IsBoot", Convert.ToString(ins.WjGprs.IsBoot));
                dicDesc.Add("IsCall", Convert.ToString(ins.WjGprs.IsCall));
                dicDesc.Add("IsDisplay", Convert.ToString(ins.WjGprs.IsDisplay));
                dicDesc.Add("IsReport", Convert.ToString(ins.WjGprs.IsReport));
                dicDesc.Add("IsRoute", Convert.ToString(ins.WjGprs.IsRoute));
                dicDesc.Add("IsSelfcheck", Convert.ToString(ins.WjGprs.IsSelfcheck));
                dicDesc.Add("IsSound", Convert.ToString(ins.WjGprs.IsSound));
                dicDesc.Add("MobileNo", Convert.ToString(ins.WjGprs.MobileNo));
                dicDesc.Add("RtuAlarmDelay", Convert.ToString(ins.WjGprs.RtuAlarmDelay));
                dicDesc.Add("RtuCommPort", Convert.ToString(ins.WjGprs.RtuCommPort));
                dicDesc.Add("RtuHeartbeatCycle", Convert.ToString(ins.WjGprs.RtuHeartbeatCycle));
                dicDesc.Add("RtuReportCycle", Convert.ToString(ins.WjGprs.RtuReportCycle));

                dicDesc.Add("AoverAlittle", Convert.ToString(ins.WjVoltage.AoverAlittle));
                dicDesc.Add("Aoverload", Convert.ToString(ins.WjVoltage.Aoverload));
                dicDesc.Add("IsSwitchinputJudgebyA", Convert.ToString(ins.WjVoltage.IsSwitchinputJudgebyA));
                dicDesc.Add("VoltageAlarmLowerlimit", Convert.ToString(ins.WjVoltage.VoltageAlarmLowerlimit));
                dicDesc.Add("VoltageAlarmUpperlimit", Convert.ToString(ins.WjVoltage.VoltageAlarmUpperlimit));
                dicDesc.Add("VoltageRange", Convert.ToString(ins.WjVoltage.VoltageRange));
                //dicDesc.Add("Aoverload", Convert.ToString(ins.WjVoltage.Aoverload));


                dicDesc.Add("IsHasElec", Convert.ToString(ins.WjVoltage.IsHasElec));
                dicDesc.Add("AphaseRadio", Convert.ToString(ins.WjVoltage.AphaseRadio));
                dicDesc.Add("BphaseRadio", Convert.ToString(ins.WjVoltage.BphaseRadio));
                dicDesc.Add("CphaseRadio", Convert.ToString(ins.WjVoltage.CphaseRadio));

                dicDesc.Add("SwitchOutCount", Convert.ToString(ins.SwitchOutCount));

                int index = 0;

                foreach (var t in ins.WjSwitchOuts)
                {
                    dicDesc.Add("SwitchId" + t.Key, Convert.ToString(t.Value.SwitchId));
                    dicDesc.Add("SwitchName" + t.Key, Convert.ToString(t.Value.SwitchName));
                    dicDesc.Add("SwitchVecotr" + t.Key, Convert.ToString(t.Value.SwitchVecotr));
                    dicDesc.Add("LoopSum" + t.Key, Convert.ToString(RtuParaSwitchOutViewModels[index++].LoopSum));
                }

                dicDesc.Add("SumSwitchInLoops", Convert.ToString(ins.WjLoops.Count));

                foreach (var t in ins.WjLoops)
                {
                    dicDesc.Add("CurrentAlarmLowerlimit" + t.Key, Convert.ToString(t.Value.CurrentAlarmLowerlimit));
                    dicDesc.Add("CurrentAlarmUpperlimit" + t.Key, Convert.ToString(t.Value.CurrentAlarmUpperlimit));
                    dicDesc.Add("CurrentRange" + t.Key, Convert.ToString(t.Value.CurrentRange));
                    dicDesc.Add("IsAlarmHop" + t.Key, Convert.ToString(t.Value.IsAlarmHop));
                    dicDesc.Add("IsShieldLoop" + t.Key, Convert.ToString(t.Value.IsShieldLoop));
                    dicDesc.Add("IsSwitchStateClose" + t.Key, Convert.ToString(t.Value.IsSwitchStateClose));
                    dicDesc.Add("LoopId" + t.Key, Convert.ToString(t.Value.LoopId));
                    dicDesc.Add("MutualInductorRatio" + t.Key, Convert.ToString(t.Value.MutualInductorRatio));
                    dicDesc.Add("ShieldLittleA" + t.Key, Convert.ToString(t.Value.ShieldLittleA));
                    dicDesc.Add("SwitchOutputId" + t.Key, Convert.ToString(t.Value.SwitchOutputId));
                    dicDesc.Add("VectorMoniliang" + t.Key, Convert.ToString(t.Value.VectorMoniliang));
                    dicDesc.Add("VectorSwitchIn" + t.Key, Convert.ToString(t.Value.VectorSwitchIn));
                    dicDesc.Add("VoltagePhaseCode" + t.Key, Convert.ToString(Convert.ToInt32(t.Value.VoltagePhaseCode)));
                }



                string dir = Directory.GetCurrentDirectory() + "\\SystemXmlConfig\\WJ3005Para";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                string extension = "xml";
                SaveFileDialog dialog = new SaveFileDialog()
                {
                    InitialDirectory = dir,
                    DefaultExt = extension,
                    Filter =
                        String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*",
                                      extension,
                                      "Xml File"),
                    FilterIndex = 1
                };
                if (dialog.ShowDialog() == true)
                {

                    Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(dicDesc, "WJ3005Para\\" + dialog.SafeFileName);
                }






            }
        }

        #endregion

        #region ReadParaFromXml
        private RelayCommand _readParaFromXml;

        /// <summary>
        /// 读取预存参数
        /// </summary>
        public ICommand ReadParaFromXml
        {
            get { return _readParaFromXml ?? (_readParaFromXml = new RelayCommand(ExReadParaFromXml, CanReadParaFromXml, true)); }
        }

        private bool CanReadParaFromXml()
        {
            return true;
        }

        private void ExReadParaFromXml()
        {
            Read_Para_from_xml();
        }


        #endregion

        #region tab
        public int Index
        {
            get { return 1; }
        }
        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public string Title
        {
            get { return "终端参数设置"; }
        }

        #endregion


        #region 批量操作
        private bool _isChecked;

        /// <summary>
        /// 是否选中该条数据
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    this.RaisePropertyChanged(() => this.IsChecked);
                }
            }
        }
        void InitEvent()
        {
            this.AddEventFilterInfo(20161012,
                                     "Wlst.Ux.WJ3005Module.Wj3005TmlInfoSetViewModel.ViewModel.RtuLoopInfoVm",true);
        }

        //private int LightOpenCloseProtect = 5;

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            //base.ExPublishedEvent(args);
            if (!IsChecked) return;

            if (args.GetParams().Count < 2) return;
            int obj1 = 10;
            try
            {
                obj1 = Convert.ToInt32(args.GetParams()[0]);
            }
            catch (Exception)
            {
            }

            var obj2 = args.GetParams()[1] as RtuLoopInfoVm;
            if (obj2 == null) return;

            switch (obj1)
            {
                case 0:
                    if (obj2.RtuId == RtuId )
                    {
                        foreach (var t in RtuParaAnalogueAmpViewModels)
                        {
                            t.IsSwitchStateCloseSelectIndex = obj2.IsSwitchStateCloseSelectIndex;
                        }
                    }
                    break;
                case 1:
                    if (obj2.RtuId == RtuId)
                    {
                        foreach (var t in RtuParaAnalogueAmpViewModels)
                        {
                            t.IsAlarmSwitchSelectIndex = obj2.IsAlarmSwitchSelectIndex;
                        }
                    }
                    break;
                case 2:
                    if (obj2.RtuId == RtuId)
                    {
                        foreach (var t in RtuParaAnalogueAmpViewModels)
                        {
                            if (t.IsEnable == true)t.VectorMoniliang = obj2.VectorMoniliang;
                        }
                    }
                    break;
                case 3:
                    if (obj2.RtuId == RtuId)
                    {
                        foreach (var t in RtuParaAnalogueAmpViewModels)
                        {
                            if (t.IsEnable == true) t.AmRange = obj2.AmRange;
                        }
                    }
                    break;
                case 4:
                    if (obj2.RtuId == RtuId)
                    {
                        foreach (var t in RtuParaAnalogueAmpViewModels)
                        {
                            if (t.IsEnable == true) t.UpperLimit = obj2.UpperLimit;
                        }
                    }
                    break;
                case 5:
                    if (obj2.RtuId == RtuId)
                    {
                        foreach (var t in RtuParaAnalogueAmpViewModels)
                        {
                            if (t.IsEnable == true) t.LowerLimit = obj2.LowerLimit;
                        }
                    }
                    break;
                case 6:
                    if (obj2.RtuId == RtuId)
                    {
                        foreach (var t in RtuParaAnalogueAmpViewModels)
                        {
                            if (t.IsEnable == true) t.SelectPhaseIndex = obj2.SelectPhaseIndex;
                        }
                    }
                    break;
                case 7:
                    if (obj2.RtuId == RtuId)
                    {
                        foreach (var t in RtuParaAnalogueAmpViewModels)
                        {
                            if (t.IsEnable == true) t.Range = obj2.Range;
                        }
                    }
                    break;
                case 8:
                    if (obj2.RtuId == RtuId)
                    {
                        foreach (var t in RtuParaAnalogueAmpViewModels)
                        {
                            if (t.IsEnable == true) t.IsShieldLittleA = obj2.IsShieldLittleA;
                        }
                    }
                    break;
                case 9:
                    if (obj2.RtuId == RtuId)
                    {
                        foreach (var t in RtuParaAnalogueAmpViewModels)
                        {
                            if (t.IsEnable == true) t.IsShieldLoopSelectIndex = obj2.IsShieldLoopSelectIndex;
                        }
                    }
                    break;
                default:
                    break;

            }
        }
        #endregion



    }






    /// <summary>
    /// Action
    /// </summary>
    public partial class TmlInformationViewModel
    {
        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders,// .wlst_cnt_wj3090_order_snd_paras ,//.ClientPart.wlst_rtuargsupdate_server_ans_clinet_order_paras4000,
                RtuParaUpdate40000,
                typeof(TmlInformationViewModel), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_zc_rtu_info,// .wlst_cnt_wj3090_order_snd_paras ,//.ClientPart.wlst_rtuargsupdate_server_ans_clinet_order_paras4000,
                RtuParaUpdate40001,
                typeof(TmlInformationViewModel), this);
        }

        private bool isFirstAllYear = false;
        public void RtuParaUpdate40000(string session, Wlst.mobile.MsgWithMobile args)
        {
            if (_isViewShow == false) return;
            var datax = args.WstRtuOrders;
            if (datax.RtuIds.Count == 0) return;
            if (datax.RtuIds[0] != RtuId) return;
            // lvf 2018年8月13日09:47:56   收到停运启运后,直接更新缓存.
            if (datax.Op == 6 || datax.Op == 7)
            {
                if (datax.RtuIds.Count == 0) return;
                var rtuid = datax.RtuIds[0];
                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuid) == false)
                    return;
                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid].RtuRealState = datax.Op ==
                                                                                                            6
                                                                                                                ? 1
                                                                                                                : 2;
            }
            // lvf 2018年8月24日11:16:52 更改3006设备型号 直接更新
            if (datax.Op == 81)
            {
                if (datax.RtuIds.Count == 0) return;
                var rtuid = datax.RtuIds[0];
                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuid) == false)
                    return;
                if (datax.Date == "3006")
                {
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid].RtuModel =
                        EnumRtuModel.Wj3006;
                }
            }

            if (datax.Op == 22 || datax.Op == 61 || datax.Op == 8)
                this.OnDataBack(datax.RtuIds[0], datax.Op, datax.Date);
            else this.OnDataBack(datax.RtuIds[0], datax.Op, "");

            //周设置应答  lvf 2018年8月31日13:43:42
            if (datax.Op == 11 || datax.Op == 12 || datax.Op == 13 )
            {
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + datax.RtuIds[0] + " 周设置   应答;" +
                             Environment.NewLine + ShowInfo;

               
                if ( datax.Op ==13)
                {

                    ShowInfos.Add(new ShowInfo()
                    {
                        Content = "----",
                        IndexOfType = 7,
                        IsBack = false,
                        Title = "正在发送周设置，第3条，共3条"
                    });
                 
                }
                UpdateXg(7);
            }
            //全年时间表 应答 lvf 2018年8月31日14:05:11
            if(datax.Op ==14)
            {
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + datax.RtuIds[0] + " 周设置   应答;" +
                             Environment.NewLine + ShowInfo;
                if (isFirstAllYear == false)
                {
                    isFirstAllYear = true;
                    UpdateSXg(8);
                }

                UpdateXg(8);
            }

            //if (datax.Op == 1) PublishEvent(datax.RtuIds, Services.EventIdAssign.RtuPara4000Id);
            //if (datax.Op == 2) PublishEvent(datax.RtuIds, Services.EventIdAssign.RtuPara4100Id);
            //if (datax.Op == 3) PublishEvent(datax.RtuIds, Services.EventIdAssign.RtuPara4200Id);
            //if (datax.Op == 4) PublishEvent(datax.RtuIds, Services.EventIdAssign.RtuPara4400Id);
            //if (datax.Op == 5) PublishEvent(datax.RtuIds, Services.EventIdAssign.RtuPara6100Id);
            //if (datax.Op == 7) PublishEvent(datax.RtuIds, Services.EventIdAssign.RtuReStrartRunId);
            //if (datax.Op == 6) PublishEvent(datax.RtuIds, Services.EventIdAssign.RtuStopRunId);

        }

        public void RtuParaUpdate40001(string session, Wlst.mobile.MsgWithMobile args)
        {

            //lvf  2018年11月12日11:05:17   改进，原本只有一个召测软件版本，写死了，现在 添加了 硬件版本&设备位置
            if (_isViewShow == false) return;

            var datax = args.WstRtuZcInfo;
            if (datax.RtuId == 0) return;
            //this.OnDataBack(datax.RtuId, 10000,datax.Version);
            if (datax.Op == 33)
            {
                if (datax.Version == null)
                {
                    ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + datax.RtuId + " 召测软件版本失败."+
                           Environment.NewLine + ShowInfo;
                    return;
                }
               ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + datax.RtuId + " 软件版本:" + datax.Version +
                           Environment.NewLine + ShowInfo;

            }
            if (datax.Op == 34)
            {
                if (datax.RtuVersion == null)
                {
                    ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + datax.RtuId + " 召测硬件版本失败." +
                           Environment.NewLine + ShowInfo;
                    return;
                }
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + datax.RtuId + " 硬件版本:" + datax.RtuVersion.HardwareVer +
                            Environment.NewLine + ShowInfo;

            }
            if (datax.Op == 40)
            {
                if (datax.RtuAssistInfo == null)
                {
                    ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + datax.RtuId + " 召测经纬度等失败." +
                           Environment.NewLine + ShowInfo;
                    return;
                }
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + datax.RtuId + " 设备位置:" +
                           datax.RtuAssistInfo.Longitude + "," + datax.RtuAssistInfo.Latitude +
                           Environment.NewLine + ShowInfo;
                //ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + datax.RtuId + " 温度:" +
                //           datax.RtuAssistInfo.Temperature + ";湿度:" + datax.RtuAssistInfo.Humidity + ";电量:" +
                //           datax.RtuAssistInfo.BatteryPower + ";主电源电压 :" +datax.RtuAssistInfo.MainVoltage+ 
                //           Environment.NewLine + ShowInfo;

                Xmap = datax.RtuAssistInfo.Longitude;
                Ymap = datax.RtuAssistInfo.Latitude;

                if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(datax.RtuId) == false)
                    return;
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[datax.RtuId].RtuMapX = Xmap;
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[datax.RtuId].RtuMapY = Ymap;
                SubmitExecute();
            }


        }


        #region Servces


        /// <summary>
        /// 工作参数
        /// </summary>
        public void Snd4000(int rtuId)
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;// .wlst_cnt_wj3090_order_snd_paras ;//.ServerPart.wlst_rtuargsupdate_clinet_order_paras4000;
            info.WstRtuOrders.Op = 1;// OrderUpdateRtuParas.OrderUpdateRtuParasItem.Para4000;
            info.WstRtuOrders.RtuIds.Add(rtuId);
            Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("6100 " + info.Head.Gid);

            SndOrderServer.OrderSnd(info, 10, 6);
        }


        /// <summary>
        /// 矢量
        /// </summary>
        public void Snd4100(int rtuId)
        {
            //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //var lst = new List<int>() {rtuId};
            //SndOrderServer.OrderSnd(Services.EventIdAssign.RtuPara4100Id, lst,
            //                        null, waitIdUpdate, 10, 6);

            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.ServerPart.wlst_rtuargsupdate_clinet_order_paras4000;
            info.WstRtuOrders.Op = 2;// OrderUpdateRtuParas.OrderUpdateRtuParasItem.Para4100;
            info.WstRtuOrders.RtuIds.Add(rtuId);
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        /// <summary>
        /// 模拟量
        /// </summary>
        public void Snd4200(int rtuId)
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.ServerPart.wlst_rtuargsupdate_clinet_order_paras4000;
            info.WstRtuOrders.Op = 3;// OrderUpdateRtuParas.OrderUpdateRtuParasItem.Para4200;
            info.WstRtuOrders.RtuIds.Add(rtuId);
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        /// <summary>
        /// 上下限
        /// </summary>
        public void Snd4400(int rtuId)
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.ServerPart.wlst_rtuargsupdate_clinet_order_paras4000;
            info.WstRtuOrders.Op = 4;// OrderUpdateRtuParas.OrderUpdateRtuParasItem.Para4400;
            info.WstRtuOrders.RtuIds.Add(rtuId);
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        /// <summary>
        /// 电压
        /// </summary>
        public void Snd6100(int rtuId)
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.ServerPart.wlst_rtuargsupdate_clinet_order_paras4000;
            info.WstRtuOrders.Op = 5;// OrderUpdateRtuParas.OrderUpdateRtuParasItem.Para6100;
            info.WstRtuOrders.RtuIds.Add(rtuId);
            SndOrderServer.OrderSnd(info, 10, 6);
        }


        public void SndStopRtu(int rtuId)
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.ServerPart.wlst_rtuargsupdate_clinet_order_paras4000;
            info.WstRtuOrders.Op = 6;// OrderUpdateRtuParas.OrderUpdateRtuParasItem.StopRunning;
            info.WstRtuOrders.RtuIds.Add(rtuId);
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        public void SndReStrartRtu(int rtuId)
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.ServerPart.wlst_rtuargsupdate_clinet_order_paras4000;
            info.WstRtuOrders.Op = 7;// OrderUpdateRtuParas.OrderUpdateRtuParasItem.StartRunning;
            info.WstRtuOrders.RtuIds.Add(rtuId);
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        public void SndZcVer(int rtuId)
        {
            var nt = Wlst.Sr.ProtocolPhone.LxRtu.wst_zc_rtu_info;
            nt.Args.Addr.Add(rtuId);
            nt.WstRtuZcInfo.Op = 33;
            //nt.WstRtuZcInfo.RtuId = rtuId;
            SndOrderServer.OrderSnd(nt);
        }

        //lvf 2018年11月12日10:44:11   召测硬件版本
        public void SndZcHardwareVer(int rtuId)
        {
            var nt = Wlst.Sr.ProtocolPhone.LxRtu.wst_zc_rtu_info;
            nt.Args.Addr.Add(rtuId);
            nt.WstRtuZcInfo.Op = 34;
            //nt.WstRtuZcInfo.RtuId = rtuId;
            SndOrderServer.OrderSnd(nt);
        }


        //lvf 2018年11月12日10:44:11   读取设备经纬度
        public void SndReadMapXy(int rtuId)
        {
            var nt = Wlst.Sr.ProtocolPhone.LxRtu.wst_zc_rtu_info;
            nt.Args.Addr.Add(rtuId);
            nt.WstRtuZcInfo.Op = 40;
            //nt.WstRtuZcInfo.RtuId = rtuId;
            SndOrderServer.OrderSnd(nt);
        }


        public void SndZcTime(int rtuId)
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;
            info.Args.Addr.Add(rtuId);
            info.WstRtuOrders.Op = 22;
            info.WstRtuOrders.RtuIds.Add(rtuId);
            SndOrderServer.OrderSnd(info);
        }

        public void SndZcSj(int rtuId, DateTime dtstart, DateTime dtend)
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;
            info.Args.Addr.Add(rtuId);
            info.WstRtuOrders.Op = 61;
            info.WstRtuOrders.RtuIds.Add(rtuId);
            info.WstRtuOrders.DtTime = dtstart.Ticks;
            info.WstRtuOrders.Date = dtend.Ticks.ToString("");
            SndOrderServer.OrderSnd(info);
        }

        public void SndFwTml(int rtuId)
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;
            info.Args.Addr.Add(rtuId);
            info.WstRtuOrders.Op = 51;
            info.WstRtuOrders.RtuIds.Add(rtuId);
            SndOrderServer.OrderSnd(info);
        }

        public void SndFwNew(int rtuId)
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;
            info.Args.Addr.Add(rtuId);
            info.WstRtuOrders.Op = 52;
            info.WstRtuOrders.RtuIds.Add(rtuId);
            SndOrderServer.OrderSnd(info);
        }

        public void SndFwMK(int rtuId)
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;
            info.Args.Addr.Add(rtuId);
            info.WstRtuOrders.Op = 53;
            info.WstRtuOrders.RtuIds.Add(rtuId);
            SndOrderServer.OrderSnd(info);
        }
        #endregion
    }

    /// <summary>
    /// 基本数据
    /// </summary>
    public partial class TmlInformationViewModel
    {
        private void SetTerminalInformationVm(Wlst .Sr .EquipmentInfoHolding .Model .Wj3005Rtu  info)
        {
            this.Alarm = info.WjGprs .IsAlarm ;
            this.Boot = info.WjGprs .IsBoot ;
            this.Call = info.WjGprs .IsCall ;
            // this.CommType = info.CommType;
            this.Display = info.WjGprs .IsDisplay ;
            this.ErrorDelays = info.WjGprs .RtuAlarmDelay ;
            this.HeartBeatPeriod = info.WjGprs .RtuHeartbeatCycle ;
            if (info.WjVoltage.RtuUsedType < 1 || info.WjVoltage.RtuUsedType > 6) 
            {
              this.intRtuType = 0;
            }
            else
            {
                 this.intRtuType = info.WjVoltage.RtuUsedType -1;
            }

            //终端地区  lvf 2019年5月5日16:30:19
            this.intRtuRegion = info.RtuRegion;

        
            this.LowerLimit = info.WjVoltage .VoltageAlarmLowerlimit  ;
            try
            {      var ipsdd =
                        new System.Net.IPAddress(BitConverter.GetBytes( info.WjGprs .StaticIp )).ToString();
                   // var xx = BitConverter.ToUInt32(System.Net.IPAddress.Parse(ipsdd).GetAddressBytes(), 0);


              this.Ip = ipsdd;
                this.UpdateTime = string.Format("{0:G}", new DateTime( info.DateUpdate ));
            }
            catch (Exception ex)
            {
            }
            this.PhyId = info.RtuPhyId ;
            this.Port = info.WjGprs.RtuCommPort ;
          //  this.Priority = info.WjGprs.;
            this.Range = info.WjVoltage .VoltageRange ;
            this.Report = info.WjGprs .IsReport  ;
            this.ReportDataPeriod = info.WjGprs .RtuReportCycle ;
            this.Route = info.WjGprs .IsRoute ;
            this.RtuId = info.RtuId;
            this.RtuModel = (int )info.RtuModel;
            this.RtuName = info.RtuName;
            this.RtuState = info.RtuStateCode ;
            this.RtuRealState = info.RtuRealState;
            if (RtuRealState == 0) RtuRealState = 3;
            if (RtuState == 0) RtuState = 3;
            this.Selfcheck = info.WjGprs.IsSelfcheck;// Selfcheck;
            //this.ServiceProvider = info.WjGprs.ServiceProvider;
            this.SimNumber = info.WjGprs.MobileNo;// SimNumber;
            this.Sound = info.WjGprs.IsSound;// Sound;
            this.UpperLimit = info.WjVoltage.VoltageAlarmUpperlimit;// UpperLimit;
            //this.Xgis = info.Xgis;
            this.Xmap = info.RtuMapX ;//.Xmap;
            //this.Ygis = info.Ygis;

            this.Aoverload = info.WjVoltage.Aoverload;
            this.AoverAlittle = info.WjVoltage.AoverAlittle;

            this.IsHasElec = info.WjVoltage.IsHasElec;
            this.AphaseRadio = info.WjVoltage.AphaseRadio;
            this.BphaseRadio = info.WjVoltage.BphaseRadio;
            this.CphaseRadio = info.WjVoltage.CphaseRadio;

            this.Ymap = info.RtuMapY ;
            this.Remark = info.RtuRemark ;
            this.InstallAddr = info.RtuInstallAddr ;
            this.IsSwitchinputJudgebyA = info.WjVoltage .IsSwitchinputJudgebyA;
            //this.IsShieldLittleA = info.WjVoltage .IsShieldLittleA;
            //this.AShield = info.WjVoltage .AShield;
            this.RtuIdf = info.Idf;
            //地区解析  lvf 2019年5月6日09:14:34
            this.intRtuRegion = info.RtuRegion;


            try
            {
                this.DataCreate = new DateTime(info.DateCreate );
            }
            catch (Exception ex)
            {
            }
        }

        //private Wj3005TerminalInformation BackVmToTerminalInfomation(Wj3005TerminalInformation info)
        //{
        //    info.Alarm = this.Alarm;
        //    info.Boot = this.Boot;
        //    info.Call = this.Call;
        //    //  info.CommType = this.CommType;
        //    info.Display = this.Display;
        //    info.ErrorDelays = this.ErrorDelays;
        //    info.HeartBeatPeriod = this.HeartBeatPeriod;
        //    info.Ip = this.Ip;
        //    info.LowerLimit = this.LowerLimit;
        //    info.Md5 = Convert.ToDateTime(UpdateTime).Ticks;
        //    info.PhyId = this.PhyId;
        //    info.Port = this.Port;
        //    info.Priority = this.Priority;
        //    info.Range = this.Range;
        //    info.Report = this.Report;
        //    info.ReportDataPeriod = this.ReportDataPeriod;
        //    info.Route = this.Route;
        //    info.RtuId = this.RtuId;
        //    info.RtuModel = this.RtuModel;
        //    info.RtuName = this.RtuName;
        //    info.RtuState = this.RtuState;
        //    if (info.RtuState == 3) info.RtuState = 0;

        //    info.RtuVoltageName = this.RtuVoltageName;
        //    info.Selfcheck = this.Selfcheck;
        //    info.ServiceProvider = this.ServiceProvider;
        //    info.SimNumber = this.SimNumber;
        //    info.Sound = this.Sound;
        //    info.UpperLimit = this.UpperLimit;
        //    //info.Xgis = this.Xgis;
        //    info.Xmap = this.Xmap;
        //    //info.Ygis = this.Ygis;
        //    info.Ymap = this.Ymap;
        //    info.Remark = this.Remark;
        //    info.InstallAddr = this.InstallAddr;
        //    info.DataCreate = this.DataCreate.Ticks;
        //    info.IsSwitchinputJudgebyA = this.IsSwitchinputJudgebyA;
        //    info.IsShieldLittleA = this.IsShieldLittleA;
        //    info.AShield = this.AShield;
        //    return info;
        //}


     

        #region priave attri

        private int _value1;
        private int _value2;
        private int _value3;
        private string _value4;
        private int _value5;
        private int _value51;
        private int _value6;
        private double _value7;
        private double _value8;
        private double _value9;
        private double _value10;

        private string _value11;
        private string _value12;
        private string _value13;
        private int _value14;
        private string _value15;
        private int _value16;
        private int _value17;
        private int _value18;
        private bool _value19;
        private bool _value20;

        private bool _value21;
        private bool _value22;
        private bool _value23;
        private bool _value24;
        private bool _value25;
        private bool _value26;
        private int  _value27;
        private string _value28;
        private int _value29;
        private int _value30;

        private int _value31;
        private string _value32;
        private string _value33;
        private DateTime _value34;

        private string _value35;
        #endregion

        #region 基本参数

        /// <summary>
        /// 终端地址  
        /// </summary>
        public int RtuId
        {
            get { return _value1; }
            set
            {
                if (value != _value1)
                {
                    _value1 = value;
                    this.RaisePropertyChanged(() => this.RtuId);
                }
            }
        }


        /// <summary>
        /// 终端识别码  
        /// </summary>
        public string RtuIdf
        {
            get { return _value35; }
            set
            {
                if (value != _value35)
                {
                    _value35 = value;
                    this.RaisePropertyChanged(() => this.RtuIdf);
                }
            }
        }


        /// <summary>
        /// 终端物理地址 默认与终端地址相同
        /// </summary>

         [Range(1.0, 10000.99, ErrorMessage = "物理地址介于1到10000")]
        public int PhyId
        {
            get { return _value2; }
            set
            {
                if (value != _value2)
                {
                    _value2 = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }

        /// <summary>
        /// 终端通信方式  1电台  6  Gprs/Cmda
        /// </summary>
        public int CommType
        {
            get { return _value3; }
            set
            {
                if (value != _value3)
                {
                    _value3 = value;
                    this.RaisePropertyChanged(() => this.CommType);
                }
            }
        }

        /// <summary>
        /// 终端名称
        /// </summary>

        //[StringLength(30, ErrorMessage = "名称长度不能大于30")]
        [Required(ErrorMessage = "输入不得为空")]
        public string RtuName
        {
            get { return _value4; }
            set
            {
                if (value != _value4)
                {
                    _value4 = value;

                    if (_value4.Contains(" ") && _value4.Contains("*"))
                    {
                        int a = _value4.IndexOf(" ");
                        int b = _value4.IndexOf("*");
                        if(a<b)
                        {
                            string tmp = _value4.Substring(a+1, b - a -1);
                            RtuIdf = tmp;
                        }

                    }

                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        /// <summary>
        /// 终端工作状态
        /// 1-停运，2-使用, 3-不用， 显示时将不用 0 转换为了3
        /// </summary>
        public int RtuState
        {
            get { return _value5; }
            set
            {
                if (value != _value5)
                {
                    _value5 = value;
                    this.RaisePropertyChanged(() => this.RtuState);
                }
            }
        }

        /// <summary>
        /// 终端实际工作状态
        /// 1-停运，2-使用, 3-不用， 显示时将不用 0 转换为了3
        /// </summary>
        public int RtuRealState
        {
            get { return _value51; }
            set
            {
                if (value != _value51)
                {
                    _value51 = value;
                    this.RaisePropertyChanged(() => this.RtuRealState);
                }
            }
        }




        /// <summary>
        /// 终端型号 默认3005
        /// </summary>
        public int RtuModel
        {
            get { return _value6; }
            set
            {
                if (value != _value6)
                {
                    _value6 = value;
                    this.RaisePropertyChanged(() => this.RtuModel);
                }
            }
        }

        #region 终端类型

        private int _intRtuType;

        public int intRtuType
        {
            get { return _intRtuType; }
            set
            {
                if (value != _intRtuType)
                {
                    _intRtuType = value;
                    RaisePropertyChanged(() => intRtuType);
                }
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _rtuType;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> RtuType
        {
            get
            {
                if (_rtuType == null)
                {
                    _rtuType = new ObservableCollection<NameValueInt>();
                    //for (int i = 1; i < 24; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() {Name = i + "小时", Value = i});
                    //}
                    //for (int i = 1; i < 94; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() {Name = i + " 天", Value = i*24});
                    //}

                    LoadRtuTypeXml();

                    if (Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType.Count!=0)
                    {
                        foreach (var g in Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType)
                        {
                            _rtuType.Add(new NameValueInt() {Name = g.Value.Item1, Value = g.Key});
                        }
                        return _rtuType;
                    }


                    _rtuType.Add(new NameValueInt() { Name = "路灯", Value = 1 });
                    _rtuType.Add(new NameValueInt() { Name = "亮化", Value = 2 });
                    _rtuType.Add(new NameValueInt() { Name = "广告", Value = 3 });
                    _rtuType.Add(new NameValueInt() { Name = "其他", Value = 4 });
                    //隧道 lvf 2019年1月10日16:35:14
                    _rtuType.Add(new NameValueInt() { Name = "隧道", Value = 100 });


                }
                return _rtuType;
            }
        }



        #endregion

        #region 终端地区

        private int _intRtuRegion;

        public int intRtuRegion
        {
            get { return _intRtuRegion; }
            set
            {
                if (value != _intRtuRegion)
                {
                    _intRtuRegion = value;
                    RaisePropertyChanged(() => intRtuRegion);
                }
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _rtuRegion;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> RtuRegion
        {
            get
            {
                if (_rtuRegion == null)
                {
                    _rtuRegion = new ObservableCollection<NameValueInt>();
                    //for (int i = 1; i < 24; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() {Name = i + "小时", Value = i});
                    //}
                    //for (int i = 1; i < 94; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() {Name = i + " 天", Value = i*24});
                    //}


                   if (Wlst.Sr.EquipmentInfoHolding.Services.Others.RegionItems.Count != 0)
                    {
                        this.RtuRegion.Add(new NameValueInt() { Name = "无", Value = 0 });
                        foreach (var g in Wlst.Sr.EquipmentInfoHolding.Services.Others.RegionItems)
                        {
                            _rtuRegion.Add(new NameValueInt() { Name = g.Item2, Value = g.Item1 });
                        }
                        return _rtuRegion;
                    }


                    _rtuRegion.Add(new NameValueInt() { Name = "无", Value =0 });
                   


                }
                return _rtuRegion;
            }
        }



        #endregion


        #region  海门特殊功能 显示 门控输出  todo
        private Visibility _isShowLineHM;

        public Visibility IsShowLineHM
        {
            get { return _isShowLineHM; }
            set
            {
                if (value != _isShowLineHM)
                {
                    _isShowLineHM = value;
                    RaisePropertyChanged(() => IsShowLineHM);
                }
            }
        }
        #endregion


        #region  动态备注  lvf http 请求 ，本地配置文件决定名称  2019年1月15日09:31:14



        private ObservableCollection<Visibility> _rtuRemarkVisi;
        public ObservableCollection<Visibility> RtuRemarkVisi
        {
            get
            {
                if (_rtuRemarkVisi == null)
                {
                    _rtuRemarkVisi = new ObservableCollection<Visibility>();
                    for (int i = 1; i < 8; i++)
                    {
                        _rtuRemarkVisi.Add(Visibility.Collapsed);
                    }
                }
                return _rtuRemarkVisi;
            }
        }


        private ObservableCollection<NameValueInt> _rtuRemarkName;
        public ObservableCollection<NameValueInt> RtuRemarkName
        {
            get
            {
                if (_rtuRemarkName == null)
                {
                    _rtuRemarkName = new ObservableCollection<NameValueInt>();

                    LoadRtuRemarkXml();
                    if (Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuRemarks.Count != 0)
                    {
                        foreach (var g in Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuRemarks)
                        {
                            _rtuRemarkName.Add(new NameValueInt() { Name = g.Value+":", Value = g.Key });
                            
                            RtuRemarkVisi[g.Key-1] = Visibility.Visible;
                            
                        }
                        return _rtuRemarkName;
                    }

                }
                return _rtuRemarkName;
            }
        }


        private ObservableCollection<string> _rtuRemark;
        /// <summary>
        /// 设备备注信息
        /// </summary>
        public ObservableCollection<string> RtuRemarks
        {
            get
            {
                if (_rtuRemark ==null)
                {
                    _rtuRemark = new ObservableCollection<string>();
                    for (int i = 1; i < 8; i++)
                    {
                        _rtuRemark.Add("");
                    }
                }
                return _rtuRemark;
            }
            set
            {
                if (value != _rtuRemark)
                {
                    _rtuRemark = value;
                    this.RaisePropertyChanged(() => this.RtuRemarks);
                }
            }
        }

        #endregion



        #region 终端更改型号

        private int _intRtuModelModify;

        public int IntRtuModelModify
        {
            get { return _intRtuModelModify; }
            set
            {
                if (value != _intRtuModelModify)
                {
                    _intRtuModelModify = value;
                    RaisePropertyChanged(() => IntRtuModelModify);
                }
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _rtuModelModify;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> RtuModelModify
        {
            get
            {
                if (_rtuModelModify == null)
                {
                    _rtuModelModify = new ObservableCollection<NameValueInt>();
                    _rtuModelModify.Add(new NameValueInt() { Name = "3090型", Value = 3090 });
                    _rtuModelModify.Add(new NameValueInt() { Name = "3005型", Value = 3005 });
                    _rtuModelModify.Add(new NameValueInt() { Name = "3006型", Value = 3006 });
                    _rtuModelModify.Add(new NameValueInt() { Name = "6005型", Value = 6005 });
                }
                return _rtuModelModify;
            }
        }

        #endregion


        /// <summary>
        /// 地图X坐标 仅JPG
        /// </summary>
        public double Xmap
        {
            get { return _value7; }
            set
            {
                if (value != _value7)
                {
                    _value7 = value;
                    this.RaisePropertyChanged(() => this.Xmap);
                }
            }
        }

        /// <summary>
        /// 地图Y坐标仅JPG
        /// </summary>
        public double Ymap
        {
            get { return _value8; }
            set
            {
                if (value != _value8)
                {
                    _value8 = value;
                    this.RaisePropertyChanged(() => this.Ymap);
                }
            }
        }

        ///// <summary>
        ///// 地图X坐标 GIS以及其他矢量地图
        ///// </summary>
        //public double Xgis
        //{
        //    get { return _value9; }
        //    set
        //    {
        //        if (value != _value9)
        //        {
        //            _value9 = value;
        //            this.RaisePropertyChanged(() => this.Xgis);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 地图Y坐标 GIS以及其他矢量地图
        ///// </summary>
        //public double Ygis
        //{
        //    get { return _value10; }
        //    set
        //    {
        //        if (value != _value10)
        //        {
        //            _value10 = value;
        //            this.RaisePropertyChanged(() => this.Ygis);
        //        }
        //    }
        //}



        public string UpdateTime
        {
            get { return _value11; }
            set
            {
                if (value != _value11)
                {
                    _value11 = value;
                    this.RaisePropertyChanged(() => this.UpdateTime);
                }
            }
        }


        /// <summary>
        /// 设备安装位置
        /// </summary>
        public virtual string InstallAddr
        {
            get { return _value32; }
            set
            {
                if (value != _value32)
                {
                    _value32 = value;
                    this.RaisePropertyChanged(() => this.InstallAddr);
                }
            }
        }

        /// <summary>
        /// 设备备注信息
        /// </summary>
        public virtual string Remark
        {
            get { return _value33; }
            set
            {
                if (value != _value33)
                {
                    _value33 = value;
                    this.RaisePropertyChanged(() => this.Remark);
                }
            }
        }

        private string _RemarkName;
        /// <summary>
        /// 设备备注名称
        /// </summary>
        public virtual string RemarkName
        {
            get { return _RemarkName; }
            set
            {
                if (value != _RemarkName)
                {
                    _RemarkName = value;
                    this.RaisePropertyChanged(() => this.RemarkName);
                }
            }
        }

        /// <summary>
        /// 设备开通日期
        /// </summary>
        public virtual DateTime DataCreate
        {
            get { return _value34; }
            set
            {
                if (value != _value34)
                {
                    _value34 = value;
                    this.RaisePropertyChanged(() => this.DataCreate);
                }
            }
        }


        #endregion

        #region IIRtuParaGprs

        ///// <summary>
        ///// 通讯参数 通讯服务商
        ///// </summary>
        //public string ServiceProvider
        //{
        //    get { return _value12; }
        //    set
        //    {
        //        if (value != _value12)
        //        {
        //            _value12 = value;
        //            this.RaisePropertyChanged(() => this.ServiceProvider);
        //        }
        //    }
        //}

        /// <summary>
        /// 通讯参数 静态IP地址
        /// </summary>
        public string Ip
        {
            get { return _value13; }
            set
            {
                if (value != _value13)
                {


                    if (System.Text.RegularExpressions.Regex.IsMatch(value,
                                                                     @"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"))
                        //[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}
                    {
                        string[] ips = value.Split('.');
                        if (ips.Length == 4 || ips.Length == 6)
                        {
                            if (System.Int32.Parse(ips[0]) < 256 &&
                                System.Int32.Parse(ips[1]) < 256 & System.Int32.Parse(ips[2]) < 256 &
                                System.Int32.Parse(ips[3]) < 256 && System.Int32.Parse(ips[0]) >= 0)
                                value = System.Int32.Parse(ips[0]) + "." + System.Int32.Parse(ips[1]) + "." +
                                        System.Int32.Parse(ips[2]) + "." + System.Int32.Parse(ips[3]);
                            else
                                return;
                        }
                        else
                            return;

                    }
                    else return;


                    _value13 = value;
                    this.RaisePropertyChanged(() => this.Ip);
                }
            }
        }

        /// <summary>
        /// 通讯参数 端口号
        /// </summary>
        public int Port
        {
            get { return _value14; }
            set
            {
                if (value != _value14)
                {
                    if (value < 1023) return;
                    if (value > 65535) return;
                    _value14 = value;
                    this.RaisePropertyChanged(() => this.Port);
                }
            }
        }

        /// <summary>
        /// 通讯参数 手机号
        /// </summary>
        ///[StringLength(11, ErrorMessage="手机号码应有11个数字")]
        public string SimNumber
        {
            get { return _value15; }
            set
            {
                if (value != _value15)
                {
                    _value15 = value;
                    this.RaisePropertyChanged(() => this.SimNumber);
                }
            }
        }

        /// <summary>
        /// 通讯参数 心跳周期
        /// </summary>
        public int HeartBeatPeriod
        {
            get { return _value16; }
            set
            {
                if (value != _value16)
                {
                    _value16 = value;
                    this.RaisePropertyChanged(() => this.HeartBeatPeriod);
                }
            }
        }

        /// <summary>
        /// 通讯参数 主报周期
        /// </summary>
        public int ReportDataPeriod
        {
            get { return _value17; }
            set
            {
                if (value != _value17)
                {
                    _value17 = value;
                    this.RaisePropertyChanged(() => this.ReportDataPeriod);
                }
            }
        }

        ///// <summary>
        ///// 通讯参数 优先次序
        ///// </summary>
        //public int Priority
        //{
        //    get { return _value18; }
        //    set
        //    {
        //        if (value != _value18)
        //        {
        //            _value18 = value;
        //            this.RaisePropertyChanged(() => this.Priority);
        //        }
        //    }
        //}

        #endregion

        #region IIRtuParaWork

        /// <summary>
        /// 工作参数 滚动显示
        /// </summary>
        public bool Display
        {
            get { return _value19; }
            set
            {
                if (value != _value19)
                {
                    _value19 = value;
                    this.RaisePropertyChanged(() => this.Display);
                }
            }
        }

        /// <summary>
        /// 工作参数 开机申请
        /// </summary>
        public bool Boot
        {
            get { return _value20; }
            set
            {
                if (value != _value20)
                {
                    _value20 = value;
                    this.RaisePropertyChanged(() => this.Boot);
                }
            }
        }

        /// <summary>
        /// 工作参数 声响报警
        /// </summary>

        public bool Sound
        {
            get { return _value21; }
            set
            {
                if (value != _value21)
                {
                    _value21 = value;
                    this.RaisePropertyChanged(() => this.Sound);
                }
            }
        }

        /// <summary>
        /// 工作参数 进入自检
        /// </summary>
        public bool Selfcheck
        {
            get { return _value22; }
            set
            {
                if (value != _value22)
                {
                    _value22 = value;
                    this.RaisePropertyChanged(() => this.Selfcheck);
                }
            }
        }

        /// <summary>
        /// 工作参数 允许报警
        /// </summary>
        public bool Alarm
        {
            get { return _value23; }
            set
            {
                if (value != _value23)
                {
                    _value23 = value;
                    this.RaisePropertyChanged(() => this.Alarm);
                }
            }
        }

        /// <summary>
        /// 工作参数 允许主报
        /// </summary>
        public bool Report
        {
            get { return _value24; }
            set
            {
                if (value != _value24)
                {
                    _value24 = value;
                    this.RaisePropertyChanged(() => this.Report);
                }
            }
        }

        /// <summary>
        /// 工作参数 允许呼叫
        /// </summary>
        public bool Call
        {
            get { return _value25; }
            set
            {
                if (value != _value25)
                {
                    _value25 = value;
                    this.RaisePropertyChanged(() => this.Call);
                }
            }
        }

        /// <summary>
        /// 工作参数 禁止路由
        /// </summary>
        public bool Route
        {
            get { return _value26; }
            set
            {
                if (value != _value26)
                {
                    _value26 = value;
                    this.RaisePropertyChanged(() => this.Route);
                }
            }
        }

        /// <summary>
        /// 工作参数 报警延时（秒）
        /// </summary>
        public int  ErrorDelays
        {
            get { return _value27; }
            set
            {
                if (value != _value27)
                {
                    if (value < 5) return;
                    if (value > 120) return;
                    _value27 = value;
                    this.RaisePropertyChanged(() => this.ErrorDelays);
                }
            }
        }

        //public long RecentUpdateTime { get; set; }

        #endregion

        #region IIRtuParaAnalogueVoltage

        /// <summary>
        /// 电压参数 显示名称
        /// </summary>
        public string RtuVoltageName
        {
            get { return _value28; }
            set
            {
                if (value != _value28)
                {
                    _value28 = value;
                    this.RaisePropertyChanged(() => this.RtuVoltageName);
                }
            }
        }

        /// <summary>
        /// 电压参数 量程
        /// </summary>
        public int Range
        {
            get { return _value29; }
            set
            {
                if (value != _value29)
                {
                    if (value < 0) return;
                    if (value > 400) return;
                    _value29 = value;
                    this.RaisePropertyChanged(() => this.Range);
                }
            }
        }

        /// <summary>
        /// 电压参数 报警上限
        /// </summary>
        public int UpperLimit
        {
            get { return _value30; }
            set
            {
                if (value != _value30)
                {
                    if (value < 0) return;
                    if (value > 400) return;
                    _value30 = value;
                    this.RaisePropertyChanged(() => this.UpperLimit);
                }
            }
        }

        /// <summary>
        /// 电压参数 报警下限
        /// </summary>
        public int LowerLimit
        {
            get { return _value31; }
            set
            {
                if (value != _value31)
                {
                    if (value < -1) return;
                    if (value > 400) return;
                    _value31 = value;

                    this.RaisePropertyChanged(() => this.LowerLimit);
                }
            }
        }



        private bool _isSwitchinputJudgebyA;

        /// <summary>
        /// 是否开关量输入状态有电流来判断 >0.3  由于其他参悟无法放入故放入电压参数中
        /// </summary>
        public bool IsSwitchinputJudgebyA
        {
            get { return _isSwitchinputJudgebyA; }
            set
            {
                if (value != _isSwitchinputJudgebyA)
                {
                    _isSwitchinputJudgebyA = value;
                    this.RaisePropertyChanged(() => this.IsSwitchinputJudgebyA);
                }
            }
        }

        private double _aoverload;
        /// <summary>
        /// 总电流过载
        /// </summary>
        public double Aoverload
        {
            get { return _aoverload; }
            set
            {
                if (value != _aoverload)
                {
                    if (value < 0) return;
                    _aoverload = value;
                    this.RaisePropertyChanged(() => this.Aoverload);
                }
            }
        }

        private double _aoverAlittle;
        /// <summary>
        /// 总电流过限
        /// </summary>
        public double AoverAlittle
        {
            get { return _aoverAlittle; }
            set
            {
                if (value != _aoverAlittle)
                {
                    if (value < 0) return;
                    _aoverAlittle = value;
                    this.RaisePropertyChanged(() => this.AoverAlittle);
                }
            }
        }


        private bool _is3006;
        /// <summary>
        /// 是否为3006设备
        /// </summary>
        public bool Is3006
        {
            get { return _is3006; }
            set
            {
                if (value != _is3006)
                {
                    _is3006 = value;
                    this.RaisePropertyChanged(() => this.Is3006);
                }
            }
        }
         




        private bool _isHasElec;
        /// <summary>
        /// 是否支持电能采集
        /// </summary>
        public bool IsHasElec 
        {
            get { return _isHasElec; }
            set
            {
                if (value != _isHasElec)
                {
                    _isHasElec = value;
                    this.RaisePropertyChanged(() => this.IsHasElec);
                }
            }
        }
         

        private int _aphaseRadio;
        /// <summary>
        /// A相互感比
        /// </summary>
        public int AphaseRadio 
        {
            get { return _aphaseRadio; }
            set
            {
                if (value != _aphaseRadio)
                {
                    if (value < 0) return;
                    _aphaseRadio = value;
                    this.RaisePropertyChanged(() => this.AphaseRadio);
                }
            }
        }


        private int _bphaseRadio;
        /// <summary>
        /// B相互感比
        /// </summary>
        public int BphaseRadio
        {
            get { return _bphaseRadio; }
            set
            {
                if (value != _bphaseRadio)
                {
                    if (value < 0) return;
                    _bphaseRadio = value;
                    this.RaisePropertyChanged(() => this.BphaseRadio);
                }
            }
        }


        private int _cphaseRadio;
        /// <summary>
        /// C相互感比
        /// </summary>
        public int CphaseRadio
        {
            get { return _cphaseRadio; }
            set
            {
                if (value != _cphaseRadio)
                {
                    if (value < 0) return;
                    _cphaseRadio = value;
                    this.RaisePropertyChanged(() => this.CphaseRadio);
                }
            }
        }


        //private bool _iIsShieldLittleA;

        ///// <summary>
        ///// 是否启用屏蔽小电流
        ///// </summary>
        //public bool IsShieldLittleA
        //{
        //    get { return _iIsShieldLittleA; }
        //    set
        //    {
        //        if (value != _iIsShieldLittleA)
        //        {
        //            _iIsShieldLittleA = value;
        //            this.RaisePropertyChanged(() => this.IsShieldLittleA);
        //        }
        //    }
        //}

        //private double _AShield;

        ///// <summary>
        ///// 屏蔽值
        ///// </summary>
        //public double AShield
        //{
        //    get { return _AShield; }
        //    set
        //    {
        //        if (value != _AShield)
        //        {
        //            _AShield = value;
        //            this.RaisePropertyChanged(() => this.AShield);
        //        }
        //    }
        //}

        #endregion
    }

    /// <summary>
    /// 参数下发
    /// </summary>
    public partial class TmlInformationViewModel
    {
        #region

        private DateTime[] _dtSndCheck = new DateTime[15];

        #region CmdSnd4000

        private void Exx2()
        {
            // if (isSndAll == false) ShowInfos.Clear();
            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在发送工作参数;" +
                       Environment.NewLine + ShowInfo;


            UpdateSXg(1);
            //ShowInfos.Add(new ShowInfo()
            //                  {
            //                      Content = "等待应答",
            //                      IndexOfType = 1,
            //                      IsBack = false,
            //                      Title = "正在发送工作参数，第1条，共1条"
            //                  }); this.RaisePropertyChanged(() => this.IsShowInfoVisi);
            // x4000 = 1;
            _dtSndCheck[0] = DateTime.Now;
            Snd4000();


        }

        private bool Canx2()
        {
            return DateTime.Now.Ticks - _dtSndCheck[0].Ticks > 10000000;
        }

        private RelayCommand _cmdx2;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdSnd4000
        {
            get { return _cmdx2 ?? (_cmdx2 = new RelayCommand(Exx2, Canx2, true)); }
        }

        #endregion

        #region CmdSnd4100

        private void Exx3()
        {
            //if (isSndAll == false) ShowInfos.Clear();
            //for (int i = 1; i < 6; i++)
            //    ShowInfos.Add(new ShowInfo()
            //                      {
            //                          Content = "等待应答",
            //                          IndexOfType = 2,
            //                          IsBack = false,
            //                          Title = "正在发送开关量参数，第" + i + "条，共5条"
            //                      }); this.RaisePropertyChanged(() => this.IsShowInfoVisi);

            UpdateSXg(2);
            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在发送开关量参数;" +
                       Environment.NewLine + ShowInfo;
            _dtSndCheck[1] = DateTime.Now;
            Snd4100();

        }

        private bool Canx3()
        {
            return DateTime.Now.Ticks - _dtSndCheck[1].Ticks >10000000;
        }

        private RelayCommand _cmdx3;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdSnd4100
        {
            get { return _cmdx3 ?? (_cmdx3 = new RelayCommand(Exx3, Canx3, true)); }
        }

        #endregion

        #region CmdSnd4200

        private void Exx7()
        {
            //if (isSndAll == false) ShowInfos.Clear();
            //for (int i = 1; i < 6; i++)
            //    ShowInfos.Add(new ShowInfo()
            //                      {
            //                          Content = "等待应答",
            //                          IndexOfType = 3,
            //                          IsBack = false,
            //                          Title = "正在发送模拟量参数，第" + i + "条，共5条"
            //                      }); this.RaisePropertyChanged(() => this.IsShowInfoVisi);

            UpdateSXg(3);
            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在发送模拟量参数;" +
                       Environment.NewLine + ShowInfo;
            _dtSndCheck[2] = DateTime.Now;
            Snd4200();
        }

        private bool Canx7()
        {
            return DateTime.Now.Ticks - _dtSndCheck[2].Ticks > 10000000;
        }

        private RelayCommand _cmdx7;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdSnd4200
        {
            get { return _cmdx7 ?? (_cmdx7 = new RelayCommand(Exx7, Canx7, true)); }
        }

        #endregion

        #region CmdSnd4400

        private void Exx9()
        {
            //if (isSndAll == false) ShowInfos.Clear();
            //ShowInfos.Add(new ShowInfo()
            //                  {
            //                      Content = "等待应答",
            //                      IndexOfType = 4,
            //                      IsBack = false,
            //                      Title = "正在发送上下限参数，第1条，共1条"
            //                  }); this.RaisePropertyChanged(() => this.IsShowInfoVisi);

            UpdateSXg(4);
            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在发送上下限参数;" +
                       Environment.NewLine + ShowInfo;
            _dtSndCheck[3] = DateTime.Now;
            Snd4400();
        }

        private bool Canx9()
        {
            return DateTime.Now.Ticks - _dtSndCheck[3].Ticks > 10000000;
        }

        private RelayCommand _cmdx9;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdSnd4400
        {
            get { return _cmdx9 ?? (_cmdx9 = new RelayCommand(Exx9, Canx9, true)); }
        }

        #endregion

        #region CmdSnd6100

        private void Exx10()
        {
            //if (isSndAll == false) ShowInfos.Clear();
            //ShowInfos.Add(new ShowInfo()
            //                  {
            //                      Content = "等待应答",
            //                      IndexOfType = 5,
            //                      IsBack = false,
            //                      Title = "正在发送电压参数，第1条，共1条"
            //                  }); this.RaisePropertyChanged(() => this.IsShowInfoVisi);


            UpdateSXg(5);
            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在发送电压参数;" +
                       Environment.NewLine + ShowInfo;
            _dtSndCheck[4] = DateTime.Now;
            Snd6100();
        }

        private bool Canx10()
        {
            return DateTime.Now.Ticks - _dtSndCheck[4].Ticks > 10000000;
        }

        private RelayCommand _cmdx10;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdSnd6100
        {
            get { return _cmdx10 ?? (_cmdx10 = new RelayCommand(Exx10, Canx10, true)); }
        }

        #endregion

        #region CmdSndElectric


        private DateTime _dtcmdSndElectric;
        private RelayCommand _cmdSndElectric;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdSndElectric
        {
            get { return _cmdSndElectric ?? (_cmdSndElectric = new RelayCommand(ExCmdSndElectric, CanCmdSndElectric, true)); }
        }


        private void ExCmdSndElectric()
        {

            //if (isSndAll == false) ShowInfos.Clear();
            //ShowInfos.Add(new ShowInfo()
            //                  {
            //                      Content = "等待应答",
            //                      IndexOfType = 5,
            //                      IsBack = false,
            //                      Title = "正在发送电压参数，第1条，共1条"
            //                  }); this.RaisePropertyChanged(() => this.IsShowInfoVisi);


            UpdateSXg(6);
            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在发送电能参数;" +
                       Environment.NewLine + ShowInfo;
            _dtcmdSndElectric = DateTime.Now;

            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.ServerPart.wlst_rtuargsupdate_clinet_order_paras4000;
            info.WstRtuOrders.Op = 8;// 发生电能参数
            info.WstRtuOrders.RtuIds.Add(RtuId);
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private bool CanCmdSndElectric()
        {
            return DateTime.Now.Ticks - _dtcmdSndElectric.Ticks > 10000000;
        }

      
        #endregion



        #region CmdSndElectric


        private DateTime _dtcmdSndWeekSet;
        private RelayCommand _cmdSndWeekSet;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdSndWeekSet
        {
            get { return _cmdSndWeekSet ?? (_cmdSndWeekSet = new RelayCommand(ExCmdSndWeekSet, CanCmdSndWeekSet, true)); }
        }


        private void ExCmdSndWeekSet()
        {

            //if (isSndAll == false) ShowInfos.Clear();
            //ShowInfos.Add(new ShowInfo()
            //                  {
            //                      Content = "等待应答",
            //                      IndexOfType = 5,
            //                      IsBack = false,
            //                      Title = "正在发送电压参数，第1条，共1条"
            //                  }); this.RaisePropertyChanged(() => this.IsShowInfoVisi);

            isFirstAllYear = false;
            UpdateSXg(7);
            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在发送周设置;" +
                       Environment.NewLine + ShowInfo;
            _dtcmdSndWeekSet = DateTime.Now;


            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.wlst_cnt_request_wj3090_measure;
            info.Args.Addr.Add(RtuId);
            info.WstRtuOrders.RtuIds.Add(RtuId);
            info.WstRtuOrders.Op = 11;
            SndOrderServer.OrderSnd(info, 0, 0, true);

        }

        private bool CanCmdSndWeekSet()
        {
            return DateTime.Now.Ticks - _dtcmdSndWeekSet.Ticks > 10000000;
        }


        #endregion



        #region CmdStopRtu

        private void Exx11()
        {
            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在停运;" +
            Environment.NewLine + ShowInfo;
            _dtSndCheck[5] = DateTime.Now;
            SndStopRtu();
        }

        private bool Canx11()
        {
            return
                   DateTime.Now.Ticks - _dtSndCheck[5].Ticks > 30000000; //RtuState == 1 &&
        }

        private RelayCommand _cmdx11;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdStopRtu
        {
            get { return _cmdx11 ?? (_cmdx11 = new RelayCommand(Exx11, Canx11, true)); }
        }

        #endregion


        #region CmdSndAll


        private bool isSndAll = false;

        private void ExxCmdSndAll()
        {
            ShowInfos.Clear();
            _dtSndCheck[8] = DateTime.Now;
            isSndAll = true;
            Exx2();

            Exx3();
            Exx7();
            Exx9();
            Exx10();
            if (IsHasElec)ExCmdSndElectric();
            isSndAll = false;
        }

        private bool CanxCmdSndAll()
        {
            return DateTime.Now.Ticks - _dtSndCheck[8].Ticks > 10000000;
        }

        private RelayCommand _cmdxCmdSndAll;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdSndAll
        {
            get { return _cmdxCmdSndAll ?? (_cmdxCmdSndAll = new RelayCommand(ExxCmdSndAll, CanxCmdSndAll, true)); }
        }

        #endregion

        #region CmdZcVer


        private void ExxCmdZcVer()
        {
            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在召测软件版本;" +
            Environment.NewLine + ShowInfo;
            _dtSndCheck[9] = DateTime.Now;
            SndZcVer();
        }

        private bool CanxCmdZcVer()
        {
            return  DateTime.Now.Ticks - _dtSndCheck[9].Ticks > 5000000;
        }

        private RelayCommand _cmdxCmdZcVer;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdZcVer
        {
            get { return _cmdxCmdZcVer ?? (_cmdxCmdZcVer = new RelayCommand(ExxCmdZcVer, CanxCmdZcVer, true)); }
        }

        #endregion

        #region CmdZcTime

        private void ExxCmdZcTime()
        {
            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在召测时钟;" +
            Environment.NewLine + ShowInfo;
            _dtSndCheck[10] = DateTime.Now;
            SndZcTime();
        }

        private bool CanxCmdZcTime()
        {
            return  DateTime.Now.Ticks - _dtSndCheck[10].Ticks > 5000000;
        }

        private RelayCommand _cmdxCmdZcTime;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdZcTime
        {
            get { return _cmdxCmdZcTime ?? (_cmdxCmdZcTime = new RelayCommand(ExxCmdZcTime, CanxCmdZcTime, true)); }
        }

        #endregion

        #region CmdZcSj


        private void ExxCmdZcSj()
        {
            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在召测终端事件；" +
            Environment.NewLine + ShowInfo;
            _dtSndCheck[14] = DateTime.Now;
            SndZcSj();
        }

        private bool CanxCmdZcSj()
        {
            return DateTime.Now.Ticks - _dtSndCheck[14].Ticks > 5000000;
        }

        private RelayCommand _cmdxCmdZcSj;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdZcSj
        {
            get { return _cmdxCmdZcSj ?? (_cmdxCmdZcSj = new RelayCommand(ExxCmdZcSj, CanxCmdZcSj, true)); }
        }

        #endregion


        #region CmdFwTml


        private void ExxCmdFwTml()
        {
            var t = WlstMessageBox.Show("确认", "是否复位终端，该操作不可逆！",
                            WlstMessageBoxType.YesNo);
            if (t == WlstMessageBoxResults.No)
                return;

            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在复位终端；" +
            Environment.NewLine + ShowInfo;

            _dtSndCheck[11] = DateTime.Now;
            SndFwTml();
        }

        private bool CanxCmdFwTml()
        {
            return DateTime.Now.Ticks - _dtSndCheck[11].Ticks > 5000000;
        }

        private RelayCommand _cmdxCmdFwTml;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdFwTml
        {
            get { return _cmdxCmdFwTml ?? (_cmdxCmdFwTml = new RelayCommand(ExxCmdFwTml, CanxCmdFwTml, true)); }
        }

        #endregion

        #region CmdFwNew


        private void ExxCmdFwNew()
        {
            var t = WlstMessageBox.Show("确认", "是否还原出厂设置，该操作不可逆！",
                WlstMessageBoxType.YesNo);
            if (t == WlstMessageBoxResults.No)
                return;

            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在还原出厂设置；" +
            Environment.NewLine + ShowInfo;

            _dtSndCheck[12] = DateTime.Now;
            SndFwNew();
        }

        private bool CanxCmdFwNew()
        {
            return DateTime.Now.Ticks - _dtSndCheck[12].Ticks > 5000000;
        }

        private RelayCommand _cmdxCmdFwNew;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdFwNew
        {
            get { return _cmdxCmdFwNew ?? (_cmdxCmdFwNew = new RelayCommand(ExxCmdFwNew, CanxCmdFwNew, true)); }
        }

        #endregion

        #region CmdFwMK


        private void ExxCmdFwMK()
        {
            var t = WlstMessageBox.Show("确认", "是否复位通讯模块，该操作不可逆！",
                WlstMessageBoxType.YesNo);
            if (t == WlstMessageBoxResults.No)
                return;

            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在复位通讯模块；" +
            Environment.NewLine + ShowInfo;

            _dtSndCheck[13] = DateTime.Now;
            SndFwMK();
        }

        private bool CanxCmdFwMK()
        {
            return DateTime.Now.Ticks - _dtSndCheck[13].Ticks > 5000000;
        }

        private RelayCommand _cmdxCmdFwMK;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdFwMK
        {
            get { return _cmdxCmdFwMK ?? (_cmdxCmdFwMK = new RelayCommand(ExxCmdFwMK, CanxCmdFwMK, true)); }
        }

        #endregion

        #region CmdReStartRtu

        private void Exx12()
        {
            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在投运;" +
            Environment.NewLine + ShowInfo;
            _dtSndCheck[6] = DateTime.Now;
            SndReStrartRtu();
        }

        private bool Canx12()
        {
            return
                   DateTime.Now.Ticks - _dtSndCheck[6].Ticks > 30000000; //RtuState != 1 &&
        }

        private RelayCommand _cmdx12;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdReStartRtu
        {
            get { return _cmdx12 ?? (_cmdx12 = new RelayCommand(Exx12, Canx12, true)); }
        }

        #endregion


        #region CmdZcHardwareVer

        //lvf 2018年11月12日10:49:36  召测硬件版本
        private void ExxCmdZcHardwareVer()
        {
            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在召测硬件版本;" +
            Environment.NewLine + ShowInfo;
            _dtSndCheck[9] = DateTime.Now;
            SndZcHardwareVer();
        }

        private bool CanxCmdZcHardwareVer()
        {
            return DateTime.Now.Ticks - _dtSndCheck[9].Ticks > 5000000;
        }

        private RelayCommand _cmdxCmdZcHardwareVer;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdZcHardwareVer
        {
            get { return _cmdxCmdZcHardwareVer ?? (_cmdxCmdZcHardwareVer = new RelayCommand(ExxCmdZcHardwareVer, CanxCmdZcHardwareVer, true)); }
        }

        #endregion



        #region CmdReadMapXy

        //lvf 2018年11月12日10:49:50  读取设备位置
        private void ExxCmdReadMapXy()
        {
            ShowInfo = DateTime.Now.ToString("HH:mm:ss") + "  --- 正在读取设备位置;" +
            Environment.NewLine + ShowInfo;
            _dtSndCheck[9] = DateTime.Now;
            SndReadMapXy();
        }

        private bool CanxCmdReadMapXy()
        {
            return DateTime.Now.Ticks - _dtSndCheck[9].Ticks > 5000000;
        }

        private RelayCommand _cmdxCmdReadMapXy;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdReadMapXy
        {
            get { return _cmdxCmdReadMapXy ?? (_cmdxCmdReadMapXy = new RelayCommand(ExxCmdReadMapXy, CanxCmdReadMapXy, true)); }
        }

        #endregion




        #endregion

        #region CmdCleanShowInfo

        private void Exx20()
        {
            ShowInfo = "";
            ShowInfoX = "";
            ShowInfos.Clear();
            this.RaisePropertyChanged(() => this.IsShowInfoVisi);
        }

        private bool Canx20()
        {
            return true;
        }

        private RelayCommand _cmdx20;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdCleanShowInfo
        {
            get { return _cmdx20 ?? (_cmdx20 = new RelayCommand(Exx20, Canx20, true)); }
        }

        #endregion


        private string showInfo;

        public string ShowInfo
        {
            get
            {
                if (showInfo == null) showInfo = "";
                return showInfo;
            }
            set
            {
                if (showInfo != value)
                {
                    showInfo = value;
                    this.RaisePropertyChanged(() => this.ShowInfo);
                }
            }
        }

        private string showInfox;

        public string ShowInfoX
        {
            get
            {
                if (showInfox == null) showInfox = "";
                return showInfox;
            }
            set
            {
                if (showInfox != value)
                {
                    showInfox = value;
                    this.RaisePropertyChanged(() => this.ShowInfoX);
                }
            }
        }

        private DateTime _dataCreateZcSjStart;

        public DateTime DataCreateZcSjStart
        {
            get
            {
                return _dataCreateZcSjStart;
            }
            set
            {
                if (_dataCreateZcSjStart != value)
                {
                    _dataCreateZcSjStart = value;
                    this.RaisePropertyChanged(() => this.DataCreateZcSjStart);
                }
            }
        }
        private DateTime _dataCreateZcSjEnd;

        public DateTime DataCreateZcSjEnd
        {
            get
            {
                return _dataCreateZcSjEnd;
            }
            set
            {
                if (_dataCreateZcSjEnd != value)
                {
                    _dataCreateZcSjEnd = value;
                    this.RaisePropertyChanged(() => this.DataCreateZcSjEnd);
                }
            }
        }

        //if (datax.Op == 1) PublishEvent(datax.RtuIds, Services.EventIdAssign.RtuPara4000Id);
        //if (datax.Op == 2) PublishEvent(datax.RtuIds, Services.EventIdAssign.RtuPara4100Id);
        //if (datax.Op == 3) PublishEvent(datax.RtuIds, Services.EventIdAssign.RtuPara4200Id);
        //if (datax.Op == 4) PublishEvent(datax.RtuIds, Services.EventIdAssign.RtuPara4400Id);
        //if (datax.Op == 5) PublishEvent(datax.RtuIds, Services.EventIdAssign.RtuPara6100Id);
        //if (datax.Op == 7) PublishEvent(datax.RtuIds, Services.EventIdAssign.RtuReStrartRunId);
        //if (datax.Op == 6) PublishEvent(datax.RtuIds, Services.EventIdAssign.RtuStopRunId);

        private void OnDataBack(int tmlId, int eventId,string str)
        {

            //var x1 = x4000 + x4100 + x4200 + x4400 + x6100;
            //if (x1 == 0) return;

            if (showInfo.Length > 3000 && eventId != 61) ShowInfo = "";
            if (eventId == 1)
            {
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + tmlId + " 工作参数     设置应答;" +
                           Environment.NewLine + ShowInfo;

                UpdateXg(1);
            }
            else if (eventId == 2)
            {
                // if (x4100 > 0) x4100--;
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + tmlId + " 开关量参数   设置应答;" +
                           Environment.NewLine + ShowInfo;
                UpdateXg(2);
            }
            else if (eventId == 3)
            {
                // if (x4200 > 0) x4200--;
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + tmlId + " 模拟量参数   设置应答;" +
                           Environment.NewLine + ShowInfo;

                UpdateXg(3);
            }
            else if (eventId == 4)
            {
                // if (x4400 > 0) x4400 = 0;
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + tmlId + " 上下限参数   设置应答;" +
                           Environment.NewLine + ShowInfo;

                UpdateXg(4);
            }
            else if (eventId == 5)
            {

                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + tmlId + " 电压参数     设置应答;" +
                           Environment.NewLine + ShowInfo;

                UpdateXg(5);
            }
            else if (eventId == 7)  //启运
            {
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + tmlId + " 恢复投运应答;" +
                           Environment.NewLine + ShowInfo;
                //this.RtuState = 2;
                //var ins = BackViewModelToTerminalInformation();
                //ins.RtuStateCode = 2;
                //Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.UpdateEquipmentInfo(ins);
            }
            else if (eventId == 6)   //停运
            {
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + tmlId + " 停运应答;" +
                           Environment.NewLine + ShowInfo;
                //this.RtuState = 1;
                //var ins = BackViewModelToTerminalInformation();
                //ins.RtuStateCode = 1;
                //Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.UpdateEquipmentInfo(ins);
            }
            else if (eventId == 22)
            {
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + tmlId + " 终端时钟:" + str + ";" + 
                           Environment.NewLine + ShowInfo;
            }
            else if (eventId == 51)
            {
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + tmlId + " 复位成功;";
            }
            else if (eventId == 52)
            {
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + tmlId + " 恢复出厂设置成功;";
            }
            else if (eventId == 53)
            {
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + tmlId + " 复位通讯模块成功;" +
                           Environment.NewLine + ShowInfo; ;
            }

            else if (eventId == 61)
            {
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + tmlId + " 召测终端事件:" +
                    Environment.NewLine + str + ";" + Environment.NewLine + ShowInfo;
            }

            else if (eventId == 10000)
            {
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + tmlId + " 软件版本:" + str +
                           Environment.NewLine + ShowInfo;
            }
            else if (eventId == 8)
            {
                var strtmp = str == "1" ? "成功" : "失败";
                ShowInfo = DateTime.Now.ToString("HH:mm:ss") + " 地址:" + tmlId + " 电能参数   设置应答:" +
                    Environment.NewLine + strtmp + ";" + Environment.NewLine + ShowInfo;
                UpdateXg(6);
            }


        }

        private void UpdateXg(int x)
        {
            foreach (var f in ShowInfos)
            {
                if (f.IndexOfType == x && f.IsBack == false)
                {
                    f.IsBack = true;
                    f.Content = "√";
                    break;
                }
            }
        }

        private void UpdateSXg(int x)
        {
            int xcount = 1;
            if (x == 2 || x == 3) xcount = 5;
            else xcount = 1;
            if (x == 7) xcount = 2;//正常周设置
            if (x == 8) xcount = 36;//全年时间表
            string info = "";
            if (x == 1) info = "工作参数";
            if (x == 2) info = "开关量参数";
            if (x == 3) info = "模拟量参数";
            if (x == 4) info = "上下限参数";
            if (x == 5) info = "电压参数";
            if (x == 6) info = "电能参数";
            if (x == 7) info = "周设置";
            if (x == 8) info = "全年周设置";
            if (isSndAll == false) ShowInfos.Clear();
            for (int f = 1; f <= xcount; f++)
                ShowInfos.Add(new ShowInfo()
                                  {
                                      Content = "----",
                                      IndexOfType = x,
                                      IsBack = false,
                                      Title = "正在发送" + info + "，第" + f + "条，共" + xcount + "条"
                                  });

            this.RaisePropertyChanged(() => this.IsShowInfoVisi);
        }
    }


    /// <summary>
    /// 附属设备
    /// </summary>
    public partial class TmlInformationViewModel
    {
        private void NavOnLoadInThisViewModelThree()
        {
            GetAllAttachEquimentModule();
            GetAllAttachEquiment();
        }

        private ObservableCollection<AttachEquipmentModuleViewModel> _attachEquipmentModuleList;

        /// <summary>
        /// 获取附属设备模型列表
        /// </summary>
        public ObservableCollection<AttachEquipmentModuleViewModel> AttachEquipmentModuleList
        {
            get
            {
                if (_attachEquipmentModuleList == null)
                {
                    _attachEquipmentModuleList = new ObservableCollection<AttachEquipmentModuleViewModel>();
                    GetAllAttachEquimentModule();
                }
                return _attachEquipmentModuleList;
            }
        }

        /// <summary>
        /// 初始化界面时  需要执行一次  
        /// </summary>
        private void GetAllAttachEquimentModule()
        {
            if (_attachEquipmentModuleList == null)
                _attachEquipmentModuleList = new ObservableCollection<AttachEquipmentModuleViewModel>();
            _attachEquipmentModuleList.Clear();

            int index = 1;
            foreach (var t in EquipmentModelComponentHolding.DicEquipmentModels)
            {
                if (t.Value.CanBeAttachEquipmnet)
                {
                    _attachEquipmentModuleList.Add(new AttachEquipmentModuleViewModel(t.Value.ModelKey,
                                                                                      t.Value.ModuleDescription,
                                                                                      index));
                    index++;
                }
            }
        }

        private ObservableCollection<AttachEquipmentViewModel> _attachEquipmentList;

        public ObservableCollection<AttachEquipmentViewModel> AttachEquipmentList
        {
            get
            {
                if (_attachEquipmentList == null)
                {
                    _attachEquipmentList = new ObservableCollection<AttachEquipmentViewModel>();
                    GetAllAttachEquiment();
                }
                return _attachEquipmentList;
            }
        }

        /// <summary>
        /// 初始化界面时  需要执行一次  
        /// </summary>
        private void GetAllAttachEquiment()
        {
            if (_attachEquipmentList == null)
                _attachEquipmentList = new ObservableCollection<AttachEquipmentViewModel>();
            _attachEquipmentList.Clear();
             

            var attLst =
                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( 
                    this .RtuId);
            if (attLst == null) return;
            int index = 1;
            foreach (var t in attLst.EquipmentsThatAttachToThisRtu )
            {
                if (
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                        InfoItems .
                        ContainsKey(t))
                {
                    var fmp =
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                            InfoItems [
                                t];
                    _attachEquipmentList.Add(new AttachEquipmentViewModel(fmp.RtuId, fmp.RtuName, index, fmp));
                    index++;
                }
            }
            //for (int i = 1; i < 37; i++)
            //{
            //    _attachEquipmentList.Add(new AttachEquipmentViewModel(i, "s" + i, i, null));
            //}
        }

        //private ObservableCollection<NameXy> _addDeletePool;

        //public ObservableCollection<NameXy> AddDeletePool
        //{
        //    get
        //    {
        //        if (_addDeletePool == null)
        //        {
        //            _addDeletePool = new ObservableCollection<NameXy>();
        //            _addDeletePool.Add(new NameXy("添加、删除附属设备请拖拽该选项到此"));
        //        }
        //        return _addDeletePool;
        //    }
        //}


        public void AddModule(int mouduleKey)
        {
            AttachEquipmentModuleViewModel attachEquipmentModuleViewModel = null;
            foreach (var ttt in this.AttachEquipmentModuleList)
            {
                if (ttt.ModuleKey == mouduleKey)
                {
                    attachEquipmentModuleViewModel = ttt;
                }
            }
            if (attachEquipmentModuleViewModel == null) return;
            //组增加终端
            //todo
            var t = WlstMessageBox.Show("确认", "即将增加：" + attachEquipmentModuleViewModel.MouduleName + "型设备一套？",
                                        WlstMessageBoxType.YesNo);
            if (t == WlstMessageBoxResults.No)
                return;

            int mode = mouduleKey;

            if (mode == 0)
            {
                UMessageBox.Show("获取设备型号出错", "获取设备型号出错", UMessageBoxButton.Ok);
                return;

            }
            //todo
            var areaId =
                Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold .MySlef .GetAreaThatRtuIn( this .RtuId );
         
            //.GetRtuInArea( ).ServicesGrpSingleInfoHold.GetRtuBelongGrp(this.RtuId).Item1;
            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.AddAttachEquipment(
                this.RtuId,
                mode,areaId);
        }


        public void DeleteAttachInstances(int instancesId)
        {
            AttachEquipmentViewModel attachEquipmentViewModel = null;
            foreach (var ttt in this.AttachEquipmentList)
            {
                if (ttt.AttachEquipmentId == instancesId)
                {
                    attachEquipmentViewModel = ttt;
                }
            }
            if (attachEquipmentViewModel != null)
            {
                if (!attachEquipmentViewModel.CanBeDelete)
                {
                    UMessageBox.Show(
                        "设备：" + attachEquipmentViewModel.AttachEquipmentName + "" + " 不允许被删除，可能为该主设备运行必备设备...", "该操作不允许",
                        UMessageBoxButton.Ok);
                    return;
                }

                var t = WlstMessageBox.Show("即将删除：" + attachEquipmentViewModel.AttachEquipmentName + "？", "确认",
                                            WlstMessageBoxType.YesNo);
                if (t == WlstMessageBoxResults.No)
                    return;
                Wlst .Sr .EquipmentInfoHolding .Services .EquipmentDataInfoHold .DeleteEquipment(
                    attachEquipmentViewModel.AttachEquipmentId);
            }
        }

         public void ResetCm(int instancesId)
         {
             foreach (var t in AttachEquipmentList)
             {
                 if (t.AttachEquipmentId == instancesId)
                 {
                     t.Cm = new ContextMenu();
                 }
             }
         }
    }

    /// <summary>
    /// event
    /// </summary>
    public partial class TmlInformationViewModel
    {
        private void NavOnLoadInClassFour()
        {
        }


        private void UpdateLdOrLhTree()
        {
            bool IsShowLdSingleTreeOnTabOrNot = false;
            bool IsShowLhSingleTreeOnTabOrNot = false;

            var info1 = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("LhTabTreeSetConfig");

            if (info1.ContainsKey("IsShowSingleTreeOnTabOrNot"))
            {
                IsShowLhSingleTreeOnTabOrNot = info1["IsShowSingleTreeOnTabOrNot"].Contains("yes");
            }

            info1 = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("LdTabTreeSetConfig");

            if (info1.ContainsKey("IsShowSingleTreeOnTabOrNot"))
            {
                IsShowLdSingleTreeOnTabOrNot = info1["IsShowSingleTreeOnTabOrNot"].Contains("yes");
            }

            if ((IsShowLhSingleTreeOnTabOrNot == true) || (IsShowLdSingleTreeOnTabOrNot == true))
            {
                var arg = new PublishEventArgs()
                {
                    EventId = EventIdAssign.SingleInfoGroupAllNeedUpdate,
                    EventType = PublishEventType.Core
                };
                EventPublish.PublishEvent(arg);
            }
        }

        #region IEventAggregator Subscription

        public void FundEventHandler(PublishEventArgs args)
        {
        
            if (
                !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .
                     ContainsKey(this .RtuId))
                return;

            try
            {
                //tmlinfo update
                if (args.EventId == EventIdAssign.EquipmentUpdateEventId)
                {
                    var info = args.GetParams()[0] as List<Tuple<int, int>>;
                    if (info == null) return;
                    if (info.Any(g => g.Item1 == this .RtuId))
                    {
                        object[] parsObjects = new object[] { this.RtuId, "noNeedRequest" };
                        this.NavOnLoad(parsObjects);

                        if (DateTime.Now.Ticks - dtSnd.Ticks < 600000000)
                        {
                            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 终端参数保存成功!!!";
                        }
                        else
                        {
                            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 接收到服务器更新终端参数信息，执行本页面的数据更新!!!";
                        }

                        UpdateLdOrLhTree();
                    }

                    var attlst =
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems [
                            this.RtuId].EquipmentsThatAttachToThisRtu ;
                    var lst = (from t in info where attlst.Contains(t.Item1) select t.Item1).ToList();
                    foreach (var f in lst)
                    {
                        this.UpdateAttachEquipment(f);
                    }
                }
                else if (args.EventId == EventIdAssign.EquipentxyPositonUpdateId)
                {
                    int rtuid = Convert.ToInt32(args.GetParams()[0]);

                    if (!Wlst .Sr .EquipmentInfoHolding .Services .EquipmentDataInfoHold .InfoItems .ContainsKey(rtuid))
                        return;
                    if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid].RtuFid  > 0) return;

                    var f = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid];
                    if (this .RtuId == rtuid)
                    {
                        //_wj3005TerminalInformation.Xmap = f.Xmap;
                        //_wj3005TerminalInformation.Ymap = f.Ymap;
                        Ymap = f.RtuMapY ;
                        Xmap = f.RtuMapX ;
                    }
                }
                else if (args.EventId == EventIdAssign.EquipmentAddEventId)
                {
                    var info = args.GetParams()[0] as List<Tuple<int, int>>;
                    if (info == null) return;
                    var ggg = (from g in info where g.Item2 == this .RtuId select g).ToList();

                    foreach (var t in ggg)
                    {
                        AddAttachEquipment(t.Item1);
                    }
                }
                else if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                {

                    var info = args.GetParams()[0] as List<Tuple<int, int>>;
                    if (info == null) return;

                    if (info.Any(g => g.Item1 == this .RtuId))
                    {
                        Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
                            WJ3005Module.Services.ViewIdAssign.Wj3005TmlInfoSetViewId, false);
                    }
                    else
                    {
                        var ggg = (from g in info where g.Item2 == this.RtuId select g).ToList();
                        foreach (var t in ggg)
                        {
                            foreach (var f in this.AttachEquipmentList)
                            {
                                if (f.AttachEquipmentId == t.Item1)
                                {
                                    this.AttachEquipmentList.Remove(f);
                                    ReIndexAttachEquipmentList();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void UpdateAttachEquipment(int attachEquipmentId)
        {
            foreach (var f in this.AttachEquipmentList)
            {
                if (f.AttachEquipmentId == attachEquipmentId)
                {
                    if (
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                            InfoItems .ContainsKey(attachEquipmentId))
                    {
                        var attIns = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                            InfoItems [attachEquipmentId];
                        if (attIns.RtuFid  == 0)
                            return;
                        f.AttachEquipmentName = attIns.RtuName;
                        f.AttachEquipmentInstance = attIns;
                    }
                    return;
                }
            }
        }

        private void AddAttachEquipment(int attachEquipmentId)
        {
        
            if (
                !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .
                     ContainsKey(this .RtuId))
                return;
            var t =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                    this .RtuId];

            var attlst =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById( 
                    this.RtuId).EquipmentsThatAttachToThisRtu ;

            if (attachEquipmentId == 0)
            {
                foreach (var f in attlst)
                {
                    bool bolfind = false;
                    foreach (var ff in this.AttachEquipmentList)
                    {
                        if (ff.AttachEquipmentId == f)
                        {
                            bolfind = true;
                            break;
                        }
                    }
                    if (!bolfind)
                    {
                        if (
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                                InfoItems .ContainsKey(f))
                        {
                            var att = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                                GetInfoById( f );//[f];

                            if (att.RtuFid  == 0)
                                continue;

                            var gggg =
                                (from gts in this.AttachEquipmentList
                                 where gts.AttachEquipmentId == att.RtuFid
                                 select gts).ToList();
                            if (gggg.Count == 0)
                                this.AttachEquipmentList.Add(new AttachEquipmentViewModel(att.RtuId, att.RtuName,
                                                                                          this.AttachEquipmentList.Count +
                                                                                          1,
                                                                                          att));
                            ReIndexAttachEquipmentList();
                        }
                    }
                }
            }
            else
            {
                if (
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                        InfoItems .
                        ContainsKey(attachEquipmentId))
                {
                    var attIns =
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                            GetInfoById( 
                                attachEquipmentId);
                    if (attIns.RtuFid  == 0)
                        return;
                    this.AttachEquipmentList.Add(new AttachEquipmentViewModel(attIns.RtuId, attIns.RtuName,
                                                                              this.AttachEquipmentList.Count + 1, attIns));
                    ReIndexAttachEquipmentList();
                }
            }
        }

        private void ReIndexAttachEquipmentList()
        {
            try
            {
                for (int i = AttachEquipmentList.Count - 1; i >= 0; i--)
                {
                    int instancesid = AttachEquipmentList[i].AttachEquipmentId;
                    bool has = false;
                    for (int j = 0; j < i; j++)
                    {
                        if (AttachEquipmentList[j].AttachEquipmentId == instancesid)
                        {
                            has = true;
                        }
                    }
                    if (has)
                    {
                        AttachEquipmentList.RemoveAt(i);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            for (int i = 0; i < AttachEquipmentList.Count; i++)
            {
                AttachEquipmentList[i].Index = i + 1;
            }
        }

        /// <summary>
        /// 事件过滤
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
               // if (_wj3005TerminalInformation == null) return false;
                if (args.EventType == PublishEventType.Core)
                {
                    //tmlinfo update
                    if (args.EventId == EventIdAssign.EquipmentAddEventId ||
                        args.EventId == EventIdAssign.EquipmentDeleteEventId ||
                        args.EventId == EventIdAssign.EquipmentUpdateEventId ||
                        args.EventId == EventIdAssign.EquipentxyPositonUpdateId
                        )
                    {
                        return true;
                    }

                    //if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuPara4100Id ||
                    //    args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuPara4000Id ||
                    //    args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuPara4200Id ||
                    //    args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuPara4400Id ||
                    //    args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuReStrartRunId ||
                    //    args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuStopRunId ||
                    //    args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuPara6100Id)
                    //{
                    //    return true;
                    //}

                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        #endregion

    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class TmlInformationViewModel
    {

        private ObservableCollection<ShowInfo> _attShowInfos;

        public ObservableCollection<ShowInfo> ShowInfos
        {
            get
            {
                if (_attShowInfos == null)
                {
                    _attShowInfos = new ObservableCollection<ShowInfo>();
                }
                return _attShowInfos;
            }
        }


        //private Visibility isShwoInfosVisi;
        public Visibility IsShowInfoVisi
        {
            get { return ShowInfos.Count > 0 ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// 工作参数
        /// </summary>
        private void Snd4000()
        {
            Snd4000(this.RtuId);

        }

      
        /// <summary>
        /// 口失参数
        /// </summary>
        private void Snd4100()
        {
            Snd4100(this.RtuId);
        }

       
        /// <summary>
        /// 模拟量参数
        /// </summary>
        private void Snd4200()
        {
            Snd4200(this.RtuId);
        }


       
        /// <summary>
        /// 上下线参数
        /// </summary>
        private void Snd4400()
        {
            Snd4400(this.RtuId);
        }

       
        /// <summary>
        /// 电压参数
        /// </summary>
        private void Snd6100()
        {
            Snd6100(this.RtuId);
        }


        private void SndStopRtu()
        {
            SndStopRtu(this.RtuId);

        }

        private void SndReStrartRtu()
        {
            SndReStrartRtu(this.RtuId);
        }

        private void SndZcVer()
        {
            SndZcVer(this.RtuId);
        }

        private void SndZcHardwareVer()
        {
            SndZcHardwareVer(this.RtuId);
        }

        private void SndReadMapXy()
        {
            SndReadMapXy(this.RtuId);
        }

        private void SndZcTime()
        {
            SndZcTime(this.RtuId);
        }

        private void SndFwTml()
        {
            SndFwTml(this.RtuId);
        }

        private void SndFwNew()
        {
            SndFwNew(this.RtuId);
        }

        private void SndFwMK()
        {
            SndFwMK(this.RtuId);
        }

        private void SndZcSj()
        {
            SndZcSj(this.RtuId, this.DataCreateZcSjStart, this.DataCreateZcSjEnd);
        }

    }


    public class ShowInfo : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public int IndexOfType;
        public bool IsBack;

        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                if (value == title) return;
                title = value;
                this.RaisePropertyChanged(() => this.Title);
            }
        }

        private string tContent;

        public string Content
        {
            get { return tContent; }
            set
            {
                if (value == tContent) return;
                tContent = value;
                this.RaisePropertyChanged(() => this.Content);
            }

        }
    }
}

