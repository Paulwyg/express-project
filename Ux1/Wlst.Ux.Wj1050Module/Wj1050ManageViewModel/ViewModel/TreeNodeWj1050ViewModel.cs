using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;


using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.Wj1050Module.Resources;
using Wlst.Ux.Wj1050Module.Wj1050ManageSettingViewModel.ViewModel;

namespace Wlst.Ux.Wj1050Module.Wj1050ManageViewModel.ViewModel
{
    public class TreeNodeWj1050ViewModel :TreeNodeBaseNode// TreeNodeBaseViewModel
    {

        //public TreeNodeWj1080ViewModel(int rtuId, int rtuName)
        //{
        //    this.RtuId = rtuId;
        //    this.RtuId = rtuName;
        //    this.AttachRtuId = rtuId;
        //}
        public static Dictionary<int, List<WeakReference>> RtuItems = new Dictionary<int, List<WeakReference>>();
        /// <summary>
        /// 终端id和电表id 对应字典  lvf 2018年3月30日08:34:59
        /// </summary>
        public static Dictionary<int, List<int>> RtuMruIds = new Dictionary<int, List<int>>();
        /// <summary>
        /// 设备
        /// </summary>
        /// <param name="rtuId">地址</param>
        /// <param name="rtuName">名称</param>
        /// <param name="attachRtuId">如果为附属设备则附属设备附属主设备地址，如果为主设备则填写主设备地址</param>
        public TreeNodeWj1050ViewModel(int rtuId, string rtuName, int attachRtuId)
        {
            this.NodeType = TypeOfTabTreeNode.IsTml;
            this.NodeId = rtuId;
            this.RtuName = "";
            var nodename  = "";


            if (true)
            {

                WeakReference refs = new WeakReference(this);
                if (RtuItems.ContainsKey(rtuId) == false)
                    RtuItems.Add(rtuId, new List<WeakReference>());
                RtuItems[rtuId].Add(refs);

                //记录主设备与附属设备对应关系
                if (attachRtuId!=0 && RtuMruIds.ContainsKey(attachRtuId)==false )
                {
                    List<int> lst = new List<int>();
                    lst.Add(rtuId);
                    RtuMruIds.Add(attachRtuId,lst);
                }
                else
                {
                    if (!RtuMruIds[attachRtuId].Contains(rtuId)) RtuMruIds[attachRtuId].Add(rtuId );
                }
            }



            if(attachRtuId==0)    //为主设备
            {
                this.PhyId = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuPhyId;
                if (Wj1050TreeSetLoad.Myself.IsShowGrpInTreeModelShowId)
                {
                    var phyId = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuPhyId;
                    nodename = string.Format("{0:D4}", phyId) + " - " + rtuName;
                }
                else
                {
                    nodename = rtuName;
                }
                this.MruName = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuRemark;
            }
            else  //为附属设备
            {
                this.PhyId = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[attachRtuId].RtuPhyId;
                if (Wj1050TreeSetLoad.Myself.IsShowGrpInTreeModelShowId)
                {
                    var phyId = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[attachRtuId].RtuPhyId;
                    nodename = string.Format("{0:D4}", phyId) + " - ";
                }
                if (Wj1050TreeSetLoad.Myself.IsShowFid)
                {
                    nodename = nodename +
                                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[attachRtuId].
                                        RtuName+" - ";
                }
                nodename = nodename + rtuName;
                this.RtuName = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[attachRtuId].
                    RtuName;
                this.MruName =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[attachRtuId].RtuRemark;

           
            }
            this.MruName = rtuName;
            this.NodeName = nodename;
            this.AttachRtuId = attachRtuId;
            this.ImagesIcon = Resources.ImageResources.MruIcon1050 ;
            UpdateTmlStateInfomation();
        }


        private int _attachRtuId;

        /// <summary>
        /// 如果连接终端 则终端地址  不允许修改
        /// </summary>
        public int AttachRtuId
        {
            get { return _attachRtuId; }
            set
            {
                if (value != _attachRtuId)
                {
                    _attachRtuId = value;
                    this.RaisePropertyChanged(() => this.AttachRtuId);
                }
            }
        }
        public override void OnNodeSelectDisActive()
        {
            base.OnNodeSelectDisActive();
            if (CmItems != null) CmItems.Clear();
        }
        public override void OnNodeSelectActive()
        {
            //base.OnNodeSelectActive();
            if (Wj1050ManageViewModel.MySelf == null) return;
            Wj1050ManageViewModel.MySelf.CurrentSelectedTreeNode = this;
            //ResetContextMenu();

            //base.OnNodeSelectActive();
            var args = new PublishEventArgs
                           {
                               EventType = PublishEventType.Core,
                               EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                           };
            args.AddParams(NodeId);

            EventPublish.PublishEvent(args);
           // ResetContextMenu();
        }

        #region  Reset ContextMenu


        private string _NodeIds;

        /// <summary>
        /// 如果连接终端 则终端地址  不允许修改
        /// </summary>
        public string NodeIds
        {
            get { return _NodeIds; }
            set
            {
                if (value != _NodeIds)
                {
                    if (!Wj1050TreeSetLoad.Myself.IsShowGrpInTreeModelShowId)
                    {
                        value = "";
                    }
                    _NodeIds = value;
                    this.RaisePropertyChanged(() => this.NodeIds);
                }
            }
        }

        public override void ResetContextMenu()
        {
            ResetCm();
        }

        public void  ResetCm()
        {
            ObservableCollection<IIMenuItem> t = null;
            if (
                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                    InfoItems .ContainsKey(NodeId))
            {
                var s =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                        InfoItems [NodeId];

                t = MenuBuilding.BulidCm( ((int)s.RtuModel).ToString(), false, s);

            }
            this.CmItems = t;
        }

        /// <summary>
        /// 图片图标  停用图标 地址 1010199  不用图标1010198  
        /// 其他状态使用 1010100+状态值
        /// </summary>
        private int _picIndex = 0;

        protected int PicIndex
        {
            get { return _picIndex; }
            set
            {
                if (value != _picIndex)
                {
                    _picIndex = value;
                    ImagesIcon = ImageResources.GetEquipmentIcon(value);
                }
            }
        }


        /// <summary>
        /// 当分组信息发生变化的时候  增量式重新加载节点
        /// One Update terminal  informaiton
        /// Two Update terminal Running information
        /// </summary>
        public override void ReUpdate(int updateArgu)
        {
            if (updateArgu == 1)
            {
                UpdateTmlStateInfomation();
            }
        }



        public void UpdateTmlStateInfomation()  //todo  lvf 2018年3月29日10:08:46 更新电表图标
        {
            if (AttachRtuId == 0) return;//电表为主设备,木有状态
            var TerInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(AttachRtuId);
            if (TerInfo == null) return;
            var runninfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(AttachRtuId);

            // int modelId = (int) TerInfo.EquipmentType;
            if (TerInfo.EquipmentType == WjParaBase.EquType.Rtu)
            {
                var s = TerInfo.RtuStateCode;
                if (s == 0)
                {
                    PicIndex = 1051;  //采用离线
                    return;
                }
                if (s == 1)
                {
                    PicIndex = 1051;
                    return;
                }

                var online = runninfo != null && runninfo.IsOnLine;
                if (online == false)
                {
                    PicIndex = 1051;
                    return;
                }
                PicIndex = 1050;
            }


        }
     
        #endregion
    }
}
