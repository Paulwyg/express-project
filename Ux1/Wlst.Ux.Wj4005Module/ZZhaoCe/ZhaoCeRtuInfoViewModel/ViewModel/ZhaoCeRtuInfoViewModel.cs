using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Ux.WJ4005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.Services;
using Wlst.Ux.Wj4005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.ViewModel;
using Wlst.client;

namespace Wlst.Ux.WJ4005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.ViewModel
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
            bool flag = false;
            int index = 0;

            for (int i = 0; i < RtusZhaoCeInfo.Count; i++ )
            {
                if(RtusZhaoCeInfo[i].RtuId == info.RtuId )
                {
                    flag = true;
                    index = i;
                }
            }

            if (flag == false)
            {
                this.RtusZhaoCeInfo.Add(info);
                CurrentSelectedItem = info;
            }
            else
            {
                var temp = RtusZhaoCeInfo[index];

                temp.DateTimeRecevie = info.DateTimeRecevie;

                if (info.SwitchInInfo != null)
                {
                    if (info.SwitchInInfo.Count != 0)
                    {
                        temp.SwitchInInfo.Clear();
                        for (int i = 0; i < info.SwitchInInfo.Count; i++)
                        {
                            temp.SwitchInInfo.Add(info.SwitchInInfo[i]);
                        }

                        temp.LoopTotal = info.LoopTotal;
                        temp.VoltageTransformer = info.VoltageTransformer;
                    }
                }

                if (info.SwitchInLimitInfo != null)
                {
                    if (info.SwitchInLimitInfo.Count != 0)
                    {
                        temp.SwitchInLimitInfo.Clear();
                        for (int i = 0; i < info.SwitchInLimitInfo.Count; i++)
                        {
                            temp.SwitchInLimitInfo.Add(info.SwitchInLimitInfo[i]);
                        }

                        temp.LoopTotal = info.LoopTotal;
                    }
                }

                if (info.SwitchOutInfo != null)
                {
                    if (info.SwitchOutInfo.Count != 0)
                    {
                        temp.SwitchOutInfo.Clear();
                        for (int i = 0; i < info.SwitchOutInfo.Count; i++)
                        {
                            temp.SwitchOutInfo.Add(info.SwitchOutInfo[i]);
                        }

                        temp.SwitchOutTotal = info.SwitchOutTotal;
                    }
                }

                this.RtusZhaoCeInfo.RemoveAt(index);
                this.RtusZhaoCeInfo.Insert(index, temp);

                CurrentSelectedItem = RtusZhaoCeInfo[index];
            }

            
            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
                WJ4005Module.Services.ViewIdAssign.ZhaoCeRtuInfoViewId, true);
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
                        WJ4005Module.Services.ViewIdAssign.ZhaoCeRtuInfoViewId, false);
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
            get { return "召测4005参数"; }
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


            //var X = new Wlst.client.ZhaoCeInfo.RtuZhaoRtuPara1();

            //X.DataMark = 2;
            //X.SwitchOutField = new ZhaoCeInfo.RtuZhaoRtuPara1.SwitchOut();
            //X.SwitchOutField.SwitchOutLoop = new List<int> { 1, 2, 3 };

            //X.SwitchOutField.SwitchOutTotal = 3;

            //X.SwitchInField = new ZhaoCeInfo.RtuZhaoRtuPara1.SwitchIn();
            //X.SwitchInField.LoopTotal = 5;
            //X.SwitchInField.VoltageTransformer = 11;
            //X.SwitchInField.CurrentPhase = new List<int>() { 0, 0, 2, 2, 1 };
            //X.SwitchInField.CurrentTransformer = new List<int>() { 2, 3, 4, 5, 6 };

            //X.SwitchInLimitField = new ZhaoCeInfo.RtuZhaoRtuPara1.SwitchInLimit();

            //X.SwitchInLimitField.LoopTotal = 5;
            //X.SwitchInLimitField.CurrentLowlimit = new List<double>() { 24.3, 25.6, 21.4, 20.8, 22 };
            //X.SwitchInLimitField.CurrentUplimit = new List<double>() { 124.3, 125.6, 121.4, 120.8, 122 };

            //X.SwitchInLimitField.VoltageLowlimit = new List<double>() { 4.3, 5.6, 1.4, 0.8, 2 };
            //X.SwitchInLimitField.VoltageUplimit = new List<double>() { 104.3, 105.6, 101.4, 100.8, 102 };

            //this.AddZhaoCeRtuInfo(new RtuZhaoCeParsViewModel(1001013, X));

            //X.DataMark = 1;

            //this.AddZhaoCeRtuInfo(new RtuZhaoCeParsViewModel(1001013, X));

            //X.DataMark = 3;

            //this.AddZhaoCeRtuInfo(new RtuZhaoCeParsViewModel(1001013, X));
        }


        public void ZcRtuInfo(string session, Wlst.mobile.MsgWithMobile infos)
        {
            try
            {
                var rtuZhaoCeInfo = infos.WstRtuZcInfo;
                if (rtuZhaoCeInfo == null) return;
                if (rtuZhaoCeInfo.Op == 210)
                {
                    this.AddZhaoCeRtuInfo(new RtuZhaoCeParsViewModel(rtuZhaoCeInfo.RtuId, rtuZhaoCeInfo.RtuPara1));
                }
                else
                {
                    return;

                }

                Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                    rtuZhaoCeInfo.RtuId, "未解析名称", Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.ServerReply, "召测终端参数");
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }


    }

}
