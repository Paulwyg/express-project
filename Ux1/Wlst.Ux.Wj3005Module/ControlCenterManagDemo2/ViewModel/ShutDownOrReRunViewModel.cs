using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.Services;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.ViewModel
{
    [Export(typeof(IIShutDownOrReRun))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ShutDownOrReRunViewModel : EventHandlerHelperExtendNotifyProperyChanged,IIShutDownOrReRun
    {

        public ShutDownOrReRunViewModel()
        {
            if (IsShutDownOrReRun)
            {
                var nodes = (from item in TreeTmlNode.GetRegisterTmlNodes() where item.Value.CheckStopTml select item.Value).ToList();

                foreach (var t in nodes)
                {
                    Items.Add(t);
                }
            }
            else
            {
                var nodes = (from item in TreeTmlNode.GetRegisterTmlNodes() where item.Value.CheckStartTml select item.Value).ToList();

                foreach (var t in nodes)
                {
                    Items.Add(t);
                }
            }
        }
        public void NavOnLoad(params object[] parsObjects)
        {

        }

        private bool _isShutDownOrReRun;
        public bool IsShutDownOrReRun
        {
            get { return _isShutDownOrReRun; }
            set
            {
                if (_isShutDownOrReRun == value) return;
                _isShutDownOrReRun = value;
                RaisePropertyChanged(() => IsShutDownOrReRun);
            }
        }

        private ObservableCollection<TreeNodeBase> _items;
        public ObservableCollection<TreeNodeBase> Items
        {
            get { return _items ?? (_items = new ObservableCollection<TreeNodeBase>()); }
        }
    }
}
