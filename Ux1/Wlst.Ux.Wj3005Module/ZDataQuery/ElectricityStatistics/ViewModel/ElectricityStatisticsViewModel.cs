using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.WJ3005Module.ZDataQuery.ElectricityStatistics.Services;
using Wlst.client;


namespace Wlst.Ux.WJ3005Module.ZDataQuery.IIElectricityStatistics.ViewModel
{
    [Export(typeof(IIElectricityStatisticsViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ElectricityStatisticsViewModel : EventHandlerHelperExtendNotifyProperyChanged, IIElectricityStatisticsViewModel
    {
        private bool _thisViewActive = false;

        public ElectricityStatisticsViewModel()
        {

            this.InitEvent();
            this.InitAction();
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            //lvf 获取区域信息  并将区域终端存于rtusbelongArea list中   2018年4月9日15:25:14
            getAreaRId();
            _thisViewActive = true ;
            DtEndTimeTime = DateTime.Now;
            DtStartTimeTime = DateTime.Now.AddDays(-1);
            QueryMode = 3;
            if (RtuId == 0)
            {
                PhyId = 0;
                RtuName = "无";
            }
            //RtuId = 0;
            //RtuName = "通过终端树勾选终端进行故障查询.";

        }


        public void OnUserHideOrClosing()
        {
            _thisViewActive = false;

            Records.Clear();
            //recordsKeys.Clear();
        }

        #region tab iinterface

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get
            {
                return "电能统计"; //I36N .Services.I36N .ConvertByCodingOne("11090001", "Setting");
                //return "Setting";
            }
        }

        public bool CanClose
        {
            get { return true; }
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

        #endregion



        #region CmdQuery


        private ICommand _cmdquery;


        private DateTime _dtQuery;

        public ICommand CmdQuery
        {
            get
            {
                if (_cmdquery == null)
                    _cmdquery = new RelayCommand(ExCmdQuery, CanCmdQuery, false);
                return _cmdquery;
            }
        }

        private void ExCmdQuery()
        {
            _dtQuery = DateTime.Now;
            Records.Clear();
            //recordsKeys.Clear();
            //var rtulst = GetRtusLst();
            //var index = 1;
            if (RtuId == 0 && QueryMode==2)
            {
                UMessageBox.Show("提醒", "未选择终端！", UMessageBoxButton.Ok);
                return;
            }
            Query();
            //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  查询成功，共计" + Records.Count + " 条数据.";
        }

        private void Query()
        {
            var tStartTime = new DateTime();
            if ( SelectTpyeId ==1 || SelectTpyeId ==4)
            {
                tStartTime = new DateTime(DtStartTimeTime.Year, DtStartTimeTime.Month, DtStartTimeTime.Day, 0, 0, 1);
            }
            else
            {
                tStartTime = new DateTime(DtStartTimeTime.Year, DtStartTimeTime.Month, 1, 0, 0, 1);
            }
            
            var tEndTime = new DateTime(DtEndTimeTime.Year, DtEndTimeTime.Month, DtEndTimeTime.Day, 23, 59, 59);
            var rtulst = GetRtusLst();
            if (rtulst.Count == 0)
            {
                UMessageBox.Show("提醒", "未符合标准的终端！", UMessageBoxButton.Ok);
                return;
            }
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_elec_stats_data;
            // .wlst_cnt_wj3090_request_open_close_light_record ;//.ServerPart.wlst_OpenCloseLight_clinet_request_rtuopencloseLightrecord;
            info.WstRtuElecStatsData.DtEnd = tEndTime.Ticks;
            info.WstRtuElecStatsData.DtStart = tStartTime.Ticks;
            info.WstRtuElecStatsData.RtuItems = rtulst;
            info.WstRtuElecStatsData.Type = SelectTpyeId;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在查询...";
        }

        private List<int> GetRtusLst()
        {
            var rtulst = new List<int>();
            if (QueryMode == 2)
            {

                if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(RtuId) == false)
                    return rtulst ;
                var rtuInfo =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[RtuId] as Wj3005Rtu;
                if (rtuInfo == null || rtuInfo.WjVoltage == null) return rtulst;
                if (rtuInfo.RtuModel != EnumRtuModel.Wj3006) return rtulst;
                if (rtuInfo.WjVoltage.IsHasElec == false) return rtulst;


                rtulst.Add(RtuId);
            }
            else
            {

                if (AreaId == -1) //全部区域终端
                {
                    //return new List<int>();
                    var tmplst = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Keys;
                    foreach (var g in tmplst)
                    {
                        if (g > 1000000 && g < 1100000)
                        {
                            if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(g) == false)
                                continue;
                            var rtuInfo =
                                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g] as Wj3005Rtu;
                            if (rtuInfo == null || rtuInfo.WjVoltage == null) continue;
                            if (rtuInfo.RtuModel != EnumRtuModel.Wj3006) continue;
                            if (rtuInfo.WjVoltage.IsHasElec == false) continue;

                            if (rtulst.Contains(g) == false) rtulst.Add(g);
                        }
                    }

                }

                if (GrpId == -1)
                {
                    var tmplst = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                    foreach (var g in tmplst)
                    {
                        if (g > 1000000 && g < 1100000)
                        {
                            if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(g) == false)
                                continue;
                            var rtuInfo =
                                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g] as Wj3005Rtu;
                            if (rtuInfo == null || rtuInfo.WjVoltage == null) continue;
                            if (rtuInfo.RtuModel != EnumRtuModel.Wj3006) continue;
                            if (rtuInfo.WjVoltage.IsHasElec == false) continue;

                            if (rtulst.Contains(g) == false) rtulst.Add(g);
                        }
                    }
                }
                else
                {
                    var tmplst = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(
                        AreaId, GrpId); //GetRtuInArea(AreaId);
                    foreach (var g in tmplst)
                    {
                        if (g > 1000000 && g < 1100000)
                        {

                            if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(g) ==false )
                                continue;
                            var rtuInfo =
                                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g] as Wj3005Rtu;
                            if (rtuInfo == null ||rtuInfo.WjVoltage == null) continue;
                            if (rtuInfo.RtuModel != EnumRtuModel.Wj3006) continue;
                            if (rtuInfo.WjVoltage.IsHasElec == false) continue;
                
                            if (rtulst.Contains(g) == false) rtulst.Add(g);
                        }
                    }
                }

            }

            return rtulst;

        }


        private bool CanCmdQuery()
        {
            return DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;
        }


        #endregion

        //打印

        #region CmdPrint

        private DateTime _dtCmdPrint;
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
            _dtCmdPrint = DateTime.Now;
            try
            {
                var tabletitle = new List<string>();
                tabletitle.Add("序号");
                tabletitle.Add("时间");
                tabletitle.Add("天数");
                tabletitle.Add("设备数");
                tabletitle.Add("A相电能");
                tabletitle.Add("B相电能");
                tabletitle.Add("C相电能");
                tabletitle.Add("总电能");
                var table = new List<List<string>>();
                foreach (var g in Records)
                {

                    var tem = new List<string>();
                    tem.Add(g.Index + "");
                    tem.Add(g.DateCreate + "");
                    tem.Add(g.Days + "");
                    tem.Add(g.RtuNums + "");
                    tem.Add(g.Aelec + "");
                    tem.Add(g.Belec + "");
                    tem.Add(g.Celec + "");
                    tem.Add(g.Abcelec + "");
                    //tem.Add(g.APowerFactor+"");


                    table.Add(tem);
                }
                Wlst.print.Prints.Print(tabletitle, table, false, "电能统计表",
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
            return DateTime.Now.Ticks - _dtCmdPrint.Ticks > 30000000;
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
                lsttitle.Add("时间");
                lsttitle.Add("天数");
                lsttitle.Add("设备数");
                lsttitle.Add("A相电能");
                lsttitle.Add("B相电能");
                lsttitle.Add("C相电能");
                lsttitle.Add("总电能");
                var lstobj = new List<List<object>>();

                foreach (var g in Records)
                {
                    var tem = new List<object>();
                    tem.Add(g.Index + "");
                    tem.Add(g.PhyId + "");
                    tem.Add(g.RtuName);
                    tem.Add(g.DateCreate);
                    tem.Add(g.Days.ToString("f2") + "");
                    tem.Add(g.RtuNums + "");
                    tem.Add(g.Aelec.ToString("f2") + "");
                    tem.Add(g.Belec.ToString("f2") + "");
                    tem.Add(g.Celec.ToString("f2") + "");
                    tem.Add(g.Abcelec.ToString("f2") + "");

                    lstobj.Add(tem);
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
     

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_elec_stats_data, // .wlst_svr_ans_cnt_request_wj3090_measure_data ,
                RecordDataRequest,
                typeof (ElectricityStatisticsViewModel), this);
        }

        //private List<Tuple<int, long>> recordsKeys = new List<Tuple<int, long>>(); 
        public void RecordDataRequest(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (!_thisViewActive) return;
            if (infos == null || infos.WstRtuElecStatsData == null) return;
            //中间层计算较慢,如果连续点击,会导致延迟应答,判一下报表类型,如果已经不是当下选的报表类型,则不处理.
            if (infos.WstRtuElecStatsData.Type != SelectTpyeId) return; 


            Records.Clear();
            //recordsKeys.Clear();

            var list = infos.WstRtuElecStatsData.Items;
            foreach (var g in list)
            {
                var item = new ElectricityStatisticsOneItem();
                item.LngDateCreate = g.DateCreate;
                item.Days = g.Days;
                item.Aelec = g.Aelec;
                item.Belec = g.Belec;
                item.Celec = g.Celec;
                item.Abcelec = g.Abcelec;
                item.Index = Records.Count + 1;
                item.RtuNums =g.RtuId ==0? GetRtusLst().Count+"":"---";
                item.RtuId = g.RtuId;

                if (SelectTpyeId == 1 || SelectTpyeId ==4) //日表
                {
                    item.DateCreate = new DateTime(g.DateCreate).ToString("yyyy-MM-dd");
                    item.Days = 1;
                }
                else if (SelectTpyeId == 2 || SelectTpyeId == 5) //月表
                {
                    item.DateCreate = new DateTime(g.DateCreate).ToString("yyyy-MM");
                }
                else if (SelectTpyeId == 3 || SelectTpyeId == 6) // 年表
                {
                    item.DateCreate = new DateTime(g.DateCreate).ToString("yyyy");
                }

                if (QueryMode == 3) //批量统计时,days 才用理论值
                {
                    if (SelectTpyeId == 2 || SelectTpyeId == 5) //月表
                    {
                        int dtMonth = DateTime.DaysInMonth(new DateTime(g.DateCreate).Year,
                                                           new DateTime(g.DateCreate).Month);
                        item.Days = dtMonth;
                    }
                    if (SelectTpyeId == 3 || SelectTpyeId == 6) //年表
                    {
                        var date1 = new DateTime(new DateTime(g.DateCreate).Year, 1, 1);
                        var date2 = new DateTime(new DateTime(g.DateCreate).Year, 12, 31);
                        System.TimeSpan diff = date2.Subtract(date1);
                        item.Days = diff.Days+1;
                    }
                }

                Records.Add(item);
                //if (recordsKeys.Contains(tu) == false) recordsKeys.Add(tu);
            }
            if (Records.Count%100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 数据统计成功，共计" + Records.Count +
                     " 条数据.";




            ////  Remind = "数据已反馈，查询命令已结束，请查看数据！";
            //var tmp = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(info.RtuId);
            //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" +
            //         (tmp == null ? info.RtuId + "" : tmp.RtuName) + "--终端数据查询成功，共计" + Records.Count + "条数据.";
            ////info.Items .Count + " 条数据.";
        }


        private void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core);
        }


        public override void ExPublishedEvent(
            PublishEventArgs args)
        {

            if (_thisViewActive == false) return;

            try
            {

                if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {
                    if (QueryMode != 2) return;
                    int id = Convert.ToInt32(args.GetParams()[0]);
                    //if (id > 1100000)
                    //{
                    //    var tmps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(id);
                    //    if (tmps == null) return;
                    //    id = tmps.RtuFid;
                    //}
                    if (id < 1000000 || id > 1100000) return;

                    RtuId = id;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }


    /// <summary>
    /// attribute
    /// </summary>
    public partial class ElectricityStatisticsViewModel
    {


        #region  QueryMode
        private int _queryMode;

        /// <summary>
        /// 查询模式   1：全部设备   2：当前设备   3：区域查询  lvf 2018年6月15日09:37:17
        /// </summary>
        public int QueryMode
        {
            get { return _queryMode; }
            set
            {
                if (value != _queryMode)
                {
                    _queryMode = value;
                    this.RaisePropertyChanged(() => this.QueryMode);
                    if (QueryMode ==2 )
                    {
                        //RtuId = 0;
                        //RtuName = "通过终端树勾选终端进行故障查询.";
                        IsMultiRtus = false;
                    }else
                    {
                        IsMultiRtus = true ;
                    }
                }
            }
        }

        #endregion

        #region  IsCheckedRtu
        private bool _isCheckedRtu;

        /// <summary>
        /// 是否需要 终端详细数据
        /// </summary>
        public bool IsCheckedRtu
        {
            get { return _isCheckedRtu; }
            set
            {
                if (value != _isCheckedRtu)
                {
                    _isCheckedRtu = value;
                    this.RaisePropertyChanged(() => this.IsCheckedRtu);
                    if (IsCheckedRtu)
                    {
                        SelectTpyeId = SelectedType.Key+3;
                    }
                    else
                    {
                        SelectTpyeId = SelectedType.Key;
                    }
                }
            }
        }

        #endregion
        

        #region AreaID

        public void getAreaRId()
        {
            AreaName.Clear();
            AreaName.Add(new AreaInt() { Value = "全部", Key = -1 });
            if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {

                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                {
                    var tmlLstOfArea =
                        Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(t.Value.AreaId);
                    if (tmlLstOfArea.Count == 0) continue;
                    string area = t.Value.AreaName;
                    AreaName.Add(new AreaInt() { Value = t.Value.AreaId.ToString("d2") + "-" + area, Key = t.Value.AreaId });
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
                        AreaName.Add(new AreaInt() { Value = t.ToString("d2") + "-" + area, Key = t });
                    }
                }
            }
            AreaComboBoxSelected = AreaName[0];

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

        private bool _blgrpEnable;

        /// <summary>
        /// 
        /// </summary>
        public bool IsGrpEnable
        {
            get { return _blgrpEnable; }
            set
            {
                if (value != _blgrpEnable)
                {
                    _blgrpEnable = value;
                    this.RaisePropertyChanged(() => this.IsGrpEnable);
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
                IsGrpEnable = false; 
            }
            else
            {
                GrpVisi = Visibility.Visible;
                IsGrpEnable = true;
                var area = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(AreaId);
                if (area == null) return;
                var grps =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoList(AreaId);
                GroupName.Add(new GroupInt() { Value = "全部", Key = -1 });
                if (grps.Count > 0)
                {
                    var grpsTmp = (from t in grps orderby t.GroupId select t).ToList();
                    foreach (var f in grpsTmp)
                    {
                        var grptml =
                            Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(AreaId,
                                                                                                          f.GroupId);
                        if (grptml.Count == 0) continue;


                        GroupName.Add(new GroupInt() { Value = f.GroupName, Key = f.GroupId });
                    }
                }
                GroupComboBoxSelected = GroupName[0];
            }



        }

        #endregion

        #region RtuId

        private int _rtuId;

        /// <summary>
        /// 终端地址
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value != _rtuId)
                {
                    _rtuId = value;
                    this.RaisePropertyChanged(() => this.RtuId);
                    //if (RtuId == 0) RtuName = "通过终端树勾选终端进行故障查询.";

                    if (RtuId == 0)
                    {
                        PhyId = 0;
                        RtuName = "无";
                    }
                    //基本信息
                    var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(_rtuId);
                    if (info == null) return;
                    RtuName = info.RtuName;
                    PhyId = info.RtuPhyId;
                    //InstallDate = new DateTime(info.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
                    //Position = info.RtuInstallAddr;

                    //LoopsNum = 0;
                    ////回路信息
                    //var tmps =
                    //    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                    //        RtuId]
                    //    as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    //if (tmps == null) return;
                    //LoopsNum = tmps.WjLoops.Count;

                    ////区域信息
                    //var areaId =
                    //    Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(_rtuId);

                    //if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(areaId))
                    //{
                    //    AreaName =
                    //        Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[areaId].AreaName;
                    //}
                    //else
                    //{
                    //    AreaName = "未知";
                    //}
                    //if (areaId == 0) AreaName = "默认区域";

                }
            }
        }

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

        private string _rtuName;

        public string RtuName
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
        #endregion

        #region Records


        private ObservableCollection<ElectricityStatisticsOneItem> _records;

        public ObservableCollection<ElectricityStatisticsOneItem> Records
        {
            get { return _records ?? (_records = new ObservableCollection<ElectricityStatisticsOneItem>()); }
            set
            {
                if (_records != value)
                {
                    _records = value;
                    this.RaisePropertyChanged(() => this.Records);
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
                this.RaisePropertyChanged(() => this.Remind);
            }
        }

        #endregion

        #region DtStartTimeTime

        private DateTime _dtStartTimeTime;

        public DateTime DtStartTimeTime
        {
            get { return _dtStartTimeTime; }
            set
            {
                if (value != _dtStartTimeTime)
                {
                    _dtStartTimeTime = value;
                    this.RaisePropertyChanged(() => this.DtStartTimeTime);

                }
            }
        }

        #endregion

        #region DtEndTimeTime

        private DateTime _dtEndTimeTime;

        public DateTime DtEndTimeTime
        {
            get { return _dtEndTimeTime; }
            set
            {
                if (value != _dtEndTimeTime)
                {
                    _dtEndTimeTime = value;
                    this.RaisePropertyChanged(() => this.DtEndTimeTime);
                }
            }
        }

        #endregion

        #region IsMultiRtus

        private bool _isMultiRtus;

        public bool IsMultiRtus
        {
            get { return _isMultiRtus; }
            set
            {
                if (value == _isMultiRtus) return;
                _isMultiRtus = value;
                this.RaisePropertyChanged(() => this.IsMultiRtus);
            }
        }

        #endregion



        private ObservableCollection<StatisticsTypeItems> _IsItem;

        public ObservableCollection<StatisticsTypeItems> StatisticsType
        {
            get
            {
                if (_IsItem == null)
                {
                    _IsItem = new ObservableCollection<StatisticsTypeItems>();
                    _IsItem.Add(new StatisticsTypeItems() { Name = "日表", Key = 1 });
                    _IsItem.Add(new StatisticsTypeItems() { Name = "月表", Key = 2 });
                    _IsItem.Add(new StatisticsTypeItems() { Name = "年表", Key = 3 });
                    SelectedType = StatisticsType[0];
                }
                
                return _IsItem;
            }
        }

        public class StatisticsTypeItems : Wlst.Cr.Core.CoreServices.ObservableObject
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

            private string _name;

            public string Name
            {
                get { return _name; }
                set
                {
                    if (value != _name)
                    {
                        _name = value;
                        this.RaisePropertyChanged(() => this.Name);
                    }
                }
            }
        }


        private StatisticsTypeItems _comboboxselectedType;
        private int SelectTpyeId;

        public StatisticsTypeItems SelectedType
        {
            get { return _comboboxselectedType; }
            set
            {
                if (_comboboxselectedType != value)
                {
                    _comboboxselectedType = value;
                    this.RaisePropertyChanged(() => this.SelectedType);
                    if (value == null) return;

                    if(IsCheckedRtu)
                    {
                        SelectTpyeId = value.Key + 3;
                    }
                    else
                    {
                        SelectTpyeId = value.Key;
                    }
                }
            }
        }


    }
}






