using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Wlst.Cr.CoreOne.TreeNodeBase;

namespace Wlst.Ux.WJ3005Module.LnEmergencyOperationCenter.Services
{
    public partial class TreeNodeBase : TreeNodeBaseViewModel
    {
        //public bool IsRtuUsed;
        private static readonly List<int> DeleteNodeIds=new List<int>();
        public TreeNodeBase()
        {
            ;
        }
        
        public List<int> GetNeedToBeDeletedNodeId()
        {
            return DeleteNodeIds;
        }

        public virtual void AddChild()
        {
            ;
        }
        protected TreeNodeBase _father;

        public TreeNodeBase Father
        {
            get { return _father; }
        }

        private ObservableCollection<TreeNodeBase> _childTreeItemsInfo;

        public ObservableCollection<TreeNodeBase> ChildTreeItems
        {
            get { return _childTreeItemsInfo ?? (_childTreeItemsInfo = new ObservableCollection<TreeNodeBase>()); }
        }

        #region IsChecked

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if(IsChecked==value) return;
                _isChecked = value;
                if(IsGroup)
                {
                    foreach (var item in ChildTreeItems)
                    {
                        item.IsChecked = IsChecked;
                        if (IsSwitch1Checked) item.IsSwitch1Checked = IsChecked;
                        if (IsSwitch2Checked) item.IsSwitch2Checked = IsChecked;
                        if (IsSwitch3Checked) item.IsSwitch3Checked = IsChecked;
                        if (IsSwitch4Checked) item.IsSwitch4Checked = IsChecked;
                        if (IsSwitch5Checked) item.IsSwitch5Checked = IsChecked;
                        if (IsSwitch6Checked) item.IsSwitch6Checked = IsChecked;
                        if (IsSwitch7Checked) item.IsSwitch7Checked = IsChecked;
                        if (IsSwitch8Checked) item.IsSwitch8Checked = IsChecked;
                    }
                }
                IsShowSelectedCheckBox = IsChecked;
                ShowK7K8 = IsChecked;
                RaisePropertyChanged(() => IsChecked);

            }
        }
        #endregion

        #region IsGroup
        private bool _isGroup;

        public bool IsGroup
        {
            get { return _isGroup; }
            set
            {
                _isGroup = value;
                this.RaisePropertyChanged(() => this.IsGroup);
            }
        }
        #endregion

        #region Is3006
        private bool _is3006;

        public bool Is3006
        {
            get { return _is3006; }
            set
            {
                _is3006 = value;
                this.RaisePropertyChanged(() => this.Is3006);
            }
        }
        #endregion

        #region ShowK7K8
        private bool _showK7K8;

        public bool ShowK7K8
        {
            get { return _showK7K8; }
            set
            {
                _showK7K8 = value;
                //if (Is3006)
                //{
                //    ShowK7K8 = true ;
                //}
                //else
                //{
                //    ShowK7K8 = false ;// false;
                //}
                this.RaisePropertyChanged(() => this.ShowK7K8);
            }
        }
        #endregion

        #region AreaName
        private string _areaName;

        /// <summary>
        /// 节点名称  终端名称或是分组名称
        /// </summary>
        public string AreaName
        {
            get { return _areaName; }
            set
            {
                if (_areaName != value)
                {
                    _areaName = value;
                    this.RaisePropertyChanged(() => this.AreaName);
                }
            }
        }
        #endregion

        #region PhysicalId

        private int _physicalId;
        public int PhysicalId
        {
            get { return _physicalId; }
            set
            {
                if(value==_physicalId) return;
                _physicalId = value;
                RaisePropertyChanged(()=>PhysicalId);
            }
        }
        #endregion

        #region IsShowSelectedCheckBox

        private bool _isShowSelectedCheckBox;
        public bool IsShowSelectedCheckBox
        {
            get { return _isShowSelectedCheckBox; }
            set
            {
                _isShowSelectedCheckBox = IsGroup || value;
                RaisePropertyChanged(()=>IsShowSelectedCheckBox);
            }
        }

        #endregion

        #region Remarks
        private string _remarks;

        /// <summary>
        /// 节点备注，是满足什么条件 所需要应急关灯
        /// </summary>
        public string Remarks
        {
            get { return _remarks; }
            set
            {
                if (_remarks != value)
                {
                    _remarks = value;
                    this.RaisePropertyChanged(() => this.Remarks);
                }
            }
        }
        #endregion

        #region Index
        private int _index;

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
        #endregion


        #region OpTime

        private string _opTime;

        /// <summary>
        /// 周设置应答
        /// </summary>
        public string OpTime
        {
            get { return _opTime; }
            set
            {
                if (_opTime != value)
                {
                    _opTime = value;
                    this.RaisePropertyChanged(() => this.OpTime);
                }
            }
        }
        #endregion
    }
    public partial class TreeNodeBase
    {
        #region 当前状态
        #region State

        private EnumTmlState _state;
        public EnumTmlState State
        {
            get { return _state; }
            set
            {
                if(value==_state) return;
                _state = value;
                RaisePropertyChanged(()=>State);
            }
        }
        #endregion

        #endregion

        #region IsSwitch0

        private bool _isSwitch0;
        public bool IsSwitch0
        {
            get { return _isSwitch0; }
            set
            {
                if (_isSwitch0 == value) return;
                _isSwitch0 = value;
                IsSwitch1Checked = _isSwitch0;
                IsSwitch2Checked = _isSwitch0;
                IsSwitch3Checked = _isSwitch0;
                IsSwitch4Checked = _isSwitch0;
                IsSwitch5Checked = _isSwitch0;
                IsSwitch6Checked = _isSwitch0;
                IsSwitch7Checked = _isSwitch0;
                IsSwitch8Checked = _isSwitch0;
                if(IsGroup)
                {
                    foreach (var item in ChildTreeItems.Where(t => t.IsChecked))
                    {
                        item.IsSwitch0 = _isSwitch0;
                    }
                }
                RaisePropertyChanged(() => IsSwitch0);
            }
        }


        #region K0选测数据反馈
        private EnumSelectionTestAns _k0SelectionTestAns;
        public EnumSelectionTestAns K0SelectionTestAns
        {
            get { return _k0SelectionTestAns; }
            set
            {
                if (_k0SelectionTestAns == value) return;
                _k0SelectionTestAns = value;
                RaisePropertyChanged(() => K0SelectionTestAns);
            }
        }
        #endregion

        #endregion

        #region Switch1

        #region IsSwitch1Checked

        private bool _isSwitch1Checked;
        public bool IsSwitch1Checked
        {
            get { return _isSwitch1Checked; }
            set
            {
                if(_isSwitch1Checked==value) return;
                _isSwitch1Checked = value;
                IsK1ShowOpenOrColseAns = _isSwitch1Checked;
                if(IsGroup)
                {
                    if (_isSwitch1Checked && IsChecked == false) IsChecked = true;
                    foreach (var item in ChildTreeItems.Where(t=>t.IsChecked))
                    {
                        item.IsSwitch1Checked = _isSwitch1Checked;
                    }
                }
                RaisePropertyChanged(()=>IsSwitch1Checked);
            }
        }
        #endregion

        #region K1开关灯应答

        private EnumOpenOrCloseAns _k1OpenOrCloseAns;
        public EnumOpenOrCloseAns K1OpenOrCloseAns
        {
            get { return _k1OpenOrCloseAns; }
            set
            {
                if(_k1OpenOrCloseAns==value) return;
                _k1OpenOrCloseAns = value;
                RaisePropertyChanged(()=>K1OpenOrCloseAns);
            }
        }
        #endregion

        #region K1开关应答显示

        private bool _isK1ShowOpenOrCloseAns;
        public bool IsK1ShowOpenOrColseAns
        {
            get { return _isK1ShowOpenOrCloseAns; }
            set
            {
                if(value==_isK1ShowOpenOrCloseAns) return;
                _isK1ShowOpenOrCloseAns = !IsGroup && value;
                RaisePropertyChanged(()=>IsK1ShowOpenOrColseAns);
            }
        }
        #endregion

        #region K1选测数据反馈
        private EnumSelectionTestAns _k1SelectionTestAns;
        public EnumSelectionTestAns K1SelectionTestAns
        {
            get { return _k1SelectionTestAns; }
            set
            {
                if (_k1SelectionTestAns == value) return;
                _k1SelectionTestAns = value;
                RaisePropertyChanged(() => K1SelectionTestAns);
            }
        }
        #endregion

        #endregion

        #region Switch2

        #region IsSwitch2Checked

        private bool _isSwitch2Checked;
        public bool IsSwitch2Checked
        {
            get { return _isSwitch2Checked; }
            set
            {
                if (_isSwitch2Checked == value) return;
                _isSwitch2Checked = value;
                IsK2ShowOpenOrColseAns = _isSwitch2Checked;
                if (IsGroup)
                {
                    if (_isSwitch2Checked && IsChecked == false) IsChecked = true;
                    foreach (var item in ChildTreeItems.Where(t => t.IsChecked))
                    {
                        item.IsSwitch2Checked = _isSwitch2Checked;
                    }
                }
                RaisePropertyChanged(() => IsSwitch2Checked);
            }
        }
        #endregion

        #region K2开关灯应答

        private EnumOpenOrCloseAns _k2OpenOrCloseAns;
        public EnumOpenOrCloseAns K2OpenOrCloseAns
        {
            get { return _k2OpenOrCloseAns; }
            set
            {
                if (_k2OpenOrCloseAns == value) return;
                _k2OpenOrCloseAns = value;
                RaisePropertyChanged(() => K2OpenOrCloseAns);
            }
        }
        #endregion

        #region K2开关应答显示

        private bool _isK2ShowOpenOrCloseAns;
        public bool IsK2ShowOpenOrColseAns
        {
            get { return _isK2ShowOpenOrCloseAns; }
            set
            {
                if (value == _isK2ShowOpenOrCloseAns) return;
                _isK2ShowOpenOrCloseAns = !IsGroup && value;
                RaisePropertyChanged(() => IsK2ShowOpenOrColseAns);
            }
        }
        #endregion

        #region K2选测数据反馈
        private EnumSelectionTestAns _k2SelectionTestAns;
        public EnumSelectionTestAns K2SelectionTestAns
        {
            get { return _k2SelectionTestAns; }
            set
            {
                if (_k2SelectionTestAns == value) return;
                _k2SelectionTestAns = value;
                RaisePropertyChanged(() => K2SelectionTestAns);
            }
        }
        #endregion

        #endregion

        #region Switch3

        #region IsSwitch3Checked

        private bool _isSwitch3Checked;
        public bool IsSwitch3Checked
        {
            get { return _isSwitch3Checked; }
            set
            {
                if (_isSwitch3Checked == value) return;
                _isSwitch3Checked = value;
                IsK3ShowOpenOrColseAns = _isSwitch3Checked;
                if (IsGroup)
                {
                    if (_isSwitch3Checked && IsChecked == false) IsChecked = true;
                    foreach (var item in ChildTreeItems.Where(t => t.IsChecked))
                    {
                        item.IsSwitch3Checked = _isSwitch3Checked;
                    }
                }
                RaisePropertyChanged(() => IsSwitch3Checked);
            }
        }
        #endregion

        #region K3开关灯应答

        private EnumOpenOrCloseAns _k3OpenOrCloseAns;
        public EnumOpenOrCloseAns K3OpenOrCloseAns
        {
            get { return _k3OpenOrCloseAns; }
            set
            {
                if (_k3OpenOrCloseAns == value) return;
                _k3OpenOrCloseAns = value;
                RaisePropertyChanged(() => K3OpenOrCloseAns);
            }
        }
        #endregion

        #region K3开关应答显示

        private bool _isK3ShowOpenOrCloseAns;
        public bool IsK3ShowOpenOrColseAns
        {
            get { return _isK3ShowOpenOrCloseAns; }
            set
            {
                if (value == _isK3ShowOpenOrCloseAns) return;
                _isK3ShowOpenOrCloseAns = !IsGroup && value;
                RaisePropertyChanged(() => IsK3ShowOpenOrColseAns);
            }
        }
        #endregion

        #region K3选测数据反馈
        private EnumSelectionTestAns _k3SelectionTestAns;
        public EnumSelectionTestAns K3SelectionTestAns
        {
            get { return _k3SelectionTestAns; }
            set
            {
                if (_k3SelectionTestAns == value) return;
                _k3SelectionTestAns = value;
                RaisePropertyChanged(() => K3SelectionTestAns);
            }
        }
        #endregion

        #endregion

        #region Switch4

        #region IsSwitch4Checked

        private bool _isSwitch4Checked;
        public bool IsSwitch4Checked
        {
            get { return _isSwitch4Checked; }
            set
            {
                if (_isSwitch4Checked == value) return;
                _isSwitch4Checked = value;
                IsK4ShowOpenOrColseAns = _isSwitch4Checked;
                if (IsGroup)
                {
                    if (_isSwitch4Checked && IsChecked == false) IsChecked = true;
                    foreach (var item in ChildTreeItems.Where(t => t.IsChecked))
                    {
                        item.IsSwitch4Checked = _isSwitch4Checked;
                    }
                }
                RaisePropertyChanged(() => IsSwitch4Checked);
            }
        }
        #endregion

        #region K4开关灯应答

        private EnumOpenOrCloseAns _k4OpenOrCloseAns;
        public EnumOpenOrCloseAns K4OpenOrCloseAns
        {
            get { return _k4OpenOrCloseAns; }
            set
            {
                if (_k4OpenOrCloseAns == value) return;
                _k4OpenOrCloseAns = value;
                RaisePropertyChanged(() => K4OpenOrCloseAns);
            }
        }
        #endregion

        #region K4开关应答显示

        private bool _isK4ShowOpenOrCloseAns;
        public bool IsK4ShowOpenOrColseAns
        {
            get { return _isK4ShowOpenOrCloseAns; }
            set
            {
                if (value == _isK4ShowOpenOrCloseAns) return;
                _isK4ShowOpenOrCloseAns = !IsGroup && value;
                RaisePropertyChanged(() => IsK4ShowOpenOrColseAns);
            }
        }
        #endregion

        #region K4选测数据反馈
        private EnumSelectionTestAns _k4SelectionTestAns;
        public EnumSelectionTestAns K4SelectionTestAns
        {
            get { return _k4SelectionTestAns; }
            set
            {
                if (_k4SelectionTestAns == value) return;
                _k4SelectionTestAns = value;
                RaisePropertyChanged(() => K4SelectionTestAns);
            }
        }
        #endregion

        #endregion

        #region Switch5

        #region IsSwitch5Checked

        private bool _isSwitch5Checked;
        public bool IsSwitch5Checked
        {
            get { return _isSwitch5Checked; }
            set
            {
                if (_isSwitch5Checked == value) return;
                _isSwitch5Checked = value;
                IsK5ShowOpenOrColseAns = _isSwitch5Checked;
                if (IsGroup)
                {
                    if (_isSwitch5Checked && IsChecked == false) IsChecked = true;
                    foreach (var item in ChildTreeItems.Where(t => t.IsChecked))
                    {
                        item.IsSwitch5Checked = _isSwitch5Checked;
                    }
                }
                RaisePropertyChanged(() => IsSwitch5Checked);
            }
        }
        #endregion

        #region K5开关灯应答

        private EnumOpenOrCloseAns _k5OpenOrCloseAns;
        public EnumOpenOrCloseAns K5OpenOrCloseAns
        {
            get { return _k5OpenOrCloseAns; }
            set
            {
                if (_k5OpenOrCloseAns == value) return;
                _k5OpenOrCloseAns = value;
                RaisePropertyChanged(() => K5OpenOrCloseAns);
            }
        }
        #endregion

        #region K5开关应答显示

        private bool _isK5ShowOpenOrCloseAns;
        public bool IsK5ShowOpenOrColseAns
        {
            get { return _isK5ShowOpenOrCloseAns; }
            set
            {
                if (value == _isK5ShowOpenOrCloseAns) return;
                _isK5ShowOpenOrCloseAns = !IsGroup && value;
                RaisePropertyChanged(() => IsK5ShowOpenOrColseAns);
            }
        }
        #endregion

        #region K5选测数据反馈
        private EnumSelectionTestAns _k5SelectionTestAns;
        public EnumSelectionTestAns K5SelectionTestAns
        {
            get { return _k5SelectionTestAns; }
            set
            {
                if (_k5SelectionTestAns == value) return;
                _k5SelectionTestAns = value;
                RaisePropertyChanged(() => K5SelectionTestAns);
            }
        }
        #endregion

        #endregion

        #region Switch6

        #region IsSwitch6Checked

        private bool _isSwitch6Checked;
        public bool IsSwitch6Checked
        {
            get { return _isSwitch6Checked; }
            set
            {
                if (_isSwitch6Checked == value) return;
                _isSwitch6Checked = value;
                IsK6ShowOpenOrColseAns = _isSwitch6Checked;
                if (IsGroup)
                {
                    if (_isSwitch6Checked && IsChecked == false) IsChecked = true;
                    foreach (var item in ChildTreeItems.Where(t => t.IsChecked))
                    {
                        item.IsSwitch6Checked = _isSwitch6Checked;
                    }
                }
                RaisePropertyChanged(() => IsSwitch6Checked);
            }
        }
        #endregion

        #region K6开关灯应答

        private EnumOpenOrCloseAns _k6OpenOrCloseAns;
        public EnumOpenOrCloseAns K6OpenOrCloseAns
        {
            get { return _k6OpenOrCloseAns; }
            set
            {
                if (_k6OpenOrCloseAns == value) return;
                _k6OpenOrCloseAns = value;
                RaisePropertyChanged(() => K6OpenOrCloseAns);
            }
        }
        #endregion

        #region K6开关应答显示

        private bool _isK6ShowOpenOrCloseAns;
        public bool IsK6ShowOpenOrColseAns
        {
            get { return _isK6ShowOpenOrCloseAns; }
            set
            {
                if (value == _isK6ShowOpenOrCloseAns) return;
                _isK6ShowOpenOrCloseAns = !IsGroup && value;
                RaisePropertyChanged(() => IsK6ShowOpenOrColseAns);
            }
        }
        #endregion

        #region K6选测数据反馈
        private EnumSelectionTestAns _k6SelectionTestAns;
        public EnumSelectionTestAns K6SelectionTestAns
        {
            get { return _k6SelectionTestAns; }
            set
            {
                if (_k6SelectionTestAns == value) return;
                _k6SelectionTestAns = value;
                RaisePropertyChanged(() => K6SelectionTestAns);
            }
        }
        #endregion

        #endregion

        #region Switch7

        #region IsSwitch7Checked

        private bool _isSwitch7Checked;
        public bool IsSwitch7Checked
        {
            get { return _isSwitch7Checked; }
            set
            {
                if (_isSwitch7Checked == value) return;          
                if (IsGroup)
                {
                    _isSwitch7Checked = value;
                    if (_isSwitch7Checked && IsChecked == false) IsChecked = true;
                    foreach (var item in ChildTreeItems.Where(t => t.IsChecked))
                    {
                        if(item.Is3006)
                        {
                            item.IsSwitch7Checked = _isSwitch7Checked;
                        }
                        else
                        {
                            item.IsSwitch7Checked = false;
                            
                        }
                        
                    }
                }
                else if (Is3006)
                {
                    _isSwitch7Checked = value;
                }

                IsK7ShowOpenOrColseAns = _isSwitch7Checked;
                RaisePropertyChanged(() => IsSwitch7Checked);
            }
        }
        #endregion

        #region K7开关灯应答

        private EnumOpenOrCloseAns _k7OpenOrCloseAns;
        public EnumOpenOrCloseAns K7OpenOrCloseAns
        {
            get { return _k7OpenOrCloseAns; }
            set
            {
                if (_k7OpenOrCloseAns == value) return;
                _k7OpenOrCloseAns = value;
                RaisePropertyChanged(() => K7OpenOrCloseAns);
            }
        }
        #endregion

        #region K7开关应答显示

        private bool _isK7ShowOpenOrCloseAns;
        public bool IsK7ShowOpenOrColseAns
        {
            get { return _isK7ShowOpenOrCloseAns; }
            set
            {
                if (value == _isK7ShowOpenOrCloseAns) return;
                _isK7ShowOpenOrCloseAns = !IsGroup && value;
                RaisePropertyChanged(() => IsK7ShowOpenOrColseAns);
            }
        }
        #endregion

        #region K7选测数据反馈
        private EnumSelectionTestAns _k7SelectionTestAns;
        public EnumSelectionTestAns K7SelectionTestAns
        {
            get { return _k7SelectionTestAns; }
            set
            {
                if (_k7SelectionTestAns == value) return;
                _k7SelectionTestAns = value;
                RaisePropertyChanged(() => K7SelectionTestAns);
            }
        }
        #endregion

        #endregion

        #region Switch8

        #region IsSwitch8Checked

        private bool _isSwitch8Checked;
        public bool IsSwitch8Checked
        {
            get { return _isSwitch8Checked; }
            set
            {
                if (_isSwitch8Checked == value) return;
                if (IsGroup)
                {
                    _isSwitch8Checked = value;
                    if (_isSwitch8Checked && IsChecked == false) IsChecked = true;
                    foreach (var item in ChildTreeItems.Where(t => t.IsChecked))
                    {
                        if (item.Is3006)
                        {
                            item.IsSwitch8Checked = _isSwitch8Checked;
                        }
                        else
                        {
                            item.IsSwitch8Checked = false;
                        }
                        
                    }
                }
                else if (Is3006)
                {
                    _isSwitch8Checked = value;
                }
                IsK8ShowOpenOrColseAns = _isSwitch8Checked;
                RaisePropertyChanged(() => IsSwitch8Checked);
            }
        }
        #endregion

        #region K8开关灯应答

        private EnumOpenOrCloseAns _k8OpenOrCloseAns;
        public EnumOpenOrCloseAns K8OpenOrCloseAns
        {
            get { return _k8OpenOrCloseAns; }
            set
            {
                if (_k8OpenOrCloseAns == value) return;
                _k8OpenOrCloseAns = value;
                RaisePropertyChanged(() => K8OpenOrCloseAns);
            }
        }
        #endregion

        #region K8开关应答显示

        private bool _isK8ShowOpenOrCloseAns;
        public bool IsK8ShowOpenOrColseAns
        {
            get { return _isK8ShowOpenOrCloseAns; }
            set
            {
                if (value == _isK8ShowOpenOrCloseAns) return;
                _isK8ShowOpenOrCloseAns = !IsGroup && value;
                RaisePropertyChanged(() => IsK8ShowOpenOrColseAns);
            }
        }
        #endregion

        #region K8选测数据反馈
        private EnumSelectionTestAns _k8SelectionTestAns;
        public EnumSelectionTestAns K8SelectionTestAns
        {
            get { return _k8SelectionTestAns; }
            set
            {
                if (_k8SelectionTestAns == value) return;
                _k8SelectionTestAns = value;
                RaisePropertyChanged(() => K8SelectionTestAns);
            }
        }
        #endregion

        #endregion

        #region 对时应答

        private bool _syncTimeAns;
        public bool SyncTimeAns
        {
            get { return _syncTimeAns; }
            set
            {
                if (_syncTimeAns == value) return;
                _syncTimeAns = value;
                RaisePropertyChanged(() => SyncTimeAns);
            }
        }

        #endregion


        #region 召测对时

        private string _zcTimeAns;
        public string ZcTimeAns
        {
            get { return _zcTimeAns; }
            set
            {
                if (_zcTimeAns == value) return;
                _zcTimeAns = value;
                RaisePropertyChanged(() => ZcTimeAns);
            }
        }

        #endregion


        #region 召测版本

        private string _zcVerAns;
        public string ZcVerAns
        {
            get { return _zcVerAns; }
            set
            {
                if (_zcVerAns == value) return;
                _zcVerAns = value;
                RaisePropertyChanged(() => ZcVerAns);
            }
        }

        #endregion


  

        #region 发送周设置应答

        private string  _weekSndAns;
        public string  WeekSndAns
        {
            get { return _weekSndAns; }
            set
            {
                if (_weekSndAns == value) return;
                _weekSndAns = value;
                RaisePropertyChanged(() => WeekSndAns);
            }
        }

        #endregion

 

        #region 勾选停运终端

        private bool _checkStopTml;
        public bool CheckStopTml
        {
            get { return _checkStopTml; }
            set
            {
                if(_checkStopTml==value) return;
                _checkStopTml = value;
                RaisePropertyChanged(()=>CheckStopTml);
            }
        }

        #endregion

        #region 勾选启运终端

        private bool _checkStartTml;
        public bool CheckStartTml
        {
            get { return _checkStartTml; }
            set
            {
                if(_checkStartTml==value) return;
                _checkStartTml = value;
                RaisePropertyChanged(()=>CheckStartTml);
            }
        }

        #endregion

        #region 选测数据显示

        private bool _selectVisi;
        public bool SelectVisi
        {
            get { return _selectVisi; }
            set
            {
                if(_selectVisi==value) return;
                _selectVisi = !IsGroup && value;
                
                RaisePropertyChanged(()=>SelectVisi);
            }
        }

        #endregion

    }
}
