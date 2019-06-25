﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;


using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.Wj1050Module.Resources;
using Wlst.Ux.Wj1050Module.Wj1050ManageSettingViewModel.ViewModel;
using Wlst.client;


namespace Wlst.Ux.Wj1050Module.Wj1050ManageViewModel.ViewModel
{
    public class TreeNodeAreaViewModel : TreeNodeBaseNode
    {
        private static TreeNodeBaseNode _currentSelectGroupNode;

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
                    if (value.NodeType == TypeOfTabTreeNode.IsAll || value.NodeType == TypeOfTabTreeNode.IsArea)
                    {

                        info.Add(-1);
                        ins.AddParams(info);
                    }
                    else
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

        public TreeNodeAreaViewModel(TreeNodeBaseNode mvvmFather, int areaId, int groupId, TypeOfTabTreeNode type)
        {

            this.AreaId = areaId;
            this.NodeType = type;
            //Visi = Visibility.Visible;
            this._father = mvvmFather;
            //TreeSingleViewModel.RegisterNodeToControl(this);

            var nodename = "--";
            if (type == TypeOfTabTreeNode.IsGrpSpecial)
            {
                nodename = "特殊分组";
                this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
                this.NodeId = 0;

            }
            if (type == TypeOfTabTreeNode.IsAll)
            {
                nodename = "全部设备";
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
                        nodename = string.Format("{0:D2}", f.Value.AreaId) + "-" + f.Value.AreaName;
                    }
                }
            }
            if (type == TypeOfTabTreeNode.IsGrp)
            {
                var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId,
                                                                                                              groupId);
                if (info != null)
                {
                    if (!Wj1050TreeSetLoad.Myself.IsShowGrpInTreeModelShowId)
                    {
                        nodename = info.GroupName;
                    }
                    else
                    {
                        nodename = string.Format("{0:D2}", info.GroupId) + "-" + info.GroupName;
                    }
                   //this.NodeName = info.GroupName;

                }

