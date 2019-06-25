using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.WjEquipmentBaseModels.Interface;

using Wlst.Sr.Menu.Services;
using Wlst.Sr.ProtocolCnt.Fault;
using Wlst.Ux.Wj1090Module.Resources;


namespace Wlst.Ux.Wj1090Module.LduTreeInfo.Base
{

   public partial class TreeNodeConcentratorViewModel : NodeBaseNode
   {
       public TreeNodeConcentratorViewModel()
       {
          
           this.NodeType = TypeOfTreeNode.IsConcentrator;
           Visi = Visibility.Visible;
           Md5 = 0;
       }


       private int RtuModelss = 0;
       public TreeNodeConcentratorViewModel(NodeBaseNode mvvmFather, IIEquipmentInfo terminalInfomation)
       {
           this.NodeType = TypeOfTreeNode.IsConcentrator ;
           Visi = Visibility.Visible;
           this._father = mvvmFather;
           Md5 = 0;

       
           

           //TreeSingleViewModel.RegisterNodeToControl(this);

           if (terminalInfomation == null)
           {
               this.NodeName = "加载出错";
           }
           else
           {
               this.NodeName = terminalInfomation.RtuName;
               //     this.ImagesIcon = ImageResources.GetTmlTreeIcon(6);
               this.NodeId = terminalInfomation.RtuId;
               Md5 = terminalInfomation.Md5;
               RtuModelss = terminalInfomation.RtuModel;
           }
           GetNoUsedCount();
           ReUpdate(11);
       }

       private void GetNoUsedCount()
       {
           var info = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(this.NodeId);
           if (info == null) return;
           var lines = info as Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
           if (lines == null) return;
           if (lines.LduLines == null) return;
           this.ConcentratorCount = 0;
           foreach (var t in lines.LduLines)
           {
               if (!t.IsUsed)
               {
                   ConcentratorCount = ConcentratorCount + 1;
               }
           }
           this.ConcentratorCountVisi = ConcentratorCount > 0 ? Visibility.Visible : Visibility.Collapsed;
       }


       #region

       private static NodeBaseNode _currentSelectedTreeNode;
       //用来清除前节点  的动态生成节点
       public static NodeBaseNode CurrentSelectedTreeNode
       {
           get { return _currentSelectedTreeNode; }
           set
           {
               if (_currentSelectedTreeNode != value)
               {
                   if (_currentSelectedTreeNode != null)
                   {
                       _currentSelectedTreeNode.CleanChildren();
                   }
                   _currentSelectedTreeNode = value;
               }
           }
       }

       #endregion

       #region  Update infomation
       /// <summary>
       /// 当分组信息发生变化的时候  增量式重新加载节点
       /// One Update terminal  informaiton
       /// Two Update terminal Running information
       /// </summary>
       public override void ReUpdate(int updateArgu)
       {
           if (updateArgu == 1)
           {
               this.UpdateInfosmation();

               this.UpdateTmlInfomation();
           }
           if(updateArgu ==11)
           {
               if (Wlst.Sr.EquipemntLightFault.Services.TmlErrorStates.IsRtuHasError(this .NodeId ))
               {
                   this.ImagesIcon = ImageResources.GetEquipmentIcon(RtuModelss + 1);
               }
               else
               {
                   this.ImagesIcon = ImageResources.GetEquipmentIcon(RtuModelss);
               }
           }
       }

       /// <summary>
       /// event update
       /// </summary>
       private void UpdateTmlInfomation()
       {
           if (Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.AttachEquipmentInfoDictionary.ContainsKey(NodeId))
           {
               var s =
                   Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.AttachEquipmentInfoDictionary[
                       NodeId];

               if (s.Md5 == this.Md5) return;
               NodeName = s.RtuName;
               this.Md5 = s.Md5;
           }
       }

       void UpdateInfosmation()
       {
           var info = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(this.NodeId);
           if (info == null) return;
           var lines = info as Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
           if (lines == null) return;
           if (lines.LduLines == null) return;
           foreach (NodeBaseNode item in ChildTreeItems)
           {
               foreach (var oneline in lines.LduLines)
               {
                   if (oneline.LduLineID == item.NodeId)
                   {
                       item.NodeName = oneline.LduLineName;
                       if (oneline.IsUsed)
                       {
                           item.IsUsed = Visibility.Visible;
                           item.NoUsed = Visibility.Collapsed;
                       }
                       else
                       {
                           item.IsUsed = Visibility.Collapsed;
                           item.NoUsed = Visibility.Visible;
                       }
                       break;
                   }
               }
           }
           GetNoUsedCount();
       }


