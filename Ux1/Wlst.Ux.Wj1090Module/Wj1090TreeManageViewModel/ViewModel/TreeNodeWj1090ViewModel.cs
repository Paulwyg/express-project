using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;

using Wlst.Ux.Wj1090Module.LduTreeSettingViewModel.ViewModel;

namespace Wlst.Ux.Wj1090Module.Wj1090TreeManageViewModel.ViewModel
{
    public class TreeNodeWj1090ViewModel : TreeNodeBaseNode
    {
        public TreeNodeWj1090ViewModel()
        {

        }

        public TreeNodeWj1090ViewModel(int rtuId, string rtuName, int attachRtuId)
            : this()
        {
            NodeId = rtuId;
            this.NodeType=TypeOfTabTreeNode.IsTml;
            var nodename = "";
            if (attachRtuId == 0)
            {
                if (Wj1090TreeSetLoad.Myself.IsShowGrpInTreeModelShowId)
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
                if (Wj1090TreeSetLoad.Myself.IsShowGrpInTreeModelShowId)
                {
                    var phyId = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[attachRtuId].RtuPhyId;
                    nodename = string.Format("{0:D4}", phyId) + " - ";
                }
                if (Wj1090TreeSetLoad.Myself.IsShowFid)
                {
                    nodename = nodename +
                                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[attachRtuId].
                                        RtuName + " - ";
                }
                nodename = nodename + rtuName;
            }
            this.NodeName = nodename;
            //if (Wj1090TreeSetLoad.Myself.IsShowFid && attachRtuId != 0)
            //{
            //    var phyId = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[attachRtuId].RtuPhyId;
            //    //NodeIds =a.Substring(a.Length-4,4) + " -";
            //    this.NodeName = string.Format("{0:D4}", phyId) + " - " + Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[attachRtuId].RtuName + " -" + rtuName;
            //}
            //else
            //{
            //    var phyId = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].RtuPhyId;
            //    //NodeIds = string.Format("{0:D4}", rtuId) + " -";
            //    NodeName = string.Format("{0:D4}", phyId) + " - " + rtuName;
            //}

            //this.ImagesIcon = Resources.ImageResources.GroupIcon;
            AttachRtuId = attachRtuId;
            GetUsedCount();
            GetRtuModel();
            UpdateWj1090StateAndEquInfomation();

        }
        private void GetRtuModel()
        {
            var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(NodeId);
            if (info == null) return;
            RtuModel = (int)info.RtuModel;
            var runningInfo = Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(NodeId);
            var tmp = false;
            if (runningInfo ==null)
            {
                tmp = false;
            }else if (runningInfo.ErrorCount>0)
            {
                tmp = true ;
            }
            
            ImagesIcon = tmp ? Resources.ImageResources.GetEquipmentIcon(RtuModel + 1) : Resources.ImageResources.GetEquipmentIcon(RtuModel);
            //ImagesIcon = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.IsRtuHasError(NodeId) ? Resources.ImageResources.GetEquipmentIcon(RtuModel + 1) : Resources.ImageResources.GetEquipmentIcon(RtuModel);
        }
        private void GetUsedCount()
        {
            var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(this.NodeId);//.GetEquipmentInfo(this.NodeId);
            if (info == null) return;
            var lines = info as Sr.EquipmentInfoHolding.Model.Wj1090Ldu;//Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
            if (lines == null) return;
            if (lines.WjLduLines == null) return;
            ConcentratorCount = 0;
            foreach (var t in lines.WjLduLines.Values)
            {
                if (t.IsUsed)
                {
                    ConcentratorCount++;
                }
            }
            ConcentratorCountVisi = ConcentratorCount > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        ////////////private ObservableCollection<TreeNodeLineViewModel> _collectionWj1090;   lvf

        /////////////// <summary>
        /////////////// 开关量输入参数
        /////////////// </summary>


        ////////////public ObservableCollection<TreeNodeLineViewModel> ChildTreeItems
        ////////////{
        ////////////    get { return _collectionWj1090 ?? (_collectionWj1090 = new ObservableCollection<TreeNodeLineViewModel>()); }
        ////////////}


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

        private int _rtuModel;
        public int RtuModel
        {
            get { return _rtuModel; }
            set
            {
                if (_rtuModel.Equals(value)) return;
                _rtuModel = value;
                RaisePropertyChanged(() => RtuModel);
            }
        }

        public override void OnNodeSelectActive()
        {
            if (ChildTreeItems.Count > 0) ChildTreeItems.Clear();
            ThisNodeAddPartsNode();
            IsExpanded = true;
            CurrentSelectNode = this;

            var args = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
            };
            args.AddParams(NodeId);

            EventPublish.PublishEvent(args);
        }

        private static TreeNodeWj1090ViewModel _currentSelectNode;

        public static TreeNodeWj1090ViewModel CurrentSelectNode
        {
            get { return _currentSelectNode; }
            set
            {
                if (_currentSelectNode != value)
                {
                    if (_currentSelectNode != null)
                    {
                        _currentSelectNode.CleanChildren();
                    }
                    _currentSelectNode = value;
                }
            }
        }

