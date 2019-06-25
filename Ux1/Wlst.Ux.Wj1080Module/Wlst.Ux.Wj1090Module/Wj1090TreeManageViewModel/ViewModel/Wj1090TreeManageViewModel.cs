using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.Wj1090Module.Wj1090TreeManageViewModel.Services;

namespace Wlst.Ux.Wj1090Module.Wj1090TreeManageViewModel.ViewModel
{
    [Export(typeof(IIWj1090ManageViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1090TreeManageViewModel : Wlst.Cr.Core.CoreServices.ObservableObject,IIWj1090ManageViewModel
    {
        public Wj1090TreeManageViewModel()
        {
              EventPublisher.AddEventSubScriptionTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
              Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(Load, 1, DelayEventHappen.EventOne);
        }
        public void NavOnLoad(params object[] parsObjects)
        {
           // Load();
        }

        #region IITab
        public string Title
        {
            get { return "线检"; }
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
    public partial class Wj1090TreeManageViewModel
    {
        private ObservableCollection<TreeNodeTmlViewModel> _collectionWj1090;
        public ObservableCollection<TreeNodeTmlViewModel> CollectionWj1090
        {
            get { return _collectionWj1090 ?? (_collectionWj1090 = new ObservableCollection<TreeNodeTmlViewModel>()); }
            set
            {
                if (value == _collectionWj1090) return;
                _collectionWj1090 = value;
                RaisePropertyChanged(() => CollectionWj1090);
            }
        }

        #region Reflesh

        private DateTime _dtReflesh;
        private ICommand _reflesh;

        public ICommand Reflesh
        {
            get { return _reflesh ?? (_reflesh = new RelayCommand(ExReflesh, CanExReflesh, true)); }
        }

        private bool CanExReflesh()
        {
            return DateTime.Now.Ticks - _dtReflesh.Ticks > 30000000;
        }

        private void ExReflesh()
        {
            _dtReflesh = DateTime.Now;
            this.Load();
        }
     
        #endregion

        private Dictionary<int, TreeNodeTmlViewModel> _equipdir = new Dictionary<int, TreeNodeTmlViewModel>();
        private void Load()
        {
            CollectionWj1090.Clear();
            var tmpssss = new List<TreeNodeTmlViewModel>();
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary)
            {
                if (t.Value.RtuModel != 1090 && t.Value.RtuModel != 30920 && t.Value.RtuModel != 30910) continue;
                if (t.Value.AttachRtuId == 0) continue;
                var tvalue = t.Value;
                if (
                    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(
                        tvalue.AttachRtuId))
                {
                    var attachInfo =
                        Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[
                            tvalue.AttachRtuId];
                    AddNode(tvalue.AttachRtuId, attachInfo.RtuName, t.Value.RtuId, t.Value.RtuName, ref tmpssss);

                }
            }
            var tmpggg = (from t in tmpssss orderby t.NodeId select t).ToList();
            var ggsssg = new ObservableCollection<TreeNodeTmlViewModel>();
            foreach (var t in tmpggg) ggsssg.Add(t);
            CollectionWj1090 = ggsssg;
            foreach (var g in CollectionWj1090)
            {
                foreach (var tm in g.CollectionWj1090)
                {
                    if (!_equipdir.ContainsKey(tm.NodeId)) _equipdir.Add(tm.NodeId, g);
                }
                if (!_equipdir.ContainsKey(g.NodeId)) _equipdir.Add(g.NodeId, g);
            }
        }

        private void AddNode(int rtuId, string rtuName, int murId, string mruName, ref List<TreeNodeTmlViewModel> infos)
        {
            foreach (var t in infos)
            {
                if (t.NodeId == rtuId)
                {
                    foreach (var f in t.CollectionWj1090)
                    {
                        if (f.NodeId == murId)
                        {
                            return;
                        }
                    }
                    t.CollectionWj1090.Add(new TreeNodeWj1090ViewModel(murId, mruName, rtuId));
                    return;
                }

            }

            var tml = new TreeNodeTmlViewModel(rtuId, rtuName);
            tml.CollectionWj1090.Add(new TreeNodeWj1090ViewModel(murId, mruName, rtuId));
            infos.Add(tml);
        }


        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (!Wj1090Module.LduTreeSettingViewModel.ViewModel.Wj1090TreeSetLoad.Myself.IsShowTreeOnTab)
                    return false;
                
                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId == EventIdAssign.EquipmentAddEventId)
                        return true;
                    if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                        return true;
                    if (args.EventId == EventIdAssign.EquipmentUpdateEventId)
                        return true;
                    if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.RtuErrorStateChanged)
                        return true;
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        private void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.Core)
                {
                    //update name

                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentUpdateEventId)
                    {
                        var lst = args.GetParams()[0] as IEnumerable<Tuple<int, int>>;
                        if (lst == null) return;
                        foreach (var t in lst)
                        {
                            if (_equipdir.ContainsKey(t.Item1))
                            {
                                _equipdir[t.Item1].EquipmentUpdate(t.Item1);
                            }
                        }
                        //UpdateNoUsedShow(lst);
                    }

                    else if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.RtuErrorStateChanged)
                    {
                        var lst = args.GetParams()[0] as List<Tuple<int, bool>>;
                        if (lst == null) return;
                        if (lst.Count > 0)
                        {
                            foreach (var g in lst)
                            {
                                if (_equipdir.ContainsKey(g.Item1))
                                {
                                    _equipdir[g.Item1].UpdateTmlStateInfomation(g.Item1);
                                }
                            }
                        }
                        ;
                    }
                    else if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                    {
                        var lst = args.GetParams()[0] as List<Tuple<int, int >>;
                        if (lst == null) return;
                        if (lst.Count > 0)
                        {
                            foreach (var g in lst)
                            {
                                if (_equipdir.ContainsKey(g.Item1))
                                {
                                    foreach (var l in _equipdir[g.Item1].CollectionWj1090)
                                    {
                                        var mtp = _equipdir[g.Item1];
                                        if (l.NodeId == g.Item1)
                                        {
                                            _equipdir[g.Item1].CollectionWj1090.Remove(l);

                                            _equipdir.Remove(g.Item1);

                                            if (mtp != null)
                                            {
                                                mtp.UpdateTmlStateInfomation(g.Item1);
                                                
                                                if (mtp.CollectionWj1090.Count < 1)
                                                {
                                                    if (this.CollectionWj1090.Contains(mtp))
                                                        this.CollectionWj1090.Remove(mtp);
                                                    if (_equipdir.ContainsKey(mtp.NodeId)) _equipdir.Remove(mtp.NodeId);
                                                    
                                                }
                                            }

                                            break;
                                        }
                                    }
                                    //_equipdir[g.Item1].UpdateTmlStateInfomation(g.Item1);
                                }
                            }
                        }
                        ;
                    }
                    else if(args.EventId == EventIdAssign.EquipmentAddEventId)
                    {
                        //todo 
                         var lst = args.GetParams()[0] as List<Tuple<int, int >>;
                        if (lst == null) return;
                        if (lst.Count > 0)
                        {
                            foreach (var g in lst)
                            {
                                if(Wlst .Sr .EquipmentInfoHolding .Services .EquipmentIdAssignRang .IsRtuIsLine( g.Item1 ))
                                {
                                    var tmp =
                                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo
                                            (g.Item1);
                                    if (tmp == null) continue;
                                    var father = tmp.AttachRtuId;
                                    if (_equipdir.ContainsKey(father))
                                    {
                                        _equipdir[father].CollectionWj1090.Add(new TreeNodeWj1090ViewModel(g.Item1,
                                                                                                           tmp.RtuName,
                                                                                                           father));
                                        if (!_equipdir.ContainsKey(g.Item1)) _equipdir.Add(g.Item1, _equipdir[g.Item1]);
                                    }
                                    else
                                    {
                                        if (father == 0) father = g.Item1;
                                        var equ = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.
                                            GetEquipmentInfo
                                            (father);
                                        if (equ == null) continue;

                                        var tml = new TreeNodeTmlViewModel(father, equ.RtuName);
                                        tml.CollectionWj1090.Add(new TreeNodeWj1090ViewModel(g.Item1,
                                                                                             tmp.RtuName,
                                                                                             father));
                                        this.CollectionWj1090.Add(tml);
                                        if (!_equipdir.ContainsKey(g.Item1)) _equipdir.Add(g.Item1, tml);
                                        if (!_equipdir.ContainsKey(father )) _equipdir.Add(father , tml);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
