using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.WjEquipmentBaseModels.Interface;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.Wj1090Module.Resources;


namespace Wlst.Ux.Wj1090Module.LduTreeInfo.Base
{
    public partial class TreeNodeTmlViewModel : NodeBaseNode 
    {
        public TreeNodeTmlViewModel()
        {
            this.NodeType = TypeOfTreeNode.IsTml;
            Visi = Visibility.Visible;
            Md5 = 0;
        }

        public TreeNodeTmlViewModel(NodeBaseNode mvvmFather, IIEquipmentInfo terminalInfomation)
        {
            this.NodeType = TypeOfTreeNode.IsTml;
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

                PicIndex = 3005;
            }
            this.ReUpdate(9);
            UpdateTmlStateInfomation();

            
        }



      

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
                this.UpdateTmlInfomation();
            }
          
            if (updateArgu == 9)
            {
                if (this.ChildTreeItems.Count > 0) this.ChildTreeItems.Clear();
                this.ThisNodeAddPartsNode();
                //this.IsExpanded = true;
            }
            if(updateArgu ==11)
            {
                UpdateTmlStateInfomation();
            }
        }

        /// <summary>
        /// event update
        /// </summary>
        private  void UpdateTmlInfomation()
        {
            if (Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary .ContainsKey(NodeId))
            {
                var s =
                    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary [
                        NodeId];
                GetNoUsedCount(); 
                if (s.Md5 == this.Md5) return;
                NodeName = s.RtuName;
                this.Md5 = s.Md5;
            }
        }


        private void UpdateTmlStateInfomation()
        {


            if (
                Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(
                    NodeId))
            {
                var equ =
                    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[
                        NodeId];
                if (equ.RtuModel == 3005 || equ.RtuModel == 3090)
                {
                    var s = equ.RtuState;
                    if (s == 0)
                    {
                        PicIndex = 3001;
                        return;
                    }
                    if (s == 1)
                    {
                        PicIndex = 3002;
                        return;
                    }
                    var haserror =
                        Wlst.Sr.EquipemntLightFault.Services.TmlErrorStates.IsRtuHasError(this.NodeId);
                    var lighton = RtuNewDataService.IsRtuHasElectric(this.NodeId);
                    int errorindex = 0;
                    if (haserror && lighton) errorindex = 3;
                    if (haserror && !lighton) errorindex = 1;
                    if (!haserror && lighton) errorindex = 2;

                    PicIndex = 3005 + errorindex;
                }
                else
                {
                    var tmp =
                        Wlst.Sr.EquipemntLightFault.Services.TmlErrorStates.IsRtuHasError(this.NodeId);
                    PicIndex = equ.RtuModel + (tmp ? 1 : 0);
                }

            }

        }

        ///// <summary>
        ///// 根据发布的信息更新终端树状态信息
        ///// </summary>
        ///// <param name="basicTmlInfomation"></param>
        //private void UpdateTerminalInfo(IIEquipmentInfo basicTmlInfomation)
        //{
        //    NodeName = basicTmlInfomation.RtuName;
        //    this.Md5 = basicTmlInfomation.Md5;
        //}

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
            this.IsExpanded = true;
        }

        private void ThisNodeAddPartsNode()
        {
            if (
                !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(
                    this.NodeId)) return;
            var
                lstatt =
                    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetMainEquipmentAttachedLst(this.NodeId);


            foreach (var t in lstatt)
            {
                var fffff = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetAttachEquipmentInfo(t);
                if (fffff != null)
                {
                    var tmp = fffff as Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
                    if (tmp == null) continue;
                    var infos = new TreeNodeConcentratorViewModel(this, fffff);
                    this.ChildTreeItems.Add(infos );
                    infos.UpdateTmlStateInfomation();
                }
            }

            GetNoUsedCount();
        }

        #endregion

        #region
        private void GetNoUsedCount()
        {
                    if (
            !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(
                this.NodeId)) return;
                    var
                        lstatt =
                            Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetMainEquipmentAttachedLst(this.NodeId);

                    if(lstatt==null) return;
                    this.ConcentratorCount = 0;
                    foreach (var t in lstatt)
                    {
                        var fffff = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetAttachEquipmentInfo(t);
                        if (fffff != null)
                        {
                            var tmp = fffff as Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
                            if (tmp == null) continue;

                            foreach (var item in tmp.LduLines)
                            {
                                if (!item.IsUsed)
                                {
                                    this.ConcentratorCount += 1;
                                }
                            }
                            
                        }
                    }
            this.ConcentratorCountVisi = ConcentratorCount > 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion


        #region  Reset ContextMenu
        public override void ResetContextMenu()
        {
            HelpCmm(ResetCm());
            this.RaisePropertyChanged(() => this.Cm);
        }

        public ObservableCollection<IIMenuItem> ResetCm()
        {
            ObservableCollection<IIMenuItem> t = null;
            if (Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary .ContainsKey(NodeId))
            {
                var tt = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary [NodeId];
                
                    t =MenuBuilding.BulidCm(tt .RtuModel.ToString(), false, tt);
                
            }
            return t;
        }
        #endregion


    }


}
