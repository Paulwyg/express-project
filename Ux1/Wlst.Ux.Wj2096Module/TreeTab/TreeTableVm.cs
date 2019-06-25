using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.Wj2096Module.TreeTab.vm;

namespace Wlst.Ux.Wj2096Module.TreeTab
{

    [Export(typeof (IITreeTab))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TreeTableVm : ObservableObject, IITreeTab
    {
        private int _hxxx = 0;

        /// <summary>
        /// 前台界面绑定的图标大小
        /// </summary>
        public int Hightxx
        {
            get
            {
                if (_hxxx < 0.1)
                {
                    _hxxx = (int) Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeightTree;
                    if (_hxxx > 24) _hxxx = 24;
                    if (_hxxx < 12) _hxxx = 12;
                }
                return _hxxx;
            }
        }

        public static TreeTableVm MySelf;

        public TreeTableVm()
        {

            if (MySelf == null) MySelf = this;
            IsLoadOnlyOneArea = true;
            EventPublish.AddEventTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
           // FirstLoad();


            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(FrLoad, 0);
        }




        #region tab iinterface

        public int Index
        {
            get { return 9; }
        }

        public string Title
        {
            get
            {
                return "物联网单灯";
                return "Map";
            }
        }


        public bool CanClose
        {
            get { return false; }
        }

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion

        private ObservableCollection<NodeItemBase> _childTreeItemsInfo;

        public ObservableCollection<NodeItemBase> ChildItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                    _childTreeItemsInfo = new ObservableCollection<NodeItemBase>();
                return _childTreeItemsInfo;
            }
            set
            {
                if (value != _childTreeItemsInfo)
                {
                    _childTreeItemsInfo = value;
                    this.RaisePropertyChanged(() => this.ChildItems);
                }
            }
        }



