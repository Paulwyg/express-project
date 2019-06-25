using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;


using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.CrissCrossEquipemntTree.GrpComSingleMuliViewModel;
using Wlst.Ux.CrissCrossEquipemntTree.Models;
using Wlst.Ux.CrissCrossEquipemntTree.Resources;
using Wlst.client;
using System;

namespace Wlst.Ux.CrissCrossEquipemntTree.GrpSingleTabShowViewModel.ViewModels
{
    /// <summary>
    /// 提供终端树节点基本结构
    /// 右键菜单从MenuBuliding中动态获取，为节约程序资源，仅当点击该终端时该终端的右键菜单立即刷新
    /// IsMarked为联动标记，初始化时必须在外部初始化，即某一个分组具有联动则其子分组具有该联动属性
    /// </summary>
    public class TreeNodeItemSingleGroupViewModel : TreeNodeBaseNode 
    {
        private static TreeNodeBaseNode _currentSelectGroupNode;

        public static Dictionary<Tuple<int, int, int>, List<WeakReference>> GrpRelationItems = new Dictionary<Tuple<int, int, int>, List<WeakReference>>();

        public static TreeNodeBaseNode CurrentSelectGroupNode
        {
            get { return _currentSelectGroupNode; }
            set
            {
                if (_currentSelectGroupNode == value) return;
                if (value == null) return;
                if (value.NodeType == TypeOfTabTreeNode.IsGrp ||
                    value.NodeType == TypeOfTabTreeNode.IsGrpSpecial ||
                    value.NodeType == TypeOfTabTreeNode.IsArea ||
                    value.NodeType == TypeOfTabTreeNode.IsAll)
                {
                    if (_currentSelectGroupNode != null)
                        _currentSelectGroupNode.IsSelected = false;
                    _currentSelectGroupNode = value;


                    if (UxTreeSetting.IsSelectGrpMapOnlyShow == false) return;
                    var ins = new PublishEventArgs()
                                  {
                                      EventType = PublishEventType.Core,
                                      EventId =
                                          Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
                                  };

                    var info = new List<int>();
                    //if (value.NodeType == TypeOfTabTreeNode.IsAll || value.NodeType == TypeOfTabTreeNode.IsArea)
                    //{

                    //    info.Add(-1);
                    //    ins.AddParams(info);
                    //}
                    //else
                    {
                        info = (from t in value.ChildTreeItems select t.NodeId).ToList();
                        ins.AddParams(info);
                    }

                    if (info.Count == 0) return;
                    if (info.Count == 1 && info[0] == -1) info.Clear();
                    EventPublish.PublishEvent(ins);
                }

            }
        }

        //public TreeNodeItemSingleGroupViewModel()
        //{
        //    this.NodeType = TypeOfTabTreeNode.IsGrp;
        //    //Visi = Visibility.Visible;
        //}

        public TreeNodeItemSingleGroupViewModel(TreeNodeBaseNode mvvmFather, int areaId, int groupId, TypeOfTabTreeNode type)
        {

            this.AreaId = areaId;
            this.NodeType = type;
            //Visi = Visibility.Visible;
            this._father = mvvmFather;
            //TreeSingleViewModel.RegisterNodeToControl(this);

            var nodename = "--";
            if (type == TypeOfTabTreeNode.IsGrpSpecial)
            {
                nodename = "特殊终端";
                this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
                this.NodeId = 0;

            }
            if (type == TypeOfTabTreeNode.IsRegionSpecial)
            {
                nodename = "特殊终端";
                this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
                this.NodeId = 0;

            }

            if (type == TypeOfTabTreeNode.IsAll)
            {
                nodename = "全部终端";
                this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
                this.NodeId = 0;
            }
            if (type == TypeOfTabTreeNode.IsArea)
            {
                this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;

                this.NodeId = areaId;
                var areaInfo = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef;
                foreach (var f in areaInfo.AreaInfo)
                {
                    if (f.Value.AreaId == areaId)
                    {
                        nodename = f.Value.AreaName;
                    }
                }
            }
            if (type == TypeOfTabTreeNode.IsGrp)
            {
                var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId,
                                                                                                              groupId);
                if (info != null)
                {
                    nodename = info.GroupName;

                }

                this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
                this.NodeId = groupId;
            }

