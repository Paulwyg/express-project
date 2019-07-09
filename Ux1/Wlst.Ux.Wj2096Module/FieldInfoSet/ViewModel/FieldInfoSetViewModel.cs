using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using Telerik.Windows.Controls.ColorEditor.Mode;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.DataValidation;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Sr.EquipmentInfoHolding.Model;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Sr.SlusglInfoHold.Services;
using Wlst.Ux.Wj2096Module.FieldInfoSet.Services;
using Wlst.Ux.Wj2096Module.Services;
using System.Collections.ObjectModel;
using Wlst.Cr.CoreOne.Models;

using System.Windows;
using DragDropExtend.ExtensionsHelper;
using System.Windows.Controls;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.client;
using EventIdAssign = Wlst.Sr.SlusglInfoHold.Services.EventIdAssign;


namespace Wlst.Ux.Wj2096Module.FieldInfoSet.ViewModel
{
    [Export(typeof(IIConcentratorParaInformationViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ConcentratorParaInformationViewModel : Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged, IIConcentratorParaInformationViewModel
    {

        public ConcentratorParaInformationViewModel()
        {
            InitAciton();
            InitEvent();
        }

        public void InitEvent()
        {
            this.AddEventFilterInfo(EventIdAssign.SluSglEquUpdate, PublishEventType.Core,true );
            this.AddEventFilterInfo(EventIdAssign.SluSglFieldGrpUpdate, PublishEventType.Core,true);
        }

        private long dtsnd = 0;
        /// <summary>
        /// 原有控制器列表
        /// </summary>
        private List<int> ctrllistfirst = new List<int>();
        private List<ControlParaItem> ControlParaItemsOld;

        /// <summary>
        /// 系统下 所有控制器条形码
        /// </summary>
        private ConcurrentDictionary<int, List<Tuple<long, int>>> allctrllist = new ConcurrentDictionary<int, List<Tuple<long, int>>>();
        /// <summary>
        /// 
        /// </summary>
        private ConcurrentDictionary<long, Tuple<int, int>> allctrllist1 = new ConcurrentDictionary<long,Tuple<int, int>>();

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            if (_terminalInformation == null) return;

            try
            {
                if (args.EventId == EventIdAssign.SluSglEquUpdate)
                {
                    var info1 = args.GetParams()[0];
                    var info = Convert.ToInt32(info1);
                    if (info < 1) return;
                    if (info != _terminalInformation.FieldId) return;


                    if (DateTime.Now.Ticks - dtsnd < 600000000)
                    {
                        Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 域参数保存成功!!!";
                    }
                    else
                    {
                        Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 接收到服务器更新域参数信息，执行本页面的数据更新!!!";
                    }

                    //if (saveflg == new Tuple<bool, bool>(true, true))
                    //{
                    NavOnLoad(info);
                    //}

                }
                if (args.EventId == EventIdAssign.SluSglFieldGrpUpdate)
                {
                    var info1 = args.GetParams()[0];
                    var info = Convert.ToInt32(info1);
                    if (info < 1) return;
                    if (info != _terminalInformation.FieldId) return;

                    if (DateTime.Now.Ticks - dtsnd < 600000000)
                    {
                        Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 域分组参数保存成功!!!";
                    }
                    else
                    {
                        Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 接收到服务器更新域分组参数信息，执行本页面的数据更新!!!";
                    }

                    //if (saveflg == new Tuple<bool, bool>(true, true))
                    //{
                    NavOnLoad(info);
                   // }
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error:" + ex);
            }
        }

        #region Msg

        private string _btMsg;

        public string Msg
        {
            get { return _btMsg; }
            set
            {
                if (_btMsg == value) return;
                _btMsg = value;
                RaisePropertyChanged(() => Msg);
            }
        }

        #endregion

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
            get { return "域参数设置"; }
        }

        #endregion

        private bool isViewActive = false;
        private int AreaId = 0;
        public void NavOnLoad(params object[] parsObjects)
        {
            ScanMode = false;
            IsCtrlGrp = false;
            saveflg = new Tuple<bool, bool>(false,false);


            int singleId = Convert.ToInt32(parsObjects[0]);
            if (singleId > 0)
            {
                var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(singleId);
                if (para == null)
                {
                    this.SingleId = singleId;
                    this.FieldName = "未知域";
                    return;
                }

                _terminalInformation = para;

                AreaId = para.AreaId;
                FieldName = para.FieldName;
                FieldId = para.FieldId;
                OtherAttri = para.OtherAttri;
                Is2096 = para.OtherAttri == 0;
                PhyId = para.PhyId;
                CtrlCount = para.CtrlLst.Count;

                foreach (var t in para.CtrlLst)
                {
                    ctrllistfirst.Add(t.CtrlId);
                }
                
                InitControlViewModel();
                InitGroupViewModel();
            }

            

        }


        public void OnUserHideOrClosing()
        {
            isViewActive = false;
            foreach (var t in ControlParaItems)
            {
                try
                {
                    t.OnAttriChanged -= this.g_OnAttriChanged;
                }
                catch (Exception ex)
                {

                }
            }
            ControlParaItems.Clear();
        }
    }

    /// <summary>
    /// 集中器基本属性
    /// </summary>
    public partial class ConcentratorParaInformationViewModel
    {

        private Wlst.client.EquipmentParameter BackConcentratorViewModelEqu()
        {
            return new EquipmentParameter()
            {
                //DateCreate = _terminalInformation.DateCreate,
                //DateUpdate = DateTime.Now.Ticks,
                //RtuId = this.SingleId,
                //RtuName = this.SingleName,
                //RtuPhyId = PhyId,
                //RtuFid = FId,
                //RtuGisX = _terminalInformation.RtuGisX,
                //RtuGisY = _terminalInformation.RtuGisY,
                //RtuArgu = this.IsAllowPatrolOnLight ? "1" : "0",// _terminalInformation.RtuArgu,
                //RtuInstallAddr = _terminalInformation.RtuInstallAddr,

                //RtuMapX = _terminalInformation.RtuMapX,
                //RtuMapY = _terminalInformation.RtuMapY,
                //RtuModel = _terminalInformation.RtuModel,
                //RtuStateCode = 2,
                //RtuRemark = Remark,
            };
        }


        private SluParameter BackConcentratorViewModelSlu()
        {
            return new SluParameter()
            {
                //AlarmCountCommucationFail = CommunicationFailureNum,
                //IsPartrolMeasured = this.IsAllowPatrol,
                //IsSndOrderAuto = this.IsReissue,
                //CurrentUpper = this.ARange,
                //PowerUpper = this.PRange,
                //SumOfControls = ControlNum,
                //DomainName = DomainName,
                //IsAlarmAuto = this.IsAllowActiveAlarm,
                //IsZigbee = this.IsZigbee ? 1 : 0,

                //BluetoothPin = this.PCode,
                //SecurityPattern = this.SafeMode,
                //RouteRunPattern = this.RouteRunMode,
                //UpperVoltage = this.VAlarmMax,
                //AlarmPowerfactorLower = this.PowerFactor,
                //LowerVoltage = this.VAlarmMin,
                //MobileNo = this.PhoneNum,
                //Longitude = this.Longitude,
                //Latitude = this.Latitude,
                //IsUsed = IsStopRun == false,
                //ChannelUsed = (from t in IsCommunicationChannel where t.IsSelected select t.Value).ToList(),
                //PowerAdjustType = PowerControl ? 2 : 1,
                //PowerAdjustBound =
                //    PowerControl ? CurrentBaudItem == null ? 2400 : CurrentBaudItem.Value : Frequency,
                //ZigbeeAddress = this.ZgbAddress,
                //RtuId = _terminalInformation.RtuId,
                //StaticIp = 0,

                //RelatedRtuId = this.RelatedRtuId,
            };


        }
        

        #region ScanMode
        private bool _isScanMode;

        public bool ScanMode
        {
            get { return _isScanMode; }
            set
            {
                if (_isScanMode == value) return;
                _isScanMode = value;
                RaisePropertyChanged(() => ScanMode);
            }
        }

        #endregion


        #region CmdCtrlSave

        private ICommand _cmdCtrlSave;

        public ICommand CmdCtrlSave
        {
            get { return _cmdCtrlSave ?? (_cmdCtrlSave = new RelayCommand(ExCtrlSave, CanCtrlSave, false)); }
        }

        private void ExCtrlSave()
        {
            if (
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "您将要修改控制器数量，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
            {
                return;
            }

            foreach (var t in ControlParaItems)
            {
                try
                {
                    t.OnAttriChanged -= this.g_OnAttriChanged;
                }
                catch (Exception ex)
                {

                }
            }
            //var count = ControlParaItems.Count;

            //var controlitems = ControlParaItems;
            ControlParaItems.Clear();


            int xCount = this.CtrlCount;
            int yCount = _terminalInformation.CtrlLst.Count;

            var ctrllistinfo = new List<EquSluSgl.ParaSluCtrl>();

            int inde = 0;
            foreach (var t in _terminalInformation.CtrlLst)
            {
                if (inde < yCount)
                {
                    inde++;
                    ctrllistinfo.Add(t);
                }
            }

            if (xCount <= yCount)
            {
                if (ctrllistinfo.Count>0)
                {
                    var tmps = (from t in ctrllistinfo orderby t.CtrlId ascending select t).ToList();
                    for (int i = 0; i < xCount; i++)
                    {
                        AddToItems(tmps[i], i,Is2096);
                    }
                }
            }
            else
            {
                if (ctrllistinfo.Count > 0)
                {
                    var tmps = (from t in ctrllistinfo orderby t.CtrlId ascending select t).ToList();
                    int ind = 0;
                    foreach (var g in tmps)
                    {
                        AddToItems(g, ind,Is2096);
                        ind++;
                    }
                }
                int max = 0;
                foreach (var g in ctrllistinfo)
                {
                    if (g.CtrlId > max) max = g.CtrlId;
                }
                max++;
                int index0 = _terminalInformation.CtrlLst.Count;
                index0++;
                int xAdd = xCount - yCount;

                //int maxid = 8000000;
                //var para = SluSglInfoHold.MySlef.Info;
                //foreach (var t in para)
                //{
                //    foreach (var tt in t.Value.CtrlLst)
                //    {
                //        if (tt.CtrlId>maxid)
                //        {
                //            maxid = tt.CtrlId;
                //        }
                //    }
                //}

                for (int i = max; i < max + xAdd; i++)
                {
                    var ntps = new ControlParaItem
                    {
                        Is2096 = this.Is2096,
                        IsChecked = false,
                        RtuId = 0,
                        Index = ControlParaItems.Count + 1,
                        LightIndex = ControlParaItems.Count + 1,
                        BarCode = i.ToString().PadLeft(13, '0'),// "000 000 000 0000",
                        //i.ToString().PadLeft(13, '0'),
                        //"000 000 000 0000",
                        IsActiveAlarm = true,
                        IsRun = true,
                        UplinkReply=false,
                        UplinkTimer=30,
                        PowerMax = 120,
                        PowerMin = 80,
                        IsPowerOnLight1 = true,
                        IsPowerOnLight2 = true,
                        IsPowerOnLight3 = true,
                        IsPowerOnLight4 = true,
                        CurrentSelectLoopVectorItem1 =
                            new NameValueInt() { Name = "LoopVector", Value = 1 },
                        CurrentSelectLoopVectorItem2 =
                            new NameValueInt() { Name = "LoopVector", Value = 2 },
                        CurrentSelectLoopVectorItem3 =
                            new NameValueInt() { Name = "LoopVector", Value = 3 },
                        CurrentSelectLoopVectorItem4 =
                            new NameValueInt() { Name = "LoopVector", Value = 4 },
                        LampCode = "新控制器" + (ControlParaItems.Count + 1).ToString(),
                        CurrentSelectLoopRatePowerIndex1 =
                            new NameValueInt() { Name = "201-250", Value = 6 },
                        CurrentSelectLoopRatePowerIndex2 =
                            new NameValueInt() { Name = "201-250", Value = 6 },
                        CurrentSelectLoopRatePowerIndex3 =
                            new NameValueInt() { Name = "201-250", Value = 6 },
                        CurrentSelectLoopRatePowerIndex4 =
                            new NameValueInt() { Name = "201-250", Value = 6 },

                       
                    };
                    if (Is2290)
                    {
                        //铭泰设备 设备型号为 其他厂商 lvf  2019年2月19日15:06:06
                        foreach (var g in ntps.CtrlTypeItems)
                        {
                            if (g.Value ==0)  ntps.CurrentCtrlTypeSelected = g;
                        }
                       
                    }

                    ControlParaItems.Add(ntps);
                    index0++;
                }
            }
            for (int i = 1; i <= ControlParaItems.Count; i++)
            {
                ControlParaItems[i - 1].Index = i;
            }
            EndCtrl = ControlParaItems.Count;


            foreach (var g in this.ControlParaItems)
            {
                g.OnAttriChanged += new EventHandler<AttriChangedArgs>(g_OnAttriChanged);
            }

        }

        private bool CanCtrlSave()
        {
            return true;
        }

        #endregion

        #region CmdImport

        private ICommand _cmdImport;

        public ICommand CmdImport
        {
            get { return _cmdImport ?? (_cmdImport = new RelayCommand(ExCmdImport, CanCmdImport, false)); }
        }

        private void ExCmdImport()
        {

            if (ControlParaItems.Count >0 )
            {
              if ( Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "已存在控制器，是否继续添加？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                {
                    return; 
                }
            }
            else
            {
                if (
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "您将要批量导入控制器，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                {
                    return;
                }
            }
            
            foreach (var t in ControlParaItems)
            {
                try
                {
                    t.OnAttriChanged -= this.g_OnAttriChanged;
                }
                catch (Exception ex)
                {

                }
            }


            var ctrlinfo = GetCtrlInfo();
            
            if ( ctrlinfo.Count == 0)
            {
                MessageBox.Show("配置文件不存在 ", "批量导入出错，Config文件夹下", MessageBoxButton.OK);
                return;
            }

            //var count = ControlParaItems.Count;

            //var controlitems = ControlParaItems;
            ControlParaItems.Clear();


            int xCount = ctrlinfo.Count;
            int yCount = _terminalInformation.CtrlLst.Count;

            var ctrllistinfo = new List<EquSluSgl.ParaSluCtrl>();

            foreach (var t in _terminalInformation.CtrlLst)
            {
                ctrllistinfo.Add(t);
            }

         
                if (ctrllistinfo.Count > 0)
                {
                    var tmps = (from t in ctrllistinfo orderby t.CtrlId ascending select t).ToList();
                    int ind = 0;
                    foreach (var g in tmps)
                    {
                        AddToItems(g, ind,Is2096);
                        ind++;
                    }
                }
                int max = 0;
                foreach (var g in ctrllistinfo)
                {
                    if (g.CtrlId > max) max = g.CtrlId;
                }
                max++;
                int index0 = _terminalInformation.CtrlLst.Count;
                index0++;

                foreach (var s in ctrlinfo)
                {
                    var ntps = new ControlParaItem
                    {
                        IsChecked = false,
                        Is2096 = this.Is2096,
                        RtuId = 0,
                        Index = ControlParaItems.Count + 1,
                        LightIndex = ControlParaItems.Count + 1,
                        BarCode = s.Item1,
                        //i.ToString().PadLeft(13, '0'),
                        //"000 000 000 0000",
                        IsActiveAlarm = true,
                        IsRun = true,
                        UplinkReply = false,
                        UplinkTimer = 30,
                        PowerMax = 120,
                        PowerMin = 80,
                        IsPowerOnLight1 = true,
                        IsPowerOnLight2 = true,
                        IsPowerOnLight3 = true,
                        IsPowerOnLight4 = true,
                        CurrentSelectLoopVectorItem1 =
                            new NameValueInt() { Name = "LoopVector", Value = 1 },
                        CurrentSelectLoopVectorItem2 =
                            new NameValueInt() { Name = "LoopVector", Value = 2 },
                        CurrentSelectLoopVectorItem3 =
                            new NameValueInt() { Name = "LoopVector", Value = 3 },
                        CurrentSelectLoopVectorItem4 =
                            new NameValueInt() { Name = "LoopVector", Value = 4 },
                        LampCode =s.Item2==""? "新控制器" + (ControlParaItems.Count + 1).ToString():s.Item2,
                        CurrentSelectLoopRatePowerIndex1 =
                            new NameValueInt() { Name = "201-250", Value = 6 },
                        CurrentSelectLoopRatePowerIndex2 =
                            new NameValueInt() { Name = "201-250", Value = 6 },
                        CurrentSelectLoopRatePowerIndex3 =
                            new NameValueInt() { Name = "201-250", Value = 6 },
                        CurrentSelectLoopRatePowerIndex4 =
                            new NameValueInt() { Name = "201-250", Value = 6 },

                        SimsCode = s.Item3
                    };

                    ControlParaItems.Add(ntps);
                    index0++;
                }




            //}
            for (int i = 1; i <= ControlParaItems.Count; i++)
            {
                ControlParaItems[i - 1].Index = i;
            }
            EndCtrl = ControlParaItems.Count;


            foreach (var g in this.ControlParaItems)
            {
                g.OnAttriChanged += new EventHandler<AttriChangedArgs>(g_OnAttriChanged);
            }

        }


        //条形码，控制器名称，imei
        private List<Tuple<string,string,string>> GetCtrlInfo()
        {
            string dir = Directory.GetCurrentDirectory() + "\\Config";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\" + "2096.txt";

            try
            {

                if (File.Exists(path))
                {


                    var ctrlinfo = new List<Tuple<string, string,string>>();
                    StreamReader sr = new StreamReader(path, Encoding.Default);

                    String str = sr.ReadToEnd();

                    sr.Close();

                    string[] line = str.Split(Environment.NewLine.ToCharArray());

                    //string[] value1 =  line.Split(',');
                    if (line.Length>0)
                    {
                        var ctrlCode = "0";
                        var ctrlName = "";
                        var ctrlImei = "0";
                        foreach (var s in line)
                        {
                            if (s == "") continue;

                            ctrlCode = "0";
                            ctrlName = "新控制器";
                            ctrlImei = "0";
                            string[] info = s.Split(',');
                            if ( info.Length ==1 )
                            {
                                ctrlCode = info[0];
                            }

                            if(info.Length==2)
                            {
                                ctrlCode = info[0];
                                ctrlName = info[1];
                            }

                            if (info.Length == 3)
                            {
                                ctrlCode = info[0];
                                ctrlName = info[1];
                                ctrlImei = info[2];
                            }

                            ctrlinfo.Add(new Tuple<string, string, string>(ctrlCode, ctrlName, ctrlImei));
                        }
                    }

                    //string[] value = line.Split(',');
                    return ctrlinfo;
                    //if (value.Length == 2)
                    //{
                    //    if (value[0] == DateTime.Now.ToString("yyyyMMdd"))
                    //    {
                    //        if (value[1].Length == 4)
                    //        {

                    //        }
                    //    }
                    //}
                }

                return  new List<Tuple<string, string,string>>();
            }
            catch (Exception)
            {
                return new List<Tuple<string, string,string>>();
            }


        }

        private bool CanCmdImport()
        {
            return true;
        }


        #endregion



        /// <summary>
        /// 域名称
        /// </summary>

        #region FieldName
        private string _fieldName;
        [StringLength(30, ErrorMessage = "名称长度不能大于30")]
        [Required(ErrorMessage = "输入不得为空")]
        public string FieldName
        {
            get { return _fieldName; }
            set
            {
                if (_fieldName == value) return;
                _fieldName = value;
                RaisePropertyChanged(() => FieldName);
            }
        }

        #endregion

        /// <summary>
        /// 域控制器分组设置
        /// </summary>
        private bool _isCtrlGrp;
        public bool IsCtrlGrp
        {
            get { return _isCtrlGrp; }
            set
            {
                if (_isCtrlGrp == value) return;


                if (!_isCtrlGrp)
                {
                    if(ControlParaItems.Count  ==0 )
                    {
                        Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                               "请先添加控制器。", WlstMessageBoxType.Ok);
                        return;
                    
                    }


                    if (ControlParaItemsOld.Count != ControlParaItems.Count)
                    {
                        Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                            "修改分组前请先保存域参数。", WlstMessageBoxType.Ok);
                        return;
                    }

                    for (int i = 0; i < ControlParaItemsOld.Count; i++)
                    {
                        if (ControlParaItems[i] != ControlParaItemsOld[i])
                        {
                            Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                            "修改分组前请先保存域参数。", WlstMessageBoxType.Ok);
                            return;
                        }
                    }

                    if (!(FieldId == _terminalInformation.FieldId && PhyId == _terminalInformation.PhyId && FieldName == _terminalInformation.FieldName
                        && AreaId == _terminalInformation.AreaId))
                    {

                        Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                            "修改分组前请先保存域参数。", WlstMessageBoxType.Ok);
                            return;
                    }
                }

                _isCtrlGrp = value;
                RaisePropertyChanged(() => IsCtrlGrp);

            }
        }

