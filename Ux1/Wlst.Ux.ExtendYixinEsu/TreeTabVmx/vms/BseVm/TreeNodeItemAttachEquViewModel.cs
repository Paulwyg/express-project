using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
 
using Wlst.Sr.Menu.Services;
using EventIdAssign = Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign;

namespace Wlst.Ux.ExtendYixinEsu.TreeTabVmx.vms.BseVm
{

   public class TreeNodeItemAttachEquViewModel : TreeNodeBaseNode
   {
       public TreeNodeItemAttachEquViewModel()
       {
           this.NodeType = TypeOfTabTreeNode.IsTmlParts ;
           Visi = Visibility.Visible;
       }

       public TreeNodeItemAttachEquViewModel(TreeNodeBaseNode mvvmFather, Wlst .Sr .EquipmentInfoHolding .Model .WjParaBase  attachInfomation)
       {
           this.NodeType = TypeOfTabTreeNode.IsTmlParts;
           Visi = Visibility.Visible;
           this._father = mvvmFather;

           if (attachInfomation == null)
           {
               this.NodeName = "加载出错";
           }
           else
           {
               this.NodeName = attachInfomation.RtuName;
               var running = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(attachInfomation.RtuId);
               if (running!=null && running .ErrorCount >0)
               {
                   this.ImagesIcon = ImageResources.GetEquipmentIcon((int )attachInfomation.RtuModel + 1);
               }
               else
               {
                   this.ImagesIcon = ImageResources.GetEquipmentIcon((int)attachInfomation.RtuModel);
               }

               this.NodeId = attachInfomation.RtuId;
               this.Md5 = attachInfomation.DateUpdate ;
           }
       }


       /// <summary>
       /// 加载节点，第一次使用
       /// </summary>
       public override void AddChild()
       {
           ChildTreeItems.Clear();
       }

       /// <summary>
       /// 当选择的终端发送变化时，如果 
       /// </summary>
       public override void OnNodeSelectActive()
       {
           //base.OnNodeSelect();
           //发布事件  选中当前节点
           var args = new PublishEventArgs
           {
               EventType = PublishEventType.Core,
               EventId = EventIdAssign.EquipmentSelected,
           };
           args.AddParams(NodeId);
           EventPublisher.EventPublish(args);

       }

       /// <summary>
       /// 终端地址或分组地址4为地址化+name
       /// </summary>
       public override string NodeIdFormat
       {
           get
           {
                   return string.Format("{0:D4}", NodeId) + "-";
           }
       }

       #region  Reset ContextMenu

       public override void ResetContextMenu()
       {
           ResetCm();
       }

       public void  ResetCm()
       {
           ObservableCollection<IIMenuItem> t = null;

           var attachEquipment = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold  
               .GetInfoById( this.NodeId);
           if (attachEquipment != null)
           {

               t = MenuBuilding.BulidCm(((int )attachEquipment.RtuModel).ToString(), false, attachEquipment);
               
           }
           CmItems = t;
       }

       #endregion


   }
}
