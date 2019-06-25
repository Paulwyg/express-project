using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.Wj2096Module.NewData.CtrlData.Services;

namespace Wlst.Ux.Wj2096Module.NewData.CtrlData.ViewModel
{

    [Export(typeof (IINewData))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewDataViewModel : Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged,
                                            IINewData,Wlst .Cr .CoreMims .CoreInterface .IIShowData 
    {
        public void NavOnLoad(params object[] parsObjects)
        {

        }

        public NewDataViewModel()
        {
            //this.AddEventFilterInfo( Wlst .Sr .EquipmentInfoHolding .Services.EventIdAssign.RunningInfoUpdate2 , PublishEventType.Core);
            this.AddEventFilterInfo(Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core);
            //this.AddEventFilterInfo(
           //     Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuDataQueryDataInfoNeedShowInTab,
           //     PublishEventType.Core);
            this.AddEventFilterInfo(
                Wlst.Sr.SlusglInfoHold.Services.EventIdAssign.SluSglMeasure,
                PublishEventType.Core);
        }


        protected int CurrentSelectedRtuId = 0;
        protected int CurrentSelectedCtrlId = 0;
        protected int CurrentApplicationSelectd = 0;
        public override void ExPublishedEvent(
            PublishEventArgs args)
        {
            try
            {

                //if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)
                //{
                //    if (args.GetParams().Count == 0) return;
                //    var rtuids = args.GetParams()[0] as List<int>;
                //    if (rtuids == null || rtuids.Count == 0) return;

                //    if (args.GetParams().Count < 2)
                //    {
                //        if (rtuids.Contains(CurrentSelectedRtuId))
                //        {
                //            //OnSluDataArrive(lst);
                //            OnSelectRtu(CurrentSelectedRtuId, 0);
                //        }
                //        return;
                //    }

                //    var lst = args.GetParams()[1] as List<int>;
                //    if (rtuids.Contains(CurrentSelectedRtuId))
                //    {
                //        OnSluDataArrive(lst);
                //    }
                //}
                if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {

                    int rtuid = Convert.ToInt32(args.GetParams()[0]);
                    CurrentApplicationSelectd = rtuid;
                   // CurrentSelectedCtrlId = 0;
                    if (rtuid > 1700000 && rtuid <1800000)
                    {
                        // OnSluDataUpdate();
                        if (args.GetParams().Count > 1)
                        {
                            int ctrlid = Convert.ToInt32(args.GetParams()[1]);
                            if (ctrlid < 1) ctrlid = 0;
                            OnSelectRtu(rtuid, ctrlid);

                           // DateTimeCtrl = DateTime.Now.Ticks+"";
                        }
                        else
                        {
                            OnSelectRtu(rtuid, 0);
                        }

                    }
                }
                //if(args .EventId ==Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuDataQueryDataInfoNeedShowInTab)
                //{
                //    var info = args.GetParams()[0] as Wlst.client.SluCtrlDataMeasureReply.DataSluCtrlData;
                //    if (info == null) return;
                //    OnOtherViewShowData(info);
                //    SelectedViewId = 5;
                //}
                if (args.EventId == Wlst.Sr.SlusglInfoHold.Services.EventIdAssign.SluSglMeasure)
                {
                    if (args.GetParams().Count == 0) return;
                    var rtuids = args.GetParams()[0] as List<int>;
                    if (rtuids == null || rtuids.Count == 0) return;

                    if (!rtuids.Contains(CtrlId)) return;
                    
                    if (args.GetParams().Count < 2)
                    {
                        if (rtuids.Contains(CtrlId))
                        {
                            OnSelectRtu(0, CtrlId);
                        }
                        return;
                    }

                    //var lst = args.GetParams()[1] as List<int>;
                    //if (rtuids.Contains(CtrlId))
                    //{
                    //    OnSluDataArrive(lst);
                    //}

                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("NB2096 New Data处理出错:" + ex);
            }
        }


        void OnOtherViewShowData(Wlst.client.SluCtrlDataMeasureReply.DataSluCtrlData data)
        {
            try
            {
                if (data == null) return;

               UpdateCtrlData5(data );
                Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(
               Ux.Wj2096Module.Services.ViewIdAssign.NewDataViewId);
            }
            catch (Exception ex)
            {

            }
        }

        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "最新数据"; }
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


        private int  _lDatsdfsdfeCreate;

        public int  SelectedViewId
        {
            get { return _lDatsdfsdfeCreate; }
            set
            {
                if (_lDatsdfsdfeCreate == value) return;
                _lDatsdfsdfeCreate = value;
                RaisePropertyChanged(() => SelectedViewId);
            }
        }
   

        public static int GetPhyIdByRtuId(int sluId,int ctrId)
        {
            var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(sluId, ctrId);
            if (para != null)
                return para.CtrlId;
            return 0;
        }
        static string  GetLampCode(int sluid, int ctrId)
        {
            var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(sluid, ctrId);
            if (para == null) return "";

            return para.CtrlName;

        }

  
    }



    

    /// <summary>
    /// Ctrl Attri Phy 4
    /// </summary>
    public partial class NewDataViewModel
    {

        #region attri

        /// <summary>
        /// 数据发生时间  与回路数据联合查询组合成最新数据
        /// </summary>

        private string _lDateCreate;

        public string DateCreate
        {
            get { return _lDateCreate; }
            set
            {
                if (_lDateCreate == value) return;
                _lDateCreate = value;
                RaisePropertyChanged(() => DateCreate);
            }
        }
        private int _lRtuId;
        public int RtuId
        {
            get { return _lRtuId; }
            set
            {
                if (_lRtuId == value) return;
                _lRtuId = value;
                RaisePropertyChanged(() => RtuId);
            }
        }

        private int _lsdfsdId;
        public int RtuPhyId
        {
            get { return _lsdfsdId; }
            set
            {
                if (_lsdfsdId == value) return;
                _lsdfsdId = value;
                RaisePropertyChanged(() => RtuPhyId);
            }
        }
        private string _lRRtuName;
        public string RtuName
        {
            get { return _lRRtuName; }
            set
            {
                if (_lRRtuName == value) return;
                _lRRtuName = value;
                RaisePropertyChanged(() => RtuName);
            }
        }

        private int _isdfsdfndexsdf;

        public int CtrlId
        {
            get { return _isdfsdfndexsdf; }
            set
            {
                if (_isdfsdfndexsdf == value) return;
                _isdfsdfndexsdf = value;
                RaisePropertyChanged(() => CtrlId);
            }
        }

        private int _issdfsddfsdfndexsdf;

        public int CtrlPhyId
        {
            get { return _issdfsddfsdfndexsdf; }
            set
            {
                if (_issdfsddfsdfndexsdf == value) return;
                _issdfsddfsdfndexsdf = value;
                RaisePropertyChanged(() => CtrlPhyId);
            }
        }


        /// <summary>
        /// 灯杆编码
        /// </summary>
        private string _ctrlLampCode;

        public string CtrlLampCode
        {
            get { return _ctrlLampCode; }
            set
            {
                if (_ctrlLampCode == value) return;
                _ctrlLampCode = value;
                RaisePropertyChanged(() => CtrlLampCode);
            }
        }

        /// <summary>
        /// 序号
        /// </summary>

        #region SluId

        private int _indexsdf;

        public int SignalStrength
        {
            get { return _indexsdf; }
            set
            {
                if (_indexsdf == value) return;
                _indexsdf = value;
                RaisePropertyChanged(() => SignalStrength);
            }
        }

        private string _inPhasedexsdf;

        public string  Phase
        {
            get { return _inPhasedexsdf; }
            set
            {
                if (_inPhasedexsdf == value) return;
                _inPhasedexsdf = value;
                RaisePropertyChanged(() => Phase);
            }
        }
        private int _iUsefulCommunicatendexsdf;

        public int UsefulCommunicate
        {
            get { return _iUsefulCommunicatendexsdf; }
            set
            {
                if (_iUsefulCommunicatendexsdf == value) return;
                _iUsefulCommunicatendexsdf = value;
                RaisePropertyChanged(() => UsefulCommunicate);
            }
        }

        private int _indAllCommunicateexsdf;

        public int AllCommunicate
        {
            get { return _indAllCommunicateexsdf; }
            set
            {
                if (_indAllCommunicateexsdf == value) return;
                _indAllCommunicateexsdf = value;
                RaisePropertyChanged(() => AllCommunicate);
            }
        }    private int _indeCtrlLoopxsdf;

        public int CtrlLoop
        {
            get { return _indeCtrlLoopxsdf; }
            set
            {
                if (_indeCtrlLoopxsdf == value) return;
                _indeCtrlLoopxsdf = value;
                RaisePropertyChanged(() => CtrlLoop);
            }
        }




        private string _indsdfsdfdf;

        public string PowerSaving
        {
            get { return _indsdfsdfdf; }
            set
            {
                if (_indsdfsdfdf == value) return;
                _indsdfsdfdf = value;
                RaisePropertyChanged(() => PowerSaving);
            }
        }

        private string _index;

        public string HasLeakage
        {
            get { return _index; }
            set
            {
                if (_index == value) return;
                _index = value;
                RaisePropertyChanged(() => HasLeakage);
            }
        }

        #endregion


        private string _lHasTemperature;

        public string HasTemperature
        {
            get { return _lHasTemperature; }
            set
            {
                if (_lHasTemperature == value) return;
                _lHasTemperature = value;
                RaisePropertyChanged(() => HasTemperature);
            }
        }


        private string _lDateReply;

        public string HasTimer
        {
            get { return _lDateReply; }
            set
            {
                if (_lDateReply == value) return;
                _lDateReply = value;
                RaisePropertyChanged(() => HasTimer);
            }
        }


        private string _liUserName;

        public string Model
        {
            get { return _liUserName; }
            set
            {
                if (_liUserName == value) return;
                _liUserName = value;
                RaisePropertyChanged(() => Model);
            }
        }

        #endregion

    }

    /// <summary>
    /// Ctrl Attri Data 5 
    /// </summary>
    public partial class NewDataViewModel
    {
        private void UpdateCtrlData5(int sluId,int ctrlId)
        {
            var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(sluId);

            if(para == null)return;

            this.RtuId = sluId;
            this.RtuPhyId = para.PhyId;
            this.RtuName = para.FieldName;
            this.CtrlId = ctrlId;
            this.CtrlPhyId = GetPhyIdByRtuId(sluId,ctrlId);
            this.CtrlLampCode = GetLampCode(sluId,ctrlId);
            //var tukey = new Tuple<int, int>(sluId, ctrlId);


            var runninfo = Wlst.Sr.SlusglInfoHold.Services.SluSglCtrlDataHold.GetRunInfo(ctrlId);
            if (runninfo == null || runninfo.SluCtrlNewData == null || runninfo.SluCtrlNewData .ContainsKey( ctrlId )==false )
            {
                ResetAllAttri5();
                return;
            }


            var tmp = runninfo.SluCtrlNewData[ctrlId ].Data5;
            if (tmp == null)
            {
                ResetAllAttri5();
                return;
            }

            DateCreate = new DateTime(tmp.Info.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
            OrderId = tmp.Info.OrderId;
            DateTimeCtrl = new DateTime(tmp.Info.DateTimeCtrl).ToString("yyyy-MM-dd HH:mm:ss");
            Temperature = tmp.Info.Temperature;
            Status = tmp.Info.Status == 0
                         ? "正常"
                         : tmp.Info.Status == 1
                               ? "电压越上限"
                               : tmp.Info.Status == 2
                                     ? "电压越下限"
                                     : "通讯故障";
            IsAdjust = tmp.Info.IsAdjust ? "已校准" : "未校准";
            IsWorkingArgsSet = tmp.Info.IsWorkingArgsSet ? "已设置" : "未设置";
            IsNoAlarm = tmp.Info.IsNoAlarm ? "禁止" : "允许";
            IsCtrlStop = tmp.Info.IsCtrlStop ? "停运" : "正常";
            IsEepromError = tmp.Info.IsEepromError ? "故障" : "正常";
            IsTemperatureSensor = tmp.Info.IsTemperatureSensor ? "故障" : "正常";

            DataSluCtrlLampItems.Clear();

            foreach (var t in tmp .Items )
            {
                this.DataSluCtrlLampItems.Add(new DataSluCtrlLampVm(t,tmp.Info .Status ));
            }
        }


        private void ResetAllAttri5()
        {
            DateCreate = "----";
            DateTimeCtrl = "----";
            DataSluCtrlLampItems.Clear();
        }

        private void UpdateCtrlData5(Wlst.client.SluCtrlDataMeasureReply.DataSluCtrlData data)
        {
            int sluId = data.Info.SluId;
           // int ctrlData = data.Info.CtrlId;

            var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(sluId);

            this.RtuId = sluId;
            this.RtuPhyId = para.PhyId;
            this.RtuName = para.FieldName;

            this.CtrlId = data .Info .CtrlId ;
            this.CtrlPhyId = GetPhyIdByRtuId(data.Info.SluId, data.Info.CtrlId);
            this.CtrlLampCode = GetLampCode(sluId, data.Info.CtrlId);


            var tmp = data;

            DateCreate = new DateTime(tmp.Info.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
            OrderId = tmp.Info.OrderId;
            if(tmp .Info .DateTimeCtrl <0)
            {
                DateTimeCtrl = "-- 终端报警数据 --" + "  -- 历史数据 --";
            }
           else  if (tmp.Info.DateTimeCtrl == 0)
            {
                DateTimeCtrl = "-- 通信故障 --" + "  -- 历史数据 --";
            }
            else 
            DateTimeCtrl = new DateTime(tmp.Info.DateTimeCtrl).ToString("yyyy-MM-dd HH:mm:ss")+"  -- 历史数据 --";
            Temperature = tmp.Info.Temperature;
            Status = tmp.Info.Status == 0
                         ? "正常"
                         : tmp.Info.Status == 1
                               ? "电压越上限"
                               : tmp.Info.Status == 2
                                     ? "电压越下限"
                                     : "通讯故障";
            IsAdjust = tmp.Info.IsAdjust ? "已校准" : "未校准";
            IsWorkingArgsSet = tmp.Info.IsWorkingArgsSet ? "已设置" : "未设置";
            IsNoAlarm = tmp.Info.IsNoAlarm ? "禁止" : "允许";
            IsCtrlStop = tmp.Info.IsCtrlStop ? "停运" : "正常";
            IsEepromError = tmp.Info.IsEepromError ? "故障" : "正常";
            IsTemperatureSensor = tmp.Info.IsTemperatureSensor ? "故障" : "正常";

            DataSluCtrlLampItems.Clear();

            foreach (var t in tmp.Items)
            {
                this.DataSluCtrlLampItems.Add(new DataSluCtrlLampVm(t, tmp.Info.Status));
            }
        }

        #region attri



        private ObservableCollection<DataSluCtrlLampVm> _iDataSluCtrlLamptems;

        public ObservableCollection<DataSluCtrlLampVm> DataSluCtrlLampItems
        {
            get
            {
                if (_iDataSluCtrlLamptems == null)
                {
                    _iDataSluCtrlLamptems = new ObservableCollection<DataSluCtrlLampVm>();

                }
                return _iDataSluCtrlLamptems;
            }
            set
            {
                if (_iDataSluCtrlLamptems == value) return;
                _iDataSluCtrlLamptems = value;
                RaisePropertyChanged(() => DataSluCtrlLampItems);
            }
        }



        public int SluIOrderIdd;

        /// <summary>
        /// 控制器地址 排序地址
        /// </summary>

        public int OrderId
        {
            get { return SluIOrderIdd; }
            set
            {
                if (SluIOrderIdd == value) return;
                SluIOrderIdd = value;
                RaisePropertyChanged(() => OrderId);
            }
        }

        public string SluDateTimeCtrl;

        /// <summary>
        /// 日 时:分  数据在控制器中生成的时间
        /// </summary>

        public string DateTimeCtrl
        {
            get { return SluDateTimeCtrl; }
            set
            {
                if (SluDateTimeCtrl == value) return;
                SluDateTimeCtrl = value;
                RaisePropertyChanged(() => DateTimeCtrl);
            }
        }

        public int SluIsdTemperature;

        /// <summary>
        /// 温度
        /// </summary>

        public int Temperature
        {
            get { return SluIsdTemperature; }
            set
            {
                if (SluIsdTemperature == value) return;
                SluIsdTemperature = value;
                RaisePropertyChanged(() => Temperature);
            }
        }

        public string SluStatus;

        /// <summary>
        /// 状态 0-正常，1-电压越上限，2-电压越下限，3-通讯故障
        /// </summary>

        public string Status
        {
            get { return SluStatus; }
            set
            {
                if (SluStatus == value) return;
                SluStatus = value;
                RaisePropertyChanged(() => Status);
            }
        }

        public string SluIsAdjust;

        /// <summary>
        /// 已校准 0-未校准，1-已校准
        /// </summary>

        public string IsAdjust
        {
            get { return SluIsAdjust; }
            set
            {
                if (SluIsAdjust == value) return;
                SluIsAdjust = value;
                RaisePropertyChanged(() => IsAdjust);
            }
        }

        public string SlIsWorkingArgsSet;

        /// <summary>
        /// 工作参数设置 0-未设置，1-已设置
        /// </summary>

        public string IsWorkingArgsSet
        {
            get { return SlIsWorkingArgsSet; }
            set
            {
                if (SlIsWorkingArgsSet == value) return;
                SlIsWorkingArgsSet = value;
                RaisePropertyChanged(() => IsWorkingArgsSet);
            }
        }

        public string SlIsNoAlarm;

        /// <summary>
        /// 禁止主动报警 0-允许，1-禁止
        /// </summary>

        public string IsNoAlarm
        {
            get { return SlIsNoAlarm; }
            set
            {
                if (SlIsNoAlarm == value) return;
                SlIsNoAlarm = value;
                RaisePropertyChanged(() => IsNoAlarm);
            }
        }

        public string SlIsCtrlStop;

        /// <summary>
        /// 停运 0-正常，1-停运
        /// </summary>

        public string IsCtrlStop
        {
            get { return SlIsCtrlStop; }
            set
            {
                if (SlIsCtrlStop == value) return;
                SlIsCtrlStop = value;
                RaisePropertyChanged(() => IsCtrlStop);
            }
        }

        public string SIsEepromError;

        /// <summary>
        /// EEPROM故障 0-正常，1-故障
        /// </summary>

        public string IsEepromError
        {
            get { return SIsEepromError; }
            set
            {
                if (SIsEepromError == value) return;
                SIsEepromError = value;
                RaisePropertyChanged(() => IsEepromError);
            }
        }


        public string SIsTemperatureSensor;

        /// <summary>
        /// 温度传感器故障 0-正常，1-故障
        /// </summary>

        public string IsTemperatureSensor
        {
            get { return SIsTemperatureSensor; }
            set
            {
                if (SIsTemperatureSensor == value) return;
                SIsTemperatureSensor = value;
                RaisePropertyChanged(() => IsTemperatureSensor);
            }
        }


        #endregion

    }

    /// <summary>
    /// 逻辑控制
    /// </summary>
    public partial class NewDataViewModel
    {
        private void ShowView()
        {
            Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(
                Ux.Wj2096Module.Services.ViewIdAssign.NewDataViewId);
        }

        private void OnSluDataArrive(List<int> types)
        {

                if (CurrentSelectedCtrlId != 0)
                {
                     var info = Wlst.Sr.SlusglInfoHold.Services.SluSglCtrlDataHold.GetRunInfo(CurrentSelectedRtuId);
               
                    if(info ==null ||info .SluCtrlNewData ==null ||info .SluCtrlNewData .ContainsKey( CurrentSelectedRtuId )==false )
                    {
                        return;
                    }

                    Sr.SlusglInfoHold.Services.CtrlMeasureInfo tmps = info.SluCtrlNewData[CurrentSelectedCtrlId];
                    var dirr = DateTime.Now.Ticks - tmps.LastUpdateTime;
                    if (dirr > 30000000) return; //间隔三秒  该控制器数据未更新的
                    if (CurrentApplicationSelectd == CurrentSelectedRtuId)
                    {
                        ShowView();
                    }

                        UpdateCtrlData5(CurrentSelectedRtuId, CurrentSelectedCtrlId);
                        SelectedViewId = 5;
                        return;

                }
        }


        private void OnSelectRtu(int sluId, int ctrlid = 0)
        {
            var para = Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(ctrlid);
            CurrentSelectedRtuId = para;

            CurrentSelectedCtrlId = ctrlid;

            ShowView();

            UpdateCtrlData5(para, ctrlid);
            SelectedViewId = 5;

        }
    }
}