        /// <summary>
        /// 域物理地址
        /// </summary>
        #region SingleId
        private int _phyId;

        [Range(1, 100000.99, ErrorMessage = "物理地址大小介于1至100000之间")]
        public int PhyId
        {
            get { return _phyId; }
            set
            {
                if (_phyId == value) return;
                _phyId = value;
                RaisePropertyChanged(() => PhyId);
            }
        }

        private int _fieldId;

        public int FieldId
        {
            get { return _fieldId; }
            set
            {
                if (_fieldId == value) return;
                _fieldId = value;
                RaisePropertyChanged(() => FieldId);
            }
        }


        //lvf 2018年10月17日09:26:34 支持2290设备
        private int _otherAttri;

        public int OtherAttri
        {
            get { return _otherAttri; }
            set
            {




                //if (_otherAttri == value) return;
                _otherAttri = value;
                RaisePropertyChanged(() => OtherAttri);

                if (OtherAttri == 1)
                {
                    Is2290 = true;
                    Is2096 = false;
                }
                else
                {
                    Is2290 = false;
                    Is2096 = true;
                }
            }
        }


        private bool _is2290;
        /// <summary>
        /// 是否为2290型设备
        /// </summary>
        public bool Is2290
        {
            get { return _is2290; }
            set
            {
                if (_is2290 == value) return;
                _is2290 = value;
                RaisePropertyChanged(() => Is2290);

            }
        }


