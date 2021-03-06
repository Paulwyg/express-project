﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using System.Xml.Serialization;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.Services;
using Wlst.client;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.ViewModel
{
    [Export(typeof (IIEquipmentFaultDefineSettingViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentFaultDefineSettingViewModel :
        Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged,
        IIEquipmentFaultDefineSettingViewModel
    {
        public EquipmentFaultDefineSettingViewModel()
        {
            //PrismEventExtend.EventHelper.EventPublisher.AddEventSubScriptionTokener(
            //    Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
            //    FundOrderFilter);
            InitEvent();
        }


        public void NavOnLoad(params object[] parsObjects)
        {
            Records.Clear();
            _isvmsettime = true;
            this.TimeLong = Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetTimeAlarmLong;
            _isvmsettime = false;

            this.VolBelow = Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetVolBelow;
            LdlBelow = Convert .ToInt32(Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetLdlBelow*100); 

            var tmpg = new ObservableCollection<TmlFaultTypeViewModel>();
         //   bool allNotSelected = true;
            var tmpss =
                (from t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary
                 orderby t.Key ascending 
                 select t.Value).ToList();
            foreach (var t in tmpss)
            {
                var f = new TmlFaultTypeViewModel(t);
                if (f.PriorityLevel == 0) f.PriorityLevel = 2;
                tmpg.Add(f);
              //  if (t.Value.IsEnable) allNotSelected = false;
            }
            this.Records = tmpg;
            //if (allNotSelected)
            //{
            //    foreach (var t in this.Records) t.IsEnable = true;
            //}

            _dtSave = DateTime.Now.AddHours(-1);
            //this._recordsHash = this.GetObservableCollectionHashCode(Records);

            bool noselected = true;
            foreach (var t in this .Records )
            {
                if (t.IsEnable)
                {
                    noselected = false;
                    break;
                }
            }
            if(noselected )
            {
                foreach (var t in this.Records)
                {
                    t.IsEnable = true;
                }
            }
        }

        public void OnUserHideOrClosing()
        {
            this.Records = new ObservableCollection<TmlFaultTypeViewModel>();
        }

        private ObservableCollection<TmlFaultTypeViewModel> _record;

        public ObservableCollection<TmlFaultTypeViewModel> Records
        {
            get { return _record ?? (_record = new ObservableCollection<TmlFaultTypeViewModel>()); }
            set
            {
                if (value == _record) return;
                _record = value;
                this.RaisePropertyChanged(() => Records);
            }
        }

        private TmlFaultTypeViewModel _currentSelectItem;

        public TmlFaultTypeViewModel CurrentSelectItem
        {
            get { return _currentSelectItem; }
            set
            {
                if (_currentSelectItem != value)
                {
                    _currentSelectItem = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectItem);
                }
            }
        }


       
        private string _showMsg;
        /// <summary>
        /// 显示在主界面上 提示用户更新情况
        /// </summary>
        public string ShowMsg
        {
            get { return _showMsg; }
            set
            {
                if (_showMsg != value)
                {
                    _showMsg = value;
                    this.RaisePropertyChanged(() => this.ShowMsg);

                }
            }
        }


        private bool _isvmsettime;
        private int  _time;

        /// <summary>
        /// 时间统计有效时间  时间为小时 大于等于1 小于 1440 
        /// </summary>
        public int TimeLong
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    if (value < 1) value = 1;
                    if (value > 1440) value = 1440;
                    _time = value;
                    this.RaisePropertyChanged(() => this.TimeLong);
                    
                    if(_isvmsettime )
                    {
                        bool find = false;
                        foreach (var t in this .TimeItems )
                        {
                            if(t.Value ==value )
                            {
                                CurrentSelectedTime = t;
                                find = true;
                                break;
                            }
                        }
                        if(find ==false )
                        {
                            foreach (var t in this.TimeItems)
                            {
                                if (t.Value > value)
                                {
                                    CurrentSelectedTime = t;
                                    find = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _timeItems=null ;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> TimeItems
        {
            get
            {
                if (_timeItems == null)
                {
                    _timeItems = new ObservableCollection<NameValueInt>();
                    for (int i = 1; i < 24; i++)
                    {
                        _timeItems.Add(new NameValueInt() {Name = i + "小时", Value = i});
                    }
                    for (int i = 1; i < 46; i++)
                    {
                        _timeItems.Add(new NameValueInt() {Name = i + " 天", Value = i*24});
                    }
                }
                return _timeItems;
            }
        }

        private NameValueInt _currenttime;

        /// <summary>
        /// 时间统计有效时间  时间为小时 大于等于1 小于 1440 
        /// </summary>
        public NameValueInt CurrentSelectedTime
        {
            get { return _currenttime; }
            set
            {
                if (_currenttime == value) return;
                _currenttime = value;
                this.RaisePropertyChanged(() => this.CurrentSelectedTime);
                if(value !=null )
                {
                    TimeLong = value.Value;
                }

            }
        }

        private int  _volbelow;

        /// <summary>
        /// 检测电压 认为电压为0的 最低条件
        /// </summary>
       [Range(0.0, 600.99, ErrorMessage = "电压介于0.0到600.99")]
        public int VolBelow
        {
            get { return _volbelow; }
            set
            {
                if (_volbelow != value)
                {
                    if (value < 1) value = 1;
                    if (value > 199) value = 199;
                    _volbelow = value;
                    this.RaisePropertyChanged(() => this.VolBelow);
                }
            }
        }
        private int  _sdfsdsdfldl;

        /// <summary>
        /// 检测亮灯率低条件 最低条件
        /// </summary>
        public int  LdlBelow
        {
            get { return _sdfsdsdfldl; }
            set
            {
                if (_sdfsdsdfldl != value)
                {
                    if (value < 30) value = 30;
                    if (value > 95) value = 95;
                    _sdfsdsdfldl = value;
                    this.RaisePropertyChanged(() => this.LdlBelow);
                }
            }
        }

        private DateTime _dtAdd;
        private DateTime _dtSave;
        private DateTime _dtDelete;

        #region CmdAdd

        private ICommand _cmdAdd;

        public ICommand CmdAdd
        {
            get { return _cmdAdd ?? (_cmdAdd = new RelayCommand(ExCmdAdd, CanExCmdAdd, true)); }
        }

        private void ExCmdAdd()
        {
            _dtAdd = DateTime.Now;
            var info = new TmlFaultTypeViewModel(new FaultTypes.FaultTypeItem()   
                                                     {
                                                       
                                                         Color = "#FFFFFF",
                                                         FaultCheckKey = "",
                                                         FaultId = 0,
                                                         FaultName = "自定义故障",
                                                         FaultNameByDefine = "自定义故障",
                                                         FaultRemak = "",
                                                         IsEnable = false  ,
                                                         AlarmTimeSet = 0,
                                                         AlarmTimeStart = 0,
                                                         AlarmTimeEnd = 0,
                                                         PriorityLevel = 0,



                                                     });
            this.Records.Add(info);
            if (Records.Count>0)
            {
                this.Records.Last().SelectColor = new TmlFaultTypeViewModel.NameColor() { Name = "黑色", Color = "#FF000000", Index = 0 };
                this.Records.Last().SelectAlarmTimeTypeIndex = new NameValueInt() { Name = "全天", Value = 0 };
                this.Records.Last().CurrentSelectProprity = new PriorityLevelHelper() { Name = "普通报警", Priority = 2 };
            }
        }

        private bool CanExCmdAdd()
        {
            return DateTime.Now.Ticks - _dtAdd.Ticks>30000000;
        }

        #endregion

        //#region CmdSetDlSxx

        //private ICommand _cmdCmdSetDlSxxAdd;

        //public ICommand CmdSetDlSxx
        //{
        //    get { return _cmdCmdSetDlSxxAdd ?? (_cmdCmdSetDlSxxAdd = new RelayCommand(ExCmdCmdSetDlSxx, CanExCmdSetDlSxx, true)); }
        //}

        //private void ExCmdCmdSetDlSxx()
        //{
        //    RtuAmpSxx.NavToRtuAmpSxx.NavTo();
        //}

        //private bool CanExCmdSetDlSxx()
        //{
        //    return true;
        //}

        //#endregion

        #region save all

        public ICommand CmdSaveAll
        {
            get { return new RelayCommand(Ex, CanEx, true); }
        }

        private void Ex()
        {


            _dtSave = DateTime.Now;
            var lst = new List<Wlst .client .FaultTypes .FaultTypeItem  >();
            foreach (var t in this.Records)
            {
                lst.Add(t.GetTmlFaultType());
            }


            //bool noselect = true;
            //foreach (var t in Records)
            //{
            //    if (t.IsEnable)
            //    {
            //        noselect = false;
            //        break;
            //    }
            //}
            //bool allselect = true;
            //foreach (var t in Records)
            //{
            //    if (t.IsEnable == false)
            //    {
            //        allselect = false;
            //        break;
            //    }
            //}
            //if (noselect || allselect)
            //{
            //    foreach (var t in lst) t.IsEnable = true;
            //}
            Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.ExUpdateFauleTypeInfoforServer(lst, TimeLong,
                                                                                                    VolBelow,
                                                                                                    LdlBelow*0.01);
            // this._recordsHash = GetObservableCollectionHashCode(Records);
            this.ShowMsg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 数据更新已经提交服务器,请等待......";
        }


        private string _recordsHash = "";

        private bool CanEx()
        {
           // string yy = GetObservableCollectionHashCode(Records);
            return DateTime.Now.Ticks - _dtSave.Ticks > 30000000 && Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D;
        }

        //public string GetObservableCollectionHashCode(object observableCollectionRequest)
        //{
        //    BinaryFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
        //    System.IO.MemoryStream memStream = new System.IO.MemoryStream();
        //    bf.Serialize(memStream, observableCollectionRequest);


        //    char[] chars = new char[memStream.Length];


        //    int lenght = System.Convert.ToInt32(memStream.Length);
        //    var sr = new MD5CryptoServiceProvider().ComputeHash(memStream.GetBuffer(), 0, lenght);
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < sr.Length; i++)
        //    {
        //        sb.Append(sr[i].ToString("x2"));
        //    }
        //    return sb.ToString();

        //    //System.Text.Encoding.UTF8.GetDecoder().GetChars(memStream.GetBuffer(), 0, lenght, chars, 0);
        //    //return chars.GetHashCode( ) ;

        //}



        #endregion

        #region CmdDelete

        public ICommand CmdDelete
        {
            get { return new RelayCommand(ExCmdDelete, CanExCmdDelete, true); }
        }

        private void ExCmdDelete()
        {
            _dtDelete = DateTime.Now;
            if (CurrentSelectItem != null && this.Records.Contains(CurrentSelectItem))
            {
                this.Records.Remove(CurrentSelectItem);
            }
        }

        private bool CanExCmdDelete()
        {
            return CurrentSelectItem != null && CurrentSelectItem.FaultId >80 && DateTime.Now.Ticks-_dtDelete.Ticks>30000000;
        }

        #endregion

        #region tab

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
            get
            {
                //var eng = I36N.Services.I36N.ConvertByCoding(EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultDefineSettingViewId.ToString()); //

                //if (!string.IsNullOrEmpty(eng) && !eng.Contains("issing")) return eng;
                return "自定义故障设置";
            }
        }

        #endregion

        #region IsChecked
        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    this.RaisePropertyChanged(() => this.IsChecked);
                }
            }
        }
        #endregion




    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class EquipmentFaultDefineSettingViewModel
    {
        private void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipemntLightFault.Services.EventIdAssign.FaultTypeRequest,
                                    PublishEventType.Core);
            this.AddEventFilterInfo(Sr.EquipemntLightFault.Services.EventIdAssign.FaultTypeUpdateId,
                                    PublishEventType.Core);
        }



        public override void ExPublishedEvent(
            Microsoft.Practices.Prism.MefExtensions.Event.EventHelper.PublishEventArgs args)
        {
            //base.ExPublishedEvent(args);
            this.NavOnLoad();

            if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.FaultTypeUpdateId)
            {
                this.ShowMsg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  服务器端数据更新.";
            }
        }

    }
}
