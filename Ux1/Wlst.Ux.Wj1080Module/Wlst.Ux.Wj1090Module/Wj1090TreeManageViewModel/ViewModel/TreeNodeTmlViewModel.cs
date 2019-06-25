using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.Wj1090Module.Resources;

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
            NodeIds = string.Format("{0:D4}", Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[rtuId].PhyId) + " -";
            NodeName = rtuName;
            GetNoUsedCount();
            PicIndex = 3005;
            UpdateTmlStateInfomation(rtuId);

            //ImagesIcon = Resources.ImageResources.GetEquipmentIcon(3005);
        }

        private ObservableCollection<TreeNodeWj1090ViewModel> _collectionWj1090;

        /// <summary>
        /// 开关量输入参数
        /// </summary>


        public ObservableCollection<TreeNodeWj1090ViewModel> CollectionWj1090
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
                foreach (var g in this.CollectionWj1090) if (g.NodeId == rtuId)
                {
                    g.UpdateWj1090StateAndEquInfomation();
                    break;
                }
            }
            GetNoUsedCount();
            if (ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey( NodeId))
            {
                var equ =ServicesEquipemntInfoHold.EquipmentInfoDictionary[ NodeId];
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

                    var lst = (from g in this.CollectionWj1090 select g.NodeId).ToList();
                    bool
                        haserror = false;
                    foreach (var g in lst)
                    {
                        var tmpssss = Sr.EquipemntLightFault.Services.TmlErrorStates.IsRtuHasError(g);
                        if (tmpssss)
                        {
                            haserror = true;
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
                    var tmp =
                        Sr.EquipemntLightFault.Services.TmlErrorStates.IsRtuHasError(NodeId);
                    PicIndex = equ.RtuModel + (tmp ? 1 : 0);
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

            EventPublisher.EventPublish(args);
            ResetContextMenu();
        }

        #region  Reset ContextMenu

        private void ResetContextMenu()
        {
            HelpCmm(ResetCm());
            this.RaisePropertyChanged(() => this.Cm);
        }


        protected void HelpCmm(ObservableCollection<IIMenuItem> t)
        {
            var ggggg = Wlst.Sr.Menu.Services.MenuBuilding.HelpCmm(t);
            Cm.Items.Clear();
            foreach (var gggggggg in ggggg)
            {
                Cm.Items.Add(gggggggg);
            }
            return;
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

        #region
        private void GetNoUsedCount()
        {
            if (!Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(NodeId)) return;
            var
                lstatt =
                    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetMainEquipmentAttachedLst(NodeId);

            if (lstatt == null) return;
            ConcentratorCount = 0;
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
                            ConcentratorCount += 1;
                        }
                    }

                }
            }
            ConcentratorCountVisi = ConcentratorCount > 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion

        #region 在树上显示统计的未使用防盗线路数

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
                foreach (var g in this .CollectionWj1090 )if(g.NodeId ==rtuId )
                {
                    g.UpdateWj1090StateAndEquInfomation();
                    g.UpdateNoUsedShow();
                    break;
                }
            }

            GetNoUsedCount();

            if (NodeId != rtuId) return;
            if (
                !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(
                    NodeId)) return;
            var s = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[NodeId];
            NodeName = s.RtuName;
        }

  
    }
}
