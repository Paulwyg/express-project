using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.ComponentHold;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.Sr.EquipemntLightFault.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.ViewModel;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLnViewModel.Services;
using Wlst.Ux.EquipemntLightFault.Models.Exchange;
using Wlst.Ux.EquipemntLightFault.SendOrderViewModel.ViewModel;
using Wlst.Ux.EquipemntLightFault.Services;
using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;


namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLnViewModel.ViewModel
{



    [Export(typeof (IIEquipmentFaultRecordQueryLnViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentFaultRecordQueryViewModel : ObservableObject, IIEquipmentFaultRecordQueryLnViewModel
    {

        public EquipmentFaultRecordQueryViewModel()
        {
            ClickTime = DateTime.Now;
            InitEvent();
            InitAction();
            DtEndTime = DateTime.Now;
            DtStartTime = DateTime.Now.AddDays(-1);
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(delayEvent, 1);
            //LoadIsShowThisViewOnNewErrArrive();
        }

        private void ntg_OnIsSelectedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            var ggg = sender as OperatorTypeItem;
            if (ggg == null || ggg.Value.Count == 0) return;
            foreach (var g in TypeItems)
            {
                if (g.IsSelected)
                {
                    g.IsShow = true;
                    for (int i = 0; i < g.Value.Count; i++)
                    {
                        g.Value[i].IsShow = true;
                    }
                }
                else
                {
                    g.IsShow = false;
                    g.IsSelectedAll = false;
                    for (int i = 0; i < g.Value.Count; i++)
                    {
                        g.Value[i].IsShow = false;
                        g.Value[i].IsSelected = false;
                    }
                }

            }
            //if(ggg.IsSelected)
            //{
            //    ggg.IsShow = true;
            //    foreach (var g in ggg.Value)
            //    {
            //        //g.IsSelected = ggg.IsSelected; 全选功能取消
            //        g.IsShow = true;
            //    }
            //}

        }

        private void ShowLevel()
        {
            HLbphUpper.Clear();
            if (string.IsNullOrEmpty(hlbplevels))
            {
                delayEvent();
                return;
            }
            var lst = new List<double>();
            var sps = hlbplevels.Split(',');
            if (sps.Count() == 0)
            {
                HLbphUpper.Add(new NameValueIntLevels()
                                   {
                                       Name = "全部",
                                       Value = 0,
                                       Value2 = 100000,
                                       IsSelected = true
                                   });
                return;
            }
        
            foreach (var sp in sps)
            {
                var tp = Convert.ToDouble(sp);
                if (lst.Contains(tp) || tp==0) continue;
                lst.Add(tp);
            }
            lst.Sort();


            for (int i = 0; i <=lst.Count-2; i++)
            {
                HLbphUpper.Add(new NameValueIntLevels()
                {
                    Name = lst[i] + "-" + lst[i + 1],
                    Value = lst[i],
                    Value2 = lst[i + 1],

                    IsSelected = true 
                });

            }

            HLbphUpper.Add(new NameValueIntLevels()
                                   {
                                       Name = lst.Last() + "以上",
                                       Value = lst.Last(),
                                       Value2 = 1000000,
                                  
                                       IsSelected = true
                                   });




        }

        private void LoadXml()
        {
            var infos = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("SystemCommonSetConfg");
            if (infos.ContainsKey("IsShowArgsInErrInfo"))
            {
                if (Convert.ToInt32(infos["IsShowArgsInErrInfo"]) == 1)
                {
                    ArgsInfoVisi = true;
                    ArgsInfoVisiE = false;
                }
                else if (Convert.ToInt32(infos["IsShowArgsInErrInfo"]) == 2)
                {
                    ArgsInfoVisi = true;
                    ArgsInfoVisiE = true;
                }
                else
                {
                    ArgsInfoVisi = false;
                    ArgsInfoVisiE = false;
                }

            }
            else
            {
                ArgsInfoVisi = false;
                ArgsInfoVisiE = false;
            }


           

        }

        private bool If_ManageInfo_Exist()
        {
            var lst = Wlst.Sr.AssetManageInfoHold.Services.LampInfoHold.GetData();

            if (lst.Count != 0)
            {
                return true;
            }

            return false;
        }

        private bool isgaojiselected = false;
        private List<int> lastselectedinfo = new List<int>();
        private string hlbplevels = "";// new List<double>(); 

        public void NavOnLoad(params object[] parsObjects)
        {
            ManageInfoExist = If_ManageInfo_Exist();
            ManageInfoVisi = false;
            ArgsInfoVisi = false;
            ArgsInfoVisiE = false;

            hlbplevels = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3601, 4);
            ShowLevel();
            //lvf  默认 查询全部
            QueryValue = 1;
            QueryValue1 = true;
            //lvf 获取区域信息  并将区域终端存于rtusbelongArea list中   2018年4月9日15:25:14
            getAreaRId();

            if (AreaName.Count > 0) AreaComboBoxSelected = AreaName[0];
            if (AreaName.Count > 1) Visi = Visibility.Visible;
            else Visi = Visibility.Collapsed;

            //是否显示 导航到控制中心  是否有权限  lvf 2018年4月9日15:24:27
            IsUserX = Visibility.Collapsed;
            if (UserInfo.UserLoginInfo.D == true || UserInfo.UserLoginInfo.AreaX.Count != 0)
                IsUserX = Visibility.Visible;

            //todotest();

            LoadXml();
            //是否呈现删除按钮,管理员权限并且勾选选项 才呈现
            CmdDeleteVisi = Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D &&
                            Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3102, 1, false)
                                ? Visibility.Visible
                                : Visibility.Collapsed;
            var lsthase = new List<int>();
            foreach (var f in EquipmentModelComponentHolding.DicEquipmentModels.Values)
            {
                lsthase.Add(f.ModelKey);

            }
            lsthase.Add(0000); //自定义故障 lvf

            CountPreErrs = false;
            _isvmsettime = true;
            this.TimeLong = Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetTimeAlarmLong;
            _isvmsettime = false;
            this.IsShowErrsCal = Sr.EquipmentInfoHolding.Services.Others.IsShowErrsCal;


            //isgaojiselected = IsAdvancedQueryChecked;
            //var count = 0;
            //foreach (var g in TypeItems) count += g.Value.Count;
            //if (count < 5)
            //{
            //    TypeItems.Clear();



            //    var lst = new Dictionary<Tuple<int, int, int, string>, List<Tuple<int, string>>>();

            //    foreach (var g in Sr.EquipemntLightFault.Services.FaultClassisDef.FaultClass)
            //    {
            //        var t = g;
            //        if (!lsthase.Contains(g.Item1)) continue;
            //        if (lst.ContainsKey(g)) continue;
            //        if (g.Item4 != "自定义故障")
            //        {
            //            t =new Tuple<int, int, int, string>(g.Item1,g.Item2,g.Item3,"  "+g.Item4+"  ");
            //        }
            //        else
            //        {
            //            t = new Tuple<int, int, int, string>(g.Item1, g.Item2, g.Item3, g.Item4);
            //        }
            //        lst.Add(t, new List<Tuple<int, string>>());

            //    }

            //    foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            //    {
            //        if (!t.Value.IsEnable) continue;
            //        foreach (var f in lst)
            //        {
            //            if (t.Key >= f.Key.Item2 && t.Key <= f.Key.Item3)
            //            {
            //                if (!f.Value.Contains(new Tuple<int, string>(t.Key, t.Value.FaultNameByDefine)))
            //                    f.Value.Add(new Tuple<int, string>(t.Key, t.Value.FaultNameByDefine));
            //            }
            //        }
            //    }


            //    foreach (var g in lst)
            //    {

            //        var gnt = new OperatorTypeItem()
            //                      {
            //                          IsSelected = false,
            //                          Name = g.Key.Item4,
            //                          Value = new ObservableCollection<NameIntBool>(),
            //                          IsShow =false ,
            //                      };
            //        foreach (var f in g.Value)
            //        {
            //            gnt.Value.Add(new NameIntBool() { IsSelected = false, Name = f.Item2, Value = f.Item1 ,IsShow = false });
            //        }
            //        if (gnt.Value.Count > 0)
            //        {
            //            gnt.OnIsSelectedChanged += new EventHandler(ntg_OnIsSelectedChanged);
            //            TypeItems.Add(gnt);
            //        }
            //    }
            //}


            _dtQuery = DateTime.Now.AddDays(-1);
            IsSingleEquipmentQuery = false;
            IsOldFaultQuery = false;


            GetAllFaultName();


            int rutid = 0;
            SelectedRtus.Clear();


            var ids = parsObjects[0] as List<int>;

            if (ids != null && ids.Count > 1)
            {

                IsAdvancedQueryChecked = true;
                IsSingleEquipmentQuery = true;
                var rtus = (from t in ids
                            where
                                t < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.RtuEnd &&
                                t > Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.RtuStart
                            select t).ToList();
                SelectedRtus.Clear();
                SelectedRtus = rtus;
                if (rtus.Count == 0)
                {
                    this.SelectedRtus.Clear();
                    RtuId = 0;
                    RtuName = "通过终端树勾选终端进行故障查询.";
                }
                else
                {
                    RtuId = SelectedRtus[0];
                    if (
                        !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                             InfoItems.ContainsKey
                             (RtuId))
                        return;
                    var tml =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                            [RtuId];
                    RtuName = tml.RtuName;
                    if (SelectedRtus.Count > 1)
                        RtuName += " [等" + SelectedRtus.Count + "个终端]";
                    Ex();
                }

            }
            else
            {
                try
                {
                    rutid = Convert.ToInt32(parsObjects[0]);

                }
                catch (Exception ex)
                {
                }
                if (rutid > 0)
                {
                    SelectedRtus.Add(rutid);
                    RtuId = rutid;

                    this.IsAdvancedQueryChecked = true;
                    this.IsSingleEquipmentQuery = true;
                    this.Ex();
                }
                IsNewAllQuery = true;

                if (rutid == -1)
                {
                    //  this.IsAdvancedQueryChecked = true;
                    if (_thisViewActive) return;
                    this.Ex(); //todo

                }
                if (rutid == -2)
                {
                    ClickTime = DateTime.Now;
                    if (_thisViewActive) return;
                    this.Ex(); //todo
                }
            }

            _thisViewActive = true;
        }


        #region AreaID

        public void getAreaRId()
        {
            AreaName.Clear();
            AreaName.Add(new AreaInt() {Value = "全部", Key = -1});
            if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {

                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                {
                    var tmlLstOfArea =
                            Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(t.Value.AreaId);
                    if (tmlLstOfArea.Count == 0) continue;
                    string area = t.Value.AreaName;
                    AreaName.Add(new AreaInt()
                                     {Value = t.Value.AreaId.ToString("d2") + "-" + area, Key = t.Value.AreaId});
                }
            }
            else
            {
                foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR)
                {
                    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                    {
                        var tmlLstOfArea =
                            Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(t);
                        if (tmlLstOfArea.Count == 0) continue;
                        string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                        AreaName.Add(new AreaInt() {Value = t.ToString("d2") + "-" + area, Key = t});
                    }
                }
            }


        }

        private static ObservableCollection<AreaInt> _devices;

        public static ObservableCollection<AreaInt> AreaName
        {
            get
            {
                if (_devices == null)
                {
                    _devices = new ObservableCollection<AreaInt>();
                }
                return _devices;
            }

        }

        public class AreaInt : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private int _key;

            public int Key
            {
                get { return _key; }
                set
                {
                    if (_key != value)
                    {
                        _key = value;
                        this.RaisePropertyChanged(() => this.Key);
                    }
                }
            }

            private string _value;

            public string Value
            {
                get { return _value; }
                set
                {
                    if (value != _value)
                    {
                        _value = value;
                        this.RaisePropertyChanged(() => this.Value);
                    }
                }
            }
        }

        private AreaInt _areacomboboxselected;

        public AreaInt AreaComboBoxSelected
        {
            get { return _areacomboboxselected; }
            set
            {
                if (_areacomboboxselected != value)
                {
                    _areacomboboxselected = value;
                    this.RaisePropertyChanged(() => this.AreaComboBoxSelected);
                    if (value == null) return;
                    AreaId = value.Key;

                    GetGrpIdByAreaId();

                    //this.Records.Clear();

                    ////将属于这个区域的终端存入 RtusBelongArea 中，查询时判断是否属于该区域， lvf 2018年4月9日15:26:32
                    //RtusBelongArea.Clear();
                    //Remind = "";
                    //var rtulst = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                    //if (rtulst.Count == 0) return;
                    //RtusBelongArea.AddRange(rtulst);

                }
            }
        }


        #region HLbphUpper  终端火零不平衡报警电流差值上限

        private ObservableCollection<NameValueIntLevels> _HLbphUpper = null;
        /// <summary>
        /// 默认10个
        /// </summary>
        public ObservableCollection<NameValueIntLevels> HLbphUpper
        {
            get
            {
                if (_HLbphUpper == null)
                {
                    _HLbphUpper = new ObservableCollection<NameValueIntLevels>();

                        //_HLbphUpper.Add(new NameValueIntLevels()
                        //{
                        //    Value2 = 0
                        //});

                        
                }
                return _HLbphUpper;
            }
            set
            {
                if (value != _HLbphUpper)
                {
                    _HLbphUpper = value;
                    RaisePropertyChanged(() => this.HLbphUpper);
                }
            }
        }

        #endregion

        public static int AreaId = new int();

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

        #endregion


        #region  Group




        private Visibility _txtgrpVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility GrpVisi
        {
            get { return _txtgrpVisi; }
            set
            {
                if (value != _txtgrpVisi)
                {
                    _txtgrpVisi = value;
                    this.RaisePropertyChanged(() => this.GrpVisi);
                }
            }
        }

        private static ObservableCollection<GroupInt> _grpdevices;

        public static ObservableCollection<GroupInt> GroupName
        {
            get
            {
                if (_grpdevices == null)
                {
                    _grpdevices = new ObservableCollection<GroupInt>();
                }
                return _grpdevices;
            }

        }

        public class GroupInt : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private int _key;

            public int Key
            {
                get { return _key; }
                set
                {
                    if (_key != value)
                    {
                        _key = value;
                        this.RaisePropertyChanged(() => this.Key);
                    }
                }
            }

            private string _value;

            public string Value
            {
                get { return _value; }
                set
                {
                    if (value != _value)
                    {
                        _value = value;
                        this.RaisePropertyChanged(() => this.Value);
                    }
                }
            }
        }

        private GroupInt _grpcomboboxselected;
        private int GrpId;

        public GroupInt GroupComboBoxSelected
        {
            get { return _grpcomboboxselected; }
            set
            {
                if (_grpcomboboxselected != value)
                {
                    _grpcomboboxselected = value;
                    this.RaisePropertyChanged(() => this.GroupComboBoxSelected);
                    if (value == null) return;
                    GrpId = value.Key;
                }
            }
        }


        public void GetGrpIdByAreaId()
        {
            GroupName.Clear();

            if (AreaId == -1) //全部区域
            {
                GrpVisi = Visibility.Collapsed;

            }
            else
            {
                GrpVisi = Visibility.Visible;
                var area = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(AreaId);
                if (area == null) return;
                var grps =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoList(AreaId);
                GroupName.Add(new GroupInt() {Value = "全部", Key = -1});
                if (grps.Count > 0)
                {
                    var grpsTmp = (from t in grps orderby t.GroupId select t).ToList();
                    foreach (var f in grpsTmp)
                    {
                        var grptml =
    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(AreaId,
                                                                                  f.GroupId);
                        if (grptml.Count == 0) continue;


                        GroupName.Add(new GroupInt() {Value = f.GroupName, Key = f.GroupId});
                    }
                }
                GroupComboBoxSelected = GroupName[0];
            }



        }

        #endregion

        public void OnUserHideOrClosing()
        {
            _thisViewActive = false;
            //Records = new ObservableCollection<EquipmentFaultViewModel>();
            //ExportVisi = Visibility.Collapsed;
            //if (Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Count
            //    > 0)
            //    ClickTime =
            //        (from t in
            //             Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.
            //             Values
            //         orderby t.DateCreate descending
            //         select t).ToList()[0].DateCreate;
            //else ClickTime = DateTime.Now;

            //var argss = new PublishEventArgs()
            //                {
            //                    EventType = PublishEventType.Core,
            //                    EventId = EventIdAssign.PushErrNums
            //                };
            //argss.AddParams(0);
            //EventPublish.PublishEvent(argss);
            //_dtEndTime = this.DtEndTime;
            //_dtStartTime = this.DtStartTime;
            this.Records.Clear();
        }

        #region tab

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
            get { return "火零不平衡查询"; }
        }

        public DateTime ClickTime { get; set; }

        #endregion

    }


    /// <summary>
    /// 打印及打印预览
    /// </summary>
    public partial class EquipmentFaultRecordQueryViewModel
    {

        //wyg  打印预览

        #region CmdPrintPriview

        private ICommand _cmdPrintPriview;

        public ICommand CmdPrintPriview
        {
            get
            {
                if (_cmdPrintPriview == null)
                    _cmdPrintPriview = new RelayCommand(ExCmdPrintPriview, CanExPrintPriview, false);
                return _cmdPrintPriview;
            }
        }

        private void ExCmdPrintPriview()
        {
            _dtCmdExport = DateTime.Now;
            try
            {
                var tabletitle = new List<string>();
                tabletitle.Add("地址");
                tabletitle.Add("名称");
                tabletitle.Add("故障回路");
                tabletitle.Add("故障名称");
                tabletitle.Add("发生时间");
                if (IsOldFaultQuery) tabletitle.Add("消除时间");
                tabletitle.Add("备注");
                var table = new List<List<string>>();
                DateTime createtime;
                DateTime removetime;
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                foreach (var g in Records)
                {
                    createtime = Convert.ToDateTime(g.DtCreateTime, dtFormat);
                    var tem = new List<string>();
                    tem.Add(g.PhyId.ToString());
                    tem.Add(g.RtuName);
                    tem.Add(g.RtuLoopName);
                    tem.Add(g.FaultName);
                    tem.Add(createtime.ToString("MM/dd HH:mm:ss"));
                    if (IsOldFaultQuery)
                    {
                        removetime = Convert.ToDateTime(g.DtRemoceTime, dtFormat);
                        tem.Add(removetime.ToString("MM/dd HH:mm:ss"));
                    }
                    tem.Add(g.Remark);
                    table.Add(tem);
                }
                print.Prints.PrintPriview(tabletitle, table, false, "故障统计表",
                                          Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName, "", "");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CanExPrintPriview()
        {
            if (Records.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
        }


        #endregion

        //打印

        #region CmdPrint

        private ICommand _cmdPrint;

        public ICommand CmdPrint
        {
            get
            {
                if (_cmdPrint == null)
                    _cmdPrint = new RelayCommand(ExCmdPrint, CanExPrint, false);
                return _cmdPrint;
            }
        }

        private void ExCmdPrint()
        {
            _dtCmdExport = DateTime.Now;
            try
            {
                var tabletitle = new List<string>();
                tabletitle.Add("地址");
                tabletitle.Add("名称");
                tabletitle.Add("故障回路");
                tabletitle.Add("故障名称");
                tabletitle.Add("发生时间");
                if (IsOldFaultQuery) tabletitle.Add("消除时间");
                tabletitle.Add("备注");
                var table = new List<List<string>>();
                DateTime createtime;
                DateTime removetime;
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                foreach (var g in Records)
                {
                    createtime = Convert.ToDateTime(g.DtCreateTime, dtFormat);
                    var tem = new List<string>();
                    tem.Add(g.PhyId.ToString());
                    tem.Add(g.RtuName);
                    tem.Add(g.RtuLoopName);
                    tem.Add(g.FaultName);
                    tem.Add(createtime.ToString("MM/dd HH:mm:ss"));
                    if (IsOldFaultQuery)
                    {
                        removetime = Convert.ToDateTime(g.DtRemoceTime, dtFormat);
                        tem.Add(removetime.ToString("MM/dd HH:mm:ss"));
                    }
                    tem.Add(g.Remark);
                    table.Add(tem);
                }
                print.Prints.Print(tabletitle, table, false, "故障统计表",
                                   Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName, "", "");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CanExPrint()
        {
            if (Records.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
        }

        #endregion

        #region CmdSendOrder

        private ICommand _cmdSendOrder;

        public ICommand CmdSendOrder
        {
            get { return _cmdSendOrder ?? (_cmdSendOrder = new RelayCommand(ExCmdSendOrder, CanExCmdSendOrder, true)); }
        }

        private void ExCmdSendOrder()
        {
            SendOrderViewModel.ViewModel.SendOrderViewModel.FaultRecord.Clear();

            foreach (var t in Records)
            {

                if (t.IsChecked == true)
                {
                    var inputFaultInfo = new InputFaultRecord
                    {
                        RtuId = t.RtuId,
                        LoopID = t.RtuLoops,
                        PriorityLevel = 10,
                        FaultName = t.FaultName,
                        FaultID = t.FaultId,
                        RtuName = t.RtuName,
                        Time = t.DateCreateId,
                        OrderID = string.IsNullOrEmpty(t.OrderId) ? 0 : Convert.ToUInt64(t.OrderId),
                        
                    };
                    //lvf 2018年9月19日14:05:33   火零不平衡 、漏电 添加电流
                    if (inputFaultInfo.FaultID == 25) inputFaultInfo.Remarks = "火线电流：" + t.V + " ，零线电流：" + t.A+" ，差值： "+t.AUpper;
                    if (inputFaultInfo.FaultID == 45) inputFaultInfo.Remarks = "报警：" + t.V + " ，" + t.A + " ， " + t.AUpper;
                    SendOrderViewModel.ViewModel.SendOrderViewModel.FaultRecord.Add(inputFaultInfo);
                }

            }

            if (SendOrderViewModel.ViewModel.SendOrderViewModel.FaultRecord.Count != 0)
            {
                RegionManage.ShowViewByIdAttachRegionWithArgu(EquipemntLightFault.Services.ViewIdAssign.SendOrderViewId, 0);
            }
        }

        private bool CanExCmdSendOrder()
        {
            return true;
        }

        #endregion

    }








    public partial class EquipmentFaultRecordQueryViewModel
    {
        //多终端选中 协议
        public List<int> SelectedRtus = new List<int>();

        ////选择区域下终端
        //public List<int> RtusBelongArea = new List<int>(); 
    }

    /// <summary>
    /// Field ,Attri, ICommand ,Methods
    /// </summary>
    public partial class EquipmentFaultRecordQueryViewModel
    {

        #region Field

        private static bool _isOnExport = true;

        #endregion

        #region Attri

        #region ExportVisi

        private Visibility _exportVisi = Visibility.Collapsed;

        public Visibility ExportVisi
        {
            get { return _exportVisi; }
            set
            {
                if (value == _exportVisi) return;
                _exportVisi = value;
                RaisePropertyChanged(() => ExportVisi);
            }
        }

        #endregion

        #region IsAdvancedQueryChecked

        private bool _isAdvancedQueryChecked;

        public bool IsAdvancedQueryChecked
        {
            get { return _isAdvancedQueryChecked; }
            set
            {
                if (_isAdvancedQueryChecked != value)
                {
                    _isAdvancedQueryChecked = value;

                    if (!_isAdvancedQueryChecked)
                    {
                        IsSingleEquipmentQuery = false;
                        //foreach (var nameIntBool in TypeItems )
                        //{
                        //    nameIntBool.IsSelected = nameIntBool.Value == 0;
                        //}

                    }
                    RaisePropertyChanged(() => IsAdvancedQueryChecked);

                    if (lastCount == 0) lastCount = DateTime.Now.Ticks;
                    if (DateTime.Now.Ticks - lastCount < 10000000) counxxxxx++;
                    else counxxxxx = 0;
                    if (counxxxxx > 5)
                    {
                        CounterLableDoubleClick = 5;
                    }
                    lastCount = DateTime.Now.Ticks;
                }
            }
        }

        private int counxxxxx = 0;
        private long lastCount = 0;

        #endregion

        #region RtuName

        private string _rtuName;

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    RaisePropertyChanged(() => RtuName);
                }
            }
        }

        #endregion

        #region RtuId

        private int _iphyd;

        public int PhyId
        {
            get { return _iphyd; }
            set
            {
                if (_iphyd != value)
                {
                    _iphyd = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }

        private int _rtuid;

        public int RtuId
        {
            get { return _rtuid; }
            set
            {

                if (value != _rtuid)
                {
                    _rtuid = value;
                    PhyId = value;
                    RaisePropertyChanged(() => RtuId);
                    RtuName = "Reserve";
                    if (
                        !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                             InfoItems.ContainsKey
                             (_rtuid))
                        return;
                    var tml =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                            [_rtuid];
                    RtuName = tml.RtuName;

                    if (tml.RtuFid == 0)
                        PhyId = tml.RtuPhyId;
                    else PhyId = value;
                }
            }
        }

        #endregion

        #region Records

        private ObservableCollection<EquipmentFaultViewModel> _record;

        public ObservableCollection<EquipmentFaultViewModel> Records
        {
            get { return _record ?? (_record = new ObservableCollection<EquipmentFaultViewModel>()); }
            set
            {
                if (_record != value)
                {
                    _record = value;
                    this.RaisePropertyChanged(() => this.Records);
                }
            }
        }

        #endregion

        #region Recordss

        private ObservableCollection<EquipmentFaultViewModel> _records;

        public ObservableCollection<EquipmentFaultViewModel> Recordss
        {
            get { return _records ?? (_records = new ObservableCollection<EquipmentFaultViewModel>()); }
            set
            {
                if (_records != value)
                {
                    _records = value;
                    this.RaisePropertyChanged(() => this.Recordss);
                }
            }
        }

        #endregion


        //#region Levels

        //private ObservableCollection<NameValueIntLevels> _levels;

        ///// <summary>
        ///// 火零不平衡档位
        ///// </summary>
        //public ObservableCollection<NameValueIntLevels> Levels
        //{
        //    get
        //    {
        //        if (_levels == null)
        //        {
        //            _levels = new ObservableCollection<NameValueIntLevels>();
        //            var levels = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3302, 4, );
        //            _levels.Add(new NameValueIntLevels() {Name = "1小时", Value = 1});
        //            _levels.Add(new NameValueIntLevels() {Name = "3小时", Value = 3});
        //            _levels.Add(new NameValueIntLevels() {Name = "6小时", Value = 6});
        //            _levels.Add(new NameValueIntLevels() {Name = "12小时", Value = 12});


        //        }
        //        return _levels;
        //    }



        //}

        //#endregion

        #region ManageInfoVisi

        private bool _ManageInfoVisi = false;

        public bool ManageInfoVisi
        {
            get { return _ManageInfoVisi; }
            set
            {
                if (value == _ManageInfoVisi) return;
                _ManageInfoVisi = value;
                RaisePropertyChanged(() => ManageInfoVisi);
            }
        }

        #endregion

        #region ArgsInfoVisi

        private bool _argsInfoVisi = false;

        public bool ArgsInfoVisi
        {
            get { return _argsInfoVisi; }
            set
            {
                if (value == _argsInfoVisi) return;
                _argsInfoVisi = value;
                RaisePropertyChanged(() => ArgsInfoVisi);
            }
        }

        private bool _argsInfoVisiE = false;

        public bool ArgsInfoVisiE
        {
            get { return _argsInfoVisiE; }
            set
            {
                if (value == _argsInfoVisiE) return;
                _argsInfoVisiE = value;
                RaisePropertyChanged(() => ArgsInfoVisiE);
            }
        }

        #endregion

        #region ManageInfoExist

        private bool _ManageInfoExist = false;

        public bool ManageInfoExist
        {
            get { return _ManageInfoExist; }
            set
            {
                if (value == _ManageInfoExist) return;
                _ManageInfoExist = value;
                RaisePropertyChanged(() => ManageInfoExist);
            }
        }

        #endregion

        #region CurrentSelectRecord

        private EquipmentFaultViewModel _currentSelectRecord;

        public EquipmentFaultViewModel CurrentSelectRecord
        {
            get { return _currentSelectRecord ?? (_currentSelectRecord = new EquipmentFaultViewModel()); }
            set
            {
                if (_currentSelectRecord == value) return;
                _currentSelectRecord = value;
                RaisePropertyChanged(() => CurrentSelectRecord);
            }
        }

        #endregion


        #region QueryValue

        private int _queryValue;
        public static int lowerLimit;
        public static int upperLimit;
        public static int numRemark;

        /// <summary>
        /// 统计值 1：全部 2：0~5   3：5~10   4:10+   lvf  2018年4月9日13:21:17 武汉火零不平衡查询
        /// </summary>
        public int QueryValue
        {
            get { return _queryValue; }
            set
            {
                if (_queryValue != value)
                {
                    _queryValue = value;
                    this.RaisePropertyChanged(() => this.QueryValue);

                    switch (QueryValue)
                    {
                        case 1:
                            lowerLimit = 0;
                            upperLimit = 0;
                            break;
                        case 2:
                            lowerLimit = 0;
                            upperLimit = 5;
                            break;
                        case 3:
                            lowerLimit = 5;
                            upperLimit = 10;
                            break;
                        case 4:
                            lowerLimit = 10;
                            upperLimit = 10;
                            break;
                        default:
                            lowerLimit = 0;
                            upperLimit = 0;
                            break;
                    }


                }
            }
        }


        private bool _queryValue1;

        public bool QueryValue1
        {
            get { return _queryValue1; }
            set
            {
                if (value == _queryValue1) return;
                _queryValue1 = value;
                RaisePropertyChanged(() => QueryValue1);
                GetUpLow();


            }
        }


        private bool _queryValue2;

        public bool QueryValue2
        {
            get { return _queryValue2; }
            set
            {
                if (value == _queryValue2) return;
                _queryValue2 = value;
                RaisePropertyChanged(() => QueryValue2);
                GetUpLow();
            }
        }

        private bool _queryValue3;

        public bool QueryValue3
        {
            get { return _queryValue3; }
            set
            {
                if (value == _queryValue3) return;
                _queryValue3 = value;
                RaisePropertyChanged(() => QueryValue3);
                GetUpLow();
            }
        }

        private void GetUpLow()
        {
            if (QueryValue1 && QueryValue2 == false && QueryValue3 == false) numRemark = 1;
            if (QueryValue1 == false && QueryValue2 && QueryValue3 == false) numRemark = 2;
            if (QueryValue1 == false && QueryValue2 == false && QueryValue3) numRemark = 4;
            if (QueryValue1 && QueryValue2 && QueryValue3 == false) numRemark = 3;
            if (QueryValue1 && QueryValue2 == false && QueryValue3) numRemark = 5;
            if (QueryValue1 == false && QueryValue2 && QueryValue3) numRemark = 6;
            if (QueryValue1 && QueryValue2 && QueryValue3) numRemark = 7;
            //switch (numRemark)
            //{
            //    case 1:
            //        lowerLimit = 0;
            //        upperLimit = 5;
            //        break;
            //    case 2:
            //        lowerLimit = 5;
            //        upperLimit = 10;
            //        break;
            //    case 4:
            //        lowerLimit = 10;
            //        upperLimit = 10;
            //        break;
            //    case 3:
            //        lowerLimit = 0;
            //        upperLimit = 10;
            //        break;
            //    case 5:
            //        lowerLimit = 10;
            //        upperLimit = 5;
            //        break;
            //    case 6:
            //        lowerLimit = 5;
            //        upperLimit = 0;
            //        break;
            //    case 7:
            //        lowerLimit = 0;
            //        upperLimit = 0;
            //        break;
            //    default:
            //        lowerLimit = 0;
            //        upperLimit = 0;
            //        break;
            //}
        }


        #endregion

        private Visibility _userX;

        public Visibility IsUserX
        {
            get { return _userX; }
            set
            {
                if (_userX != value)
                {
                    _userX = value;
                    this.RaisePropertyChanged(() => this.IsUserX);
                }
            }
        }


        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _timeItems = null;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> TimeItems
        {
            get
            {
                if (_timeItems == null)
                {
                    _timeItems = new ObservableCollection<NameValueInt>();
                    //for (int i = 1; i < 24; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() { Name = i + "小时", Value = i });
                    //}
                    //for (int i = 1; i < 94; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() { Name = i + " 天", Value = i * 24 });
                    //}
                    _timeItems.Add(new NameValueInt() {Name = "1小时", Value = 1});
                    _timeItems.Add(new NameValueInt() {Name = "3小时", Value = 3});
                    _timeItems.Add(new NameValueInt() {Name = "6小时", Value = 6});
                    _timeItems.Add(new NameValueInt() {Name = "12小时", Value = 12});

                    _timeItems.Add(new NameValueInt() {Name = "1天", Value = 1*24});
                    _timeItems.Add(new NameValueInt() {Name = "3天", Value = 3*24});
                    _timeItems.Add(new NameValueInt() {Name = "7天", Value = 7*24});
                    _timeItems.Add(new NameValueInt() {Name = "14天", Value = 14*24});
                    _timeItems.Add(new NameValueInt() {Name = "30天", Value = 30*24});
                    _timeItems.Add(new NameValueInt() {Name = "60天", Value = 60*24});
                    _timeItems.Add(new NameValueInt() {Name = "90天", Value = 90*24});
                }
                return _timeItems;
            }
        }

        private NameValueInt _currenttime;

        /// <summary>
        /// 时间统计有效时间  时间为小时 大于等于1 小于 1440 
        /// </summary>
        public NameValueInt CurrentSelectedTime
        {
            get { return _currenttime; }
            set
            {
                if (_currenttime == value) return;
                _currenttime = value;
                this.RaisePropertyChanged(() => this.CurrentSelectedTime);
                if (value != null)
                {
                    TimeLong = value.Value;
                }

            }
        }

        private bool _isvmsettime;
        private int _time;

        /// <summary>
        /// 时间统计有效时间  时间为小时 大于等于1 小于 1440 
        /// </summary>
        public int TimeLong
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    if (value < 1) value = 1;
                    if (value > 1440) value = 1440;
                    _time = value;
                    this.RaisePropertyChanged(() => this.TimeLong);

                    if (_isvmsettime)
                    {
                        bool find = false;
                        foreach (var t in this.TimeItems)
                        {
                            if (t.Value == value)
                            {
                                CurrentSelectedTime = t;
                                find = true;
                                break;
                            }
                        }
                        if (find == false)
                        {
                            foreach (var t in this.TimeItems)
                            {
                                if (t.Value > value)
                                {
                                    CurrentSelectedTime = t;
                                    find = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        #region FaultType

        //private ObservableCollection<NameIntBoolXg> _fauleType;

        //public ObservableCollection<NameIntBoolXg> FaultType
        //{
        //    get { return _fauleType ?? (_fauleType = new ObservableCollection<NameIntBoolXg>()); }
        //}


        private ObservableCollection<OperatorTypeItem> _typeItems;

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<OperatorTypeItem> TypeItems
        {
            get { return _typeItems ?? (_typeItems = new ObservableCollection<OperatorTypeItem>()); }
            set
            {
                if (value == _typeItems) return;
                _typeItems = value;
                this.RaisePropertyChanged(() => TypeItems);
            }
        }

        #endregion

        #region IsSingleEquipmentQuery

        private bool _isSingleEquipmentQuery;

        /// <summary>
        /// 是否为单个终端故障查询
        /// </summary>
        public bool IsSingleEquipmentQuery
        {
            get { return _isSingleEquipmentQuery; }
            set
            {
                if (_isSingleEquipmentQuery != value)
                {
                    _isSingleEquipmentQuery = value;
                    RaisePropertyChanged(() => IsSingleEquipmentQuery);
                }
            }
        }

        #endregion

        #region IsOldFaultQuery

        private bool _isOldFaultQuery;

        /// <summary>
        /// 是否为新故障查询，是否就为历史故障查询
        /// </summary>
        public bool IsOldFaultQuery
        {
            get { return _isOldFaultQuery; }
            set
            {
                if (_isOldFaultQuery == value) return;
                _isOldFaultQuery = value;
                RaisePropertyChanged(() => IsOldFaultQuery);
                Records.Clear();
                OnStartTimeEnableChange();
            }
        }


        private bool _isCountPreErrs;

        /// <summary>
        /// 是否统计历史故障
        /// </summary>
        public bool CountPreErrs
        {
            get { return _isCountPreErrs; }
            set
            {
                if (_isCountPreErrs == value) return;
                _isCountPreErrs = value;
                RaisePropertyChanged(() => CountPreErrs);
            }
        }

        private bool _isLastPreErrs; //todo

        /// <summary>
        /// 是否是最后一条
        /// </summary>
        public bool CountLastPreErrs
        {
            get { return _isLastPreErrs; }
            set
            {
                if (_isLastPreErrs == value) return;
                _isLastPreErrs = value;
                RaisePropertyChanged(() => CountLastPreErrs);
            }
        }

        private bool _isCountErrs;

        /// <summary>
        /// 是否统计故障
        /// </summary>
        public bool CountErrs
        {
            get { return _isCountErrs; }
            set
            {
                if (_isCountErrs == value) return;
                _isCountErrs = value;
                RaisePropertyChanged(() => CountErrs);
            }
        }

        private bool _isCountNewErrs;

        /// <summary>
        /// 是否统计现存故障
        /// </summary>
        public bool CountNewErrs
        {
            get { return _isCountNewErrs; }
            set
            {
                if (_isCountNewErrs == value) return;
                _isCountNewErrs = value;
                RaisePropertyChanged(() => CountNewErrs);
            }
        }

        private bool _isOlIsNewAllQuerydFaultQuery;

        /// <summary>
        /// 
        /// </summary>
        public bool IsNewAllQuery
        {
            get { return _isOlIsNewAllQuerydFaultQuery; }
            set
            {
                if (_isOlIsNewAllQuerydFaultQuery == value) return;
                _isOlIsNewAllQuerydFaultQuery = value;
                RaisePropertyChanged(() => IsNewAllQuery);
                OnStartTimeEnableChange();
            }
        }


        private bool _isOlIsNewIsFaultQueryTimeStartEnableAllQuerydFaultQuery;

        /// <summary>
        /// 
        /// </summary>
        public bool IsFaultQueryTimeStartEnable
        {
            get { return _isOlIsNewIsFaultQueryTimeStartEnableAllQuerydFaultQuery; }
            set
            {
                if (_isOlIsNewIsFaultQueryTimeStartEnableAllQuerydFaultQuery == value) return;
                _isOlIsNewIsFaultQueryTimeStartEnableAllQuerydFaultQuery = value;
                RaisePropertyChanged(() => IsFaultQueryTimeStartEnable);
            }
        }


        private void OnStartTimeEnableChange()
        {
            if (IsOldFaultQuery) IsFaultQueryTimeStartEnable = true;
            else
            {
                if (IsNewAllQuery) IsFaultQueryTimeStartEnable = false;
                else IsFaultQueryTimeStartEnable = true;
            }
        }

        #endregion

        #region DtEndTime

        private DateTime _dtEndTime;

        /// <summary>
        /// 故障查询结束时间
        /// </summary>
        public DateTime DtEndTime
        {
            get { return _dtEndTime; }
            set
            {
                if (_dtEndTime != value)
                {
                    _dtEndTime = value;

                    RaisePropertyChanged(() => DtEndTime);
                }
            }
        }

        #endregion

        #region DtStartTime

        private DateTime _dtStartTime;

        /// <summary>
        /// 故障查询起始时间
        /// </summary>
        public DateTime DtStartTime
        {
            get { return _dtStartTime; }
            set
            {
                if (_dtStartTime != value)
                {
                    _dtStartTime = value;
                    RaisePropertyChanged(() => DtStartTime);
                }
            }
        }

        #endregion

        #region Remind

        private string _remind;

        public string Remind
        {
            get { return _remind; }
            set
            {
                if (value == _remind) return;
                _remind = value;
                RaisePropertyChanged(() => Remind);
            }
        }

        #endregion

        #region IsShowErrsCal

        private bool _isShowErrsCal;

        /// <summary>
        /// 是否呈现故障统计功能 
        /// </summary>
        public bool IsShowErrsCal
        {
            get { return _isShowErrsCal; }
            set
            {
                if (_isShowErrsCal == value) return;
                _isShowErrsCal = value;
                RaisePropertyChanged(() => IsShowErrsCal);
            }
        }

        #endregion



        #endregion

        #region ICommand

        #region CmdQuery

        private DateTime _dtQuery;

        public ICommand CmdQuery
        {
            get { return new RelayCommand(Ex, CanEx, true); }
        }

        private List<int> GetSelectFaultType()
        {
            var ntg = new List<int>();

            foreach (var g in TypeItems)
            {
                foreach (var f in g.Value)
                {
                    if (f.IsSelected && !ntg.Contains(f.Value)) ntg.Add(f.Value);
                }
            }
            return ntg;
        }

        private List<int> GetAllFaultType()
        {
            var ntg = new List<int>();

            foreach (var g in TypeItems)
            {
                foreach (var f in g.Value)
                {
                    ntg.Add(f.Value);
                }
            }
            return ntg;
        }

        private void Ex()
        {
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮
            //Remind = "查询命令已发送...请等待数据反馈！";
            _dtQuery = DateTime.Now;
            Records.Clear();
            if (!GetCheckedInformation()) return;
            this.Records.Clear();
            var tmptype = new List<int>(); //GetSelectFaultType();

            tmptype.Add(25);

            Remind = "";
            CountPreErrs = false;
            CountNewErrs = false;
            CountErrs = false;
            //ArgsInfoVisi = Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowArgsInErrInfo;

            if (IsSingleEquipmentQuery) //单个终端查询
            {
                if (RtuId == 0)
                {
                    UMessageBox.Show("提醒", "未选择终端！", UMessageBoxButton.Ok);
                    return;
                }
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
                if (!IsOldFaultQuery) //新故障查询
                {
                    // if (tmptype.Count == 0) return;
                    if (tmptype.Count == 0) //所有故障
                    {
                        QueryNewErrorSingleFault(RtuId, false);
                    }
                    else //单个故障
                    {
                        // if (tmptype.Contains(0)) tmptype.Remove(0);

                        QueryNewErrorSingleFault(RtuId, tmptype, false);
                    }
                }
                else
                {

                    // if (tmptype.Count == 0) return;
                    if (tmptype.Count == 0) //所有故障
                    {
                        QueryPreErrorSingleFault(RtuId);
                    }
                    else //单个故障
                    {
                        //  if (tmptype.Contains(0)) tmptype.Remove(0);
                        QueryPreErrorSingleFault(RtuId, tmptype);
                    }
                }
            }
            else
            {
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";

                if (!IsOldFaultQuery) //新故障查询
                {
                    //  if (tmptype.Count == 0) return;
                    if (tmptype.Count == 0) //所有故障
                    {
                        //QueryNewErrorAllRtuFault();
                        ResolveNewErrorAllRtuFault(false);
                    }
                    else //单个故障
                    {
                        //if (tmptype.Contains(0)) tmptype.Remove(0);
                        QueryNewErrorAllRtuFault(tmptype, false);
                    }
                }
                else
                {

                    // if (tmptype.Count == 0) return;
                    if (tmptype.Count == 0) //所有故障
                    {
                        QueryPreErrorAllRtuFault();
                    }
                    else //单个故障
                    {
                        //if (tmptype.Contains(0)) tmptype.Remove(0);
                        QueryPreErrorAllRtuFault(tmptype);
                    }
                }
            }



            ManageInfoVisi = ManageInfoExist & (!IsOldFaultQuery) & EquipemntLightFaultSetting.IsShowCQJandDGGH;

            if (ManageInfoVisi == true)
            {
                Write_ManageInfo_To_Records();
            }

            _isOnExport = false;
            ExportVisi = Visibility.Visible;

        }


        private DateTime _dtPreQueryStartTime;
        private DateTime _dtPreQueryEndTime;

        private bool CanEx()
        {
            if (IsOldFaultQuery)
            {
                if (DtStartTime > DtEndTime) return false;
            }
            else
                return (_dtPreQueryEndTime.Ticks != DtEndTime.Ticks || _dtPreQueryStartTime.Ticks != DtStartTime.Ticks) &&
                       DateTime.Now.Ticks - _dtQuery.Ticks > 30000000 && DateTime.Now.Ticks - _dtOneAfter.Ticks > 300000 &&
                       DateTime.Now.Ticks - _dtOneBefore.Ticks > 300000;

            return (_dtPreQueryEndTime.Ticks != DtEndTime.Ticks || _dtPreQueryStartTime.Ticks != DtStartTime.Ticks) &&
                   DateTime.Now.Ticks - _dtQuery.Ticks > 3000000 && DateTime.Now.Ticks - _dtOneAfter.Ticks > 300000 &&
                   DateTime.Now.Ticks - _dtOneBefore.Ticks > 300000;
        }

        #endregion

        #region CmdCountNow

        private DateTime _dtCountNow;

        public ICommand CmdCountNow
        {
            get { return new RelayCommand(ExCountNow, CanExCountNow, true); }
        }

        public List<EquipmentFaultCurr.OneFaultItem> FaultItemsTemp = new List<EquipmentFaultCurr.OneFaultItem>();

        private void ExCountNow()
        {
            CountPreErrs = false;
            CountNewErrs = true;
            CountErrs = true;
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮
            //Remind = "查询命令已发送...请等待数据反馈！";
            _dtQuery = DateTime.Now;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
            Records.Clear();
            Recordss.Clear();
            var tmptype = GetSelectFaultType();
            if (!GetCheckedInformation()) return;

            this.Recordss.Clear();
            if (this.TimeLong == Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetTimeAlarmLong)
            {
                CountNewErrs = false;
                if (IsSingleEquipmentQuery) //单个终端查询
                {
                    if (tmptype.Count == 0) //所有故障
                    {
                        QueryNewErrorSingleFault(RtuId, true);
                    }
                    else //单个故障
                    {
                        // if (tmptype.Contains(0)) tmptype.Remove(0);

                        QueryNewErrorSingleFault(RtuId, tmptype, true);
                    }
                }
                else
                {
                    if (tmptype.Count == 0) //所有故障
                    {
                        //QueryNewErrorAllRtuFault();
                        ResolveNewErrorAllRtuFault(true);
                    }
                    else //单个故障
                    {
                        //if (tmptype.Contains(0)) tmptype.Remove(0);
                        QueryNewErrorAllRtuFault(tmptype, true);
                    }
                }

                return;
            }

            QueryNowErrCount(TimeLong, FaultItemsTemp);
            _isOnExport = false;
            ExportVisi = Visibility.Visible;

        }

        private bool CanExCountNow()
        {
            return (_dtPreQueryEndTime.Ticks != DtEndTime.Ticks || _dtPreQueryStartTime.Ticks != DtStartTime.Ticks) &&
                   DateTime.Now.Ticks - _dtQuery.Ticks > 30000000 && DateTime.Now.Ticks - _dtOneAfter.Ticks > 300000 &&
                   DateTime.Now.Ticks - _dtOneBefore.Ticks > 300000;

        }

        #endregion

        #region CmdCountOld

        private DateTime _dtCountOld;

        public ICommand CmdCountOld
        {
            get { return new RelayCommand(ExCountOld, CanExCountOld, true); }
        }


        private void ExCountOld()
        {
            var tmptype = GetSelectFaultType();
            int timeTmp;
            if ((TimeLong/24) < 1)
            {
                UMessageBox.Show("提醒", "请选择大于1天！", UMessageBoxButton.Ok);
                return;
            }
            else
            {
                timeTmp = TimeLong/24;
            }

            CountPreErrs = true;
            CountErrs = true;
            Records.Clear();
            Recordss.Clear();
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮
            //Remind = "查询命令已发送...请等待数据反馈！";
            _dtQuery = DateTime.Now;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
            if (!GetCheckedInformation()) return;



            if (IsSingleEquipmentQuery) //单个终端查询
            {
                if (RtuId == 0) return;

                {
                    // if (tmptype.Count == 0) return;
                    if (tmptype.Count == 0) //所有故障
                    {
                        Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(SelectedRtus,
                                                                                              DtEndTime.AddDays(-timeTmp),
                                                                                              DtEndTime); //默认统计前7天
                    }
                    else //单个故障
                    {
                        Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(SelectedRtus,
                                                                                              DtEndTime.AddDays(-timeTmp),
                                                                                              DtEndTime,
                                                                                              tmptype);
                        //  if (tmptype.Contains(0)) tmptype.Remove(0);
                        //QueryPreErrorSingleFault(RtuId, tmptype);
                    }
                }
            }
            else
            {

                // if (tmptype.Count == 0) return;
                if (tmptype.Count == 0) //所有故障
                {
                    Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(DtEndTime.AddDays(-timeTmp),
                                                                                          DtEndTime);
                    //QueryPreErrorAllRtuFault();
                }
                else //单个故障
                {
                    Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(DtEndTime.AddDays(-timeTmp),
                                                                                          DtEndTime, tmptype);
                    //if (tmptype.Contains(0)) tmptype.Remove(0);
                    //QueryPreErrorAllRtuFault(tmptype);
                }

            }
            _isOnExport = false;
            ExportVisi = Visibility.Visible;

        }

        private DateTime _dtCountOldStartTime;
        private DateTime _dtCountOldEndTime;

        private bool CanExCountOld()
        {
            return (_dtPreQueryEndTime.Ticks != DtEndTime.Ticks || _dtPreQueryStartTime.Ticks != DtStartTime.Ticks) &&
                   DateTime.Now.Ticks - _dtQuery.Ticks > 30000000 && DateTime.Now.Ticks - _dtOneAfter.Ticks > 300000 &&
                   DateTime.Now.Ticks - _dtOneBefore.Ticks > 300000;
        }

        #endregion

        #region CmdOneAfter

        private DateTime _dtOneAfter;

        public ICommand CmdOneAfter
        {
            get { return new RelayCommand(ExQueryAfter, CanExQueryAfter, true); }
        }


        private void ExQueryAfter()
        {
            CountPreErrs = false;
            //CountLastPreErrs = false;
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮
            //Remind = "查询命令已发送...请等待数据反馈！";
            _dtQuery = DateTime.Now;

            var tmptype = GetSelectFaultType();
            if (tmptype.Count == 0) tmptype = GetAllFaultType();
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
            if (!GetCheckedInformation()) return;
            //this.Records.Clear();
            //if (CountLastPreErrs )
            //{
            //    UMessageBox.Show("提醒", "无数据,这是最后一条数据！", UMessageBoxButton.Ok);
            //    return;
            //}

            if (IsSingleEquipmentQuery)
            {
                if (Records.Count == 0)
                {
                    QueryErrAtSomeTime(RtuId, tmptype, false);
                }
                else
                {
                    if (RtuId == Records[0].RtuId)
                    {
                        DateTime DtTmp = new DateTime();
                        DtTmp = Convert.ToDateTime(Records[0].DtCreateTime).Date.AddDays(1);
                        Sr.EquipemntLightFault.Services.PreErrorServices.RequestErrAtSomeTime(RtuId, DtTmp, tmptype,
                                                                                              false);
                    }
                    else
                    {
                        QueryErrAtSomeTime(RtuId, tmptype, false);
                    }
                }

            }
            else
            {
                RtuId = 0;
                if (Records.Count == 0)
                {
                    QueryErrAtSomeTime(RtuId, tmptype, true);
                }
                else
                {
                    DateTime tmp = new DateTime();
                    tmp = Convert.ToDateTime(Records[0].DtCreateTime).Date.AddDays(1);
                    Sr.EquipemntLightFault.Services.PreErrorServices.RequestErrAtSomeTime(RtuId, tmp, tmptype, false);
                }
            }
            //this.Records.Clear();
            _isOnExport = false;
            ExportVisi = Visibility.Visible;

        }

        private DateTime _dtPreQueryAfterStartTime;
        private DateTime _dtPreQueryAfterEndTime;

        private bool CanExQueryAfter()
        {
            return (_dtPreQueryEndTime.Ticks != DtEndTime.Ticks || _dtPreQueryStartTime.Ticks != DtStartTime.Ticks) &&
                   DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;
        }

        #endregion

        #region CmdOneBefore/

        private DateTime _dtOneBefore;

        public ICommand CmdOneBefore
        {
            get { return new RelayCommand(ExQueryBefore, CanExQueryBefore, true); }
        }


        private void ExQueryBefore()
        {
            CountPreErrs = false;
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮
            //Remind = "查询命令已发送...请等待数据反馈！";
            _dtQuery = DateTime.Now;
            var tmptype = GetSelectFaultType();
            if (tmptype.Count == 0) tmptype = GetAllFaultType();
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
            //Records.Clear();
            if (!GetCheckedInformation()) return;
            //if (CountLastPreErrs)
            //{
            //    UMessageBox.Show("提醒", "无数据，这是第一条数据~", UMessageBoxButton.Ok);
            //    return;
            //}
            if (IsSingleEquipmentQuery)
            {
                if (Records.Count == 0)
                {
                    QueryErrAtSomeTime(RtuId, tmptype, true);
                }
                else
                {
                    if (RtuId == Records[0].RtuId)
                    {
                        DateTime tmp = new DateTime();
                        tmp = Convert.ToDateTime(Records[0].DtCreateTime).Date;
                        Sr.EquipemntLightFault.Services.PreErrorServices.RequestErrAtSomeTime(RtuId, tmp, tmptype, true);
                    }
                    else
                    {
                        QueryErrAtSomeTime(RtuId, tmptype, true);
                    }
                }
            }
            else
            {
                RtuId = 0;
                if (Records.Count == 0)
                {
                    QueryErrAtSomeTime(RtuId, tmptype, true);
                }
                else
                {
                    //if (RtuId == Records[0].RtuId)
                    //{
                    DateTime tmp = new DateTime();
                    tmp = Convert.ToDateTime(Records[0].DtCreateTime).Date.AddDays(-1);
                    Sr.EquipemntLightFault.Services.PreErrorServices.RequestErrAtSomeTime(RtuId, tmp, tmptype, true);
                    //}
                    //else
                    //{
                    //    QueryErrAtSomeTime(RtuId, tmptype, true);
                    //}
                }

                //DtStartTime = DtStartTime.AddDays(-1);
                //Sr.EquipemntLightFault.Services.PreErrorServices.RequestErrAtSomeTime(RtuId, DtStartTime, tmptype, true);
            }
            //Records.Clear();
            _isOnExport = false;
            ExportVisi = Visibility.Visible;

        }

        private DateTime _dtPreQueryBeforeStartTime;
        private DateTime _dtPreQueryBeforeEndTime;

        private bool CanExQueryBefore()
        {
            return (_dtPreQueryEndTime.Ticks != DtEndTime.Ticks || _dtPreQueryStartTime.Ticks != DtStartTime.Ticks) &&
                   DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;
        }

        #endregion


        #region CmdAllSelected

        private ICommand _CmdAllSelected;

        public ICommand CmdAllSelected
        {
            get { return _CmdAllSelected ?? (_CmdAllSelected = new RelayCommand(ExCmdAllSelected, CanCmdAllSelected, true)); }
        }


        private void ExCmdAllSelected()
        {
            if (Records.Count > 0)
            {
                if (Records[0].IsChecked == false)
                {
                    foreach (var t in Records)
                    {
                        t.IsChecked = true;
                    }
                }
                else
                {
                    foreach (var t in Records)
                    {
                        t.IsChecked = false;
                    }
                }
            }
        }


        private bool CanCmdAllSelected()
        {
            return true;
        }

        #endregion


        #region CmdControl

        private bool IsFastControl = false;

        private ICommand _cmdcontrol;

        public ICommand CmdControl
        {
            get
            {
                if (_cmdcontrol == null)
                    _cmdcontrol = new RelayCommand(ExCmdControl, CanCmdControl, false);
                return _cmdcontrol;
            }
        }

        private void ExCmdControl()
        {
            //Wlst.Cr.CoreOne.Models.MenuItemBase.ExNavWithArgs(
            //                   Wlst.Ux.EmergencyDispatch.Services.ViewIdAssign.NavToLdlViewId,
            //                   0);
            var dicR = new ConcurrentDictionary<int, List<int>>();
            //if (tmpList2.Count == 0)
            //{
            //    tmpListChk.Clear();
            //    foreach (var t in ChildTreeItems)
            //    {
            //        var tmp = CalcChkTmls(t);
            //        //if (t.NodeType != TypeOfTabTreeNode.IsTml) continue;
            //        //if (t.IsChecked && !tmpList2.Contains(t)) tmpListChk.Add(t);
            //        foreach (var g in tmp)
            //        {
            //            if ( g.IsChecked &&　!tmpListChk.Contains(g)) tmpListChk.Add(g);
            //        }
            //    }
            //    if (tmpListChk.Count > 0) tmpList2 = tmpListChk;
            //    //return;
            //}
            //if (tmpList2.Count == 0) return;
            if (Records.Count < 0) return;


            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.ControlCenterIsShow)
            {
                WlstMessageBox.Show("警告", "控制中心已经打开，正在处理其他操作，请关闭控制中心界面，重试", WlstMessageBoxType.Ok);
                return;
            }


            foreach (var t in Records) //ChildTreeItemsSearch
            {
                if (t.IsChecked)
                {

                    var tmps =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                            t.RtuId]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (tmps.WjLoops.ContainsKey(t.RtuLoops) == false) continue;
                    var switchId = tmps.WjLoops[t.RtuLoops].SwitchOutputId;
                    if (dicR.ContainsKey(t.RtuId) == false)
                    {
                        var lst = new List<int>();
                        lst.Add(switchId);
                        dicR.TryAdd(t.RtuId, lst);
                    }
                    else
                    {
                        if (dicR[t.RtuId].Contains(switchId) == false)
                        {
                            dicR[t.RtuId].Add(switchId);
                        }

                    }

                }
            }

            if (dicR.Count < 1)
            {

                WlstMessageBox.Show("警告", "您未勾选终端，请勾选终端。", WlstMessageBoxType.Ok);
            }
            else
            {
                RegionManage.ShowViewByIdAttachRegionWithArgu(1102820, dicR);
            }

        }

        private bool CanCmdControl()
        {
            return Records.Count > 0;
        }


        #endregion

        #region CmdAskForSetup

        private ICommand _cmdAskForSetup;
        private DateTime _dtAskForSetupTime;

        public ICommand CmdAskForSetup
        {
            get { return _cmdAskForSetup ?? (_cmdAskForSetup = new RelayCommand(ExCmdAskForSetup, CanCmdAskForSetup, true)); }
        }


        private void ExCmdAskForSetup()
        {
            var info = Sr.ProtocolPhone.LxRtuTime.wst_rtutime_time_table_emerg;
            info.WstRtutimeTimeTableEmerg.Op = 3;
            SndOrderServer.OrderSnd(info, 10, 6);
            _dtAskForSetupTime = DateTime.Now;
        }


        private bool CanCmdAskForSetup()
        {
            return DateTime.Now.Ticks - _dtAskForSetupTime.Ticks > 30000000;
        }

        #endregion

        #endregion

        #region Methods

        private static string GetRtuName(int rtuId)
        {
            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuId))
            {
                return "Unknown";
            }
            var fff =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId];

            return fff.RtuName;

        }

        private bool GetCheckedInformation()
        {
            //if (DtStartTime.AddDays(63) < DtEndTime)
            //{
            //    UMessageBox.Show("提醒", "请重新选择时间，时间需选择在62天以内", UMessageBoxButton.Ok);
            //    //WLSTMessageBox.WpfMessageBox.Show("请重新选择时间，时间需选择在30天以内");
            //    return false;
            //}
            return true;
        }



        #endregion


        #region DeleteFault

        //// EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultDefineSettingViewId
        //private bool GetIsCurrentHasRightToDelete()
        //{
        //    return
        //        Wlst.Cr.CoreOne.Models.MenuItemBase.IsglobalIdHasOperatorRight(
        //            EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultDefineSettingViewId);
        //}

        private int _counterLableDoubleClick;

        public int CounterLableDoubleClick
        {
            get { return _counterLableDoubleClick; }
            set
            {
                if (_counterLableDoubleClick == value) return;

                _counterLableDoubleClick = value;
                if (_counterLableDoubleClick > 2)
                {
                    _counterLableDoubleClick = 0;
                    //lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮
                    //CmdDeleteVisi = Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D ? Visibility.Visible : Visibility.Collapsed;

                }
            }
        }

        private Visibility _cmdDeleteVisi;

        public Visibility CmdDeleteVisi
        {
            get { return _cmdDeleteVisi; }
            set
            {
                if (value == _cmdDeleteVisi) return;
                _cmdDeleteVisi = value;
                this.RaisePropertyChanged(() => this.CmdDeleteVisi);
            }
        }


        #region CmdDelete


        public ICommand CmdDelete
        {
            get { return new RelayCommand(ExDelete, CanExDelete, true); }
        }

        private DateTime _dtDelete;

        private void ExDelete()
        {
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮
            Remind = "删除命令已经发送，1秒后可重新查询...";

            this.DeleteQuery();
            Records.Clear();
        }


        private bool CanExDelete()
        {

            if (this.Records.Count < 1) return false;
            return DateTime.Now.Ticks - _dtDelete.Ticks > 30000000;
        }

        #endregion

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
                lsttitle.Add("终端地址");
                lsttitle.Add("终端名称");
                lsttitle.Add("故障回路");
                lsttitle.Add("故障名称");
                lsttitle.Add("发生时间");
                if (IsOldFaultQuery) lsttitle.Add("消除时间");

                lsttitle.Add("火线电流A");
                lsttitle.Add("零线电流A");
                lsttitle.Add("差值");
                lsttitle.Add("所属分组");
                lsttitle.Add("所属区域");


                lsttitle.Add("备注");
                //if (IsOldFaultQuery) 
                lsttitle.Add("电流上下限备注");


                var lstobj = new List<List<object>>();

                foreach (var g in Records)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.Index);
                    tmp.Add(g.PhyId);
                    tmp.Add(g.RtuName);
                    tmp.Add(g.RtuLoopName);
                    tmp.Add(g.FaultName);
                    tmp.Add(g.DtCreateTime);
                    if (IsOldFaultQuery) tmp.Add(g.DtRemoceTime);

                    tmp.Add(g.V);
                    tmp.Add(g.A);
                    tmp.Add(g.AUpper);
                    tmp.Add(g.GroupName);
                    tmp.Add(g.AreaName);


                    tmp.Add(g.Remark);
                    //if (IsOldFaultQuery)
                    //{
                    try
                    {
                        if (g.FaultName == "回路电流越上限")
                        {
                            var zhi = g.Remark.Substring(0, g.Remark.IndexOf(","));
                            zhi = zhi.Substring(3, zhi.Length - 3);
                            var xian = g.Remark.Substring(g.Remark.IndexOf(",") + 4,
                                                          g.Remark.Length - 4 - g.Remark.IndexOf(","));
                            double dzhi, dxian, chazhi;
                            if (double.TryParse(zhi, out dzhi) && double.TryParse(xian, out dxian))
                            {
                                chazhi = dzhi - dxian;
                                tmp.Add(chazhi.ToString("F3"));
                            }
                            else
                            {
                                tmp.Add("Error");
                            }

                        }
                        else if (g.FaultName == "回路电流越下限")
                        {
                            var zhi = g.Remark.Substring(0, g.Remark.IndexOf(","));
                            zhi = zhi.Substring(3, zhi.Length - 3);
                            var xian = g.Remark.Substring(g.Remark.IndexOf(",") + 4,
                                                          g.Remark.Length - 4 - g.Remark.IndexOf(","));
                            double dzhi, dxian, chazhi;
                            if (double.TryParse(zhi, out dzhi) && double.TryParse(xian, out dxian))
                            {
                                chazhi = dxian - dzhi;
                                tmp.Add(chazhi.ToString("F3"));
                            }
                            else
                            {
                                tmp.Add("Error");
                            }
                        }
                        else
                        {
                            tmp.Add("");
                        }
                    }
                    catch
                    {
                        tmp.Add("Error");
                    }
                    //}

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
            if (Records.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
            return false;
        }

        #endregion



        #endregion
    }


    /// <summary>
    /// Data Query
    /// </summary>
    public partial class EquipmentFaultRecordQueryViewModel
    {
        private void Write_ManageInfo_To_Records()
        {
            var lst = Wlst.Sr.AssetManageInfoHold.Services.LampInfoHold.GetData();

            for (int i = 0; i < Records.Count; i++)
            {
                foreach (var t in lst)
                {
                    if (t.Value.RtuId == Records[i].RtuId)
                    {
                        Records[i].DYGH = t.Value.Dygh;
                        Records[i].CQJ = t.Value.Cqj;

                        break;
                    }
                }
            }
        }

        private void QueryNewErrorSingleFault(int rtuId, List<int> faultIds, bool isCol)
        {

            var sss = new List<FaultInfoBase>();


            if (AreaComboBoxSelected.Key != -1)
            {
                if (IsNewAllQuery)
                {
                    sss = (from gg in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                           where
                               SelectedRtus.Contains(gg.RtuId) && faultIds.Contains(gg.FaultId)
                           select gg).ToList(); //
                }
                else
                {
                    var dts = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 1).Ticks;
                    sss = (from gg in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                           where
                               gg.DateCreate.Ticks > dts && SelectedRtus.Contains(gg.RtuId) &&
                               faultIds.Contains(gg.FaultId)
                           select gg).ToList();
                }

            }
            else
            {


                if (IsNewAllQuery)
                {
                    sss = (from gg in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                           where
                               SelectedRtus.Contains(gg.RtuId) && faultIds.Contains(gg.FaultId)
                           //&&
                           //RtusBelongArea.Contains(gg.RtuId)
                           //是否属于该区域
                           select gg).ToList(); //
                }
                else
                {
                    var dts = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 1).Ticks;
                    sss = (from gg in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                           where
                               gg.DateCreate.Ticks > dts && SelectedRtus.Contains(gg.RtuId) &&
                               faultIds.Contains(gg.FaultId)
                           //&& RtusBelongArea.Contains(gg.RtuId)
                           //是否属于该区域
                           select gg).ToList();
                }
            }
            Recordss.Clear();
            Records.Clear();
            FaultItemsTemp.Clear();

            bool isloopError = false;
            var obss = new List<EquipmentFaultCurr.OneFaultItem>();

            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            //var ff = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetLstInfoByRtuId(FaultWarmType.Rtu,
            //               rtuId);
            int intx = 0;
            foreach (var t in sss)
            {
                //this.AddErrorInfo(t);
                var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                if (error == null) continue;


                //判断 差值是否符合统计值区间 lvf 2018年4月9日14:31:15
                //if (QueryValue >1 && QueryValue<4)
                //{
                //    if (error.AUpper < lowerLimit || error.AUpper >= upperLimit) continue;
                //}
                //else if (QueryValue == 4 && error.AUpper < upperLimit )
                //{
                //    continue;
                //}
                if (IsInTheSection(Math.Abs(error.AUpper)) == false) continue;

                intx++;

                if (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 && error.Aeding < 0.0001 &&
                    error.V < 0.0001)
                {
                    isloopError = false;
                }
                else
                {
                    isloopError = true;
                }

                var dtremocetime = "--";
                var dtcreatetime = "--";
                if (isCol)
                {
                    dtremocetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    dtcreatetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }


                obs.Add(new EquipmentFaultViewModel
                            {
                                Index = intx,
                                DtCreateTime = dtcreatetime,
                                DtRemoceTime = dtremocetime,
                                FaultId = t.FaultId,
                                RtuId = t.RtuId,
                                RtuLoopName = error.RtuLoopName,
                                RtuLoops = t.LoopId,
                                FaultName = error.FaultName,
                                PhyId = error.RtuPhyId,
                                Count = t.AlarmCount,
                                RtuName = error.RtuName,
                                Color = error.Color,
                                Remark = error.Remark,
                                DateCreateId = error.RecordId,
                                DateRemoveId = 0,
                                LampId = error.LampId,
                                A = !isloopError ? "---" : error.A + "",
                                AUpper = !isloopError ? "---" : error.AUpper.ToString("f2") + "",
                                ALower = !isloopError ? "---" : error.ALower.ToString("f2") + "",
                                Aeding = !isloopError ? "---" : error.Aeding.ToString("f2") + "",
                                V = !isloopError ? "---" : error.V + "",
                            });
                obss.Add(new EquipmentFaultCurr.OneFaultItem
                             {
                                 AlarmCount = 0,
                                 FaultId = t.FaultId,
                                 DateCreate = error.DateCreate.Ticks,
                                 LoopId = t.LoopId,
                                 RtuId = t.RtuId,
                                 Remark = error.Remark,
                                 LampId = error.LampId,
                             });

            }
            FaultItemsTemp.AddRange(obss);
            if (!CountNewErrs)
            {

                FilterAreaErrs(obs);
                //if (isCol) Recordss = obs;
                //else Records = obs;

                //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + Records.Count + " 条数据.";
            }
        }

        private void QueryNewErrorSingleFault(int rtuId, bool isCol)
        {

            var sss = new List<FaultInfoBase>();
            if (IsNewAllQuery)
            {
                sss = (from gg in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                       where SelectedRtus.Contains(gg.RtuId)
                       select gg).ToList();
            }
            else
            {
                var dts = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 1).Ticks;
                sss =
                    (from gg in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                     where gg.DateCreate.Ticks > dts && SelectedRtus.Contains(gg.RtuId)
                     select gg).ToList();
            }

            Recordss.Clear();
            Records.Clear();
            FaultItemsTemp.Clear();
            bool isloopError = false;
            var obss = new List<EquipmentFaultCurr.OneFaultItem>();
            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            //var ff = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetLstInfoByRtuId(FaultWarmType.Rtu,
            //                                                                                      rtuId);
            int intx = 0;
            foreach (var t in sss)
            {
                //this.AddErrorInfo(t);
                var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                if (error == null) continue;
                intx++;

                if (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 && error.Aeding < 0.0001 &&
                    error.V < 0.0001)
                {
                    isloopError = false;
                }
                else
                {
                    isloopError = true;
                }

                var dtremocetime = "--";
                var dtcreatetime = "--";
                if (isCol)
                {
                    dtremocetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    dtcreatetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }


                obs.Add(new EquipmentFaultViewModel
                            {
                                Index = intx,
                                DtCreateTime = dtcreatetime,
                                DtRemoceTime = dtremocetime,
                                FaultId = t.FaultId,
                                PhyId = error.RtuPhyId,
                                RtuId = t.RtuId,
                                RtuLoopName = error.RtuLoopName,
                                RtuLoops = t.LoopId,
                                FaultName = error.FaultName,
                                Count = t.AlarmCount,
                                Color = error.Color,
                                RtuName = error.RtuName,
                                Remark = error.Remark,
                                DateCreateId = error.RecordId,
                                DateRemoveId = 0,
                                LampId = error.LampId,

                                A = !isloopError ? "---" : error.A + "",
                                AUpper = !isloopError ? "---" : error.AUpper + "",
                                ALower = !isloopError ? "---" : error.ALower + "",
                                Aeding = !isloopError ? "---" : error.Aeding + "",
                                V = !isloopError ? "---" : error.V + "",
                            });
                obss.Add(new EquipmentFaultCurr.OneFaultItem
                             {
                                 AlarmCount = 0,
                                 FaultId = t.FaultId,
                                 DateCreate = error.DateCreate.Ticks,
                                 LoopId = t.LoopId,
                                 RtuId = t.RtuId,
                                 Remark = error.Remark,
                                 LampId = error.LampId,

                             });

            }
            FaultItemsTemp.AddRange(obss);
            if (!CountNewErrs)
            {
                if (isCol) Recordss = obs;
                else Records = obs;
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + Records.Count + " 条数据.";
            }
        }

        private void ResolveNewErrorAllRtuFault(bool isCol)
        {

            bool isloopError = false;
            var dts = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 1).Ticks;
            FaultItemsTemp.Clear();
            Recordss.Clear();
            Records.Clear();
            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            var obs2 = new ObservableCollection<EquipmentFaultViewModel>();
            var obss = new List<EquipmentFaultCurr.OneFaultItem>();
            int intx = 0;

            var tmox = (from t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                        orderby t.DateCreate descending
                        select t).ToList(); //t.IsShowAtTop descending ,
            foreach (var t in tmox)
            {
                if (IsNewAllQuery == false)
                {
                    if (t.DateCreate.Ticks < dts) continue;
                }
                //this.AddErrorInfo(t);
                var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);

                if (error == null) continue;
                intx++;

                var dtcreatetime = "--";
                var dtremocetime = "--";


                if (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 && error.Aeding < 0.0001 &&
                    error.V < 0.0001)
                {
                    isloopError = false;
                }
                else
                {
                    isloopError = true;
                }

                //if (isCol)
                //{
                //    dtremocetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                //} 
                //else
                //{
                //    dtcreatetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                //}

                //if (error.DateFirst.Ticks > 100)
                //{
                //    dtcreatetime = error.DateFirst.ToString("yyyy-MM-dd HH:mm:ss");
                //    dtremocetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                //}
                //else 
                if (!isCol)
                {
                    dtcreatetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    dtremocetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (isCol)
                {
                    obs.Add(new EquipmentFaultViewModel
                                {
                                    Index = intx,
                                    DtCreateTime = dtcreatetime,
                                    DtRemoceTime = dtremocetime,
                                    FaultId = t.FaultId,
                                    RtuId = t.RtuId,
                                    RtuLoopName = error.RtuLoopName,
                                    RtuLoops = t.LoopId,
                                    PhyId = error.RtuPhyId,
                                    FaultName = error.FaultName,
                                    Count = t.AlarmCount,
                                    Color = error.Color,
                                    RtuName = error.RtuName,
                                    Remark = error.Remark,
                                    DateCreateId = error.RecordId,
                                    DateRemoveId = 0,

                                    A = !isloopError ? "---" : error.A + "",
                                    AUpper = !isloopError ? "---" : error.AUpper + "",
                                    ALower = !isloopError ? "---" : error.ALower + "",
                                    Aeding = !isloopError ? "---" : error.Aeding + "",
                                    V = !isloopError ? "---" : error.V + "",
                                });
                }
                else
                {
                    obs.Add(new EquipmentFaultViewModel
                                {
                                    Index = intx,
                                    DtCreateTime = dtcreatetime,
                                    DtRemoceTime = dtremocetime,
                                    FaultId = t.FaultId,
                                    RtuId = t.RtuId,
                                    RtuLoopName = error.RtuLoopName,
                                    RtuLoops = t.LoopId,
                                    PhyId = error.RtuPhyId,
                                    FaultName = error.FaultName,
                                    Count = t.AlarmCount,
                                    Color = error.Color,
                                    RtuName = error.RtuName,
                                    Remark = error.Remark,
                                    DateCreateId = error.RecordId,
                                    DateRemoveId = 0,
                                    A = !isloopError ? "---" : error.A + "",
                                    AUpper = !isloopError ? "---" : error.AUpper + "",
                                    ALower = !isloopError ? "---" : error.ALower + "",
                                    Aeding = !isloopError ? "---" : error.Aeding + "",
                                    V = !isloopError ? "---" : error.V + "",
                                });

                }
                obss.Add(new EquipmentFaultCurr.OneFaultItem
                             {
                                 AlarmCount = 0,
                                 FaultId = t.FaultId,
                                 DateCreate = error.DateCreate.Ticks,
                                 LoopId = t.LoopId,
                                 RtuId = t.RtuId,
                                 Remark = error.Remark,
                                 LampId = error.LampId,

                                 A = error.A,
                                 AUpper = error.AUpper,
                                 ALower = error.ALower,
                                 Aeding = error.Aeding,
                                 V = error.V,
                             }
                    );
            }
            FaultItemsTemp.AddRange(obss);
            if (!CountPreErrs)
            {
                if (isCol)
                {
                    Recordss = obs;
                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + Recordss.Count + " 条数据.";

                }
                else
                {
                    Records = obs;
                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + Records.Count + " 条数据.";

                }
            }

        }



        private void QueryNewErrorAllRtuFault(List<int> faultIds, bool isCol)
        {
            Recordss.Clear();
            Records.Clear();
            FaultItemsTemp.Clear();

            bool isloopError = false;
            var obss = new List<EquipmentFaultCurr.OneFaultItem>();

            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            int intx = 0;
            var dts = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 1).Ticks;
            foreach (var t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values)
            {



                if (!faultIds.Contains(t.FaultId)) continue;

                //if (AreaComboBoxSelected.Key != -1)
                //{
                //    //判断 终端是否属于所选区域
                //    if (!RtusBelongArea.Contains(t.RtuId)) continue;

                //}


                if (IsNewAllQuery == false)
                {
                    if (t.DateCreate.Ticks < dts) continue;
                }
                //this.AddErrorInfo(t);
                var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                if (error == null) continue;

                //判断 差值是否符合统计值区间 lvf 2018年4月9日14:31:15
                //if (QueryValue > 1 && QueryValue < 4)
                //{
                //    if (error.AUpper < lowerLimit || error.AUpper >= upperLimit) continue;
                //}
                //else if (QueryValue == 4 && error.AUpper < upperLimit)
                //{
                //    continue;
                //}
                if (IsInTheSection(Math.Abs(error.AUpper)) == false) continue;

                int rtuphyid;
                //if (error.FaultId == 48) rtuphyid = error.RtuFhyId;
                //else rtuphyid = error.RtuPhyId;

                if (error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 && error.Aeding < 0.0001 &&
                    error.V < 0.0001)
                {
                    isloopError = false;
                }
                else
                {
                    isloopError = true;
                }


                intx++;

                var dtremocetime = "--";
                var dtcreatetime = "--";
                if (isCol)
                {
                    dtremocetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    dtcreatetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                var tmpobs = new EquipmentFaultViewModel
                                 {
                                     Index = intx,
                                     DtCreateTime = dtcreatetime,
                                     DtRemoceTime = dtremocetime,
                                     FaultId = t.FaultId,
                                     RtuId = t.RtuId,
                                     RtuLoopName = error.RtuLoopName,
                                     RtuLoops = t.LoopId,
                                     PhyId = error.RtuPhyId,
                                     FaultName = error.FaultName,
                                     Count = t.AlarmCount,
                                     Color = error.Color,
                                     RtuName = error.RtuName,
                                     Remark = error.Remark,
                                     DateCreateId = error.RecordId,
                                     DateRemoveId = 0,
                                     A = !isloopError ? "---" : error.A.ToString("f2") + "",
                                     AUpper = !isloopError ? "---" : error.AUpper.ToString("f2") + "",
                                     ALower = !isloopError ? "---" : error.ALower.ToString("f2") + "",
                                     Aeding = !isloopError ? "---" : error.Aeding.ToString("f2") + "",
                                     V = !isloopError ? "---" : error.V.ToString("f2") + "",
                                 };


                var tmps =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                        t.RtuId]
                    as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if (tmps == null) continue;
                if (tmps.WjLoops.ContainsKey(t.LoopId) == false) continue;
                //var lopname = tmps.WjLoops[item.LoopId].LoopName;
                var looopname = tmps.WjLoops[t.LoopId].LoopName;
                if (looopname.Substring(looopname.Length - 2) == "火线")
                    looopname = looopname.Substring(0, looopname.Length - 2);
                tmpobs.RtuLoopName = looopname;
                //获取终端分组信息
                var groupInfo = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(t.RtuId);
                if (groupInfo != null)
                {
                    var infosss =
                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.
                            GetGroupInfomation(
                                groupInfo.Item1, groupInfo.Item2);
                    if (infosss != null)
                        tmpobs.GroupName = infosss.GroupName; // +" - " + infosss.GroupId;

                    //  if (infosss != null) DtRtuMsg += infosss.GroupName + " - ";

                }
                else
                {
                    tmpobs.GroupName = "特殊终端";
                }
                //获取终端所属区域信息
                var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(t.RtuId);
                var areaInfo = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(areaId);
                if (areaInfo == null)
                {
                    tmpobs.AreaName = "无区域信息";
                }
                else
                {
                    tmpobs.AreaName = areaInfo.AreaId + " - " + areaInfo.AreaName;
                }
                obs.Add(tmpobs);

                obss.Add(new EquipmentFaultCurr.OneFaultItem
                             {
                                 AlarmCount = 0,
                                 FaultId = t.FaultId,
                                 DateCreate = error.DateCreate.Ticks,
                                 LoopId = t.LoopId,
                                 RtuId = t.RtuId,
                                 Remark = error.Remark,
                                 LampId = error.LampId,

                                 A = error.A,
                                 AUpper = error.AUpper,
                                 ALower = error.ALower,
                                 Aeding = error.Aeding,
                                 V = error.V,
                             });

            }






            FaultItemsTemp.AddRange(obss);
            if (!CountNewErrs)
            {
                FilterAreaErrs(obs);
                //if (isCol) Recordss = obs;
                //else Records = obs;
                //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + Records.Count + " 条数据.";
            }
        }


        private void QueryPreErrorSingleFault(int rtuId, List<int> faultIds)
        {
            Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(SelectedRtus, DtStartTime, DtEndTime,
                                                                                  faultIds);
        }

        private void QueryPreErrorSingleFault(int rtuId)
        {
            Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(SelectedRtus, DtStartTime, DtEndTime);
        }

        private void QueryPreErrorAllRtuFault()
        {
            Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(DtStartTime, DtEndTime);

        }

        private void QueryPreErrorAllRtuFault(List<int> faultIds)
        {
            Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(DtStartTime, DtEndTime, faultIds);
        }

        private void QueryNowErrCount(int dt, List<EquipmentFaultCurr.OneFaultItem> lstOneFaultItem)
        {
            Sr.EquipemntLightFault.Services.PreErrorServices.RequestErrCountBetweenSomeTime(dt, lstOneFaultItem);
        }

        private void QueryErrAtSomeTime(int rtuId, List<int> faultIds, bool isPre)
        {
            Sr.EquipemntLightFault.Services.PreErrorServices.RequestErrAtSomeTime(rtuId, DtStartTime, faultIds, isPre);
        }

        private void DeleteQuery()
        {
            if (this.Records.Count == 0) return;

            var nt = Wlst.Sr.ProtocolPhone.LxFault.wlst_delete_falut_cur;
                //.ProtocolCnt.ServerPart.wlst_EquipemntLightFault_clinet_delete_curFault;
            foreach (var t in this.Records)
            {


                nt.WstFaultDeleteCurr.DeleteItems.Add(new EquipmentFaultDelete.EquipmentFaultDeleteItem()
                                                          {
                                                              FaultCode = t.FaultId,
                                                              LoopId = t.RtuLoops,
                                                              RtuId = t.RtuId,
                                                              LampId = t.LampId
                                                          });
            }


            Wlst.Sr.PPPandSocketSvr.Server.SocketClient.SndData(nt);
        }

        //private void OnPreDataBack(EquipmentPreFaultExChange info)
        //{
        //    int index = 1;
        //    Records.Clear();
        //    var obs = new ObservableCollection<EquipmentFaultViewModel>();
        //    foreach (var t in info.Info)
        //    {

        //      var mtpsss = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(RtuId);




        //        var itemsss = new EquipmentFaultViewModel
        //                          {
        //                              DtCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
        //                              DtRemoceTime = t.DateRemove.ToString("yyyy-MM-dd HH:mm:ss"),
        //                              FaultId = t.FaultCodeId,
        //                              FaultName = "",

        //                              Index = index,
        //                              RtuId = t.RtuId,
        //                              PhyId = mtpsss == null ? 0 : mtpsss.PhyId,
        //                              RtuLoopName = mtpsss == null ? t.LoopId + "" : mtpsss.GetRtuLoopName( t.LoopId ),
        //                              RtuLoops = t.LoopId,
        //                              RtuName = mtpsss == null ? "" : mtpsss.RtuName,
        //                              Remark = t.Remark,

        //                              DateCreateId = t.RecordAlarmId,
        //                              DateRemoveId = t.RecordRemoveId,
        //                              LampId = t.LampId
        //                          };
        //        var typ = Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetInfoById(t.FaultCodeId);
        //        if (typ != null)
        //        {
        //            itemsss.FaultName = typ.FaultNameByDefine;
        //            itemsss.Color = typ.Color;
        //        }
        //        if (mtpsss == null)
        //        {


        //            itemsss.RtuName = GetRtuName(t.RtuId);
        //            itemsss.PhyId =
        //                Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetPhysicalIdByLogicalId(t.RtuId);
        //        }
        //        obs.Add(itemsss);
        //        index++;
        //    }
        //    Records = obs;
        //    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + Records.Count + " 条数据.";
        //}
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class EquipmentFaultRecordQueryViewModel
    {


        public void OnRequestServerData(EquipmentFaultViewModel info)
        {
            if (info == null) return;
            Sr.EquipemntLightFault.Services.PreErrorServices.RequestDataWhenErrorHappen(info.RtuId, info.RtuLoops,
                                                                                        info.DateCreateId);

            //发布事件  选中当前节点
            var args = new PublishEventArgs
                           {
                               EventType = PublishEventType.Core,
                               EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                               EventAttachInfo = "RequestDataWhenErrorHappenEqu",
                           };

            args.AddParams(info.RtuId);
            EventPublish.PublishEvent(args);
        }

        private void InitEvent()
        {
            EventPublish.AddEventTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);

        }



        #region EventSubScriptionTokener

        private bool _thisViewActive;

        public void FundEventHandler(PublishEventArgs args) // should do somework
        {


            try
            {
                if (args.EventType == PublishEventType.Core)
                {


                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {
                        //  
                        if (OptionXmlSvr.GetOptionInt(4001, 2) == 1) return;
                        if (!_thisViewActive) return;
                        if (args.EventAttachInfo == "RequestDataWhenErrorHappenEqu") return;
                        int id = Convert.ToInt32(args.GetParams()[0]);
                        if (id < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.RtuStart) return;
                        if (RtuId == id) return;

                        SelectedRtus.Clear();
                        SelectedRtus.Add(id);
                        RtuId = id;

                        if (IsSingleEquipmentQuery)
                        {

                            Records.Clear();
                            CountLastPreErrs = false;

                            Ex();
                        }

                        //if (!IsOldFaultQuery)
                        //{
                        //    Ex();
                        //}else
                        //{
                        //     if (IsSingleEquipmentQuery) Ex();
                        //}
                    }

                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentMulSelected)
                    {
                        if (!_thisViewActive) return;
                        if (OptionXmlSvr.GetOptionInt(4001, 2) != 1) return;
                        // if (args.EventAttachInfo == "RequestDataWhenErrorHappenEqu") return;
                        var ids = args.GetParams()[0] as List<int>;

                        var rtus = (from t in ids
                                    where
                                        t < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.RtuEnd &&
                                        t > Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.RtuStart
                                    select t).ToList();
                        SelectedRtus.Clear();
                        SelectedRtus = rtus;
                        if (rtus.Count == 0)
                        {
                            this.SelectedRtus.Clear();
                            RtuId = 0;
                            RtuName = "通过终端树勾选终端进行故障查询.";
                        }
                        else
                        {
                            RtuId = SelectedRtus[0];
                            if (
                                !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                                     InfoItems.ContainsKey
                                     (RtuId))
                                return;
                            var tml =
                                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                                    [RtuId];
                            RtuName = tml.RtuName;
                            if (SelectedRtus.Count > 1)
                                RtuName += " [等" + SelectedRtus.Count + "个终端]";

                        }

                        //if (RtuId == id) return;


                        //if (IsSingleEquipmentQuery)
                        //{
                        //    RtuId = id;
                        //    Records.Clear();
                        //    CountLastPreErrs = false;

                        //    Ex();
                        //}

                        //if (!IsOldFaultQuery)
                        //{
                        //    Ex();
                        //}else
                        //{
                        //     if (IsSingleEquipmentQuery) Ex();
                        //}
                    }
                    //if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.EquipmentExistFaultAddId)
                    //{
                    //    if (Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowNewErrArriveOnUi)
                    //    {
                    //        if (args.GetParams().Count > 0)
                    //        {
                    //            var rtx = args.GetParams()[0] as List<int>;
                    //            if (rtx == null) return;

                    //            if (rtx.Count > 1)
                    //            {
                    //                if (Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Count
                    //                    > 0)
                    //                    ClickTime =
                    //                        (from t in
                    //                             Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.
                    //                             InfoDictionary.
                    //                             Values
                    //                         orderby t.DateCreate descending
                    //                         select t).ToList()[0].DateCreate;
                    //            }
                    //            else
                    //            {
                    //                var tmox =
                    //                    (from t in
                    //                         Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.
                    //                         Values
                    //                     where t.DateCreate > ClickTime && t.IsThisUserShow
                    //                     orderby t.DateCreate descending
                    //                     select t).ToList();

                    //                var argss = new PublishEventArgs()
                    //                                {
                    //                                    EventType = PublishEventType.Core,
                    //                                    EventId = EventIdAssign.PushErrNums
                    //                                };
                    //                argss.AddParams(tmox.Count);
                    //                EventPublish.PublishEvent(argss);


                    //            }
                    //        }
                    //    }
                    //    if (!_thisViewActive) return;
                    //    if (IsLockThisViewOnNewErrArrive) return;
                    //    var info = args.GetParams()[0] as List<int>;
                    //    if (info == null) return;
                    //    foreach (var t in info)
                    //    {
                    //        var ntgs =
                    //            Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.
                    //                GetFaultInfoById
                    //                (t);
                    //        if (ntgs != null)
                    //            AddErrorInfo(ntgs, true);
                    //    }


                    //}
                }
                //if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.PreExistErrorRequestId)
                //{
                //    var infos = args.GetParams()[1] as EquipmentPreFaultExChange;
                //    if (infos == null) return;
                //    OnPreDataBack(infos);
                //}
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(
                    "EquipmentDataQuery.EquipmentFaultRecordQueryViewModel FundEventHandler occer an error:" +
                    ex);
            }

        }

        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
        {
            if (!_thisViewActive) return false;
            // if (!IsSingleEquipmentQuery) return false;
            try
            {
                if (args.EventType == PublishEventType.Core)
                {
                    //if (!IsSingleEquipmentQuery) return false;  lvf
                    //if (args.EventId == Sr.EquipmentGroupInfoHolding.Services.EventIdAssign.MainSingleTreeNodeActive)
                    //{
                    //    if (Convert.ToInt32(args.GetParams()[1]) == 2)
                    //    {
                    //        return true;
                    //    }
                    //}
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {
                        if (OptionXmlSvr.GetOptionInt(4001, 2) == 1) return false;

                        return true;
                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentMulSelected)
                    {
                        if (OptionXmlSvr.GetOptionInt(4001, 2) == 1) return true;
                        return false;

                    }
                    //if (args.EventId ==Sr.EquipemntLightFault.Services.EventIdAssign.EquipmentExistFaultAddId)
                    //{
                    //    return true;
                    //}

                }
                //if (args.EventType == PublishEventType.Sevr &&
                //    args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.PreExistErrorRequestId)
                //{
                //    return true;
                //}
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        private void AddErrorInfo(FaultInfoBase error, bool dongtaiupdate)
        {
            return;
            // var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(id);
            if (error == null) return;

            var infovm = new EquipmentFaultViewModel
                             {
                                 Index = 0,
                                 DtCreateTime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                                 DtRemoceTime = "--",
                                 FaultId = error.FaultId,
                                 RtuId = error.RtuId,
                                 RtuLoopName = error.RtuLoopName,
                                 RtuLoops = error.LoopId,
                                 PhyId = error.RtuPhyId,
                                 FaultName = error.FaultName,
                                 Count = error.AlarmCount + 1,
                                 Color = error.Color,
                                 RtuName = error.RtuName,
                                 Remark = error.Remark,
                                 DateCreateId = error.RecordId,
                                 DateRemoveId = 0,
                             };

            Records.Insert(0, infovm);


            //if (Records.Count > MaxRecordHold)
            //{
            //    Records.RemoveAt(0);
            //}
        }

        #endregion

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone.LxFault.wlst_fault_pre,
                OnRequestFaultPre,
                typeof (EquipmentFaultRecordQueryViewModel), this);
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone.LxFault.wlst_fault_curr_time_cal, //现存故障统计
                OnRequestFaultCurrTimeCal,
                typeof (EquipmentFaultRecordQueryViewModel), this);
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone.LxFault.wlst_fault_pre_for_single, //查询最近的前一条 or后一条
                OnRequestFaultPreForSingle,
                typeof (EquipmentFaultRecordQueryViewModel), this);
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone.LxRtuTime.wst_rtutime_time_table_emerg, //请求已经设定的设备回路
                OnRequestSetUpTmls,
                typeof (EquipmentFaultRecordQueryViewModel), this);
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxFault.wst_fault_hlbph_level,
                //.ClientPart.wlst_server_ans_clinet_request_sys_title,
                ActionRcvLnSetting,
                typeof (EquipmentFaultRecordQueryViewModel), this,true );
            //lvf todo
        }

        /// <summary>
        /// 请求火零不平衡设置
        /// </summary>
        private void delayEvent()
        {
            var xxxinfo = Wlst.Sr.ProtocolPhone.LxFault.wst_fault_hlbph_level;
                //.ServerPart.wlst_clinet_request_sys_title;
            xxxinfo.WstFaultHlbphLevel.Op = 1;
            SndOrderServer.OrderSnd(xxxinfo, 1, 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="infos"></param>
        public void ActionRcvLnSetting(string session, Wlst.mobile.MsgWithMobile infos)
        {

            if (infos.WstFaultHlbphLevel == null) return;
            try
            {
                if (infos.WstFaultHlbphLevel.Op == 1)
                {
                    //HLbphUpper =new ObservableCollection<NameValueIntLevels>();
                    var ntg =
                        (from t in infos.WstFaultHlbphLevel.Levels where t > 0.001 orderby t ascending select t).ToList();
                    if (ntg.Count ==0)
                    {
                        HLbphUpper.Add(new NameValueIntLevels()
                        {
                            Name =  "0以上",
                            Value =0,
                            Value2 = 100000,
                            IsSelected = true 
                        });
                        return;

                    }
                    double max = 0;
                    for (int i = 0; i < ntg.Count; i++)
                    {
                        if (i > 9) break;
                        if (ntg[i] == 0) continue;
                        HLbphUpper.Add(new NameValueIntLevels()
                        {
                            Name =
                                i == 0
                                    ? "0-" + ntg[i].ToString("f2")
                                    : ntg[i - 1].ToString("f2") + "-" + ntg[i].ToString("f2"),
                            Value = i == 0 ? 0 : ntg[i - 1],
                            Value2 = ntg[i],
                            IsSelected = false
                        });
                        if (max < ntg[i]) max = ntg[i];

                    }
                    var num = ntg.Count;
                    HLbphUpper.Add(new NameValueIntLevels()
                    {
                        Name = max + "以上",
                        Value = max,
                        Value2 = 100000,
                        IsSelected = false
                    });



                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }

        //private string pathcx = Environment.CurrentDirectory + "\\Config\\IsShowThisViewOnNewErrArrive.txt";
        //private void LoadIsShowThisViewOnNewErrArrive()
        //{
        //    var ft = Wlst.Ux.EquipemntLightFault.Services.fileread.Read(pathcx);
        //    if (string.IsNullOrEmpty(ft)) IsShowThisViewOnNewErrArrive = false;
        //    else
        //    {
        //        int x = 0;
        //        if (Int32.TryParse(ft, out x))
        //        {
        //            IsShowThisViewOnNewErrArrive = (x == 1);
        //        }
        //    }
        //}

        //public static bool IsShowThisViewOnNewErrArriveInfo = false;
        private bool _cheIsLockThisViewOnNewErrArriveck;

        public bool IsLockThisViewOnNewErrArrive
        {
            get { return _cheIsLockThisViewOnNewErrArriveck; }
            set
            {
                if (_cheIsLockThisViewOnNewErrArriveck != value)
                {
                    _cheIsLockThisViewOnNewErrArriveck = value;
                    this.RaisePropertyChanged(() => this.IsLockThisViewOnNewErrArrive);
                    IsLockThisViewOnNewErrArrive = value;
                }
            }
        }

        private bool IsInTheSection(double aup)
        {

            foreach (var t in HLbphUpper)
            {
                if (t.IsSelected == false) continue;

                double idf = Math.Abs(aup);
                if (idf >= t.Value && idf < t.Value2)
                {
                    return true;
                }
            }



            return false;



            //switch (numRemark)
            //{
            //    case 1:
            //        if (aup >= 0 && aup < 5) return true;
            //        return false;
            //        lowerLimit = 0;
            //        upperLimit = 5;
            //        break;
            //    case 2:
            //        if (aup >= 5 && aup < 10) return true;
            //        return false;
            //        lowerLimit = 5;
            //        upperLimit = 10;
            //        break;
            //    case 4:
            //        if (aup >= 10) return true;
            //        return false;
            //        lowerLimit = 10;
            //        upperLimit = 10;
            //        break;
            //    case 3:
            //        if (aup >= 0 && aup < 10) return true;
            //        return false;
            //        lowerLimit = 0;
            //        upperLimit = 10;
            //        break;
            //    case 5:
            //        if (aup >= 10 || aup < 5) return true;
            //        return false;
            //        lowerLimit = 10;
            //        upperLimit = 5;
            //        break;
            //    case 6:
            //        if (aup >= 5) return true;
            //        return false;
            //        lowerLimit = 5;
            //        upperLimit = 0;
            //        break;
            //    case 7:
            //        return true;
            //        lowerLimit = 0;
            //        upperLimit = 0;
            //        break;
            //    default:
            //        return false;
            //}


        }

        public void OnRequestFaultPre(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (!_thisViewActive) return;
            var list = infos.WstFaultPre;

            //界面为现存故障查询 退出
            if (IsOldFaultQuery == false) return;

            if (CountPreErrs)
            {
                OnRequestFaultPreCount(list);
                return;
            }
            //var list = obj as List<PreErrorItem>;
            //if (list == null) return;
            //  Records.Clear();
            var tmp = new Dictionary<Tuple<int, int, int>, int>();

            bool isloopError = false;
            Records.Clear();
            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            int indexx = 0;


            //lvf  2018年4月10日15:46:06   按照发生时间排序
            var faultItems = (from t in list.FaultItems orderby t.DateCreate ascending select t).ToList();
            foreach (var item in faultItems) //list.FaultItems
            {
                //如果不是火零不平衡的报警  继续
                if (item.FaultId != 25) continue;


                //if (AreaComboBoxSelected.Key != -1)
                //{
                //    //判断 终端是否属于所选区域
                //    if (!RtusBelongArea.Contains(item.RtuId)) continue;

                //}
                ////////判断 差值是否符合统计值区间 lvf 2018年4月9日14:31:15
                //////if (QueryValue > 1 && QueryValue < 4)
                //////{
                //////    if (item.AUpper < lowerLimit || item.AUpper >= upperLimit) continue;
                //////}
                //////else if (QueryValue == 4 && item.AUpper < upperLimit)
                //////{
                //////    continue;
                //////}


                //添加绝对值  lvf 2018年7月1日15:29:36
                if (IsInTheSection(Math.Abs(item.AUpper)) == false) continue;

                int count;

                //if (IsUseTimeLongQuery)
                //{

                var tu = new Tuple<int, int, int>(item.RtuId, item.LoopId, item.FaultId);
                if (tmp.ContainsKey(tu)) count = tmp[tu];
                else
                {
                    count =
                        (from t in list.FaultItems
                         where
                             t.RtuId == item.RtuId && t.LoopId == item.LoopId &&
                             t.FaultId == item.FaultId
                         select t).Count();
                    tmp.Add(tu, count);

                }
                indexx++;
                var mtpsss = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(item.RtuId);
                int py = item.RtuId;
                string rtuname = "";
                if (mtpsss != null)
                {
                    rtuname = mtpsss.RtuName;
                    if (mtpsss.RtuFid > 0)
                    {
                        var nfx =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                                mtpsss.RtuFid);
                        if (nfx != null)
                        {
                            rtuname = nfx.RtuName + "-" + mtpsss.RtuName;
                            py = nfx.RtuPhyId;
                        }
                    }
                    else py = mtpsss.RtuPhyId;
                }
                //var loopName = mtpsss != null
                //                                ? mtpsss.GetLoopName(item.LoopId)
                //                                  : item.LoopId.ToString(CultureInfo.InvariantCulture);
                //if (item.FaultId == 21 || item.FaultId ==20) loopName = "开关量输出K" + item.LoopId;
                string loopName = "";

                if (item.FaultId == 20 || item.FaultId == 21)
                {
                    var t =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[item.RtuId]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (t == null) return;
                    if (t.WjSwitchOuts.ContainsKey(item.LoopId))
                    {
                        loopName = t.WjSwitchOuts[item.LoopId].SwitchName;
                    }
                    else
                    {
                        loopName = "开关量输出 K" + item.LoopId;
                    }

                }
                else if (item.FaultId > 5 && item.FaultId < 18)
                {
                    loopName = mtpsss != null
                                   ? mtpsss.GetLoopName(item.LoopId)
                                   : item.LoopId.ToString(CultureInfo.InvariantCulture);
                    if (loopName.Trim() == "")
                    {
                        loopName = "回路" + item.LoopId;

                    }

                }
                else if (item.FaultId >= 50 && item.FaultId < 80 && item.LoopId > 0)
                {
                    loopName = mtpsss != null
                                   ? mtpsss.GetLoopName(item.LoopId)
                                   : item.LoopId.ToString(CultureInfo.InvariantCulture);
                    if (item.LampId > 0) loopName = loopName + "," + item.LampId;
                }
                else
                {

                    loopName = mtpsss != null
                                   ? mtpsss.GetLoopName(item.LoopId)
                                   : item.LoopId.ToString(CultureInfo.InvariantCulture);

                }

                if (item.A < 0.0001 && item.ALower < 0.0001 && item.AUpper < 0.0001 && item.Aeding < 0.0001 &&
                    item.V < 0.0001)
                {
                    isloopError = false;
                }
                else
                {
                    isloopError = true;
                }

                var tmpobs = new EquipmentFaultViewModel
                                 {
                                     DtCreateTime = new DateTime(item.DateCreate).ToString("yyyy-MM-dd HH:mm:ss"),
                                     DtRemoceTime = new DateTime(item.DateRemove).ToString("yyyy-MM-dd HH:mm:ss"),
                                     Index = indexx,
                                     FaultId = item.FaultId,
                                     RtuId = item.RtuId,
                                     PhyId = py,
                                     RtuLoopName = loopName,
                                     RtuLoops = item.LoopId,
                                     RtuName = rtuname,
                                     //mtpsss != null ? mtpsss.RtuName : GetRtuName(item.RtuId),
                                     FaultName = GetFaultName(item.FaultId).Item1,
                                     Color = GetFaultColor(item.FaultId),
                                     Count = count,
                                     Remark = item.Remark,
                                     DateCreateId = item.DateCreate,
                                     DateRemoveId = item.DateRemove,
                                     LampId = item.LampId,
                                     IsShowAtTop = GetFaultName(item.FaultId).Item2,

                                     A = !isloopError ? "---" : item.A.ToString("f2") + "",
                                     AUpper = !isloopError ? "---" : item.AUpper.ToString("f2") + "",
                                     ALower = !isloopError ? "---" : item.ALower.ToString("f2") + "",
                                     Aeding = !isloopError ? "---" : item.Aeding.ToString("f2") + "",
                                     V = !isloopError ? "---" : item.V.ToString("f2") + "",
                                 };
                var tmps =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                        item.RtuId]
                    as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if (tmps == null) continue;
                if (tmps.WjLoops.ContainsKey(item.LoopId) == false) continue;
                //var lopname = tmps.WjLoops[item.LoopId].LoopName;
                var looopname = tmps.WjLoops[item.LoopId].LoopName;
                if (looopname.Substring(looopname.Length - 2) == "火线")
                    looopname = looopname.Substring(0, looopname.Length - 2);
                tmpobs.RtuLoopName = looopname;
                //获取终端分组信息
                var groupInfo =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(item.RtuId);
                if (groupInfo != null)
                {
                    var infosss =
                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.
                            GetGroupInfomation(
                                groupInfo.Item1, groupInfo.Item2);
                    if (infosss != null)
                        tmpobs.GroupName = infosss.GroupName; //+ " - " + infosss.GroupId;

                    //  if (infosss != null) DtRtuMsg += infosss.GroupName + " - ";

                }
                else
                {
                    tmpobs.GroupName = "特殊终端";
                }
                //获取终端所属区域信息
                var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(item.RtuId);
                var areaInfo = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(areaId);
                if (areaInfo == null)
                {
                    tmpobs.AreaName = "无区域信息";
                }
                else
                {
                    tmpobs.AreaName = areaInfo.AreaId + " - " + areaInfo.AreaName;
                }
                obs.Add(tmpobs);
                // if (indexx % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent(); //todo
            }

            var mtp = (from t in obs orderby t.DateCreateId ascending select t).ToList(); //t.IsShowAtTop descending,

            Records.Clear();

            FilterAreaErrs(mtp);
            //foreach (var f in mtp )
            //{
            //    indexx++;
            //    Records.Add(f);
            //    if (indexx % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
            //}

            ////  Remind = "数据已反馈完毕，请查看数据！";
            //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + Records.Count + " 条数据.";
        }

        public void OnRequestFaultPreCount(EquipmentFaultPre list)
        {
            var tmp = new Dictionary<Tuple<int, int, int>, Tuple<int, long, long>>();

            Records.Clear();
            Recordss.Clear();
            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            // var lst = new List<Tuple<int, int, int>>();

            var dic = new Dictionary<Tuple<int, int, int>, Tuple<int, long, long>>();


            var llll = (from t in list.FaultItems
                        orderby t.DateCreate
                        select t);

            foreach (var item in llll) //todo 不算现存的最新的一条
            {
                var tu = new Tuple<int, int, int>(item.RtuId, item.LoopId, item.FaultId);

                if (!dic.ContainsKey(tu))
                {
                    var tu2 = new Tuple<int, long, long>(1, item.DateCreate, item.DateCreate);
                    dic.Add(tu, tu2);
                }
                else
                {
                    var tu2 = new Tuple<int, long, long>(dic[tu].Item1 + 1, dic[tu].Item2, item.DateCreate);
                    dic[tu] = tu2;
                }
            }

            tmp = dic;

            //foreach (var item in list.FaultItems)
            //{
            //    int count;
            //    long lastTime;
            //    long firstTime;
            //    //if (IsUseTimeLongQuery)
            //    //{

            //    var tu = new Tuple<int, int, int>(item.RtuId, item.LoopId, item.FaultId);
            //    var lll = (from t in list.FaultItems
            //               orderby t.DateCreate ascending
            //               where
            //                   t.RtuId == item.RtuId && t.LoopId == item.LoopId &&
            //                   t.FaultId == item.FaultId
            //               select t);

            //    if (tmp.ContainsKey(tu))
            //    {
            //        count = tmp[tu].Item1;
            //        firstTime = tmp[tu].Item2;
            //        lastTime = tmp[tu].Item3;
            //    }
            //    else
            //    {
            //        count = lll.Count();
            //        //(from t in list.FaultItems
            //        // orderby t.DateCreate ascending
            //        // where
            //        //     t.RtuId == item.RtuId && t.LoopId == item.LoopId &&
            //        //     t.FaultId == item.FaultId
            //        // select t).Count();
            //        firstTime = lll.First().DateCreate;
            //        lastTime = lll.Last().DateCreate;
            //        var a = new Tuple<int, long, long>(count, firstTime, lastTime);
            //        tmp.Add(tu, a);

            //    }
            //}
            int indexx = 0;
            foreach (var g in tmp)
            {

                indexx++;

                indexx++;
                int rtuid = g.Key.Item1;
                int py = g.Key.Item1; //.RtuId;
                int loopId = g.Key.Item2;
                int faultId = g.Key.Item3;
                int count = g.Value.Item1;
                long firstTime = g.Value.Item2;
                long lastTime = g.Value.Item3;
                string firstT = "";
                string lastT = "";
                string rtuname = "";




                lastT = new DateTime(lastTime).ToString("yyyy-MM-dd HH:mm:ss");

                if (count == 1)
                {
                    firstT = "---";
                }
                else
                {
                    firstT = new DateTime(firstTime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                var mtpsss = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuid);
                if (mtpsss != null)
                {
                    rtuname = mtpsss.RtuName;
                    if (mtpsss.RtuFid > 0)
                    {
                        var nfx =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                                mtpsss.RtuFid);
                        if (nfx != null)
                        {
                            rtuname = nfx.RtuName + "-" + mtpsss.RtuName;
                            py = nfx.RtuPhyId;
                        }
                    }
                    else py = mtpsss.RtuPhyId;
                }

                //var loopName = mtpsss != null
                //                                ? mtpsss.GetLoopName(loopId) //loopid
                //                                  : loopId.ToString(CultureInfo.InvariantCulture);
                //if (faultId == 21) loopName = "开关量输出K" + loopId; //faultID

                string loopName = "";

                if (faultId == 20 || faultId == 21)
                {
                    var t =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (t == null) return;
                    if (t.WjSwitchOuts.ContainsKey(loopId))
                    {
                        loopName = t.WjSwitchOuts[loopId].SwitchName;
                    }
                    else
                    {
                        loopName = "开关量输出 K" + loopId;
                    }

                }
                else if (faultId > 5 && faultId < 18)
                {
                    loopName = mtpsss != null
                                   ? mtpsss.GetLoopName(loopId)
                                   : loopId.ToString(CultureInfo.InvariantCulture);
                    if (loopName.Trim() == "")
                    {
                        loopName = "回路" + loopId;

                    }

                }
                else if (faultId >= 50 && faultId < 80 && loopId > 0)
                {
                    loopName = "控制器" + loopId;
                }
                else
                {

                    loopName = mtpsss != null
                                   ? mtpsss.GetLoopName(loopId)
                                   : loopId.ToString(CultureInfo.InvariantCulture);

                }

                obs.Add(new EquipmentFaultViewModel
                            {
                                DtCreateTime = firstT,
                                //new DateTime(firstTime).ToString("yyyy-MM-dd HH:mm:ss"), //new DateTime(item.DateCreate).ToString("yyyy-MM-dd HH:mm:ss"),
                                DtRemoceTime = lastT,
                                //new DateTime(item.DateRemove).ToString("yyyy-MM-dd HH:mm:ss"),
                                Index = indexx,
                                FaultId = faultId,
                                RtuId = py,
                                PhyId = py,
                                RtuLoopName = loopName,
                                RtuLoops = loopId,
                                RtuName = rtuname,
                                //mtpsss != null ? mtpsss.RtuName : GetRtuName(item.RtuId),
                                FaultName = GetFaultName(faultId).Item1,
                                Color = GetFaultColor(faultId),
                                Count = count,
                                //Remark = mtpsss.RtuRemark,//.Remark,
                                DateCreateId = firstTime,
                                //item.DateCreate,
                                DateRemoveId = lastTime,
                                //item.DateRemove,
                                //LampId = item.LampId,
                                IsShowAtTop = GetFaultName(faultId).Item2,


                            });
                if (count == 1)
                {

                }
                // if (indexx % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent(); //todo
            }



            var mtp = (from t in obs orderby t.DtRemoceTime descending select t).ToList(); //t.IsShowAtTop descending,

            Recordss.Clear();
            Records.Clear();
            foreach (var f in mtp)
            {
                indexx++;
                Recordss.Add(f);
                if (indexx%100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
            }

            //  Remind = "数据已反馈完毕，请查看数据！";
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + mtp.Count + " 条数据.";
                //list.FaultItems.Count + " 条数据.";
        }


        public void OnRequestFaultCurrTimeCal(string session, Wlst.mobile.MsgWithMobile infos)
        {

            if (!_thisViewActive) return;
            var list = infos.WstFaultCurrForTimeCal;
            ArgsInfoVisi = false; //统计故障时，隐藏单条具体参数信息

            //var list = obj as List<PreErrorItem>;
            //if (list == null) return;
            //  Records.Clear();
            var tmp = new Dictionary<Tuple<int, int, int>, int>();



            Records.Clear();
            Recordss.Clear();
            var obs = new ObservableCollection<EquipmentFaultViewModel>();

            int indexx = 0;
            foreach (var item in list.FaultItems)
            {
                int count;
                //if (IsUseTimeLongQuery)
                //{

                //var tu = new Tuple<int, int, int>(item.RtuId, item.LoopId, item.FaultId);
                //if (tmp.ContainsKey(tu)) count = tmp[tu];
                //else
                //{
                //    count =
                //        (from t in list.FaultItems
                //         where
                //             t.RtuId == item.RtuId && t.LoopId == item.LoopId &&
                //             t.FaultId == item.FaultId
                //         select t).Count();
                //    tmp.Add(tu, count);

                //}
                indexx++;
                var mtpsss = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(item.RtuId);
                int py = item.RtuId;
                string rtuname = "";
                if (mtpsss != null)
                {
                    rtuname = mtpsss.RtuName;
                    if (mtpsss.RtuFid > 0)
                    {
                        var nfx =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                                mtpsss.RtuFid);
                        if (nfx != null)
                        {
                            rtuname = nfx.RtuName + "-" + mtpsss.RtuName;
                            py = nfx.RtuPhyId;
                        }
                    }
                    else py = mtpsss.RtuPhyId;
                }

                //var loopName = mtpsss != null
                //                                ? mtpsss.GetLoopName(item.LoopId)
                //                                  : item.LoopId.ToString(CultureInfo.InvariantCulture);
                //if (item.FaultId == 21 || item.FaultId == 20) loopName = "开关量输出K" + item.LoopId;
                string loopName = "";

                if (item.FaultId == 20 || item.FaultId == 21)
                {
                    var t =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[item.RtuId]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (t == null) return;
                    if (t.WjSwitchOuts.ContainsKey(item.LoopId))
                    {
                        loopName = t.WjSwitchOuts[item.LoopId].SwitchName;
                    }
                    else
                    {
                        loopName = "开关量输出 K" + item.LoopId;
                    }

                }
                else if (item.FaultId > 5 && item.FaultId < 18)
                {
                    loopName = mtpsss != null
                                   ? mtpsss.GetLoopName(item.LoopId)
                                   : item.LoopId.ToString(CultureInfo.InvariantCulture);
                    if (loopName.Trim() == "")
                    {
                        loopName = "回路" + item.LoopId;

                    }

                }
                else if (item.FaultId >= 50 && item.FaultId < 80 && item.LoopId > 0)
                {
                    loopName = mtpsss != null
                                   ? mtpsss.GetLoopName(item.LoopId)
                                   : item.LoopId.ToString(CultureInfo.InvariantCulture);
                    if (item.LampId > 0) loopName = loopName + "," + item.LampId;
                }
                else
                {

                    loopName = mtpsss != null
                                   ? mtpsss.GetLoopName(item.LoopId)
                                   : item.LoopId.ToString(CultureInfo.InvariantCulture);

                }

                obs.Add(new EquipmentFaultViewModel
                            {
                                DtCreateTime =
                                    item.DtErrFirstAlarm < 1
                                        ? "--"
                                        : new DateTime(item.DtErrFirstAlarm).ToString("yyyy-MM-dd HH:mm:ss"),
                                DtRemoceTime = new DateTime(item.DateCreate).ToString("yyyy-MM-dd HH:mm:ss"),
                                Index = indexx,
                                FaultId = item.FaultId,
                                RtuId = item.RtuId,
                                PhyId = py,
                                RtuLoopName = loopName,
                                RtuLoops = item.LoopId,
                                RtuName = rtuname,
                                //mtpsss != null ? mtpsss.RtuName : GetRtuName(item.RtuId),
                                FaultName = GetFaultName(item.FaultId).Item1,
                                Color = GetFaultColor(item.FaultId),
                                Count = item.AlarmCount,
                                Remark = item.Remark,
                                //DateCreateId = item.DateRemove,
                                DateRemoveId = item.DateCreate,
                                LampId = item.LampId,
                                IsShowAtTop = GetFaultName(item.FaultId).Item2
                            });
                // if (indexx % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent(); //todo
            }

            var mtp = (from t in obs orderby t.DateCreateId ascending select t).ToList(); //t.IsShowAtTop descending,

            Recordss.Clear();
            foreach (var f in mtp)
            {
                indexx++;
                Recordss.Add(f);
                if (indexx%100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
            }
            CountNewErrs = false;
            //  Remind = "数据已反馈完毕，请查看数据！";
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + list.FaultItems.Count + " 条数据.";
        }


        public void OnRequestFaultPreForSingle(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var list = infos.WstFaultPreForSingle;
            ArgsInfoVisi = false; //统计故障时，隐藏单条具体参数信息
            if (_thisViewActive == false) return;
            if (list.FaultItems.Count == 0)
            {
                //CountLastPreErrs = true;
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，无数据.";
                UMessageBox.Show("提醒", "无数据,这是最后一条数据！", UMessageBoxButton.Ok);
                return;
            }
            //var list = obj as List<PreErrorItem>;
            //if (list == null) return;
            //  Records.Clear();
            var tmp = new Dictionary<Tuple<int, int, int>, int>();

            Records.Clear();
            Recordss.Clear();
            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            string dateRemove = "";
            int indexx = 0;
            foreach (var item in list.FaultItems)
            {
                int count;
                //if (IsUseTimeLongQuery)
                //{
                if (item.DateRemove > 0)
                {
                    dateRemove = new DateTime(item.DateRemove).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    dateRemove = "---";
                }
                var tu = new Tuple<int, int, int>(item.RtuId, item.LoopId, item.FaultId);
                if (tmp.ContainsKey(tu)) count = tmp[tu];
                else
                {
                    count =
                        (from t in list.FaultItems
                         where
                             t.RtuId == item.RtuId && t.LoopId == item.LoopId &&
                             t.FaultId == item.FaultId
                         select t).Count();
                    tmp.Add(tu, count);

                }
                indexx++;
                var mtpsss = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(item.RtuId);
                int py = item.RtuId;
                string rtuname = "";
                if (mtpsss != null)
                {
                    rtuname = mtpsss.RtuName;
                    if (mtpsss.RtuFid > 0)
                    {
                        var nfx =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                                mtpsss.RtuFid);
                        if (nfx != null)
                        {
                            rtuname = nfx.RtuName + "-" + mtpsss.RtuName;
                            py = nfx.RtuPhyId;
                        }
                    }
                    else py = mtpsss.RtuPhyId;
                }

                //var loopName = mtpsss != null
                //                                ? mtpsss.GetLoopName(item.LoopId)
                //                                  : item.LoopId.ToString(CultureInfo.InvariantCulture);
                //if (item.FaultId == 21 || item.FaultId == 20) loopName = "开关量输出K" + item.LoopId;

                string loopName = "";

                if (item.FaultId == 20 || item.FaultId == 21)
                {
                    var t =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[item.RtuId]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (t == null) return;
                    if (t.WjSwitchOuts.ContainsKey(item.LoopId))
                    {
                        loopName = t.WjSwitchOuts[item.LoopId].SwitchName;
                    }
                    else
                    {
                        loopName = "开关量输出 K" + item.LoopId;
                    }

                }
                else if (item.FaultId > 5 && item.FaultId < 18)
                {
                    loopName = mtpsss != null
                                   ? mtpsss.GetLoopName(item.LoopId)
                                   : item.LoopId.ToString(CultureInfo.InvariantCulture);
                    if (loopName.Trim() == "")
                    {
                        loopName = "回路" + item.LoopId;

                    }

                }
                else if (item.FaultId >= 50 && item.FaultId < 80 && item.LoopId > 0)
                {
                    loopName = mtpsss != null
                                   ? mtpsss.GetLoopName(item.LoopId)
                                   : item.LoopId.ToString(CultureInfo.InvariantCulture);
                    if (item.LampId > 0) loopName = loopName + "," + item.LampId;
                }
                else
                {

                    loopName = mtpsss != null
                                   ? mtpsss.GetLoopName(item.LoopId)
                                   : item.LoopId.ToString(CultureInfo.InvariantCulture);

                }


                obs.Add(new EquipmentFaultViewModel
                            {
                                DtCreateTime = new DateTime(item.DateCreate).ToString("yyyy-MM-dd HH:mm:ss"),
                                DtRemoceTime = dateRemove,
                                // new DateTime(item.DateRemove).ToString("yyyy-MM-dd HH:mm:ss"),
                                Index = indexx,
                                FaultId = item.FaultId,
                                RtuId = item.RtuId,
                                PhyId = py,
                                RtuLoopName = loopName,
                                RtuLoops = item.LoopId,
                                RtuName = rtuname,
                                //mtpsss != null ? mtpsss.RtuName : GetRtuName(item.RtuId),
                                FaultName = GetFaultName(item.FaultId).Item1,
                                Color = GetFaultColor(item.FaultId),
                                Count = count,
                                Remark = item.Remark,
                                DateCreateId = item.DateCreate,
                                DateRemoveId = item.DateRemove,
                                LampId = item.LampId,
                                IsShowAtTop = GetFaultName(item.FaultId).Item2
                            });
                // if (indexx % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent(); //todo
            }

            var mtp = (from t in obs orderby t.DateCreateId ascending select t).ToList(); //t.IsShowAtTop descending,

            Records.Clear();
            Recordss.Clear();
            foreach (var f in mtp)
            {
                indexx++;
                Records.Add(f);
                Recordss.Add(f);
                if (indexx%100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
            }

            //  Remind = "数据已反馈完毕，请查看数据！";
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + list.FaultItems.Count + " 条数据.";
        }



        /// <summary>
        /// 请求应急关灯的设备和回路   lvf 2018年6月14日08:46:45
        /// </summary>
        public void OnRequestSetUpTmls(string session, Wlst.mobile.MsgWithMobile infos)
        {
            //if (!_thisViewActive) return;
            //var list = infos.WstRtutimeTimeTableEmerg;
            //if (list == null) return;
            //if (list.Op != 3) return;
            //if (list.RtuQueryItems == null || list.RtuQueryItems.Count == 0) return;

            //var dicR = new Dictionary<int, List<int>>();
            //foreach (var g in list.RtuQueryItems)
            //{


            //    if (dicR.ContainsKey(g.RtuId) == false)
            //    {
            //        var lst = new List<int>();
            //        lst.Add(g.LoopId);
            //        dicR.Add(g.RtuId, lst);
            //    }
            //    else
            //    {
            //        if (dicR[g.RtuId].Contains(g.LoopId) == false)
            //        {
            //            dicR[g.RtuId].Add(g.LoopId);
            //        }

            //    }
            //}


            //if (Wlst.Sr.EquipmentInfoHolding.Services.Others.ControlCenterIsShow)
            //{
            //    WlstMessageBox.Show("警告", "控制中心已经打开，正在处理其他操作，请关闭控制中心界面，重试", WlstMessageBoxType.Ok);
            //    return;
            //}

            //if (dicR.Count < 1)
            //{

            //    WlstMessageBox.Show("警告", "没有应急关灯的设备。", WlstMessageBoxType.Ok);
            //}
            //else
            //{
            //    RegionManage.ShowViewByIdAttachRegionWithArgu(1102814, dicR);
            //}

        }

        private readonly List<NameIntBool> _faultName = new List<NameIntBool>();

        private void GetAllFaultName()
        {
            foreach (var item in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                _faultName.Add(new NameIntBool
                                   {
                                       Name = item.Value.FaultNameByDefine,
                                       Value = item.Value.FaultId,
                                       AreaId = item.Value.PriorityLevel
                                   });
            }
        }

        private Tuple<string, int> GetFaultName(int faultid)
        {
            foreach (var item in _faultName.Where(item => faultid == item.Value))
            {
                return new Tuple<string, int>(item.Name, item.AreaId);
            }
            return new Tuple<string, int>("no name", 0);
        }


        private string GetFaultColor(int faultid)
        {
            var tmp = Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetInfoById(faultid);
            if (tmp != null) return tmp.Color;
            return null;
        }


        private void FilterAreaErrs(ObservableCollection<EquipmentFaultViewModel> records, bool iscol = false)
        {
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计0条数据.";
            if (AreaId == -1)
            {
                if (iscol == false)
                {
                    Records = records;
                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + Records.Count + " 条数据.";
                }
                else
                {
                    Recordss = records;
                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + Recordss.Count + " 条数据.";
                }

            }
            else
            {
                if (GrpId == -1) //全部分组
                {
                    foreach (var g in records)
                    {
                        var areaid =
                            Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(g.RtuId);
                        if (areaid == AreaId)
                        {
                            if (iscol == false)
                            {
                                Records.Add(g);
                                if (Records.Count%100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + Records.Count +
                                         " 条数据.";
                            }
                            else
                            {
                                Recordss.Add(g);
                                if (Recordss.Count%100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + Recordss.Count +
                                         " 条数据.";
                            }



                        }
                    }
                }
                else //选择了分组
                {
                    var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(
                        AreaId, GrpId); //GetRtuInArea(AreaId);

                    var pb = (from t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                              where lstInArea.Contains(t.Value.RtuFid)
                              select t.Key).ToList();
                    lstInArea.AddRange(pb);

                    foreach (var g in records)
                    {

                        if (lstInArea.Contains(g.RtuId) == false) continue;
                        if (iscol == false)
                        {
                            Records.Add(g);
                            if (Records.Count%100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + Records.Count +
                                     " 条数据.";
                        }
                        else
                        {
                            Recordss.Add(g);
                            if (Recordss.Count%100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + Recordss.Count +
                                     " 条数据.";
                        }




                    }
                }

            }

        }

        private void FilterAreaErrs(List<EquipmentFaultViewModel> records, bool iscol = false)
        {
            ObservableCollection<EquipmentFaultViewModel> tmp = new ObservableCollection<EquipmentFaultViewModel>();
            foreach (var g in records)
            {
                tmp.Add(g);
            }
            FilterAreaErrs(tmp, iscol);
        }




    }




    /// <summary>
    /// 操作类型模型定义
    /// </summary>
    public class OperatorTypeItem : ObservableObject
    {

        public event EventHandler OnIsSelectedChanged;


        private bool _check;

        public bool IsSelected
        {
            get { return _check; }
            set
            {
                if (_check != value)
                {
                    _check = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                    if (OnIsSelectedChanged != null)
                    {
                        OnIsSelectedChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        private bool _isShow;

        public bool IsShow
        {
            get { return _isShow; }
            set
            {
                if (_isShow != value)
                {
                    _isShow = value;
                    this.RaisePropertyChanged(() => this.IsShow);
                }
            }
        }

        private bool _checkall;

        public bool IsSelectedAll
        {
            get { return _checkall; }
            set
            {
                if (value == _checkall) return;
                _checkall = value;
                foreach (var item in Value)
                {
                    item.IsSelected = _checkall;
                }

                RaisePropertyChanged(() => IsSelectedAll);
            }
        }

        private string _name;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private ObservableCollection<NameIntBool> _value;

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<NameIntBool> Value
        {
            get { return _value ?? (_value = new ObservableCollection<NameIntBool>()); }
            set
            {
                if (value == _value) return;
                _value = value;
                RaisePropertyChanged(() => Value);
            }
        }
    }



    [Serializable]
    public class NameValueIntLevels : ObservableObject
    {
        public int Index;

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }



        private double _value;
        /// <summary>
        /// 下限
        /// </summary>
        public double Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    this.RaisePropertyChanged(() => this.Value);
                }
            }
        }

        private double _value2;
        /// <summary>
        /// 上限
        /// </summary>
        public double Value2
        {
            get { return _value2; }
            set
            {
                if (_value2 != value)
                {
                    _value2 = value;
                    this.RaisePropertyChanged(() => this.Value2);
                }
            }
        }
        private bool _check;

        public bool IsSelected
        {
            get { return _check; }
            set
            {
                if (_check != value)
                {
                    _check = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                    //if (OnIsSelectedChanged != null) OnIsSelectedChanged(this, EventArgs.Empty);
                }
            }
        }
    }
}



