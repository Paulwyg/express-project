using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;


using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.LdEquipemntTree.GrpSingleTabShowViewModel.ViewModels;
using Wlst.Ux.LdEquipemntTree.Models;
using Wlst.Ux.LdEquipemntTree.Resources;

namespace Wlst.Ux.LdEquipemntTree.GrpComSingleMuliViewModel
{
    public class TreeNodeItemTmlViewModel : TreeNodeBaseNode
    {
       // private TreeNodeBaseNode tn = new TreeNodeBaseNode();

        public TreeNodeItemTmlViewModel()
        {
            this.NodeType = TypeOfTabTreeNode.IsTml;
            //Visi = Visibility.Visible;
            Md5 = 0;
        }


        public static Dictionary<int, List<WeakReference>> RtuItems = new Dictionary<int, List<WeakReference>>();

      //  protected Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase TerInfo;
        public TreeNodeItemTmlViewModel(TreeNodeBaseNode mvvmFather, Wlst .Sr .EquipmentInfoHolding .Model .WjParaBase  terminalInfomation,bool newInArea=false )
        {
            this.NodeType = TypeOfTabTreeNode.IsTml;
            //Visi = Visibility.Visible;
            this._father =  mvvmFather;
            Md5 = 0;
            //TerInfo = terminalInfomation;

            if (terminalInfomation != null)
            {

                WeakReference refs = new WeakReference(this);
                if (RtuItems.ContainsKey(terminalInfomation.RtuId) == false)
                    RtuItems.Add(terminalInfomation.RtuId, new List<WeakReference>());
                RtuItems[terminalInfomation.RtuId].Add(refs);
            }

            //TreeSingleViewModel.RegisterNodeToControl(this);

            if (terminalInfomation == null)
            {
                this.NodeName = "加载出错";
            }
            else
            {
                if (newInArea) this.NodeName ="*_"+ terminalInfomation.RtuName;
                    else 
                this.NodeName = terminalInfomation.RtuName;
                // this.ImagesIcon = ImageResources.GetEquipmentIcon(1010198);
                this.NodeId = terminalInfomation.RtuId;
                this.RtuOnly = terminalInfomation.Idf;
                //this.RtuOnlyCode = terminalInfomation.Idf;
                Md5 = terminalInfomation.DateUpdate;
                UpdateTmlStateInfomation();
                PhyId = terminalInfomation.RtuPhyId;
                TmlState = PicIndex;
                RtuInstallAddr = terminalInfomation.RtuInstallAddr;
                var paras = terminalInfomation as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

                if (paras != null && paras.WjGprs!=null)
                {
                    PhoneNumber = paras.WjGprs.MobileNo;
                    IpAddr = new System.Net.IPAddress(BitConverter.GetBytes(paras.WjGprs.StaticIp)).ToString();
                }
                #region Attach
                //var attachToRtu = terminalInfomation.EquipmentsThatAttachToThisRtu;
                //if(attachToRtu ==null) return;
                //foreach(var f in attachToRtu)
                //{
                //    var nodeInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                //    string nodeName = nodeInfo.RtuName;

                //    if (f < 1199999)
                //        this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(this, AreaId, 0,
                //                                                                     TypeOfTabTreeNode.IsTmlParts) { NodeId = f, NodeName = nodeName + "-防盗", ImagesIcon = ImageResources.LduIcon });
                //    else if (f < 1299999)
                //        this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(this, AreaId, 0,
                //                                                                     TypeOfTabTreeNode.IsTmlParts)
                //                                    {
                //                                        NodeId = f,
                //                                        NodeName = nodeName + "- 节电",
                //                                        ImagesIcon = ImageResources.EsuIcon
                //                                    });
                //    else if(f<1399999)
                //        this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(this, AreaId, 0,
                //                                                                         TypeOfTabTreeNode.IsTmlParts)
                //        {
                //            NodeId = f,
                //            NodeName = nodeName + "- 电表",
                //            ImagesIcon = ImageResources.MruIcon
                //        });
                //    else if (f < 1499999)
                //        this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(this, AreaId, 0,
                //                                                                         TypeOfTabTreeNode.IsTmlParts)
                //        {
                //            NodeId = f,
                //            NodeName = nodeName + "- 光控",
                //            ImagesIcon = ImageResources.LuxIcon
                //        });
                //    else if (f < 1599999)
                //        this.ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(this, AreaId, 0,
                //                                                                         TypeOfTabTreeNode.IsTmlParts)
                //        {
                //            NodeId = f,
                //            NodeName = nodeName + "- 单灯",
                //            ImagesIcon = ImageResources.SluIcon 
                //        });
                //}
                #endregion

            }

            if (!BaseNodes.Nodess.ContainsKey((NodeId)))
            {
                BaseNodes.AddNode(  this);

            }
            else
            {
                //Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(NodeId, "0000", OperatrType.SystemInfo, "Error");
            }
        }