        private bool _is2096;
        /// <summary>
        /// 是否为2096型设备
        /// </summary>
        public bool Is2096
        {
            get { return _is2096; }
            set
            {
                if (_is2096 == value) return;
                _is2096 = value;
                RaisePropertyChanged(() => Is2096);

            }
        }


        private int _ctrlCount;

        [Range(1, 1000, ErrorMessage = "控制器数量介于1至1000之间")]
        public int CtrlCount
        {
            get { return _ctrlCount; }
            set
            {
                if (_ctrlCount == value) return;
                if (value > 1000) value = 1000;
                _ctrlCount = value;
                RaisePropertyChanged(() => CtrlCount);
            }
        }

        private int _relatedRtuId;
        [Range(1, 10000.99, ErrorMessage = "终端物理地址大小介于1至10000之间")]
        public int RelatedRtuId
        {
            get { return _relatedRtuId; }
            set
            {
                if (_relatedRtuId == value) return;
                _relatedRtuId = value;
                RaisePropertyChanged(() => RelatedRtuId);
            }
        }

        private int _singleId;
        public int SingleId
        {
            get { return _singleId; }
            set
            {
                if (_singleId == value) return;
                _singleId = value;
                RaisePropertyChanged(() => SingleId);

                var tmps = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value);
                ShowSluId = tmps == null ? value + "" : tmps.RtuPhyId.ToString("d4");
            }
        }

        private string _singShowSluIdleId;
        public string ShowSluId
        {
            get { return _singShowSluIdleId; }
            set
            {
                if (_singShowSluIdleId == value) return;
                _singShowSluIdleId = value;
                RaisePropertyChanged(() => ShowSluId);
            }
        }



        #endregion


        /// <summary>
        /// 控制器数量
        /// </summary>

        #region ControlNum
        private int _controlNum;
        [Range(1, 256, ErrorMessage = "控制器数量在1到256之间")]
        public int ControlNum
        {
            get { return _controlNum; }
            set
            {
                if (_controlNum == value) return;
                _controlNum = value;
                RaisePropertyChanged(() => ControlNum);
            }
        }

        #endregion


        #region SaveFieldCtrl

        private ICommand _cmdSaveFieldCtrl;

        public ICommand CmdSaveFieldCtrl
        {
            get
            {
                return _cmdSaveFieldCtrl ??
                       (_cmdSaveFieldCtrl = new RelayCommand(Ex_cmdSaveFieldCtrl, Can_cmdSaveFieldCtrl, true));
            }
        }

       
        private void Ex_cmdSaveFieldCtrl()
        {
            dtsnd = DateTime.Now.Ticks;

            if (!IsCtrlGrp)
            {
               // SaveField();
                SaveFieldCtrl();
            }
            else
            {
               // SaveField();
                SaveFieldCtrlGrp();
            }

            //SaveField();
            //SaveFieldCtrl();
            //SaveFieldCtrlGrp();

            saveflg = new Tuple<bool, bool>(false,false);
        }

        private bool Can_cmdSaveFieldCtrl()
        {
            if (DateTime.Now.Ticks - dtsnd > 10 * 10000000 && _terminalInformation != null) return true;
            return false;
        }

        //private void SaveField()
        //{

        //    var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_field_info;
        //    info.WstSlusglFieldInfo.Op = 3;             // todo 单域更新
        //    info.WstSlusglFieldInfo.Items.Clear();

        //    var lst = new List<int>();
        //    foreach (var i in ControlParaItems)
        //    {
        //        lst.Add(i.RtuId);
        //    }


        //    info.WstSlusglFieldInfo.Items.Add(new FieldSluSgl.FieldSluSglItem()
        //    {
        //        FieldId = SingleId,
        //        AreaId = AreaId,
        //        FieldName = FieldName,
        //        Order = PhyId,
        //        CtrlLst = lst
        //    });

        //    SndOrderServer.OrderSnd(info, 10, 6);

            
        //}

        private void SaveFieldCtrl()
        {
            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_equ;
            info.WstSlusglEqu.Items.Clear();
            info.WstSlusglEqu.Op = 3;

            //allctrllist.Clear();

            allctrllist1.Clear();
            var para = Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Info;
            foreach (var t in para)
            {
                if (t.Value.PhyId == PhyId && t.Value.FieldId != FieldId)
                {
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "物理地址有重复。", WlstMessageBoxType.Ok);
                    return;
                }
                //if (allctrllist.ContainsKey(t.Key) == false)
                //{
                //    allctrllist.TryAdd(t.Key,null);
                //}
                //else
                //{
                //    allctrllist[t.Key] = null;
                //}
                //lvf 判断控制器条形码 是否重复
                foreach (var g in t.Value.CtrlLst)
                {

                    if (allctrllist1.ContainsKey(g.BarCodeId) == false)
                    {
                        allctrllist1.TryAdd(g.BarCodeId, new Tuple<int, int>(t.Key, g.CtrlId));
                    }
                    else
                    {
                        allctrllist1[g.BarCodeId]=new Tuple<int, int>(t.Key, g.CtrlId);
                    }
                        


                    
                }


            }


            
            var ctrlremove = ctrllistfirst;
            //lvf 自己控制器编号是否有重复or有0
            //条形码-物理地址
            var ctrllist = new ConcurrentDictionary<long,int>(); //  new List<long>();
            foreach (var t in ControlParaItems)
            {
                //if (t.RtuId == 0) continue;

                if (ctrllistfirst.Contains(t.RtuId))
                {
                    ctrlremove.Remove(t.RtuId);
                }

                //lvf 自己控制器编号是否有重复or有0
                if (ctrllist.ContainsKey(t.BarCodeId) == false)
                {
                    ctrllist.TryAdd(t.BarCodeId,t.RtuId);
                }
                else
                {
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "条形码有重复。重复条形码为" + t.BarCodeId + " \r\n物理地址为：" + t.RtuId+" , "+ctrllist[t.BarCodeId], WlstMessageBoxType.Ok);
                    return;
                }
                if(t.BarCodeId == 0)
                {
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                           "条形码不能为0，请重新设置" + "物理地址为：" + t.RtuId, WlstMessageBoxType.Ok);
                    return;
                }

                //foreach (var g in allctrllist1)
                //{
                    
                if ( allctrllist1.ContainsKey(t.BarCodeId))
                {
                    if (allctrllist1[t.BarCodeId].Item1 != FieldId)
                    {
                        Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                            "条形码有重复。重复条形码为 " + t.BarCodeId + " , 物理地址为： " + t.RtuId + "\r\n集中器" + allctrllist1[t.BarCodeId].Item1 + " , " +
                            allctrllist1[t.BarCodeId].Item2 , WlstMessageBoxType.Ok);
                        return;
                    }
                }

                    //如果列表中存在该条形码 且 是该集中器，则通过 否则
                    //if(list.Contains(t.BarCodeId) )
                    //{
                    //    if(g.Key!=FieldId)
                    //    {
                    //        Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                    //      "条形码有重复。重复条形码为" + t.BarCodeId + "物理地址为：" + t.RtuId+"；集中器"+g.Key+" , "+g.Value.Item2+"为该条形码", WlstMessageBoxType.Ok);
                    //        return;
                    //    }

                    //}
                //}
                if (Is2096)
                {
                    if (t.UplinkTimer % 5 != 0 || t.UplinkTimer < 5 || t.UplinkTimer > 635)
                    {

                        Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                               "主报周期范围为5到635，且必须是5的倍数", WlstMessageBoxType.Ok);
                        return;
                    }
                }

              
            }






            EquSluSgl.ParaFieldSluSgl slusgl =new EquSluSgl.ParaFieldSluSgl();
            slusgl.AreaId = AreaId;
            slusgl.FieldId = FieldId;
            slusgl.PhyId = PhyId;
            slusgl.OtherAttri = OtherAttri;
            slusgl.FieldName = FieldName;
            slusgl.CtrlLst=new List<EquSluSgl.ParaSluCtrl>();

            foreach (var t in ControlParaItems)
            {
                //if (Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.HaveSameBarCode(FieldId,t.BarCodeId))
                //{

                //    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                //            "条形码有重复。重复条形码为："+ t.BarCodeId, WlstMessageBoxType.Ok);
                //    return;
                
                //}


                if (t.CurrentSelectLoopVectorItem1 == null)
                {
                    t.CurrentSelectLoopVectorItem1 = t.LoopVectorItems1.First();
                }
                if (t.CurrentSelectLoopVectorItem2 == null)
                {
                    t.CurrentSelectLoopVectorItem2 = t.LoopVectorItems2.First();
                }
                if (t.CurrentSelectLoopVectorItem3 == null)
                {
                    t.CurrentSelectLoopVectorItem3 = t.LoopVectorItems3.First();
                }
                if (t.CurrentSelectLoopVectorItem4 == null)
                {
                    t.CurrentSelectLoopVectorItem4 = t.LoopVectorItems4.First();
                }

                if (t.CurrentSelectLoopRatePowerIndex1 == null)
                {
                    t.CurrentSelectLoopRatePowerIndex1 = t.LoopRatePowerItems1.First();
                }
                if (t.CurrentSelectLoopRatePowerIndex2 == null)
                {
                    t.CurrentSelectLoopRatePowerIndex2 = t.LoopRatePowerItems2.First();
                }
                if (t.CurrentSelectLoopRatePowerIndex3 == null)
                {
                    t.CurrentSelectLoopRatePowerIndex3 = t.LoopRatePowerItems3.First();
                }
                if (t.CurrentSelectLoopRatePowerIndex4 == null)
                {
                    t.CurrentSelectLoopRatePowerIndex4 = t.LoopRatePowerItems4.First();
                }
                var errrid = 0;
                try
                {
                    //////var ctrlinfo = new EquSluSgl.ParaSluCtrl()
                    //////                   {

                    //////                       BarCodeId = t.BarCodeId,
                    //////                       CtrlId = t.RtuId,
                    //////                       CtrlName = t.LampCode,
                    //////                       IsAlarmAuto = t.IsActiveAlarm,
                    //////                       IsUsed = t.IsRun,
                    //////                       UplinkReply = t.UplinkReply,
                    //////                       UplinkTimer = t.UplinkTimer,
                    //////                       //LightCount = t.CurrentSelectLoopNumItem == null ? 0 : t.CurrentSelectLoopNumItem.Value,
                    //////                       IsAutoOpenLightWhenElec1 = t.IsPowerOnLight1,
                    //////                       IsAutoOpenLightWhenElec2 = t.IsPowerOnLight2,
                    //////                       IsAutoOpenLightWhenElec3 = t.IsPowerOnLight3,
                    //////                       IsAutoOpenLightWhenElec4 = t.IsPowerOnLight4,
                    //////                       VectorLoop1 = t.CurrentSelectLoopVectorItem1.Value,
                    //////                       VectorLoop2 = t.CurrentSelectLoopVectorItem2.Value,
                    //////                       VectorLoop3 = t.CurrentSelectLoopVectorItem3.Value,
                    //////                       VectorLoop4 = t.CurrentSelectLoopVectorItem4.Value,

                    //////                       PowerRate1 = t.CurrentSelectLoopRatePowerIndex1.Value,
                    //////                       PowerRate2 = t.CurrentSelectLoopRatePowerIndex2.Value,
                    //////                       PowerRate3 = t.CurrentSelectLoopRatePowerIndex3.Value,
                    //////                       PowerRate4 = t.CurrentSelectLoopRatePowerIndex4.Value,

                    //////                       UpperPower = t.PowerMax,
                    //////                       LowerPower = t.PowerMin,

                    //////                       RoutePass1 = t.Route1,
                    //////                       RoutePass2 = t.Route2,
                    //////                       RoutePass3 = t.Route3,
                    //////                       RoutePass4 = t.Route4,

                    //////                       OrderId = t.Index,

                    //////                       CtrlGisX = t.Xgis,
                    //////                       CtrlGisY = t.Ygis,

                    //////                       Imei = t.SimsCodeId,
                    //////                       //NBType = t.CurrentCtrlTypeSelected == null ? 0 : t.CurrentCtrlTypeSelected.Value,

                    //////                   };

                    var ctrlinfo = new EquSluSgl.ParaSluCtrl();

                    ctrlinfo.BarCodeId = t.BarCodeId;
                    ctrlinfo.CtrlId = t.RtuId;
                    ctrlinfo.CtrlName = t.LampCode;
                    ctrlinfo.IsAlarmAuto = t.IsActiveAlarm;
                    ctrlinfo.IsUsed = t.IsRun;
                    ctrlinfo.UplinkReply = t.UplinkReply;
                    ctrlinfo.UplinkTimer = t.UplinkTimer;
                    ctrlinfo.LightCount = t.CurrentSelectLoopNumItem == null ? 0 : t.CurrentSelectLoopNumItem.Value;
                    ctrlinfo.IsAutoOpenLightWhenElec1 = t.IsPowerOnLight1;
                    ctrlinfo.IsAutoOpenLightWhenElec2 = t.IsPowerOnLight2;
                    ctrlinfo.IsAutoOpenLightWhenElec3 = t.IsPowerOnLight3;
                    ctrlinfo.IsAutoOpenLightWhenElec4 = t.IsPowerOnLight4;
                    ctrlinfo.VectorLoop1 = t.CurrentSelectLoopVectorItem1.Value;
                    ctrlinfo.VectorLoop2 = t.CurrentSelectLoopVectorItem2.Value;
                    ctrlinfo.VectorLoop3 = t.CurrentSelectLoopVectorItem3.Value;
                    ctrlinfo.VectorLoop4 = t.CurrentSelectLoopVectorItem4.Value;

                    ctrlinfo.PowerRate1 = t.CurrentSelectLoopRatePowerIndex1.Value;
                    ctrlinfo.PowerRate2 = t.CurrentSelectLoopRatePowerIndex2.Value;
                    ctrlinfo.PowerRate3 = t.CurrentSelectLoopRatePowerIndex3.Value;
                    ctrlinfo.PowerRate4 = t.CurrentSelectLoopRatePowerIndex4.Value;

                    ctrlinfo.UpperPower = t.PowerMax;
                    ctrlinfo.LowerPower = t.PowerMin;

                    ctrlinfo.RoutePass1 = t.Route1;
                    ctrlinfo.RoutePass2 = t.Route2;
                    ctrlinfo.RoutePass3 = t.Route3;
                    ctrlinfo.RoutePass4 = t.Route4;

                    ctrlinfo.OrderId = t.Index;

                    ctrlinfo.CtrlGisX = t.Xgis;
                    ctrlinfo.CtrlGisY = t.Ygis;

                    ctrlinfo.Imei = t.SimsCodeId;
                    ctrlinfo.NBType = t.CurrentCtrlTypeSelected == null ? 0 : t.CurrentCtrlTypeSelected.Value;
                    slusgl.CtrlLst.Add(ctrlinfo);
                    errrid++;

                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError("最新数据控制器提取最新数据导出页面出错:" + ex.ToString()+errrid);
                }


            }

            info.WstSlusglEqu.Items.Add(slusgl);
            if (ctrlremove.Count>0)
                info.WstSlusglEqu.FieldOrCtrlLst = ctrlremove;

            

            if (info.WstSlusglEqu.Items.Count == 0 )
            {
                WlstMessageBox.Show("警告", "数据有误", WlstMessageBoxType.Ok);

                return;
            }
            SndOrderServer.OrderSnd(info, 10,0);//6
        }

        private void SaveFieldCtrlGrp()
        {
            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_field_grp_info;
            info.WstSlusglFieldGrpInfo.Op = 3;
            info.WstSlusglFieldGrpInfo.FieldId = FieldId;  
            info.WstSlusglFieldGrpInfo.Items.Clear();

            var dic = new Dictionary <int,Tuple<string,List<int>>>();

            foreach (var i in TreeItems)
            {
                var lst = new List<int>();
                foreach (var t in i.ChildTreeItems)
                {
                    lst.Add(t.NodeId);
                }
                var tu = new Tuple<string, List<int>>(i.NodeName,lst);
                dic.Add(i.NodeId,tu);
            }

            foreach (var i in dic)
            {  
                info.WstSlusglFieldGrpInfo.Items.Add(new GrpFieldSluSglCtrl.GrpFieldSluSglItem()
                                                     {
                                                         CtrlLst = i.Value.Item2,
                                                         FieldId = FieldId,
                                                         GrpId = i.Key,
                                                         GrpName = i.Value.Item1
                                                     });
            }

            SndOrderServer.OrderSnd(info, 10, 6);
        }

        #endregion

        #region CmdSndCtrlPara

        private ICommand _cmdNeCmdSndCtrlPara;

        public ICommand CmdSndCtrlPara
        {
            get
            {
                return _cmdNeCmdSndCtrlPara ??
                       (_cmdNeCmdSndCtrlPara = new RelayCommand(Ex_cmdNeCmdSndCtrlPara, Can_cmdNeCmdSndCtrlPara, true));
            }
        }

        private long dtsndctrl = DateTime.Now.AddDays(-1).Ticks;

        private void Ex_cmdNeCmdSndCtrlPara()
        {
            if (ControlParaItemsOld.Count != ControlParaItems.Count)
            {
                WlstMessageBox.Show("发送控制器参数前请先保存。", WlstMessageBoxType.Ok);
                return;
            }

            for (int i = 0; i < ControlParaItemsOld.Count; i++)
            {
                if (ControlParaItems[i] != ControlParaItemsOld[i])
                {
                   WlstMessageBox.Show("发送控制器参数前请先保存。", WlstMessageBoxType.Ok);
                    return;
                }
            }

            if (!(FieldId == _terminalInformation.FieldId && PhyId == _terminalInformation.PhyId && FieldName == _terminalInformation.FieldName
                && AreaId == _terminalInformation.AreaId))
            {

                WlstMessageBox.Show("发送控制器参数前请先保存。", WlstMessageBoxType.Ok);
                return;
            }

            dtsndctrl = DateTime.Now.Ticks;

            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_zc_or_set;
            info.Args.Cid = _terminalInformation.FieldId;

            info.Args.Addr.Clear();

            bool nochecked = true;
            foreach (var tt in ControlParaItems)
            {
                if (tt.IsChecked)
                {
                    nochecked = false;
                    info.Args.Addr.Add(tt.RtuId);
                }
            }
            if (nochecked)
            {
                WlstMessageBox.Show("无控制器选择", "请勾选控制器，然后下发参数", WlstMessageBoxType.Ok);
                return;
            }

            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private bool Can_cmdNeCmdSndCtrlPara()
        {
            if (Is2096 == false) return false;
            if (ControlParaItems == null) return false;
            int rf = (from t in ControlParaItems where t.IsChecked select t).Count();
            if (rf == 0) return false;
            if (DateTime.Now.Ticks - dtsndctrl > 10 * 10000000 && _terminalInformation != null) return true;

            return false;
        }

        #endregion

    }

    /// <summary>
    /// 控制器属性
    /// </summary>
    public partial class ConcentratorParaInformationViewModel
    {
        private void InitControlViewModel()
        {
            BtnName = ">>>";
            FlagVisi = false;
            StartCtrl = 1;
            foreach (var t in ControlParaItems)
            {
                try
                {
                    t.OnAttriChanged -= this.g_OnAttriChanged;
                }
                catch (Exception ex)
                {

                }
            }
            ControlParaItems.Clear();

            int xCount = this.ControlNum;
            int yCount = _terminalInformation.CtrlLst.Count;

            if (xCount <= yCount)
            {
                if (_terminalInformation != null)
                {
                    var tmps = (from t in _terminalInformation.CtrlLst orderby t.OrderId ascending select t).ToList();
                    for (int i = 0; i < yCount; i++)
                    {
                        AddToItems(tmps[i], i,Is2096);
                    }
                }
            }
            else
            {
                var tmps = (from t in _terminalInformation.CtrlLst orderby t.OrderId ascending select t).ToList();
                int ind = 0;
                foreach (var g in tmps)
                {
                    AddToItems(g, ind,Is2096);
                    ind++;
                }
                int max = 0;
                foreach (var g in _terminalInformation.CtrlLst)
                {
                    if (g.CtrlId > max) max = g.CtrlId;
                }
                max++;
                int index0 = _terminalInformation.CtrlLst.Count;
                index0++;
                int xAdd = xCount - yCount;

                //int maxid = 8000000;
                //var para = SluSglFieldHold.MySlef.Info;
                //foreach (var t in para)
                //{
                //    foreach (var tt in t.Value.CtrlLst)
                //    {
                //        if (tt > maxid)
                //        {
                //            maxid = tt;
                //        }
                //    }
                //}

                for (int i = max; i < max + xAdd; i++)
                {
                    var ntps = new ControlParaItem
                                   {
                                       Is2096 = this.Is2096,
                                       RtuId = 0,
                                       Index = 0,
                                       LightIndex = ControlParaItems.Count + 1,
                                       BarCode = i.ToString().PadLeft(13, '0'),
                                       //"000 000 000 0000",
                                       IsActiveAlarm = true,
                                       UplinkReply = false,
                                       UplinkTimer = 30,
                                       IsRun = true,
                                       PowerMax = 120,
                                       PowerMin = 80,
                                       IsPowerOnLight1 = true,
                                       IsPowerOnLight2 = true,
                                       IsPowerOnLight3 = true,
                                       IsPowerOnLight4 = true,
                                       LampCode = i + "",
                                       IsChecked = false,
                  
                                       

                                   };

                    ControlParaItems.Add(ntps);
                    index0++;
                }
            }

            for (int i = 1; i <= ControlParaItems.Count; i++)
            {
                ControlParaItems[i - 1].Index = i;
            }
            EndCtrl = ControlParaItems.Count;

            foreach (var g in this.ControlParaItems)
            {
                g.OnAttriChanged += new EventHandler<AttriChangedArgs>(g_OnAttriChanged);
            }



            ControlParaItemsOld = new List<ControlParaItem>();
            foreach (var t in ControlParaItems)
            {
                ControlParaItemsOld.Add(t);
            }

            //ControlParaItemsOld = ControlParaItems;
        }

        private void g_OnAttriChanged(object sender, AttriChangedArgs e)
        {
            //throw new NotImplementedException();

            var changedItem = sender as ControlParaItem;
            if (changedItem == null) return;
            foreach (var g in ControlParaItems)
            {
                if (g.IsChecked == false) continue;
                switch (e.AttriIndex)
                {
                    case 1:
                        g.IsActiveAlarm = changedItem.IsActiveAlarm;
                        break;
                    case 2:
                        g.IsRun = changedItem.IsRun;
                        break;
                    case 3:
                        foreach (var t in g.LoopNumItems)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopNumItem.Value)
                                g.CurrentSelectLoopNumItem = t;
                        }
                        break;
                    case 4:
                        g.IsPowerOnLight1 = changedItem.IsPowerOnLight1;
                        break;
                    case 5:
                        g.IsPowerOnLight2 = changedItem.IsPowerOnLight2;
                        break;
                    case 6:
                        g.IsPowerOnLight3 = changedItem.IsPowerOnLight3;
                        break;
                    case 7:
                        g.IsPowerOnLight4 = changedItem.IsPowerOnLight4;
                        break;
                    case 8:
                        foreach (var t in g.LoopVectorItems1)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopVectorItem1.Value)
                                g.CurrentSelectLoopVectorItem1 = t;
                        }
                        break;
                    case 9:
                        foreach (var t in g.LoopVectorItems2)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopVectorItem2.Value)
                                g.CurrentSelectLoopVectorItem2 = t;
                        }
                        break;
                    case 10:
                        foreach (var t in g.LoopVectorItems3)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopVectorItem3.Value)
                                g.CurrentSelectLoopVectorItem3 = t;
                        }
                        break;
                    case 11:
                        foreach (var t in g.LoopVectorItems4)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopVectorItem4.Value)
                                g.CurrentSelectLoopVectorItem4 = t;
                        }
                        break;
                    case 12:
                        foreach (var t in g.LoopRatePowerItems1)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopRatePowerIndex1.Value)
                                g.CurrentSelectLoopRatePowerIndex1 = t;
                        }
                        break;
                    case 13:
                        foreach (var t in g.LoopRatePowerItems2)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopRatePowerIndex2.Value)
                                g.CurrentSelectLoopRatePowerIndex2 = t;
                        }
                        break;
                    case 14:
                        foreach (var t in g.LoopRatePowerItems3)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopRatePowerIndex3.Value)
                                g.CurrentSelectLoopRatePowerIndex3 = t;
                        }
                        break;
                    case 15:
                        foreach (var t in g.LoopRatePowerItems4)
                        {
                            if (t.Value == changedItem.CurrentSelectLoopRatePowerIndex4.Value)
                                g.CurrentSelectLoopRatePowerIndex4 = t;
                        }
                        break;
                    case 16:
                        g.PowerMax = changedItem.PowerMax;
                        break;
                    case 17:
                        g.PowerMin = changedItem.PowerMin;
                        break;
                    case 18:
                        g.UplinkReply = changedItem.UplinkReply;
                        break;
                    case 19:
                        g.UplinkTimer = changedItem.UplinkTimer;
                        break;
                    case 20:
                        foreach (var k in g.CtrlTypeItems)
                        {
                            if (k.Value == changedItem.CurrentCtrlTypeSelected.Value)g.CurrentCtrlTypeSelected = k;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void AddToItems(EquSluSgl.ParaSluCtrl tmps,int index,bool is2096)
        {

            var gf = new ControlParaItem()
            {
                Is2096 = is2096,
                RtuId = tmps.CtrlId,
                Index = tmps.OrderId,
                BarCodeId = tmps.BarCodeId,
                IsActiveAlarm = tmps.IsAlarmAuto,
                UplinkReply=tmps.UplinkReply,
                UplinkTimer=tmps.UplinkTimer,
                IsRun = tmps.IsUsed,
                PowerMax = tmps.UpperPower,
                PowerMin = tmps.LowerPower,
                Route1 = tmps.RoutePass1,
                Route2 = tmps.RoutePass2,
                Route3 = tmps.RoutePass3,
                Route4 = tmps.RoutePass4,
                LampCode = tmps.CtrlName,
                LightIndex = tmps.OrderId,
                Xgis = tmps.CtrlGisX,
                Ygis = tmps.CtrlGisY,
                
                SimsCodeId = tmps.Imei,
            };
            gf.BarCode = string.Format("{0:D13}", gf.BarCodeId);
            gf.SimsCode = string.Format("{0:D15}", gf.SimsCodeId);


            //for (int i = 0; i < 3; i++)
            //{
            //    gf.BarCode = gf.BarCode.Insert(4 * i + 3, " ");
            //}

            foreach (var g in gf.LoopRatePowerItems1)
            {
                if (g.Value == tmps.PowerRate1)
                {
                    gf.CurrentSelectLoopRatePowerIndex1 = g;
                    break;
                }
            }
            foreach (var g in gf.LoopRatePowerItems2)
            {
                if (g.Value == tmps.PowerRate2)
                {
                    gf.CurrentSelectLoopRatePowerIndex2 = g;
                    break;
                }
            }
            foreach (var g in gf.LoopRatePowerItems3)
            {
                if (g.Value == tmps.PowerRate3)
                {
                    gf.CurrentSelectLoopRatePowerIndex3 = g;
                    break;
                }
            }
            foreach (var g in gf.LoopRatePowerItems4)
            {
                if (g.Value == tmps.PowerRate4)
                {
                    gf.CurrentSelectLoopRatePowerIndex4 = g;
                    break;
                }
            }



            foreach (var g in gf.LoopVectorItems1)
            {
                if (g.Value == tmps.VectorLoop1)
                {
                    gf.CurrentSelectLoopVectorItem1 = g;
                    break;
                }
            }
            foreach (var g in gf.LoopVectorItems2)
            {
                if (g.Value == tmps.VectorLoop2)
                {
                    gf.CurrentSelectLoopVectorItem2 = g;
                    break;
                }
            }
            foreach (var g in gf.LoopVectorItems3)
            {
                if (g.Value == tmps.VectorLoop3)
                {
                    gf.CurrentSelectLoopVectorItem3 = g;
                    break;
                }
            }
            foreach (var g in gf.LoopVectorItems4)
            {
                if (g.Value == tmps.VectorLoop4)
                {
                    gf.CurrentSelectLoopVectorItem4 = g;
                    break;
                }
            }

            //lvf  2018年12月14日10:31:00   控制器类型（厂商）
            foreach (var g in gf.CtrlTypeItems)
            {
                if (g.Value == tmps.NBType)
                {
                    gf.CurrentCtrlTypeSelected = g;
                    break;
                }
            }


            gf.IsPowerOnLight1 = tmps.IsAutoOpenLightWhenElec1;
            gf.IsPowerOnLight2 = tmps.IsAutoOpenLightWhenElec2;
            gf.IsPowerOnLight3 = tmps.IsAutoOpenLightWhenElec3;
            gf.IsPowerOnLight4 = tmps.IsAutoOpenLightWhenElec4;

            foreach (var g in gf.LoopNumItems)
            {
                if (g.Value == tmps.LightCount)
                {
                    gf.CurrentSelectLoopNumItem = g;
                    break;
                }
            }
            ControlParaItems.Add(gf);

        }

        public List<SluRegulatorParameter> BackControlViewModelSluCtrl()
        {
            string existInfo = null;
            //existBarCode = false;
            var lst = new List<SluRegulatorParameter>();
            for (int i = 0; i < ControlParaItems.Count; i++)
            {
                for (int j = i + 1; j < ControlParaItems.Count; j++)
                {
                    if (ControlParaItems[i].BarCodeId == ControlParaItems[j].BarCodeId)
                    {
                        //existBarCode = true;
                        existInfo = "第" + ControlParaItems[i].Index + "控制器与第" + ControlParaItems[j].Index + "控制器条形码相同，请重新设置";
                        Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("有条形码相同的控制器", existInfo, UMessageBoxButton.Ok);
                        return null;
                    }
                }
                //if (existBarCode)
                //    break;
            }

            foreach (var g in this.ControlParaItems)
            {
                var info = new SluRegulatorParameter()
                {
                    BarCodeId = g.BarCodeId,
                    OrderId = g.LightIndex,
                    CtrlId = g.RtuId,
                    CtrlPhyId = g.Index,
                    LampCode = g.LampCode,
                    IsAlarmAuto = g.IsActiveAlarm,
                    IsUsed = g.IsRun,
                    LightCount =
                        g.CurrentSelectLoopNumItem == null
                            ? 0
                            : g.CurrentSelectLoopNumItem.Value,
                    IsAutoOpenLightWhenElec1 = g.IsPowerOnLight1,
                    IsAutoOpenLightWhenElec2 = g.IsPowerOnLight2,
                    IsAutoOpenLightWhenElec3 = g.IsPowerOnLight3,
                    IsAutoOpenLightWhenElec4 = g.IsPowerOnLight4,
                    VectorLoop1 =
                        g.CurrentSelectLoopVectorItem1 == null
                            ? 1
                            : g.CurrentSelectLoopVectorItem1.Value,
                    VectorLoop2 =
                        g.CurrentSelectLoopVectorItem2 == null
                            ? 2
                            : g.CurrentSelectLoopVectorItem2.Value,
                    VectorLoop3 =
                        g.CurrentSelectLoopVectorItem3 == null
                            ? 3
                            : g.CurrentSelectLoopVectorItem3.Value,
                    VectorLoop4 =
                        g.CurrentSelectLoopVectorItem4 == null
                            ? 4
                            : g.CurrentSelectLoopVectorItem4.Value,
                    PowerRate1 =
                        g.CurrentSelectLoopRatePowerIndex1 == null
                            ? 0
                            : g.CurrentSelectLoopRatePowerIndex1.Value,
                    PowerRate2 =
                        g.CurrentSelectLoopRatePowerIndex2 == null
                            ? 0
                            : g.CurrentSelectLoopRatePowerIndex2.Value,
                    PowerRate3 =
                        g.CurrentSelectLoopRatePowerIndex3 == null
                            ? 0
                            : g.CurrentSelectLoopRatePowerIndex3.Value,
                    PowerRate4 =
                        g.CurrentSelectLoopRatePowerIndex4 == null
                            ? 0
                            : g.CurrentSelectLoopRatePowerIndex4.Value,
                    RoutePass1 = g.Route1,
                    RtuName = "控制器" + g.RtuId,
                    RoutePass2 = g.Route2,
                    RoutePass3 = g.Route3,
                    RoutePass4 = g.Route4,
                    LowerPower = g.PowerMin,
                    SluId = _terminalInformation.FieldId,
                    UpperPower = g.PowerMax,
                    CtrlGisX = g.Xgis,
                    CtrlGisY = g.Ygis,
                    
                   
                };
                lst.Add(info);
            }
            return lst;
        }
    }

    public partial class ConcentratorParaInformationViewModel
    {
        #region ControlParaItems


        private ObservableCollection<ControlParaItem> _controlParaItems;

        public ObservableCollection<ControlParaItem> ControlParaItems
        {
            get { return _controlParaItems ?? (_controlParaItems = new ObservableCollection<ControlParaItem>()); }
        }


        #endregion

        #region CurrentSelectControlParaItem

        private ControlParaItem _currentSelectControlParaItem;

        public ControlParaItem CurrentSelectControlParaItem
        {
            get { return _currentSelectControlParaItem; }
            set
            {
                if (_currentSelectControlParaItem == value) return;
                _currentSelectControlParaItem = value;
                RaisePropertyChanged(() => CurrentSelectControlParaItem);
                //  _currentSelectControlParaItem = null;
            }
        }

        #endregion



        #region UpMove

        private ICommand _CUpMovemdUp;

        public ICommand UpMove
        {
            get { return _CUpMovemdUp ?? (_CUpMovemdUp = new RelayCommand(ExUpMove, CanUpMove, false)); }
        }

        private void ExUpMove()
        {

            for (int i = 1; i < ControlParaItems.Count; i++)
            {
                if (ControlParaItems[i].IsChecked && ControlParaItems[i - 1].IsChecked == false)
                {
                    var tmpg = ControlParaItems[i - 1];
                    ControlParaItems.RemoveAt(i - 1);
                    ControlParaItems.Insert(i, tmpg);
                }
            } for (int i = 1; i < ControlParaItems.Count + 1; i++)
            {
                ControlParaItems[i - 1].Index = i;
                //ControlParaItems[i - 1].LightIndex  = i;
            }
        }

        private bool CanUpMove()
        {

            if (ControlParaItems.Count == 0) return false;
            if (ControlParaItems[0].IsChecked) return false;
            foreach (var g in ControlParaItems) if (g.IsChecked) return true;
            return false;
        }

        #endregion

        #region DownMove

        private ICommand _cmdDownMove;

        public ICommand DownMove
        {
            get { return _cmdDownMove ?? (_cmdDownMove = new RelayCommand(ExDownMove, CanDownMove, false)); }
        }

        private void ExDownMove()
        {
            for (int i = ControlParaItems.Count - 2; i >= 0; i--)
            {
                if (ControlParaItems[i].IsChecked && ControlParaItems[i + 1].IsChecked == false)
                {
                    var tmpg = ControlParaItems[i];
                    ControlParaItems.RemoveAt(i);
                    ControlParaItems.Insert(i + 1, tmpg);
                }
            }
            for (int i = 1; i < ControlParaItems.Count + 1; i++)
            {
                ControlParaItems[i - 1].Index = i;
                //ControlParaItems[i - 1].LightIndex  = i;
            }
            //Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(ViewIdAssign.ControlInfoSetViewId, ViewIdAssign.ControlInfoSetViewAttachRegion,1);
        }

        private bool CanDownMove()
        {
            if (ControlParaItems.Count == 0) return false;
            if (ControlParaItems[ControlParaItems.Count - 1].IsChecked) return false;
            foreach (var g in ControlParaItems) if (g.IsChecked) return true;
            return false;
        }

        #endregion


        #region DeleteItem

        private ICommand _cmdDeleteItem;

        public ICommand DeleteItem
        {
            get { return _cmdDeleteItem ?? (_cmdDeleteItem = new RelayCommand(Ex_cmdDeleteItem, Can_cmdDeleteItem, false)); }
        }

        private void Ex_cmdDeleteItem()
        {
            if (CurrentSelectControlParaItem == null || CurrentSelectControlParaItem.IsChecked == false) return;
            if (ControlParaItems.Contains(CurrentSelectControlParaItem))
            {
                ControlParaItems.Remove(CurrentSelectControlParaItem);
                CurrentSelectControlParaItem = null;

                for (int i = 1; i < ControlParaItems.Count + 1; i++)
                {
                    ControlParaItems[i - 1].Index = i;
                    //ControlParaItems[i - 1].LightIndex  = i;
                }
                if (ControlNum > 1) ControlNum -= 1;
            }
        }

        private bool Can_cmdDeleteItem()
        {
            return CurrentSelectControlParaItem != null && CurrentSelectControlParaItem.IsChecked;
        }

        #endregion


        private int _startCtrl;
        /// <summary>
        /// 起始控制器
        /// </summary>
        [Required(ErrorMessage = "必填选项")]
        [Range(1, 256, ErrorMessage = "起始控制器在1-256号之间")]
        public int StartCtrl
        {
            get { return _startCtrl; }
            set
            {
                if (_startCtrl != value)
                {
                    _startCtrl = value;
                    this.RaisePropertyChanged(() => this.StartCtrl);
                }
                if (StartCtrl < 1) return;
            }
        }

        private int _endCtrl;
        /// <summary>
        /// 结束控制器
        /// </summary>
        [Required(ErrorMessage = "必填选项")]
        [Range(1, 256, ErrorMessage = "结束控制器在1-256号之间")]
        public int EndCtrl
        {
            get { return _endCtrl; }
            set
            {
                if (_endCtrl != value)
                {

                    if (value > ControlParaItems.Count)
                        value = ControlParaItems.Count;
                    _endCtrl = value;
                    this.RaisePropertyChanged(() => this.EndCtrl);

                }
            }
        }

        #region CmdCtrlSelected

        private ICommand _cmdCtrlSelected;

        public ICommand CmdCtrlSelected
        {
            get { return _cmdCtrlSelected ?? (_cmdCtrlSelected = new RelayCommand(ExCtrlSelected, CanCtrlSelected, false)); }
        }

        private void ExCtrlSelected()
        {
            foreach (var t in ControlParaItems)
            {
                t.IsChecked = false;
            }
            for (int i = StartCtrl - 1; i < EndCtrl; i++)
            {
                ControlParaItems[i].IsChecked = true;
            }
        }

        private bool CanCtrlSelected()
        {
            return true;
        }

        #endregion

        #region CmdCtrlEnd

        private ICommand _cmdCtrlEnd;

        public ICommand CmdCtrlEnd
        {
            get { return _cmdCtrlEnd ?? (_cmdCtrlEnd = new RelayCommand(ExCtrlEnd, CanCtrlEnd, false)); }
        }

        private void ExCtrlEnd()
        {
            foreach (var t in ControlParaItems)
            {
                if (t.IsChecked)
                {
                    t.IsChecked = false;
                    //CurrentSelectControlParaItem = null;
                }

            }
        }

        private bool CanCtrlEnd()
        {
            return true;
        }

        #endregion

        #region CmdCtrlDelete

        private ICommand _cmdCtrlDelete;

        public ICommand CmdCtrlDelete
        {
            get { return _cmdCtrlDelete ?? (_cmdCtrlDelete = new RelayCommand(ExCtrlDelete, CanCtrlDelete, false)); }
        }

        private void ExCtrlDelete()
        {
            if (
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "您将要删除选中的控制器，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
            {
                return;
            }
            for (int i = 0; i < ControlParaItems.Count; i++)
            {
                if (ControlParaItems[i].IsChecked)
                {
                    try
                    {
                        ControlParaItems[i].OnAttriChanged -= this.g_OnAttriChanged;
                    }
                    catch (Exception ex)
                    {

                    }
                    ControlParaItems.RemoveAt(i);
                    ControlNum = ControlNum - 1;
                    i--;
                }
            }
            for (int i = 1; i <= ControlParaItems.Count; i++)
            {
                ControlParaItems[i - 1].Index = i;
            }
        }

        private bool CanCtrlDelete()
        {
            return true;
        }

        #endregion


        #region BtnName

        private string _btnName;

        public string BtnName
        {
            get { return _btnName; }
            set
            {
                if (_btnName == value) return;
                _btnName = value;
                RaisePropertyChanged(() => BtnName);
            }
        }

        #endregion

        #region FlagVisi

        private bool _flagVisi;

        public bool FlagVisi
        {
            get { return _flagVisi; }
            set
            {
                if (_flagVisi == value) return;
                _flagVisi = value;
                RaisePropertyChanged(() => FlagVisi);
            }
        }

        #endregion

        #region CmdStretch

        private ICommand _cmdStretch;

        public ICommand CmdStretch
        {
            get { return _cmdStretch ?? (_cmdStretch = new RelayCommand(ExStretch, CanStrech, false)); }
        }

        private void ExStretch()
        {
            if (!FlagVisi)
            {
                FlagVisi = true;
                BtnName = "<<<";
            }
            else
            {
                FlagVisi = false;
                BtnName = ">>>";
            }
        }

        private bool CanStrech()
        {
            return true;
        }

        #endregion
    }

    public partial class ConcentratorParaInformationViewModel
    {
        private EquSluSgl.ParaFieldSluSgl _terminalInformation = null;

    }


    public partial class ConcentratorParaInformationViewModel
    {

        ObservableCollection<NameIntBool> _isPowerOnLight;

        public ObservableCollection<NameIntBool> SndSelectedItems
        {
            get
            {
                if (_isPowerOnLight == null)
                {
                    _isPowerOnLight = new ObservableCollection<NameIntBool>();
                    for (int i = 1; i < 9; i++)
                    {
                        _isPowerOnLight.Add(
                            new NameIntBool() { IsSelected = false, Name = "", Value = i });
                    }

                }
                return _isPowerOnLight;
            }
        }



        private void InitAciton()
        {
            //ProtocolServer.RegistProtocol(
            //   Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_equ,
            //   OnSndCtrlPara,
            //   typeof(EquSluSgl.ParaSluCtrl), this);

            //ProtocolServer.RegistProtocol(
            //   Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_equ,
            //   OnCtrlUpdate,
            //   typeof(EquSluSgl.ParaSluCtrl), this);

            //ProtocolServer.RegistProtocol(
            //    Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_field_info,
            //    OnFieldUpdate,
            //    typeof(SluSglFieldHold), this);

            //ProtocolServer.RegistProtocol(
            //    Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_field_grp_info,
            //    OnGroupUpdate,
            //    typeof(SluSglFieldGrpHold), this);
        }

        private void OnSndCtrlPara(string sessionid, Wlst.mobile.MsgWithMobile info)
        {
            if (info == null || info.WstSlusglEqu == null) return;
            if (info.WstSlusglEqu.Op != 3) return;

            var data = info.WstSlusglEqu.Items;

            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 控制器参数下发成功.";
        }

        private Tuple<bool,bool> saveflg = new Tuple<bool, bool>(false,false);
        private void OnCtrlUpdate(string sessionid, Wlst.mobile.MsgWithMobile info)
        {
            if (info == null || info.WstSlusglEqu == null) return;
            if (info.WstSlusglEqu.Op !=3 ) return;

            var data = info.WstSlusglEqu.Items;

            saveflg= new Tuple<bool, bool>(true,saveflg.Item2);
            if (data.Count == 0)
            {
                Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 无控制器参数.";
                return;
            }
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 控制器参数更新成功.";

            if (saveflg == new Tuple<bool, bool>(true,true))
            {
                NavOnLoad();
            }

        }

         private void OnFieldUpdate(string sessionid, Wlst.mobile.MsgWithMobile info)
         {
             //if (info == null || info.WstSlusglFieldInfo == null) return;
             //if (info.WstSlusglFieldInfo.Op != 3) return;
             //var data = info.WstSlusglFieldInfo.Items;

             //if (data.Count == 0)
             //{
             //    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 无域参数.";
             //    return;
             //}

             //bool flg = false;
             //foreach (var t in data)
             //{
             //    if (t.FieldId == SingleId)
             //    {
             //        flg = true;
             //    }
             //}

             //saveflg = new Tuple<bool, bool, bool>(saveflg.Item1, true, saveflg.Item3);
             //if (flg) Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 域参数更新成功.";
             //else Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 无当前域参数.";


             //if (saveflg == new Tuple<bool, bool,bool>(true, true,true))
             //{
             //    NavOnLoad();
             //}
         }

         private void OnGroupUpdate(string sessionid, Wlst.mobile.MsgWithMobile info)
         {
             if (info == null || info.WstSlusglFieldGrpInfo == null) return;
             if (info.WstSlusglFieldGrpInfo.Op != 3) return;

             var data = info.WstSlusglFieldGrpInfo.Items;


             if (data.Count == 0)
             {
                 Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 无组信息.";
                 return;
             }

             saveflg = new Tuple<bool, bool>(saveflg.Item1, saveflg.Item2);

             Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 控制器分组信息更新成功.";

             if (saveflg == new Tuple<bool, bool>(true, true))
             {
                 NavOnLoad();
             }

         }

    }


    public partial class ConcentratorParaInformationViewModel
    {
        private void InitGroupViewModel()
        {
            StartNode = 1;
            NodeSpace = 1;
            TreeItems.Clear();
            foreach (var t in ControlParaItems)
            {
                if (t.IsChecked)
                    t.IsChecked = false;
            }
            //对分组子节点 进行数据加载
            var grps = Wlst.Sr.SlusglInfoHold.Services.SluSglFieldGrpHold.MySlef.Get(FieldId);

            var grpsorder = (from t in grps orderby t.GrpId select t).ToList();

            foreach (var t in grpsorder)
            {
                this.TreeItems.Add(new TreeItemGrplViewModel(t,t.GrpId , true));
                //t.AddChild(SingleId);
            }
            foreach (var tt in TreeItems)
            {
                tt.AddChild();
            }
        }

        private List<Wlst.client.SluRegulatorGroupParameter> BackGroupViewModelSluCtrlgrp()
        {
            bool noCtrls = false;
            var updateLst = new List<Wlst.client.SluRegulatorGroupParameter>();
            foreach (var t in TreeItems)
            {
                var grpControlLst = new List<int>();
                foreach (var tt in t.ChildTreeItems)
                {
                    if (tt.NodeId <= ControlParaItems.Count)
                        grpControlLst.Add(tt.NodeId);

                }
                var grpInfo = new Wlst.client.SluRegulatorGroupParameter()
                {

                    GrpId = t.NodeId,
                    GrpName = t.NodeName,
                    CtrlPhyLst = grpControlLst
                };
                if (grpControlLst.Count > 0)
                {
                    updateLst.Add(grpInfo);
                }
                else
                {
                    noCtrls = true;

                }
            }
            if (noCtrls)
            {
                var infos = WlstMessageBox.Show("提示", "分组中没有控制器，不能保存分组信息", WlstMessageBoxType.Close);
            }

            return updateLst;
            //   SrInfo.CtrGrpInfo.MySelf.UpdateGroupInfo(updateLst, SingleId);
        }

        private int _startNode;
        /// <summary>
        /// 开始节点
        /// </summary>
        public int StartNode
        {
            get { return _startNode; }
            set
            {
                if (_startNode != value)
                {
                    _startNode = value;
                    this.RaisePropertyChanged(() => this.StartNode);
                }
            }
        }

        private int _nodeSpace;
        /// <summary>
        /// 节点间隔
        /// </summary>
        public int NodeSpace
        {
            get { return _nodeSpace; }
            set
            {
                if (_nodeSpace != value)
                {
                    _nodeSpace = value;
                    this.RaisePropertyChanged(() => this.NodeSpace);
                }
            }
        }

        private ObservableCollection<TreeItemGrplViewModel> _treeItems;

        /// <summary>
        /// 终端树  根节点
        /// </summary>
        public ObservableCollection<TreeItemGrplViewModel> TreeItems
        {
            get
            {
                if (_treeItems == null)
                    _treeItems = new ObservableCollection<TreeItemGrplViewModel>();
                return _treeItems;
            }
        }

        public void TreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 如果使用附加属性来开启右键选中功能，
            // 那么在这里面的代码发生在TreeViewHelper中的代码之后，逻辑正确
        }

        public void TreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 注意，这里的sender是TreeView
            // 我们需要从e.OriginalSource拿到TreeViewItem
            var item = VisualTreeExtensions.GetTemplatedAncestor<TreeViewItem>(e.OriginalSource as FrameworkElement);
            if (item != null)
            {
                item.IsSelected = true;
            }
        }


        #region CmdExport
        private DateTime _dtCmdExport;
        private ICommand _cmdCmdExport;

        public ICommand CmdExport
        {
            get
            {
                if (_cmdCmdExport == null)
                    _cmdCmdExport = new RelayCommand(ExCmdExport, CanExCmdExport, false);
                return _cmdCmdExport;
            }
        }

        private void ExCmdExport()
        {
            _dtCmdExport = DateTime.Now;
            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("序号");
                lsttitle.Add("条形码");
                lsttitle.Add("IMEI号码");
                lsttitle.Add("设备型号");
                lsttitle.Add("控制器名称");
                lsttitle.Add("主动告警");
                lsttitle.Add("是否应答");
                lsttitle.Add("投运");
                lsttitle.Add("回路数量");
                lsttitle.Add("回路1上电开灯");
                lsttitle.Add("回路2上电开灯");
                lsttitle.Add("回路3上电开灯");
                lsttitle.Add("回路4上电开灯");
                lsttitle.Add("回路1矢量");
                lsttitle.Add("回路2矢量");
                lsttitle.Add("回路3矢量");
                lsttitle.Add("回路4矢量");
                lsttitle.Add("回路1额定功率");
                lsttitle.Add("回路2额定功率");
                lsttitle.Add("回路3额定功率");
                lsttitle.Add("回路4额定功率");
                lsttitle.Add("功率上限(%)");
                lsttitle.Add("功率下限(%)");
                lsttitle.Add("开灯序号");



                var lstobj = new List<List<object>>();

                foreach (var g in ControlParaItems)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.Index);
                    tmp.Add(g.BarCode.ToString().PadLeft(16, '0'));

                    //添加   imei 和设备型号  lvf 2019年4月18日09:09:02 
                    tmp.Add(g.SimsCode);
                    tmp.Add(g.CurrentCtrlTypeSelected.Name);



                    tmp.Add(g.LampCode);

                    var flg = "";
                    if (g.IsActiveAlarm) flg = "是";
                    else flg = "否";
                    tmp.Add(flg);

                    //添加 是否应答
                    flg = "";
                    if (g.UplinkReply) flg = "是";
                    else flg = "否";
                    tmp.Add(flg);


                    flg = "";
                    if (g.IsRun) flg = "是";
                    else flg = "否";
                    tmp.Add(flg);

                    tmp.Add(g.UplinkTimer);

                    flg = "";
                    if (g.UplinkReply) flg = "是";
                    else flg = "否";
                    tmp.Add(flg);

                    tmp.Add(g.CurrentSelectLoopNumItem.Value);

                    if (g.CurrentSelectLoopNumItem.Value > 0)
                    {
                        flg = "";
                        if (g.IsEnableByLoop[0].IsSelected) flg = "是";
                        else flg = "否";
                    }
                    else
                    {
                        flg = "--";
                    }
                    tmp.Add(flg);

                    if (g.CurrentSelectLoopNumItem.Value > 1)
                    {
                        flg = "";
                        if (g.IsEnableByLoop[1].IsSelected) flg = "是";
                        else flg = "否";
                    }
                    else
                    {
                        flg = "--";
                    }
                    tmp.Add(flg);

                    if (g.CurrentSelectLoopNumItem.Value > 2)
                    {
                        flg = "";
                        if (g.IsEnableByLoop[2].IsSelected) flg = "是";
                        else flg = "否";
                    }
                    else
                    {
                        flg = "--";
                    }
                    tmp.Add(flg);

                    if (g.CurrentSelectLoopNumItem.Value > 3)
                    {
                        flg = "";
                        if (g.IsEnableByLoop[3].IsSelected) flg = "是";
                        else flg = "否";
                    }
                    else
                    {
                        flg = "--";
                    }
                    tmp.Add(flg);

                    if (g.CurrentSelectLoopNumItem.Value > 0)
                    {
                        tmp.Add(g.CurrentSelectLoopVectorItem1.Value);
                    }
                    else
                        tmp.Add("--");

                    if (g.CurrentSelectLoopNumItem.Value > 1)
                    {
                        tmp.Add(g.CurrentSelectLoopVectorItem2.Value);
                    }
                    else
                        tmp.Add("--");

                    if (g.CurrentSelectLoopNumItem.Value > 2)
                    {
                        tmp.Add(g.CurrentSelectLoopVectorItem3.Value);
                    }
                    else
                        tmp.Add("--");

                    if (g.CurrentSelectLoopNumItem.Value > 3)
                    {
                        tmp.Add(g.CurrentSelectLoopVectorItem4.Value);
                    }
                    else
                        tmp.Add("--");

                    if (g.CurrentSelectLoopNumItem.Value > 0)
                    {
                        tmp.Add(g.CurrentSelectLoopRatePowerIndex1.Name);
                    }
                    else
                        tmp.Add("--");

                    if (g.CurrentSelectLoopNumItem.Value > 1)
                    {
                        tmp.Add(g.CurrentSelectLoopRatePowerIndex2.Name);
                    }
                    else
                        tmp.Add("--");

                    if (g.CurrentSelectLoopNumItem.Value > 2)
                    {
                        tmp.Add(g.CurrentSelectLoopRatePowerIndex3.Name);
                    }
                    else
                        tmp.Add("--");

                    if (g.CurrentSelectLoopNumItem.Value > 3)
                    {
                        tmp.Add(g.CurrentSelectLoopRatePowerIndex4.Name);
                    }
                    else
                        tmp.Add("--");

                    tmp.Add(g.PowerMax);
                    tmp.Add(g.PowerMin);
                    tmp.Add(g.LightIndex);

                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
            }

        }

        private bool CanExCmdExport()
        {
            if (ControlParaItems.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
            return false;
        }

        #endregion

        #region CmdAddGroup
        private ICommand _cmdAddGroup;

        public ICommand CmdAddGroup
        {
            get { return _cmdAddGroup ?? (_cmdAddGroup = new RelayCommand(ExAddGroup, CanAddGroup, false)); }
        }

        private void ExAddGroup()
        {
            int childId = GetMaxGrpId();
            if (childId > 255) return;

            if (childId == -1) return;
            var gi = new GrpFieldSluSglCtrl.GrpFieldSluSglItem()
            {
                GrpId = childId,
                GrpName = "新控制器分组",
            };
            this.TreeItems.Add(new TreeItemGrplViewModel(gi, _terminalInformation.FieldId, true));
        }

        private bool CanAddGroup()
        {
            return true;
        }

        public int GetMaxGrpId()
        {
            int max = 0;
            foreach (var t in this.TreeItems)
            {
                if (t.NodeType == TreeNodeType.IsGrp)
                {
                    if (t.NodeId > max) max = t.NodeId;
                }
            }
            return max + 1;
        }

        #endregion

        #region CmdCancelGroup
        private ICommand _cmdCancelGroup;

        public ICommand CmdCancelGroup
        {
            get { return _cmdCancelGroup ?? (_cmdCancelGroup = new RelayCommand(ExCancelGroup, CanCancelGroup, false)); }
        }

        private void ExCancelGroup()
        {
            if (
                   Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                       "您将要删除选中控制器分组，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
            {
                return;
            }

            for (int i = TreeItems.Count -1; i >=0 ; i--)
            {
                if (TreeItems[i].IsSelected)
                {
                    if (TreeItems[i].NodeType == TreeNodeType.IsGrp)
                    {
                        TreeItems.RemoveAt(i);
                    }
                }
            }
        }

        private bool CanCancelGroup()
        {
            return true;
        }
        #endregion

        #region CmdAddToGrp
        private ICommand _cmdAddToGrp;

        public ICommand CmdAddToGrp
        {
            get { return _cmdAddToGrp ?? (_cmdAddToGrp = new RelayCommand(ExAddToGrp, CanAddToGrp, false)); }
        }

        private void ExAddToGrp()
        {
            foreach (var t in TreeItems)
            {
                if (t.IsTreeChecked)
                {
                    foreach (var tt in ControlParaItems)
                    {
                        if (tt.IsChecked)
                        {
                            bool find = false;
                            for (int i = 0; i < t.ChildTreeItems.Count; i++)
                            {
                                if (t.ChildTreeItems[i].NodeId == tt.RtuId)
                                {
                                    find = true;
                                }
                            }
                            if (find) continue;

                            var gi = new EquSluSgl.ParaSluCtrl()
                            {
                                CtrlId = tt.RtuId,
                                CtrlName = "" + tt.LampCode,
                                LampCode = tt.LampCode,
                                OrderId = tt.Index
                            };
                            t.ChildTreeItems.Add(new TreeItemGrplViewModel(gi, _terminalInformation.FieldId, false));
                            ////tt.IsChecked = false;
                        }
                    }
                }
            }
        }

        private bool CanAddToGrp()
        {
            bool flag = false;
            foreach (var t in ControlParaItems)
            {
                if (t.IsChecked)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
        #endregion

        #region CmdCancelFromGrp
        private ICommand _cmdCancelFromGrp;

        public ICommand CmdCancelFromGrp
        {
            get { return _cmdCancelFromGrp ?? (_cmdCancelFromGrp = new RelayCommand(ExCancelFromGrp, CanCancelFromGrp, false)); }
        }

        private void ExCancelFromGrp()
        {
            for (int i = TreeItems.Count - 1; i >= 0; i--)
            {
                if (TreeItems[i].IsTreeChecked)
                {
                    //for (int j = TreeItems[i].ChildTreeItems.Count - 1; j >= 0;j-- )
                    //{
                    //    TreeItems[i].ChildTreeItems.RemoveAt(j);
                    //}
                    //TreeItems[i].IsTreeChecked = false;
                    TreeItems.RemoveAt(i);
                }
                else
                {
                    for (int j = TreeItems[i].ChildTreeItems.Count - 1; j >= 0; j--)
                    {
                        if (TreeItems[i].ChildTreeItems[j].IsTreeChecked)
                        {
                            TreeItems[i].ChildTreeItems.RemoveAt(j);
                            //  TreeItems[i].ChildTreeItems[j].IsTreeChecked = false;
                            //  break;
                        }
                    }
                }
            }
        }

        private bool CanCancelFromGrp()
        {
            bool flag = false;
            foreach (var t in TreeItems)
            {
                if (t.IsTreeChecked)
                {
                    flag = true;
                    break;
                }
                else
                {
                    foreach (var tt in t.ChildTreeItems)
                    {
                        if (tt.IsTreeChecked)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
            }
            return flag;
        }
        #endregion

        #region CmdSglNode
        private ICommand _cmdSglNode;

        public ICommand CmdSglNode
        {
            get { return _cmdSglNode ?? (_cmdSglNode = new RelayCommand(ExSglNode, CanSglNode, false)); }
        }

        private void ExSglNode()
        {
            foreach (var t in ControlParaItems)
            {
                t.IsChecked = false;
            }
            for (int i = 0; i < ControlParaItems.Count; i += 2)
            {
                ControlParaItems[i].IsChecked = true;
            }
        }

        private bool CanSglNode()
        {
            return true;
        }
        #endregion

        #region CmdDblNode
        private ICommand _cmdDblNode;

        public ICommand CmdDblNode
        {
            get { return _cmdDblNode ?? (_cmdDblNode = new RelayCommand(ExDblNode, CanDblNode, false)); }
        }

        private void ExDblNode()
        {
            foreach (var t in ControlParaItems)
            {
                t.IsChecked = false;
            }
            for (int i = 1; i < ControlParaItems.Count; i += 2)
            {
                ControlParaItems[i].IsChecked = true;
            }
        }

        private bool CanDblNode()
        {
            return true;
        }
        #endregion

        #region CmdCleanSelected
        private ICommand _cmdCleanSelected;

        public ICommand CmdCleanSelected
        {
            get { return _cmdCleanSelected ?? (_cmdCleanSelected = new RelayCommand(ExCleanSelected, CanCleanSelected, false)); }
        }

        private void ExCleanSelected()
        {
            foreach (var tt in ControlParaItems)
            {
                if (tt.IsChecked)
                {
                    tt.IsChecked = false;
                }
            }
        }

        private bool CanCleanSelected()
        {
            return true;
        }
        #endregion

        #region CmdSelectAll
        private ICommand _cmdSelectAll;

        public ICommand CmdSelectAll
        {
            get { return _cmdSelectAll ?? (_cmdSelectAll = new RelayCommand(ExSelectAll, CanSelectAll, false)); }
        }

        private void ExSelectAll()
        {
            foreach (var tt in ControlParaItems)
            {
                tt.IsChecked = true;
            }
        }

        private bool CanSelectAll()
        {
            return true;
        }
        #endregion

        #region CmdSelected
        private ICommand _cmdSelected;

        public ICommand CmdSelected
        {
            get { return _cmdSelected ?? (_cmdSelected = new RelayCommand(ExSelected, CanSelected, false)); }
        }

        private void ExSelected()
        {
            foreach (var t in ControlParaItems)
            {
                t.IsChecked = false;
            }
            for (int i = StartNode - 1; i < ControlParaItems.Count; i += NodeSpace + 1)
            {
                ControlParaItems[i].IsChecked = true;
            }
        }

        private bool CanSelected()
        {
            return true;
        }
        #endregion

        //#region CmdZcDelay
        //private ICommand _cmdZcDelay;

        //public ICommand CmdZcDelay
        //{
        //    get { return _cmdZcDelay ?? (_cmdZcDelay = new RelayCommand(ExAddsdfsdfGroup, CanAsdfsdfddGroup, false)); }
        //}

        //private void ExAddsdfsdfGroup()
        //{
        //    this.ZcDelayInfo();
        //}

        //private bool CanAsdfsdfddGroup()
        //{

        //    return this.IsZigbee == false;
        //}


        //#endregion
    }

}
