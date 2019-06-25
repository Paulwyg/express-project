using System.Windows.Input;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Sr.ProtocolCnt.Fault;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingManagViewModel.ViewModel
{
    public class AddFaultSettingViewModel : ObservableObject
    {
        #region OneItem
        private TmlFaultTypeViewModel _oneItem;
        public TmlFaultTypeViewModel OneItem
        {
            get { return _oneItem; }
            set
            {
                if (_oneItem == value) return;
                _oneItem = value;
                RaisePropertyChanged(() => OneItem);
            }
        }
        #endregion

        #region ICommand
        #region CmdAdd

        private ICommand _cmdSave;
        public ICommand CmdSave
        {
            get { return _cmdSave ?? (_cmdSave = new RelayCommand(ExCmdSave, CanExCmdSave, true)); }
        }

        private void ExCmdSave()
        {
            var ar = new PublishEventArgs
                         {
                             EventId = EquipemntLightFault.Services.EventIdAssign.EventSaveAddFaultSettingRecordId,
                             EventType =PublishEventType.Core
                         };
            EventPublisher.EventPublish(ar);
        }

        private static bool CanExCmdSave()
        {
            return true;
        }

        #endregion

        #region CmdCancel
        private ICommand _cmdCancel;
        public ICommand CmdCancel
        {
            get { return _cmdCancel ?? (_cmdCancel = new RelayCommand(ExCmdCancel, CanExCmdCancel, true)); }
        }

        private void ExCmdCancel()
        {
            var ar = new PublishEventArgs
            {
                EventId = EquipemntLightFault.Services.EventIdAssign.EventCancelDefineSettingAnimationId,
                EventType = PublishEventType.None
            };
            EventPublisher.EventPublish(ar);
        }

        private static bool CanExCmdCancel()
        {
            return true;
        }
        #endregion
        #endregion
    }
}
