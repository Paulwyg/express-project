using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.Wj1050Module.Resources;
using Wlst.Ux.Wj1050Module.Wj1050ManageSettingViewModel.ViewModel;
using Wlst.Ux.Wj1050Module.Wj1050ManageViewModel.Sercives;
using Wlst.client;

namespace Wlst.Ux.Wj1050Module.Wj1050ManageViewModel.ViewModel
{
    [Export(typeof(IIWj1050ManageViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1050ManageViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, Sercives.IIWj1050ManageViewModel
    {
        public static Wj1050ManageViewModel MySelf;
        public event EventHandler<NodeSelectedArgs> OnClearSerchTest;
        private int _hxxx = 0;
        /// <summary>
        /// 前台界面绑定的图标大小
        /// </summary>
        public int Hightxx
        {
            get
            {
                if (_hxxx < 0.1)
                {
                    _hxxx = (int)Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeightTree;
                    if (_hxxx > 24) _hxxx = 24;
                    if (_hxxx < 12) _hxxx = 12;
                }
                return _hxxx;
            }
        }

        public Wj1050ManageViewModel()
        {
            if (MySelf == null) MySelf = this;
            //Load();
           EventPublish.AddEventTokener( 
                    Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(LoadNode, 1, DelayEventHappen.EventOne);
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            // Load();
        }


       //public static Wj1050ManageViewModel MySelf;
        private static TreeNodeWj1050ViewModel _currentSelectedTreeNode;

        public TreeNodeWj1050ViewModel CurrentSelectedTreeNode
        {
            get { return _currentSelectedTreeNode; }
            set
            {
                if (_currentSelectedTreeNode != value)
                {
                    _currentSelectedTreeNode = value;
                    if (_currentSelectedTreeNode != null)
                    {
                        // UpdateViewById(_currentSelectedTreeNode.NodeId);
                        //var args = new PublishEventArgs
                        //{
                        //    EventType = PublishEventType.Core,
                        //    EventId =Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                        //};
                        //args.AddParams(_currentSelectedTreeNode.AttachRtuId);
                        //args.AddParams(_currentSelectedTreeNode.NodeId);
                        //EventPublish.PublishEvent(args);
                    }
                }
            }
        }

        public static bool OnSelectNodeChangeNavToParsSet = true;

        public void UpdateViewById(int mruId)
        {
            if (OnSelectNodeChangeNavToParsSet == false) return;
            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(
                Wj1050Module.Services.ViewIdAssign.Wj1050InfoSetViewId, mruId);

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
            this.LoadNode();


            List<int> lst = new List<int>();
            lst.Add(1000010);
            lst.Add(1000011);
            lst.Add(1000012);
            lst.Add(1000013);
            lst.Add(1000014);
            if (lst == null) return;

        }

        #endregion




     


        //private EventHandler<NodeSelectedArgs> OnSelectedNodeByCodeIns;
        //event EventHandler<NodeSelectedArgs> IISingleTree.OnSelectedNodeByCode
        //{
        //    add { OnSelectedNodeByCodeIns += value; }
        //    remove { if (OnSelectedNodeByCodeIns != null) OnSelectedNodeByCodeIns -= value; }
        //}



        private ObservableCollection<TreeNodeTmlViewModel> _collectionWj1050;

        /// <summary>
        /// 开关量输入参数
        /// </summary>

        public ObservableCollection<TreeNodeTmlViewModel> CollectionWj1050
        {
            get { return _collectionWj1050 ?? (_collectionWj1050 = new ObservableCollection<TreeNodeTmlViewModel>()); }
            set
            {
                if (value == _collectionWj1050) return;
                _collectionWj1050 = value;
                this.RaisePropertyChanged(() => this.CollectionWj1050);
            }
        }



        #region LoadNode by Lvf

        private ObservableCollection<TreeNodeBaseNode> _childTreeItemsInfo;

        public ObservableCollection<TreeNodeBaseNode> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                    _childTreeItemsInfo = new ObservableCollection<TreeNodeBaseNode>();
                return _childTreeItemsInfo;
            }
            set
            {
                if (value != _childTreeItemsInfo)
                {
                    _childTreeItemsInfo = value;
                    this.RaisePropertyChanged(() => this.ChildTreeItems);
                }
            }
        }

        protected bool IsLoadOnlyOneArea = false;
        public  void LoadNode()
        {
            if (ServicesGrpSingleInfoHold.InfoGroups.Count == 0 &&
                Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0)
                return;
            ChildTreeItems.Clear();
            var userProperty = UserInfo.UserLoginInfo;
            List<int> areaLst = new List<int>();
            areaLst.AddRange(userProperty.AreaX);
            foreach (var t in userProperty.AreaW)
            {
                if (!areaLst.Contains(t))
                {
                    areaLst.Add(t);
                }
            }
            foreach (var f in userProperty.AreaR)
            {
                if (!areaLst.Contains(f))
                {
                    areaLst.Add(f);
                }
            }
            IsLoadOnlyOneArea = areaLst.Count < 2;

            if (userProperty.D == true)
            {
                if (Wj1050TreeSetLoad.Myself.IsShowArea)
                {
                    foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                    {
                        var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        var rtuLst = new List<int>();
                        foreach (var a in lstInArea)
                        {
                            var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(a);
                            if (pb == null) continue;
                            if (pb.EquipmentType == WjParaBase.EquType.Mru && pb.RtuFid == 0) //线路为主设备
                            {
                                rtuLst.Add(pb.RtuId);
                                //if (IsLoadOnlyOneArea)
                                //{
                                //    int AreaId = areaLst[0];
                                //    ShowGrpInArea(AreaId);
                                //}
                                //else
                                //{
                                //    this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
                                //}
                                
                                //break; 
                            }
                            else if (pb.EquipmentType == WjParaBase.EquType.Rtu &&
                                     pb.EquipmentsThatAttachToThisRtu.Count > 0) //haha 特殊终端下有线路
                            {
                                foreach (var g in pb.EquipmentsThatAttachToThisRtu)
                                {
                                    var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                                    if (pa == null) continue;
                                    if (pa.EquipmentType == WjParaBase.EquType.Mru && pa.RtuFid > 0)
                                    {
                                        rtuLst.Add(g);
                                        //if (IsLoadOnlyOneArea)
                                        //{
                                        //    int AreaId = areaLst[0];
                                        //    ShowGrpInArea(AreaId);
                                        //}
                                        //else
                                        //{
                                        //     this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
                                        //}

                                        //break;

                                    }
                                }
                            }
                        }
                        if (rtuLst.Count > 0)
                        {
                            if (IsLoadOnlyOneArea)
                            {
                                int AreaId = areaLst[0];
                                ShowGrpInArea(AreaId);

                            }
                            else
                            {
                                this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0,
                                                                                  TypeOfTabTreeNode.IsArea));
                            }
                        }
                    }
                }
                else
                {
                    foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                    {
                       ShowGrpInArea(f);
                    }
                }
            }
            else
            {

                if (IsLoadOnlyOneArea)
                {
                    int AreaId = areaLst[0];
                    ShowGrpInArea(AreaId);
                }
                else
                {

                    foreach (var f in areaLst)
                    {
                        var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        var rtuLst = new List<int>();
                        foreach (var a in lstInArea)
                        {
                            var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(a);
                            if (pb == null) continue;
                            if (pb.EquipmentType == WjParaBase.EquType.Mru && pb.RtuFid == 0) //线路为主设备
                            {
                                rtuLst.Add(pb.RtuId);
                                //this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
                                //break; ;
                            }
                            else if (pb.EquipmentType == WjParaBase.EquType.Rtu &&
                                     pb.EquipmentsThatAttachToThisRtu.Count > 0) //haha 特殊终端下有线路
                            {
                                foreach (var g in pb.EquipmentsThatAttachToThisRtu)
                                {
                                    var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                                    if (pa == null) continue;
                                    if (pa.EquipmentType == WjParaBase.EquType.Mru && pa.RtuFid > 0)
                                    {
                                        rtuLst.Add(g);
                                        //this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
                                        //break;

                                    }
                                }
                            }
                        }
                        if (rtuLst.Count > 0)
                        {
                            if (IsLoadOnlyOneArea)
                            {
                                int AreaId = areaLst[0];
                                ShowGrpInArea(AreaId);

                            }
                            else
                            {
                                this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0,
                                                                                  TypeOfTabTreeNode.IsArea));
                            }
                        }
                    }
                }
                


            }
            for (int i = this.ChildTreeItems.Count - 1; i > 0; i--)
            {
                var t = this.ChildTreeItems[i];
                if (t.NodeType == TypeOfTabTreeNode.IsTml) continue;
                t.GetChildRtuCount();
                if (t.RtuCount == 0)
                {
                    ChildTreeItems.Remove(t);
                }
            }
            foreach (var t in this.ChildTreeItems) t.GetChildRtuCount();
        }
        public  void ShowGrpInArea(int AreaId)
        {

            if (Wj1050TreeSetLoad.Myself.IsShowGrp)
            {
                var grp =
                            (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                             where t.Key.Item1 == AreaId
                             orderby t.Value.Index
                             select t.Value).ToList();


                this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, AreaId, 0,
                                                      TypeOfTabTreeNode.IsAll));

                foreach (var f in grp)
                {
                    var rtuList = new List<int>();
                    foreach (var fff in f.LstTml)
                    {
                        var rtu = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(fff);
                        if (rtu == null) continue;
                        if (rtu.EquipmentType == WjParaBase.EquType.Rtu &&
                            rtu.EquipmentsThatAttachToThisRtu.Count > 0)
                        {
                            foreach (var g in rtu.EquipmentsThatAttachToThisRtu)
                            {
                                var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                                if (pa == null) continue;
                                if (pa.EquipmentType == WjParaBase.EquType.Mru && pa.RtuFid > 0)
                                {
                                    rtuList.Add(g);
                                }
                            }
                        }
                        else if (rtu.EquipmentType == WjParaBase.EquType.Mru && rtu.RtuFid == 0)
                        {
                            rtuList.Add(rtu.RtuId);
                        }
                    }
                    if (rtuList.Count < 1) continue;
                    this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f.AreaId, f.GroupId,
                                                                      TypeOfTabTreeNode.IsGrp));
                }
                var sp =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);//返回列表只有主设备
                var rtuLst = new List<int>();
               
                foreach (var v in sp)
                {
                    var rtu = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(v);
                    if (rtu == null) continue;
                    if (rtu.EquipmentType == WjParaBase.EquType.Rtu &&
                        rtu.EquipmentsThatAttachToThisRtu.Count > 0)
                    {
                        foreach (var g in rtu.EquipmentsThatAttachToThisRtu)
                        {
                            var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                            if (pa == null) continue;
                            if (pa.EquipmentType == WjParaBase.EquType.Mru && pa.RtuFid > 0)
                            {
                                rtuLst.Add(g);
                            }
                        }
                    }else if(rtu.EquipmentType==WjParaBase.EquType.Mru && rtu.RtuFid ==0)
                    {
                        rtuLst.Add(rtu.RtuId);
                    }
                }
                if (rtuLst.Count > 0)
                    this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, AreaId, 0,
                                                                      TypeOfTabTreeNode.IsGrpSpecial));
            }
            else
            {


                this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, AreaId, 0,
                                                                      TypeOfTabTreeNode.IsAll));

                //var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                //foreach (var f in lstInArea)
                //{
                //    var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                //    if (pb == null) continue;
                //    if (pb.EquipmentType == WjParaBase.EquType.Mru && pb.RtuFid == 0) //电表为主设备
                //    {
                //        this.ChildTreeItems.Add(new TreeNodeWj1050ViewModel(pb.RtuId, pb.RtuName, pb.RtuFid));
                //    }
                //    else if (pb.EquipmentType == WjParaBase.EquType.Rtu && pb.EquipmentsThatAttachToThisRtu.Count > 0) //haha 特殊终端下有电表
                //    {

                //        foreach (var g in pb.EquipmentsThatAttachToThisRtu)
                //        {
                //            var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                //            if(pa==null) continue; 
                //            if (pa.EquipmentType == WjParaBase.EquType.Mru && pa.RtuFid > 0)
                //            {
                //                this.ChildTreeItems.Add(new TreeNodeWj1050ViewModel(pa.RtuId, pa.RtuName, pa.RtuFid));
                //            }
                //        }
                //    }
                //}
            }
        }


        #endregion

        private void Load()
        {
            CollectionWj1050.Clear();


            var tmpssss = new List<TreeNodeTmlViewModel>();//new List<TreeNodeTmlViewModel>();

            foreach (
                var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                )
            {
                if (t.Value.RtuModel != EnumRtuModel.Wj1050) continue;
                if (t.Value.RtuFid == 0) continue;


                var ggg = t.Value;
                if (
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.
                        ContainsKey(ggg.RtuFid))
                {
                    var ggggg =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                            [
                                ggg.RtuFid];
                    this.AddNode(ggg.RtuFid, ggggg.RtuName, t.Value.RtuId, t.Value.RtuName, ref tmpssss);

                }
                // CollectionWj1050.Add(new TreeNodeWj1050ViewModel(fff.RtuId, fff.RtuName, ggg.AttachRtuId));
            }
            var tmpggg = (from t in tmpssss orderby t.NodeId select t).ToList();
            //foreach (var t in tmpggg) this.CollectionWj1050.Add(t);
            var ggsssg = new ObservableCollection<TreeNodeTmlViewModel>();//new ObservableCollection<TreeNodeTmlViewModel>();
            foreach (var t in tmpggg) ggsssg.Add(t);
            CollectionWj1050 = ggsssg;
        }

        private void AddNode(int rtuId, string rtuName, int murId, string mruName, ref List<TreeNodeTmlViewModel> infos)
        {
            foreach (var t in this.CollectionWj1050)
            {
                if (t.NodeId == rtuId)
                {
                    foreach (var f in t.CollectionWj1050)
                    {
                        if (f.NodeId == murId)
                        {
                            return;
                        }
                    }
                    t.CollectionWj1050.Add(new TreeNodeWj1050ViewModel(murId, mruName, rtuId));
                    return;
                }

            }

            var tml = new TreeNodeTmlViewModel(rtuId, rtuName);
            //  this.CollectionWj1050.Add(tml);
            var wj1050lst = new TreeNodeWj1050ViewModel(murId, mruName, rtuId);
            tml.CollectionWj1050.Add(new TreeNodeWj1050ViewModel(murId, mruName, rtuId));
            infos.Add(tml);
        }


        #region tab iinterface

        public int Index
        {
            get { return 6; }
        }
        public string Title
        {
            get
            {
                return "电表设备";
                return "Map";
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


        //public string SearchText
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}



    }

    //event
    public partial class Wj1050ManageViewModel
    {
        #region IEventAggregator Subscription

        /// <summary>
        /// 事件过滤
        /// 目前只处理  lvf  2018年3月29日14:03
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.SvAv) return true;
                if (args.EventType == PublishEventType.ReCn) return true;
                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId ==
                        global::Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
                        return true;
                    if (args.EventId == EventIdAssign.EquipmentAddEventId)
                        return true;
                    if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                        return true;

                    if (args.EventId == EventIdAssign.EquipmentUpdateEventId)
                        return true;
                    //if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate1)
                    //    return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                        return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)
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
                if (args.EventType == PublishEventType.ReCn) LoadNode();
                if (args.EventType == PublishEventType.SvAv)
                {
                    LoadNode();
                    return;
                }
                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId ==
                        global::Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
                    {
                        LoadNode();
                    }



                    if (args.EventId == EventIdAssign.EquipmentAddEventId)
                    {
                        LoadNode();
                    }
                    if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                    {

                        LoadNode();
                    }
                    if (args.EventId == EventIdAssign.RunningInfoUpdate2) //todo  那终端id 变电表状态
                    {
                        var lst = args.GetParams()[0] as IEnumerable<int>;
                        if (lst == null) return;
                        foreach (var t in lst)
                        {

                            if (!TreeNodeWj1050ViewModel.RtuMruIds.ContainsKey(t)) continue;


                            //var id = GetImageIconByState(t);
                            var mruId = TreeNodeWj1050ViewModel.RtuMruIds[t];
                            foreach (var i in mruId)
                            {
                                if (TreeNodeWj1050ViewModel.RtuItems.ContainsKey(i))
                                {
                                    foreach (var f in TreeNodeWj1050ViewModel.RtuItems[i])
                                    {
                                        if (f.Target != null)
                                        {

                                            var xg = f.Target as TreeNodeBaseNode;
                                            if (xg != null) xg.ReUpdate(1);


                                        }
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

        /// <summary>
        /// 获取终端状态 ,电表状态跟随终端 lvf 2018年3月29日14:03:39
        /// </summary>
        /// <param name="rtuid">终端id</param>
        /// <returns>电表图标名称</returns>
        public static int GetImageIconByState(int rtuid)  
        {

            var TerInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuid);
            if (TerInfo == null) return 0;
            var runninfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(rtuid);

            // int modelId = (int) TerInfo.EquipmentType;
            if (TerInfo.EquipmentType == WjParaBase.EquType.Rtu)
            {
                var s = TerInfo.RtuStateCode;
                if (s == 0)
                {
                    return 10500;  //电表不在线图标
                }
                if (s == 1)
                {
                    return 10500;

                }
                var online = runninfo != null && runninfo.IsOnLine;
                if (online == false)
                {
                    return 10500; //电表不在线图标

                }
               
            }
            return 10501;//电表在线图标
        }


        #endregion
    }


    //search
    public partial class Wj1050ManageViewModel
    {

        private ObservableCollection<TreeNodeBaseNode> _searchchildTreeItemsInfo;

        public ObservableCollection<TreeNodeBaseNode> ChildTreeItemsSearch
        {
            get
            {
                if (_searchchildTreeItemsInfo == null)
                    _searchchildTreeItemsInfo = new ObservableCollection<TreeNodeBaseNode>();
                return _searchchildTreeItemsInfo;
            }
            set
            {
                if (value != _searchchildTreeItemsInfo)
                {
                    _searchchildTreeItemsInfo = value;
                    this.RaisePropertyChanged(() => this.ChildTreeItemsSearch);
                }
            }
        }

        #region Search Node

        private bool StartSearch = false;
        private int SearchLimit = 0;

        private string _searchText;
        public delegate void SearchNodeInvoke(string text);

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {

                    _searchText = value;
                    this.RaisePropertyChanged(() => this.SearchText);

                    if (string.IsNullOrEmpty(value) || value == "")
                    {
                        IsSearchTreeVisi = Visibility.Collapsed;
                    }
                    if (SearchLimit == 1) return;
                    SearchNodeold(_searchText);

                    if (UxTreeSetting.IsSelectGrpMapOnlyShow == false) return;
                    if (string.IsNullOrEmpty(value) || value == "")
                    {
                        IsSearchTreeVisi = Visibility.Collapsed;
                        var ins = new PublishEventArgs()
                        {
                            EventType = PublishEventType.Core,
                            EventId =
                                Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.
                                RtuGroupSelectdWantedMapUp
                        };
                        EventPublish.PublishEvent(ins);
                    }

                    //////StartSearch = true;
                    //////timer_count = 0;
                    //if (UxTreeSetting.IsShowRapidOp == 1)
                    //{

                    //SearchNode(_searchText);
                    //}
                    //else
                    //{
                    //    SearchNodeold(_searchText);
                    //}


                }
            }
        }

        //CmdClearUpSearchText
        #region CmdClearUpSearchText

        private ICommand _cmdCmdClearUpSearchText;

        public ICommand CmdClearUpSearchText
        {
            get
            {
                if (_cmdCmdClearUpSearchText == null)
                    _cmdCmdClearUpSearchText = new RelayCommand(ExCmdClearUpSearchText, CanCmdClearUpSearchText, false);

                return _cmdCmdClearUpSearchText;
            }
        }

        private void ExCmdClearUpSearchText()
        {
            SearchText = "";


            if (UxTreeSetting.IsSelectGrpMapOnlyShow == false) return;
            var ins = new PublishEventArgs()
            {
                EventType = PublishEventType.Core,
                EventId =
                    Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
            };

            var info = new List<int>();
            ins.AddParams(info);

            EventPublish.PublishEvent(ins);



        }



        private bool CanCmdClearUpSearchText()
        {
            return ChildTreeItemsSearch.Count > 0;
            return !string.IsNullOrEmpty(SearchText);

        }



        #endregion

        #region CmdQuickQuery

        private ICommand _cmdQuickQuery;

        public ICommand CmdQuickQuery
        {
            get
            {
                if (_cmdQuickQuery == null)
                    _cmdQuickQuery = new RelayCommand<string>(ExCmdQuickQuery, CanCmdQuickQuery, false);
                return _cmdQuickQuery;
            }
        }

        private void ExCmdQuickQuery(string s)
        {

            int x = 0;
            if (Int32.TryParse(s, out x))
            {
                if (x == 1)
                {
                    SearchNodeold("掉线");
                }
                else if (x == 2)
                {
                    SearchNodeold("亮灯");
                }
                else if (x == 3)
                {
                    SearchNodeold("灭灯");
                }
                else if (x == 4)
                {
                    SearchNodeold("停电");
                }
            }

        }

        private bool CanCmdQuickQuery(string s)
        {
            return true;
        }


        #endregion





        ObservableCollection<TreeNodeBaseNode> Getallnode(TreeNodeBaseNode node)
        {
            foreach (var f in node.ChildTreeItems) if (f.NodeType == TypeOfTabTreeNode.IsAll) return f.ChildTreeItems;
            return new ObservableCollection<TreeNodeBaseNode>();
        }
        //查询终端

        private ConcurrentQueue<string> searchKeys = new ConcurrentQueue<string>();
        private bool _running = false;
        private void SearchNode(string keyWord)
        {
            searchKeys.Enqueue(keyWord);
            Application.Current.Dispatcher.Invoke(
                new Action(() =>
                {
                    if (_running) return;
                    _running = true;
                    try
                    {
                        string key = string.Empty;
                        while (searchKeys.TryDequeue(out key))
                        {
                            if (searchKeys.Count > 0) continue;
                            if (string.IsNullOrEmpty(key))
                            {
                                IsSearchTreeVisi = Visibility.Collapsed;
                                ChildTreeItemsSearch.Clear();
                            }
                            else
                            {
                                var tmpList2 = SearchNodepri(key);
                                ChildTreeItemsSearch.Clear();
                                int index = 0;

                                bool needbreak = false;
                                foreach (var t in tmpList2)
                                {
                                    if (searchKeys.Count > 0)
                                    {
                                        needbreak = true;
                                        break;
                                    }
                                    index++;
                                    ChildTreeItemsSearch.Add(t);
                                    if (index % 20 == 1)
                                    {
                                        Wlst.Cr.CoreOne.OtherHelper.Delay.DelayEvent();

                                    }
                                }
                                if (needbreak) continue;
                                IsSearchTreeVisi = Visibility.Visible;

                            }

                            if (UxTreeSetting.IsSelectGrpMapOnlyShow == false) return;

                            var ins = new PublishEventArgs()
                            {
                                EventType = PublishEventType.Core,
                                EventId =
                                    Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign
                                    .RtuGroupSelectdWantedMapUp
                            };

                            if (string.IsNullOrEmpty(key)) EventPublish.PublishEvent(ins);
                            else
                            {
                                var info = (from t in ChildTreeItemsSearch select t.NodeId).ToList();
                                ins.AddParams(info);
                                if (info.Count > 0)
                                {
                                    EventPublish.PublishEvent(ins);
                                }
                            }





                            if (OnClearSerchTest != null)
                                OnClearSerchTest(this, new NodeSelectedArgs() { SearchText = key });


                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }), DispatcherPriority.DataBind);
            _running = false;
        }




        //private long searchtime = 0;
        private List<TreeNodeBaseNode> SearchNodepri(string keyWord)
        {

            if (keyWord == "")
            {
                // IsSearchTreeVisi = Visibility.Collapsed;
                return new List<TreeNodeBaseNode>();
            }

            //var kesss =
            //    (from t in GrpComSingleMuliViewModel.BaseNodes.Nodess.Keys orderby t ascending select t).ToList();

            var lst = new List<TreeNodeBaseNode>();
            foreach (var f in ChildTreeItems)
            {
                if (f.NodeType == TypeOfTabTreeNode.IsAll)
                {
                    lst.AddRange(f.ChildTreeItems);
                }
                else if (f.NodeType == TypeOfTabTreeNode.IsArea)
                {
                    lst.AddRange(Getallnode(f));
                }

            }
            #region edit

            List<TreeNodeBaseNode> tmpList = new List<TreeNodeBaseNode>();
            if (keyWord.Length > 0)
            {
                if (keyWord.Contains(","))
                {
                    List<List<TreeNodeBaseNode>> MultiKeyLst = new List<List<TreeNodeBaseNode>>() { new List<TreeNodeBaseNode>() };
                    MultiKeyLst[0].AddRange(lst);
                    string[] keyLst = keyWord.Split(',');

                    for (int i = 0; i < keyLst.Length; i++)
                    {
                        if (keyLst[i] == "")
                        {
                            MultiKeyLst.Add(new List<TreeNodeBaseNode>());
                            MultiKeyLst[i + 1].AddRange(MultiKeyLst[i]);
                            continue;
                        }
                        MultiKeyLst.Add(new List<TreeNodeBaseNode>());

                        #region foreach

                        foreach (var nodeId in MultiKeyLst[i])
                        {
                            //nodeId.ExtendSerachConten = null;
                            nodeId.Mark = null;
                            if (nodeId.PhyId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("物理地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += "物理地址-" + nodeId.PhyId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.NodeId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("逻辑地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.PhoneNumber.Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("手机号码")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (StringContainKeyword(nodeId.NodeName, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端名称")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端名称";
                                }

                                nodeId.Mark = "mark";
                            }


                            if (StringContainKeyword(nodeId.IpAddr, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("Ip")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " Ip-" + nodeId.IpAddr.Trim();
                                }

                                nodeId.Mark = "mark";
                            }




                            //if (StringContainKeyword(nodeId.RtuOnly, keyLst[i]))
                            //{
                            //    if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端识别号")) || nodeId.ExtendSerachConten == null)
                            //    {
                            //        nodeId.ExtendSerachConten += " 终端识别号-" + nodeId.RtuOnly;
                            //    }

                            //    nodeId.Mark = "mark";
                            //}

                           
                            if (nodeId.Mark != null) MultiKeyLst[i + 1].Add(nodeId);

                            // ChildTreeItemsSearch.Add(nodeId);

                        }


                        #endregion
                    }
                    tmpList.AddRange(MultiKeyLst[MultiKeyLst.Count - 1]);
                }
                else if (keyWord.Contains("，"))
                {
                    List<List<TreeNodeBaseNode>> MultiKeyLst = new List<List<TreeNodeBaseNode>>() { new List<TreeNodeBaseNode>() };
                    MultiKeyLst[0].AddRange(lst);
                    string[] keyLst = keyWord.Split('，');

                    for (int i = 0; i < keyLst.Length; i++)
                    {
                        if (keyLst[i] == "") continue;
                        MultiKeyLst.Add(new List<TreeNodeBaseNode>());

                        #region foreach

                        foreach (var nodeId in MultiKeyLst[i])
                        {
                            //nodeId.ExtendSerachConten = null;
                            nodeId.Mark = null;
                            if (nodeId.PhyId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("物理地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += "物理地址-" + nodeId.PhyId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.NodeId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("逻辑地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.PhoneNumber.Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("手机号码")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (StringContainKeyword(nodeId.NodeName, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端名称")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端名称";
                                }

                                nodeId.Mark = "mark";
                            }


                            if (StringContainKeyword(nodeId.IpAddr, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("Ip")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " Ip-" + nodeId.IpAddr.Trim();
                                }

                                nodeId.Mark = "mark";
                            }



                         

                            //if (StringContainKeyword(nodeId.RtuOnly, keyLst[i]))
                            //{
                            //    if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端识别号")) || nodeId.ExtendSerachConten == null)
                            //    {
                            //        nodeId.ExtendSerachConten += " 终端识别号-" + nodeId.RtuOnly;
                            //    }

                            //    nodeId.Mark = "mark";
                            //}

                            if (nodeId.Mark != null) MultiKeyLst[i + 1].Add(nodeId);

                            // ChildTreeItemsSearch.Add(nodeId);

                        }


                        #endregion
                    }
                    tmpList.AddRange(MultiKeyLst[MultiKeyLst.Count - 1]);
                }

                else
                {
                    #region foreach

                    foreach (var nodeId in lst)
                    {
                        nodeId.ExtendSerachConten = null;
                        if (nodeId.PhyId.ToString().Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten = "物理地址-" + nodeId.PhyId;

                        }
                        if (nodeId.NodeId.ToString().Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;


                        }
                        if (nodeId.PhoneNumber.Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;


                        }

                        if (StringContainKeyword(nodeId.NodeName, keyWord))
                        {
                            nodeId.ExtendSerachConten += " 终端名称";

                        }


                        if (StringContainKeyword(nodeId.IpAddr, keyWord))
                        {
                            nodeId.ExtendSerachConten += " Ip-" + nodeId.IpAddr.Trim();

                        }




                        //if (StringContainKeyword(nodeId.RtuOnly, keyWord))
                        //{
                        //    nodeId.ExtendSerachConten += " 终端识别号-" + nodeId.RtuOnly;

                        //}

                     
                        if (nodeId.ExtendSerachConten != null) tmpList.Add(nodeId);
                        // ChildTreeItemsSearch.Add(nodeId);

                    }

                    #endregion
                }

            }
            #endregion

            var tmpList2 = (from t in tmpList orderby t.NodeId ascending select t).ToList();
            return tmpList2;

            int index = 0;
            foreach (var t in tmpList2)
            {

                index++;
                ChildTreeItemsSearch.Add(t);
                if (index % 20 == 1)
                {
                    Wlst.Cr.CoreOne.OtherHelper.Delay.DelayEvent();

                }
            }


            IsSearchTreeVisi = Visibility.Visible;

            var ins = new PublishEventArgs()
            {
                EventType = PublishEventType.Core,
                EventId =
                    Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
            };


            var info = (from t in ChildTreeItemsSearch select t.NodeId).ToList();
            ins.AddParams(info);
            if (info.Count > 0)
            {
                EventPublish.PublishEvent(ins);
            }

        }

        private List<TreeNodeBaseNode> tmpList2 = new List<TreeNodeBaseNode>();
        private List<TreeNodeBaseNode> tmpListChk = new List<TreeNodeBaseNode>();
        public void SearchNodeold(string keyWord)
        {
            tmpList2.Clear();
            ChildTreeItemsSearch.Clear();
            if (keyWord == "")
            {
                IsSearchTreeVisi = Visibility.Collapsed;
                ChildTreeItemsSearch.Clear();
                return;
            }

            //var kesss =
            //    (from t in GrpComSingleMuliViewModel.BaseNodes.Nodess.Keys orderby t ascending select t).ToList();

            var lst = new List<TreeNodeBaseNode>();
            foreach (var f in ChildTreeItems)
            {
                if (f.NodeType == TypeOfTabTreeNode.IsGrp)
                {
                    lst.AddRange(f.ChildTreeItems);
                }
                else if (f.NodeType == TypeOfTabTreeNode.IsArea)
                {
                    lst.AddRange(Getallnode(f));
                }

            }
            #region edit

            List<TreeNodeBaseNode> tmpList = new List<TreeNodeBaseNode>();
            if (keyWord.Length > 0)
            {
                if (keyWord.Contains(","))
                {
                    List<List<TreeNodeBaseNode>> MultiKeyLst = new List<List<TreeNodeBaseNode>>() { new List<TreeNodeBaseNode>() };
                    MultiKeyLst[0].AddRange(lst);
                    string[] keyLst = keyWord.Split(',');

                    for (int i = 0; i < keyLst.Length; i++)
                    {
                        if (keyLst[i] == "")
                        {
                            MultiKeyLst.Add(new List<TreeNodeBaseNode>());
                            MultiKeyLst[i + 1].AddRange(MultiKeyLst[i]);
                            continue;
                        }
                        MultiKeyLst.Add(new List<TreeNodeBaseNode>());

                        #region foreach

                        foreach (var nodeId in MultiKeyLst[i])
                        {
                            //nodeId.ExtendSerachConten = null;
                            nodeId.Mark = null;
                            if (nodeId.PhyId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("物理地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += "物理地址-" + nodeId.PhyId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.NodeId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("逻辑地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.PhoneNumber.Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("手机号码")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (StringContainKeyword(nodeId.RtuName, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端名称")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端名称";
                                }

                                nodeId.Mark = "mark";
                            }




                            //if (StringContainKeyword(nodeId.RtuOnly, keyLst[i]))
                            //{
                            //    if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端识别号")) || nodeId.ExtendSerachConten == null)
                            //    {
                            //        nodeId.ExtendSerachConten += " 终端识别号-" + nodeId.RtuOnly;
                            //    }

                            //    nodeId.Mark = "mark";
                            //}

                            if (StringContainKeyword(nodeId.MruName, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("电表名称")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 电表名称";
                                }

                                nodeId.Mark = "mark";
                            }

                            if (StringContainKeyword(nodeId.MruRemark, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("电表备注")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 电表备注";
                                }

                                nodeId.Mark = "mark";
                            }


                            if (nodeId.Mark != null) MultiKeyLst[i + 1].Add(nodeId);

                            // ChildTreeItemsSearch.Add(nodeId);

                        }


                        #endregion
                    }
                    tmpList.AddRange(MultiKeyLst[MultiKeyLst.Count - 1]);
                }
                else if (keyWord.Contains("，"))
                {
                    List<List<TreeNodeBaseNode>> MultiKeyLst = new List<List<TreeNodeBaseNode>>() { new List<TreeNodeBaseNode>() };
                    MultiKeyLst[0].AddRange(lst);
                    string[] keyLst = keyWord.Split('，');

                    for (int i = 0; i < keyLst.Length; i++)
                    {
                        if (keyLst[i] == "") continue;
                        MultiKeyLst.Add(new List<TreeNodeBaseNode>());

                        #region foreach

                        foreach (var nodeId in MultiKeyLst[i])
                        {
                            //nodeId.ExtendSerachConten = null;
                            nodeId.Mark = null;
                            if (nodeId.PhyId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("物理地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += "物理地址-" + nodeId.PhyId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.NodeId.ToString().Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("逻辑地址")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (nodeId.PhoneNumber.Contains(keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("手机号码")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;
                                }

                                nodeId.Mark = "mark";

                            }

                            if (StringContainKeyword(nodeId.RtuName, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端名称")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 终端名称";
                                }

                                nodeId.Mark = "mark";
                            }

                            if (StringContainKeyword(nodeId.MruName, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("电表名称")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 电表名称";
                                }

                                nodeId.Mark = "mark";
                            }

                            if (StringContainKeyword(nodeId.MruRemark, keyLst[i]))
                            {
                                if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("电表备注")) || nodeId.ExtendSerachConten == null)
                                {
                                    nodeId.ExtendSerachConten += " 电表备注";
                                }

                                nodeId.Mark = "mark";
                            }
                            //if (StringContainKeyword(nodeId.RtuOnly, keyLst[i]))
                            //{
                            //    if ((nodeId.ExtendSerachConten != null && !nodeId.ExtendSerachConten.Contains("终端识别号")) || nodeId.ExtendSerachConten == null)
                            //    {
                            //        nodeId.ExtendSerachConten += " 终端识别号-" + nodeId.RtuOnly;
                            //    }

                            //    nodeId.Mark = "mark";
                            //}

                          
                            if (nodeId.Mark != null) MultiKeyLst[i + 1].Add(nodeId);

                            // ChildTreeItemsSearch.Add(nodeId);

                        }


                        #endregion
                    }
                    tmpList.AddRange(MultiKeyLst[MultiKeyLst.Count - 1]);
                }

                else
                {
                    #region foreach

                    foreach (var nodeId in lst)
                    {
                        nodeId.ExtendSerachConten = null;
                        if (nodeId.PhyId.ToString().Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten = "物理地址-" + nodeId.PhyId;

                        }
                        if (nodeId.NodeId.ToString().Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;


                        }
                        if (nodeId.PhoneNumber.Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;


                        }

                        if (StringContainKeyword(nodeId.RtuName, keyWord))
                        {
                            nodeId.ExtendSerachConten += " 终端名称";

                        }
                        if (StringContainKeyword(nodeId.MruRemark, keyWord))
                        {
                            nodeId.ExtendSerachConten += " 电表备注";

                        }
                        if (StringContainKeyword(nodeId.MruName, keyWord))
                        {
                            nodeId.ExtendSerachConten += " 电表名称";

                        }

                     
                        if (nodeId.ExtendSerachConten != null) tmpList.Add(nodeId);


                        //if (StringContainKeyword(nodeId.RtuOnly, keyWord))
                        //{
                        //    nodeId.ExtendSerachConten += " 终端识别号-" + nodeId.RtuOnly;

                        //}


                   

                   
                    }

                    #endregion
                }

            }
            #endregion

            tmpList2 = (from t in tmpList orderby t.NodeId ascending select t).ToList();
            int index = 0;
            foreach (var t in tmpList2)
            {

                index++;
                if (SearchLimit != 0 && SearchLimit != 1 && index > SearchLimit) break;//todo lvf test
                ChildTreeItemsSearch.Add(t);
                //if (index % 20 == 1)
                //{
                //    Wlst.Cr.CoreOne.OtherHelper.Delay.DelayEvent();
                //}
            }


            IsSearchTreeVisi = Visibility.Visible;

            //if (UxTreeSetting.IsSelectGrpMapOnlyShow == false) return;
            //var ins = new PublishEventArgs()
            //{
            //    EventType = PublishEventType.Core,
            //    EventId =
            //        Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
            //};


            //var info = (from t in ChildTreeItemsSearch select t.NodeId).ToList();
            //ins.AddParams(info);
            //if (info.Count > 0)
            //{
            //    EventPublish.PublishEvent(ins);
            //}

        }
        private Visibility _isSearchTreeVisi;

        public Visibility IsSearchTreeVisi
        {
            get { return _isSearchTreeVisi; }
            set
            {
                if (value == _isSearchTreeVisi) return;
                _isSearchTreeVisi = value;
                this.RaisePropertyChanged(() => this.IsSearchTreeVisi);
            }
        }


        //private Visibility _isShowRapidOper;

        //public Visibility IsShowRapidOper
        //{
        //    get { return _isShowRapidOper; }
        //    set
        //    {
        //        if (value == _isShowRapidOper) return;
        //        _isShowRapidOper = value;
        //        this.RaisePropertyChanged(() => this.IsShowRapidOper);
        //    }
        //}





        /// <summary>
        /// 前者是否包含后者数据 
        /// </summary>
        /// <param name="containerStinng"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private bool StringContainKeyword(string containerStinng, string keyword)
        {
            if (containerStinng == null) return false;
            if (containerStinng.Contains(keyword)) return true;
            string conv = chinesecap(containerStinng);
            if (conv.Contains(keyword)) return true;
            if (containerStinng.ToUpper().Contains(keyword.ToUpper())) return true;
            return false;
        }


        /// <summary>
        /// 返回汉字字符串的拼音的首字母
        /// </summary>
        /// <param name="chinesestr">要转换的字符串</param>
        /// <returns></returns>
        public string chinesecap(string chinesestr)
        {
            byte[] zw = new byte[2];
            string charstr = "";
            string capstr = "";
            for (int i = 0; i <= chinesestr.Length - 1; i++)
            {
                charstr = chinesestr.Substring(i, 1).ToString(CultureInfo.InvariantCulture);
                zw = System.Text.Encoding.Default.GetBytes(charstr);
                // 得到汉字符的字节数组
                if (zw.Length == 2)
                {
                    int i1 = (short)(zw[0]);
                    int i2 = (short)(zw[1]);
                    long chinesestrInt = i1 * 256 + i2;
                    //table of the constant list
                    // a; //45217..45252
                    // z; //54481..55289
                    capstr += GetChinesefirst(chinesestrInt);
                }
                else
                {
                    capstr += charstr;
                }

                //capstr = capstr + chinastr;
            }

            return capstr;
        }

        private string GetChinesefirst(long chinesestrInt)
        {
            string chinastr = "";
            //table of the constant list
            // a; //45217..45252
            // b; //45253..45760
            // c; //45761..46317
            // d; //46318..46825
            // e; //46826..47009
            // f; //47010..47296
            // g; //47297..47613

            // h; //47614..48118
            // j; //48119..49061
            // k; //49062..49323
            // l; //49324..49895
            // m; //49896..50370
            // n; //50371..50613
            // o; //50614..50621
            // p; //50622..50905
            // q; //50906..51386

            // r; //51387..51445
            // s; //51446..52217
            // t; //52218..52697
            //没有u,v
            // w; //52698..52979
            // x; //52980..53640
            // y; //53689..54480
            // z; //54481..55289

            if ((chinesestrInt >= 45217) && (chinesestrInt <= 45252))
            {
                chinastr = "a";
            }
            else if ((chinesestrInt >= 45253) && (chinesestrInt <= 45760))
            {
                chinastr = "b";
            }
            else if ((chinesestrInt >= 45761) && (chinesestrInt <= 46317))
            {
                chinastr = "c";
            }
            else if ((chinesestrInt >= 46318) && (chinesestrInt <= 46825))
            {
                chinastr = "d";
            }
            else if ((chinesestrInt >= 46826) && (chinesestrInt <= 47009))
            {
                chinastr = "e";
            }
            else if ((chinesestrInt >= 47010) && (chinesestrInt <= 47296))
            {
                chinastr = "f";
            }
            else if ((chinesestrInt >= 47297) && (chinesestrInt <= 47613))
            {
                chinastr = "g";
            }
            else if ((chinesestrInt >= 47614) && (chinesestrInt <= 48118))
            {
                chinastr = "h";
            }

            else if ((chinesestrInt >= 48119) && (chinesestrInt <= 49061))
            {
                chinastr = "j";
            }
            else if ((chinesestrInt >= 49062) && (chinesestrInt <= 49323))
            {
                chinastr = "k";
            }
            else if ((chinesestrInt >= 49324) && (chinesestrInt <= 49895))
            {
                chinastr = "l";
            }
            else if ((chinesestrInt >= 49896) && (chinesestrInt <= 50370))
            {
                chinastr = "m";
            }

            else if ((chinesestrInt >= 50371) && (chinesestrInt <= 50613))
            {
                chinastr = "n";
            }
            else if ((chinesestrInt >= 50614) && (chinesestrInt <= 50621))
            {
                chinastr = "o";
            }
            else if ((chinesestrInt >= 50622) && (chinesestrInt <= 50905))
            {
                chinastr = "p";
            }
            else if ((chinesestrInt >= 50906) && (chinesestrInt <= 51386))
            {
                chinastr = "q";
            }

            else if ((chinesestrInt >= 51387) && (chinesestrInt <= 51445))
            {
                chinastr = "r";
            }
            else if ((chinesestrInt >= 51446) && (chinesestrInt <= 52217))
            {
                chinastr = "s";
            }
            else if ((chinesestrInt >= 52218) && (chinesestrInt <= 52697))
            {
                chinastr = "t";
            }
            else if ((chinesestrInt >= 52698) && (chinesestrInt <= 52979))
            {
                chinastr = "w";
            }
            else if ((chinesestrInt >= 52980) && (chinesestrInt <= 53640))
            {
                chinastr = "x";
            }
            else if ((chinesestrInt >= 53689) && (chinesestrInt <= 54480))
            {
                chinastr = "y";
            }
            else if ((chinesestrInt >= 54481) && (chinesestrInt <= 55289))
            {
                chinastr = "z";
            }
            return chinastr;
        }

        #endregion




    }

}
