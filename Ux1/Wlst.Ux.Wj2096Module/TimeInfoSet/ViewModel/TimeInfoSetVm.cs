using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.SlusglInfoHold.Services;
using Wlst.Ux.Wj2096Module.TimeInfoSet.Services;
using Wlst.Ux.Wj2096Module.TreeTab.vm;
using Wlst.client;

namespace Wlst.Ux.Wj2096Module.TimeInfoSet.ViewModel
{
    [Export(typeof (IITimeInfoSet))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TimeInfoSetVm : Wlst.Cr .Core .EventHandlerHelper .EventHandlerHelperExtendNotifyProperyChanged , Services.IITimeInfoSet
    {

        public TimeInfoSetVm ()
        {
            this.AddEventFilterInfo(Wlst.Sr.SlusglInfoHold.Services.EventIdAssign.SluSglTimeInfoUpdate, PublishEventType.Core,true );
            this.AddEventFilterInfo(Wlst.Sr.SlusglInfoHold.Services.EventIdAssign.SluSglTimeInfoRequest, PublishEventType.Core,true );
            this.AddEventFilterInfo(Wlst.Sr.SlusglInfoHold.Services.EventIdAssign.SluSglTimeInfoDelete, PublishEventType.Core, true);
        }

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            //请求成功
            //if (args.EventId == EventIdAssign.SluSglTimeInfoRequest)
            //{
            //    var bts =
            //        (from t in TimeInfos.MySelf.Info where t.Key.Item1 == AreaId orderby t.Key ascending select t.Value)
            //            .ToList();
            //    foreach (var g in bts)
            //    {
            //        TimeItems.Add(new TimeInfoOneVm(AreaId, g));
            //    }
            //    if (TimeItems.Count > 0) CurrentSelectedTimeItem = TimeItems[0];
            //    else CurrentSelectedTimeItem = null;
            //    deleteing = false;
            //}
            //更新或增加成功
            if (args.EventId == EventIdAssign.SluSglTimeInfoUpdate)
            {
                if (DateTime.Now.Ticks - _dtCmdSaveTimeTable.Ticks < 100000000)
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "更新成功.");
                else
                {
                    TimeItems.Clear();
                    var bts =
                        (from t in TimeInfos.MySelf.Info
                         where t.Key.Item1 == AreaId
                         orderby t.Key ascending
                         select t.Value).ToList();
                    foreach (var g in bts)
                    {
                        TimeItems.Add(new TimeInfoOneVm(AreaId, g));
                    }
                    if (TimeItems.Count > 0) CurrentSelectedTimeItem = TimeItems[0];
                    else CurrentSelectedTimeItem = null;
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "接收到服务器更新数据，界面执行刷新...");
                }
            }
            //删除成功
            if (args.EventId == EventIdAssign.SluSglTimeInfoDelete)
            {
                if (DateTime.Now.Ticks - _dtCmdDeleteTimeTable.Ticks < 100000000)
                {
                    TimeItems.Remove(CurrentSelectedTimeItem);
                    if (TimeItems.Count > 0) CurrentSelectedTimeItem = TimeItems[0];
                    else CurrentSelectedTimeItem = null;
                    deleteing = false;
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "删除成功.");
                }
                else
                {
                    TimeItems.Clear();
                    var bts =
                        (from t in TimeInfos.MySelf.Info
                         where t.Key.Item1 == AreaId
                         orderby t.Key ascending
                         select t.Value).ToList();
                    foreach (var g in bts)
                    {
                        TimeItems.Add(new TimeInfoOneVm(AreaId, g));
                    }
                    if (TimeItems.Count > 0) CurrentSelectedTimeItem = TimeItems[0];
                    else CurrentSelectedTimeItem = null;
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "接收到服务器更新数据，界面执行刷新...");
                }
            }
            //this.NavOnLoad();
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            deleteing = true ;
            CleanSluCtrls();
            AreaName.Clear();
            if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var t in Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                {
                    string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                    AreaName.Add(new AreaInt() { Value = area, Key = t });
                }
            }
            else
            {
                foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
                {
                    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                    {
                        string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                        AreaName.Add(new AreaInt() { Value = area, Key = t });
                    }
                }
            }
            
            if (AreaName.Count > 0)
                AreaComboBoxSelected = AreaName[0];
            if (AreaName.Count > 1)
            {
                Visi = Visibility.Visible;
            }
            else
            {
                Visi = Visibility.Collapsed;
            }

            //LoadTimeItem(AreaId);
            //LoadTimeItems();

        }

        public void OnUserHideOrClosing()
        {
            TimeItems.Clear();
            CleanSluCtrls();
        }

        void CleanSluCtrls()
        {
            foreach (var g in SluCtrls)
            {
                try
                {
                    g.OnSelectedSelfDefContrls -= OnUserSelectedSefDef;
                }
                catch (Exception ex)
                {

                }
            }
            SluCtrls.Clear();
        }

        #region IITab

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "物联网单灯方案设置"; }
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


    }

    /// <summary>
    /// 控制数显示
    /// </summary>
    public partial class TimeInfoSetVm
    {
        public event EventHandler OnBackNeedShowCtrlView;

        private void OnUserSelectedSefDef(object sender, EventArgs args)
        {
            var vm = sender as SluTimeCtrlSluOneVm;
            if (vm == null) return;

            if (CurrentSelectedSluCtrls == null || CurrentSelectedSluCtrls.SluId != vm.SluId)
            {
                CurrentSelectedSluCtrls = vm;
            }
            //if (vm.Is485 == false)
                OnShowCtrlTree(vm.SluId, vm.AddrsCtrls);
            //else OnShowCtrlGrpTree(vm.SluId, vm.AddrsCtrls);
            if (OnBackNeedShowCtrlView != null) OnBackNeedShowCtrlView(this, EventArgs.Empty);
        }


        private ObservableCollection<TreeNodeGrp> _childTreeItemsInfo;

        public ObservableCollection<TreeNodeGrp> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                    _childTreeItemsInfo = new ObservableCollection<TreeNodeGrp>();
                return _childTreeItemsInfo;
            }
            set
            {
                if (value != _childTreeItemsInfo)
                {
                    _childTreeItemsInfo = value;
                    this.RaisePropertyChanged(() => this.ChildTreeItems);
                }
            }
        }

        protected void OnShowCtrlGrpTree(int sluId, List<int> selectGrps)
        {
            this.ChildTreeItems.Clear();
            var para =
                Wlst.Sr.SlusglInfoHold.Services.SluSglFieldGrpHold.MySlef.Get(sluId);
            if (para == null) return;

            var nts = para  ;

            foreach (var g in nts)
            {
                var tu = new Tuple<int, int>(AreaId , g.GrpId);
                //if (Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu))
                //{
                    var nps = new TreeNodeGrp() { NodeId = g.GrpId, NodeName = g.GrpName, IsSelected = selectGrps.Contains(g.GrpId) };
                    this.ChildTreeItems.Add(nps);
                //}
               

            }
        }


        /// <summary>
        /// 显示终端数
        /// </summary>
        /// <param name="sluId"></param>
        /// <param name="selecttrls"></param>
        protected void OnShowCtrlTree(int sluId, List<int> selecttrls)
        {
            this.ChildTreeItems.Clear();

            var holdins = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(sluId).CtrlLst;
            if (holdins == null) return;
            var ctrlitem = holdins.OrderBy(u => u.CtrlId);
            var dirs = new Dictionary<int, string>();
            foreach (var g in ctrlitem)
            {
                var ntgsdf = string.IsNullOrEmpty(g.CtrlName) ? "控制器" + g.CtrlId : g.CtrlName;
                if (dirs.ContainsKey(g.CtrlId)) dirs[g.CtrlId] = ntgsdf;
                else dirs.Add(g.CtrlId, ntgsdf);
            }
            var para =
                Wlst.Sr.SlusglInfoHold.Services.SluSglFieldGrpHold.MySlef.Get(sluId);
            if (para == null) return;
            var nts = para;

            var lstall = (from t in dirs select t.Key).ToList();
            foreach (var g in nts)
            {
                var nps = new TreeNodeGrp() {NodeId = g.GrpId, NodeName = g.GrpName};
                foreach (var f in g.CtrlLst)
                {
                    if (lstall.Contains(f)) lstall.Remove(f);
                    if (dirs.ContainsKey(f))
                    {
                        nps.ChildTreeItems.Add(new TreeNodeCtrl()
                                                   {IsSelected = selecttrls.Contains(f), NodeId = f, NodeName = dirs[f]});
                    }
                }
                if (nps.ChildTreeItems.Count > 0)
                {
                    nps.GetThisCheckByChild();
                    nps.SetCount();
                    this.ChildTreeItems.Add(nps);
                }
            }
            if (lstall.Count > 0)
            {
                var nps = new TreeNodeGrp() {NodeId = 0, NodeName = "未分组控制器"};
                foreach (var f in lstall)
                {
                    // if (lstall.Contains(f)) lstall.Remove(f);
                    if (dirs.ContainsKey(f))
                    {
                        nps.ChildTreeItems.Add(new TreeNodeCtrl()
                                                   {IsSelected = selecttrls.Contains(f), NodeId = f, NodeName = dirs[f]});
                    }
                }
                nps.GetThisCheckByChild();
                nps.SetCount();
                this.ChildTreeItems.Add(nps);
            }
        }


        /// <summary>
        /// 设置完成后 将控制器终端回归方案中
        /// </summary>
        public void OnUserSetOverSelectedSefDef()
        {
            if (CurrentSelectedSluCtrls == null) return;
            if (CurrentSelectedSluCtrls.OperatorTypeSelected == 4)
            {
                if (CurrentSelectedSluCtrls.Is485)
                {
                    var lst = new List<int>();
                    foreach (var f in this.ChildTreeItems)
                    {
                        if (f.IsSelected == false) continue;
                        if (lst.Contains(f.NodeId)) continue;
                        lst.Add(f.NodeId);
                    }
                    CurrentSelectedSluCtrls.UpdateAddrsCtrls(lst);
                }
                else
                {
                    var lst = new List<int>();
                    foreach (var g in this.ChildTreeItems)
                    {
                        foreach (var f in g.ChildTreeItems)
                        {
                            if (f.IsSelected == false) continue;
                            if (lst.Contains(f.NodeId)) continue;
                            lst.Add(f.NodeId);
                        }
                    }
                    CurrentSelectedSluCtrls.UpdateAddrsCtrls(lst);
                }
            }


        }


        /// <summary>
        /// 设置完成后 将集中器终端回归方案中
        /// </summary>
        public bool OnUserSetOverSlus()
        {
            // bool bolreturn = false;
            if (_currentselectTimeItems == null) return true;
            _currentselectTimeItems.Ctrls.Clear();

            int xCount = 0;
            foreach (var g in this.SluCtrls)
            {
                if (g.OperatorTypeSelected == 4 && g.AddrsCtrls.Count == 0)
                {
                    g.IsShowSelfSelected = false;
                }
                if (g.IsShowSelfSelected == false) g.OperatorTypeSelected = 101;

                if (g.OperatorTypeSelected == 101)
                {
                    g.IsShowSelfSelected = false;
                    continue;
                }
                
                xCount++; // = true;
                if (!_currentselectTimeItems.Ctrls.ContainsKey(g.SluId))
                    _currentselectTimeItems.Ctrls.Add(g.SluId, new VSluTimeScheme.VSluTimeSchemeItem.VSluTimeCtrlSluOne()
                                                                   {
                                                                       CtrlOrGrp  = g.AddrsCtrls,
                                                                       OperatorType = g.OperatorTypeSelected,
                                                                       VSluId = g.SluId
                                                                   });
                else
                    _currentselectTimeItems.Ctrls[g.SluId] = new VSluTimeScheme.VSluTimeSchemeItem.VSluTimeCtrlSluOne()
                                                                 {
                                                                     CtrlOrGrp  = g.AddrsCtrls,
                                                                     OperatorType = g.OperatorTypeSelected,
                                                                     VSluId = g.SluId
                                                                 };
            }
            _currentselectTimeItems.UsedSluCount = xCount;
            return xCount > 0;
        }
        // <summary>
        // 是否有集中器
        // </summary>
        public bool HaveSlu() 
        {
            // bool bolreturn = false;


            return this.SluCtrls.Count>0;
        }
    }


    /// <summary>
    /// Add  Delete
    /// </summary>
    public partial class TimeInfoSetVm
    {
        #region  CmdAddTimeTable

        private DateTime _dtCmdAddTimeTable;
        private ICommand _cmdAddTimeTable;

        public ICommand CmdAddTimeTable
        {
            get
            {
                return _cmdAddTimeTable ??
                       (_cmdAddTimeTable = new RelayCommand(ExCmdAddTimeTable, CanCmdAddTimeTable, true));
            }
        }

        private bool CanCmdAddTimeTable()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW .Count >0 && DateTime.Now.Ticks - _dtCmdAddTimeTable.Ticks > 10000000;
        }

        private void ExCmdAddTimeTable()
        {
            _dtCmdAddTimeTable = DateTime.Now;

            var nts = new TimeInfoOneVm();            
            this.TimeItems.Add(nts);
            CurrentSelectedTimeItem = nts;
        }

        #endregion



        #region  CmdDeleteTimeTable

        private DateTime _dtCmdDeleteTimeTable;
        private ICommand _cmdCmdDeleteTimeTable;

        public ICommand CmdDeleteTimeTable
        {
            get
            {
                return _cmdCmdDeleteTimeTable ??
                       (_cmdCmdDeleteTimeTable = new RelayCommand(ExCmdDeleteTimeTable, CanCmdDeleteTimeTable, true));
            }
        }

        private bool CanCmdDeleteTimeTable()
        {
            if (TimeItems.Count < 2) return false;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW .Count >0 && DateTime.Now.Ticks - _dtCmdDeleteTimeTable.Ticks > 10000000 && CurrentSelectedTimeItem != null;
        }


        private bool deleteing = false;

        private void ExCmdDeleteTimeTable()
        {
            _dtCmdDeleteTimeTable = DateTime.Now;

            if (TimeItems.Contains(CurrentSelectedTimeItem))
            {
                var infoss = WlstMessageBox.Show("确认删除", "即将删除当前选中方案，是 继续，否 退出.", WlstMessageBoxType.YesNo);
                if (infoss != WlstMessageBoxResults.Yes) return;
                deleteing = true;
                //var nts = new VSluTimeScheme.VSluTimeSchemeItem {SchemeId = CurrentSelectedTimeItem.SchemeId};
                var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_time_scheme;
                info.WstVsluTimeSchemeInfo.Op = 4;
                info.WstVsluTimeSchemeInfo.Argu = CurrentSelectedTimeItem.SchemeId;
                //info.WstVsluTimeSchemeInfo.Items.Add(nts);
                SndOrderServer.OrderSnd(info, 10, 3);
                Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "提交服务器进行删除....");
                //TimeItems.Remove(CurrentSelectedTimeItem);
                //if (TimeItems.Count > 0) CurrentSelectedTimeItem = TimeItems[0];
                //else CurrentSelectedTimeItem = null;
                //deleteing = false;
            }

        }

        #endregion

        #region  CmdSendTimeTable

        private DateTime _dtCmdSendTimeTable;
        private ICommand _cmdCmdSendTimeTable;

        public ICommand CmdSendTimeTable
        {
            get
            {
                return _cmdCmdSendTimeTable ??
                       (_cmdCmdSendTimeTable = new RelayCommand(ExCmdSendTimeTable, CanCmdSendTimeTable, true));
            }
        }

        private bool CanCmdSendTimeTable()
        {
            if (TimeItems.Count < 2) return false;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdSendTimeTable.Ticks > 10000000 && CurrentSelectedTimeItem != null;
        }




        private void ExCmdSendTimeTable()
        {
            _dtCmdSendTimeTable = DateTime.Now;

            if (TimeItems.Contains(CurrentSelectedTimeItem))
            {
                var infoss = WlstMessageBox.Show("确认下发", "即将下发当前选中方案，是 继续，否 退出.", WlstMessageBoxType.YesNo);
                if (infoss != WlstMessageBoxResults.Yes) return;
                //var nts = new VSluTimeScheme.VSluTimeSchemeItem {SchemeId = CurrentSelectedTimeItem.SchemeId};
                var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_time_scheme;
                info.WstVsluTimeSchemeInfo.Op = 5;
                info.WstVsluTimeSchemeInfo.Argu = CurrentSelectedTimeItem.SchemeId;
                //info.WstVsluTimeSchemeInfo.Items.Add(nts);
                SndOrderServer.OrderSnd(info, 10, 3);
                Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "提交服务器进行下发....");
                //TimeItems.Remove(CurrentSelectedTimeItem);
                //if (TimeItems.Count > 0) CurrentSelectedTimeItem = TimeItems[0];
                //else CurrentSelectedTimeItem = null;
                //deleteing = false;
            }

        }

        #endregion

        #region  CmdSaveTimeTable

        private DateTime _dtCmdSaveTimeTable;
        private ICommand _cmdCmdSaveTimeTable;

        public ICommand CmdSaveTimeTable
        {
            get
            {
                return _cmdCmdSaveTimeTable ??
                       (_cmdCmdSaveTimeTable = new RelayCommand(ExCmdSaveTimeTable, CanCmdSaveTimeTable, true));
            }
        }

        private bool CanCmdSaveTimeTable()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW .Count >0 && DateTime.Now.Ticks - _dtCmdSaveTimeTable.Ticks > 50000000 && CurrentSelectedTimeItem != null;
        }



        private void ExCmdSaveTimeTable()
        {

            var xg = _currentselectTimeItems.OnChanged();
            if (xg.Item1 == false)
            {

                WlstMessageBox.Show("方案设置有遗漏,请完善该方案", xg.Item2, WlstMessageBoxType.Ok);
                return;
            }
            if (OnUserSetOverSlus() == false)
            {
                WlstMessageBox.Show("方案设置有遗漏,请完善该方案", "未设置任何使用该方案的集中器与控制器...", WlstMessageBoxType.Ok);
                return;
            }

            //var rtns = kgdx();
            //if(rtns .Count >0)
            //{
            //    WlstMessageBox.Show("方案设置有遗漏,请完善该方案", "光控操作仅支持开灯或关灯操作，混合操作与节能不支持...", WlstMessageBoxType.Ok);
            //    return;

            //}

            _dtCmdSaveTimeTable = DateTime.Now;

            var xxg = CurrentSelectedTimeItem.OnChanged();
            if (xxg.Item1 == false)
            {

                WlstMessageBox.Show("方案设置有遗漏,请完善该方案", xg.Item2, WlstMessageBoxType.Ok);
                this.RaisePropertyChanged(() => this.CurrentSelectedTimeItem);
                return;
            }
            var nts = CurrentSelectedTimeItem.BackToSluTimeSchemeOne();
            nts.AreaId = AreaId;

            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_time_scheme;
            var tu = new Tuple<int, int>(AreaId, CurrentSelectedTimeItem.SchemeId);
            info.WstVsluTimeSchemeInfo.Op = CurrentSelectedTimeItem.SchemeId == 0 ? 2 : 3;
            info.WstVsluTimeSchemeInfo.Items.Add(nts);
            SndOrderServer.OrderSnd(info, 10, 3);
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "提交服务器进行更新....");
        }


        List<Tuple<int,int>> kgdx()
        {
            List<Tuple<int, int>> rtn = new List<Tuple<int, int>>();
            //var rtn = new List<Tuple<int,int>>();
            foreach (var g in this.TimeItems)
            {
                if (g.OperationMethod == 11)
                {
                    if (g.CmdType != 4)
                    {
                        var tu = new Tuple<int, int>(AreaId ,g.SchemeId);
                        rtn.Add(tu);
                        continue;
                    }
                    var lst = new List<int>();
                    if (g.CmdMix1 != 0) lst.Add(g.CmdMix1);
                    if (g.CmdMix2 != 0) lst.Add(g.CmdMix2);
                    if (g.CmdMix3 != 0) lst.Add(g.CmdMix3);
                    if (g.CmdMix4 != 0) lst.Add(g.CmdMix4);

                    if(lst .Count ==0)
                    {
                        var tu = new Tuple<int, int>(AreaId, g.SchemeId);  //lvf
                        rtn.Add(tu);
                        continue;
                    }
                    int xg = lst[0];
                    bool allthesam = true;
                    foreach (var ff in lst )
                    {
                        if (ff != xg) allthesam = false;
                    }
                    if(allthesam==false  )
                    {
                        var tu = new Tuple<int, int>(AreaId, g.SchemeId);
                        rtn.Add(tu);
                        continue;
                        
                    }
                }
            }
            return rtn;
        }

        #endregion


        #region  CmdSetYearTime

        private DateTime _dtCmdSetYearTime;
        private ICommand _cmdSetYearTime;

        public ICommand CmdSetYearTime
        {
            get
            {
                return _cmdSetYearTime ??
                       (_cmdSetYearTime = new RelayCommand(ExCmdSetYearTime, CanCmdSetYearTime, true));
            }
        }

        private bool CanCmdSetYearTime()
        {
            return UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdSetYearTime.Ticks > 10000000 &&
                   CurrentSelectedTimeItem != null;
        }

        private void ExCmdSetYearTime()
        {
            _dtCmdSetYearTime = DateTime.Now;
            RegionManage.ShowViewByIdAttachRegionWithArgu(Wj2096Module.Services.ViewIdAssign.SetYearTimeViewId, CurrentSelectedTimeItem);
        }

        #endregion

        private string _cursdfsdf;

        public string Msg
        {
            get { return _cursdfsdf; }
            set
            {
                if (value != _cursdfsdf)
                {
                    _cursdfsdf = value;
                    this.RaisePropertyChanged(() => this.Msg);
                }

            }
        }

    }

    /// <summary>
    /// 当前选中方案控制的控制器列表信息
    /// </summary>
    public partial class TimeInfoSetVm
    {
        private ObservableCollection<SluTimeCtrlSluOneVm> _sluCtrls;

        /// <summary>
        /// 操作参数信息
        /// </summary>
        // public SluTimePlan PlanOperator;
        /// <summary>
        /// 本次操作需要操作的集中器与控制器信息； 一次操作的所有的自定义控制器不得超过一万个；
        /// </summary>
        public ObservableCollection<SluTimeCtrlSluOneVm> SluCtrls
        {
            get
            {
                if (_sluCtrls == null) _sluCtrls = new ObservableCollection<SluTimeCtrlSluOneVm>();
                return _sluCtrls;
            }
        }

        private SluTimeCtrlSluOneVm _currentselectTSluCtrls;

        public SluTimeCtrlSluOneVm CurrentSelectedSluCtrls
        {
            get { return _currentselectTSluCtrls; }
            set
            {
                if (value != _currentselectTSluCtrls)
                {
                    if (_currentselectTSluCtrls != null)
                    {
                        _currentselectTSluCtrls.Marked = "";
                    }

                    _currentselectTSluCtrls = value;
                    if (_currentselectTSluCtrls != null)
                    {
                        _currentselectTSluCtrls.Marked = "设置中...";
                    }
                    

                    this.RaisePropertyChanged(() => this.CurrentSelectedSluCtrls);
                }

            }
        }





         private bool  _currIsSelectSluEnable;

        public bool IsSelectSluEnable
        {
            get { return _currIsSelectSluEnable; }
            set
            {
                if (value != _currIsSelectSluEnable)
                {
                    _currIsSelectSluEnable = value;
                    this.RaisePropertyChanged(() => this.IsSelectSluEnable);
                }

            }
        }


        private static ObservableCollection<AreaInt> _devices;

        public static ObservableCollection<AreaInt> AreaName
        {
            get
            {
                if (_devices == null)
                {
                    _devices = new ObservableCollection<AreaInt>();
                }
                return _devices;
            }

        }
          private Visibility _txtVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility Visi
        {
            get { return _txtVisi; }
            set
            {
                if (value != _txtVisi)
                {
                    _txtVisi = value;
                    this.RaisePropertyChanged(() => this.Visi);
                }
            }
        }

        public class AreaInt : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private int _key;

            public int Key
            {
                get { return _key; }
                set
                {
                    if (_key != value)
                    {
                        _key = value;
                        this.RaisePropertyChanged(() => this.Key);
                    }
                }
            }

            private string _value;

            public string Value
            {
                get { return _value; }
                set
                {
                    if (value != _value)
                    {
                        _value = value;
                        this.RaisePropertyChanged(() => this.Value);
                    }
                }
            }
        }

        private AreaInt _areacomboboxselected;
        private  int AreaId;

        public AreaInt AreaComboBoxSelected
        {
            get { return _areacomboboxselected; }
            set
            {
                if (_areacomboboxselected != value)
                {
                    _areacomboboxselected = value;
                    this.RaisePropertyChanged(() => this.AreaComboBoxSelected);
                    if (value == null) return;
                    AreaId = value.Key;
                    LoadTimeItem(AreaId);

                    //LoadTimeTableInfoFromSr();
                    //this.LoadRtuOrGrpBandingInfo();
                }
            }
        }

        private ObservableCollection<TimeInfoOneVm> _sTimeItems;

        public ObservableCollection<TimeInfoOneVm> TimeItems
        {
            get
            {
                if (_sTimeItems == null) _sTimeItems = new ObservableCollection<TimeInfoOneVm>();
                return _sTimeItems;
            }
        }


        private TimeInfoOneVm _currentselectTimeItems;

        public TimeInfoOneVm CurrentSelectedTimeItem
        {
            get { return _currentselectTimeItems; }
            set
            {
                IsSelectSluEnable = value != null;

                if (value != _currentselectTimeItems)
                {

                    if (_currentselectTimeItems != null && deleteing == false)
                    {
                        ////////var xg = _currentselectTimeItems.OnChanged();
                        ////////if (xg.Item1 == false)
                        ////////{

                        ////////    WlstMessageBox.Show("方案设置有遗漏,请完善该方案", xg.Item2, WlstMessageBoxType.Ok);
                        ////////    this.RaisePropertyChanged(() => this.CurrentSelectedTimeItem);
                        ////////    return;
                        ////////}
                        //if (OnUserSetOverSlus() == false)
                        //{
                        //    //_currentselectTimeItems = value;
                        //    ////WlstMessageBox.Show("方案设置有遗漏,请完善该方案", "未设置任何使用该方案的集中器与控制器...", WlstMessageBoxType.Ok);
                        //    //this.RaisePropertyChanged(() => this.CurrentSelectedTimeItem);
                        //    OnSelectedTimeItemChanged();
                        //    return;
                        //}

                        _currentselectTimeItems.Marked = "";
                    }

                    _currentselectTimeItems = value;
                    if (_currentselectTimeItems != null) _currentselectTimeItems.Marked = "设置中...";
                    this.RaisePropertyChanged(() => this.CurrentSelectedTimeItem);
                    OnSelectedTimeItemChanged();
                }
                else
                {
                     
                    //todo
                }

            }
        }
        //private void LoadTimeItems()
        //{
        //    var nts =
        //           (from t in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
        //            where t.Value.EquipmentType == WjParaBase.EquType.Slu && t.Value.AreaId==AreaId 
        //            orderby t.Key ascending
        //            select t.Key).ToList();
        //    foreach (var g in nts)
        //    {
                
        //        var gt = new SluTimeCtrlSluOneVm(g);
        //        gt.OnSelectedSelfDefContrls += OnUserSelectedSefDef;
                
        //        SluCtrls.Add(gt);
        //    }

        //    TimeItems.Clear();

        //    var bts =
        //        (from t in TimeInfos.MySelf.Info where t.Key.Item1==AreaId orderby t.Key ascending select t.Value).ToList();
        //    foreach (var g in bts)
        //    {
        //        TimeItems.Add(new TimeInfoOneVm(AreaId,g));
        //    }
        //    if (TimeItems.Count > 0) CurrentSelectedTimeItem = TimeItems[0];
        //    else CurrentSelectedTimeItem = null;
        //    deleteing = false;
        //}

        private void LoadTimeItem(int areaId) 
        {
            CleanSluCtrls();
            var gt = new RtuGrpItem(AreaId, 0, 3);
            foreach (var t in gt.ChildItems)
            {
                var tt = new SluTimeCtrlSluOneVm(t.NodeId)
                             {
                                 SluId = t.NodeId,
                                 SluName = t.NodeName
                             };
                tt.OnSelectedSelfDefContrls += OnUserSelectedSefDef;
                SluCtrls.Add(tt);
            }

            TimeItems.Clear();
            //var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_time_scheme;
            //info.WstVsluTimeSchemeInfo.Op = 1;
            //SndOrderServer.OrderSnd(info, 10, 3);
            var bts =
                (from t in TimeInfos.MySelf.Info where t.Key.Item1 == areaId orderby t.Key ascending select t.Value).ToList();
            foreach (var g in bts)
            {
                TimeItems.Add(new TimeInfoOneVm(areaId, g));
            }
            if (TimeItems.Count > 0) CurrentSelectedTimeItem = TimeItems[0];
            else CurrentSelectedTimeItem = null;
            deleteing = false;
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

        private void OnSelectedTimeItemChanged()
        {
             
            if (_currentselectTimeItems == null )
            {
                foreach (var g in this.SluCtrls)
                {
                    g.UpdateInfoBySluOne(0);
                }
            }
            else
            {
                foreach (var g in this.SluCtrls)
                {
                    if (_currentselectTimeItems.Ctrls.ContainsKey(g.SluId))
                    {
                        g.UpdateInfoBySluOne(_currentselectTimeItems.Ctrls[g.SluId]);
                    }
                    else
                    {
                        g.UpdateInfoBySluOne(0);
                    }
                }
            }
        }

    }
}
