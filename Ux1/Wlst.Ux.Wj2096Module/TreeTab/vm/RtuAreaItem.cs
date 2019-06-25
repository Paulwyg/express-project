using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Sr.Menu.Services;

namespace Wlst.Ux.Wj2096Module.TreeTab.vm
{

    public class RtuAreaItem : NodeItemBase
    {
        /// <summary>
        /// 0 全部终端，99999 特殊分组
        /// </summary>
        /// <returns></returns>
        public Dictionary< int ,List< int >> GetBelongGrpFiled()
        {
            var rtn = new Dictionary<int, List<int>>();
            //组
            foreach (var f in ChildItems)
            {
                if (f.ChildItems.Count == 0) continue;
                if(rtn .ContainsKey( f.NodeId )==false )rtn .Add( f.NodeId ,(from t in f.ChildItems select f.NodeId).ToList());
            
            }
            return rtn;
        }

        public override void OnNodeSelect()
        {
            //CmItems = MenuBuilding.BulidCm("20965", false,
            //                               new Tuple<string, List<int>>(NodeName,
            //                                                            (from t in ChildItems select t.NodeId).ToList()));
            base.OnNodeSelect();
        }

        public RtuAreaItem(int areaId)
        {
            NodeTypeLevel = 1;
            this.AreaId = areaId;

            this.ImagesIcon = Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("RtuGroupIcon");


            var nodename = "--";

            this.NodeShowId = areaId.ToString("d2");
            var areaInfo = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef;
            foreach (var f in areaInfo.AreaInfo)
            {
                if (f.Value.AreaId == areaId)
                {
                    nodename = f.Value.AreaName;
                    break;
                }
            }


            this.NodeName = nodename;
            this.AddChild();

        }

        /// <summary>
        /// 加载节点，第一次使用
        /// </summary>
        private void AddChild()
        {
            ChildItems.Clear();

            var grp =
                (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                 where t.Key.Item1 == AreaId
                 orderby t.Value.Index
                 select t.Value).ToList();

            this.ChildItems.Add(new RtuGrpItem(AreaId, 0, 3));
            foreach (var f in grp)
            {
                this.ChildItems.Add(new RtuGrpItem(AreaId, f.GroupId, 2));
            }
            //this.ChildItems.Add(new RtuGrpItem(AreaId, 0, 4));


            for (int i = ChildItems.Count - 1; i >= 0; i--)
            {
                if (ChildItems[i].ChildItems.Count == 0) ChildItems.RemoveAt(i);
            }
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

            var info = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(this.AreaId);
            if (info != null)
            {
                NodeName = info.AreaName;
            }

            this.UpdateChildPara();

        }


        private void UpdateChildPara()
        {

            var allgrp = this.ChildItems[0];
            allgrp.SortIndex = 0;
            //allgrp.UpdateShowInfo();

            var spegrp = this.ChildItems[ChildItems.Count - 1];
            spegrp.SortIndex = 99999;

            //spegrp.UpdateShowInfo();



            var grpLst =
                (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                 where t.Key.Item1 == AreaId
                 orderby t.Value.Index
                 select t.Value.GroupId).ToList();

            //var tmlLst =
            //       (from t in Wlst.Sr.SlusglInfoHold.Services.SluSglFieldHold.MySlef.Info
            //        where t.Value.AreaId == AreaId && lstneworder.Contains(t.Key)
            //        select t.Key).ToList();


            //delete
            var dlt = (from t in ChildItems where t.NodeId != 0 && grpLst.Contains(t.NodeId) == false select t).ToList();
            foreach (var f in dlt) if (ChildItems.Contains(f)) ChildItems.Remove(f);


            //update setinfo
            foreach (var f in ChildItems) f.UpdateShowInfo();



            //newadd
            var ex = (from t in ChildItems select t.NodeId).ToList();
            var newadd = (from t in grpLst where ex.Contains(t) == false select t).ToList();
            foreach (var f in newadd) ChildItems.Add(new RtuGrpItem(AreaId, f, 2));

            grpLst.Insert(0, 0);
            grpLst.Add(99999);
            this.Sort(grpLst);


            ////sort
            //var dic = new Dictionary<int, int>();
            //for (int i = 0; i <= grpLst.Count; i++)
            //{
            //    if (dic.ContainsKey(grpLst[i]) == false) dic.Add(grpLst[i], i + 3);
            //}
            //foreach (var f in ChildItems)
            //{
            //    if (dic.ContainsKey(f.NodeId)) f.SortIndex = dic[f.NodeId];
            //}

            //this.ChildItems = (from t in ChildItems orderby t.SortIndex ascending select t).ToList();

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
