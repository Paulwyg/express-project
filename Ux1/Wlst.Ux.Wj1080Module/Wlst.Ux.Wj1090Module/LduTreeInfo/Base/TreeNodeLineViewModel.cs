using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.WjEquipmentBaseModels.Interface;
using Wlst.Sr.Menu.Services;

namespace Wlst.Ux.Wj1090Module.LduTreeInfo.Base
{

    public class TreeNodeLineViewModel : NodeBaseNode
    {
        public TreeNodeLineViewModel()
        {
            this.NodeType = TypeOfTreeNode.IsLine;
            Visi = Visibility.Visible;
            Md5 = 0;

            RightMenuKey = "10909";
        }

        public TreeNodeLineViewModel(NodeBaseNode mvvmFather, string name, int lineId, bool beenUsed)
        {
            this.NodeType = TypeOfTreeNode.IsLine;
            Visi = Visibility.Visible;
            this._father = mvvmFather;
            Md5 = 0;
            RightMenuKey = "10909";
            LineId = lineId;
            this.NodeName = name;
            //     this.ImagesIcon = ImageResources.GetTmlTreeIcon(6);
            this.NodeId = lineId;

            if (beenUsed)
            {
                IsUsed = Visibility.Visible;
                NoUsed = Visibility.Collapsed;
                ConcentratorCount = 0;
            }
            else
            {
                IsUsed = Visibility.Collapsed;
                NoUsed = Visibility.Visible;
                ConcentratorCount = 1;
            }
            UpdateTmlStateInfomation();
        }

        protected int LineId = 0;


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
            //ResetContextMenu();
            var ar = new PublishEventArgs()
                         {
                             EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                             EventType = PublishEventType.Core
                         };
            ar.AddParams(this.Father.Father.NodeId); //终端地址
            ar.AddParams(this.Father.NodeId); //集中器地址
            ar.AddParams(this.NodeId); //线路ID
            EventPublisher.EventPublish(ar);


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
            if (Father == null) return new ObservableCollection<IIMenuItem>();

            ObservableCollection<IIMenuItem> t = null;
            var xxx = new int[2] {Father.NodeId, NodeId};

            t = MenuBuilding.BulidCm(RightMenuKey, false, xxx);
            return t;
        }

        #endregion


        public override void UpdateTmlStateInfomation()
        {
            PicIndex = 10909;

        }

    }
}