public class ElectricityStatisticsOneItem : ObservableObject
{
    public ElectricityStatisticsOneItem()
    {

        RtuName = "所有终端";
        PhyId = 0;

        RtuId = 0;
        Index = 0;
        RtuNums = "---";

    }

    #region   attri


    #region Index
    private int _index;

    public int Index
    {
        get { return _index; }
        set
        {
            if (_index != value)
            {
                _index = value;
                this.RaisePropertyChanged(() => this.Index);
            }
        }
    }
    #endregion

    #region RtuId
    private int _rtuId;

    /// <summary>
    /// 终端地址
    /// </summary>
    public int RtuId
    {
        get { return _rtuId; }
        set
        {
            if (value != _rtuId)
            {
                _rtuId = value;
                this.RaisePropertyChanged(() => this.RtuId);

                //基本信息
                var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(_rtuId);
                if (info == null) return;
                RtuName = info.RtuName;
                PhyId = info.RtuPhyId;
                //InstallDate = new DateTime(info.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
                //Position = info.RtuInstallAddr;

                //LoopsNum = 0;
                ////回路信息
                //var tmps =
                //    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                //        RtuId]
                //    as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                //if (tmps == null) return;
                //LoopsNum = tmps.WjLoops.Count;

                ////区域信息
                //var areaId =
                //    Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(_rtuId);

                //if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(areaId))
                //{
                //    AreaName =
                //        Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[areaId].AreaName;
                //}
                //else
                //{
                //    AreaName = "未知";
                //}
                //if (areaId == 0) AreaName = "默认区域";

            }
        }
    }

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

