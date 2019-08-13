using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.Services;
using Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.ViewModel;
using Wlst.Ux.WJ3005Module.RtuLdlSet.Services;
using Wlst.client;

namespace Wlst.Ux.WJ3005Module.RtuLdlSet.ViewModels
{
    [Export(typeof (IILdlSetView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LdlSetViewModel : Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged,
                                           IILdlSetView
    {

        public LdlSetViewModel ()
        {
            this.InitEvent();
            this.InitAction();

        }

        #region IITab
        public int Index
        {
            get { return 1; }
        }
        private string xx7;
        public string Remark
        {
            get { return xx7; }
            set
            {
                if (xx7 == value) return;
                xx7 = value;
                this.RaisePropertyChanged(() => this.Remark);
            }
        }

        public string Title
        {
            get { return "终端亮灯率"; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion

        public void NavInitBeforShow(params object[] parsObjects)
        {
            page1visi = Visibility.Visible;
            page2visi = Visibility.Collapsed;
        }

        private bool _isViewShow = false;
        private long dttime = 0;
        public void NavOnLoad(params object[] parsObjects)
        {
            dttime = DateTime.Now.Ticks;

            if (_isViewShow) return;
            _isViewShow = true;    
            
            page1visi = Visibility.Visible;
            page2visi = Visibility.Collapsed;
            CmdText = "下一步";
            LoadTreeNode();
            Items.Clear();

            IsJumpCan = false;

        }

        #region CmdNext

        private ICommand _cmCmdSetLdldAsynTime;
        public ICommand CmdSetLdl
        {
            get { return _cmCmdSetLdldAsynTime ?? (_cmCmdSetLdldAsynTime = new RelayCommand(ExCmdSetLdl, CanCmdSetLdl, true)); }
        }
        private void ExCmdSetLdl()
        {
            if (page1visi==Visibility.Visible  )
            {
                page2visi = Visibility.Visible    ;
                page1visi = Visibility.Collapsed ;
                CmdText = "返回上一步";    
                
                var rtus = new List<int>();
                foreach (var f in ItemsTree )
                {
                    foreach (var g in f.ChildTreeItems)
                    {
                        if (g.IsChecked) rtus.Add(g.NodeId);
                    }
                }

                NextStep(rtus );
            }
            else
            {
                page2visi = Visibility.Collapsed  ;
                page1visi = Visibility.Visible  ;
                CmdText = "下一步";


            
            }
        }

        private bool CanCmdSetLdl()
        {
            return true;
        }

        private Visibility _page1visi;

        public Visibility page1visi
        {
            get { return _page1visi; }
            set { if (value == _page1visi) return;
                _page1visi = value;
                this.RaisePropertyChanged(() => page1visi);
            }
        }

        private Visibility _page2visi;

        public Visibility page2visi
        {
            get { return _page2visi; }
            set
            {
                if (value == _page2visi) return;
                _page2visi = value;
                this.RaisePropertyChanged(() => page2visi);
            }
        }

        private string  _cmdtext;

        public string  CmdText
        {
            get { return _cmdtext; }
            set
            {
                if (value == _cmdtext) return;
                _cmdtext = value;
                this.RaisePropertyChanged(() => CmdText);
            }
        }
        #endregion


        void NextStep(List<int> rtus)
        {

            bool exadd = Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 1;

            Dictionary<int, string> tmpdir = new Dictionary<int, string>();
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                )
            {
                foreach (var g in t.Value.LstTml)
                {
                    if (tmpdir.ContainsKey(g)) continue;
                    tmpdir.Add(g, exadd ? t.Value.AreaId + "-" + t.Key.Item2 + "-" + t.Value.GroupName : t.Key.Item2 + "-" + t.Value.GroupName);
                }
            }

            Items.Clear();
            int index = 1;
            foreach (var t in rtus)
            {
                if (tmpdir.ContainsKey(t))
                {
                    Items.Add(new ItemOne(t, tmpdir[t], index++));
                }
                else
                {
                    Items.Add(new ItemOne(t, "--", index++));
                }
            }
        }

        public void OnUserHideOrClosing()
        {
            Items.Clear();
            ItemsTree.Clear();
            _isViewShow = false;
        }
        private int _areaCount;

        /// <summary>
        /// 节点名称  终端名称或是分组名称
        /// </summary>
        public int AreaCount
        {
            get { return _areaCount; }
            set
            {
                if (_areaCount != value)
                {
                    _areaCount = value;
                    this.RaisePropertyChanged(() => this.AreaCount);
                }
            }
        }

        private bool _isJumpCan;

        /// <summary>
        /// 是否跳过清除数据
        /// </summary>
        public bool IsJumpCan
        {
            get { return _isJumpCan; }
            set
            {
                if (_isJumpCan != value)
                {
                    _isJumpCan = value;
                    this.RaisePropertyChanged(() => this.IsJumpCan);
                }
            }
        }
        //初始化时加载左侧树终端节点
        private void LoadTreeNode()
        {
            Items.Clear();
       
            var areas = new List<int>();
            foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaX)
            {
                if (areas.Contains(f) == false) areas.Add(f);
            }
            foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
            {
                if (areas.Contains(f) == false) areas.Add(f);
            }
            //foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR )
            //{
            //    if (areas.Contains(f) == false) areas.Add(f);
            //}
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                {
                    if (areas.Contains(f) == false) areas.Add(f);
                }
            }
            AreaCount = areas.Count < 2 ? 0 : 150;
           // bool isone = areas.Count < 2 ? true: false;
            foreach (var f in areas)
            {
                var grps = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoList(f);
                foreach (var g in grps)
                {
                    if (g.LstTml.Count>0)
                    {
                        this.ItemsTree.Add(new TreeGroupNode(f, g.GroupId));
                    }
                }
                this.ItemsTree.Add(new TreeGroupNode(f, 0));
            }
            foreach (var child in ItemsTree) {
                foreach(var rtu in child.ChildTreeItems)
                {
                    rtu.NodeName = rtu.PhysicalId.ToString().PadLeft(3,'0') + "-" + rtu.NodeName;
                    rtu.StateString = rtu.State == EnumTmlState.Use ? "使用" : rtu.State == EnumTmlState.NotUse ? "不用" : "停运";
                }
            }
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                if (ItemsTree[i].ChildTreeItems.Count == 0) Items.RemoveAt(i);
            }
        }


        #region Items

        private ObservableCollection<TreeNodeBase> _items;
        public ObservableCollection<TreeNodeBase> ItemsTree
        {
            get { return _items ?? (_items = new ObservableCollection<TreeNodeBase>()); }
        }

        #endregion

        private ObservableCollection<ItemOne> _measurePatrolData;

        public ObservableCollection<ItemOne> Items
        {
            get
            {
                if (_measurePatrolData == null)
                    _measurePatrolData = new ObservableCollection<ItemOne>();

                return _measurePatrolData;
            }
            set
            {
                if (value == _measurePatrolData) return;
                _measurePatrolData = value;
                this.RaisePropertyChanged(() => this.Items);
            }
        }


        private ItemOne curr;

        public ItemOne CurrentSelectItem
        {
            get { return curr; }
            set
            {
                if (curr == value) return;
                curr = value;
                this.RaisePropertyChanged(() => this.CurrentSelectItem);
            }
        }


    }


    public partial class LdlSetViewModel
    {
        public void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2 ,
                                    PublishEventType.Core,true);
        }