        ///// <summary>
        ///// 当前终端被选中
        ///// </summary>
        //public new bool IsSelected
        //{
        //    get { return base.IsSelected; }
        //    set
        //    {
        //        base.IsSelected = value;
        //        ResetContextMenu();
        //    }
        //}

        #region

       // public string PhoneNumber = "";
        private static TreeNodeBaseNode _currentSelectedTreeNode;

        public static TreeNodeBaseNode CurrentSelectedTreeNode
        {
            get { return _currentSelectedTreeNode; }
            set
            {
                if (_currentSelectedTreeNode != value)
                {
                    if (_currentSelectedTreeNode != null)
                    {
                        if (_currentSelectedTreeNode.NodeType == TypeOfTabTreeNode.IsTml)
                        {
                            _currentSelectedTreeNode.CleanChildren();
                          
                         //   MenuItemCleanUp(_currentSelectedTreeNode.Cm);  
                            _currentSelectedTreeNode.CmItems.Clear();
                            _currentSelectedTreeNode.IsSelected = false;

                        }
                    }
                    _currentSelectedTreeNode = value;
                }
            }
        }

      
        #endregion

        #region  Update infomation

        /// <summary>
        /// 当分组信息发生变化的时候  增量式重新加载节点
        /// One Update terminal  informaiton
        /// Two Update terminal Running information
        /// </summary>
        public override void ReUpdate(int updateArgu)
        {
            if (updateArgu == 1)
            {
                this.UpdateTmlInfomation();
                UpdateTmlStateInfomation();
            }
            if (updateArgu == 2)
            {
                this.UpdateTmlStateInfomation();
            }
        }

        /// <summary>
        /// event update
        /// </summary>
        private void UpdateTmlInfomation()
        {
            var rtu = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(NodeId);
            if (rtu == null) return;
            if (rtu.DateUpdate == this.Md5) return;
            UpdateTerminalInfo();

            return;

        }