    private string _rtuName;

    public string RtuName
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

    #endregion

    #region RtuNums
    private string _rtuNums;

    public string RtuNums
    {
        get { return _rtuNums; }
        set
        {
            if (_rtuNums != value)
            {
                _rtuNums = value;
                this.RaisePropertyChanged(() => this.RtuNums);
            }
        }
    }
    #endregion

    #region Days
    private int _days;

    public int Days
    {
        get { return _days; }
        set
        {
            if (_days != value)
            {
                _days = value;
                this.RaisePropertyChanged(() => this.Days);
            }
        }
    }
    #endregion

    #region DateCreate
    private string _dateCreate;

    /// <summary>
    /// 客户的接收到数据的时间
    /// </summary>
    public string DateCreate
    {
        get { return _dateCreate; }
        set
        {
            if (value != _dateCreate)
            {
                _dateCreate = value;
                this.RaisePropertyChanged(() => this.DateCreate);
            }
        }
    }
    #endregion

    #region LngDateCreate
    private long _lngDateCreate;

    /// <summary>
    /// 
    /// </summary>
    public long LngDateCreate
    {
        get { return _lngDateCreate; }
        set
        {
            if (value != _lngDateCreate)
            {
                _lngDateCreate = value;
                this.RaisePropertyChanged(() => this.LngDateCreate);
            }
        }
    }

