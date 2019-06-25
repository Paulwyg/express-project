using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;



namespace Wlst.Ux.Wj2090Module.ZcInfo.SluTime
{
    [Export(typeof (IIZcSluTime))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ZcSluTimeVm : Wlst.Cr.Core.CoreServices.ObservableObject, IIZcSluTime
    {

        public ZcSluTimeVm()
        {
            isViewActive = false;
            this.InitAciton();
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
            get { return "集中器时间方案召测"; }
        }

        #endregion

        private bool isViewActive = false;

        public void NavOnLoad(params object[] parsObjects)
        {

            isViewActive = true;
            _sluId = 0;

        }

        public void OnUserHideOrClosing()
        {
            isViewActive = false;
            this.Items.Clear();
            _sluId = 0;
        }




        private ObservableCollection<OneSluTimeInfo> _itemTmlsInfo;

        public ObservableCollection<OneSluTimeInfo> Items
        {
            get
            {
                if (_itemTmlsInfo == null)
                    _itemTmlsInfo = new ObservableCollection<OneSluTimeInfo>();
                return _itemTmlsInfo;
            }

        }

        private OneSluTimeInfo _curitem;

        public OneSluTimeInfo CurrentSelectedItem
        {
            get { return _curitem; }
            set
            {
                if (value == _curitem) return;
                _curitem = value;
                this.RaisePropertyChanged(() => this.CurrentSelectedItem);
            }
        }
    }

    /// <summary>
    /// Action
    /// </summary>
    public partial class ZcSluTimeVm
    {

        private void InitAciton()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxSlu .wst_read_time_plan_in_slu ,// .wlst_svr_ans_cnt_wj2090_order_zc_short_time ,//.ClientPart.wlst_Wj2090_svr_to_clinet_zc_slu_short_time, 
                OnZcOrSetBack,
                typeof (ZcSluTimeVm), this);
        }

        private int _sluId;

        private void OnZcOrSetBack(string sessionid,Wlst .mobile .MsgWithMobile  info)
        {
            if (info == null || info.WstSluReadTimePlanInSlu  == null) return;
            _sluId = info.WstSluReadTimePlanInSlu.SluId;

            //lvf 如果不是本客户端操作的 不处理 2018年8月9日09:34:11
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.ContainsKey(_sluId) == false) return;
            System.TimeSpan t = DateTime.Now - Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus[_sluId];
            if (t.Minutes > 5)
            {
                //可以不清除,但可能占用内存
                Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.Remove(_sluId);
                return;
            }



            foreach (var g in info.WstSluReadTimePlanInSlu.Iems)
            {
                this.Items.Add(new OneSluTimeInfo(info.WstSluReadTimePlanInSlu.SluId, g));
            }
            if (isViewActive == false)
                this.ActiveView();

        }

        private void ActiveView()
        {
            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
                Wj2090Module.Services.ViewIdAssign.ZcSluTimeViewId,true);
        }



        #region CmdClear

        private ICommand _CmdSaveAndSnd;

        public ICommand CmdClear
        {
            get { return _CmdSaveAndSnd ?? (_CmdSaveAndSnd = new RelayCommand(ExCmdSaveAndSnd, CanCmdSaveAndSnd, false)); }
        }

        private long dtsnd = 0;

        private void ExCmdSaveAndSnd()
        {
            this.Items.Clear();
        }


        private bool CanCmdSaveAndSnd()
        {
            return Items.Count > 0;
        }

        #endregion

        //#region CmdClearSluTime

        //private ICommand _cmdClearSluTime;

        //public ICommand CmdClearSluTime
        //{
        //    get { return _cmdClearSluTime ?? (_cmdClearSluTime = new RelayCommand(ExCmdClearSluTime, CanCmdClearSluTime, false)); }
        //}



        ////private void ExCmdClearSluTime()
        ////{
        ////    //this.Items.Clear();
        ////    if (_sluId == 0) return;
        ////    var info = Sr.ProtocolPhone.LxSlu.wst_slu_snd_order;
        ////    // .wlst_cnt_wj3090_order_open_close_light_center ;//.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLightCenter;
        ////    // info.WstCntOrderWj3090OpenClsoeCenter  = data;
        ////    info.WstSluSndOrder.SluId = _sluId;
        ////    info.WstSluSndOrder.Op = 10;
        ////    SndOrderServer.OrderSnd(info, 10, 6);

        ////}


        //private bool CanCmdClearSluTime()
        //{
        //    return Items.Count > 0;
        //}

        //#endregion
    }
}
