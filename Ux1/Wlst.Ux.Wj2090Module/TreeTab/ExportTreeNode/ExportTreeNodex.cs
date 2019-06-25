using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.CoreInterface;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Services;


namespace Wlst.Ux.Wj2090Module.TreeTab.ExportTreeNode
{
    [Export(typeof (IITreeNodeLoadExport))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ExportTreeNodex : IITreeNodeLoadExport
    {
        private List<int> _rtumodels = null;

        public List<int> RtuModes
        {
            get
            {
                if (_rtumodels == null)
                {
                    _rtumodels = new List<int>();
                    _rtumodels.Add(2090);
                }
                return _rtumodels;

            }
        }

        public ObservableCollection<TreeNodeBaseViewModel> GetTreeNodeInfo(int rtuId)
        {
            var ntg = LoadSluInfo(rtuId);
            var rtn = new ObservableCollection<TreeNodeBaseViewModel>();
            foreach (var f in ntg) rtn.Add(f);
            return rtn;
        }


        private ObservableCollection<TreeNodeSlu> LoadSluInfo(int NodeId)
        {

            var sluinfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( NodeId);
            if (NodeId < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluStart ||
                NodeId > Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluEnd)
                return new ObservableCollection<TreeNodeSlu>();

            var childTreeItems = new ObservableCollection<TreeNodeSlu>();


            var sluPara = sluinfo as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
            if (sluPara == null) return new ObservableCollection<TreeNodeSlu>(); 
            
            
            var concolls = new List<int>();
          
                var lst = (from t in sluPara .WjSluCtrlGrps .Values  orderby t.GrpId ascending select t).ToList();


                foreach (var g in lst)
                {

                    if (g.CtrlPhyLst.Count == 0) return new ObservableCollection<TreeNodeSlu>();
                    childTreeItems.Add(new TreeNodeSlu(g.GrpId, NodeTypeEnmu.ConGrp, NodeId));
                    concolls.AddRange(g.CtrlPhyLst);

                }

 
            var lstcons = (from t in sluPara .WjSluCtrls  select t.Value.CtrlPhyId).ToList();

            var spe = new List<int>();
            foreach (var g in lstcons)
            {
                if (!concolls.Contains(g)) spe.Add(g);
            }
            if (Set.Wj2090TreeSetLoad.Myself.IsShowConOnNodeSelected)
                if (spe.Count > 0)
                {
                    var nts = new TreeNodeSlu(0, NodeTypeEnmu.ConGrpNoneGrp, NodeId)
                                  {NodeIds = "000", NodeName = "未分组控制器"};
                    foreach (var g in spe)
                    {
                        nts.ChildTreeItems.Add(new TreeNodeSlu(g, NodeTypeEnmu.ConNode,  NodeId));
                    }
                    nts.NodeName = "未分组控制器 [" + nts.ChildTreeItems.Count + "个]";
                    childTreeItems.Add(nts);

                }

            for (int i = childTreeItems.Count - 1; i >= 0; i--)
            {
                if (childTreeItems[i].ChildTreeItems.Count == 0) childTreeItems.RemoveAt(i);
            }
            LoadConImageSatesBySlu(childTreeItems, NodeId);
            return childTreeItems;
        }


        public void LoadConImageSatesBySlu(ObservableCollection<TreeNodeSlu> ChildTreeItems, int sluId)
        {
            var runninfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(sluId);
            if (runninfo == null || runninfo.SluNewData == null) return;
            var datax =
                (from t in runninfo .SluCtrlNewData    select t ).ToList();
            var dirc = new Dictionary<int, Tuple<bool, bool, bool>>();
            // 在线  开灯  故障


            var xdf = Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultLstInfoByRtuId(sluId);
            var errx = (from t in xdf select t.LoopId).ToList();


            foreach (var f in datax)
            {

                if (dirc.ContainsKey(f.Key)) dirc.Remove(f.Key);

                if (f.Value.Data5 == null)
                {
                    dirc.Add(f.Key, new Tuple<bool, bool, bool>(true, false, errx.Contains(f.Key)));
                    continue;
                }
                var dx = f.Value.Data5;
                bool isOn = false;


                bool allbigger100 = true;
                foreach (var fx in dx.Items)
                {
                    if (fx.StateWorkingOn == 0) isOn = true;
                    if (fx.DateCtrlCreate < 100) allbigger100 = false;

                }

                bool isonline = dx.Info.Status != 3;
                if (allbigger100 == false) isonline = false;


                dirc.Add(f.Key, new Tuple<bool, bool, bool>(isonline, isOn, errx.Contains(f.Key)));
            }
            foreach (var fx in ChildTreeItems)
            {
                if (fx.ChildTreeItems.Count > 0)
                    foreach (var f in fx.ChildTreeItems)
                    {
                        if (f.NodeType == NodeTypeEnmu.ConNode)
                        {

                            //lvf 2018年5月11日12:59:17  改为新图标
                            int lampnum = 1;
                            if (f.LightCount > 1) lampnum = 2;
                            int errorIndex = 2090 * 1000 + lampnum * 100 ;
                                

                            var idx = f.CtrlId;
                            if (!dirc.ContainsKey(idx)) continue;

                            var tux = dirc[idx];
                            if (tux.Item1 == false)
                            {
                                f.ImagesIcon = Services.ImageResources.GetEquipmentIcon(errorIndex);//2090100
                                continue;
                            }
                            int x = 0;
                            // 在线  开灯  故障
                            if (tux.Item2 == false && tux.Item3 == false)
                            {
                                x = 1; //关灯 无故障 
                            }
                            if (tux.Item2 == false && tux.Item3)
                            {
                                x = 2; //关灯 故障
                            }
                            if (tux.Item2 && tux.Item3 == false)
                            {
                                x = 3; //开灯 无故障
                            }
                            if (tux.Item2 && tux.Item3)
                            {
                                x = 4; //开灯 故障
                            }


                            errorIndex = 2090 * 1000 + lampnum * 100 + x;
                            f.ImagesIcon = Services.ImageResources.GetEquipmentIcon(errorIndex);


                            // this.ImagesIcon = Services.ImageResources.GetEquipmentIcon(20901);
                        }
                    }
            }

        }

    }






    public  class TreeNodeSlu : TreeNodeBaseViewModel
    {
        private string _foreExtendSerachContenGround;

        /// <summary>
        /// 
        /// </summary>
        public string ExtendSerachConten
        {
            get { return _foreExtendSerachContenGround; }
            set
            {
                if (value == _foreExtendSerachContenGround) return;
                _foreExtendSerachContenGround = value;
                this.RaisePropertyChanged(() => this.ExtendSerachConten);
            }
        }

        public NodeTypeEnmu NodeType;
    

        protected bool IsGprs = false;
        public int CtrlId = 0;
        protected int Sluid = 0;
        public TreeNodeSlu(int nodeId, NodeTypeEnmu nodetype,int sluNode)
        {
            Sluid = sluNode;
            this.NodeId = nodeId;
            NodeType = nodetype;
          


          
            if (nodetype == NodeTypeEnmu.ConGrp)
            {
                LoadConGrpInfo();
                this.ImagesIcon = Services.ImageResources.GroupIcon;
            }
            if (nodetype == NodeTypeEnmu.ConNode )
            {
                LoadConNodeInfo();
                
            }
            

            if (nodetype == NodeTypeEnmu.ConGrpSpecial1 || nodetype == NodeTypeEnmu.ConGrpSpecial2 || nodetype == NodeTypeEnmu.ConGrpNoneGrp)
            {
                this.ImagesIcon = Services.ImageResources.GroupIcon;
            }
        }

    
        private void LoadConGrpInfo()
        {
            if (NodeType != NodeTypeEnmu.ConGrp)
            {
                return;
            }
            NodeIds = string.Format("{0:D3}", NodeId);

 

            var sluinfo =
                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( Sluid);

            var cons = sluinfo as  Wlst .Sr .EquipmentInfoHolding .Model .Wj2090Slu ;
            if (cons == null) return;
            var lstcons = (from t in cons.WjSluCtrls  select t.Value.CtrlPhyId).ToList();



            if (Set.Wj2090TreeSetLoad.Myself.IsShowConOnNodeSelected)
                foreach (var g in cons .WjSluCtrlGrps .Values )
                {
                    if (g.GrpId == NodeId)
                    {
                        NodeName = g.GrpName;
                        this.ChildTreeItems.Clear();
                        foreach (var f in g.CtrlPhyLst)
                        {
                            if (!lstcons.Contains(f)) continue;
                            ChildTreeItems.Add(new TreeNodeSlu(f, NodeTypeEnmu.ConNode, Sluid));
                        }
                        NodeName = g.GrpName + " [" + this.ChildTreeItems.Count + "个]";
                        break;
                    }
                }

        }

        private void LoadConNodeInfo() //l
        {
            if (NodeType != NodeTypeEnmu.ConNode) return;

            NodeIds = string.Format("{0:D3}", NodeId);
            var nodename = "-" + "控制器";


            var sluinfo =
                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( Sluid);
            var cons = sluinfo as  Wlst .Sr .EquipmentInfoHolding .Model .Wj2090Slu ;
            if (cons == null) return;
            int lampnum = 1;
            foreach (var g in cons.WjSluCtrls )
            {
                if (g.Value.CtrlPhyId == NodeId)
                {
                    nodename = "-" + g.Value.LampCode;
                    if (string.IsNullOrEmpty(NodeName))
                    {
                        nodename = "控制器-" + NodeId;
                    }
                    //NodeName = nodename;
                    CtrlId = g.Value.CtrlId; //转换为逻辑地址
                    //lvf 2018年5月11日14:34:32  新图标机制 带灯头
                    if (g.Value.LightCount > 1) lampnum = 2;
                    break;
                }
            }
            NodeName = nodename;

            
            int errorIndex = 2090 * 1000 + lampnum * 100;
            this.ImagesIcon = Services.ImageResources.GetEquipmentIcon(errorIndex);//20901
        }


       
        private ObservableCollection<TreeNodeSlu> _collectionWj2090;

        /// <summary>
        /// 开关量输入参数
        /// </summary>

        public ObservableCollection<TreeNodeSlu> ChildTreeItems
        {
            get
            {
                if (_collectionWj2090 == null)
                    _collectionWj2090 = new ObservableCollection<TreeNodeSlu>();
                return _collectionWj2090;
            }
        }


        public override void OnNodeSelectActive()
        {
         
            if (NodeType == NodeTypeEnmu.ConNode)
            {
               
                    var args = new PublishEventArgs
                    {
                        EventType = PublishEventType.Core,
                        EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                    };
                    args.AddParams(Sluid );
                    args.AddParams(NodeId);

                    EventPublish.PublishEvent(args);
                    ResetContextMenu();
                

            }
            if (NodeType == NodeTypeEnmu.ConGrp || NodeType == NodeTypeEnmu.ConGrpSpecial1 ||
                NodeType == NodeTypeEnmu.ConGrpSpecial2)
            {
                ResetContextMenu();
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
                    if (!Wj2090Module.TreeTab.Set.Wj2090TreeSetLoad.Myself.IsShowGrpInTreeModelShowId)
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
            if (NodeType == NodeTypeEnmu.ConNode)
                HelpCmmSluNode(ResetCm());
            if (NodeType == NodeTypeEnmu.ConGrp || NodeType == NodeTypeEnmu.ConGrpSpecial1 ||
                NodeType == NodeTypeEnmu.ConGrpSpecial2)
            {
                HelcCmmConNode();
            }

            this.RaisePropertyChanged(() => this.Cm);
        }


        private ContextMenu cm;

        public ContextMenu Cm
        {
            get
            {
                if (cm == null)
                {
                    cm = new ContextMenu() {BorderThickness = new Thickness(0)};
                }
                return cm;
            }
            set
            {
                if (cm == value) return;
                cm = value;
                this.RaisePropertyChanged(() => this.Cm);
            }
        }


        protected void HelpCmmSluNode(ObservableCollection<IIMenuItem> t)
        {
            var ggggg = Wlst.Sr.Menu.Services.MenuBuilding.HelpCmm(t);
            Cm.Items.Clear();
            foreach (var gggggggg in ggggg)
            {
                Cm.Items.Add(gggggggg);
            }
            return;
        }

        private ObservableCollection<IIMenuItem> ResetCm()
        {
            ObservableCollection<IIMenuItem> t = null;

            t = MenuBuilding.BulidCm("20900", false, new Tuple<int, int>(Sluid, NodeId));

            
            return t;
        }

        #endregion

        protected void HelcCmmConNode()
        {
            if (Cm != null && Cm.Items.Count > 0) return;
            if (NodeType == NodeTypeEnmu.ConGrp || NodeType == NodeTypeEnmu.ConGrpSpecial1 ||
                NodeType == NodeTypeEnmu.ConGrpSpecial2)
            {

            }
            else return;

            Cm.Items.Clear();
            try
            {
                var fis = new MenuItem()
                {

                    ToolTip = "混合控制",
                    Header = "混合控制"
                };
                fis.Items.Add(RightOper[0]);
                fis.Items.Add(RightOper[1]);
                //  fis.Items.Add(RightOper[2]);
                fis.Items.Add(RightOper[2]);

                var fiss = new MenuItem()
                {

                    ToolTip = "节能调光",
                    Header = "节能调光"
                };
                fiss.Items.Add(RightOper[3]);
                fiss.Items.Add(RightOper[4]);
                fiss.Items.Add(RightOper[5]);
                fiss.Items.Add(RightOper[6]);
                fiss.Items.Add(RightOper[7]);
                fiss.Items.Add(RightOper[8]);
                fiss.Items.Add(RightOper[9]);
                fiss.Items.Add(RightOper[10]);
                fiss.Items.Add(RightOper[11]);
                fiss.Items.Add(RightOper[12]);
                fiss.Items.Add(RightOper[13]);
                Cm.Items.Add(fis);
                Cm.Items.Add(fiss);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        #region RightOper


        private MenuItem[] _DRightOper = null;

        private string[] titles = new string[14] { "开灯", "调档节能", "关灯", "0%调光", "10%调光", "20%调光", "30%调光", "40%调光", "50%调光", "60%调光", "70%调光", "80%调光", "90%调光", "100%调光" };// "0%调光", "10%调光", "20%调光", "30%调光", "40%调光", "50%调光",

        public MenuItem[] RightOper
        {
            get
            {
                if (_DRightOper == null)
                {
                    _DRightOper = new MenuItem[14];
                    for (int i = 1; i < 15; i++)
                    {
                       
                        _DRightOper[i - 1] = new MenuItem()
                                                 {

                                                     ToolTip = titles[i - 1],
                                                     Header = titles[i - 1],
                                                     CommandParameter = i,
                                                     Command =
                                                         new RelayCommand<int>(ExRightOper, CanExRightOper, true),
                                                 };

                    }


                }
                return _DRightOper;
            }
        }

        private void ExRightOper(int x)
        {
            if (NodeType == NodeTypeEnmu.ConGrp || NodeType == NodeTypeEnmu.ConGrpSpecial1 ||
                NodeType == NodeTypeEnmu.ConGrpSpecial2)
            {

                int grpid = this.NodeId;
                if (NodeType == NodeTypeEnmu.ConGrpSpecial1) grpid = -2;
                if (NodeType == NodeTypeEnmu.ConGrpSpecial2) grpid = -1;

                var infs = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( Sluid );
                if (infs == null) return;


                this.Operatorfun(Sluid, grpid, x, infs.RtuFid  == 0);
            }
        }


        private bool CanExRightOper(int x)
        {
            return true;
        }








        /// <summary>
        /// -2 单数节点 -1双数节点 其他分组
        /// </summary>
        /// <param name="sluId"></param>
        /// <param name="grpId"></param>
        /// <param name="operatorId"></param>
        /// <param name="isGprs"> </param>
        private void Operatorfun(int sluId, int grpId, int operatorId, bool isGprs)
        {
            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(sluId) == false) return;


            var info = Wlst.Sr.ProtocolPhone.LxSlu .wst_slu_right_operator ;//.wlst_cnt_wj2090_order_right_operator;
            //.ServerPart.wlst_Wj2090_clinet_right_operator_slu;

            var data = new client.SluRightOperators.SluRightOperator();
            data.SluId = sluId;
            data.OperationOrder = 0;
            if (grpId < 1)
            {
                data.AddrType = 2;
                if (grpId == -2) data.Addr = 21;
                else data.Addr = 20;
            }
            else
            {
                //if (isGprs)
                //{
                //    data.AddrType = 4;
                //    foreach (var g in this.ChildTreeItems)
                //    {
                //        if (g.NodeId < 256)
                //            data.Addrs.Add(g.NodeId);
                //    }
                //}
                //else
                {
                    data.AddrType = 1;
                    data.Addr = grpId;
                }

            }
            data.Addrs = new List<int>();
            if (operatorId < 4)
            {
                data.CmdType = 4;
                if (operatorId == 3)
                {
                    operatorId = 4;
                }
                data.CmdMix = new List<int>() {operatorId, operatorId, operatorId, operatorId};
                data.CmdPwmField = new client.SluRightOperators.SluRightOperator.CmdPwm()
                                       {
                                           LoopCanDo = new List<int>() {},
                                           Scale = 0
                                       };
            }
            else
            {
                data.CmdType = 5;
                data.CmdMix = new List<int>() {};
                data.CmdPwmField = new client.SluRightOperators.SluRightOperator.CmdPwm()
                                       {
                                           LoopCanDo = new List<int>() {1, 2, 3, 4},
                                           Scale = operatorId - 4
                                       };
            }
            info.WstSluRightOperator .OperatorItems.Add(data);
            SndOrderServer.OrderSnd(info);
        }


        #endregion

    }


    public enum NodeTypeEnmu
    {
        /// <summary>
        /// 控制器分组
        /// </summary>
        ConGrp,
        /// <summary>
        /// 控制器节点
        /// </summary>
        ConNode,
        /// <summary>
        /// 单数节点
        /// </summary>
        ConGrpSpecial1,
        /// <summary>
        /// 双数节点
        /// </summary>
        ConGrpSpecial2,
        /// <summary>
        /// 控制器未分组
        /// </summary>
        ConGrpNoneGrp,
    }
}