            this.NodeName = nodename;
            this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
            this.AddChild();


            //if (ChildTreeItems.Count == 0)
            //{
            //    if (mvvmFather != null)
            //    {
            //        bool flag = false;

            //        do
            //        {
            //            flag = false;

            //            for (int i = 0; i < mvvmFather.ChildTreeItems.Count; i++)
            //            {
            //                if (mvvmFather.ChildTreeItems[i].ChildTreeItems.Count == 0)
            //                {
            //                    mvvmFather.DeleteChild(i);
            //                    flag = true;
            //                    break;
            //                }
            //            }

            //        } while (flag == true);
            //    }
            //}
        }








        /// <summary>
        /// 
        /// </summary>
        /// <param name="mvvmFather"></param>
        /// <param name="grpInfo">grpid,grpname</param>
        /// <param name="type"></param>
        public TreeNodeItemSingleGroupViewModel(TreeNodeBaseNode mvvmFather,int areaId,int grpId,string grpName, TypeOfTabTreeNode type)
        {

            //this.AreaId = areaId;
            this.NodeType = type;
            //Visi = Visibility.Visible;
            this._father = mvvmFather;
            //TreeSingleViewModel.RegisterNodeToControl(this);


            //记录分组  组合 id  ,下级还有分组 -99,特殊分组为0 ,全部分组为 0 无视 就一个
            var tukey = new Tuple<int, int, int>(0,0,0);

            var nodename = grpName;
            if (type == TypeOfTabTreeNode.IsGrpSpecial)
            {
                tukey = new Tuple<int, int, int>(areaId,0,-99);
                nodename = "特殊终端";
                this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
                this.NodeId = 0;

            }
            if (type == TypeOfTabTreeNode.IsAll)
            {
                tukey = new Tuple<int, int, int>(areaId, 0, 0);
                nodename = "全部终端";
                this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
                this.NodeId = 0;
            }
            if (type == TypeOfTabTreeNode.IsArea)
            {
                tukey = new Tuple<int, int, int>(areaId, -99,-99);

                this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;

                this.NodeId = areaId;
                var areaInfo = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef;
                foreach (var f in areaInfo.AreaInfo)
                {
                    if (f.Value.AreaId == areaId)
                    {
                        nodename = f.Value.AreaName;
                    }
                }
            }
            if (type == TypeOfTabTreeNode.IsGrp )
            {
                tukey = new Tuple<int, int, int>(areaId,grpId, -99);
                this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
                this.NodeId = grpId;
            }

            if (type == TypeOfTabTreeNode.IsRegion)
            {
                tukey = new Tuple<int, int, int>(areaId,mvvmFather.NodeId,grpId);
                this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
                this.NodeId = grpId;
            }

            if (type == TypeOfTabTreeNode.IsRegionSpecial)
            {
                tukey = new Tuple<int, int, int>(areaId, mvvmFather.NodeId, 0);
                nodename = "特殊终端";
                this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
                this.NodeId = 0;

            }

            WeakReference refs = new WeakReference(this);
            if (GrpRelationItems.ContainsKey(tukey) == false)
                GrpRelationItems.Add(tukey, new List<WeakReference>());
            GrpRelationItems[tukey].Add(refs);


            this.NodeName = nodename;
            this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
            this.AddChild();


        }


