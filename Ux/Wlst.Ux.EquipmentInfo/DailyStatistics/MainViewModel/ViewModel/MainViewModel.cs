using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Ux.EquipmentInfo.DailyStatistics.MainViewModel.Services;
using Wlst.Ux.EquipmentInfo.DailyStatistics.TerminalViewModel.ViewModel;

namespace Wlst.Ux.EquipmentInfo.DailyStatistics.MainViewModel.ViewModel
{
    [Export(typeof (IIMainViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MainViewModel : EventHandlerHelperExtendNotifyProperyChanged, IIMainViewModel
    {


        public void NavOnLoad(params object[] parsObjects)
        {
            RequestAllOperatorType();
        }

        private MainViewModel()
        {
            InitEvent();
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
                return "终端日数据统计";
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

        public void OnUserHideOrClosing()
        {
        }

        private void RequestAllOperatorType()
        {
            //  LogInfo.Log("正在请求所有操作类型信息!!!");
            var info = Sr.ProtocolPhone.LxSys.wst_sys_operator_record;
            info.WstSysOperatorRecord.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private int _indexs = 0;
        /// <summary>
        ///
        /// </summary>
        public int Indexs
        {
            get { return _indexs; }
            set
            {
                if (value != _indexs)
                {
                    _indexs = value;
                    this.RaisePropertyChanged(() => this.Indexs);
                }
            }
        }

        private void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core, true);
        }

        public override void ExPublishedEvent(
            PublishEventArgs args)
        {
            try
            {

                if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {
                    int id = Convert.ToInt32(args.GetParams()[0]);
                    if (id > 1000000 && id < 1100000) Indexs = 0;
                    if (id > 1500000 && id < 1600000) Indexs = 1;
                    if (id > 1600000 && id < 1700000) Indexs = 2;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}
