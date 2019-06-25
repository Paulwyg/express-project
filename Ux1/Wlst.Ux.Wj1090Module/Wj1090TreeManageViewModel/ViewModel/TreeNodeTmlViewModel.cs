using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.Wj1090Module.Resources;
using Wlst.client;

namespace Wlst.Ux.Wj1090Module.Wj1090TreeManageViewModel.ViewModel
{
    public class TreeNodeTmlViewModel : TreeNodeBaseViewModel
    {
        public TreeNodeTmlViewModel()
        {
              
        }

        public TreeNodeTmlViewModel(int rtuId, string rtuName):this()
        {
            NodeId = rtuId;
            NodeIds = string.Format("{0:D4}", Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuPhyId) + " -";
            NodeName = rtuName;
            GetUsedCount();
            PicIndex = 3005;
            UpdateTmlStateInfomation(rtuId);

            //ImagesIcon = Resources.ImageResources.GetEquipmentIcon(3005);
        }

        private ObservableCollection<TreeNodeWj1090ViewModel> _collectionWj1090;

        /// <summary>
        /// 开关量输入参数
        /// </summary>


        public ObservableCollection<TreeNodeWj1090ViewModel> ChildTreeItems
        {
            get { return _collectionWj1090 ?? (_collectionWj1090 = new ObservableCollection<TreeNodeWj1090ViewModel>()); }
        }

        private string _nodeIds;

        /// <summary>
        /// 如果连接终端 则终端地址  不允许修改
        /// </summary>
        public string NodeIds
        {
            get { return _nodeIds; }
            set
            {
                if (value != _nodeIds)
                {
                    _nodeIds = value;
                    this.RaisePropertyChanged(() => this.NodeIds);
                }
            }
        }


        private string _nExtendSerachConten;

        /// <summary>
        /// 如果连接终端 则终端地址  不允许修改
        /// </summary>
        public string ExtendSerachConten
        {
            get { return _nExtendSerachConten; }
            set
            {
                if (value != _nExtendSerachConten)
                {
                    _nExtendSerachConten = value;
                    this.RaisePropertyChanged(() => this.ExtendSerachConten);
                }
            }
        }

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

        public void UpdateTmlStateInfomation(int rtuId)
        {
            if(rtuId !=this .NodeId )
            {
                foreach (var g in this.ChildTreeItems) if (g.NodeId == rtuId)
                {
                    g.UpdateWj1090StateAndEquInfomation();
                    break;
                }
            }
            GetUsedCount();
            if (EquipmentDataInfoHold.InfoItems.ContainsKey( NodeId))
            {
                var equ =EquipmentDataInfoHold.InfoItems[NodeId];//.EquipmentInfoDictionary[ NodeId];
                if (equ.RtuModel ==EnumRtuModel.Wj3005 || equ.RtuModel ==EnumRtuModel.Wj3090 )
                {
                    var s = equ.RtuStateCode;
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

                    var lst = (from g in this.ChildTreeItems select g.NodeId).ToList();
                    bool
                        haserror = false;
                    foreach (var g in lst)
                    {
                        var runninginfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(g);

                        if (runninginfo!=null )
                        {
                            haserror = runninginfo .ErrorCount >0;
                            break;
                        }
                    }

                    //  var lighton = RtuNewDataService.IsRtuHasElectric(NodeId);
                    int errorindex = 0;
                    if (haserror) errorindex = 1;

                    PicIndex = 3005 + errorindex;
                }
                else
                {
                    //var tmp =
                    //    Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.IsRtuHasError(NodeId);
                    var runningInfo = Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(NodeId);
                    var tmp = runningInfo.ErrorCount>0;
                    PicIndex = (int)equ.RtuModel + (tmp ? 1 : 0);
                }

            }

        }

        public override void OnNodeSelectActive()
        {
            var args = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
            };
            args.AddParams(NodeId);

            EventPublish.PublishEvent(args);
            ResetContextMenu();
        }

        public override void OnNodeSelectDisActive()
        {
            base.OnNodeSelectDisActive();
            if (CmItems  != null)CmItems .Clear();
        }

        #region  Reset ContextMenu

        public override  void ResetContextMenu()
        {
            ResetCm();
        }



        public void  ResetCm()
        {
            ObservableCollection<IIMenuItem> t = null;
            if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems .ContainsKey(NodeId))
            {
                var tt = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[NodeId];

                t = MenuBuilding.BulidCm(tt.RtuModel.ToString(), false, tt);

            }
            CmItems = t;
        }

        #endregion

        #region
        private void GetUsedCount()
        {
            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(NodeId)) return;
            var
                lstatt =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[NodeId].
                        EquipmentsThatAttachToThisRtu;

                    //Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetMainEquipmentAttachedLst(NodeId);

            if (lstatt == null) return;
            ConcentratorCount = 0;
            foreach (var t in lstatt)
            {
                var fffff = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById(t);
                if (fffff != null)
                {
                    var tmp = fffff as Sr.EquipmentInfoHolding.Model.Wj1090Ldu;//Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
                    if (tmp == null) continue;

                    foreach (var item in tmp.WjLduLines.Values)
                    {
                        if (item.IsUsed)
                        {
                            ConcentratorCount += 1;
                        }
                    }

                }
            }
            ConcentratorCountVisi = ConcentratorCount > 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion

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

      
       
        public void EquipmentUpdate(int rtuId)
        {

            if(rtuId !=this .NodeId )
            {
                foreach (var g in this .ChildTreeItems )if(g.NodeId ==rtuId )
                {
                    g.UpdateWj1090StateAndEquInfomation();
                    g.UpdateNoUsedShow();
                    break;
                }
            }

            GetUsedCount();

            if (NodeId != rtuId) return;
            if (
                !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(
                    NodeId)) return;
            var s = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[NodeId];
            NodeName = s.RtuName;
        }

  
    }
}
