using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.Wj2090Module.NewData.PartolView.Models;
using Wlst.Ux.Wj2090Module.NewData.PartolView.Services;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;


namespace Wlst.Ux.Wj2090Module.NewData.DataSluAssisQuery
{
    [Export(typeof(IIDataSluAssisQuery))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class DataSluAssisQueryViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IIDataSluAssisQuery
    {
        public DataSluAssisQueryViewModel()
        {
            InitEvent();
            InitAciton();


        }

        private bool _thisViewActive = false;

        public void NavOnLoad(params object[] parsObjects)
        {
            _thisViewActive = true;
            DtEndTime = DateTime.Now;
            DtStartTime = DateTime.Now.AddDays(-1);
            try
            {
                CurrentSluId = Convert.ToInt32(parsObjects[0]);
            }
            catch (Exception ex)
            {

            }
            if ( CurrentSluId>1500000 && CurrentSluId<1599999) Ex();

        }
        private static int CurrentSluId = 0;
        public static void SetCurrentSluId(int sluId)
        {
            CurrentSluId = sluId;
        }

        public void OnUserHideOrClosing()
        {
            _thisViewActive = false;
            CurrentSluId = 0;
            SluMaxMinItems.Clear();
        }

        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "单灯巡测数据"; }
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


        private string _remind;

        public string Remind
        {
            get { return _remind; }
            set
            {
                if (_remind == value) return;
                _remind = value;
                RaisePropertyChanged(() => Remind);
            }
        }



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


        private ObservableCollection<SingleMaxMinData> _sluMaxMinItems;

        public ObservableCollection<SingleMaxMinData> SluMaxMinItems
        {
            get
            {
                if (_sluMaxMinItems == null)
                {
                    _sluMaxMinItems = new ObservableCollection<SingleMaxMinData>();

                }
                return _sluMaxMinItems;
            }
            set
            {
                if (_sluMaxMinItems == value) return;
                _sluMaxMinItems = value;
                RaisePropertyChanged(() => SluMaxMinItems);
            }
        }

    
    
    
    
    }


