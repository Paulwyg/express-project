using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Ux.Wj6005Module.Jd601TmlInfo.Jd601InstantData.Service;
using Wlst.client;

namespace Wlst.Ux.Wj6005Module.Jd601TmlInfo.Jd601InstantData.ViewModel
{
    [Export(typeof(IIJd601InstantData))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Jd601InstantDataViewModel : Cr.Core.CoreServices.ObservableObject, IIJd601InstantData
    {
        public Jd601InstantDataViewModel()
        {
            InitAction();
        }
        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "即时数据"; }
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
        public void NavOnLoad(params object[] parsObjects)
        {
            OneData = new EsuDataOneItemViewModel();
            TwoData.ResetAllArgs();
            var tmlId = (int)parsObjects[0];
            if (tmlId > 0)
            {
                RtuId = tmlId;
            }
        }
    }
    /// <summary>
    /// Attri,ICommand,Field
    /// </summary>
     public partial class Jd601InstantDataViewModel
     {
         #region Field

        private int _attachRtuId;
        #endregion
         #region Attri
         private EsuDataOneItemViewModel _currentselectDataOne;

         public EsuDataOneItemViewModel OneData
         {
             get { return _currentselectDataOne ?? (_currentselectDataOne = new EsuDataOneItemViewModel()); }
             set
             {
                 if (value == _currentselectDataOne) return;
                 _currentselectDataOne = value;
                 RaisePropertyChanged(() => OneData);
             }
         }


         private EsuDataTwoItemViewModel _twoData;

         public EsuDataTwoItemViewModel TwoData
         {
             get { return _twoData ?? (_twoData = new EsuDataTwoItemViewModel()); }
             set
             {
                 if (value == _twoData) return;
                 _twoData = value;
                 RaisePropertyChanged(() => TwoData);
             }
         }

         private int _rtuId;

         /// <summary>
         /// 节能设备地址
         /// </summary>

         public int RtuId
         {
             get { return _rtuId; }
             set
             {
                 _rtuId = value;
                 RaisePropertyChanged(() => RtuId);

                 //var mmm = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById(
                 //    _rtuId);
                 //if (mmm != null)
                 //{
                 //    _attachRtuId = mmm.RtuFid;

                 //    RtuName = mmm.RtuName;

                 //    return;
                 //}
                 var gg =
                     Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                         _rtuId);
                 RtuName = "未知";
                 _attachRtuId = 0;
                 if (gg == null) return;

                 RtuName = gg.RtuName;
                 _attachRtuId = gg.RtuFid;
             }
         }

         private string _rtuName;

         /// <summary>
         /// 节能设备
         /// </summary>

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
         #region ICommand
         private DateTime [] _dateTimes=new DateTime[2];
         #region CmdZcDataOne

         private void ExCmdZcDataOne()
         {
             _dateTimes[0] = DateTime.Now;
             ZcJd601DataOne(_attachRtuId, RtuId);
         }

         private bool CanCmdZcDataOne()
         {
             return RtuId > 1000 && DateTime.Now.Ticks-_dateTimes[0].Ticks>30000000;
         }

        private ICommand _cmdZcDataOne;

         /// <summary>
         ///   
         /// </summary>
         public ICommand CmdZcDataOne
         {
             get { return _cmdZcDataOne ?? (_cmdZcDataOne = new RelayCommand(ExCmdZcDataOne, CanCmdZcDataOne, true)); }
         }

         #endregion

         #region CmdZcDataTwo

         private void ExCmdZcDataTwo()
         {
             _dateTimes[1] = DateTime.Now;
             ZcJd601DataTwo(_attachRtuId, RtuId);
         }

         private bool CanCmdZcDataTwo()
         {
             return RtuId > 1000 && DateTime.Now.Ticks - _dateTimes[1].Ticks > 30000000;
         }

        private ICommand _cmdZcDataTwo;

         /// <summary>
         ///   
         /// </summary>
         public ICommand CmdCmdZcDataTwo
         {
             get
             {
                 return _cmdZcDataTwo ??
                        (_cmdZcDataTwo = new RelayCommand(ExCmdZcDataTwo, CanCmdZcDataTwo, true));
             }
         }

         #endregion
         #endregion
     }

    /// <summary>
    /// Event
    /// </summary>
     public partial class Jd601InstantDataViewModel
     {
         public void InitAction()
         {

             ProtocolServer.RegistProtocol(
               Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_jd601_order_measure_one ,//.ClientPart.wlst_Jd601_server_ans_clinet_order_MeasureOne,
               MeasereJd601One,
               typeof(Jd601InstantDataViewModel), this);
             ProtocolServer.RegistProtocol(
               Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_jd601_order_measure_two ,//.ClientPart.wlst_Jd601_server_ans_clinet_order_MeasureTwo,
               MeasureJd601Two,
               typeof(Jd601InstantDataViewModel), this);

         }
         public void MeasereJd601One(string session, Wlst .mobile .MsgWithMobile  args)
         {
             var infos = args.WstSvrAnsCntRequestOrMeasureJd601Data;
             if (infos == null) return;
             if (infos.RtuId.Count == 0) return;
             if (RtuId != infos.RtuId[0]) return;
             if (!infos.LastOneRecord || infos.Info.Count <= 0) return;
             var infosss = new EsuDataOneItemViewModel(infos.Info[0]);
             //this.Items.Add(infosss);
             OneData = infosss;
         }
         public void MeasureJd601Two(string session,Wlst .mobile .MsgWithMobile  args)
         {
             var infos = args.WstSvrAnsCntMeasureJd601DataTwo ;
             if (infos == null) return;
             if (RtuId != infos.RtuId) return;
             var infosss = new EsuDataTwoItemViewModel(infos.Info);
             TwoData = infosss;
         }
     }
    /// <summary>
    /// Socket
    /// </summary>
     public partial class Jd601InstantDataViewModel
     {
         private void ZcJd601DataOne(int mainRtuId, int rtuId)
         {
             var nt = Sr.ProtocolPhone .ServerListen.wlst_cnt_jd601_order_measure_one ;//.ServerPart.wlst_Jd601_clinet_order_MeasureOne;
             nt.WstCntRequestJd601ZcDataOrParOrTimeOrVersion .AttachRtuId = mainRtuId;
             nt.WstCntRequestJd601ZcDataOrParOrTimeOrVersion.RtuId = rtuId;
             SndOrderServer.OrderSnd(nt, 10, 3);
         }

         private void ZcJd601DataTwo(int mainRtuId, int rtuId)
         {
             var nt = Sr.ProtocolPhone .ServerListen .wlst_cnt_jd601_order_measure_two ;//.ServerPart.wlst_Jd601_clinet_order_MeasureTwo;
             nt.WstCntRequestJd601ZcDataOrParOrTimeOrVersion.AttachRtuId = mainRtuId;
             nt.WstCntRequestJd601ZcDataOrParOrTimeOrVersion.RtuId = rtuId;
             SndOrderServer.OrderSnd(nt, 10, 3);
         }
     }
}
