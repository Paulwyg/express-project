using System.Collections.ObjectModel;
using System.Windows.Controls;


using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.Wj1080Module.Wj1080ManageSettingViewModel.ViewModel;
using System;

namespace Wlst.Ux.Wj1080Module.Wj1080ManageViewModel.ViewModel
{
    public class TreeNodeWj1080ViewModel : TreeNodeBaseNode
    {

        //public TreeNodeWj1080ViewModel(int rtuId, int rtuName)
        //{
        //    this.RtuId = rtuId;
        //    this.RtuId = rtuName;
        //    this.AttachRtuId = rtuId;
        //}

        /// <summary>
        /// 光控设备
        /// </summary>
        /// <param name="rtuId">光控地址</param>
        /// <param name="rtuName">光控名称</param>
        /// <param name="attachRtuId">如果为附属设备则附属设备附属主设备地址，如果为主设备则填写主设备地址</param>
        public TreeNodeWj1080ViewModel(int rtuId, string rtuName, int attachRtuId)
        {
            
            this.NodeId = rtuId;
            this.NodeType = TypeOfTabTreeNode.IsTml;
            this.AttachRtuId = attachRtuId;

            //var areaGrp = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(rtuId);

            //int fistshow = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuPhyId;//phyId
            var nodename = "";
            if (attachRtuId == 0)
            {
                if (wj1080TreeSetLoad.Myself.IsShowGrpInTreeModelShowId)
                {
                    var phyId = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuPhyId;
                    nodename = string.Format("{0:D4}", phyId) + " - " + rtuName;
                }
                else
                {
                    nodename = rtuName;
                }
            }
            else
            {
                if (wj1080TreeSetLoad.Myself.IsShowGrpInTreeModelShowId)
                {
                    var phyId = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[attachRtuId].RtuPhyId;
                    nodename = string.Format("{0:D4}", phyId) + " - ";
                }
                if (wj1080TreeSetLoad.Myself.IsShowFid)
                {
                    nodename = nodename +
                                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[attachRtuId].
                                        RtuName + " - ";
                }
                nodename = nodename + rtuName;
            }

            this.NodeName = nodename;
            //if (this.AttachRtuId > 0)
            //{
            //    //fistshow = this.AttachRtuId;
            //    if (!wj1080TreeSetLoad.Myself.IsShowFid)
            //    {
            //        this.NodeName = string.Format("{0:D4}", fistshow) + "-" + rtuName;
            //    }
            //    else
            //    {
            //         var phyId = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[attachRtuId].RtuPhyId.ToString();
            //         this.NodeName = string.Format("{0:D4}", phyId) + "-" + Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[attachRtuId].RtuName + " -" + rtuName;
            //    }
            //}
            //else
            //{
            //    this.NodeName = string.Format("{0:D4}", fistshow) + "-" + rtuName;
            //}


            //var runningInfo = Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(NodeId);
            //if (runningInfo == null) return;
            //var tmp = runningInfo.ErrorCount > 0;
            //ImagesIcon = tmp ? Resources.ImageResources.GetEquipmentIcon(RtuModel + 1) : Resources.ImageResources.GetEquipmentIcon(RtuModel);
            this.ImagesIcon = Resources.ImageResources.MruIcon1080 ;
        }


        /// <summary>
        /// 终端地址或分组地址4为地址化+name
        /// </summary>
        ////public override string NodeIdFormat
        ////{
        ////    get
        ////    {
        ////        if (wj1080TreeSetLoad.Myself.IsShowGrpInTreeModelShowId)
        ////        {
        ////            return "-" + string.Format("{0:D4}", NodeId);
        ////        }
        ////        if (wj1080TreeSetLoad.Myself.IsShowFid)
        ////        {
        ////            //
        ////        }
        ////        return "";
        ////    }
        ////}


        private int _attachRtuId;

        /// <summary>
        /// 如果连接终端 则终端地址  不允许修改
        /// </summary>
        public int AttachRtuId
        {
            get { return _attachRtuId; }
            set
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsRtuLight(value))
                {
                    var res =
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value).RtuPhyId;//.GetPhysicalIdByLogicalId(value);
                    if (res > 0) value = res;
                }


                if (value != _attachRtuId)
                {
                    _attachRtuId = value;
                    this.RaisePropertyChanged(() => this.AttachRtuId);
                }
            }
        }


        private int _phyid;

        /// <summary>
        /// 如果连接终端 则终端地址  不允许修改
        /// </summary>
        public int PhyId
        {
            get { return _phyid; }
            set
            {
                if (value != _phyid)
                {
                    _phyid = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                    this.RaisePropertyChanged(() => this.NodeName );
                }
            }

        }
        public override void OnNodeSelectActive()
        {
            base.OnNodeSelectActive();
            //Wj1080ManageViewModel.CurrentSelectedTreeNode = this;
            //发布事件  选中当前节点
            var args = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
            };
            args.AddParams(NodeId);
            EventPublish.PublishEvent(args);
            ResetContextMenu();

            Wj1080ManageViewModel.MySelf .UpdateViewByLuxId(this  );
        }

        #region  Reset ContextMenu
        public override void ResetContextMenu()
        {
            ResetCm();
        }

        public void  ResetCm()
        {
            ObservableCollection<IIMenuItem> t = null;
            if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(NodeId))//.EquipmentInfoDictionary .ContainsKey(NodeId))
            {
                var tt = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[NodeId ];//.EquipmentInfoDictionary [NodeId];

                t = MenuBuilding.BulidCm( ((int)tt.RtuModel).ToString(), false, tt);
                
            }
            CmItems = t;
        }

       
        #endregion
    }
}
