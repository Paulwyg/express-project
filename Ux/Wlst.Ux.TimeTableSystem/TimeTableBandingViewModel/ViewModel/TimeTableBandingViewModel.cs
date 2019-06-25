using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Sr.ProtocolCnt.TmlBngTimeTable;
using Wlst.Sr.TimeTableSystem.Models;
using Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.Services;

namespace Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.ViewModel
{
    [Export(typeof (IITimeTableBandingViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class TimeTableBandingViewModel :
        Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged, IITimeTableBandingViewModel
    {

        public TimeTableBandingViewModel()
        {
           // LoadNode();
            InitAction();
        }

        private void InitAction()
        {
            this.AddEventFilterInfo(Wlst.Sr.TimeTableSystem.Services.IdServices.EventIdAssign.TmlBelongTimeTableUpdate,
                                    PublishEventType.Core);
        }

        public override void ExPublishedEvent(
            Microsoft.Practices.Prism.MefExtensions.Event.EventHelper.PublishEventArgs args)
        {
            //base.ExPublishedEvent(args);

            if (!_isActive) return;
            if (DateTime.Now.Ticks - dtSnd.Ticks < 100000000)
            {
                Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 保存成功.";
            }
            if (DateTime.Now.Ticks - dtSnd.Ticks < 600000000)
            {
                Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 保存成功.";
                _canReflesh = true;
            }
            else
            {
                Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  数据更新. ";
                _canReflesh = true;
            }
            this.LoadNode();
        }


        #region load node

        //加载终端节点
        private void LoadNode()
        {
            ChildTreeItems.Clear();
            if (Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(0))
            {
                var tmp = Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[0].LstGrp;
                var atttmp = Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(tmp);

               // var atttmp = (from t in tmp orderby t ascending select t).ToList();
                foreach (var t in atttmp)
                {
                    if (
                        !Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(t))
                        continue;
                    this.ChildTreeItems.Add(new ListTreeGroupNode(null, t));
                }
            }

            AddSepcialTmltoTree();
        }


        private List<int> _lstContentTml = new List<int>();
        //加载无分组终端节点
        private void AddSepcialTmltoTree()
        {
            _lstContentTml.Clear();
            foreach (var t in ChildTreeItems)
            {
                if (t.IsListTreeNodeGroup)
                {
                    GetChildTmlNode(t);
                }
                else
                {
                    _lstContentTml.Add(t.NodeId);
                }
            }



            List<int> lstAllSpecial = new List<int>();
            foreach (var t in Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary)
            {
                var equipmentInfo = t.Value as IIRtuParaWork;
                if (equipmentInfo == null) continue;
                if (!_lstContentTml.Contains(equipmentInfo.RtuId)) lstAllSpecial.Add(equipmentInfo.RtuId);
            }

            if (lstAllSpecial.Count > 0)
            {
                var f = new ListTreeGroupNode() {NodeId = 0, NodeName = "未分组终端"};
                var atttmp = Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(lstAllSpecial);

                foreach (var t in atttmp)
                {
                    f.ChildTreeItems.Add(new ListTreeTmlNode(f, t));
                }
                this.ChildTreeItems.Add(f);
                foreach (var t in f.ChildTreeItems)
                {
                    t.IsThisTmlSpecialTerminal = true;
                    t.IsThisTmlSpecialTerminalEnable = false;
                }
                f.IsThisGroupHasSpecialTermial = true;
            }
        }

        //计算分组节点中的终端  终端信息
        private void GetChildTmlNode(ListTreeNodeBase node)
        {
            foreach (var t in node.ChildTreeItems)
            {
                if (t.IsListTreeNodeGroup)
                {
                    GetChildTmlNode(t);
                }
                else
                {
                    _lstContentTml.Add(t.NodeId);
                }
            }
        }


        #endregion

        #region tab iinterface

        private string _msg;

        public string Msg
        {
            get { return _msg; }
            set
            {
                if (value == _msg) return;
                _msg = value;
                RaisePropertyChanged(() => Msg);
            }
        }

        public string Title
        {
            get
            {
                //return I36N .Services.I36N .ConvertByCodingOne("11020016", "全局分组");
                return "时间表与终端绑定";
            }
        }


        public bool CanClose
        {
            get { return true; }
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

        private DateTime dtSnd;
        private bool _canReflesh;
        private DateTime[] _datetime=new DateTime[2];

        #region  CmdReflesh

        private ICommand _cmdReflesh;

        public ICommand CmdReflesh
        {
            get { return _cmdReflesh ?? (_cmdReflesh = new RelayCommand(ExCmdReflesh, CanExCmdReflesh, false)); }
        }

        private bool CanExCmdReflesh()
        {
            return _canReflesh && DateTime.Now.Ticks-_datetime[0].Ticks>30000000;
        }

        private void ExCmdReflesh()
        {
            _datetime[0] = DateTime.Now;
            NavOnLoad();

        }

        #endregion

        #region CmdSave

        private ICommand _cmdSave;

        public ICommand CmdSave
        {
            get { return _cmdSave ?? (_cmdSave = new RelayCommand(Ex, CanExSave, true)); }
        }

        private bool CanExSave()
        {
            return DateTime.Now.Ticks-_datetime[1].Ticks>30000000;
        }

        private void Ex()
        {
            _datetime[1] = DateTime.Now;

            var infoss = WlstMessageBox.Show("确认保存", "即将保存信息，是 继续，否 退出.", WlstMessageBoxType.YesNo);
            if (infoss != WlstMessageBoxResults.Yes) return;

            _lstTmlbandingTimeTable.Clear();
            foreach (var t in ChildTreeItems)
            {
                GetTmlbandingTimeTable(t);
                if (t.IsListTreeNodeGroup)
                {
                    GetGrpbandingTimeTable(t);
                }
            }
            DeleteTmlBandingTimeTableSameData();
            //todo  update time
            if (_lstTmlbandingTimeTable.Count == 0) return;
            var lst = new List<TmlWeekTimeTaleBelongInfomation>();
            foreach (var t in _lstTmlbandingTimeTable)
            {
                lst.Add(new TmlWeekTimeTaleBelongInfomation()
                            {rtu_id = t.Item1, rtuLoopId = t.Item2, timeTableId = t.Item3});
            }


            dtSnd = DateTime.Now;
            Sr.TimeTableSystem.Services.TmlLoopBngTimeTableInfoService.UpdateTmlLoopBngTt(lst);

            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在保存 ...";
            
        }

        private List<Tuple<int, int, int>> _lstTmlbandingTimeTable = new List<Tuple<int, int, int>>();
                                           //tml  loop  timetable

        private void GetGrpbandingTimeTable(ListTreeNodeBase tmlNode)
        {
            foreach (var t in tmlNode.ChildTreeItems)
            {
                GetTmlbandingTimeTable(t);
                if (t.IsListTreeNodeGroup)
                {

                    GetGrpbandingTimeTable(t);
                }
            }
        }

        private void GetTmlbandingTimeTable(ListTreeNodeBase tmlNode) //tml  loop  timetable
        {
            if (tmlNode.K1TimeTalbe > 0)
                _lstTmlbandingTimeTable.Add(new Tuple<int, int, int>(tmlNode.NodeId, 1, tmlNode.K1TimeTalbe));
            if (tmlNode.K2TimeTalbe > 0)
                _lstTmlbandingTimeTable.Add(new Tuple<int, int, int>(tmlNode.NodeId, 2, tmlNode.K2TimeTalbe));
            if (tmlNode.K3TimeTalbe > 0)
                _lstTmlbandingTimeTable.Add(new Tuple<int, int, int>(tmlNode.NodeId, 3, tmlNode.K3TimeTalbe));
            if (tmlNode.K4TimeTalbe > 0)
                _lstTmlbandingTimeTable.Add(new Tuple<int, int, int>(tmlNode.NodeId, 4, tmlNode.K4TimeTalbe));
            if (tmlNode.K5TimeTalbe > 0)
                _lstTmlbandingTimeTable.Add(new Tuple<int, int, int>(tmlNode.NodeId, 5, tmlNode.K5TimeTalbe));
            if (tmlNode.K6TimeTalbe > 0)
                _lstTmlbandingTimeTable.Add(new Tuple<int, int, int>(tmlNode.NodeId, 6, tmlNode.K6TimeTalbe));
        }

        private void DeleteTmlBandingTimeTableSameData()
        {
            return;
            var lst = (from t in _lstTmlbandingTimeTable where t.Item3 == 0 select t).ToList();
            foreach (var t in lst)
            {
                _lstTmlbandingTimeTable.Remove(t);
            }
            return;

            foreach (
                var t in Sr.TimeTableSystem.Services.TmlLoopBngTimeTableInfoService.TmlWeekTableBelongInfoDictionary)
            {
                //t.Key  is  terminal id 
                foreach (var g in t.Value)
                {
                    //g.key is terminal loop;g.value is timetable id
                    var tmp = new Tuple<int, int, int>(t.Key, g.Key, g.Value);
                    if (_lstTmlbandingTimeTable.Contains(tmp))
                    {
                        _lstTmlbandingTimeTable.Remove(tmp);
                    }
                }
            }

            for (int i = _lstTmlbandingTimeTable.Count - 1; i >= 0; i--)
            {
                int rtuid = _lstTmlbandingTimeTable[i].Item1;
                int loopid = _lstTmlbandingTimeTable[i].Item2;
                if (Sr.TimeTableSystem.Services.TmlLoopBngTimeTableInfoService.TmlWeekTableBelongInfoDictionary.
                        ContainsKey(rtuid)
                    &&
                    Sr.TimeTableSystem.Services.TmlLoopBngTimeTableInfoService.TmlWeekTableBelongInfoDictionary[rtuid].
                        ContainsKey(
                            loopid))
                {
                }
                else
                {
                    if (_lstTmlbandingTimeTable[i].Item3 < 1)
                        _lstTmlbandingTimeTable.RemoveAt(i);
                }
            }
        }


        #endregion

        private ObservableCollection<ListTreeNodeBase> _childTreeItemsInfo;

        public ObservableCollection<ListTreeNodeBase> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                    _childTreeItemsInfo = new ObservableCollection<ListTreeNodeBase>();
                return _childTreeItemsInfo;
            }
            set
            {
                if (value == _childTreeItemsInfo) return;
                _childTreeItemsInfo = value;
                this.RaisePropertyChanged(() => ChildTreeItems);
            }
        }


        private bool _isActive = false;
        public void NavOnLoad(params object[] parsObjects)
        {
            this.LoadNode();
            Msg = "";
            _canReflesh = false;
            dtSnd = DateTime.Now.AddDays(-1);
            _isActive = true;
        }

        public void OnUserHideOrClosing()
        {
            //throw new NotImplementedException();
            ChildTreeItems = new ObservableCollection<ListTreeNodeBase>();
            _isActive = false;
        }

        #region update time banding show

        public void UpdatRtuTimeTable(bool isGroup, int rtuIdOrGroupId, int newTimeTabelId, int kLoops,
                                      int applyRtuCls)
        {
            foreach (var t in ChildTreeItems)
            {
                if (t.NodeId == rtuIdOrGroupId)
                {
                    if (t.IsListTreeNodeGroup)
                        UpdateGroupTimeTable(t, kLoops, newTimeTabelId, applyRtuCls);
                    else
                    {
                        UpdateNodeTimeTable(t, kLoops, newTimeTabelId);
                    }
                }
                else if (t.IsListTreeNodeGroup)
                {
                    UpdatRtuTimeTable(t, isGroup, rtuIdOrGroupId, newTimeTabelId, kLoops,
                                      applyRtuCls);
                }
            }

        }

        /// <summary>
        /// 递归查询跟新分组或终端
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isGroup"></param>
        /// <param name="rtuIdOrGroupId"></param>
        /// <param name="newTimeTabelId"></param>
        /// <param name="kLoops"></param>
        /// <param name="applyRtuCls"></param>
        public void UpdatRtuTimeTable(ListTreeNodeBase node, bool isGroup, int rtuIdOrGroupId, int newTimeTabelId,
                                      int kLoops, int applyRtuCls)
        {
            foreach (var t in node.ChildTreeItems)
            {
                if (t.NodeId == rtuIdOrGroupId)
                {
                    if (t.IsListTreeNodeGroup)
                        UpdateGroupTimeTable(t, kLoops, newTimeTabelId, applyRtuCls);
                    else
                    {
                        UpdateNodeTimeTable(t, kLoops, newTimeTabelId);
                    }
                }
                else if (t.IsListTreeNodeGroup)
                {
                    UpdatRtuTimeTable(t, isGroup, rtuIdOrGroupId, newTimeTabelId, kLoops,
                                      applyRtuCls);
                }
            }
        }

        /// <summary>
        /// 具体实现更新本分组
        /// </summary>
        /// <param name="node"></param>
        /// <param name="loop"></param>
        /// <param name="newTimeTableId"></param>
        /// <param name="applyRtuCls"></param>
        private void UpdateGroupTimeTable(ListTreeNodeBase node, int loop, int newTimeTableId,
                                          int applyRtuCls)
        {
            UpdateNodeTimeTable(node, loop, newTimeTableId);


            foreach (var t in node.ChildTreeItems)
            {
                if (t.IsListTreeNodeGroup)
                {

                    if (applyRtuCls == 2 || applyRtuCls == 4)
                        UpdateGroupTimeTable(t, loop, newTimeTableId, applyRtuCls);
                }
                else
                {
                    if (applyRtuCls == 1|| applyRtuCls == 2)
                    {
                        if (!t.IsThisTmlSpecialTerminal)
                        {
                            UpdateNodeTimeTable(t, loop, newTimeTableId);
                        }
                    }
                    else if (applyRtuCls == 3 || applyRtuCls == 4)
                    {
                        UpdateNodeTimeTable(t, loop, newTimeTableId);
                    }

                }
            }
            CalcateGroupSame(node);
        }

        /// <summary>
        /// 具体实现跟新本终端
        /// </summary>
        /// <param name="node"></param>
        /// <param name="loop"></param>
        /// <param name="newTimeTableId"></param>
        private void UpdateNodeTimeTable(ListTreeNodeBase node, int loop, int newTimeTableId)
        {
            switch (loop)
            {
                case 1:
                    node.K1TimeTalbe = newTimeTableId;
                    break;
                case 2:
                    node.K2TimeTalbe = newTimeTableId;
                    break;
                case 3:
                    node.K3TimeTalbe = newTimeTableId;
                    break;
                case 4:
                    node.K4TimeTalbe = newTimeTableId;
                    break;
                case 5:
                    node.K5TimeTalbe = newTimeTableId;
                    break;
                case 6:
                    node.K6TimeTalbe = newTimeTableId;
                    break;
            }
        }


        private void CalcateGroupSame(ListTreeNodeBase node)
        {
            if (!node.IsListTreeNodeGroup && node.Father != null)
            {
                if (node.K1TimeTalbe == node.Father.K1TimeTalbe &&
                    node.K2TimeTalbe == node.Father.K2TimeTalbe &&
                    node.K3TimeTalbe == node.Father.K3TimeTalbe &&
                    node.K4TimeTalbe == node.Father.K4TimeTalbe &&
                    node.K5TimeTalbe == node.Father.K5TimeTalbe &&
                    node.K6TimeTalbe == node.Father.K6TimeTalbe
                    )
                    node.IsThisTmlSpecialTerminal = false;
                else
                    node.IsThisTmlSpecialTerminal = true;

                return;
            }

            foreach (var t in node .ChildTreeItems )
            {
                CalcateGroupSame(t);
            }


            bool bolHasSpecial = false;
            foreach (var t in node.ChildTreeItems)
            {
                if (t.IsListTreeNodeGroup)
                {
                    if (t.IsThisGroupHasSpecialTermial) bolHasSpecial = true;
                }
                else
                {
                    if (t.IsThisTmlSpecialTerminal) bolHasSpecial = true;
                }
            }
            node.IsThisGroupHasSpecialTermial = bolHasSpecial;
        }
        #endregion
    };

}
