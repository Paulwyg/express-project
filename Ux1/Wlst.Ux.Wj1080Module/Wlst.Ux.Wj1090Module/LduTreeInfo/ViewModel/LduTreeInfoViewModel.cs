using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.Wj1090Module.LduTreeInfo.Base;
using Wlst.Ux.Wj1090Module.LduTreeInfo.Services;
using Wlst.Ux.Wj1090Module.Resources;

namespace Wlst.Ux.Wj1090Module.LduTreeInfo.ViewModel
{
    [Export(typeof (IILduTreeInfoViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LduTreeInfoViewModel : ObservableObject, IILduTreeInfoViewModel
    {

        public LduTreeInfoViewModel()
        {
            EventPublisher.AddEventSubScriptionTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);

          //  EventPublisher.AddEventSubScriptionTokener(Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
            LoadNode();
        }

        #region load node

        //加载终端节点
        private void LoadNode()
        {
            ChildTreeItems.Clear();
            var lssst = (from t in Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.MainEquipmentInfoDictionary
                       orderby t.Key ascending
                       select t).ToList();
            foreach (var t in lssst)
            {
                var lst = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetMainEquipmentAttachedLst(t.Key);
                foreach (var g in lst)
                {
                    if (g > 1100000 && g < 1199999)
                    {
                        var info = new TreeNodeTmlViewModel(null, t.Value);
                        ChildTreeItems.Add(info );
                        info.UpdateTmlStateInfomation();
                        break;
                    }
                }
            }
            foreach (var t in ChildTreeItems)
            {
                if (!UpdateChildTreeItems.ContainsKey(t.NodeId)) UpdateChildTreeItems.Add(t.NodeId, t);
                foreach (var g in t.ChildTreeItems)
                    if (!UpdateChildTreeItems.ContainsKey(g.NodeId)) UpdateChildTreeItems.Add(g.NodeId, g);
            }
        }

        #endregion

        #region tab iinterface

        public string Title
        {
            get
            {
                return "线检";
                return "Map";
            }
        }


        public bool CanClose
        {
            get { return false; }
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

        private ObservableCollection<NodeBaseNode> _childTreeItemsInfo;

        public ObservableCollection<NodeBaseNode> ChildTreeItems
        {
            get { return _childTreeItemsInfo ?? (_childTreeItemsInfo = new ObservableCollection<NodeBaseNode>()); }
        }

        protected Dictionary<int, NodeBaseNode> UpdateChildTreeItems = new Dictionary<int, NodeBaseNode>();


    };



    //event
    public partial class LduTreeInfoViewModel
    {
        #region IEventAggregator Subscription

        /// <summary>
        /// 事件过滤
        /// 目前只处理
        /// 1、系统当前选中的终端或分组变更，提供联动
        /// 2、终端参数发生变化的时候，即使更新显示数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
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
                    if (args.EventId == EventIdAssign.EquipmentAddEventId)
                    {
                        var info = args.GetParams()[0] as List<Tuple<int, int>>;
                        if (info == null) return;
                        try
                        {
                            OnEquipmentAdd(info);
                        }
                        catch (Exception ex)
                        {
                            Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("防盗终端树EquipmentAddEventId出错:" + ex);
                        }
                      
                        
                    }
                    if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                    {
                        var info = args.GetParams()[0] as IEnumerable<Tuple<int, int>>;
                        if (info == null) return;

                        var tmls = (from t in this.ChildTreeItems select t.NodeId).ToList();
                        foreach (var t in info)
                        {
                            if (UpdateChildTreeItems.ContainsKey(t.Item1)) UpdateChildTreeItems.Remove(t.Item1);

                            if (t.Item1 > 1100000 && t.Item1 < 1199999)
                            {
                                if (tmls.Contains(t.Item2))
                                {
                                    var atts =
                                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.
                                            GetMainEquipmentAttachedLst(t.Item2);
                                    bool bolhas = false;
                                    foreach (var ggg in atts)
                                    {
                                        if (ggg > 1100000 && ggg < 1199999)
                                        {
                                            bolhas = true;
                                            break;
                                        }
                                    }
                                    if (!bolhas)
                                    {
                                        foreach (var g in this.ChildTreeItems)
                                        {
                                            if (g.NodeId == t.Item2)
                                            {
                                                this.ChildTreeItems.Remove(g);
                                                
                                                if(UpdateChildTreeItems .ContainsKey( t.Item2 ))
                                                {
                                                    var tmps =
                                                        (from gsgt in UpdateChildTreeItems[t.Item2].ChildTreeItems
                                                         select gsgt.NodeId).ToList();
                                                    foreach (var gsgdgsdg in tmps) if (UpdateChildTreeItems.ContainsKey(gsgdgsdg))
                                                        UpdateChildTreeItems.Remove(gsgdgsdg);

                                                    UpdateChildTreeItems.Remove(t.Item2);
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        foreach (var g in this.ChildTreeItems)
                                        {
                                            if (g.NodeId == t.Item2)
                                            {
                                                g.ReUpdate(9);
                                                break;
                                            }
                                        }
                                }
                            }
                        }
                    }

                    //update name
                    if (args.EventId == EventIdAssign.EquipmentUpdateEventId)
                    {
                        var lst = args.GetParams()[0] as IEnumerable<Tuple<int, int>>;
                        if (lst == null) return;

                        OnEuqipmentUpdate(lst);
                    }
                    if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.RtuErrorStateChanged)
                    {
                        try
                        {

                            var lst = args.GetParams()[0] as List<Tuple<int, bool>>;
                            if (lst == null) return;
                            foreach (var x in lst)
                            {
                             

                                if (UpdateChildTreeItems.ContainsKey(x.Item1))
                                    UpdateChildTreeItems[x.Item1].ReUpdate(11);
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        void OnEquipmentAdd(List<Tuple<int, int>> info)//增加设备地址-父设备地址
        {
            var tmpObs = new List<NodeBaseNode>();
            var updatetmls = new List<int>();

            //对增加的附属设备查找 主设备，如果已经存在列表中 这更新这一部分
            foreach (var t in info)
                if (t.Item1 > 1100000 && t.Item1 < 1199999 && !updatetmls.Contains(t.Item2)) updatetmls.Add(t.Item2);

            foreach (var t in this.ChildTreeItems)
            {
                if (updatetmls.Contains(t.NodeId))
                {
                    t.ReUpdate(9);
                    updatetmls.Remove(t.NodeId);
                }
                tmpObs.Add(t);
            }


            //对增加的附属设备 未在显示树中的  立即增加到临时列表中  后续排序后一同增加
            foreach (var t in updatetmls)
            {
                var equinfo = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetMainEquipmentInfo(t);

                var infos = new TreeNodeTmlViewModel(null, equinfo);
               // ChildTreeItems.Add(infos);
                infos.UpdateTmlStateInfomation();
                tmpObs.Add(infos);
            }


            //对增加的主设备地址查找  是否该主设备有附属设备 如果有附属设备则增加的临时列表中
            var lssssst = (from t in tmpObs select t.NodeId).ToList();
            updatetmls.Clear();
            foreach (var t in info)
                if (t.Item2 == 0 && !updatetmls.Contains(t.Item1) && !lssssst.Contains(t.Item1))
                    updatetmls.Add(t.Item2); //主设备地址

            foreach (var t in updatetmls)
            {
                var lst = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetMainEquipmentAttachedLst(t);
                foreach (var g in lst)
                {
                    if (g > 1100000 && g < 1199999)
                    {
                        var equinfo = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetMainEquipmentInfo(t);

                        var infos = new TreeNodeTmlViewModel(null, equinfo);
                        ChildTreeItems.Add(infos);
                        infos.UpdateTmlStateInfomation();
                        tmpObs.Add(infos);
                        break;
                    }
                }
            }

            //删除列表中 无子节点的设备
            var lstNeedDelete = (from t in tmpObs where t.ChildTreeItems.Count == 0 select t).ToList();
            foreach (var t in lstNeedDelete)
            {
                if (tmpObs.Contains(t)) tmpObs.Remove(t);
            }

            //排序后增加的 树中
            var lstfffffffffffff = (from t in tmpObs orderby t.NodeId ascending select t).ToList();
            int index = 0;

            foreach (var t in lstfffffffffffff)
            {
                bool find = false;
                for (int i = index; i < this.ChildTreeItems.Count; i++)
                {
                    if (t.NodeId > ChildTreeItems[i].NodeId && i + 1 < ChildTreeItems.Count)
                    {
                        this.ChildTreeItems.Insert(i + 1, t);
                        find = true;
                        index++;
                        break;
                    }
                }
                if (find == false) this.ChildTreeItems.Add(t);
            }

            foreach (var t in ChildTreeItems)
            {
                if (!UpdateChildTreeItems.ContainsKey(t.NodeId)) UpdateChildTreeItems.Add(t.NodeId, t);
                foreach (var g in t.ChildTreeItems)
                    if (!UpdateChildTreeItems.ContainsKey(g.NodeId)) UpdateChildTreeItems.Add(g.NodeId, g);
            }

        }


        void OnEuqipmentUpdate(IEnumerable<Tuple<int, int>> lst)
        {
            // var tmls = (from t in this.ChildTreeItems select t.NodeId).ToList();
            //this.ReUpdateLoadChild();
            foreach (var t in lst)
            {
                if (UpdateChildTreeItems.ContainsKey(t.Item1)) UpdateChildTreeItems[t.Item1].ReUpdate(1);
                if(UpdateChildTreeItems.ContainsKey(t.Item2)) UpdateChildTreeItems[t.Item2].ReUpdate(1);
            }
        }

        #endregion
    }
}