        public void CleanChildren()
        {
            for (int i = ChildTreeItems.Count - 1; i > -1; i--)
            {
                ChildTreeItems.RemoveAt(i);
            }
        }

        private void ThisNodeAddPartsNode()
        {
            var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(this.NodeId);
            if (info == null) return;
            var lines = info as Sr.EquipmentInfoHolding.Model.Wj1090Ldu;//Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
            if (lines == null) return;
            if (lines.WjLduLines == null) return;

            var errors =
                Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultLstInfoByRtuId(this.NodeId);
            var lineerrs = new List<Tuple<int, int>>();
            foreach (var infogggg in errors)
            {
                // var errorinfo = Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t);
                //if (errorinfo == null) continue;
                //if
                //    (Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.ContainsKey(errorinfo .FaultId))
                //{
                //    var infogggg = Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary[errorinfo.FaultId];
                lineerrs.Add(new Tuple<int, int>(infogggg.LoopId, infogggg.FaultId)); //41 被盗 42 短路
                // }
            }

            foreach (var t in lines.WjLduLines.Values)
            {

                var str = "";
                var strForeGround = "Black";
                if (t.IsUsed)
                {
                    strForeGround = "Black";
                    if (lineerrs.Contains(new Tuple<int, int>(t.LduLineId, 42)))
                    {
                        str = " -短路";
                        strForeGround = "Red";
                    }
                    if (lineerrs.Contains(new Tuple<int, int>(t.LduLineId, 41)))
                    {
                        str = " -被盗";
                        strForeGround = "Red";
                    }
                }

                //var infos = new TreeNodeLineViewModel(t.LduLineName + str, t.LduLineId, t.IsUsed, this.NodeId);
                //infos.ForeGround = strForeGround;
                //ChildTreeItems.Add(infos);
            }

        }


        private void UpdateLinesInfoAndError()
        {
            var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(this.NodeId);//.GetEquipmentInfo(this.NodeId);
            if (info == null) return;
            var lines = info as Sr.EquipmentInfoHolding.Model.Wj1090Ldu;//Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
            if (lines == null) return;
            if (lines.WjLduLines == null) return;
            var idr = new Dictionary<int, string>();
            foreach (var g in lines.WjLduLines.Values) if (!idr.ContainsKey(g.LduLineId)) idr.Add(g.LduLineId, g.LduLineName);

            var errors =
                Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultLstInfoByRtuId(this.NodeId);
            var lineerrs = new List<Tuple<int, int>>();
            foreach (var infogggg in errors)
            {
                //var errorinfo = Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t);
                //if (errorinfo == null) continue;
                //if
                //    (Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.ContainsKey(t))
                //{
                //    var infogggg = Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary[t];
                lineerrs.Add(new Tuple<int, int>(infogggg.LoopId, infogggg.FaultId)); //41 被盗 42 短路

            }
            foreach (var g in this.ChildTreeItems)
            {
                var str = "";
                var strForeGround = "Black";
                if (g.NoUsed == Visibility.Collapsed )
                {
                    strForeGround = "Black";
                    if (lineerrs.Contains(new Tuple<int, int>(g.NodeId, 42)))
                    {
                        str = " -短路";
                        strForeGround = "Red";
                    }
                    if (lineerrs.Contains(new Tuple<int, int>(g.NodeId, 41)))
                    {
                        str = " -被盗";
                        strForeGround = "Red";
                    }
                    if (idr.ContainsKey(g.NodeId)) g.NodeName = idr[g.NodeId] + str;
                    else g.NodeName = "线路" + g.NodeId + str;
                    //  g.NodeName = g.oriName + str;
                }
                g.ForeGround = strForeGround;
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

        public void ResetCm()
        {
            ObservableCollection<IIMenuItem> t = null;
            if (
                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                    InfoItems.ContainsKey(NodeId))
            {
                var s =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                        InfoItems[NodeId];

                t = MenuBuilding.BulidCm(((int)s.RtuModel).ToString(), false, s);

            }
            this.CmItems = t;
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


        public void UpdateNoUsedShow()
        {
            //foreach (var t in lst)
            //{
            //    if(NodeId==t.Item1)
            //    {
            if (ChildTreeItems.Count > 0) ChildTreeItems.Clear();
            ThisNodeAddPartsNode();
            GetUsedCount();
            //    }
            //}
        }

        public void UpdateWj1090StateAndEquInfomation()
        {
            var runningInfo = Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(NodeId);
            if (runningInfo == null) return;
            var tmp = runningInfo.ErrorCount > 0;
            ImagesIcon = tmp ? Resources.ImageResources.GetEquipmentIcon(RtuModel + 1) : Resources.ImageResources.GetEquipmentIcon(RtuModel);
            //ImagesIcon = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.IsRtuHasError(NodeId) ? Resources.ImageResources.GetEquipmentIcon(RtuModel + 1) : Resources.ImageResources.GetEquipmentIcon(RtuModel);
            UpdateLinesInfoAndError();

            var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(NodeId);//.GetAttachEquipmentInfo(NodeId);
            if (info == null) return;
            //NodeName = info.RtuName;
        }
    }
}
