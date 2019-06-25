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
using Wlst.Ux.EquipemntTree.GrpSingleTabShowViewModel.ViewModels;
using Wlst.Ux.EquipemntTree.Models;
using Wlst.Ux.EquipemntTree.Resources;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EquipemntTree.GrpComSingleMuliViewModel
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

        /// <summary>
        /// 此复选框仅供 全局分组使用  本地分组不可使用  否则会两边都进行选择导致 选中的终端错误
        /// </summary>
        public override void OnNodeChecked()
        {
            GrpSingleTabShowViewModel.ViewModels.TreeSingleViewModel.MySelf.OnNodeChecked(AreaId, this.NodeId,
                                                                                          this, IsChecked);
            base.OnNodeChecked();
        }


        //  protected Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase TerInfo;
        public TreeNodeItemTmlViewModel(TreeNodeBaseNode mvvmFather, Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase terminalInfomation, bool newInArea = false)
        {
            this.NodeType = TypeOfTabTreeNode.IsTml;
            this.NodeColor = "Black";
            //Visi = Visibility.Visible;
            this._father = mvvmFather;
            Md5 = 0;
            this.IsShowChkTree = Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 2, false)
                                     ? Visibility.Visible
                                     : Visibility.Collapsed;
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
                if (newInArea) this.NodeName = "*_" + terminalInfomation.RtuName;
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

                if (paras != null && paras.WjGprs != null)
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


                //lvf AddNodeColor 2018年12月13日14:26:47  
                if (paras != null && paras.WjVoltage != null)
                {
                    if (Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType.Count > 0)
                    {
                        if (
                            Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType.ContainsKey(
                                paras.WjVoltage.RtuUsedType))
                        {
                            NodeColor =
                                Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType[paras.WjVoltage.RtuUsedType].
                                    Item2;
                        }


                    }
                }

            }

            if (!BaseNodes.Nodess.ContainsKey((NodeId)))
            {
                BaseNodes.AddNode(this);

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
            else
            {
                PicIndex = updateArgu;
            }
            return;
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
            this.PhyId = TerInfo.RtuPhyId;

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
            if (TerInfo.EquipmentType == WjParaBase.EquType.Rtu)
            {
                //先判断实际状态是否为停运  如果是，则变图标 lvf 2018年11月9日13:32:48
                var b = TerInfo.RtuRealState;
                if (b == 1)
                {
                    PicIndex = 3002;
                    return;
                }


                var s = TerInfo.RtuStateCode;
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
                var ShieldAList = new Dictionary<int, Tuple<double, double>>();
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
            else if (TerInfo.EquipmentType == WjParaBase.EquType.Slu)
            {
                if (TerInfo.RtuStateCode != 2)
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
                    haserror = runninfo.ErrorCount > 0;
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
                var tmp = runninfo != null && runninfo.ErrorCount > 0; ;
                PicIndex = (int)TerInfo.EquipmentType + (tmp ? 1 : 0);
            }
        }
        /// <summary>
        /// 获取终端故障编号
        /// </summary>
        /// <param name="rtuid"></param>
        /// <returns></returns>
        public static int GetImageIconByState(int rtuid)
        {

            var TerInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuid);
            if (TerInfo == null) return 0;
            var runninfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(rtuid);

            // int modelId = (int) TerInfo.EquipmentType;
            if (TerInfo.EquipmentType == WjParaBase.EquType.Rtu)
            {
                var s = TerInfo.RtuStateCode;
                if (s == 0)
                {
                    return 3001;

                }
                if (s == 1)
                {
                    return 3002;

                }

                var online = runninfo != null && runninfo.IsOnLine;
                if (online == false)
                {
                    return 3003;

                }
                var haserror = false;
                if (UxTreeSetting.IsRutsNotShowError == false)
                    haserror = runninfo.ErrorCount > 0;
                var lighton = runninfo.IsLightHasElectric; // RtuNewDataService.IsRtuHasElectric(this.NodeId);
                int errorindex = 0;
                // var ShieldAList = new Dictionary<int, Tuple<double, double>>();
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

                return 3005 + errorindex;
            }
            else if (TerInfo.EquipmentType == WjParaBase.EquType.Slu)
            {
                if (TerInfo.RtuStateCode != 2)
                {
                    return (int)WjParaBase.EquType.Slu + 2;

                }
                var online = runninfo != null && runninfo.IsOnLine;
                if (online == false)
                {
                    return (int)WjParaBase.EquType.Slu + 3;

                }
                var haserror = false;
                if (UxTreeSetting.IsRutsNotShowError == false)
                    haserror = runninfo.ErrorCount > 0;
                if (haserror)
                {
                    return (int)WjParaBase.EquType.Slu + 1;
                }
                else
                {
                    return (int)WjParaBase.EquType.Slu;
                }

            }
            else
            {
                var tmp = runninfo != null && runninfo.ErrorCount > 0;
                ;
                return (int)TerInfo.EquipmentType + (tmp ? 1 : 0);
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

        public static int TreeSelectedOne = 0;
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
                    EventAttachInfo = NodeId != TreeSelectedOne ? "Tree" : "TreeSelf",//如果点击的是联动查询框中的节点则发送Treeaa，在事件接收处屏蔽相应操作。
                };
                args.AddParams(NodeId);

                EventPublish.PublishEvent(args);
                //lvf  2018年5月22日14:40:48  记录当前点击终端
                Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId = NodeId;

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
                EventAttachInfo = "TreeSelf",
            };
            args.AddParams(NodeId);
            EventPublish.PublishEvent(args);
            //lvf  2018年5月22日14:40:48  记录当前点击终端
            Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId = NodeId;
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
                var fffff = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t);
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


            //添加集中器右击菜单  西安需求  2019年5月23日13:54:21 读取 292.xml_1003 GetOptionIsThisValue

            CmItems = MenuBuilding.BulidCm(((int)TerInfo.RtuModel).ToString(), false, TerInfo); ;

            //不是默认值，则要加载集中器菜单
            if (Wlst.Cr.CoreMims.SystemOption.GetOptionIsDefaults(1003, 0) == true) return;

            var sluid = EquipmentDataInfoHold.GetSluIdByRtuId(TerInfo.RtuPhyId);
            if (sluid > 0)
            {

                var TerInfoSlu = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(sluid);
                if (TerInfoSlu != null)
                {
                    var SluItems = MenuBuilding.BulidCm(((int)TerInfoSlu.RtuModel).ToString(), false, TerInfoSlu);


                    var menuItemFile = new MenuItemBase()
                    { IsCheckable = false, IsEnabled = true, Id = 195959, Visibility = Visibility.Visible };

                    menuItemFile.Text = TerInfoSlu.RtuName;
                    menuItemFile.TextTmp = TerInfoSlu.RtuName;
                    foreach (var f in SluItems)
                        menuItemFile.CmItems.Add(f);

                    CmItems.Add(menuItemFile);
                }
            }
        }






        #endregion


    }
}
