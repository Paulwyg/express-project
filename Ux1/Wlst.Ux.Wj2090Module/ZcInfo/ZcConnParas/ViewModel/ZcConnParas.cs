using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Wlst.Ux.Wj2090Module.ZcInfo.ZcConnParas.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;

namespace Wlst.Ux.Wj2090Module.ZcInfo.ZcConnParas.ViewModel
{
    [Export(typeof(IIZcConnParas))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ZcConnParas : Wlst.Cr.Core.CoreServices.ObservableObject, IIZcConnParas
    {
        public ZcConnParas()
        {
            isViewActive = false;
            this.InitAciton();
        }

        //#region Msg

        //private string _btMsg;

        //public string Msg
        //{
        //    get { return _btMsg; }
        //    set
        //    {
        //        if (_btMsg == value) return;
        //        _btMsg = value;
        //        RaisePropertyChanged(() => Msg);
        //    }
        //}

        //#endregion

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
            get { return "控制器参数召测"; }
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
            this.Items.Clear();

        }

        #region

        private int sdSluId;

        /// <summary>
        /// 召测序号  ~~~~
        /// </summary>
        public int SluId
        {
            get { return sdSluId; }
            set
            {
                if (value == sdSluId) return;
                sdSluId = value;
                this.RaisePropertyChanged(() => this.SluId);

                var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( value);
                if (info != null)
                {
                    SluPhyId = info.RtuPhyId ;
                    SluName = info.RtuName;
                }
                else
                {
                    SluPhyId = value;
                    SluName = "" + value;
                }
            }
        }


        private int sdSsdfluId;

        /// <summary>
        /// 召测序号  ~~~~
        /// </summary>
        public int SluPhyId
        {
            get { return sdSsdfluId; }
            set
            {
                if (value == sdSsdfluId) return;
                sdSsdfluId = value;
                this.RaisePropertyChanged(() => this.SluPhyId);
            }
        }

        private string sdfsdfds;

        /// <summary>
        /// 召测序号  ~~~~
        /// </summary>
        public string SluName
        {
            get { return sdfsdfds; }
            set
            {
                if (value == sdfsdfds) return;
                sdfsdfds = value;
                this.RaisePropertyChanged(() => this.SluName);
            }
        }

        #endregion


        private ObservableCollection<OneConnPara> _itemTmlsInfo;

        public ObservableCollection<OneConnPara> Items
        {
            get
            {
                if (_itemTmlsInfo == null)
                    _itemTmlsInfo = new ObservableCollection<OneConnPara>();
                return _itemTmlsInfo;
            }

        }

    }


    /// <summary>
    /// Action
    /// </summary>
    public partial class ZcConnParas
    {

        private void InitAciton()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxSlu .wst_read_ctrl_paras_in_slu ,// .wlst_svr_ans_cnt_wj2090_order_zc_conn_paras ,//.ClientPart.wlst_Wj2090_svr_to_clinet_zc_conn_paras,
                OnZcOrSetBack,
                typeof(ZcConnParas), this);
        }


        private void OnZcOrSetBack(string sessionid,Wlst .mobile .MsgWithMobile  info)
        {
            if (info == null || info.WstSluReadCtrlParasInSlu == null) return;

            SluId = info.WstSluReadCtrlParasInSlu.SluId;

            //lvf 如果不是本客户端操作的 不处理 2018年8月9日09:34:11
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.ContainsKey(SluId) == false) return;
            System.TimeSpan t = DateTime.Now - Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus[SluId];
            if (t.Minutes > 5)
            {
                //可以不清除,但可能占用内存
                Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.Remove(SluId);
                return;
            }

            foreach (var g in info.WstSluReadCtrlParasInSlu.Items)
            {
                this.Items.Add(new OneConnPara(g, info.WstSluReadCtrlParasInSlu.SluId));
            }
            if (isViewActive == false)
            {
                this.ActiveView();
            }


        }

        private void ActiveView()
        {
            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
                Wj2090Module.Services.ViewIdAssign.ZcConnParasViewId, true);
        }



        #region CmdClear

        private ICommand _CmdClear;

        public ICommand CmdClear
        {
            get { return _CmdClear ?? (_CmdClear = new RelayCommand(ExCmdClear, CanCmdClear, false)); }
        }

        private long dtsnd = 0;

        private void ExCmdClear()
        {
            this.Items.Clear();
        }


        private bool CanCmdClear()
        {
            return Items.Count > 0;
        }

        #endregion
    }
}
