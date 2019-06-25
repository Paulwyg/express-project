using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.client;


namespace Wlst.Ux.Wj1050Module.Wj1050InfoSetViewModel.ViewModel
{

    public partial class TmlInfomationViewModelBase : Wlst .Cr .Core .CoreServices .ObservableObject 
    {
        public TmlInfomationViewModelBase()
        {
        }

        private Wlst .Sr .EquipmentInfoHolding .Model .Wj1050Mru  _wj1050TerminalInformation;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="luxId"></param>
        public void NavOnLoadByBase(int mruId)
        {

            if (mruId > 0)
            {
                SelectedTmlChange(mruId);
                UpdateAttachMainIdName(mruId);
            }
        }

        #region

        /// <summary>
        /// 提供外界更改终端
        /// </summary>
        /// <param name="rtuId">终端地址</param>
        public void SelectedTmlChange(int rtuId)
        {
            _wj1050TerminalInformation  = null;
            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems  .ContainsKey(rtuId))
            {
                _wj1050TerminalInformation =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems  [rtuId] as
                    Wlst .Sr .EquipmentInfoHolding .Model .Wj1050Mru ;
            }

            if (_wj1050TerminalInformation == null) return;
 
 
            InitVm(_wj1050TerminalInformation);
        }


        /// <summary>
        /// 将回路信息、输入信、输出信息还原为 终端信息
        /// </summary>
        /// <returns></returns>
        private Wlst .Sr .EquipmentInfoHolding .Model .Wj1050Mru  BackViewModelToTerminalInformation()
        {
            var mtp = new EquipmentParameter()
                          {
                            //  AreaId = _wj1050TerminalInformation.AreaId,
                              DateCreate = _wj1050TerminalInformation.DateCreate,
                              DateUpdate = _wj1050TerminalInformation.DateUpdate,
                              RtuArgu = _wj1050TerminalInformation.RtuArgu,
                              RtuInstallAddr = _wj1050TerminalInformation.RtuInstallAddr,
                              RtuId = _wj1050TerminalInformation.RtuId,
                              RtuName = RtuName,
                              RtuPhyId = _wj1050TerminalInformation.RtuPhyId,
                              RtuFid = _wj1050TerminalInformation.RtuFid,
                              RtuGisX = _wj1050TerminalInformation.RtuGisX,
                              RtuGisY = _wj1050TerminalInformation.RtuGisY,
                              RtuMapX = _wj1050TerminalInformation.RtuMapX,
                              RtuMapY = _wj1050TerminalInformation.RtuMapY,
                              RtuModel = _wj1050TerminalInformation.RtuModel,
                              RtuRemark = MruRemark,
                              RtuStateCode = _wj1050TerminalInformation.RtuStateCode,

                          };
            return new Wj1050Mru(mtp, BackToTerminal());
 
        }

        #endregion


        private void UpdateAttachMainIdName(int rtuId)
        {
            var info =
                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( rtuId);
            AttachRtuId = 0;
            AttachRtuName = "--";
            if (info == null) return;

            AttachRtuId = info.RtuFid ;
            var maininfo =
                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( AttachRtuId);
            if (maininfo == null) return;
            AttachRtuName = maininfo.RtuName;

        }

        #region  保存终端信息

        private void SubmitExecute()
        {
            _dtSaveAllCommand = DateTime.Now;
            var ins = BackViewModelToTerminalInformation();
            if (ins == null) return;

            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.UpdateEquipmentInfo(ins);

            //new LoadUpdateDeleteTerminalInfo().AddOrUpdateTmlInfomation(ins);

        }

        private DateTime _dtSaveAllCommand;
        private RelayCommand _relayCommand;

        /// <summary>
        /// 提交更新  保存终端信息你
        /// </summary>
        public ICommand SaveAllCommand
        {
            get { return _relayCommand ?? (_relayCommand = new RelayCommand(SubmitExecute, CanExSaveAll,true)); }
        }

        private bool CanExSaveAll()
    {
        return DateTime.Now.Ticks-_dtSaveAllCommand.Ticks>30000000;
    }

        #endregion

        #region AttachRtuId

