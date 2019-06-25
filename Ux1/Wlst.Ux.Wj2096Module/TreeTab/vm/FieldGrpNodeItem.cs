using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Services;
using Wlst.client;

namespace Wlst.Ux.Wj2096Module.TreeTab.vm
{

    public class FieldGrpNodeItem : NodeItemBase
    {

        //private ObservableCollection<CtrlNodeItem> _childTreeItemsInfo = null;

        //public ObservableCollection<CtrlNodeItem> ChildItems
        //{
        //    get
        //    {
        //        if (_childTreeItemsInfo == null)
        //            _childTreeItemsInfo = new ObservableCollection<CtrlNodeItem>();

        //        return _childTreeItemsInfo;
        //    }
        //    //set
        //    //{
        //    //    if (value == _childTreeItemsInfo) return;
        //    //    _childTreeItemsInfo = value;
        //    //    this.RaisePropertyChanged(() => this.ChildItems);
        //    //}
        //}




        public override void OnNodeSelect()
        {
            CmItems = MenuBuilding.BulidCm("20961", false, FileId);
            base.OnNodeSelect();

            //var args = new PublishEventArgs
            //{
            //    EventType = PublishEventType.Core,
            //    EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
            //};

            ////var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(NodeId);

            //args.AddParams(NodeId);

            //EventPublish.PublishEvent(args);
        }


        //归属区域
        //public int AreaId = -1;
        public int FileId = -1;

        public FieldGrpNodeItem(int fieldid, int grpid)
        {
            NodeTypeLevel = 4;
            this.ImagesIcon = Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("RtuGroupIcon");

            this.FileId = fieldid;
            this.NodeId = grpid;

            if (this.NodeId != 0)
            {
                var para = Wlst.Sr.SlusglInfoHold.Services.SluSglFieldGrpHold.MySlef.Get(fieldid, grpid);

                if (para == null)
                {
                    this.NodeShowId = grpid + "";
                    this.NodeName = "-未知组";
                    return;
                }

                this.NodeShowId = grpid + "";
                this.NodeName = "-" + para.GrpName;
                this.AddChild(para);
                this.NodeName += "-[" + this.ChildItems.Count + "]";
            }
            else
            {
                var speciallist = Wlst.Sr.SlusglInfoHold.Services.SluSglFieldGrpHold.MySlef.GetSpecial(fieldid);

                this.NodeShowId = 0 + "";
                this.NodeName = "-未分组控制器";
                this.AddSpecialChild(speciallist);
                this.NodeName += "-[" + this.ChildItems.Count + "]";
            }
        }

        public override  void UpdateShowInfo()
        {
            if (this.NodeId != 0)
            {
                var para = Wlst.Sr.SlusglInfoHold.Services.SluSglFieldGrpHold.MySlef.Get(FileId, NodeId);

                if (para == null)
                {
                    this.NodeShowId = NodeId + "";
                    this.NodeName = "-未知组";
                    return;
                }

                this.NodeShowId = NodeId + "";
                this.NodeName = para.GrpName;
                this.AddChild(para);
                this.NodeName += "-[" + this.ChildItems.Count + "]";
            }
            else
            {
                var speciallist = Wlst.Sr.SlusglInfoHold.Services.SluSglFieldGrpHold.MySlef.GetSpecial(FileId);

                this.NodeShowId = 0 + "";
                this.NodeName = "-未分组控制器";
                this.AddSpecialChild(speciallist);
                this.NodeName += "-[" + this.ChildItems.Count + "]";
            }
        }

       

        private void AddChild(GrpFieldSluSglCtrl.GrpFieldSluSglItem para)
        {
            var para1 = new List<EquSluSgl.ParaSluCtrl>();

            foreach (var f in para.CtrlLst)
            {
                var para2 = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(para.FieldId, f);
                para1.Add(para2);
            }

            para1 = (from t in para1 orderby t.OrderId select t).ToList();

            foreach (var f in para1)
            {
                this.ChildItems.Add(new CtrlNodeItem(FileId, f.CtrlId));
            }
        }


        private void AddSpecialChild(List<int> para)
        {

            var para1 = new List<EquSluSgl.ParaSluCtrl>();

            foreach (var f in para)
            {
                var para2 = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(FileId, f);
                para1.Add(para2);
            }

            para1 = (from t in para1 orderby t.OrderId select t).ToList();

            foreach (var f in para1)
            {
                this.ChildItems.Add(new CtrlNodeItem(FileId, f.CtrlId));
            }


            //foreach (var f in para)
            //{
            //    //if (Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Info.ContainsKey(f))
            //        this.ChildItems.Add(new CtrlNodeItem(FileId, f));
            //}
        }
        /// <summary>
        /// 更新组下所有设备的参数 位置  增删改等
        /// </summary>
        private  void UpdateChildPara()
        {
            var para = Wlst.Sr.SlusglInfoHold.Services.SluSglFieldGrpHold.MySlef.Get(FileId, NodeId);
            if (para == null)
            {
                this.ChildItems.Clear();
                return;
            }

            //delete
            var dlt = (from t in ChildItems where para.CtrlLst.Contains(t.NodeId) == false select t).ToList();
            foreach (var f in dlt) if (ChildItems.Contains(f)) ChildItems.Remove(f);

            var dic = new List<int>();
            //update setinfo
            foreach (var f in ChildItems)
            {
                f.UpdateShowInfo();
                dic.Add(f.NodeId);
            }

            //newadd
            foreach (var f in para.CtrlLst)
            {
                if (dic.Contains(f)) continue;
                ChildItems.Add(  new CtrlNodeItem(FileId,f));
            }
            this.Sort(para.CtrlLst);

            ////add  and  sort
            //for (int i = 0; i < para.CtrlLst.Count; i++)
            //{
            //    int cur = para.CtrlLst[i];
            //    if (ChildItems.Count < i + 1) ChildItems.Add(dic[cur]);
            //    else if (ChildItems[i].NodeId != cur) ChildItems.Insert(i, dic[cur]);
            //}

        }

        /// <summary>
        /// 更新指定设备的显示参数信息
        /// </summary>
        /// <param name="ctrlids"></param>
        public override void UpdateChildPara(List<int> ctrlids)
        {
            foreach (var f in ChildItems)
            {
                if (ctrlids.Contains(f.NodeId))
                    f.UpdateShowInfo();
            }
        }


        public override void UpdateChildImage(List<int> ctrlids)
        {
            var ntg = (from t in ChildItems where ctrlids.Contains(t.NodeId) select t).ToList();
            foreach (var f in ntg )
            {
                int ctrlid = f.NodeId;
                //
            }
        }

    }
}