        /// <summary>
        /// 根据发布的信息更新终端树状态信息
        /// </summary>
        private void UpdateTerminalInfo()
        {
            var TerInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(NodeId);
            if (TerInfo == null) return;
            NodeName = TerInfo.RtuName;
            this.Md5 = TerInfo.DateUpdate;
            this.RtuOnly = TerInfo.Idf;
            this.IpAddr = TerInfo.RtuInstallAddr;
            
            var tmps = TerInfo as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

            if (tmps != null)
            {
                PhoneNumber = tmps.WjGprs.MobileNo;
            }

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

        private void UpdateTmlStateInfomation()
        {
            var TerInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(NodeId);
            if (TerInfo == null) return;
            var runninfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(this.NodeId);
            
           // int modelId = (int) TerInfo.EquipmentType;
                if (TerInfo .EquipmentType  == WjParaBase.EquType.Rtu )
                {
                    var s = TerInfo.RtuStateCode ;
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
                    
                    var online = runninfo != null && runninfo.IsOnLine;
                    if (online == false)
                    {
                        PicIndex = 3003;
                        return;
                    }
                    var haserror = false;
                    if (UxTreeSetting.IsRutsNotShowError == false)
                        haserror = runninfo.ErrorCount > 0;
                    var lighton = runninfo.IsLightHasElectric;// RtuNewDataService.IsRtuHasElectric(this.NodeId);
                    int errorindex = 0;                    
                    var ShieldAList = new Dictionary<int, Tuple<double,double>>();
                    if (haserror && lighton) errorindex = 3;
                    if (haserror && !lighton) errorindex = 1;
                    if (!haserror && lighton)
                    {
                        //foreach(var t in runninfo.RtuNewData.LstNewLoopsData   )
                        //{
                        //    ShieldAList.Add(t.LoopId, new Tuple<double, double>(t.A, t.ShieldLittleA));
                        //}
                        //foreach(var t in ShieldAList )
                        //{
                        //    int count = 0;
                        //    if (t.Value.Item1 < t.Value.Item2 || t.Value.Item1 == 0.0) count++;
                        //}
                        
                        errorindex = 2;
                    }
                    if (!haserror && !lighton) errorindex = 0;

                    PicIndex = 3005 + errorindex;
                }
                else if (TerInfo .EquipmentType  == WjParaBase.EquType.Slu )
                {
                    if (TerInfo .RtuStateCode  != 2)
                    {
                        PicIndex = (int)WjParaBase.EquType.Slu + 2;
                        return;
                    }
                    var online = runninfo != null && runninfo.IsOnLine;
                    if (online == false)
                    {
                        PicIndex = (int)WjParaBase.EquType.Slu + 3;
                        return;
                    }
                    var haserror = false;
                    if (UxTreeSetting.IsRutsNotShowError == false)
                        haserror =   runninfo.ErrorCount >0;
                    if (haserror)
                    {
                        PicIndex = (int)WjParaBase.EquType.Slu + 1;
                    }
                    else
                    {
                        PicIndex = (int)WjParaBase.EquType.Slu;
                    }

                }
                else
                {
                    var tmp = runninfo != null && runninfo.ErrorCount >0;;
                    PicIndex = (int)TerInfo .EquipmentType  + (tmp ? 1 : 0);
                }

            

        }

        //private void UpdateTerminalStateInfo(TerminalRunningInfomation basicTmlInfomation)
        //{
        //    //base.NodeModel = basicTmlInfomation.Model;
        //    if (!basicTmlInfomation.IsConnected)
        //    {
        //        ForeGround = "#FFA9A9A9";
        //        BackGround = "#FFFF1493";
        //        ImagesIcon = ImageResources.GetTmlTreeIcon(1);
        //    }
        //    else
        //    {
        //        ForeGround = "#FF000";
        //        //终端图标代码 1 连接断开 0关灯正常 2关灯故障  3 开灯正常 4开灯故障
        //        ImagesIcon = ImageResources.GetTmlTreeIcon(basicTmlInfomation.ImageCode);
        //    }
        //}

        #endregion

        /// <summary>
        /// 加载节点，第一次使用
        /// </summary>
        public override void AddChild()
        {
            ChildTreeItems.Clear();
        }

        #region Node Select

        internal bool IsSelectedByCode = false;
        /// <summary>
        /// 当选择的终端发送变化时，如果 
        /// </summary>
        public override void OnNodeSelectActive()
        {
            if (IsSelectedByCode == false)
            {
                //base.OnNodeSelect();
                //发布事件  选中当前节点
                var args = new PublishEventArgs
                               {
                                   EventType = PublishEventType.Core,
                                   EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                               };
                args.AddParams(NodeId);
                EventPublish.PublishEvent(args);
            }

            if (this.ChildTreeItems.Count > 0) this.ChildTreeItems.Clear();
            if (UxTreeSetting.IsShowGrpInTreeModelShowTmlChildNode)
            {
                //  this.ThisNodeAddLoopsNode();
                this.ThisNodeAddPartsNode();
                this.IsExpanded = true;
            }
            
            TreeNodeItemTmlViewModel.CurrentSelectedTreeNode = this;

            //ResetContextMenu();
        }

        public override void OnDoubleClick()
        {
            base.OnDoubleClick();

            //发布事件  选中当前节点
            var args = new PublishEventArgs
            {
                EventType = PublishEventType.Core,
                EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
            };
            args.AddParams(NodeId);
            EventPublish.PublishEvent(args);
        }

        //private void ThisNodeAddLoopsNode()
        //{
        //    if (!UxTreeSetting.IsShowGrpInTreeModelShowTmlChildNode) return;
        //    if (
        //        !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey
        //             (
        //                 this.NodeId))
        //        return;

        //    var iiswitchout =
        //        Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[NodeId]

        //        as IISwitchOut;
        //    if (iiswitchout == null) return;
        //    if (iiswitchout.SwitchOut == null) return;
        //    foreach (var t in iiswitchout.SwitchOut.GetAllRtuParaSwitchOut())
        //    {
        //        TerminalPartsInfomation tp = new TerminalPartsInfomation();
        //        tp.Id = t.SwitchOutId;
        //        // tp.ImagesIcon = ImageResources.GetTmlTreeIcon(3);
        //        tp.Name = string.IsNullOrEmpty(t.SwichtOutName) ? "K" + tp.Id : t.SwichtOutName;
        //        tp.RightMenuKey = "K" + tp.Id + "Key";
        //        //this.ChildTreeItems.Add(new SingleTreeNodeViewItemViewModel(this, tp));//todo
        //    }
        //}

        private void ThisNodeAddPartsNode()
        {
            if (!UxTreeSetting.IsShowGrpInTreeModelShowTmlChildNode) return;
            var TerInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(NodeId);
            if (TerInfo == null) return;
            var lstatt = TerInfo.EquipmentsThatAttachToThisRtu;


            foreach (var t in lstatt)
            {
                var fffff = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( t);
                if (fffff != null)
                {
                    this.ChildTreeItems.Add(new TreeNodeItemAttachEquViewModel(this, fffff));

                }
            }

        }

        #endregion

        #region  Reset ContextMenu

        public override void ResetContextMenu()
        {
            UpdateCm(NodeId);
        }



        public void UpdateCm(int rtuId)
        {
            var TerInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(NodeId);
            if (TerInfo == null) return;

                CmItems = MenuBuilding.BulidCm( ((int )TerInfo .RtuModel).ToString(), false, TerInfo ); ;
            
        }

        #endregion


    }
}