        /// <summary>
        /// 加载节点，第一次使用
        /// </summary>
        public override void AddChild()
        {
            ChildTreeItems.Clear();
           
            if (NodeType == TypeOfTabTreeNode.IsGrp)
            {
                var regionGrps = ServiceGrpRegionInfoHold.GetGrpByType(AreaId,1);
                //先加载全局分组  再加载地区分组
                if (regionGrps == null || regionGrps.Count == 0) return;
                foreach (var f in regionGrps)
                {
                    this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(this,AreaId,f.Key, f.Value,
                                                                                 TypeOfTabTreeNode.IsRegion));
                }

                //添加特殊分组  有grpid 未分区域
                var spLst = Wlst.Sr.EquipmentInfoHolding.Services.ServiceGrpRegionInfoHold.GetRtulstByGrpRegionId(AreaId,NodeId, 0);
                if (spLst == null) return;
                if (spLst.Count == 0) return;


                this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(this, AreaId, NodeId,"特殊终端",
                                                                                 TypeOfTabTreeNode.IsRegionSpecial));





            }
            if (NodeType == TypeOfTabTreeNode.IsRegion)
            {
                var rtulst = Wlst.Sr.EquipmentInfoHolding.Services.ServiceGrpRegionInfoHold.GetRtulstByGrpRegionId(AreaId,_father.NodeId, NodeId);
                if (rtulst == null || rtulst.Count == 0) return;

                foreach (var f in rtulst)
                {
                  
                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f.Item1);
                    if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;
                    var imageInfo = Wlst.Sr.EquipmentInfoHolding.Services.ServiceGrpRegionInfoHold.GetImageIdByRtuid(f.Item1);


                    this.ChildTreeItems.Add(new TreeNodeItemTmlViewModel(this, para));
       
                }
            }
            //有grpid  没有地区的设备
            if (NodeType == TypeOfTabTreeNode.IsRegionSpecial)
            {
                var rtulst = Wlst.Sr.EquipmentInfoHolding.Services.ServiceGrpRegionInfoHold.GetRtulstByGrpRegionId(AreaId,_father.NodeId, 0);
                if (rtulst == null || rtulst.Count == 0) return;
                foreach (var f in rtulst)
                {

                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f.Item1);
                    if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;

                    this.ChildTreeItems.Add(new TreeNodeItemTmlViewModel(this, para));
                }
            }

            if (NodeType == TypeOfTabTreeNode.IsAll)
            {
                var grp = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(AreaId);
                var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                if (grp == null) return;
                var gprs = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp.LstTml,AreaId ,true );
                
                foreach (var f in gprs.Item1 )
                {
                    if (tmlLstOfArea.Contains(f) == false) continue;
                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                    if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;

                    var vol = para as Wj3005Rtu;

                    this.ChildTreeItems.Add(new TreeNodeItemTmlViewModel(this, para, true));
     
                }
                var sortLst =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(gprs.Item2);
                foreach (var f in sortLst )
                {
                    if (tmlLstOfArea.Contains(f) == false) continue;
                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                    if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;
                    var vol = para as Wj3005Rtu;


                    this.ChildTreeItems.Add(new TreeNodeItemTmlViewModel(this, para));

                }
            }
            if (NodeType == TypeOfTabTreeNode.IsGrpSpecial)
            {
                var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
                if (grp.Count == 0) return;
                var gprs = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp);
                foreach (var f in gprs)
                {
                    if (tmlLstOfArea.Contains(f) == false) continue;
                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                    if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;

                    var vol = para as Wj3005Rtu;

                    this.ChildTreeItems.Add(new TreeNodeItemTmlViewModel(this, para));
                }
            }

            if (NodeType == TypeOfTabTreeNode.IsArea)
            {
                var grp =
                    (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                     where t.Key.Item1 == AreaId
                     orderby t.Value.Index
                     select t.Value).ToList();



                this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(this, AreaId, 0,"全部终端",
                                                                                 TypeOfTabTreeNode.IsAll));


                foreach (var f in grp)
                {
                    var rtuList = new List<int>();
                    foreach (var fff in f.LstTml)
                    {
                        if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(fff))
                            continue;
                        if (fff < 1099999) rtuList.Add(fff);
                    }
                    if (rtuList.Count < 1) continue;
                    //if (f.LstTml.Count < 1) continue;
                    this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(this, f.AreaId, f.GroupId,f.GroupName,
                                                                                 TypeOfTabTreeNode.IsGrp));
                }


                var sp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
                var rtuLst = new List<int>();
                foreach (var f in sp)
                {
                    if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(f))
                        continue;
                    if (f < 1099999) rtuLst.Add(f);
                }


                if (rtuLst.Count > 0)

                    this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(this, AreaId, 0,"特殊终端",
                                                                                 TypeOfTabTreeNode.IsGrpSpecial));

            }

        }

        public override void ReUpdate(int updateArgu)
        {
            if (NodeType == TypeOfTabTreeNode.IsGrp) 
                this.UpdateGroup();
            if (NodeType == TypeOfTabTreeNode.IsAll) 
                this.UpdateAll();
            if (NodeType == TypeOfTabTreeNode.IsArea) 
                this.UpdateArea();
            if (NodeType == TypeOfTabTreeNode.IsGrpSpecial) 
                this.UpdateSpecial();
            foreach (var f in ChildTreeItems) f.GetChildRtuCount();
        }

        /// <summary>
        /// 当分组信息发生变化的时候  增量式重新加载节点  
        /// </summary>
        public  void UpdateGroup()
        {
            var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId, NodeId);
            if (info == null)
            {
               this.ChildTreeItems.Clear();
                if (_father != null) _father.ChildTreeItems.Remove(this);
                return;
            }

            this.NodeName = info.GroupName;

  
            //node delete
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (ChildTreeItems[i].NodeType != TypeOfTabTreeNode.IsTml)
                {
                    this.ChildTreeItems.RemoveAt(i);
                    continue;
                }

                if (info.LstTml.Contains(ChildTreeItems[i].NodeId) == false ||
                    !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(
                        ChildTreeItems[i].NodeId))
                {
                    this.ChildTreeItems.RemoveAt(i);
                }

            }


            //tml  add and update
            var exist = (from t in ChildTreeItems select t.NodeId).ToList();
            foreach (var t in info.LstTml)
            {
                if (exist.Contains(t)) continue;

                var para = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t);
                if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;

                if (para.RtuFid != 0) continue;


                var vol = para as Wj3005Rtu;

                if (vol != null && vol.WjVoltage.RtuUsedType == 2)
                {
                    ChildTreeItems.Add(new TreeNodeItemTmlViewModel(this, para));
                }
            }
            //按给定顺序排序
            var ntss = ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(info .LstTml );
            var idc = new Dictionary<int, TreeNodeBaseNode>();
            foreach (var f in this .ChildTreeItems )
            {
                if(!ntss .Contains( f.NodeId )) continue;
                int index = ntss.IndexOf(f.NodeId);
                if(idc .ContainsKey( index )==false ) idc.Add(index, f);
            }

            for (int i=0;i<ChildTreeItems .Count ;i++)
            {
                if(idc .ContainsKey( i)==false ) continue;
                if(ChildTreeItems .Count <i ) continue;
                if(ChildTreeItems [i].NodeId !=idc [i].NodeId )
                {
                    if(ChildTreeItems .Contains( idc [i])) ChildTreeItems.Remove(idc[i]);
                    ChildTreeItems.Insert(i, idc[i]);
                }
            }

        }


        /// <summary>
        /// 当分组信息发生变化的时候  增量式重新加载节点  
        /// </summary>
        public void UpdateSpecial()
        {
            var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
            if (info.Count  == 0)
            {
              this.ChildTreeItems.Clear();
                if (_father != null) _father.ChildTreeItems.Remove(this);
                return;
            }
             

            //node delete
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (ChildTreeItems[i].NodeType != TypeOfTabTreeNode.IsTml)
                {
                    this.ChildTreeItems.RemoveAt(i);
                    continue;
                }

                if (info.Contains(ChildTreeItems[i].NodeId) == false ||
                    !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(
                        ChildTreeItems[i].NodeId))
                {
                    this.ChildTreeItems.RemoveAt(i);
                }

            }


            //tml  add and update
            var exist = (from t in ChildTreeItems select t.NodeId).ToList();
            foreach (var t in info)
            {
                if (exist.Contains(t)) continue;

                var para = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t);
                if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;

                if (para.RtuFid != 0) continue;


                var vol = para as Wj3005Rtu;

                if (vol != null && vol.WjVoltage.RtuUsedType == 2)
                {
                    ChildTreeItems.Add(new TreeNodeItemTmlViewModel(this, para));
                }
            }

            //按给定顺序排序
            var ntss = ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(info);
            var idc = new Dictionary<int, TreeNodeBaseNode>();
            foreach (var f in this.ChildTreeItems)
            {
                if (!ntss.Contains(f.NodeId)) continue;
                int index = ntss.IndexOf(f.NodeId);
                if (idc.ContainsKey(index) == false) idc.Add(index, f);
            }

            for (int i = 0; i < ChildTreeItems.Count; i++)
            {
                if (idc.ContainsKey(i) == false) continue;
                if (ChildTreeItems.Count < i) continue;
                if (ChildTreeItems[i].NodeId != idc[i].NodeId)
                {
                    if (ChildTreeItems.Contains(idc[i])) ChildTreeItems.Remove(idc[i]);
                    ChildTreeItems.Insert(i, idc[i]);
                }
            }

        }


         public void UpdateAll()
        {
             if(this.NodeType  != TypeOfTabTreeNode.IsAll  )
             {
                this.ChildTreeItems.Clear();
                 if (_father != null) _father.ChildTreeItems.Remove(this);
                 return;
             }
            var info = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold .MySlef .GetRtuInArea(AreaId);
            if (info.Count  == 0)
            {
               this.ChildTreeItems.Clear();
                if (_father != null) _father.ChildTreeItems.Remove(this);
                return;
            }
             

            //node delete
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (ChildTreeItems[i].NodeType != TypeOfTabTreeNode.IsTml)
                {
                    this.ChildTreeItems.RemoveAt(i);
                    continue;
                }

                if (info.Contains(ChildTreeItems[i].NodeId) == false ||
                    !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(
                        ChildTreeItems[i].NodeId))
                {
                    this.ChildTreeItems.RemoveAt(i);
                }

            }


            //tml  add and update
            var exist = (from t in ChildTreeItems select t.NodeId).ToList();
            foreach (var t in info)
            {
                if (exist.Contains(t)) continue;

                var para = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t);
                if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;

                if (para.RtuFid != 0) continue;


                var vol = para as Wj3005Rtu;

                if (vol != null && vol.WjVoltage.RtuUsedType == 2)
                {
                    ChildTreeItems.Add(new TreeNodeItemTmlViewModel(this, para));
                }
            }
             ////按给定顺序排序
            //var ntss = ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(info);
            //var idc = new Dictionary<int, TreeNodeBaseNode>();
            //foreach (var f in this.ChildTreeItems)
            //{
            //    if (!ntss.Contains(f.NodeId)) continue;
            //    int index = ntss.IndexOf(f.NodeId);
            //    if (idc.ContainsKey(index) == false) idc.Add(index, f);
            //}

            //for (int i = 0; i < ChildTreeItems.Count; i++)
            //{
            //    if (idc.ContainsKey(i) == false) continue;
            //    if (ChildTreeItems.Count < i) continue;
            //    if (ChildTreeItems[i].NodeId != idc[i].NodeId)
            //    {
            //        if (ChildTreeItems.Contains(idc[i])) ChildTreeItems.Remove(idc[i]);
            //        ChildTreeItems.Insert(i, idc[i]);
            //    }
            //}

        }

        /// <summary>
        /// 当分组信息发生变化的时候  增量式重新加载节点  
        /// </summary>
         public void UpdateArea()
        {
            var info = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(this.AreaId);
                //.Values.ToList();
            if (info == null)
            {
               this.ChildTreeItems.Clear();
                if (_father != null) _father.ChildTreeItems.Remove(this);
                return;
            }

            this.NodeName = info.AreaName;

            var arealst = (from t in info.LstTml select t).ToList();

            var gprlst = (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                          where t.Key.Item1 == AreaId
                          select t.Key.Item2).ToList();
            //var spe = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
            //if(spe .Count >0) 
            //   gprlst.Add(0);


            //node delete
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (ChildTreeItems[i].NodeId == 0) continue;
                if (gprlst.Contains(ChildTreeItems[i].NodeId) == false) ChildTreeItems.RemoveAt(i);
                if (ChildTreeItems[i].NodeType != TypeOfTabTreeNode.IsGrpSpecial &&
                    ChildTreeItems[i].NodeType != TypeOfTabTreeNode.IsGrp &&
                    ChildTreeItems[i].NodeType != TypeOfTabTreeNode.IsAll)
                {
                    this.ChildTreeItems.RemoveAt(i);
                }
            }


            //tml  add and update
            var exist = (from t in ChildTreeItems select t.NodeId).ToList();
            var lstUp = new List<int>();
            foreach (var t in info.LstTml)
            {
                if (exist.Contains(t))
                {
                    lstUp.Add(t);
                    continue;
                }

                var para = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId, t);
                if (para == null) continue;

                if (para.LstTml.Count == 0) continue;
                ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(this, AreaId ,t,TypeOfTabTreeNode.IsGrp ));
            }

            foreach (var f in this.ChildTreeItems)
            {
                if (!lstUp.Contains(f.NodeId)) continue;
                f.ReUpdate(0);
            }

            TreeNodeBaseNode spe = null;
            var spelst = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
            foreach (var f in this.ChildTreeItems)
            {
                if (f.NodeType == TypeOfTabTreeNode.IsAll)
                {
                    f.ReUpdate(0);
                }
                if (f.NodeType == TypeOfTabTreeNode.IsGrpSpecial)
                {
                    if(spelst .Count ==0)
                    {
                        this.ChildTreeItems.Remove(f);
                        break;
                    }
                    f.ReUpdate(0);
                    spe = f;
                }
            }

             if (spe ==null && spelst .Count >0)
             {
                 this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(this, AreaId, 0,
                                                                              TypeOfTabTreeNode.IsGrpSpecial));
             }
           
        }


        #region Node Select

        /// <summary>
        /// 当选择的终端发送变化时，如果 
        /// </summary>
        public override void OnNodeSelectActive()
        {
            if (NodeType != TypeOfTabTreeNode.IsGrp && NodeType != TypeOfTabTreeNode.IsAll) return;
            if (NodeType == TypeOfTabTreeNode.IsGrp)
            {
                var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId,
                                                                                                              NodeId);
                if (info == null) return;
            }

            //base.OnNodeSelect();
                //发布事件  选中当前节点
                var args = new PublishEventArgs
                               {
                                   EventType = PublishEventType.Core,
                                   EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.GroupSelected,
                               };

                args.AddParams(new Wlst.Sr.EquipmentInfoHolding.Model.SelectedInfo(AreaId, NodeId,
                                                                                   SelectedInfo.SelectType.SingleGrp));

                EventPublish.PublishEvent(args);

                //  base.OnNodeSelectActive();
                TreeNodeItemSingleGroupViewModel.CurrentSelectGroupNode = this;

                // ResetContextMenu();

        }


        #region  Reset ContextMenu
        public override  void ResetContextMenu()
        {
            ResetCm();
        }

        public void ResetCm()
        {
            ObservableCollection<IIMenuItem> t = null;

            if (NodeType != TypeOfTabTreeNode.IsGrp) return;


            var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId, NodeId);
            if (info == null) return;
            this.CmItems = MenuBuilding.BulidCm("RightMenuSingle", false, info);
        }

        #endregion

        #endregion


        private int _regionId;
        /// <summary>
        /// 地区ID    
        /// </summary>
        public int RegionId
        {
            get { return _regionId; }
            set
            {
                if (_regionId != value)
                {
                    _regionId = value;
                    this.RaisePropertyChanged(() => this.RegionId);
                }
            }
        }

    }
}
