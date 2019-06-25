using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;


namespace Wlst.Ux.EquipemntLightFault.RecordMsg
{

    [Export(typeof (IIRecordMsg))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class RecordMsgViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IIRecordMsg
    {
        public RecordMsgViewModel()
        {

            InitAction();
            //Thread.CurrentThread.CurrentCulture = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();

            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd HH:mm:ss";
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
        }
        public void OnUserHideOrClosing()
        {
            RecordItems = new ObservableCollection<MsgRecord>();
            ExportVisi=Visibility.Collapsed;
            _isViewShowd = false;
        }
           bool _isViewShowd = false;

        public void NavOnLoad(params object[] parsObjects)
        {
            _isViewShowd = true;
            DtStartTime = DateTime.Now.AddDays(-1);
            DtEndTime = DateTime.Now;

            var info = Wlst.Sr.ProtocolPhone .LxLogin  .wst_request_user_info  ;//.ProtocolCnt.ServerPart.wlst_EquipemntLightFault_clinet_request_AllUserInfomationOne;
            SndOrderServer.OrderSnd(info, 10, 2);
        }
        #region tab
        public int Index
        {
            get { return 1; }
        }
        public bool CanClose
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public string Title
        {
            get { return "短信记录"; }
        }

        #endregion

    }

    public partial class RecordMsgViewModel
    {
        #region Field
        private bool _isOnExport = true;
        #endregion

        #region  attri

        #region ExportVisi

        private Visibility _exportVisi = Visibility.Collapsed;
        public Visibility ExportVisi
        {
            get { return _exportVisi; }
            set
            {
                if (value == _exportVisi) return;
                _exportVisi = value;
                RaisePropertyChanged(() => ExportVisi);
            }
        }
        #endregion

        #region RecordItems
        private ObservableCollection<MsgRecord> _recordItems;

        public ObservableCollection<MsgRecord> RecordItems
        {
            get
            {
                if (_recordItems == null)
                {
                    _recordItems = new ObservableCollection<MsgRecord>();
                }
                return _recordItems;
            }
            set
            {
                if (value == _recordItems) return;
                _recordItems = value;
                this.RaisePropertyChanged(() => RecordItems);
            }
        }
        #endregion

        #region UserItems
        private ObservableCollection<NameLong> _devices;

        public ObservableCollection<NameLong> UserItems
        {
            get
            {
                if (_devices == null)
                {
                    _devices = new ObservableCollection<NameLong>();
                }
                return _devices;
            }
        }
        #endregion

        #region CurrentSelectUser
        private NameLong _currentSelectDevice;
        public NameLong CurrentSelectUser
        {
            get { return _currentSelectDevice; }
            set
            {
                if (value != _currentSelectDevice)
                {
                    _currentSelectDevice = value;
                    this.RaisePropertyChanged(() => CurrentSelectUser);
                }
            }
        }
        #endregion

        #region DtStartTime
        private DateTime _msgSndTo;
        public DateTime DtStartTime
        {
            get { return _msgSndTo; }
            set
            {
                if (_msgSndTo != value)
                {
                    _msgSndTo = value;
                    this.RaisePropertyChanged(() => DtStartTime);
                }
            }
        }
        #endregion

        #region DtEndTime
        private DateTime _msgSnd;

        public DateTime DtEndTime
        {
            get { return _msgSnd; }
            set
            {
                if (_msgSnd != value)
                {
                    _msgSnd = value;
                    this.RaisePropertyChanged(() => DtEndTime);
                }
            }
        }
        #endregion

        #region  ShowDetailMsg
        private string _showDetailMsg;
        public string ShowDetailMsg
        {
            get { return _showDetailMsg; }
            set
            {
                if (_showDetailMsg == value) return;
                _showDetailMsg = value;
                RaisePropertyChanged(() => ShowDetailMsg);
            }
        }
        #endregion

        #endregion

        #region ICommand

        #region CmdQuery

        private DateTime _dtQuerty;
        private ICommand _CmdQuery;

        public ICommand CmdQuery
        {
            get
            {
                if (_CmdQuery == null) _CmdQuery = new RelayCommand(Ex, CanEx, true);
                return _CmdQuery;
            }
        }

        private bool CanEx()
        {

            //if ((DateTime.Now.Ticks - _dtQuerty.Ticks > 30000000) && DtStartTime.Ticks < DtEndTime.Ticks && CurrentSelectUser != null) return true;
            if ((DateTime.Now.Ticks - _dtQuerty.Ticks > 30000000) && DtStartTime.Ticks < DtEndTime.Ticks) return true;
            return false;
        }

        public void Ex()
        {
            //RecordItems.Clear();
            if (DtEndTime.Subtract(DtStartTime) > TimeSpan.FromDays(30))
            {
                UMessageBox.Show("时间跨度", "选择的时间不能大于一个月，请重新选择", UMessageBoxButton.Ok);
                return;
            }
            _dtQuerty = DateTime.Now;
            var info = Wlst.Sr.ProtocolPhone .LxFault  .wlst_msg_record  ;//.ProtocolCnt.ServerPart.wlst_EquipemntLightFault_clinet_request_msgRecordInfo;
            if (CurrentSelectUser != null)
            {
                info.WstFaultMsgRecord  .UserName = CurrentSelectUser.Name;
                info.WstFaultMsgRecord.UserPhoneNumber = CurrentSelectUser.Value;
            }


            var tStartTime = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 1);
            var tEndTime = new DateTime(DtEndTime.Year, DtEndTime.Month, DtEndTime.Day, 23, 59, 59);


            info.WstFaultMsgRecord.DtEndTime = tEndTime.Ticks;
            info.WstFaultMsgRecord.DtStartTime = tStartTime.Ticks;
            SndOrderServer.OrderSnd(info, 10, 6);
            _isOnExport = false;
            ExportVisi=Visibility.Visible;
         //   ShowDetailMsg = "查询命令已发送...请等待数据反馈！！！";
           ShowDetailMsg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在查询 ...";
        }

        #endregion

        #endregion
    }
    /// <summary>
    /// Socket
    /// </summary>
    public partial class RecordMsgViewModel
    {
        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone .LxLogin  .wst_request_user_info  ,//.ProtocolCnt.ClientPart.wlst_EquipemntLightFault_server_ans_clinet_request_AllUserInfomationOne,
                OnRequestAllUserInfomationBack,
                typeof (RecordMsgViewModel), this,true);
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone .LxFault  .wlst_msg_record  ,//.ProtocolCnt.ClientPart.wlst_EquipemntLightFault_server_ans_clinet_request_msgRecordInfo,
                OnRequestUserRtuAlarmsInfoBack,
                typeof (RecordMsgViewModel), this,true);
        }

        public void OnRequestAllUserInfomationBack(string session,Wlst .mobile .MsgWithMobile   infos)
        {
            if (_isViewShowd == false) return;

            this.UserItems.Clear();
            foreach (var t in infos.WstLoginRequestUserInfo   .UserInfo)
            {
                try
                {
                    long x = Convert.ToInt64(t.PhoneNumber);
                    UserItems.Add(new NameLong() {Name = t.UserName, Value = x});
                }
                catch (Exception ex)
                {

                }

            }

        }


        public void OnRequestUserRtuAlarmsInfoBack(string session,Wlst .mobile .MsgWithMobile   infos)
        {
            RecordItems.Clear();

            foreach (var t in infos.WstFaultMsgRecord  .Items )
            {
                this.RecordItems.Add(new MsgRecord()
                                         {
                                             CreateTime = new DateTime(t.DateCreate),
                                             Msg = t.Msg,
                                             MoveHereTime = new DateTime(t.MoveHereTime),
                                             MoveType = t.MoveType == 0 ? "自动" : "人为手动",
                                             MsgState = t.MsgState == 1 ? "发送成功" : t.MsgState == 2 ? "超次数失败" : "未知",
                                             RecordId = t.RecordID,
                                             Remark = t.Remark,
                                             SndTime = t.TimeSndList,
                                             SndTimes = t.SndTimes,
                                             StationResponseID = t.StationResponseID,
                                             UserName = t.UserName,
                                             UserPhoneNumber = t.UserPhoneNumber
                                         });
            }
            ShowDetailMsg =DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 数据查询成功，共计"+this .RecordItems .Count +" 条数据";
        }
    }

    public class MsgRecord : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        private int _recordId;

        public int RecordId
        {
            get { return _recordId; }
            set
            {
                if (value != _recordId)
                {
                    _recordId = value;
                    this.RaisePropertyChanged(() => RecordId);
                }
            }
        }



        private DateTime _createTime;

        public DateTime CreateTime
        {
            get { return _createTime; }
            set
            {
                if (value != _createTime)
                {
                    _createTime = value;
                    this.RaisePropertyChanged(() => CreateTime);
                }
            }
        }


        private string _sndTime;

        public string SndTime
        {
            get { return _sndTime; }
            set
            {
                if (value != _sndTime)
                {
                    _sndTime = value;
                    this.RaisePropertyChanged(() => SndTime);
                }
            }
        }

        private string _msg;

        public string Msg
        {
            get { return _msg; }
            set
            {
                if (value != _msg)
                {
                    _msg = value;
                    this.RaisePropertyChanged(() => Msg);
                }
            }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    this.RaisePropertyChanged(() => UserName);
                }
            }
        }

        private long _userPhoneNumber;

        public long UserPhoneNumber
        {
            get { return _userPhoneNumber; }
            set
            {
                if (value != _userPhoneNumber)
                {
                    _userPhoneNumber = value;
                    this.RaisePropertyChanged(() => UserPhoneNumber);
                }
            }
        }

        private int _sndTimes;

        public int SndTimes
        {
            get { return _sndTimes; }
            set
            {
                if (value != _sndTimes)
                {
                    _sndTimes = value;
                    this.RaisePropertyChanged(() => SndTimes);
                }
            }
        }

        private string _msgState;

        public string MsgState
        {
            get { return _msgState; }
            set
            {
                if (value != _msgState)
                {
                    _msgState = value;
                    this.RaisePropertyChanged(() => MsgState);
                }
            }
        }

        private int _stationResponseID;

        public int StationResponseID
        {
            get { return _stationResponseID; }
            set
            {
                if (value != _stationResponseID)
                {
                    _stationResponseID = value;
                    this.RaisePropertyChanged(() => StationResponseID);
                }
            }
        }

        private string _remark;

        public string Remark
        {
            get { return _remark; }
            set
            {
                if (value != _remark)
                {
                    _remark = value;
                    this.RaisePropertyChanged(() => Remark);
                }
            }
        }

        private DateTime _moveHereTime;

        public DateTime MoveHereTime
        {
            get { return _moveHereTime; }
            set
            {
                if (value != _moveHereTime)
                {
                    _moveHereTime = value;
                    this.RaisePropertyChanged(() => MoveHereTime);
                }
            }
        }

        private string _moveType;

        /// <summary>
        /// 0 为短消息告警自动移动 其他为人为移动
        /// </summary>
        public string MoveType
        {
            get { return _moveType; }
            set
            {
                if (value != _moveType)
                {
                    _moveType = value;
                    this.RaisePropertyChanged(() => MoveType);
                }
            }
        }


    }

    public class NameLong : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }

        private long _value;

        public long Value
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
}
