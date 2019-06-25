using System.Collections.ObjectModel;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.Wj1050Module.Wj1050ManageSettingViewModel.ViewModel;

namespace Wlst.Ux.Wj1050Module.Wj1050ManageViewModel.ViewModel
{
    public class TreeNodeTmlViewModel : TreeNodeBaseViewModel
    {
        /// <summary>
        /// 设备
        /// </summary>
        /// <param name="rtuId">地址</param>
        /// <param name="rtuName">名称</param>
        public TreeNodeTmlViewModel(int rtuId, string rtuName)
        {
            this.NodeId = rtuId;
            if(Wlst .Sr .EquipmentInfoHolding .Services .EquipmentDataInfoHold .InfoItems .ContainsKey( rtuId ))
            NodeIds = string.Format("{0:D4}", Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems[rtuId].RtuPhyId) + " -";
            this.NodeName = rtuName;
            this.ImagesIcon = Resources.ImageResources.RtuIcon3005 ;
        }


        private ObservableCollection<TreeNodeWj1050ViewModel> _collectionWj1050;

        /// <summary>
        /// 开关量输入参数
        /// </summary>

        public ObservableCollection<TreeNodeWj1050ViewModel> CollectionWj1050
        {
            get
            {
                if (_collectionWj1050 == null)
                    _collectionWj1050 = new ObservableCollection<TreeNodeWj1050ViewModel>();
                return _collectionWj1050;
            }
        }


        public override void OnNodeSelectActive()
        {
            //base.OnNodeSelectActive();
            var args = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
            };
            args.AddParams(NodeId);

            EventPublish.PublishEvent(args);
       //     ResetContextMenu();

           
        }
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
                    if(!Wj1050TreeSetLoad.Myself.IsShowGrpInTreeModelShowId)
                    {
                        value = "";
                    }
                    _NodeIds = value;
                    this.RaisePropertyChanged(() => this.NodeIds);
                }
            }
        }

        #region  Reset ContextMenu

       public override void ResetContextMenu()
       {
           ResetCm();
       }

        public override void OnNodeSelectDisActive()
        {
            base.OnNodeSelectDisActive();
            if (CmItems != null) CmItems.Clear();
        }

       
        public void  ResetCm()
        {
            ObservableCollection<IIMenuItem> t = null;
            if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .ContainsKey(NodeId))
            {
                var tt = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems [NodeId];

                t = MenuBuilding.BulidCm(((int)tt.RtuModel).ToString(), false, tt);

            }
            this .CmItems = t;
        }

        #endregion
    }
}
