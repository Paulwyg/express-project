using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.TimeTableSystem.TunnelInfo.TunnelInfoSet.ViewModel
{
    public class OneItemTerminal : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private bool _isSelected;
        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    if (_isSelected  ==false )
                    {
                        if (TunnelInformation.PassTerminalItems != null)
                        {
                            foreach (var t in TunnelInformation.PassTerminalItems)
                            {
                                if (t.RtuId == _rtuid)
                                {
                                    var infoss = WlstMessageBox.Show("警告",
                                                                     "当前终端已加入方案中，" +
                                                                     "是否继续操作？ 是 继续，否 取消.", WlstMessageBoxType.YesNo);
                                    if (infoss != WlstMessageBoxResults.Yes) _isSelected = true;
                                }
                            }
                        }
                    }
                    this.RaisePropertyChanged(() => this.IsSelected);
                }
            }
        }


        private bool _isEnable;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                if (value != _isEnable)
                {
                    _isEnable = value;
                    this.RaisePropertyChanged(() => this.IsEnable);
                }
            }
        }


        private bool _isEnableUsed;
        /// <summary>
        /// 是否归属于其他方案中，是能使用，否不能使用
        /// </summary>
        public bool IsEnableUsed
        {
            get { return _isEnableUsed; }
            set
            {
                if (value != _isEnableUsed)
                {
                    _isEnableUsed = value;
                    this.RaisePropertyChanged(() => this.IsEnableUsed);
                }
            }
        }


        private int _index;
        /// <summary>
        /// 序号
        /// </summary>
        public int Index
        {
            get { return _index; }
            set
            {
                if (_index != value)
                {
                    _index = value;
                    this.RaisePropertyChanged(() => this.Index);
                }
            }
        }

        private int _rtuid;
        /// <summary>
        /// 终端地址
        /// </summary>
        public int RtuId
        {
            get { return _rtuid; }
            set
            {
                if (_rtuid != value)
                {
                    _rtuid = value;
                    this.RaisePropertyChanged(() => this.RtuId);

                    if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(_rtuid) == false)
                        return;
                    var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[_rtuid];
                    if (info != null)
                    {
                        RtuName = info.RtuName;
                        PhyId = info.RtuPhyId.ToString("D4");
                    }

                    //加载 switch item 
                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(_rtuid);
                    if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) return;

                    var vol = para as Wj3005Rtu;

                    //if (vol != null && vol.WjVoltage.RtuUsedType == 0)
                    if (vol != null)
                    {
                        var wjswout = (from t in vol.WjSwitchOuts orderby t.Key select t).ToList();
                        foreach (var t in wjswout)
                        {
                            Items.Add(new TimeInfoName
                                               {
                                                   TimeTableName = t.Value.SwitchName,
                                                   IsCheckSwitch = false,
                                                   IsEnabledOn = true
                                               });
                        }
                    }
                }
            }
        }


        private string _phyid;
        /// <summary>
        /// 物理地址
        /// </summary>
        public string PhyId
        {
            get { return _phyid; }
            set
            {
                if (_phyid != value)
                {
                    _phyid = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }



        private string _ownerScheme;
        /// <summary>
        /// 归属方案
        /// </summary>
        public string OwnerScheme
        {
            get { return _ownerScheme; }
            set
            {
                if (_ownerScheme != value)
                {
                    _ownerScheme = value;
                    this.RaisePropertyChanged(() => this.OwnerScheme);
                }
            }
        }


        private string _rtuname;
        /// <summary>
        /// 终端名称
        /// </summary>
        public string RtuName
        {
            get { return _rtuname; }
            set
            {
                if (_rtuname != value)
                {
                    _rtuname = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }



        private ObservableCollection<TimeInfoName> _items = null;
        /// <summary>
        /// 终端回路信息
        /// </summary>
        public ObservableCollection<TimeInfoName> Items
        {
            get
            {
                if (_items == null) _items = new ObservableCollection<TimeInfoName>();
                return _items;
            }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    RaisePropertyChanged(() => Items);
                }
            }
        }

    }

    public class TimeInfoName : Wlst.Cr.Core.CoreServices.ObservableObject
    {


        /// <summary>
        /// k 时间表地址序号
        /// </summary>
        public int TimeTable;

        private string _k1TimeTableName;

        /// <summary>
        /// k 开关名称
        /// </summary>
        public string TimeTableName
        {
            get { return _k1TimeTableName; }
            set
            {
                if (_k1TimeTableName != value)
                {
                    _k1TimeTableName = value;
                    this.RaisePropertyChanged(() => this.TimeTableName);

                }
            }
        }

        private bool _isCheckSwitch;
        /// <summary>
        /// 是否选中开关
        /// </summary>
        public bool IsCheckSwitch
        {
            get { return _isCheckSwitch; }
            set
            {
                if (_isCheckSwitch != value)
                {
                    _isCheckSwitch = value;
                    RaisePropertyChanged(() => IsCheckSwitch);

                }
            }
        }

        private bool _isEnabledOn;
        /// <summary>
        /// 开关是否能启用
        /// </summary>
        public bool IsEnabledOn
        {
            get { return _isEnabledOn; }
            set
            {
                if (_isEnabledOn != value)
                {
                    _isEnabledOn = value;
                    RaisePropertyChanged(() => IsEnabledOn);

                }
            }
        }
    }
}