        private int _AttachRtuId;
        /// <summary>
        /// 主设备地址  
        /// </summary>
        public int AttachRtuId
        {
            get { return _AttachRtuId; }
            set
            {
                if (value != _AttachRtuId)
                {
                    _AttachRtuId = value;
                    this.RaisePropertyChanged(() => this.AttachRtuId);
                }
            }
        }



        private string _AttachrtuName;

        /// <summary>
        /// 主设备名称
        /// </summary>
        public string AttachRtuName
        {
            get { return _AttachrtuName; }
            set
            {
                if (value != _AttachrtuName)
                {
                    _AttachrtuName = value;
                    this.RaisePropertyChanged(() => this.AttachRtuName);
                }
            }
        }
        #endregion

    };

    public partial class TmlInfomationViewModelBase 
    {
        private void InitVm(Wlst .Sr  .EquipmentInfoHolding .Model .Wj1050Mru   wj1050Equipment)
        {
            this.RtuId = wj1050Equipment.RtuId;
            this.RtuName = wj1050Equipment.RtuName;
            this.PhyId = wj1050Equipment .RtuPhyId ;
            this.MruAddr1 = wj1050Equipment.WjMru.MruAddr1;
            this.MruAddr2 = wj1050Equipment.WjMru.MruAddr2;
            this.MruAddr3 = wj1050Equipment.WjMru.MruAddr3;
            this.MruAddr4 = wj1050Equipment.WjMru.MruAddr4;
            this.MruAddr5 = wj1050Equipment.WjMru.MruAddr5;
            this.MruAddr6 = wj1050Equipment.WjMru.MruAddr6;
            this.MruBandRate = wj1050Equipment.WjMru.MruBaudrate;
            this.MruRatio = wj1050Equipment.WjMru.MruRatio;
            this.MruType = wj1050Equipment.WjMru.MruType;


            this.MruRemark = wj1050Equipment.RtuRemark ;
        }

        private Wlst.client.MruParameter BackToTerminal()
        {
            return new MruParameter()
                       {
                           MruAddr1 = this.MruAddr1,
                           MruAddr2 = this.MruAddr2,
                           MruAddr3 = this.MruAddr3,
                           MruAddr4 = this.MruAddr4,
                           MruAddr5 = this.MruAddr5,
                           MruAddr6 = this.MruAddr6,
                           MruBaudrate = this.MruBandRate,
                           MruRatio = this.MruRatio,
                           MruType = this.MruType,

                       };

        }

        private int _rtuId;

        /// <summary>
        /// 逻辑地址  
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value != _rtuId)
                {
                    _rtuId = value;
                    this.RaisePropertyChanged(() => this.RtuId);
                }
            }
        }

        private string _rtuName;

        /// <summary>
        /// 名称
        /// </summary>

        [StringLength(30,ErrorMessage="名称长度不能大于30")]
        [Required (ErrorMessage="输入名称不能为空")]
        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        private int _phyId;

        /// <summary>
        /// 终端地址
        /// </summary>
        public int PhyId
        {
            get { return _phyId; }
            set
            {
                if (value != _phyId)
                {
                    _phyId = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }


        private int _mruBandRate;

        /// <summary>
        /// 电表波特率
        /// </summary>
        public int MruBandRate
        {
            get { return _mruBandRate; }
            set
            {
                if (value != _mruBandRate)
                {
                    _mruBandRate = value;
                    this.RaisePropertyChanged(() => this.MruBandRate);
                }
            }
        }

        private int _mruRatio;

        /// <summary>
        /// 电表变比
        /// </summary>
        [Range(0, 1000, ErrorMessage = "")]
        public int MruRatio
        {
            get { return _mruRatio; }
            set
            {

                _mruRatio = value;
                this.RaisePropertyChanged(() => this.MruRatio);
            }
        }

        private int _mruType;

        /// <summary>
        /// 电表类型 1 ：1997协议；2 ：2007 协议
        /// </summary>
        public int MruType
        {
            get { return _mruType; }
            set
            {
                _mruType = value;
                this.RaisePropertyChanged(() => this.MruType);
                _backgroundChangeMruType = true;
                OnMruTypeChange();
                _backgroundChangeMruType = false;
               

            }
        }

        private bool _btnReaderEnabled;
        public bool BtnReaderEnabled
        {
            get { return _btnReaderEnabled; }
            set
            {
                if(value==_btnReaderEnabled) return;
                _btnReaderEnabled = value;
                RaisePropertyChanged(()=>BtnReaderEnabled);
            }
        }

        /// <summary>
        /// 1 A,2 B,3 C,4 ALL,0 None
        /// </summary>
        private int _radioType;

        public int RadioMruTypeSelectValue
        {
            get { return _radioType; }
            set
            {
                if (value != _radioType)
                {
                    _radioType = value;
                    this.RaisePropertyChanged(() => this.RadioMruTypeSelectValue);
                }
            }
        }


        private bool _backgroundChangeMruType;

        private void OnMruTypeChange()
        {
            if (MruType == 1)
            {
                //this.MruAddr1 = 153;
                //this.MruAddr2 = 153;
                //this.MruAddr3 = 153;
                //this.MruAddr4 = 153;
                //this.MruAddr5 = 153;
                //this.MruAddr6 = 153;
                this.IsMruAddrEnable = false;
                RadioMruTypeSelectValue = 4;
                BtnReaderEnabled = false;

            }
            else
            {
                this.IsMruAddrEnable = true;
                BtnReaderEnabled = true;
            }
        }

        private bool  _isMruAddrEnable;

        /// <summary>
        /// 
        /// </summary>
        public bool IsMruAddrEnable
        {
            get { return _isMruAddrEnable; }
            set
            {

                _isMruAddrEnable = value;
                this.RaisePropertyChanged(() => this.IsMruAddrEnable);
                if (!_backgroundChangeMruType)
                {
                    if (value) MruType = 2;
                    else MruType = 1;
                }
            }
        }



        private string _mruRemark;

        /// <summary>
        /// 备注
        /// </summary>
        public string MruRemark
        {
            get { return _mruRemark; }
            set
            {

                _mruRemark = value;
                this.RaisePropertyChanged(() => this.MruRemark);
            }
        }

        private int _mruAddr1;

        /// <summary>
        /// 电表地址
        /// </summary>
        public int MruAddr1
        {
            get { return _mruAddr1; }
            set
            {
                if (value > 255 || value < 0) return;
                _mruAddr1 = value;
                this.RaisePropertyChanged(() => this.MruAddr1);
            }
        }

        private int _mruAddr2;

        /// <summary>
        /// 电表地址
        /// </summary>
        public int MruAddr2
        {
            get { return _mruAddr2; }
            set
            {
                if (value > 255 || value < 0) return;
                _mruAddr2 = value;
                this.RaisePropertyChanged(() => this.MruAddr2);
            }
        }

        private int _mruAddr3;

        /// <summary>
        /// 电表地址
        /// </summary>
        public int MruAddr3
        {
            get { return _mruAddr3; }
            set
            {
                if (value > 255 || value < 0) return;
                _mruAddr3 = value;
                this.RaisePropertyChanged(() => this.MruAddr3);
            }
        }


        private int _mruAddr4;

        /// <summary>
        /// 电表地址
        /// </summary>
        public int MruAddr4
        {
            get { return _mruAddr4; }
            set
            {
                if (value > 255 || value < 0) return;
                _mruAddr4 = value;
                this.RaisePropertyChanged(() => this.MruAddr4);
            }
        }


        private int _mruAddr5;

        /// <summary>
        /// 电表地址
        /// </summary>
        public int MruAddr5
        {
            get { return _mruAddr5; }
            set
            {
                if (value > 255 || value < 0) return;
                _mruAddr5 = value;
                this.RaisePropertyChanged(() => this.MruAddr5);
            }
        }

        private int _mruAddr6;

        /// <summary>
        /// 电表地址
        /// </summary>
        public int MruAddr6
        {
            get { return _mruAddr6; }
            set
            {
                if (value > 255 || value < 0) return;
                _mruAddr6 = value;
                this.RaisePropertyChanged(() => this.MruAddr6);
            }
        }
    }
}
