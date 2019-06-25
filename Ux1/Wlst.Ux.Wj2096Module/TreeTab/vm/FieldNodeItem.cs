using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Services;

namespace Wlst.Ux.Wj2096Module.TreeTab.vm
{

    public class FieldNodeItem : NodeItemBase
    {

        //private ObservableCollection<FieldGrpNodeItem> _childTreeItemsInfo = null;

        //public ObservableCollection<FieldGrpNodeItem> ChildItems
        //{
        //    get
        //    {
        //        if (_childTreeItemsInfo == null)
        //            _childTreeItemsInfo = new ObservableCollection<FieldGrpNodeItem>();

        //        return _childTreeItemsInfo;
        //    }
        //    //set
        //    //{
        //    //    if (value == _childTreeItemsInfo) return;
        //    //    _childTreeItemsInfo = value;
        //    //    this.RaisePropertyChanged(() => this.ChildItems);
        //    //}
        //}


        /// <summary>
        /// sluid*10000+ ctrlid  控制器的快速索引
        /// </summary>
        public static ConcurrentDictionary<long, List<WeakReference>> ConnItems = new ConcurrentDictionary<long, List<WeakReference>>();

        public override void OnNodeSelect()
        {
           // var tu = new Tuple<int, int,int>(NodeId, 0,0);
            CmItems = MenuBuilding.BulidCm("20960", false, NodeId);
            base.OnNodeSelect();

            var args = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
            };

          //  var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(NodeId);

          //  args.AddParams(para);
            args.AddParams(NodeId);

            EventPublish.PublishEvent(args);
        }

        ////归属区域
        //public int AreaId = -1;

        public FieldNodeItem(int fieldid)
        {
            NodeTypeLevel = 3;
            this.ImagesIcon = Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage("RtuGroupIcon");

            this.NodeId = fieldid;

            var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(fieldid);
            if (para == null)
            {
                this.NodeShowId = fieldid + "";
                this.NodeName = "未知域";
                return;
            }
            AreaId = para.AreaId;
            this.NodeShowId = para.PhyId + "";
            this.NodeName = para.FieldName;
            this.AddChild();
            this.NodeName += "-[" + GetCtrlsCont() + "]";
        }

        /// <summary>
        /// 当参数更新时 或组调整时候  需要调用
        /// </summary>
        public override void UpdateShowInfo()
        {

            var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(NodeId);
            if (para == null)
            {
                this.NodeShowId = NodeId + "";
                this.NodeName = "-未知域";
                return;
            }
            AreaId = para.AreaId;
            this.NodeShowId = para.PhyId.ToString("d4") + "";
            this.NodeName = "-" + para.FieldName;
            this .UpdateChildPara();
            this.NodeName += "-[" + GetCtrlsCont() + "]";
        }

        int GetCtrlsCont()
        {
            var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(NodeId);
            return para.CtrlLst.Count;
        }
        private void AddChild()
        {
            var child = Wlst.Sr.SlusglInfoHold.Services.SluSglFieldGrpHold.MySlef.Get(NodeId);

            foreach (var f in child)
            {
                if (f.CtrlLst.Count == 0) continue;
                this.ChildItems.Add(new FieldGrpNodeItem(NodeId, f.GrpId));
            }

            var special = Wlst.Sr.SlusglInfoHold.Services.SluSglFieldGrpHold.MySlef.GetSpecial(NodeId);
            if (special.Count>0)
            {
                this.ChildItems.Add(new FieldGrpNodeItem(NodeId, 0));
            }

            //for (int i = ChildItems.Count - 1; i >= 0; i--)
            //{
            //    if (ChildItems[i].ChildItems.Count == 0) ChildItems.RemoveAt(i);
            //}
        }

        /// <summary>
        /// 更新组下所有设备的参数 位置  增删改等
        /// </summary>
        private  void UpdateChildPara()
        {
            var child = Wlst.Sr.SlusglInfoHold.Services.SluSglFieldGrpHold.MySlef.Get(NodeId);
            if (child == null)
            {
                this.ChildItems.Clear();
                return;
            }
            var lst = (from t in child where t.CtrlLst.Count > 0 select t.GrpId).ToList();

            //delete
            var dlt = (from t in ChildItems where lst.Contains(t.NodeId) == false select t).ToList();
            foreach (var f in dlt) if (ChildItems.Contains(f)) ChildItems.Remove(f);

            var dic = new List<int>();
            //update setinfo
            foreach (var f in ChildItems)
            {
                f.UpdateShowInfo();
                dic.Add(f.NodeId);
            }

            //newadd
            foreach (var f in lst)
            {
                if (dic.Contains(f)) continue;
                ChildItems.Add(  new FieldGrpNodeItem(NodeId, f));
            }
            this.Sort(lst);

            ////add  and  sort
            //for (int i = 0; i < lst.Count; i++)
            //{
            //    int cur = lst[i];
            //    if (ChildItems.Count < i + 1) ChildItems.Add(dic[cur]);
            //    else if (ChildItems[i].NodeId != cur) ChildItems.Insert(i, dic[cur]);
            //}

            //for (int i = ChildItems.Count - 1; i >= 0; i--)
            //{
            //    if (ChildItems[i].ChildItems.Count == 0) ChildItems.RemoveAt(i);
            //}

        }

        /// <summary>
        /// 更新指定设备的显示参数信息
        /// </summary>
        /// <param name="ctrlids"></param>
        public override void UpdateChildPara(List<int> ctrlids)
        {
            if(ctrlids .Contains( NodeId ))
            {
                UpdateChildPara();
                return;
            }
            foreach (var f in ChildItems)
            {
                f.UpdateChildPara(ctrlids);
            }
        }


        public override void UpdateChildImage(List<int> ctrlids)
        {
            foreach (var f in ChildItems)
            {
                f.UpdateChildImage(ctrlids);
            }
        }

    }
}