    #endregion

    #region Aelec
    private double _aelec;

    /// <summary>
    ///  A相电能
    /// </summary>
    public double Aelec
    {
        get { return _aelec; }
        set
        {
            if (_aelec != value)
            {
                _aelec = value;
                this.RaisePropertyChanged(() => this.Aelec);
            }
        }
    }
    #endregion

    #region Belec
    private double _belec;

    /// <summary>
    ///  B相电能
    /// </summary>
    public double Belec
    {
        get { return _belec; }
        set
        {
            if (_belec != value)
            {
                _belec = value;
                this.RaisePropertyChanged(() => this.Belec);
            }
        }
    }
    #endregion

    #region Celec
    private double _celec;

    /// <summary>
    /// C相电能
    /// </summary>
    public double Celec
    {
        get { return _celec; }
        set
        {
            if (_celec != value)
            {
                _celec = value;
                this.RaisePropertyChanged(() => this.Celec);
            }
        }
    }
    #endregion

    #region Abcelec
    private double _abcelec;

    /// <summary>
    /// 总电能
    /// </summary>
    public double Abcelec
    {
        get { return _abcelec; }
        set
        {
            if (_abcelec != value)
            {
                _abcelec = value;
                this.RaisePropertyChanged(() => this.Abcelec);
            }
        }
    }
    #endregion

    #endregion

}

