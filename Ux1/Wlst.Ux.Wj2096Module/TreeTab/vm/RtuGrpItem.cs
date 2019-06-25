using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Sr.Menu.Services;

namespace Wlst.Ux.Wj2096Module.TreeTab.vm
{
    public class RtuGrpItem : NodeItemBase
    {
        private int NodeType = -1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="groupId"></param>
        /// <param name="type"> 2、分组，3、全部设备，4、特殊设备</param>
        public RtuGrpItem(int areaId, int groupId, int type)
        {
            NodeTypeLevel = 2;
            this.AreaId = areaId;
            NodeType = type;
            this.ImagesIcon = Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("RtuGroupIcon");


            var nodename = "--";
            if (type == 4)
            {
                nodename = "特殊设备";
                this.NodeId = 99999;
            }
            if (type == 3)
            {
                nodename = "全部设备";
                this.NodeId = 0;
            }

            if (type == 2)
            {
                var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId,
                                                                                                              groupId);
                if (info != null)
                {
                    nodename = info.GroupName;
                }
                this.NodeShowId = groupId.ToString("d2") + "";
            }

            this.NodeName = nodename;
            this.AddChild();

        }

        public override void OnNodeSelect()
        {
            CmItems = MenuBuilding.BulidCm("20966", false,
                                           new Tuple<string, List<int>>(NodeName,
                                                                        (from t in ChildItems select t.NodeId).ToList()));
            base.OnNodeSelect();
        }

