using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Sr.ProtocolCnt.Fault;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingManagViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingManagViewModel.ViewModel
{
    [Export(typeof(IIFaultDefineSettingManagViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class FaultDefineSettingManagViewModel : EventHandlerHelperExtendNotifyProperyChanged, IIFaultDefineSettingManagViewModel
    {
        public FaultDefineSettingManagViewModel()
        {
            InitEvent();
        }
        public void NavOnLoad(params object[] parsObjects)
        {
            Records.Clear();
            foreach (var f in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary.Select(t => new TmlFaultTypeViewModel(t.Value)))
            {
                Records.Add(f);
            }
            _recordsHash = GetObservableCollectionHashCode(Records);
        }
        #region IITab
        public string Title
        {
            get { return "自定义故障管理"; }
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
    }

    /// <summary>
    /// Attribute , ICommand ,Function,Field
    /// </summary>
    public partial class FaultDefineSettingManagViewModel
    {
        #region Field

        private  string _recordsHash="";
        #endregion

        #region Attribute
        private ObservableCollection<TmlFaultTypeViewModel> _record;

        public ObservableCollection<TmlFaultTypeViewModel> Records
        {
            get { return _record ?? (_record = new ObservableCollection<TmlFaultTypeViewModel>()); }
        }

        private TmlFaultTypeViewModel _currentSelectItem;

        public TmlFaultTypeViewModel CurrentSelectItem
        {
            get { return _currentSelectItem; }
            set
            {
                if (_currentSelectItem == value) return;
                _currentSelectItem = value;
                RaisePropertyChanged(() => CurrentSelectItem);
            }
        }

        private AddFaultSettingViewModel _addModel;
        public AddFaultSettingViewModel AddModel
        {
            get { return _addModel??(_addModel=new AddFaultSettingViewModel()); }
            set
            {
                if(_addModel==value) return;
                _addModel = value;
                RaisePropertyChanged(()=>AddModel);
            }
        }


        #endregion

        #region ICommand

        #region CmdAdd

        private ICommand _cmdAdd;
        public ICommand CmdAdd
        {
            get { return _cmdAdd ?? (_cmdAdd = new RelayCommand(ExCmdAdd, CanExCmdAdd, true)); }
        }

        private void ExCmdAdd()
        {
            AddModel.OneItem = new TmlFaultTypeViewModel(new EquipmentFaultType
                                                             {
                                                                 AlarmTimeType = 1,
                                                                 Color = "#FFFFFF",
                                                                 FaultCheckKey = "",
                                                                 FaultId = 0,
                                                                 FaultName = "自定义故障",
                                                                 FaultNameByDefine = "自定义故障",
                                                                 FaultRemak = "",
                                                                 FaultType = FaultWarmType.Sse,
                                                                 HourEndAlarm = 1,
                                                                 HourStartAlarm = 1,
                                                                 IsEnable = false,
                                                             });

            //发布事件，调用动画载入增加自定义故障界面
            var ar = new PublishEventArgs
            {
                EventId =EquipemntLightFault.Services.EventIdAssign.EventAddFaultDefineSetttingAnimationId,
                EventType = PublishEventType.None
            };
            EventPublisher.EventPublish(ar);
         
        }

        private static bool CanExCmdAdd()
        {
            return true;
        }

        #endregion

        #region save all

        public ICommand CmdSaveAll
        {
            get { return new RelayCommand(Ex, CanEx, true); }
        }

        private void Ex()
        {
            var lst = Records.Select(t => t.GetTmlFaultType()).ToList();
            Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.ExUpdateFauleTypeInfoforServer(lst);
            _recordsHash = GetObservableCollectionHashCode(Records);
        }


        private bool CanEx()
        {
            var yy = GetObservableCollectionHashCode(Records);
            return _recordsHash != yy;
        }

        #endregion

        #region CmdDelete

        public ICommand CmdDelete
        {
            get { return new RelayCommand(ExCmdDelete, CanExCmdDelete, true); }
        }

        private void ExCmdDelete()
        {
            if (CurrentSelectItem != null && Records.Contains(CurrentSelectItem))
            {
                Records.Remove(CurrentSelectItem);
            }
        }

        private bool CanExCmdDelete()
        {
            return CurrentSelectItem != null && CurrentSelectItem.FaultType == FaultWarmType.Sse;
        }

        #endregion

        #region CmdModify

        private ICommand _cmdModify;
        public ICommand CmdModify
        {
            get { return _cmdModify ?? (_cmdModify=new RelayCommand(ExModify, CanModify, true)); }
        }
        private static bool CanModify()
        {
            return true;
        }
        private  void ExModify()
        {
            AddModel.OneItem = CurrentSelectItem;

            var ar = new PublishEventArgs
            {
                EventId = EquipemntLightFault.Services.EventIdAssign.EventAddFaultDefineSetttingAnimationId,
                EventType = PublishEventType.None
            };
            EventPublisher.EventPublish(ar);
        }
        #endregion

        #endregion

        #region Function
        public string GetObservableCollectionHashCode(object observableCollectionRequest)
        {
            var bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
            var memStream = new System.IO.MemoryStream();
            bf.Serialize(memStream, observableCollectionRequest);


            int lenght = System.Convert.ToInt32(memStream.Length);
            var sr = new MD5CryptoServiceProvider().ComputeHash(memStream.GetBuffer(), 0, lenght);
            var sb = new StringBuilder();
            for (var i = 0; i < sr.Length; i++)
            {
                sb.Append(sr[i].ToString("x2"));
            }
            return sb.ToString();

        }
        #endregion
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class FaultDefineSettingManagViewModel
    {
        private void InitEvent()
        {
            AddEventFilterInfo(Sr.EquipemntLightFault.Services.EventIdAssign.FaultTypeRequest,
                                    PublishEventType.Core);
            AddEventFilterInfo(Sr.EquipemntLightFault.Services.EventIdAssign.FaultTypeUpdateId,
                                    PublishEventType.Core);
            AddEventFilterInfo(EquipemntLightFault.Services.EventIdAssign.EventSaveAddFaultSettingRecordId,
                                   PublishEventType.Core);
        }



        public override void ExPublishedEvent( PublishEventArgs args)
        {
            //base.ExPublishedEvent(args);
            NavOnLoad();
            if (args.EventType != PublishEventType.Core) return;
            switch (args.EventId)
            {
                case Sr.EquipemntLightFault.Services.EventIdAssign.FaultTypeRequest:
                    NavOnLoad();
                    break;
                case Sr.EquipemntLightFault.Services.EventIdAssign.FaultTypeUpdateId:
                    var ar = new PublishEventArgs()
                                 {
                                     EventId = EquipemntLightFault.Services.EventIdAssign.EventCancelDefineSettingAnimationId,
                                     EventType = PublishEventType.None
                                 };
                    EventPublisher.EventPublish(ar);
                    NavOnLoad();
                    break;
                case EquipemntLightFault.Services.EventIdAssign.EventSaveAddFaultSettingRecordId:
                    var isContain=false;
                    foreach (var model in Records.Where(model => model.FaultId==AddModel.OneItem.FaultId))
                    {
                        isContain = true;

                        model.MinuteStartAlarm = AddModel.OneItem.MinuteStartAlarm;
                        model.MinuteEndAlarm = AddModel.OneItem.MinuteEndAlarm;
                        model.IsSelfDefineFault = AddModel.OneItem.IsSelfDefineFault;
                        model.IsEnable = AddModel.OneItem.IsEnable;
                        model.HourStartAlarm = AddModel.OneItem.HourStartAlarm;
                        model.HourEndAlarm = AddModel.OneItem.HourEndAlarm;
                        model.FaultTypeName = AddModel.OneItem.FaultTypeName;
                        model.FaultType = AddModel.OneItem.FaultType;
                        model.FaultRemak = AddModel.OneItem.FaultRemak;
                        model.FaultNameByDefine = AddModel.OneItem.FaultNameByDefine;
                        model.FaultName = AddModel.OneItem.FaultName;
                        model.FaultCheckKey = AddModel.OneItem.FaultCheckKey;
                        model.Color = AddModel.OneItem.Color;

                    }
                   
                
                    var lst = Records.Select(t => t.GetTmlFaultType()).ToList();
                    if(!isContain)
                    {
                        lst.Add(AddModel.OneItem.GetTmlFaultType());
                    }
               
                    Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.ExUpdateFauleTypeInfoforServer(lst);
                    _recordsHash = GetObservableCollectionHashCode(Records);
                    break;
                default:
                    break;
            }
        }   
    }
}
