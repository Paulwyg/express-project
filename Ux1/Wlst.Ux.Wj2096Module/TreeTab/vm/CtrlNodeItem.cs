using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.Wj2090Module.Services;

namespace Wlst.Ux.Wj2096Module.TreeTab.vm
{
    public class CtrlNodeItem : NodeItemBase
    {
        private int _imageCode = -1;

        public void UpdateImage(int codeid)
        {
            //20905 开灯有故障
            //20903 关灯有故障
            //20904 开灯无故障
            //20902 关灯无故障
            //20901 掉线
            //是否添加  20900 未知状态？？？
            if (codeid == _imageCode) return;
            _imageCode = codeid;
            this.ImagesIcon = Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage(codeid);
        }

        public override void OnNodeSelect()
        {
            var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(NodeId);
            var tu = new Tuple<int, int>(para, NodeId);
            CmItems = MenuBuilding.BulidCm("20962", false, tu);
            base.OnNodeSelect();

            var args = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
            };

            //var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(NodeId);
            
            args.AddParams(para);
            args.AddParams(NodeId);

            EventPublish.PublishEvent(args);
        }

        //public override void ResetCm()
        //{
        //    CmItems = MenuBuilding.BulidCm("2096", false, NodeId);
        //    base.ResetCm();
        //}

        public CtrlNodeItem(int fieldid, int ctrlid)
        {
            NodeTypeLevel = 5;
            this.NodeId = ctrlid;
            //UpdateImage(20902);

            var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(fieldid, ctrlid);
            if (para == null)
            {
                this.NodeShowId = ctrlid + "";
                this.NodeName = "-未知设备";
                return;
            }
            this.NodeShowId = para.OrderId + "";
            this.NodeName = "-" + para.CtrlName;

          

            int imagecode = Wlst.Sr.SlusglInfoHold.Services.SluSglCtrlDataHold.GetCtrlImageCode(ctrlid);
            var imagesIcon = Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage(imagecode);

            ImagesIcon = imagesIcon;//para.LightCount > 1 ? ImageResources.CtrlIconTwo : ImageResources.CtrlIconOne;
            //单灯控制器的快速 索引


            long ctrlkeyid = ctrlid;//fieldid*10000L +
            if (FieldNodeItem.ConnItems.ContainsKey(ctrlkeyid) == false)
                FieldNodeItem.ConnItems.TryAdd(ctrlkeyid, new List<WeakReference>());
            FieldNodeItem.ConnItems[ctrlkeyid].Add(new WeakReference(this));



        }

        public override   void UpdateShowInfo()
        {

            var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(NodeId);
            if (para == null)
            {
                return;
            }
            this.NodeShowId = para.OrderId + "";
            this.NodeName = "-" + para.CtrlName;
        }

    }
}
