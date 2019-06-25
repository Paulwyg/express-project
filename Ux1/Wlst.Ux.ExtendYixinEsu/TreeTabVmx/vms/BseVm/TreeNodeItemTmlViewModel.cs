using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using EventIdAssign = Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign;

namespace Wlst.Ux.ExtendYixinEsu.TreeTabVmx.vms.BseVm
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

        private int modelx = 0;
        public TreeNodeItemTmlViewModel(TreeNodeBaseNode mvvmFather, Wlst .Sr .EquipmentInfoHolding .Model .WjParaBase  terminalInfomation)
        {
            this.NodeType = TypeOfTabTreeNode.IsTml;
            Visi = Visibility.Visible;
            this._father =  mvvmFather;
            Md5 = 0;

            //TreeSingleViewModel.RegisterNodeToControl(this);

            if (terminalInfomation == null)
            {
                this.NodeName = "加载出错";
            }
            else
            {
                modelx = (int )terminalInfomation.RtuModel;
                this.NodeName = terminalInfomation.RtuName;
                // this.ImagesIcon = ImageResources.GetEquipmentIcon(1010198);
                this.NodeId = terminalInfomation.RtuId;
                Md5 = terminalInfomation.DateUpdate ;
                UpdateTmlStateInfomation();
                PhyId = terminalInfomation.RtuPhyId ;

         
                    var tmps =terminalInfomation as Wlst .Sr .EquipmentInfoHolding .Model .Wj3005Rtu ;
                    if(tmps !=null && tmps .WjGprs !=null )
                    {
                        PhoneNumber = tmps.WjGprs .MobileNo ;
                    }
                
               
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
                            if(_currentSelectedTreeNode .CmItems !=null )
                            _currentSelectedTreeNode.CmItems .Clear();
                            _currentSelectedTreeNode.IsSelected = false;
                           
                        }
                    }
                    _currentSelectedTreeNode = value; 
                    BaseNodes.CurrentSelectedGrpIdChanged(value);
                }
            }
        }

        //private static  void MenuItemCleanUp(ContextMenu cm)
        //{
        //    try
        //    {
        //        if (cm == null) return;
        //        for (int i = 0; i < cm.Items.Count; i++)
        //        {
        //            var tmp = cm.Items[i] as MenuItem;
        //            if (tmp != null)
        //                MenuItemCleanUp(tmp);
        //            tmp = null;
        //            cm.Items[i] = null;
        //        }
        //        cm.Items.Clear();
        //    }
        //    catch (Exception ex ){}
        //}

        //private static  void MenuItemCleanUp(MenuItem   cm)
        //{
        //    if (cm == null) return;
        //    for (int i = 0; i < cm.Items.Count;i++ )
        //    {
        //        var tmp = cm .Items [i] as MenuItem;
        //        if (tmp != null)
        //            MenuItemCleanUp(tmp);
        //        if (tmp == null) return;
        //        tmp.Icon = null;

        //        tmp = null;
        //        cm.Items[i] = null;
        //    }
        //}

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
            if (EquipmentDataInfoHold.InfoItems .ContainsKey(NodeId))
            {
                var s =
                    EquipmentDataInfoHold.InfoItems [
                        NodeId];
                if (s.DateUpdate  == this.Md5) return;
                UpdateTerminalInfo(s);
            }
            return;

        }

        /// <summary>
        /// 根据发布的信息更新终端树状态信息
        /// </summary>
        /// <param name="basicTmlInfomation"></param>
        private void UpdateTerminalInfo(Wlst.Sr .EquipmentInfoHolding .Model .WjParaBase  basicTmlInfomation)
        {
            NodeName = basicTmlInfomation.RtuName;
            this.Md5 = basicTmlInfomation.DateUpdate ;


            var tmps = basicTmlInfomation as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if (tmps != null && tmps .WjGprs !=null )
                {
                    PhoneNumber = tmps.WjGprs .MobileNo ;
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
            var runninginfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(this.NodeId);

            if (
                EquipmentDataInfoHold.InfoItems .ContainsKey(
                    NodeId))
            {
                var equ =
                    EquipmentDataInfoHold.InfoItems [
                        NodeId];
                if (equ.EquipmentType ==WjParaBase.EquType.Rtu )
                {
                    var s = equ.RtuStateCode ;
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
                    var online = runninginfo != null && runninginfo.IsOnLine;
                        
                    if (online == false)
                    {
                        PicIndex = 3003;
                        return;
                    }
                    var haserror = runninginfo.ErrorCount > 0; 
                    var lighton =runninginfo .IsLightHasElectric ;
                    int errorindex = 0;
                    if (haserror && lighton) errorindex = 3;
                    if (haserror && !lighton) errorindex = 1;
                    if (!haserror && lighton) errorindex = 2;

                    PicIndex = 3005 + errorindex;
                }
                else if (equ.EquipmentType  == WjParaBase.EquType.Slu )
                {
                    if (equ.RtuStateCode  != 2)
                    {
                        PicIndex = (int )equ.RtuModel + 2;
                        return;
                    }
                    var online = runninginfo != null && runninginfo.IsOnLine;
                    if (online == false)
                    {
                        PicIndex = (int)equ.RtuModel + 3;
                        return;
                    }
                    var haserror = runninginfo.ErrorCount > 0; 
                    if (haserror)
                    {
                        PicIndex = (int)equ.RtuModel + 1;
                    }
                    else
                    {
                        PicIndex = (int)equ.RtuModel;
                    }

                }
                else
                {
                    var tmp = runninginfo != null && runninginfo.ErrorCount > 0; 
                    PicIndex = (int)equ.RtuModel + (tmp ? 1 : 0);
                }

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

        /// <summary>
        /// 当选择的终端发送变化时，如果 
        /// </summary>
        public override void OnNodeSelectActive()
        {
            //base.OnNodeSelect();
            //发布事件  选中当前节点
            var args = new PublishEventArgs
                           {
                               EventType = PublishEventType.Core,
                               EventId = EventIdAssign.EquipmentSelected,
                           };
            args.AddParams(NodeId);
            EventPublisher.EventPublish(args);


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
                EventId = EventIdAssign.EquipmentSelected,
            };
            args.AddParams(NodeId);
            EventPublisher.EventPublish(args);
        }

        //private void ThisNodeAddLoopsNode()
        //{
        //    if (!UxTreeSetting.IsShowGrpInTreeModelShowTmlChildNode) return;
        //    if (
        //        !ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey
        //             (
        //                 this.NodeId))
        //        return;

        //    var iiswitchout =
        //        ServicesEquipemntInfoHold.EquipmentInfoDictionary[NodeId]

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
            if (!EquipmentDataInfoHold.InfoItems .ContainsKey(this.NodeId)) return;
            var tm  =EquipmentDataInfoHold.GetInfoById( this.NodeId);
            if(tm !=null )

            foreach (var t in tm .EquipmentsThatAttachToThisRtu )
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
            ResetCm();
        }

        private void  ResetCm()
        {
            ObservableCollection<IIMenuItem> t = null;
            if (EquipmentDataInfoHold .InfoItems .ContainsKey(NodeId))
            {
                t = MenuBuilding.BulidCm(modelx.ToString(), false, EquipmentDataInfoHold.InfoItems[NodeId ]);

            }
            CmItems = t;
        }

        #endregion


    }
}
