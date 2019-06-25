using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;

namespace Wlst.Ux.Wj1090Module.Wj1090TreeManageViewModel.ViewModel
{
    public class TreeNodeWj1090ViewModel : TreeNodeBaseViewModel
    {
        public TreeNodeWj1090ViewModel()
        {
             
        }

        public TreeNodeWj1090ViewModel(int rtuId, string rtuName, int attachRtuId):this()
        {
            NodeId = rtuId;
            NodeIds = string.Format("{0:D4}", rtuId) + " -";
            NodeName = rtuName;
            AttachRtuId = attachRtuId;
            GetNoUsedCount();
            GetRtuModel();
            UpdateWj1090StateAndEquInfomation();

        }
        private void GetRtuModel()
        {
           var info= Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetAttachEquipmentInfo(NodeId);
            if(info==null) return;
            RtuModel=info.RtuModel;
            ImagesIcon = Sr.EquipemntLightFault.Services.TmlErrorStates.IsRtuHasError(NodeId) ? Resources.ImageResources.GetEquipmentIcon(RtuModel + 1) : Resources.ImageResources.GetEquipmentIcon(RtuModel);
        }
        private void GetNoUsedCount()
        {
            var info = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(this.NodeId);
            if (info == null) return;
            var lines = info as Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
            if (lines == null) return;
            if (lines.LduLines == null) return;
            ConcentratorCount = 0;
            foreach (var t in lines.LduLines)
            {
                if(!t.IsUsed)
                {
                    ConcentratorCount++;
                }
            }
            ConcentratorCountVisi = ConcentratorCount > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private ObservableCollection<TreeNodeLineViewModel> _collectionWj1090;

        /// <summary>
        /// 开关量输入参数
        /// </summary>


        public ObservableCollection<TreeNodeLineViewModel> CollectionWj1090
        {
            get { return _collectionWj1090 ?? (_collectionWj1090 = new ObservableCollection<TreeNodeLineViewModel>()); }
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
                if(_rtuModel.Equals(value)) return;
                _rtuModel = value;
                RaisePropertyChanged(()=>RtuModel);
            }
        }

        public override void OnNodeSelectActive()
        {
            if(CollectionWj1090.Count>0) CollectionWj1090.Clear();
            ThisNodeAddPartsNode();
            IsExpanded = true;
            CurrentSelectNode = this;

            var args = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
            };
            args.AddParams(NodeId);

            EventPublisher.EventPublish(args);
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
            for (int i = CollectionWj1090.Count - 1; i > -1; i--)
            {
                CollectionWj1090.RemoveAt(i);
            }
        }

        private void ThisNodeAddPartsNode()
        {
            var info = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(this.NodeId);
            if (info == null) return;
            var lines = info as Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
            if (lines == null) return;
            if (lines.LduLines == null) return;

            var errors =
                Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetLstInfoByRtuId(this.NodeId);
            var lineerrs = new List<Tuple<int, int>>();
            foreach (var t in errors)
            {
                var errorinfo = Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetShowInfoById(t);
                if (errorinfo == null) continue;
                if
                    (Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.ContainsKey(t))
                {
                    var infogggg = Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary[t];
                    lineerrs.Add(new Tuple<int, int>(infogggg.LoopId, infogggg.FaultCodeId)); //41 被盗 42 短路
                }
            }

            foreach (var t in lines.LduLines)
            {

                var str = "";
                var strForeGround = "Black";
                if (t.IsUsed)
                {
                    strForeGround = "Black";
                    if (lineerrs.Contains(new Tuple<int, int>(t.LduLineID, 42)))
                    {
                        str = " -短路";
                        strForeGround = "Red";
                    }
                    if (lineerrs.Contains(new Tuple<int, int>(t.LduLineID, 41)))
                    {
                        str = " -被盗";
                        strForeGround = "Red";
                    }
                }

                var infos = new TreeNodeLineViewModel( t.LduLineName + str, t.LduLineID, t.IsUsed,this .NodeId );
                infos.ForeGround = strForeGround;
                CollectionWj1090.Add(infos);
            }

        }


        private void UpdateLinesInfoAndError()
        {
            var info = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(this.NodeId);
            if (info == null) return;
            var lines = info as Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
            if (lines == null) return;
            if (lines.LduLines == null) return;
            var idr = new Dictionary<int, string>();
            foreach (var g in lines.LduLines) if (!idr.ContainsKey(g.LduLineID)) idr.Add(g.LduLineID, g.LduLineName);
           
            var errors =
                Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetLstInfoByRtuId(this.NodeId);
            var lineerrs = new List<Tuple<int, int>>();
            foreach (var t in errors)
            {
                var errorinfo = Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetShowInfoById(t);
                if (errorinfo == null) continue;
                if
                    (Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.ContainsKey(t))
                {
                    var infogggg = Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary[t];
                    lineerrs.Add(new Tuple<int, int>(infogggg.LoopId, infogggg.FaultCodeId)); //41 被盗 42 短路
                }
            }
            foreach (var g in this .CollectionWj1090 )
            {
                var str = "";
                var strForeGround = "Black";
                if (g.NoUsed ==Visibility.Collapsed)
                {
                    strForeGround = "Black";
                    if (lineerrs.Contains(new Tuple<int, int>(g.NodeId , 42)))
                    {
                        str = " -短路";
                        strForeGround = "Red";
                    }
                    if (lineerrs.Contains(new Tuple<int, int>(g.NodeId , 41)))
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
            HelpCmm(ResetCm());
            this.RaisePropertyChanged(() => this.Cm);
        }

        public ObservableCollection<IIMenuItem> ResetCm()
        {
            ObservableCollection<IIMenuItem> t = null;
            if (
                Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.
                    EquipmentInfoDictionary.ContainsKey(NodeId))
            {
                var s =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.
                        EquipmentInfoDictionary[NodeId];

                t = MenuBuilding.BulidCm(s.RtuModel.ToString(), false, s);

            }
            return t;
        }


        protected void HelpCmm(ObservableCollection<IIMenuItem> t)
        {
            Cm.Items.Clear();
            var infos = MenuBuilding.HelpCmm(t);
            foreach (var g in infos) this.Cm.Items.Add(g);

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

    
        public void UpdateNoUsedShow()
        {
            //foreach (var t in lst)
            //{
            //    if(NodeId==t.Item1)
            //    {
                    if (CollectionWj1090.Count > 0) CollectionWj1090.Clear();
                    ThisNodeAddPartsNode();
                    GetNoUsedCount();
            //    }
            //}
        }

        public  void UpdateWj1090StateAndEquInfomation()
        {
            ImagesIcon = Sr.EquipemntLightFault.Services.TmlErrorStates.IsRtuHasError(NodeId) ? Resources.ImageResources.GetEquipmentIcon(RtuModel + 1) : Resources.ImageResources.GetEquipmentIcon(RtuModel);

            UpdateLinesInfoAndError();

            var info = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetAttachEquipmentInfo(NodeId);
            if (info == null) return;
            NodeName  = info.RtuName ;
        }
    }
}
