using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Ux.WJ3005Module.ZZhaoCe.ZhaoCeRtuHolidaySetViewModel.Services;


namespace Wlst.Ux.WJ3005Module.ZZhaoCe.ZhaoCeRtuHolidaySetViewModel.ViewModel
{
    [Export(typeof(IIZhaoCeRtuHolidaySetViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ZhaoCeRtuHolidaySetViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IIZhaoCeRtuHolidaySetViewModel
    {
        public ZhaoCeRtuHolidaySetViewModel()
        {
            InitAction();
        }


        private ObservableCollection<OneRtuZhaoCeHolidayTime> _rtusHolidaySet;

        public ObservableCollection<OneRtuZhaoCeHolidayTime> RtusHoliday
        {
            get
            {
                if (_rtusHolidaySet == null)
                    _rtusHolidaySet = new ObservableCollection<OneRtuZhaoCeHolidayTime>();
                return _rtusHolidaySet;
            }
        }
        public void OnUserHideOrClosing()
        {
            this.RtusHoliday.Clear();
        }

        private OneRtuZhaoCeHolidayTime _currentSelectedItem;

        public OneRtuZhaoCeHolidayTime CurrentSelectedItem
        {
            get { return _currentSelectedItem; }
            set
            {
                if (_currentSelectedItem != value)
                {
                    _currentSelectedItem = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectedItem);
                }
            }
        }

        private void AddRtuZhaoceInfo(int rtuId, List<Wlst .client .HolidaySchduleTimeItem >  info,int op)
        {
            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
                WJ3005Module.Services.ViewIdAssign.ZhaoCeRtuHolidaySetViewId , true);


            foreach (var t in RtusHoliday)
            {
                if (t.RtuId == rtuId)
                {

                    t.AddHolidayData(rtuId, info,op);

                    CurrentSelectedItem = t;
                    return;
                }
            }
            var ff = new OneRtuZhaoCeHolidayTime(rtuId, info,op);
            this.RtusHoliday.Add(ff);
            CurrentSelectedItem = ff;

        }

        
        #region delete current



        private ICommand _deleteCurrentCommand;

        public ICommand DeleteCurrentCommand
        {
            get
            {
                if (_deleteCurrentCommand == null)
                    _deleteCurrentCommand = new RelayCommand(ExDelete, CanExDelete,false );
                return _deleteCurrentCommand;
            }
        }

        private bool CanExDelete()
        {
            return this.RtusHoliday.Count > 0;
        }

        private void ExDelete()
        {
            if (this.RtusHoliday.Contains(this.CurrentSelectedItem))
            {
                this.RtusHoliday.Remove(this.CurrentSelectedItem);
                if (this.RtusHoliday.Count > 0) this.CurrentSelectedItem = this.RtusHoliday[this.RtusHoliday.Count - 1];
            }
            if (this.RtusHoliday.Count < 1)
            {
                Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
                    WJ3005Module.Services.ViewIdAssign.ZhaoCeRtuWeekSetViewIdFor4, false);
            }
        }

        #endregion

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
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "召测节假日设置"; }
        }
    }

    public partial class ZhaoCeRtuHolidaySetViewModel
    {
  

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxRtu .wst_zc_rtu_info ,// .wlst_svr_ans_cnt_request_zc_holiday_info ,//.ClientPart.wlst_TimeTable_server_ans_clinet_order_zc_time_holiday   ,
                ZcRtuHolidaySetS1S4Info,
                typeof(ZhaoCeRtuHolidaySetViewModel), this);
        }

        public void ZcRtuHolidaySetS1S4Info(string session,Wlst .mobile .MsgWithMobile  args)
        {

            try
            {
                if (args == null || args.WstRtuZcInfo == null) return;
                if (args.WstRtuZcInfo.Op < 11 || args.WstRtuZcInfo.Op > 13) return;

                int rtuId = args.WstRtuZcInfo.RtuId;


                //lvf 如果不是本客户端操作的 不处理 2018年8月9日09:34:11
                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.ContainsKey(rtuId) == false) return;
                System.TimeSpan t = DateTime.Now - Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus[rtuId];
                if(t.Minutes>5)
                {
                    //可以不清除,但可能占用内存
                    Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.Remove(rtuId);
                    return;
                }

                //Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.Remove(rtuId);

                var rtuZhaoCeInfo = args.WstRtuZcInfo.HolidayTimes;
                if (rtuZhaoCeInfo == null) return;
                this.AddRtuZhaoceInfo(rtuId, rtuZhaoCeInfo, args.WstRtuZcInfo.Op - 10);

                string name = "未解析名称";
                var rtuInfomation =
                           Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                               InfoItems [rtuId];
                if (rtuInfomation != null)
                {
                    name = rtuInfomation.RtuName;
                }

                 Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                    rtuId, name, Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.ServerReply, "终端召测节假日设置数据");
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }

     

    }

}
