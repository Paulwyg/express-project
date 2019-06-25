using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.Wj1090Module.LduTreeSettingViewModel.Services;
using Wlst.Ux.Wj1090Module.LduTreeSettingViewModel.ViewModel;
using Wlst.Ux.Wj1090Module.NewData.Services;
using Wlst.Ux.Wj1090Module.Wj1090DataSelection.ViewModel;
using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.Wj1090Module.NewData.ViewModel
{
    [Export(typeof(IINewData))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewDataViewModel :ObservableObject, IINewData, Wlst.Cr.CoreMims.CoreInterface.IIShowData
    {
        public NewDataViewModel ()
        {
            this.InitAction();
            this.InitEvent();
        } 

        #region 采集时间是否显示
        private bool _isShowCJSJ;

        public bool IsShowCJSJ
        {
            get { return _isShowCJSJ; }
            set
            {
                if (_isShowCJSJ != value)
                {
                    _isShowCJSJ = value;
                    this.RaisePropertyChanged(() => this.IsShowCJSJ);

                }
            }
        }
        #endregion

        #region 状态是否显示
        private bool _isShowZT;

        public bool IsShowZT
        {
            get { return _isShowZT; }
            set
            {
                if (_isShowZT != value)
                {
                    _isShowZT = value;
                    this.RaisePropertyChanged(() => this.IsShowZT);

                }
            }
        }
        #endregion

        #region 有功功率是否显示
        private bool _isShowYGGL;

        public bool IsShowYGGL
        {
            get {return _isShowYGGL; }
            set
            {
                if (_isShowYGGL != value)
                {
                    _isShowYGGL = value;
                    this.RaisePropertyChanged(() => this.IsShowYGGL);

                }
            }
        }
        #endregion

        #region 无功功率是否显示
        private bool _isShowWGGL;

        public bool IsShowWGGL
        {
            get { return _isShowWGGL; }
            set
            {
                if (_isShowWGGL != value)
                {
                    _isShowWGGL = value;
                    this.RaisePropertyChanged(() => this.IsShowWGGL);

                }
            }
        }
        #endregion

        #region 功率因数是否显示
        private bool _isShowGLYS;

        public bool IsShowGLYS
        {
            get { return _isShowGLYS; }
            set
            {
                if (_isShowGLYS != value)
                {
                    _isShowGLYS = value;
                    this.RaisePropertyChanged(() => this.IsShowGLYS);

                }
            }
        }
        #endregion

        #region 亮灯率是否显示
        private bool _isShowLDL;

        public bool IsShowLDL
        {
            get { return _isShowLDL; }
            set
            {
                if (_isShowLDL != value)
                {
                    _isShowLDL = value;
                    this.RaisePropertyChanged(() => this.IsShowLDL);

                }
            }
        }
        #endregion

        #region 信号强度是否显示
        private bool _isShowXHQD;

        public bool IsShowXHQD
        {
            get { return _isShowXHQD; }
            set
            {
                if (_isShowXHQD != value)
                {
                    _isShowXHQD = value;
                    this.RaisePropertyChanged(() => this.IsShowXHQD);

                }
            }
        }
        #endregion

        #region 回路阻抗是否显示
        private bool _isShowHLZK;

        public bool IsShowHLZK
        {
            get { return _isShowHLZK; }
            set
            {
                if (_isShowHLZK != value)
                {
                    _isShowHLZK = value;
                    this.RaisePropertyChanged(() => this.IsShowHLZK);

                }
            }
        }
        #endregion

        #region 有用信号是否显示
        private bool _isShowYYXH;

        public bool IsShowYYXH
        {
            get { return _isShowYYXH; }
            set
            {
                if (_isShowYYXH != value)
                {
                    _isShowYYXH = value;
                    this.RaisePropertyChanged(() => this.IsShowYYXH);

                }
            }
        }
        #endregion

        #region 信号总数是否显示
        private bool _isShowXHZS;

        public bool IsShowXHZS
        {
            get { return _isShowXHZS; }
            set
            {
                if (_isShowXHZS != value)
                {
                    _isShowXHZS = value;
                    this.RaisePropertyChanged(() => this.IsShowXHZS);

                }
            }
        }
        #endregion

        #region 报警设置是否显示
        private bool _isShowBJSZ;

        public bool IsShowBJSZ
        {
            get { return _isShowBJSZ; }
            set
            {
                if (_isShowBJSZ != value)
                {
                    _isShowBJSZ = value;
                    this.RaisePropertyChanged(() => this.IsShowBJSZ);

                }
            }
        }
        #endregion

    }

    public partial class NewDataViewModel
    {
        private ObservableCollection<SelectDataModel> _selectDataItems;

        public ObservableCollection<SelectDataModel> SeleteDataItems
        {
            get { return _selectDataItems ?? (_selectDataItems = new ObservableCollection<SelectDataModel>()); }
        }

        #region 终端名称

        /// <summary>
        ///终端名称
        /// </summary>
        private string _rtuName;

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value.Equals(_rtuName)) return;
                _rtuName = value;
                RaisePropertyChanged("RtuName");
            }
        }

        #endregion

        #region 终端地址

        /// <summary>
        ///终端地址
        /// </summary>
        private string _rtuID;

        public string RtuID
        {
            get { return _rtuID; }
            set
            {
                if (value.Equals(_rtuID)) return;
                _rtuID = value;
                RaisePropertyChanged("RtuID");
            }
        }

        #endregion

        #region  Measure

        private DateTime _dtMeasureBtn;
        private ICommand _cmdMeasureBtn;

        public ICommand CmdMeasureBtn
        {
            get { return _cmdMeasureBtn ?? (_cmdMeasureBtn = new RelayCommand(ExCmdMeasureBtn, CanCmdMeasureBtn, true)); }
        }

        private bool CanCmdMeasureBtn()
        {
            return DateTime.Now.Ticks - _dtMeasureBtn.Ticks > 300000000;
        }

        private void ExCmdMeasureBtn()
        {
            _dtMeasureBtn = DateTime.Now;

            SendMeasureOrder();
        }

        #endregion

        private void SendMeasureOrder()
        {

            if (SeleteDataItems.Count != 0)
            {
                int _rtuid = SeleteDataItems[0].RtuId;
                int _loopid = SeleteDataItems[0].LineLoopId;

                SeleteDataItems.Clear();

                var info = Sr.ProtocolPhone.LxLdu.wst_ldu_orders;
                info.WstLduOrders.LduId = _rtuid;
                info.WstLduOrders.LineIds.Add(_loopid);
                info.WstLduOrders.Op = 1;
                SndOrderServer.OrderSnd(info, 10, 6);
            }


        }

        private void Get_Rtu_Info(int LduId, ref string RtuId, ref string RtuName)
        {
            RtuId = string.Empty;
            RtuName = string.Empty;

            var dh = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(LduId);

            if (dh != null && dh.RtuFid != 0)
            {
                var dx = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(dh.RtuFid);
                
                RtuId = Convert.ToString(dx.RtuPhyId);
                RtuName = dx.RtuName;
            }
        }

        private void ReDrewView()
        {


            int _lduID = 0;

            var lst = new List<Wlst .client .LduLineData>();
            for (int i = 1; i < 7; i++)
            {
                lst.Add(Services.DataHolding.GetInfo(_currentSelectedLduId, i));
            }

            SeleteDataItems.Clear();
            foreach (Wlst .client .LduLineData item in lst)
            {
                if (item == null) continue;
                SeleteDataItems.Add(new SelectDataModel(item));
                _lduID = item.LduId;
                
            }

            string _rtuid = string.Empty;
            string _rtuname = string.Empty;

            Get_Rtu_Info(_lduID, ref _rtuid, ref _rtuname);
            RtuID = _rtuid;
            RtuName = _rtuname;

        }

        private void OnOtherViewShowData(Wlst.client.LduLineData info)
        {
            if (info == null) return;
            _currentSelectedLduId = info.LduId ;
            SeleteDataItems.Clear();
            SeleteDataItems.Add(new SelectDataModel(info));

            string _rtuid = string.Empty;
            string _rtuname = string.Empty;

            Get_Rtu_Info(info.LduId, ref _rtuid, ref _rtuname);
            RtuID = _rtuid;
            RtuName = _rtuname;
        }
    }

    public partial class NewDataViewModel
    {

        private int _currentSelectedLduId;

        public void InitEvent()
        {
           EventPublish.AddEventTokener( Assembly.GetExecutingAssembly().GetName().ToString(),
                                                       FundEventHandlers, FundOrderFilters);
        }

        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                this.IsShowCJSJ = Wj1090TreeSetLoad.Myself.IsShowCJSJ;
                this.IsShowZT = Wj1090TreeSetLoad.Myself.IsShowZT;
                this.IsShowYGGL = Wj1090TreeSetLoad.Myself.IsShowYGGL;
                this.IsShowWGGL = Wj1090TreeSetLoad.Myself.IsShowWGGL;
                this.IsShowGLYS = Wj1090TreeSetLoad.Myself.IsShowGLYS;
                this.IsShowLDL = Wj1090TreeSetLoad.Myself.IsShowLDL;
                this.IsShowXHQD = Wj1090TreeSetLoad.Myself.IsShowXHQD;
                this.IsShowHLZK = Wj1090TreeSetLoad.Myself.IsShowHLZK;
                this.IsShowYYXH = Wj1090TreeSetLoad.Myself.IsShowYYXH;
                this.IsShowXHZS = Wj1090TreeSetLoad.Myself.IsShowXHZS;
                this.IsShowBJSZ = Wj1090TreeSetLoad.Myself.IsShowBJSZ;

                if (args.EventType == PublishEventType.Core &&
                    args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {
                    return true;
                }
                if (args.EventType == PublishEventType.Core &&
                   args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuDataQueryDataInfoNeedShowInTab)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                // WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        public void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {
                    int rtuid = Convert.ToInt32(args.GetParams()[0]);
                    if (!Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsLine(rtuid)) return;
                    _currentSelectedLduId = rtuid;

                    _dtMeasureBtn = DateTime.Now.AddTicks(-300000000);

                    ReDrewView();
                    Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(
                        Wj1090Module.Services.ViewIdAssign.NewDataViewId);
                }
                 if(args .EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuDataQueryDataInfoNeedShowInTab)
                 {  try
                        {
                            var info = args.GetParams()[0] as Wlst.client.LduLineData;
                            if (info == null) return;
                            OnOtherViewShowData(info);
                        }
                        catch (Exception ex)
                        {
                            
                        }
                 }
            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("Ldu  showdata error in FundEventHandlers:ex:" + xe);
            }
        }



        public void InitAction()
        {


            ProtocolServer.RegistProtocol(Sr.ProtocolPhone .LxLdu .wst_svr_ans_ldu_orders ,// .wlst_svr_ans_cnt_wj1090_order_measure ,//.ClientPart.wlst_Wj1090_server_ans_clinet_order_Measure,
                                          GetRecMeasureData, typeof(NewDataViewModel), this);

        }


        private void GetRecMeasureData(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            var info = infos.WstLduSvrAnsOrders  ;
            if (info == null) return;
            if(info .Op !=1)return;
            foreach (var t in info .ItemsData )
            {
                Services.DataHolding.UpdateLduInfo(t);
            }

            var tmps = (from t in info.ItemsData  where t.LduId  == _currentSelectedLduId select t).ToList();
            if (tmps.Count == 0) return;
            ReDrewView();
        }
    }
}
