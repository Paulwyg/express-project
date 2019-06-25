using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.Wj6005Module.Jd601ManageSettingViewModel.ViewModel;
using Wlst.Ux.Wj6005Module.Resources;
using Wlst.Ux.Wj6005Module.Services;

namespace Wlst.Ux.Wj6005Module.Jd601ManageViewModel.ViewModel
{


    public class TreeNodeWj1090ViewModel : TreeNodeBaseViewModel
    {

        //public TreeNodeWj1080ViewModel(int rtuId, int rtuName)
        //{
        //    this.RtuId = rtuId;
        //    this.RtuId = rtuName;
        //    this.AttachRtuId = rtuId;
        //}

        /// <summary>
        /// 集中器设备
        /// </summary>
        /// <param name="rtuId">集中器地址</param>
        /// <param name="rtuName">集中器名称</param>
        /// <param name="attachRtuId">如果为附属设备则附属设备附属主设备地址，如果为主设备则填写主设备地址</param>
        public TreeNodeWj1090ViewModel(int rtuId, string rtuName, int attachRtuId)
        {
            this.NodeId = rtuId;
          //  this.NodeName = rtuName;
            this.AttachRtuId = attachRtuId;
            if (this.AttachRtuId > 0)
            {
                if (Jd601LoadSet.Myself.IsShowGrpInTreeModelShowId)
                {
                    this.NodeName = rtuName + " -" + string.Format("{0:D4}", NodeId);
                }
               else this.NodeName = rtuName;
            }
            else this.NodeName = rtuName;
            this.ImagesIcon = ImageResources.EsuIcon6005 ;
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
                    if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(value))
                    {
                        var res =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[value].RtuPhyId;
                            //.GetPhysicalIdByLogicalId(value);
                        if (res != 0)
                        {
                            value = res;
                        }
                    }
                    _attachRtuId = value;
                    this.RaisePropertyChanged(() => this.AttachRtuId);
                }
            }
        }

        /// <summary>
        /// 终端地址或分组地址4为地址化+name
        /// </summary>
        public override string NodeIdFormat
        {
            get
            {
                if (Jd601LoadSet.Myself.IsShowGrpInTreeModelShowId)
                {
                    return string.Format("{0:D4}", AttachRtuId) + "-";
                }
                return "";
            }
        }

        public override void OnNodeSelectDisActive()
        {
            base.OnNodeSelectDisActive();
            if (CmItems != null) CmItems .Clear();
        }

        public override void OnNodeSelectActive()
        {
            //base.OnNodeSelectActive();
            Jd601ManageViewModel.CurrentSelectedTreeNode = this;
            // ResetContextMenu();
        }

        #region  Reset ContextMenu

        public override void ResetContextMenu()
        {
            ResetCm();
        }

        public void  ResetCm()
        {
            ObservableCollection<IIMenuItem> t = null;
            if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(NodeId))
            {
                var tt = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[NodeId];

                t = MenuBuilding.BulidCm(tt.RtuModel.ToString(), false, tt);

            }
            CmItems = t;
        }

   



        #endregion
    }
}
