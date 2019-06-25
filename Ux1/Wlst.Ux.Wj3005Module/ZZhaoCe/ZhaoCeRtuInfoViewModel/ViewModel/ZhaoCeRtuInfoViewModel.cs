using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.Ux.WJ3005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.Services;

namespace Wlst.Ux.WJ3005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.ViewModel
{
    [Export(typeof(IIZhaoCeRtuInfoViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ZhaoCeRtuInfoViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IIZhaoCeRtuInfoViewModel
    {
        private ObservableCollection<RtuZhaoCeParsViewModel> _rtusZhaoCeInfo;

        /// <summary>
        /// 所有召测的终端信息
        /// </summary>
        public  ObservableCollection<RtuZhaoCeParsViewModel> RtusZhaoCeInfo
        {
            get
            {
                if (_rtusZhaoCeInfo == null)
                    _rtusZhaoCeInfo = new ObservableCollection<RtuZhaoCeParsViewModel>();
                return _rtusZhaoCeInfo;
            }
        }

        public void OnUserHideOrClosing()
        {
            this.RtusZhaoCeInfo.Clear();
        }

        public ZhaoCeRtuInfoViewModel()
        {
            InitAction() ;
        }



        private void AddZhaoCeRtuInfo(RtuZhaoCeParsViewModel info)
        {
            this.RtusZhaoCeInfo.Add(info);
            //if (this.RtusZhaoCeInfo.Count == 1)
                CurrentSelectedItem = info;
            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
                WJ3005Module.Services.ViewIdAssign.ZhaoCeRtuInfoViewId, true);
        }


        private RtuZhaoCeParsViewModel _currentSelectedItem;

        public RtuZhaoCeParsViewModel CurrentSelectedItem
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

        #region delete current

        private DateTime _dtDelete;

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
            return this.RtusZhaoCeInfo.Count > 0 && DateTime.Now.Ticks-_dtDelete.Ticks>30000000;

        }

        private void ExDelete()
        {
            _dtDelete = DateTime.Now;
            if (this.RtusZhaoCeInfo.Contains(this.CurrentSelectedItem))
            {
                this.RtusZhaoCeInfo.Remove(this.CurrentSelectedItem);

                if (this.RtusZhaoCeInfo.Count > 0)
                {
                    this.CurrentSelectedItem = this.RtusZhaoCeInfo[this.RtusZhaoCeInfo.Count - 1];
                }
                else
                {
                    Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
                        WJ3005Module.Services.ViewIdAssign.ZhaoCeRtuInfoViewId, false);
                }
            }
        }

        #endregion

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
            get { return "召测终端参数"; }
        }
    }

    /// <summary>
    /// Evenet
    /// </summary>
    public partial class ZhaoCeRtuInfoViewModel
    {


        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxRtu .wst_zc_rtu_info ,// .wlst_svr_ans_cnt_request_zc_rtu_para ,//.ClientPart.wlst_ZhaoCeRtuInfo_server_ans_clinet_order_ZcRtuInfo ,
                ZcRtuInfo,
                typeof(ZhaoCeRtuInfoViewModel), this);
        }

  
        public void ZcRtuInfo(string session, Wlst .mobile .MsgWithMobile  infos)
        {
            try
            {
                //21 原来的召测  22 应答K7K8的召测 SwitchOutInfo 有数据 其他都是null  todo
                var rtuZhaoCeInfo = infos.WstRtuZcInfo  ;
                if (rtuZhaoCeInfo == null) return;
                if (rtuZhaoCeInfo.Op != 21 && rtuZhaoCeInfo.Op!=22 && rtuZhaoCeInfo.Op!=23) return;

                //lvf 如果不是本客户端操作的 不处理 2018年8月9日09:34:11
                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.ContainsKey(rtuZhaoCeInfo.RtuId) == false) return;
                System.TimeSpan t = DateTime.Now - Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus[rtuZhaoCeInfo.RtuId];
                if (t.Minutes > 5)
                {
                    //可以不清除,但可能占用内存
                    Wlst.Sr.EquipmentInfoHolding.Services.Others.ZcRtus.Remove(rtuZhaoCeInfo.RtuId);
                    return;
                }

                if (rtuZhaoCeInfo.Op == 21)
                {
                    if (rtuZhaoCeInfo.RtuPara.Count < 1) return;
                    //lvf  2018年7月27日10:52:37  更改 参数,为了支持电能参数
                    this.AddZhaoCeRtuInfo(new RtuZhaoCeParsViewModel(rtuZhaoCeInfo));//.RtuId , rtuZhaoCeInfo.RtuPara[0]
                }
                else if (rtuZhaoCeInfo.Op ==22)
                {
                    if (rtuZhaoCeInfo.RtuPara.Count < 1) return;
                    int rtuId = rtuZhaoCeInfo.RtuId;
                    for (int i=this .RtusZhaoCeInfo .Count -1;i>=0;i--)
                    {
                        if(RtusZhaoCeInfo[i].RtuId   ==rtuId )
                        {
                            RtusZhaoCeInfo[i].Addk7K8(rtuId , rtuZhaoCeInfo.RtuPara [0]);
                        }
                    }
                }else if(rtuZhaoCeInfo.Op == 23) //电能参数恢复  lvf 2018年7月31日08:36:53
                {
                    int rtuId = rtuZhaoCeInfo.RtuId;
                    for (int i = this.RtusZhaoCeInfo.Count - 1; i >= 0; i--)
                    {
                        if (RtusZhaoCeInfo[i].RtuId == rtuId)
                        {
                            RtusZhaoCeInfo[i].AddElectricityArgs(rtuId, rtuZhaoCeInfo.ABCradio);
                        }
                    }
                    
                }
                Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                  rtuZhaoCeInfo.RtuId, "未解析名称", Wlst .Cr .CoreMims .ShowMsgInfo .OperatrType.ServerReply, "召测终端参数");
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }


    }

}