        //private EventHandler<NodeSelectedArgs> OnSelectedNodeByCodeIns;
        //event EventHandler<NodeSelectedArgs> IIAreaTree.OnSelectedNodeByCode
        //{
        //    add { OnSelectedNodeByCodeIns += value; }
        //    remove { if (OnSelectedNodeByCodeIns != null) OnSelectedNodeByCodeIns -= value; }
        //}


    };


    
    //load update
    public partial class TreeTableVm
    {
        private RtuAreaItem _refArear = null;
        protected bool IsLoadOnlyOneArea = false;


        void FrLoad()
        {
            ChildItems.Clear();
            var rtn = GetShowNode();
            _refArear = null;
            if (rtn.Count == 0) return;
            if (rtn.Count == 1)
            {
                _refArear = rtn[0];
                ChildItems = rtn[0].ChildItems;
            }
            else foreach (var f in rtn) ChildItems.Add(f);
        }

        private List<int> GetUserCanControlAreas()
        {
            var areaLst = new List<int>();
            var userProperty = UserInfo.UserLoginInfo;
            if (userProperty.D == true)
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0)
                    return new List<int>();
                areaLst.AddRange(Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys.ToList());
            }
            else
            {
                foreach (var t in userProperty.AreaX)
                {
                    if (t >= 0)
                    {
                        areaLst.Add(t);
                    }
                }

                foreach (var t in userProperty.AreaW)
                {
                    if (!areaLst.Contains(t) && t >= 0)
                    {
                        areaLst.Add(t);
                    }
                }
                foreach (var f in userProperty.AreaR)
                {
                    if (!areaLst.Contains(f) && f >= 0)
                    {
                        areaLst.Add(f);
                    }
                }
            }
            return areaLst;
        }

        //加载终端节点
        private List<RtuAreaItem> GetShowNode()
        {
            var rtn = new List<RtuAreaItem>();
            if (ServicesGrpSingleInfoHold.InfoGroups.Count == 0 &&
                Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0)
                return new List<RtuAreaItem>();


            var areas = GetUserCanControlAreas();
            foreach (var f in areas)
            {
                rtn.Add(new RtuAreaItem(f));
            }


            for (int i = rtn.Count - 1; i >= 0; i--)
            {
                if (rtn[i].ChildItems.Count == 0)
                {
                    rtn.RemoveAt(i);
                }
            }
            return rtn;
        }


        ///// <summary>
        ///// 当分组信息发生变化的时候  增量式重新加载节点  
        ///// </summary>
        //public void UpdateAreaOrGrp(int areaIdorGrpId)
        //{
        //    if (_refArea != null) _refArea.UpdateShowInfo();
        //    foreach (var f in ChildItems)
        //        if (f.AreaId == areaIdorGrpId)
        //        {
        //            f.UpdateShowInfo();
        //            break;
        //        }
        //}


        private void OnAreaOrGrpChangedOrReqback()
        {
            //区域 - 组 - 组内设备列表
            var _tmpdata =
                new Dictionary<int, Dictionary<int, List<int>>>();
            if (_refArear != null)
            {
                var tmp = _refArear.GetBelongGrpFiled();
                _tmpdata.Add(_refArear.AreaId, tmp);
            }
            else
            {
                foreach (var f in ChildItems)
                {
                    var gs = f as RtuAreaItem;
                    if (gs != null && _tmpdata.ContainsKey(gs.AreaId) == false)
                    {
                        _tmpdata.Add(gs.AreaId, gs.GetBelongGrpFiled());
                    }
                }
            }

            var newdata = new Dictionary<int, Dictionary<int, List<int>>>();
            var ccarea = GetUserCanControlAreas();
            foreach (var f in ccarea)
            {
                var grp =
                    (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                     where t.Key.Item1 == f
                     orderby t.Value.Index
                     select t.Value).ToList();
                foreach (var g in grp)
                {
                    var rts =
                        (from t in Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Info
                         where t.Value.AreaId == f && g.LstTml.Contains(t.Value.FieldId)
                         select t.Value.FieldId).ToList();
                    if (rts.Count > 0)
                    {
                        if (newdata.ContainsKey(f) == false) newdata.Add(f, new Dictionary<int, List<int>>());
                        if (newdata[f].ContainsKey(g.GroupId) == false) newdata[f].Add(g.GroupId, rts);
                    }

                }
            }

            //区域调整 区域数不等
            if (newdata.Count != _tmpdata.Count)
            {
                this.ChildItems.Clear();
                this.FrLoad();
                return;
            }

            //区域调整 区域内容不同
            foreach (var f in newdata)
            {
                if (_tmpdata.ContainsKey(f.Key) == false)
                {
                    this.ChildItems.Clear();
                    this.FrLoad();
                    return;
                }
            }

            //区域数量与名称一致   比对区域内数据是否一致
            var lst = new List<int>();
            var lstneedcon = new List<int>();
            foreach (var f in _tmpdata)
            {
                //全部 与 特殊 
                if (f.Value.Count - 2 != newdata[f.Key].Count)
                {
                    lst.Add(f.Key);
                }
                else
                {
                    lstneedcon.Add(f.Key);
                }
            }
            if (_tmpdata.Count == 1 && lst.Count > 0) //只有一个区域 全部更新
            {
                this.ChildItems.Clear();
                this.FrLoad();
                return;
            }

            foreach (var f in ChildItems)
            {
                f.UpdateShowInfo();
            }

        }

        private void OnNewAddGrp(int areaid,int filedid)
        {
            this.ChildItems.Clear();
            FrLoad();
        }


        //整个所有区域调整 增删改   重新加载
        //区域内分组调整            区域重绘 

        //单灯控制器归属域调整      区域重绘

        //域内分组调整 增删改       单个域重绘
        //增加域                    动态增加

        //控制器参数调整  批量      批量控制器更新
        //控制器状态变化  批量      批量控制器更新

    }
 
    //event
    public partial class TreeTableVm
    {
        #region IEventAggregator Subscription

        /// <summary>
        /// 事件过滤
        /// 目前只处理
        /// 1、系统当前选中的终端或分组变更，提供联动
        /// 2、终端参数发生变化的时候，即使更新显示数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.SvAv) return true;
                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId == // 区域-组 请求或调整
                        global::Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
                        return true;
                    if (args.EventId == Sr.SlusglInfoHold.Services.EventIdAssign.SluSglEquAdd)
                    {
                        return true;
                    }
                    if (args.EventId == Sr.SlusglInfoHold.Services.EventIdAssign.SluSglEquUpdate)
                    {
                        return true; ;
                    }
                    if (args.EventId == Sr.SlusglInfoHold.Services.EventIdAssign.SluSglFieldGrpUpdate)
                    {
                        return true; ;
                    }
                    if (args.EventId == Sr.SlusglInfoHold.Services.EventIdAssign.SluSglEquDelete)
                    {
                        return true; ;
                    }
                    if (args.EventId == Sr.SlusglInfoHold.Services.EventIdAssign.SluSglEquReqOver)
                    {
                        return true; ;
                    }
                    if (args.EventId == Sr.SlusglInfoHold.Services.EventIdAssign.SluSglFieldUpdate)
                    {
                        return true; ;
                    }
                    if (args.EventId == Sr.SlusglInfoHold.Services.EventIdAssign.SluSglRunningInfoUpdate)//单灯图标变色
                        return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                        return true;
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        private void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.SvAv)
                {
                    ChildItems.Clear();
                    FrLoad();
                    return;
                }
                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
                    {
                        OnAreaOrGrpChangedOrReqback();
                    }
                    if (args.EventId ==  Sr.SlusglInfoHold.Services.EventIdAssign.SluSglEquAdd )
                    {
                        ChildItems.Clear();
                        FrLoad();
                        return;
                    }
                    if (args.EventId == Sr.SlusglInfoHold.Services.EventIdAssign.SluSglEquUpdate )
                    {
                        var lst = args.GetParams()[0] as List<int>;
                        if (lst == null)
                        {
                            var inx =
                            Convert.ToInt32(args.GetParams()[0]);
                            lst=new List<int>(inx);
                        }
                        foreach (var f in ChildItems) f.UpdateChildPara(lst);
                        ChildItems.Clear();
                        FrLoad();
                        return;
                    }
                    if (args.EventId == Sr.SlusglInfoHold.Services.EventIdAssign.SluSglFieldGrpUpdate )
                    {
                        ChildItems.Clear();
                        FrLoad();
                        return;
                    }
                    if (args.EventId == Sr.SlusglInfoHold.Services.EventIdAssign.SluSglEquDelete )
                    {
                        ChildItems.Clear();
                        FrLoad();
                        return;
                    }
                    if (args.EventId == Sr.SlusglInfoHold.Services.EventIdAssign.SluSglEquReqOver )
                    {
                        ChildItems.Clear();
                        FrLoad();
                        return;
                    }
                    if (args.EventId == Sr.SlusglInfoHold.Services.EventIdAssign.SluSglFieldUpdate )
                    {
                        var lst = (int) args.GetParams()[0];
                        var ls = new List<int>();
                        ls.Add(lst);

                        foreach (var f in ChildItems) f.UpdateChildPara(ls);
                        ChildItems.Clear();
                        FrLoad();
                        return;
                    }
                    if (args.EventId == Sr.SlusglInfoHold.Services.EventIdAssign.SluSglRunningInfoUpdate)
                    {
                        var lst = args.GetParams()[0] as IEnumerable<long>;
                        if (lst == null) return;
                        foreach (var f in lst)
                        {
                            if (FieldNodeItem.ConnItems.ContainsKey(f) == false) continue;
                            int imagecode = Wlst.Sr.SlusglInfoHold.Services.SluSglCtrlDataHold. GetCtrlImageCode(f);
                            var imagesIcon = Wlst.Cr.CoreMims.Services.ImageIcon.GetBitmapImage(imagecode);
                            foreach (var l in FieldNodeItem.ConnItems[f])
                            {
                                if (l.Target != null)
                                {
                                    var fff = l.Target as CtrlNodeItem;
                                    if (fff != null)
                                    {
                                        fff.ImagesIcon = imagesIcon;
                                    }
                                }
                            }
                        }
                    }

                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {
                        try
                        {

                            int x = Convert.ToInt32(args.GetParams()[0]);
                            if (x > 0)//&& Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsRtuLight(x)
                            {
                                if (args.GetParams().Count > 1)
                                {
                                    int y = Convert.ToInt32(args.GetParams()[1]);
                                    OnCurrentSelectedCtrlNode(x, y);
                                }
                                else
                                {
                                    OnCurrentSelectedNode(x);
                                }

                                //var tmp = TreeNodeItemSluViewModel.CurrentSelectedTreeNode;

                                //if (tmp != null) OnCurrentSelectedNode(tmp.NodeId);
                                //if (tmp == null || tmp.NodeId != x)
                                //{
                                //    if (OnSelectedNodeByCodeIns != null)
                                //    {
                                //        OnSelectedNodeByCodeIns(this, new NodeSelectedArgs() { SluIdSelected = x });
                                //    }
                                //}
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        private bool isvier;

        public bool IsVir
        {
            get { return isvier; }
            set
            {
                if (isvier != value)
                {
                    isvier = value;
                    this.RaisePropertyChanged(() => this.IsVir);
                }
            }
        }


        #region Reflesh

        private DateTime _dtQuery;
        private ICommand _cmdQuery;

        public ICommand Reflesh
        {
            get { return _cmdQuery ?? (_cmdQuery = new RelayCommand(ExQuery, CanQuery, false)); }
        }

        private void ExQuery()
        {
            _dtQuery = DateTime.Now;
            this.FrLoad();
        }

        private bool CanQuery()
        {
            return DateTime.Now.Ticks - _dtQuery.Ticks > 60000000;
        }

        #endregion

      
        #region CmdOc

        private int _curnetselectrtu = 0;
        private int _curselectctrl = 0;

        private void OnCurrentSelectedNode(int nodeid)
        {
            var equip = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(nodeid);
            //var equip = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(nodeid);
            if (equip != null)
            {
                if (_curnetselectrtu == nodeid) return;
                _curnetselectrtu = nodeid;
                _curselectctrl = 0;
                //_curstate = equip.RtuStateCode;
                //CurRtuInof = "快速操作: " + equip.RtuPhyId.ToString("d3") + " - " + equip.RtuName;

            }
        }
        private void OnCurrentSelectedCtrlNode(int nodeid, int ctrlid)
        {
            var equip = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(nodeid);
            if (equip != null)
            {
                if (_curnetselectrtu == nodeid && _curselectctrl == ctrlid) return;
                _curnetselectrtu = nodeid;
                _curselectctrl = ctrlid;
                ////_curstate = equip.RtuStateCode;
                //CurRtuInof = "快速操作: " + equip.RtuPhyId.ToString("d3") + " - " + equip.RtuName;

            }
        }

        private ICommand _cmdCmdOcrchText;

        public ICommand CmdOpenCloselight
        {
            get
            {
                if (_cmdCmdOcrchText == null)
                    _cmdCmdOcrchText = new RelayCommand<string>(ExCmdOpenCloselight, CanCmdOpenCloselight, true);
                return _cmdCmdOcrchText;
            }
        }

        private void ExCmdOpenCloselight(string s)
        {
            try
            {
                if (_curselectctrl < 1) return;
                int x = 0;
                if (Int32.TryParse(s, out x))
                {


                    var loops = (from t in KxInfo where t.IsSelected orderby t.Value ascending select t.Value).ToList();
                    if (loops.Count == 0) return;

                    string tr = x == 1 ? "开灯" : "关灯";
                    if (x == 1)
                    {
                        if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 1)
                        {
                            if (
                                Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                                    "您将要进行" + tr + "操作，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                            {
                                return;
                            }
                        }
                        else if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                        {


                            var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                            if (sss == UMessageBoxWantPassWord.CancelReturn)
                            {
                                return;
                            }
                            if (sss != UserInfo.UserLoginInfo.UserPassword)
                            {
                                UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                                 UMessageBoxButton.Yes);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CloseLightSecondConfirm == 1)
                        {
                            if (
                                Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                                    "您将要进行" + tr + "操作，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                            {
                                return;
                            }
                        }
                        else if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CloseLightSecondConfirm == 2)
                        {


                            var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                            if (sss == UMessageBoxWantPassWord.CancelReturn)
                            {
                                return;
                            }
                            if (sss != UserInfo.UserLoginInfo.UserPassword)
                            {
                                UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                                 UMessageBoxButton.Yes);
                                return;
                            }
                        }
                    }

                    OpenClsoelIGT(_curnetselectrtu, _curselectctrl, loops, x == 1);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OpenClsoelIGT(int rtuId, int ctrlId, List<int> loops, bool isOpen)
        {


            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slu_sgl_right_operator;
            var data = new client.SluRightOperators.SluRightOperator();
            bool l1 = KxInfo[0].IsSelected;
            bool l2 = KxInfo[1].IsSelected;
            bool l3 = KxInfo[2].IsSelected;
            bool l4 = KxInfo[3].IsSelected;
            if (l1 == false && l2 == false && l3 == false && l4 == false) return;

           

            data.SluId = rtuId;
            data.AddrType = 4;
            data.Addr = ctrlId;
            data.Addrs = new List<int>();
            data.Addrs.Add(ctrlId);

            data.CmdType = 4;  //todo
            int orderid = isOpen ? 1 : 4;
            //var scale = orderid;
            //if (scale > 4) scale = scale - 5;

            data.CmdMix = new List<int>() { l1 ? orderid : 0, l2 ? orderid : 0, l3 ? orderid : 0, l4 ? orderid : 0 };
            data.CmdPwmField = new Wlst.client.SluRightOperators.SluRightOperator.CmdPwm()
            {
                LoopCanDo = new List<int>() { 1, 2, 3, 4 },
                Scale = 0
            };

            info.WstSluRightOperator.OperatorItems.Add(data);
            SndOrderServer.OrderSnd(info, 0, 0, true);


        }


        private bool CanCmdOpenCloselight(string s)
        {
            if (_curnetselectrtu < 1000) return false;
            if (_curselectctrl < 1) return false;
            if ((from t in KxInfo where t.IsSelected select t).Count() == 0) return false;
            return true;
        }



        #endregion

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool> _searchchsdfsdfsInfo;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool> KxInfo
        {
            get
            {
                if (_searchchsdfsdfsInfo == null)
                {
                    _searchchsdfsdfsInfo = new ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool>();
                    for (int i = 1; i < 17; i++)
                        _searchchsdfsdfsInfo.Add(new NameIntBool() { Name = "K" + i, Value = i, IsSelected = false });
                }
                return _searchchsdfsdfsInfo;
            }
        }

    }

 
    
}