        public override bool FundOrderFilterForExtendCheck(PublishEventArgs args)
        {
            if (_isViewShow == false) return false;
            if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)
            {
                if (_isViewShow == false) return false;
                var lst = args.GetParams()[0] as List<int>;
                if (lst == null || lst.Count == 0) return false;
                foreach (var f in lst)
                {
                    var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(f);
                    if (run == null || run.RtuNewData == null) continue;


                    foreach (var gts in this.Items)
                    {
                        if (gts.RtuId == f)
                        {
                            return true;
                        }
                    }

                }
                return false;
            }
            return true;
        }


        public override void ExPublishedEvent(
            PublishEventArgs args)
        {
            if (_isViewShow == false) return;
            var lst = args.GetParams()[0] as List<int>;
            if (lst == null || lst.Count == 0) return;
            foreach (var f in lst)
            {
                var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(f);
                if (run == null ||run .RtuNewData ==null ) continue;


                    foreach (var gts in this.Items)
                    {
                        if (gts.RtuId == f )
                        {
                            if (gts.LastSetTime >= run.RtuNewData.DateCreate.Ticks) break;

                            gts.SetNewData(run .RtuNewData,IsJumpCan );
                            break;
                        }
                    }
                
            }
        }



        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set,
                // .wlst_svr_ans_cnt_update_wj3090_ldl_para ,//.ClientPart.wlst_Measures_server_ans_clinet_update_ldl_para, 
                OnZcOrSetBack,
                typeof (LdlSetViewModel), this);
        }


        private long dtSave = DateTime.Now.AddDays(-1).Ticks;

        private void OnZcOrSetBack(string sessionid, Wlst.mobile.MsgWithMobile info)
        {
            if (info.WstRtuLdlSxxAvgSet == null || info.WstRtuLdlSxxAvgSet.Op != 12) return;

            if (DateTime.Now.Ticks - dtSave < 100000000)
                Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  保存亮灯率成功!";
        }
    }


    public partial class LdlSetViewModel
    {

        #region CmdZcOrSnd

        private ICommand _cmCmdZcOrSnd;

        public ICommand CmdZcOrSnd
        {
            get { return _cmCmdZcOrSnd ?? (_cmCmdZcOrSnd = new RelayCommand<string>(ExCmdZcOrSnd, CanCmdZcOrSnd, true)); }
        }


        private long lastexute = 0;
        private int lastexutetpara = 0;

        private void ExCmdZcOrSnd(string str)
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }
            lastexute = DateTime.Now.Ticks;
            lastexutetpara = x;

            ExAc(x);

        }

        private bool CanCmdZcOrSnd(string str)
        {
            int xx = 0;

            try
            {
                xx = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }

            if (curr == null)
            {
               
                    if (xx == 7 || xx == 8)
                    {
                        return DateTime.Now.Ticks - lastexute > 30000000;
                    }
                
                else
                    return false;
            }

            int x = 0;
            try
            {
                x = Convert.ToInt32(str);

            }
            catch (Exception ex)
            {

            }
            if (x == lastexutetpara)
            {
                if (x == 1 || x == 7 || x == 8)
                    return DateTime.Now.Ticks - lastexute > 30000000;
            }
            return true;
        }

        #endregion


        private void ExAc(int x)
        {
            if (curr == null && x != 7 && x != 8) return;
            if (x == 1)
            {
                var info = Sr.ProtocolPhone.LxRtu .wst_rtu_orders ;//.wlst_cnt_request_wj3090_measure;
                info.Args .Addr .Add(curr.RtuId);
                info.WstRtuOrders.Op = 31;
                info.WstRtuOrders.RtuIds.Add(curr.RtuId);
                SndOrderServer.OrderSnd(info);
                Remark = "选测 " + curr.ShowId + " 命令已经发送...";
            }
            else if (x > 1 && x < 6)
            {
                curr.CleanData(x - 1);
            }
            else if (x == 6)
            {
                if (Items.Contains(curr)) Items.Remove(curr);
                curr = null;
            }
            else if (x == 7)
            {
                var lst = (from t in Items where t.NeedNewData() select t.RtuId).ToList();
                if (lst.Count == 0)
                {
                    Remark = "所有终端最新数据均达到四条，不再执行选测...";
                    return;
                }
                //var info = Wlst.Sr.ProtocolPhone.ServerListen.wlst_cnt_request_wj3090_measure;
                //info.Args .Addr .AddRange(lst);
                //SndOrderServer.OrderSnd(info);

                var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.wlst_cnt_request_wj3090_measure;
                info.Args.Addr.AddRange(lst);
                info.WstRtuOrders.Op = 31;
                info.WstRtuOrders.RtuIds.AddRange( lst);
                SndOrderServer.OrderSnd(info);
                Remark = "巡测 命令已经发送...[仅选测最新数据未达到四条的终端]";
            }
            else if (x == 8)
            {

                Remark = "测试亮灯率低于75%的回路数据系统将不予以保存；";
                //保存亮灯率
                var info = Wlst.Sr.ProtocolPhone .LxRtu .wst_ldl_sxx_avg_set ;// .wlst_cnt_update_wj3090_ldl_para ;//.ServerPart.wlst_Measures_clinet_update_ldl_para;
                info.WstRtuLdlSxxAvgSet.Op = 12;
                foreach (var f in Items)
                {

                    var tu = new List<Tuple<int, double>>();
                    foreach (var g in f.Loops)
                    {
                        if (g.TargetTest < 0.75 || g.TargetTest > 100) continue;
                        tu.Add(new Tuple<int, double>(g.LoopId, g.LdlValue));
                    }
                    if (tu.Count > 0)
                    {
                        var tmp = new  RtuSets.RtuLdlItems() { RtuId = f.RtuId, Loops = new List<int>(), Ldls = new List<double>() };
                        foreach (var fff in tu)
                        {
                            tmp.Ldls.Add(fff.Item2);
                            tmp.Loops.Add(fff.Item1);
                        }
                        info.WstRtuLdlSxxAvgSet.LdlItems.Add(tmp);
                    }
                }
                if (info.WstRtuLdlSxxAvgSet.LdlItems.Count == 0)
                {
                    Remark += "无数据保存...";
                    return;
                }
                SndOrderServer.OrderSnd(info);
                dtSave = DateTime.Now.Ticks; 
                Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  数据已提交保存...";
            }
        }
    }
}
