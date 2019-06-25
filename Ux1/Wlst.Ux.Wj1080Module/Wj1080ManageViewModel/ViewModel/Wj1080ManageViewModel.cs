using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media.Imaging;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.Wj1080Module.Wj1080ManageSettingViewModel.ViewModel;
using Wlst.Ux.Wj1080Module.Wj1080ManageViewModel.Sercives;
using Wlst.client;

namespace Wlst.Ux.Wj1080Module.Wj1080ManageViewModel.ViewModel
{
    [Export(typeof(IIWj1080ManageViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1080ManageViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, Sercives.IIWj1080ManageViewModel
    {
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

        public Wj1080ManageViewModel()
        {
            MySelf = this;
            //Load();
           EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(LoadNode, 1, DelayEventHappen.EventOne);
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            //  Load();
        }


        public static Wj1080ManageViewModel MySelf;
        // private static TreeNodeWj1080ViewModel _currentSelectedTreeNode;

        //public static TreeNodeWj1080ViewModel CurrentSelectedTreeNode
        //{
        //    get { return _currentSelectedTreeNode; }
        //    set
        //    {
        //        //if (_currentSelectedTreeNode != null && _currentSelectedTreeNode != value)
        //        //    _currentSelectedTreeNode.IsSelected = false;

        //        if (_currentSelectedTreeNode != value)
        //        {
        //            _currentSelectedTreeNode = value;
        //            if (_currentSelectedTreeNode != null)
        //            {
        //                //  var view = CoreRun.CoreService.CoreServices.GetViewById(0, _currentSelectedTreeNode.NodeId);
        //                if (MySelf != null)
        //                {
        //                    MySelf.UpdateViewByLuxId(_currentSelectedTreeNode.NodeId);
        //                }
        //            }
        //        }
        //    }
        //}


        public static bool OnSelectNodeChangeNavToParsSet = true;
        //     private static TreeNodeWj1080ViewModel _currentselectednode = null;
        public void UpdateViewByLuxId(TreeNodeWj1080ViewModel luxinfo)
        {
            //if (_currentselectednode != null) _currentselectednode.IsSelected = false;
            // _currentselectednode = luxinfo;

            if (OnSelectNodeChangeNavToParsSet == false) return;
            //this.NavOnLoadByBase(luxId);
            if (luxinfo == null) return;
            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(
                Wj1080Module.Services.ViewIdAssign.Wj1080TmlInfoSetViewId, luxinfo.NodeId);
        }


        #region Reflesh

        private DateTime _dtReflesh;
        public ICommand _reflesh;

        public ICommand Reflesh
        {
            get
            {
                if (_reflesh == null) _reflesh = new RelayCommand(ExReflesh, CanExReflesh, true);
                return _reflesh;
            }
        }

        private bool CanExReflesh()
        {
            return DateTime.Now.Ticks - _dtReflesh.Ticks > 30000000;
        }

        private void ExReflesh()
        {
            _dtReflesh = DateTime.Now;
            this.LoadNode();
        }

        #endregion

        private ObservableCollection<TreeNodeWj1080ViewModel> _collectionWj1080;

        /// <summary>
        /// 开关量输入参数
        /// </summary>

        public ObservableCollection<TreeNodeWj1080ViewModel> CollectionWj1080
        {
            get
            {
                if (_collectionWj1080 == null)
                    _collectionWj1080 = new ObservableCollection<TreeNodeWj1080ViewModel>();
                return _collectionWj1080;
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
        private void LoadNode()
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
                if (wj1080TreeSetLoad.Myself.IsShowArea)
                {
                    foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                    {
                        //var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        //if (tmlLstOfArea.Count == 0) continue;
                         var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                         var rtuLst = new List<int>();
                         foreach (var a in lstInArea)
                         {
                             var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(a);
                             if (pb == null) continue;
                             if (pb.EquipmentType == WjParaBase.EquType.Lux && pb.RtuFid == 0) //线路为主设备
                             {
                                 rtuLst.Add(pb.RtuId);
                                 //if (IsLoadOnlyOneArea)
                                 //{
                                 //    int AreaId = areaLst[0];
                                 //    ShowGrpInArea(AreaId);
                                 //}else
                                 //{
                                 //    this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
                                 //}
                                 
                                 //break; ;
                             }
                             else if (pb.EquipmentType == WjParaBase.EquType.Rtu &&
                                      pb.EquipmentsThatAttachToThisRtu.Count > 0) //haha 特殊终端下有线路
                             {
                                 foreach (var g in pb.EquipmentsThatAttachToThisRtu)
                                 {
                                     var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                                     if (pa == null) continue;
                                     if (pa.EquipmentType == WjParaBase.EquType.Lux && pa.RtuFid > 0)
                                     {
                                         rtuLst.Add(g);
                                         
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
                        //var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        //if(tmlLstOfArea.Count ==0) continue;
                        //this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
                        var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        var rtuList = new List<int>();
                        foreach (var a in lstInArea)  
                        {
                            var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(a);
                            if (pb == null) continue;
                            if (pb.EquipmentType == WjParaBase.EquType.Lux && pb.RtuFid == 0) //光控为主设备
                            {
                                rtuList.Add(pb.RtuId);
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
                                    if (pa.EquipmentType == WjParaBase.EquType.Lux && pa.RtuFid > 0)
                                    {
                                        rtuList.Add(g);
                                        //this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
                                        //break;

                                    }
                                }
                            }
                        }
                        if (rtuList.Count > 0)
                        {

                                this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0,
                                                                                  TypeOfTabTreeNode.IsArea));
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

        private void ShowGrpInArea(int AreaId)
        {

            if (wj1080TreeSetLoad.Myself.IsShowGrp)
            {
                var grp =
                            (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                             where t.Key.Item1 == AreaId
                             orderby t.Value.Index
                             select t.Value).ToList();
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
                                if (pa.EquipmentType == WjParaBase.EquType.Lux && pa.RtuFid > 0)
                                {
                                    rtuList.Add(g);
                                }
                            }
                        }
                        else if (rtu.EquipmentType == WjParaBase.EquType.Lux && rtu.RtuFid == 0)
                        {
                            rtuList.Add(rtu.RtuId);
                        }
                    }
                    if (rtuList.Count < 1) continue;
                    this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f.AreaId, f.GroupId,
                                                                      TypeOfTabTreeNode.IsGrp));
                }
                var sp =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
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
                            if (pa.EquipmentType == WjParaBase.EquType.Lux && pa.RtuFid > 0)
                            {
                                rtuLst.Add(g);
                            }
                        }
                    }
                    else if (rtu.EquipmentType == WjParaBase.EquType.Lux  && rtu.RtuFid == 0)
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
                var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                foreach (var f in lstInArea)
                {
                    var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                    if (pb == null) continue;
                    if (pb.EquipmentType == WjParaBase.EquType.Lux && pb.RtuFid == 0) //光控为主设备
                    {
                        this.ChildTreeItems.Add(new TreeNodeWj1080ViewModel(pb.RtuId, pb.RtuName, pb.RtuFid));
                    }
                    else if (pb.EquipmentType == WjParaBase.EquType.Rtu && pb.EquipmentsThatAttachToThisRtu.Count > 0) //haha 特殊终端下有线路
                    {

                        foreach (var g in pb.EquipmentsThatAttachToThisRtu)
                        {
                            var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                            if (pa == null) continue;
                            if (pa.EquipmentType == WjParaBase.EquType.Lux && pa.RtuFid > 0)
                            {
                                this.ChildTreeItems.Add(new TreeNodeWj1080ViewModel(pa.RtuId, pa.RtuName, pa.RtuFid));
                            }
                        }
                    }
                }
            }
        }
        //    if (wj1080TreeSetLoad.Myself.IsShowArea == true)
        //    {
        //        foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
        //        {
        //            this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
        //        }
        //    }
        //    else if (wj1080TreeSetLoad.Myself.IsShowGrp == true)
        //    {
        //        foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
        //        {
        //            var grp =
        //                (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
        //                 where t.Key.Item1 == f
        //                 orderby t.Value.Index
        //                 select t.Value).ToList();

        //            //this.ChildTreeItems.Add(new TreeNodeAreaViewModel(this, AreaId, 0, TypeOfTabTreeNode.IsAll));
        //            foreach (var g in grp)
        //            {
        //                this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, g.AreaId, g.GroupId,
        //                                                                  TypeOfTabTreeNode.IsGrp));
        //            }
        //            var sp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(f);
        //            if (sp.Count > 0)
        //                this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0,
        //                                                                  TypeOfTabTreeNode.IsGrpSpecial));
        //        }
        //    }
        //    else
        //    {
        //        foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
        //        {
        //            var grp = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(f);
        //            if (grp == null) return;
        //            var gprs =
        //                Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp.LstTml);
        //            foreach (var g in gprs)
        //            {
        //                var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
        //                if (para == null || para.EquipmentType != WjParaBase.EquType.Mru) continue;
        //                this.ChildTreeItems.Add(new TreeNodeWj1080ViewModel(para.RtuId, para.RtuName, para.RtuFid));
        //            }
        //        }
        //    }
        //}


        #endregion
        private void Load()
        {
            CollectionWj1080.Clear();
            foreach (var t in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems //.EquipmentInfoDictionary 
                )
            {
                if (t.Key < 1400000 || t.Key > 1500000) continue;
                var fff = t.Value as Sr.EquipmentInfoHolding.Model.Wj1080Lux;
                if (fff == null) continue;
                var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(fff.RtuId);
                //var areaGrp = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(fff.RtuId);

                CollectionWj1080.Add(new TreeNodeWj1080ViewModel(fff.RtuId, fff.RtuName, fff.RtuFid));
            }


            //if (CollectionWj1080.Count > 0)
            //{
            //    CurrentSelectedTreeNode = CollectionWj1080[0];
            //}
        }


        public int Index
        {
            get { return 4; }
        }
        public bool CanClose
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public string Title
        {
            get { return "光控设备"; }
        }


        private object _imagesIcon;

        /// <summary>
        /// 前台界面绑定的图标
        /// </summary>
        public object ImageReflesh
        {
            get
            { return Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("Reflesh"); 
                
            }
        }
    }


    //event
    public partial class Wj1080ManageViewModel
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
                    //if (args.EventId == EventIdAssign.RunningInfoUpdate1)
                    //{
                    //    var lst = args.GetParams()[0] as IEnumerable<int>;
                    //    if (lst == null) return;
                    //    foreach (var t in lst)
                    //    {
                    //        if (TreeNodeItemSluViewModel.RtuItems.ContainsKey(t))
                    //        {
                    //            foreach (var f in TreeNodeItemSluViewModel.RtuItems[t])
                    //            {
                    //                if (f.Target != null)
                    //                {
                    //                    var xg = f.Target as SluTreeNodeBase;
                    //                    if (xg != null) xg.ReUpdate(2);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion
    }
}
