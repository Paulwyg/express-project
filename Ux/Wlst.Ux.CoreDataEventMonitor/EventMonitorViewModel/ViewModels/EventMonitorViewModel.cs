using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
 
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.CoreDataEventMonitor.EventMonitorViewModel.Services;

namespace Wlst.Ux.CoreDataEventMonitor.EventMonitorViewModel.ViewModels
{
    [Export(typeof (IIEventMonitorViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class EventMonitorViewModel : ObservableObject, IIEventMonitorViewModel
    {
        public EventMonitorViewModel()
        {
            EventPublisher.AddEventSubScriptionTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler, FundOrderFilter);
        }

        #region IEventAggregator Subscription

        public void FundEventHandler(PublishEventArgs args) // should do somework
        {
            //事件记录、标记、日志、提示等
            string eventtype = "";
            int eventid = 0;
            string eventargs = "";
            try
            {
                eventtype = args.EventType;
                eventid = args.EventId;
                int i = 1;
                foreach (object obj in args.GetParams())
                {
                    eventargs += i + ":" + obj.ToString() + ";";
                    i++;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                //contents += "fail";
            }
            try
            {
                this.AddItems(new EventItemInfo() {EventType = eventtype, EventId = eventid, EventPars = eventargs});
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Core_Monitor EventMonitorViewModel FundEventHandler Occer an error :" +
                                       ex.ToString());
            }
        }

        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
        {
            // TerminalInformation 
            if (_bolMonitor)
            {
                if (args.EventType.Contains("SocketServer")) return false;
                return true;
            }
            return false;
        }

        #endregion



        #region IITab
        public int  Index
        {
            get { return  0; }
        }
        /// <summary>
        /// 当显示在主界面的tab页面时 显示的title
        /// </summary>
        public string Title
        {
            get { return "事件信息"; }
        }

        /// <summary>
        /// 当显示在主界面tab时是否允许用户关闭  地图不运行关闭
        /// </summary>
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

        private void AddItems(EventItemInfo item)
        {
            try
            {
                Items.Add(item);
                if (Items.Count > 300) Items.Clear();
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }

        private ObservableCollection<EventItemInfo> _iitems;

        public ObservableCollection<EventItemInfo> Items
        {
            get
            {
                if (_iitems == null) _iitems = new ObservableCollection<EventItemInfo>();
                return _iitems;
            }
        }


        private bool _bolMonitor;
        private ICommand _cmdMonitor;

        public ICommand CmdMonitor
        {
            get
            {
                if (_cmdMonitor == null) _cmdMonitor = new RelayCommand(ExMoni );
                return _cmdMonitor;
            }
        }

        private void ExMoni()
        {
            _bolMonitor = !_bolMonitor;
            Items.Clear();
            this.RaisePropertyChanged(() => this.CmdName);
        }



        public string CmdName
        {
            get
            {
                if (!_bolMonitor)
                    return "Moni";
                else
                {
                    return "No Moni";
                }
            }
        }
    }
}
