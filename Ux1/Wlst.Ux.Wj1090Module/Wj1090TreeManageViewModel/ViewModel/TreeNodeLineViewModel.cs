using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;

namespace Wlst.Ux.Wj1090Module.Wj1090TreeManageViewModel.ViewModel
{
    public class TreeNodeLineViewModel : TreeNodeBaseViewModel
    {
       
        public TreeNodeLineViewModel( string name, int lineId, bool beenUsed,int confaRtuId)
        {
           // this.NodeType = TypeOfTreeNode.IsLine;
            Visi = Visibility.Visible;
         
            NodeName = name ;
            NodeId = lineId;
            FatherId = confaRtuId;
            NodeIds = string.Format("{0:D2}", lineId) + " -";
            NoUsed = beenUsed ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// 集中器地址
        /// </summary>
        protected  int FatherId;
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
                    _NodeIds = value;
                    this.RaisePropertyChanged(() => this.NodeIds);
                }
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
            args.AddParams(FatherId);
            args.AddParams(NodeId);

            EventPublish.PublishEvent(args);
        }

        #region 在树上显示统计的未使用线路检测线路数

        //统计显示控制

        #region ConcentratorCountVisi

        private Visibility _concentratorCountVisi = Visibility.Collapsed;

        public Visibility ConcentratorCountVisi
        {
            get { return _concentratorCountVisi; }
            set
            {
                if (_concentratorCountVisi == value) return;
                _concentratorCountVisi = value;
                RaisePropertyChanged(() => this.ConcentratorCountVisi);
            }
        }

        #endregion

        //统计数

        #region  ConcentratorCount

        private int _concentratorCount;

        public int ConcentratorCount
        {
            get { return _concentratorCount; }
            set
            {
                if (_concentratorCount == value) return;
                _concentratorCount = value;
                RaisePropertyChanged(() => this.ConcentratorCount);
            }
        }

        #endregion

        #region NoUsed

        private Visibility _noUsed = Visibility.Collapsed;

        public Visibility NoUsed
        {
            get { return _noUsed; }
            set
            {
                if (_noUsed != value)
                {
                    _noUsed = value;
                    RaisePropertyChanged(() => this.NoUsed);
                }

            }
        }

        #endregion

        #endregion
    }
}
