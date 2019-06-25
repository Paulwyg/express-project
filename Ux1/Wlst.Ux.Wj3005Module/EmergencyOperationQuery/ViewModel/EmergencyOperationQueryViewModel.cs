using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Ux.WJ3005Module.EmergencyOperationQuery.Services;
using Wlst.Cr.Core.EventHandlerHelper;

namespace Wlst.Ux.WJ3005Module.EmergencyOperationQuery.ViewModel
{
    [Export(typeof(IIEmergencyOperationQuery))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EmergencyOperationQueryViewModel : EventHandlerHelperExtendNotifyProperyChanged,IIEmergencyOperationQuery
    {
        public EmergencyOperationQueryViewModel()
        {
            InitAction();
        }
        private bool _isViewActive = false;
        public void NavOnLoad(params object[] parsObjects)
        {
            _isViewActive = true;
            Items.Clear();
            BeginDate = new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day,0,0,0);
            EndDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,23,59,59);
            Remind = "请设置好查询日期...";
        }
        public void OnUserHideOrClosing()
        {
            _isViewActive = false;
        }

        #region tab iinterface

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "应急操作查询"; }
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

        #region BeginDate
        private DateTime _beginDate;
        /// <summary>
        /// 查询开始时间
        /// </summary>
        public DateTime BeginDate
        {
            get
            {
                return _beginDate;
            }
            set
            {
                if (value == _beginDate) return;
                _beginDate = value;
                RaisePropertyChanged(() => BeginDate);
            }
        }
        #endregion

        #region  EndDate

        private DateTime _endDate;
        /// <summary>
        /// 查询结束时间
        /// </summary>
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (value == _endDate) return;
                _endDate = value;
                RaisePropertyChanged(() => EndDate);
            }
        }

        #endregion

        #region Remind

        private string _remind;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Remind
        {
            get { return _remind; }
            set
            {
                if (value == _remind) return;
                _remind = value;
                RaisePropertyChanged(() => Remind);
            }
        }
        #endregion

        #region Items

        private ObservableCollection<EmergencyOperatorOneRecordViewModel> _items;
        /// <summary>
        /// 查询结果信息
        /// </summary>
        public ObservableCollection<EmergencyOperatorOneRecordViewModel> Items
        {
            get { return _items ?? (_items = new ObservableCollection<EmergencyOperatorOneRecordViewModel>()); }
            set
            {
                if (_items == value) return;
                _items = value;
                this.RaisePropertyChanged(() => this.Items);
            }
        }

        #endregion

        #region IsD


        private bool _cheIsD;

        public bool IsD
        {
            get { return _cheIsD; }
            set
            {
                if (value != _cheIsD)
                {
                    _cheIsD = value;
                    RaisePropertyChanged(() => IsD);
                }
            }
        }



        #endregion

    }

    public partial class EmergencyOperationQueryViewModel
    {
        #region CmdQuery

        private DateTime _dtQuery;
        private ICommand _cmdQuery;
        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand CmdQuery
        {
            get { return _cmdQuery ?? (_cmdQuery = new RelayCommand(ExCmdQuery, CanCmdQuery, true)); }
        }
        private void ExCmdQuery()
        {
            _dtQuery = DateTime.Now;
            Query(BeginDate,EndDate);
            Remind = "查询命令已发送...，请等待数据反馈！";
        }
        private bool CanCmdQuery()
        {
            return DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;
        }
        #endregion

        private void Query(DateTime dtstarttime, DateTime dtendtime)
        {
            var info = Wlst.Sr.ProtocolPhone.LxEmeOper.wst_emergence_record_query;
            info.WstEmergenceRecordQuery.DtEndTime = new DateTime(dtendtime.Year, dtendtime.Month, dtendtime.Day, 23, 59, 59).Ticks;
            info.WstEmergenceRecordQuery.DtStartTime =new DateTime(dtstarttime.Year,dtstarttime.Month, dtstarttime.Day,0,0,0).Ticks;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询，请等待...";
        }
    }

    public partial class EmergencyOperationQueryViewModel
    {
        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxEmeOper.wst_emergence_record_query,
                OnRequestEmergencyOperation,
                typeof(EmergencyOperationQueryViewModel), this);
        }

        public void OnRequestEmergencyOperation(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var infoss = infos.WstEmergenceRecordQuery;
            if (infoss == null) return;
            Items.Clear();
            int i = 0;
            foreach (var t in infoss.Items)
            {
                i++;
                var groupidx = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(t.RtuId);
                var groupid = "";
                if(groupidx!=null)
                {             
                    var infosss =
                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(
                            groupidx.Item1, groupidx.Item2);
                    if(infosss!=null) groupid = infosss.GroupName;
                }
                var tmp = new EmergencyOperatorOneRecordViewModel()
                              {
                                  RecordIndex = i,
                                  RtuId = t.RtuId,
                                  OperatorType =
                                      t.OrderType == 1
                                          ? "开灯"
                                          : t.OrderType == 2 ? "关灯" : t.OrderType == 0 ? "用户操作" : "下发周设置",
                                  UpLoadOrDownLoad = t.IsSnd == 1 ? "下发" : "应答",
                                  Time = new DateTime(t.DateCreate).ToString(),
                                  Remark = t.Remark,
                                  Group = groupid,
                                  Loop =
                                      t.LoopId == 13
                                          ? "K1-K3"
                                          : t.LoopId == 46 ? "K4-K6" : t.LoopId == 78 ? "K7K8" : (t.LoopId + "")
                              };
                if (t.OrderType == 0)  //如果是手动关闭开启智能模式
                {
                    tmp.UpLoadOrDownLoad = "---";
                    tmp.Loop = "---";
                    tmp.RtuName = "---";
                }

                Items.Add(tmp);
            }
            Remind = "数据装载完毕...请查看...";
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 操作记录查询成功，共计" + infoss.Items.Count +
                     " 条数据.";
        }
    }

    public class EmergencyOperatorOneRecordViewModel : EventHandlerHelperExtendNotifyProperyChanged
    {
        #region 序号

        private int _recordIndex;
        public int RecordIndex
        {
            get { return _recordIndex; }
            set
            {
                if (value == _recordIndex) return;
                _recordIndex = value;
                RaisePropertyChanged(() => RecordIndex);
            }
        }


        #endregion

        #region Time

        private string _time;
        public string Time
        {
            get { return _time; }
            set
            {
                if (_time == value) return;
                _time = value;
                RaisePropertyChanged(() => Time);
            }
        }

        #endregion

        #region RtuName
        private string _rtuName;

        /// <summary>
        /// 
        /// </summary>
        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    RaisePropertyChanged(() => RtuName);
                }
            }
        }
        #endregion

        #region PhyId
        private int _phyId;

        /// <summary>
        /// 
        /// </summary>
        public int PhyId
        {
            get { return _phyId; }
            set
            {
                if (value == _phyId) return;
                _phyId = value;
                RaisePropertyChanged(() => PhyId);
            }
        }
        #endregion

        #region RtuId
        private int _rtuId;

        /// <summary>
        /// 
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value == _rtuId) return;
                _rtuId = value;
                //PhyId = value;
                RaisePropertyChanged(() => RtuId);
                RtuName = "";
                if (
                    !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                         InfoItems.ContainsKey
                         (_rtuId))
                    return;
                var tml =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                        [_rtuId];
                RtuName = tml.RtuName;
                PhyId = tml.RtuPhyId;
                //PhyId = tml.PhyId;
            }
        }
        #endregion

        #region Loop
        private string _loop;
        public string Loop
        {
            get { return _loop; }
            set
            {
                if (_loop == value) return;
                _loop = value;
                RaisePropertyChanged(() => Loop);
            }
        }
        #endregion

        #region OperatorType
        private string _operatorType;
        public string OperatorType
        {
            get { return _operatorType; }
            set
            {
                if (_operatorType == value) return;
                _operatorType = value;
                RaisePropertyChanged(() => OperatorType);
            }
        }
        #endregion

        #region UpLoadOrDownLoad
        private string _upLoadOrDownLoad;
        public string UpLoadOrDownLoad
        {
            get { return _upLoadOrDownLoad; }
            set
            {
                if (_upLoadOrDownLoad == value) return;
                _upLoadOrDownLoad = value;
                RaisePropertyChanged(() => UpLoadOrDownLoad);
            }
        }
        #endregion

        #region Remark
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set
            {
                if (_remark == value) return;
                _remark = value;
                RaisePropertyChanged(() => Remark);
            }
        }
        #endregion

        #region Group
        private string _group;
        public string Group
        {
            get { return _group; }
            set
            {
                if (_group == value) return;
                _group = value;
                RaisePropertyChanged(() => Group);
            }
        }
        #endregion
    }
}