        /// <summary>
        /// 加载节点，第一次使用
        /// </summary>
        private void AddChild()
        {
            ChildItems.Clear();

            if (NodeType == 2) //grp
            {
                var tmlLstOfArea =
                    (from t in Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Info where t.Value.AreaId == AreaId  orderby t.Value.PhyId
                     select t.Key).ToList();
                var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId,
                                                                                                             NodeId);
                if (grp == null) return;
                var gprs = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp.LstTml);
                foreach (var f in gprs)
                {
                    if (tmlLstOfArea.Contains(f) == false) continue;
                    this.ChildItems.Add(new FieldNodeItem(f));
                }
            }

            if (NodeType == 3) //all
            {
                var grp = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(AreaId);
                var tmlLstOfArea = (from t in Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Info
                                    where t.Value.AreaId == AreaId 
                                    orderby t.Value.PhyId
                                   select t.Key).ToList();
                if (grp == null) return;
                var allrtus = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp.LstTml,
                                                                                                            AreaId, true);

                foreach (var f in allrtus.Item1)
                {
                    if (tmlLstOfArea.Contains(f) == false) continue;
                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                    this.ChildItems.Add(new FieldNodeItem(f));
                    tmlLstOfArea.Remove(f);
                }
                var sortLst =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(allrtus.Item2);
                foreach (var f in sortLst)
                {
                    if (tmlLstOfArea.Contains(f) == false) continue;
                    this.ChildItems.Add(new FieldNodeItem(f));
                    tmlLstOfArea.Remove(f);
                }
                foreach (var f in tmlLstOfArea) this.ChildItems.Add(new FieldNodeItem(f)); //todo xxxxxxxxxxxxxxxxxx  此代码应删除  当前测试使用
            }
            if (NodeType == 4) //spe
            {
                var tmlLstOfArea = (from t in Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Info
                                    where t.Value.AreaId == AreaId 
                                select t.Key).ToList();
                var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
                if (grp.Count == 0) return;
                var gprs = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp);
                foreach (var f in tmlLstOfArea) if (gprs.Contains(f) == false) gprs.Add(f);
                foreach (var f in gprs)
                {
                    if (tmlLstOfArea.Contains(f) == false) continue;
                    this.ChildItems.Add(new FieldNodeItem(f));
                }
            }


            //for (int i = ChildItems.Count - 1; i >= 0; i--)
            //{
            //    if (ChildItems[i].ChildItems.Count == 0) ChildItems.RemoveAt(i);
            //}
        }


        public override void UpdateChildImage(List<int> ctrlids)
        {
            foreach (var f in ChildItems) f.UpdateChildImage(ctrlids);
            //base.UpdateChildImage(ctrlids);
        }

        public override void UpdateChildPara(List<int> ctrlids)
        {
            foreach (var f in ChildItems) f.UpdateChildPara(ctrlids);
            //base.UpdateChildPara(ctrlids);
        }


        public override void UpdateShowInfo()
        {
            //base.UpdateShowInfo();
            if (NodeType == 2)
            {
                var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId,
                                                                                                              NodeId);
                if (info != null)
                {
                    NodeName = info.GroupName;
                }
            }

            this.UpdateChildPara();

        }

        //1、区域，2、分组，3、全部设备，4、特殊设备
        private void UpdateChildPara()
        {
            var lstneworder = new List<int>();

            if (NodeType == 2) //grp
            {
                var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId,
                                                                                                             NodeId);
                if (grp != null)
                {
                    lstneworder =
                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp.LstTml);
                }
            }

            if (NodeType == 3) //all
            {
                var grp = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(AreaId);
                if (grp != null)
                {
                    var gprs =
                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp.LstTml,
                                                                                                         AreaId, true);

                    foreach (var f in gprs.Item1)
                    {
                        lstneworder.AddRange(gprs.Item1);
                    }
                    lstneworder.AddRange(
                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(gprs.Item2));
                }
            }
            if (NodeType == 4) //spe
            {

                var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
                if (grp.Count == 0) return;
                lstneworder.AddRange(
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp));
            }
            if (NodeType >= 2 && NodeType <= 4)
            {
                var tmlLst =
                    (from t in Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Info
                     where t.Value.AreaId == AreaId && lstneworder.Contains(t.Key)
                     select t.Key).ToList();


                //delete
                var dlt = (from t in ChildItems where tmlLst.Contains(t.NodeId) == false select t).ToList();
                foreach (var f in dlt) if (ChildItems.Contains(f)) ChildItems.Remove(f);

                var dic = new List<int>();
                //update setinfo
                foreach (var f in ChildItems)
                {
                    f.UpdateShowInfo();
                    dic.Add(f.NodeId);
                }

                //newadd
                foreach (var f in tmlLst)
                {
                    if (dic.Contains(f)) continue;
                    ChildItems.Add(new FieldNodeItem(f));
                }
                this.Sort(tmlLst);


                ////add  and  sort
                //for (int i = 0; i < tmlLst.Count; i++)
                //{
                //    int cur = tmlLst[i];
                //    if (ChildItems.Count < i + 1) ChildItems.Add(dic[cur]);
                //    else if (ChildItems[i].NodeId != cur) ChildItems.Insert(i, dic[cur]);
                //}
            }

            for (int i = ChildItems.Count - 1; i >= 0; i--)
            {
                if (ChildItems[i].ChildItems.Count == 0) ChildItems.RemoveAt(i);
            }
        }




        //#region Node Select

        ///// <summary>
        ///// 当选择的终端发送变化时，如果 
        ///// </summary>
        //public override void OnNodeSelectActive()
        //{
        //    //if (NodeType != TypeOfTabTreeNode.IsGrp && NodeType != TypeOfTabTreeNode.IsAll &&
        //    //    NodeType != TypeOfTabTreeNode.IsGrpSpecial) return;
        //    //if (NodeType == TypeOfTabTreeNode.IsGrp)
        //    //{
        //    //    var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId,
        //    //                                                                                                  NodeId);
        //    //    if (info == null) return;
        //    //}

        //    ////base.OnNodeSelect();
        //    ////发布事件  选中当前节点
        //    //var args = new PublishEventArgs
        //    //               {
        //    //                   EventType = PublishEventType.Core,
        //    //                   EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.GroupSelected,
        //    //               };

        //    //args.AddParams(new Wlst.Sr.EquipmentInfoHolding.Model.SelectedInfo(AreaId, NodeId,
        //    //                                                                   SelectedInfo.SelectType.SingleGrp));

        //    //EventPublish.PublishEvent(args);



        //}



        //public override void ResetContextMenu()
        //{
        //    ResetCm();
        //}

        //public void ResetCm()
        //{
        //    ObservableCollection<IIMenuItem> t = null;

        //    if (NodeType != TypeOfTabTreeNode.IsGrp) return;


        //    var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId, NodeId);
        //    if (info == null) return;
        //    this.CmItems = MenuBuilding.BulidCm("RightMenuSingle", false, info);
        //}




        //public override void OnNodeChecked()
        //{
        //    //if (NodeType != TypeOfTabTreeNode.IsGrp && NodeType != TypeOfTabTreeNode.IsAll && NodeType != TypeOfTabTreeNode.IsGrpSpecial) return;

        //    GrpSingleTabShowViewModel.ViewModels.TreeSingleViewModel.MySelf.OnNodeChecked(AreaId, this.NodeId,
        //                                                                                  this, IsChecked);
        //    base.OnNodeChecked();

        //}



        //#endregion


    }
}
