using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Sr.ProtocolCnt.Fault;
using Wlst.Ux.EmergencyDispatch.LightFaultShieldSet.Services;

namespace Wlst.Ux.EmergencyDispatch.LightFaultShieldSet.ViewModels
{
    [Export(typeof(IILightFaultShieldSetViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LightFaultShieldSetViewModel : EventHandlerHelperExtendNotifyProperyChanged,IILightFaultShieldSetViewModel
   {
       #region IITab
       public string Title
       {
           get { return "屏蔽报警管理"; }
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

        public LightFaultShieldSetViewModel()
        {
            InitAction();
        }

       public void NavOnLoad(params object[] parsObjects)
       {

           LoadNode();
           LoadFaultItems();
           RequestNowExistShieldTmlFaultSet();
       }

       public void OnUserHideOrClosing()
       {
           ChildTreeLeftItems.Clear();
           FaultType.Clear();
       }
    }
    /// <summary>
    /// Attri, ICommand
    /// </summary>
    public partial class LightFaultShieldSetViewModel
    {

        #region Attri
        #region Remind

        private string _remind;
        public string Remind
        {
            get { return _remind; }
            set
            {
                if (value == _remind) return;
                    _remind = value;
                RaisePropertyChanged(()=>Remind);
            }
        }

        #endregion

        #region IsEnabled

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if(_isEnabled==value) return;
                _isEnabled = value;
                foreach (var item in ChildTreeLeftItems)
                {
                    item.IsEnabled = IsEnabled;
                }
                RaisePropertyChanged(()=>IsEnabled);
            }
        }
        #endregion

        #region ShieldName

        private string _shieldName;
        public string ShieldName
        {
            get { return _shieldName; }
            set
            {
                if(_shieldName==value) return;
                _shieldName = value;
                RaisePropertyChanged(()=>ShieldName);
                CurrentShieldItem.Name = ShieldName;
            }
        }
        #endregion

        #region BeginDate

        private DateTime _beginDate;
        public DateTime BeginDate
        {
            get { return _beginDate; }
            set
            {
                if(value==_beginDate) return;
                _beginDate = value;
                RaisePropertyChanged(()=>BeginDate);
                CurrentShieldItem.TimeStart = BeginDate;
            }
        }

        #endregion

        #region EndDate

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (value == _endDate) return;
                _endDate = value;
                RaisePropertyChanged(()=>EndDate);
            }
        }

        #endregion

        #region FaultType

        private ObservableCollection<NameIntBool> _faultType;
        public ObservableCollection<NameIntBool> FaultType
        {
            get { return _faultType ?? (_faultType = new ObservableCollection<NameIntBool>()); }
        }

        #endregion

        #region ChildTreeLeftItems

        private ObservableCollection<ListTreeNodeBase> _childTreeLeftItems;
        public ObservableCollection<ListTreeNodeBase> ChildTreeLeftItems
        {
            get { return _childTreeLeftItems ?? (_childTreeLeftItems = new ObservableCollection<ListTreeNodeBase>()); }
        }

        #endregion

        #region ShieldItems

        private ObservableCollection<ShieldFaultModel> _shieldItems;
        public ObservableCollection<ShieldFaultModel>  ShieldItems
        {
            get { return _shieldItems ?? (_shieldItems = new ObservableCollection<ShieldFaultModel>()); }
        }
        #endregion

        #region CurrentShieldItem

        private ShieldFaultModel _currentShieldItem;
        public ShieldFaultModel CurrentShieldItem
        {
            get { return _currentShieldItem; }
            set
            {
                if (_currentShieldItem == value) return;
                _currentShieldItem = value;
                if(_currentShieldItem==null) return;
                LoadCurrentItem(_currentShieldItem);
                RaisePropertyChanged(()=>CurrentShieldItem);
            }
        }

        #endregion
        #endregion

        #region ICommand
        #region CmdSave

        private DateTime _dtSave;
        private ICommand _cmdSave;
        public ICommand CmdSave
        {
            get { return _cmdSave ?? (_cmdSave = new RelayCommand(ExSave, CanSave, true)); }
        }
        private bool CanSave()
        {
            return DateTime.Now.Ticks-_dtSave.Ticks>30000000;
        }
        private void ExSave()
        {
            _dtSave = DateTime.Now;
           //保存终端列表，故障列表数据
            CurrentShieldItem.FaultsShield.Clear();
            CurrentShieldItem.FaultsShield.AddRange(from item in FaultType where item.IsSelected select item.Value );

            CurrentShieldItem.RtusShield.Clear();
            CurrentShieldItem.RtusShield.AddRange(from item in ListTreeTmlNode.GetRegisterNodes() where item.Value.IsChecked select item.Value.NodeId );

            IsEnabled = false;

        }
        #endregion

        #region CmdNew

        private DateTime _dtNew;
        private ICommand _cmdNew;
        public ICommand CmdNew
        {
            get { return _cmdNew ?? (_cmdNew = new RelayCommand(ExNew, CanNew, true)); }
        }
        private void ExNew()
        {
            _dtNew = DateTime.Now;
            ShieldItems.Add(new ShieldFaultModel
                                {
                                    Id = DateTime.Now.Ticks,  
                                    TimeStart = DateTime.Now,
                                    TimeEnd = DateTime.Now.AddHours(3),
                                    Name = "NewShileName"
                                    
                                });
            CurrentShieldItem = ShieldItems[ShieldItems.Count-1];
            IsEnabled = true;
        }
        private bool CanNew()
        {
            return DateTime.Now.Ticks - _dtNew.Ticks > 30000000;
        }
        #endregion

        #region CmdModify

        private DateTime _dtModify;
        private ICommand _cmdModify;
        public ICommand CmdModify
        {
            get { return _cmdModify ?? (_cmdModify = new RelayCommand(ExModify, CanModify, true)); }
        }
        private void ExModify()
        {
            _dtModify = DateTime.Now;
            if(CurrentShieldItem !=null)
            {
                IsEnabled = true;
            }
        }
        private bool CanModify()
        {
            return DateTime.Now.Ticks - _dtModify.Ticks > 30000000;
        }
        #endregion

        #region CmdDelete

        private DateTime _dtDelete;
        private ICommand _cmdDelete;
        public ICommand CmdDelete
        {
            get { return _cmdDelete ?? (_cmdDelete = new RelayCommand(ExDelete,CanDelete,true)); }
        }
        private void ExDelete()
        {
            _dtDelete = DateTime.Now;

            for (var i = 0; i < ShieldItems.Count;i++ ) //新增的未提交给服务的数据先删除
            {
                if (!ShieldItems[i].IsChecked) continue;
                ShieldItems.Remove(ShieldItems[i]);
                i--;
            }
            if(ShieldItems.Count>0)
            {
                CurrentShieldItem = ShieldItems[ShieldItems.Count - 1];
            }
            else
            {
                foreach (var item in FaultType)
                {
                    item.IsSelected = false;
                }
                foreach (var item in ListTreeTmlNode.GetRegisterNodes())
                {
                    item.Value.IsChecked = false;
                }
                ShieldName = "";
                BeginDate = DateTime.Now;
                EndDate = DateTime.Now.AddHours(3);

            }
            var data = new ExchangedRtuFaultShield();
            foreach (var item in ShieldItems)
            {
                data.Info.Add(new FaultShieldItemInfo
                {
                    TimeStart = item.TimeStart.Ticks,
                    TimeEnd = item.TimeEnd.Ticks,
                    FaultsShield = item.FaultsShield,
                    RtusShield = item.RtusShield,
                    Id = item.Id,
                    Name = item.Name,
                    CreateUser = item.CreateUser,
                });
            }
            var info = Sr.ProtocolCnt.ServerPart.wlst_EquipemntLightFault_clinet_update_RtuFaultShield;
            info.Data = data;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = "删除屏蔽终端报警信息命令已发出...请等待服务器反馈...";
        }
        private bool CanDelete()
        {
            return DateTime.Now.Ticks - _dtDelete.Ticks > 30000000;
        }
        #endregion

        #region CmdSndToServer

        private DateTime _dtSndToServer;
        private ICommand _cmdSndToServer;
        public ICommand CmdSndToServer
        {
            get { return _cmdSndToServer ?? (_cmdSndToServer = new RelayCommand(ExSndToServer, CanSndToServer, true)); }
        }

        private void ExSndToServer()
        {
            _dtSndToServer = DateTime.Now;
            var data = new ExchangedRtuFaultShield();
            foreach (var item in ShieldItems)
            {
                data.Info.Add(new FaultShieldItemInfo
                                  {
                                      TimeStart = item.TimeStart.Ticks,
                                      TimeEnd = item.TimeEnd.Ticks,
                                      FaultsShield = item.FaultsShield,
                                      RtusShield = item.RtusShield,
                                      Id = item.Id,
                                      Name = item.Name,
                                      CreateUser = item.CreateUser,
                                  });
            }
            var info = Sr.ProtocolCnt.ServerPart.wlst_EquipemntLightFault_clinet_update_RtuFaultShield;
            info.Data = data;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = "屏蔽终端故障信息列表已发出...请等待反馈...";

        }
        public bool CanSndToServer()
        {
            return DateTime.Now.Ticks - _dtSndToServer.Ticks > 30000000;
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// Methods
    /// </summary>
    public partial class LightFaultShieldSetViewModel
    {
        //加载终端节点
        private void LoadNode()
        {
            ChildTreeLeftItems.Clear();

            if (Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(0))
                foreach (var t in Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[0].LstGrp)
                {
                    if (
                        !Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(t))
                        continue;
                    ChildTreeLeftItems.Add(new ListTreeGroupNode(null, t));
                }

            var rtus = (from t in ListTreeTmlNode.GetRegisterNodes() select t.Value.NodeId).ToList();
            var lstAllSpecial = (from equipmentInfo in Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.Select(t => t.Value).OfType<IIRtuParaWork>() where !rtus.Contains(equipmentInfo.RtuId) select equipmentInfo.RtuId).ToList();

            if (lstAllSpecial.Count > 0)
            {
                var f = new ListTreeGroupNode { NodeId = -1, NodeName = "未分组终端" };
                foreach (var t in lstAllSpecial)
                {
                    f.ChildTreeItems.Add(new ListTreeTmlNode(f, t));
                }
                ChildTreeLeftItems.Add(f);

            }
        }

        private void LoadFaultItems()
        {
            FaultType.Clear();
            //加载故障列表
            foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                if (!t.Value.IsEnable) continue;
                FaultType.Add(!string.IsNullOrEmpty(t.Value.FaultNameByDefine)
                                  ? new NameIntBool { Name = t.Value.FaultNameByDefine, Value = t.Value.FaultId, IsSelected = false }
                                  : new NameIntBool { Name = t.Value.FaultName, Value = t.Value.FaultId, IsSelected = false });
            }
        }

        private void LoadCurrentItem(ShieldFaultModel model)
        {
            ShieldName = model.Name;
            BeginDate = model.TimeStart;
            EndDate = model.TimeEnd;

            foreach (var item in FaultType)
            {
                item.IsSelected = model.FaultsShield.Contains(item.Value);
            }
            TraversalTmlTree(model.RtusShield);
        }

        #region 遍历左侧终端树
        private void TraversalTmlTree(List<int> lst )
        {
            foreach (var childTree in ChildTreeLeftItems)
            {
                if(childTree.IsGroup)
                {
                    TraversalChildTmlTree(childTree as ListTreeGroupNode , lst);
                }
               
            }
        }
        private void TraversalChildTmlTree(ListTreeGroupNode tree, List<int> lst )
        {
            //tree.CheckNum = 0;
            if(lst.Count==0)
            {
                foreach (var item in tree.ChildTreeItems)
                {
                    item.IsChecked = false;
                }
                return;
            }
            //加载前，清零将树设置成默认状态
            foreach (var item in tree.ChildTreeItems)
            {
                if(item.IsGroup)
                {
                    TraversalChildTmlTree( item as ListTreeGroupNode, lst);
                }
                else
                {
                    item.IsChecked = false;
                }
            }
            tree.IsChecked = false;
            foreach (var item in tree.ChildTreeItems)
            {
                
                if(item.IsGroup)
                {
                    TraversalChildTmlTree(item as ListTreeGroupNode, lst);
                }
                else
                {
                    item.IsChecked = lst.Contains(item.NodeId);
                   
                }
            }
        }
        #endregion
    }
    /// <summary>
    /// Event
    /// </summary>
   public partial class LightFaultShieldSetViewModel
    {
        private void InitAction()
        {
            ProtocolServer.RegistProtocol(Sr.ProtocolCnt.ClientPart.wlst_EquipemntLightFault_server_ans_clinet_request_AllRtuFaultShield, GetAllRtuFaultShield,
typeof(LightFaultShieldSetViewModel), this);
            ProtocolServer.RegistProtocol(Sr.ProtocolCnt.ClientPart.wlst_EquipemntLightFault_server_ans_clinet_update_RtuFaultShield, UpdateShieldTmlFaultSet,
typeof(LightFaultShieldSetViewModel), this);
            ProtocolServer.RegistProtocol(Sr.ProtocolCnt.ClientPart.wlst_EquipemntLightFault_server_ans_clinet_add_OneRtuFaultShield, GetOneRtuFaultShield,
typeof(LightFaultShieldSetViewModel), this);
            ProtocolServer.RegistProtocol(Sr.ProtocolCnt.ClientPart.wlst_EquipemntLightFault_server_ans_clinet_delete_RtuFaultShield, GetDeleteOneRtuFaultShield,
typeof(LightFaultShieldSetViewModel), this);
        }
        private void GetAllRtuFaultShield(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<ExchangedRtuFaultShield> infos)
        {
            if (infos == null) return;
            var data = infos.Data.Info;
            if (data.Count < 1) return;
            ShieldItems.Clear();
            foreach (var item in data)
            {
                ShieldItems.Add(new ShieldFaultModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    RtusShield = item.RtusShield,
                    FaultsShield = item.FaultsShield,
                    TimeStart = new DateTime(item.TimeStart),
                    TimeEnd = new DateTime(item.TimeEnd),
                    CreateUser = item.CreateUser
                });
            }
            if (ShieldItems.Count > 0)
            {
                CurrentShieldItem = ShieldItems[0];
            }
            Remind = "服务器数据更新成功，并返回！！！";
        }
        private void UpdateShieldTmlFaultSet(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<ExchangedRtuFaultShield> infos)
       {
           if(infos==null) return;
            var data = infos.Data.Info;
            if(data.Count<1) return;
            ShieldItems.Clear();
            foreach (var item in data)
            {
                ShieldItems.Add(new ShieldFaultModel
                                    {
                                        Id = item.Id,
                                        Name=item.Name,
                                        RtusShield=item.RtusShield,
                                        FaultsShield=item.FaultsShield,
                                        TimeStart=new DateTime(item.TimeStart),
                                        TimeEnd=new DateTime(item.TimeEnd),
                                        CreateUser=item.CreateUser
                                    });
            }
            if(ShieldItems.Count>0)
            {
                CurrentShieldItem = ShieldItems[0];
            }
            Remind = "服务器数据更新成功，并返回！！！";
       }

        private void GetOneRtuFaultShield(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<ExchangedRtuFaultShield> infos)
        {
            if (infos == null) return;
            var data = infos.Data.Info;
            if (data.Count < 1) return;
            foreach (var item in data)
            {
                if(!(from tt in ShieldItems select tt.Id).ToList().Contains(item.Id))
                {
                    ShieldItems.Add(new ShieldFaultModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        RtusShield = item.RtusShield,
                        FaultsShield = item.FaultsShield,
                        TimeStart = new DateTime(item.TimeStart),
                        TimeEnd = new DateTime(item.TimeEnd),
                        CreateUser = item.CreateUser
                    });
                    
                }
            }
            var ar = new PublishEventArgs()
            {
                EventId =EmergencyDispatch.Services.EventIdAssign.AddOneFaultShield,
                EventType = PublishEventType.Core
            };
            ar.AddParams(ShieldItems[ShieldItems.Count-1].Id);
            EventPublisher.EventPublish(ar);
            if (ShieldItems.Count > 0)
            {
                CurrentShieldItem = ShieldItems[ShieldItems.Count-1];
            }
        }

        private void GetDeleteOneRtuFaultShield(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<long> infos)
        {
            if (infos == null) return;
            var data =infos.Data;
            var lst = (from item in ShieldItems select item.Id).ToList();
            var deteleOne = (from item in ShieldItems where item.Id == data select item).ToList();
            if(deteleOne.Count<1) return;
            foreach (var t in deteleOne)
            {
                ShieldItems.Remove(t);
            }
            var ar = new PublishEventArgs()
            {
                EventId = EmergencyDispatch.Services.EventIdAssign.DeleteOneFaultShield,
                EventType = PublishEventType.Core
            };
            ar.AddParams(data);
            EventPublisher.EventPublish(ar);
            if (ShieldItems.Count > 0)
            {
                CurrentShieldItem = ShieldItems[ShieldItems.Count - 1];
            }
        }

    }
    /// <summary>
    /// Socket
    /// </summary>
   public partial class LightFaultShieldSetViewModel
    {
         private void RequestNowExistShieldTmlFaultSet()
         {
             var info = Sr.ProtocolCnt.ServerPart.wlst_EquipemntLightFault_clinet_request_AllRtuFaultShield;
             
             SndOrderServer.OrderSnd(info, 10, 6);
         }
    }
}