    //Action
    public partial class DataSluAssisQueryViewModel
    {
        private void InitAciton()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSlu.wst_slu_max_min_data, OnSluMeasure,
                typeof(DataSluAssisQueryViewModel), this);
        }

        private void OnSluMeasure(string sessionid, Wlst.mobile.MsgWithMobile info)
        {
            if (info == null) return;
            if (_thisViewActive == false) return;
            var data = info.WstSluMaxMinData;
            if (data == null) return;
            if (data.SluId.Count == 0) return; 
            var sluId = data.SluId[0];
            if (sluId < 1) return;
            if (sluId != CurrentSluId) return;

            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  返回数据";

            if (data.SluMaxMinDataItems != null && data.SluMaxMinDataItems.Count > 0)
            {
                int indexx =1;
                foreach (var g in data.SluMaxMinDataItems)
                {
                    if (g == null) continue;

                    var gg = new SingleMaxMinData(sluId, g.CtrlId, g);
                    gg.Index = indexx++;

                    this.SluMaxMinItems.Add(gg);
      
                }
                //Remind += "控制器辅助数据;";
            }

        
            //Remind += "请手动翻阅。";
        }


        private void InitEvent()
        {
            EventPublish.AddEventTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);

        }

        public void FundEventHandler(PublishEventArgs args) // should do somework
        {


            try
            {
                if (args.EventType == PublishEventType.Core)
                {


                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {
                        if (_thisViewActive == false) return;
                        //if (OptionXmlSvr.GetOptionInt(4001, 2) == 1) return;
                        //if (!_isViewShow) return;
                        if (args.EventAttachInfo == "RequestDataWhenErrorHappenEqu") return;
                        int id = Convert.ToInt32(args.GetParams()[0]);
                        //if (id < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.RtuStart) return;
                        if (CurrentSluId > 1599999 || CurrentSluId < 1500000) return;
                        if (CurrentSluId == id) return;
                        CurrentSluId = id;


                    }

             

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
                Cr.Core.UtilityFunction.WriteLog.WriteLogError(
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
                        //无视多选框
                        //if (OptionXmlSvr.GetOptionInt(4001, 2) == 1) return false;

                        return true;
                    }
                  


                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }


    }



    public partial class DataSluAssisQueryViewModel
    {

        #region CmdQuery

        private DateTime _dtQuery;

        public ICommand CmdQuery
        {
            get { return new RelayCommand(Ex, CanEx, true); }
        }


        private void Ex()
        {
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮

            _dtQuery = DateTime.Now;

            if (!GetCheckedInformation()) return;
            SluMaxMinItems.Clear();
            var rtulst = new List<int>();


            if (CurrentSluId == 0)
            {
                UMessageBox.Show("提醒", "请选择设备", UMessageBoxButton.Ok);
                return;
            }
            rtulst = new List<int>();
            rtulst.Add(CurrentSluId);

            ReqeustData(rtulst, DtStartTime, DtEndTime);
            Remind = "查询命令已发送...请等待数据反馈！";
        }

        /// <summary>
        /// 请求亮灯率数据
        /// </summary>
        /// <param name="rtuId">设备地址</param>
        /// <param name="dtStartTime">起始时间</param>
        /// <param name="dtEndtime">结束时间</param>
        public static void ReqeustData(List<int> rtuId, DateTime dtStartTime, DateTime dtEndtime)
        {
            //if (rtuId.Count == 0) return; 
            var dts = new DateTime(dtStartTime.Year, dtStartTime.Month, dtStartTime.Day, 0, 0, 1);
            var dte = new DateTime(dtEndtime.Year, dtEndtime.Month, dtEndtime.Day, 23, 59, 59);
            var info = Wlst.Sr.ProtocolPhone.LxSlu.wst_slu_max_min_data;
            info.WstSluMaxMinData.SluId = rtuId;
            info.WstSluMaxMinData.DtEnd = dte.Ticks;
            info.WstSluMaxMinData.DtStart = dts.Ticks;

            SndOrderServer.OrderSnd(info, 10, 6);
        }


        private bool CanEx()
        {

            if (DtStartTime > DtEndTime) return false;



            return DateTime.Now.Ticks - _dtQuery.Ticks > 3000000;
        }

        /// <summary>
        /// 是否可以点击查询按钮
        /// </summary>
        /// <returns></returns>
        private bool GetCheckedInformation()
        {
            if (DtStartTime.AddDays(63) < DtEndTime)
            {
                UMessageBox.Show("提醒", "请重新选择时间，时间需选择在62天以内", UMessageBoxButton.Ok);
                //WLSTMessageBox.WpfMessageBox.Show("请重新选择时间，时间需选择在30天以内");
                return false;
            }
            return true;
        }

        #endregion

        // 导出excel

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
                lsttitle.Add("集中器地址");
                lsttitle.Add("集中器名称");
                lsttitle.Add("控制器地址");
                lsttitle.Add("控制器名称");
                lsttitle.Add("灯杆编码");
                lsttitle.Add("最大电流");
                lsttitle.Add("最大电压");
                lsttitle.Add("最小电流");
                lsttitle.Add("最小电压");

                lsttitle.Add("条形码");

                var lstobj = new List<List<object>>();

                foreach (var g in SluMaxMinItems)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.Index);
                    tmp.Add(g.SluId);
                    tmp.Add(g.SluName);
                    tmp.Add(g.CtrlId);
                    tmp.Add(g.CtrlName);
                    tmp.Add(g.CtrlLampCode);
                    tmp.Add(g.MaxCurrent);
                    tmp.Add(g.MaxVoltage);
                    tmp.Add(g.MinCurrent);
                    tmp.Add(g.MinVoltage);
                    tmp.Add(g.BarCode );

                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
            }

        }

        private bool CanExCmdExport()
        {
            if (SluMaxMinItems.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
            return false;
        }

        #endregion

    }
}