       //private void UpdateTmlStateInfomation()
       //{
       //    if (EquipmentRunningInfoHolding.TmlRunningInfoDictionary.ContainsKey(this.NodeId))
       //    {
       //        this.UpdateTerminalStateInfo(EquipmentRunningInfoHolding.TmlRunningInfoDictionary[this.NodeId]);
       //    }
       //}

       //private void UpdateTerminalStateInfo(TerminalRunningInfomation basicTmlInfomation)
       //{
       //    //base.NodeModel = basicTmlInfomation.Model;
       //    if (!basicTmlInfomation.IsConnected)
       //    {
       //        ForeGround = "#FFA9A9A9";
       //        BackGround = "#FFFF1493";
       //        ImagesIcon = ImageResources.GetTmlTreeIcon(1);
       //    }
       //    else
       //    {
       //        ForeGround = "#FF000";
       //        //终端图标代码 1 连接断开 0关灯正常 2关灯故障  3 开灯正常 4开灯故障
       //        ImagesIcon = ImageResources.GetTmlTreeIcon(basicTmlInfomation.ImageCode);
       //    }
       //}

       #endregion

       /// <summary>
       /// 加载节点，第一次使用
       /// </summary>
       public override void AddChild()
       {
           ChildTreeItems.Clear();
       }

       #region Node Select
       /// <summary>
       /// 当选择的终端发送变化时，如果 
       /// </summary>
       public override void OnNodeSelectActive()
       {
           ////base.OnNodeSelect();
           ////发布事件  选中当前节点
           //var args = new PublishEventArgs
           //{
           //    EventType = PublishEventType.Core,
           //    EventId = Sr.EquipmentGroupInfoHolding.Services.EventIdAssign.MainSingleTreeNodeActive,
           //};
           //args.AddParams(NodeId);
           //args.AddParams(2);
           //EventPublisher.EventPublish(args);


           if (this.ChildTreeItems.Count > 0) this.ChildTreeItems.Clear();
           //  this.ThisNodeAddLoopsNode();
           this.ThisNodeAddPartsNode();
           this.IsExpanded = true;

           TreeNodeConcentratorViewModel.CurrentSelectedTreeNode = this;
           //ResetContextMenu();

           var ar = new PublishEventArgs()
           {
               EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
               EventType = PublishEventType.Core
           };
           ar.AddParams(this.Father.NodeId); //终端地址
           ar.AddParams(this.NodeId);        //集中器地址
           ar.AddParams(0);               //线路ID，0表示全部线路
           EventPublisher.EventPublish(ar);
       }


       private void ThisNodeAddPartsNode()
       {
           //if (!Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(this.NodeId)) return;
           var info = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(this.NodeId);
           if (info == null) return;
           var lines = info as Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
           if (lines == null) return;
           if (lines.LduLines == null) return;

           var errors =
               Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetLstInfoByRtuId(this.NodeId);
           var lineerrs = new List<Tuple<int, int>>();
           foreach (var t in errors)
           {
               var errorinfo = Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetShowInfoById(t);
               if (errorinfo == null) continue;
               if
                   (Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.ContainsKey(t))
               {
                   var infogggg = Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary[t];
                   lineerrs.Add(new Tuple<int, int>(infogggg.LoopId, infogggg.FaultCodeId)); //41 被盗 42 短路
               }
           }

           foreach (var t in lines.LduLines)
           {

               var str = "";
               var strForeGround = "Black";
               if (t.IsUsed)
               {
                   strForeGround = "Black";
                   if (lineerrs.Contains(new Tuple<int, int>(t.LduLineID, 42)))
                   {
                       str = " -短路";
                       strForeGround = "Red";
                   }
                   if (lineerrs.Contains(new Tuple<int, int>(t.LduLineID, 41)))
                   {
                       str = " -被盗";
                       strForeGround = "Red";
                   }
               }

               var infos = new TreeNodeLineViewModel(this, t.LduLineName + str, t.LduLineID, t.IsUsed);
               infos.ForeGround = strForeGround;
               this.ChildTreeItems.Add(infos);
               infos.UpdateTmlStateInfomation();
           }

       }


       #endregion

       #region  Reset ContextMenu
       public override  void ResetContextMenu()
       {
           HelpCmm(ResetCm());
           this.RaisePropertyChanged(() => this.Cm);
       }

       public ObservableCollection<IIMenuItem> ResetCm()
       {
           ObservableCollection<IIMenuItem> t = null;
           if (Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(NodeId))
           {
               var tt = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[NodeId];

               t = MenuBuilding.BulidCm(tt.RtuModel.ToString(), false, tt);

           }
           return t;
       }
       #endregion


   }


}
