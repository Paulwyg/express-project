using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;


using Telerik.Windows.Controls;
using Wlst.Cr.Core.ComponentHold;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.SettingHolding;
using Wlst.Ux.Setting.SettingViewModel.Services;

namespace Wlst.Ux.Setting.SettingViewModel.ViewModel
{
    [Export(typeof (IISettingViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SettingViewModel : ObservableObject, Services.IISettingViewModel
    {
        /// <summary>
        /// 选中设置节点变更时事件发布Id  模块内部事件使用
        /// </summary>
        public static int EventId = 100020;

        public static string EventType = "InfrastructureSettingViewModelNodeChange";
        private ObservableCollection<TreeNodeViewModel> _childTreeItems;

        public ObservableCollection<TreeNodeViewModel> ChildTreeItems
        {
            get
            {
                if (_childTreeItems == null) _childTreeItems = new ObservableCollection<TreeNodeViewModel>();
                return _childTreeItems;
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        public SettingViewModel()
        {
           EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler, FundOrderFilter);
        }

        #region IEventAggregator Subscription

        private object _view;

        public object View
        {
            get { return _view; }
            set
            {
                if (_view != value)
                {
                    _view = value;
                    this.RaisePropertyChanged(() => View);
                }
            }
        }

        public void FundEventHandler(PublishEventArgs args)
        {
            int viewId = 0;
            try
            {
                viewId = Convert.ToInt32(args.GetParams()[0]);
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }

            var obs = Wlst.Cr.Core.CoreServices.RegionManage.GetViewById(viewId);

            if (obs != null)
                View = obs;
        }


        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == EventType && args.EventId == EventId)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        #endregion

        public void NavOnLoad(params object[] parsObjects)
        {
            var userProperty = UserInfo.UserLoginInfo;
            this.ChildTreeItems.Clear();
            foreach (var t in SettingComponentHolding .GetAllComponentIDs)
            {
                var tmp = SettingComponentHolding.GetMenuItemById(t);
                if (tmp == null) continue;
                if (string.IsNullOrEmpty(tmp.Describle) == false && tmp.Describle.Contains("admin") && userProperty.D ==false )
                {
                    continue ;
                }
                this.AddNode(tmp);
            }
        }

        private void AddNode(IISetting  keyValue)
        {
            if (keyValue.Key.Contains("Setting"))
            {
                //string path =
                //    I36N .Services.I36N .ConvertByCodingOne(keyValue.Id.ToString(CultureInfo.InvariantCulture), "Null");
                //if (path.Contains("Missing"))
                //{
                string path = keyValue.PathSetting;
               

                string[] sp = path.Split('#');
                if (sp.Length == 1)
                {
                    this.ChildTreeItems.Add(new TreeNodeViewModel(keyValue.ViewId, sp[0]));
                }
                else
                {
                    if (sp.Length < 2) return;
                    foreach (var t in this.ChildTreeItems)
                    {
                        if (t.NodeName.Equals(sp[0].Trim()))
                        {
                            t.ChildTreeItems.Add(new TreeNodeViewModel(keyValue.ViewId, sp[1]));
                            return;
                        }
                    }
                    var f = new TreeNodeViewModel(0, sp[0].Trim());
                    this.ChildTreeItems.Add(f);
                    f.ChildTreeItems.Add(new TreeNodeViewModel(keyValue.ViewId, sp[1]));
                }
            }
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
                return "选项";
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
    }
}