                this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
                this.NodeId = groupId;
            }

            this.NodeName = nodename;
            // this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
            this.AddChild();

        }
        public bool HaveChild(TypeOfTabTreeNode type,int v)  //need modi
        {
            //var sp =new List<int>();
            //if (type == TypeOfTabTreeNode.IsGrpSpecial) {  sp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId); }
            //if (type == TypeOfTabTreeNode.IsGrp) { sp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId); }

            //if (sp == null) return false;
            
            var rtuLst = new List<int>();
            //foreach (var v in sp)
            //{
                var rtu = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(v);
                if (rtu == null) return false ;
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
                }

            return rtuLst.Count > 0;
        }

        public override void AddChild()
        {
            ChildTreeItems.Clear();
            if (NodeType == TypeOfTabTreeNode.IsGrp)
            {
                var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId,
                                                                                                             NodeId);
                if (grp == null) return;
                var gprs = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp.LstTml);
                //排序
                foreach (var f in gprs)
                {
                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);

                    if (para == null) continue; //|| para.EquipmentType != WjParaBase.EquType.Mru


                    if (para.EquipmentType == WjParaBase.EquType.Mru && para.RtuFid == 0) //是电表，而且是主设备
                    {
                        this.ChildTreeItems.Add(new TreeNodeWj1050ViewModel(para.RtuId, para.RtuName, para.RtuFid));
                    }
                    else if (para.EquipmentType == WjParaBase.EquType.Rtu && para.EquipmentsThatAttachToThisRtu.Count > 0)
                    {
                        foreach (var g in para.EquipmentsThatAttachToThisRtu)
                        {
                            var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                            if (pa == null) continue;
                            if (pa.EquipmentType == WjParaBase.EquType.Mru && pa.RtuFid > 0)
                            {
                                this.ChildTreeItems.Add(new TreeNodeWj1050ViewModel(pa.RtuId, pa.RtuName, pa.RtuFid));
                            }
                        }
                    }
                }
            }
            if (NodeType == TypeOfTabTreeNode.IsAll)
            {
                var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                foreach (var f in lstInArea)
                {
                    var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                    if (pb == null) continue;
                    if (pb.EquipmentType == WjParaBase.EquType.Mru && pb.RtuFid == 0) //电表为主设备
                    {
                        this.ChildTreeItems.Add(new TreeNodeWj1050ViewModel(pb.RtuId, pb.RtuName, pb.RtuFid));
                    }
                    else if (pb.EquipmentType == WjParaBase.EquType.Rtu && pb.EquipmentsThatAttachToThisRtu.Count > 0)
                    //haha 特殊终端下有电表
                    {

                        foreach (var g in pb.EquipmentsThatAttachToThisRtu)
                        {
                            var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                            if (pa == null) continue;
                            if (pa.EquipmentType == WjParaBase.EquType.Mru && pa.RtuFid > 0)
                            {
                                this.ChildTreeItems.Add(new TreeNodeWj1050ViewModel(pa.RtuId, pa.RtuName,
                                                                                    pa.RtuFid));
                            }
                        }
                    }
                }
            
            }
            if (NodeType == TypeOfTabTreeNode.IsGrpSpecial)
            {
                var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
                if (grp.Count == 0) return;
                var gprs = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp);
                foreach (var f in gprs)
                {
                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                    if (para == null) continue; //|| para.EquipmentType != WjParaBase.EquType.Mru
                    if (para.EquipmentType == WjParaBase.EquType.Mru && para.RtuFid == 0) //电表为主设备
                    {
                        this.ChildTreeItems.Add(new TreeNodeWj1050ViewModel(para.RtuId, para.RtuName, para.RtuFid));
                    }
                    else if (para.EquipmentType == WjParaBase.EquType.Rtu && para.EquipmentsThatAttachToThisRtu.Count > 0)
                        //haha 特殊终端下有电表
                    {
                        foreach (var g in para.EquipmentsThatAttachToThisRtu)
                        {
                            var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                            if (pa == null) continue;
                            if (pa.EquipmentType == WjParaBase.EquType.Mru && pa.RtuFid > 0)
                            {
                                this.ChildTreeItems.Add(new TreeNodeWj1050ViewModel(pa.RtuId, pa.RtuName, pa.RtuFid));
                            }
                        }
                    }
                }
            }
            if (NodeType == TypeOfTabTreeNode.IsArea)
            {
                if (Wj1050TreeSetLoad.Myself.IsShowGrp)
                {
                    var grp =
                        (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                         where t.Key.Item1 == AreaId
                         orderby t.Value.Index
                         select t.Value).ToList();

                    this.ChildTreeItems.Add(new TreeNodeAreaViewModel(this, AreaId, 0, TypeOfTabTreeNode.IsAll));
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
                        this.ChildTreeItems.Add(new TreeNodeAreaViewModel(this, f.AreaId, f.GroupId,
                                                                          TypeOfTabTreeNode.IsGrp));
                    }
                    var sp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
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
                        }
                        else if (rtu.EquipmentType == WjParaBase.EquType.Mru && rtu.RtuFid == 0)
                        {
                            rtuLst.Add(rtu.RtuId);
                        }
                            
                    }
                    if (rtuLst.Count > 0)
                        this.ChildTreeItems.Add(new TreeNodeAreaViewModel(this, AreaId, 0,
                                                                          TypeOfTabTreeNode.IsGrpSpecial));
                }
                else
                {
                    this.ChildTreeItems.Add(new TreeNodeAreaViewModel(this, AreaId, 0, TypeOfTabTreeNode.IsAll));

                    //var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                    //foreach (var f in lstInArea)
                    //{
                    //    var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                    //    if (pb == null) continue;
                    //    if (pb.EquipmentType == WjParaBase.EquType.Mru && pb.RtuFid == 0) //电表为主设备
                    //    {
                    //        this.ChildTreeItems.Add(new TreeNodeWj1050ViewModel(pb.RtuId, pb.RtuName, pb.RtuFid));
                    //    }
                    //    else if (pb.EquipmentType == WjParaBase.EquType.Rtu && pb.EquipmentsThatAttachToThisRtu.Count > 0)
                    //        //haha 特殊终端下有电表
                    //    {

                    //        foreach (var g in pb.EquipmentsThatAttachToThisRtu)
                    //        {
                    //            var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                    //            if (pa == null) continue;
                    //            if (pa.EquipmentType == WjParaBase.EquType.Mru && pa.RtuFid > 0)
                    //            {
                    //                this.ChildTreeItems.Add(new TreeNodeWj1050ViewModel(pa.RtuId, pa.RtuName,
                    //                                                                    pa.RtuFid));
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
        }
    }
}