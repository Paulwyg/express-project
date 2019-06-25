using System.Collections.ObjectModel;
using System.Windows;
using Wlst.Cr.CoreOne.TreeNodeBase;


namespace Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.Services
{

    public class ListTreeNodeBase : TreeNodeBaseViewModel
    {
        private bool _isListTreeNodeGroup;

        public bool IsListTreeNodeGroup
        {
            get { return _isListTreeNodeGroup; }
            set
            {
                _isListTreeNodeGroup = value;
                CalShowContent();
            }
        }

        /// <summary>
        /// 父节点
        /// </summary>
        protected ListTreeNodeBase _father;

        public ListTreeNodeBase Father
        {
            get { return _father; }
        }

        #region time table id & name & description; show name & tooptios

        #region time table id

        int _k1TimeTalbe;
        int _k2TimeTalbe;
        int _k3TimeTalbe;
        int _k4TimeTalbe;
        int _k5TimeTalbe;
        int _k6TimeTalbe;

        /// <summary>
        /// k1 时间表地址序号
        /// </summary>
        public int K1TimeTalbe
        {
            get { return _k1TimeTalbe; }
            set
            {
                if (value != _k1TimeTalbe)
                {
                    _k1TimeTalbe = value;
                    var t = Sr .TimeTableSystem .Services.WeekTimeTableInfoService .GeteekTimeTableInfo(this._k1TimeTalbe);
                    this.K1TimeTableName = t == null ? "-" : t.time_name;
                    this.K1TimeTableNameDescriotion = t == null ? "-" : t.time_desc; CalShowContent();
                }
            }
        }

        /// <summary>
        /// k2 时间表地址序号
        /// </summary>
        public int K2TimeTalbe
        {
            get { return _k2TimeTalbe; }
            set
            {
                if (value != _k2TimeTalbe)
                {
                    _k2TimeTalbe = value;
                    var t = Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GeteekTimeTableInfo(this._k2TimeTalbe);
                    this.K2TimeTableName = t == null ? "-" : t.time_name;
                    this.K2TimeTableNameDescriotion = t == null ? "-" : t.time_desc; CalShowContent();
                }
            }
        }

        /// <summary>
        /// k3 时间表地址序号
        /// </summary>
        public int K3TimeTalbe
        {
            get { return _k3TimeTalbe; }
            set
            {
                if (value != _k3TimeTalbe)
                {
                    _k3TimeTalbe = value;
                    var t = Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GeteekTimeTableInfo(this._k3TimeTalbe);
                    this.K3TimeTableName = t == null ? "-" : t.time_name;
                    this.K3TimeTableNameDescriotion = t == null ? "-" : t.time_desc; CalShowContent();
                }
            }
        }

        /// <summary>
        /// k4 时间表地址序号
        /// </summary>
        public int K4TimeTalbe
        {
            get { return _k4TimeTalbe; }
            set
            {
                if (value != _k4TimeTalbe)
                {
                    _k4TimeTalbe = value;
                    var t = Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GeteekTimeTableInfo(this._k4TimeTalbe);
                    this.K4TimeTableName = t == null ? "-" : t.time_name;
                    this.K4TimeTableNameDescriotion = t == null ? "-" : t.time_desc; CalShowContent();
                }
            }
        }

        /// <summary>
        /// k5 时间表地址序号
        /// </summary>
        public int K5TimeTalbe
        {
            get { return _k5TimeTalbe; }
            set
            {
                if (value != _k5TimeTalbe)
                {
                    _k5TimeTalbe = value;
                    var t = Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GeteekTimeTableInfo(this._k5TimeTalbe);
                    this.K5TimeTableName = t == null ? "-" : t.time_name;
                    this.K5TimeTableNameDescriotion = t == null ? "-" : t.time_desc; CalShowContent();
                }
            }
        }

        /// <summary>
        /// k6 时间表地址序号
        /// </summary>
        public int K6TimeTalbe
        {
            get { return _k6TimeTalbe; }
            set
            {
                if (value != _k6TimeTalbe)
                {
                    _k6TimeTalbe = value;
                    var t = Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GeteekTimeTableInfo(this._k6TimeTalbe);
                    this.K6TimeTableName = t == null ? "-" : t.time_name;
                    this.K6TimeTableNameDescriotion = t == null ? "-" : t.time_desc; CalShowContent();
                }
            }
        }
        #endregion

        #region time table name
        private string _k1TimeTableName;

        /// <summary>
        /// k1 时间表名称
        /// </summary>
        public string K1TimeTableName
        {
            get { return _k1TimeTableName; }
            set
            {
                if (_k1TimeTableName != value)
                {
                    _k1TimeTableName = value;
                    this.RaisePropertyChanged(() => this.K1TimeTableName);
                    
                }
            }
        }

        private string _k2TimeTableName;

        /// <summary>
        /// k2时间表名称
        /// </summary>
        public string K2TimeTableName
        {
            get { return _k2TimeTableName; }
            set
            {
                if (_k2TimeTableName != value)
                {
                    _k2TimeTableName = value;
                    this.RaisePropertyChanged(() => this.K2TimeTableName);
                    
                }
            }
        }

        private string _k3TimeTableName;

        /// <summary>
        /// k3 时间表名称
        /// </summary>
        public string K3TimeTableName
        {
            get { return _k3TimeTableName; }
            set
            {
                if (_k3TimeTableName != value)
                {
                    _k3TimeTableName = value;
                    this.RaisePropertyChanged(() => this.K3TimeTableName);
                    
                }
            }
        }

        private string _k4TimeTableName;

        /// <summary>
        /// k4 时间表名称
        /// </summary>
        public string K4TimeTableName
        {
            get { return _k4TimeTableName; }
            set
            {
                if (_k4TimeTableName != value)
                {
                    _k4TimeTableName = value;
                    this.RaisePropertyChanged(() => this.K4TimeTableName);
                    
                }
            }
        }

        private string _k5TimeTableName;

        /// <summary>
        /// k5 时间表名称
        /// </summary>
        public string K5TimeTableName
        {
            get { return _k5TimeTableName; }
            set
            {
                if (_k5TimeTableName != value)
                {
                    _k5TimeTableName = value;
                    this.RaisePropertyChanged(() => this.K5TimeTableName);
                    
                }
            }
        }

        private string _k6TimeTableName;

        /// <summary>
        /// k6 时间表名称
        /// </summary>
        public string K6TimeTableName
        {
            get { return _k6TimeTableName; }
            set
            {
                if (_k6TimeTableName != value)
                {
                    _k6TimeTableName = value;
                    this.RaisePropertyChanged(() => this.K6TimeTableName);
                    
                }
            }
        }
        #endregion

        #region time table name descioption
        private string _k1TimeTableNameDescriotion;

        /// <summary>
        /// k1 时间表名称
        /// </summary>
        public string K1TimeTableNameDescriotion
        {
            get { return _k1TimeTableNameDescriotion; }
            set
            {
                if (_k1TimeTableNameDescriotion != value)
                {
                    _k1TimeTableNameDescriotion = value;
                    this.RaisePropertyChanged(() => this.K1TimeTableNameDescriotion);
                    
                }
            }
        }

        private string _k2TimeTableNameDescriotion;

        /// <summary>
        /// k2时间表名称
        /// </summary>
        public string K2TimeTableNameDescriotion
        {
            get { return _k2TimeTableNameDescriotion; }
            set
            {
                if (_k2TimeTableNameDescriotion != value)
                {
                    _k2TimeTableNameDescriotion = value;
                    this.RaisePropertyChanged(() => this.K2TimeTableNameDescriotion);
                    
                }
            }
        }

        private string _k3TimeTableNameDescriotion;

        /// <summary>
        /// k3 时间表名称
        /// </summary>
        public string K3TimeTableNameDescriotion
        {
            get { return _k3TimeTableNameDescriotion; }
            set
            {
                if (_k3TimeTableNameDescriotion != value)
                {
                    _k3TimeTableNameDescriotion = value;
                    this.RaisePropertyChanged(() => this.K3TimeTableNameDescriotion);
                    
                }
            }
        }

        private string _k4TimeTableNameDescriotion;

        /// <summary>
        /// k4 时间表名称
        /// </summary>
        public string K4TimeTableNameDescriotion
        {
            get { return _k4TimeTableNameDescriotion; }
            set
            {
                if (_k4TimeTableNameDescriotion != value)
                {
                    _k4TimeTableNameDescriotion = value;
                    this.RaisePropertyChanged(() => this.K4TimeTableNameDescriotion);
                    
                }
            }
        }

        private string _k5TimeTableNameDescriotion;

        /// <summary>
        /// k5 时间表名称
        /// </summary>
        public string K5TimeTableNameDescriotion
        {
            get { return _k5TimeTableNameDescriotion; }
            set
            {
                if (_k5TimeTableNameDescriotion != value)
                {
                    _k5TimeTableNameDescriotion = value;
                    this.RaisePropertyChanged(() => this.K5TimeTableNameDescriotion);
                    
                }
            }
        }

        private string _k6TimeTableNameDescriotion;

        /// <summary>
        /// k6 时间表名称
        /// </summary>
        public string K6TimeTableNameDescriotion
        {
            get { return _k6TimeTableNameDescriotion; }
            set
            {
                if (_k6TimeTableNameDescriotion != value)
                {
                    _k6TimeTableNameDescriotion = value;
                    this.RaisePropertyChanged(() => this.K6TimeTableNameDescriotion);
                    
                }
            }
        }
        #endregion

        #region time table show name
        private string _k1Show;

        /// <summary>
        /// k1 时间表名称
        /// </summary>
        public string K1Show
        {
            get { return _k1Show; }
            set
            {
                if (_k1Show != value)
                {
                    _k1Show = value;
                    this.RaisePropertyChanged(() => this.K1Show);
                }
            }
        }

        private string _k2Show;

        /// <summary>
        /// k2时间表名称
        /// </summary>
        public string K2Show
        {
            get { return _k2Show; }
            set
            {
                if (_k2Show != value)
                {
                    _k2Show = value;
                    this.RaisePropertyChanged(() => this.K2Show);
                }
            }
        }

        private string _k3Show;

        /// <summary>
        /// k3 时间表名称
        /// </summary>
        public string K3Show
        {
            get { return _k3Show; }
            set
            {
                if (_k3Show != value)
                {
                    _k3Show = value;
                    this.RaisePropertyChanged(() => this.K3Show);
                }
            }
        }

        private string _k4Show;

        /// <summary>
        /// k4 时间表名称
        /// </summary>
        public string K4Show
        {
            get { return _k4Show; }
            set
            {
                if (_k4Show != value)
                {
                    _k4Show = value;
                    this.RaisePropertyChanged(() => this.K4Show);
                }
            }
        }

        private string _k5Show;

        /// <summary>
        /// k5 时间表名称
        /// </summary>
        public string K5Show
        {
            get { return _k5Show; }
            set
            {
                if (_k5Show != value)
                {
                    _k5Show = value;
                    this.RaisePropertyChanged(() => this.K5Show);
                }
            }
        }

        private string _k6Show;

        /// <summary>
        /// k6 时间表名称
        /// </summary>
        public string K6Show
        {
            get { return _k6Show; }
            set
            {
                if (_k6Show != value)
                {
                    _k6Show = value;
                    this.RaisePropertyChanged(() => this.K6Show);
                }
            }
        }
        #endregion

        #region time table show tooltips
        private string _k1ShowTooltips;

        /// <summary>
        /// k1 时间表名称
        /// </summary>
        public string K1ShowTooltips
        {
            get { return _k1ShowTooltips; }
            set
            {
                if (_k1ShowTooltips != value)
                {
                    _k1ShowTooltips = value;
                    this.RaisePropertyChanged(() => this.K1ShowTooltips);
                }
            }
        }

        private string _k2ShowTooltips;

        /// <summary>
        /// k2时间表名称
        /// </summary>
        public string K2ShowTooltips
        {
            get { return _k2ShowTooltips; }
            set
            {
                if (_k2ShowTooltips != value)
                {
                    _k2ShowTooltips = value;
                    this.RaisePropertyChanged(() => this.K2ShowTooltips);
                }
            }
        }

        private string _k3ShowTooltips;

        /// <summary>
        /// k3 时间表名称
        /// </summary>
        public string K3ShowTooltips
        {
            get { return _k3ShowTooltips; }
            set
            {
                if (_k3ShowTooltips != value)
                {
                    _k3ShowTooltips = value;
                    this.RaisePropertyChanged(() => this.K3ShowTooltips);
                }
            }
        }

        private string _k4ShowTooltips;

        /// <summary>
        /// k4 时间表名称
        /// </summary>
        public string K4ShowTooltips
        {
            get { return _k4ShowTooltips; }
            set
            {
                if (_k4ShowTooltips != value)
                {
                    _k4ShowTooltips = value;
                    this.RaisePropertyChanged(() => this.K4ShowTooltips);
                }
            }
        }

        private string _k5ShowTooltips;

        /// <summary>
        /// k5 时间表名称
        /// </summary>
        public string K5ShowTooltips
        {
            get { return _k5ShowTooltips; }
            set
            {
                if (_k5ShowTooltips != value)
                {
                    _k5ShowTooltips = value;
                    this.RaisePropertyChanged(() => this.K5ShowTooltips);
                }
            }
        }

        private string _k6ShowTooltips;

        /// <summary>
        /// k6 时间表名称
        /// </summary>
        public string K6ShowTooltips
        {
            get { return _k6ShowTooltips; }
            set
            {
                if (_k6ShowTooltips != value)
                {
                    _k6ShowTooltips = value;
                    this.RaisePropertyChanged(() => this.K6ShowTooltips);
                }
            }
        }
        #endregion
        #endregion

        #region  if group then


        private bool _isHasSpecialTermial;

        /// <summary>
        /// 是否具有特殊设置时间表终端
        /// </summary>
        public bool IsThisGroupHasSpecialTermial
        {
            get { return _isHasSpecialTermial; }
            set
            {
                if (_isHasSpecialTermial != value)
                {
                    _isHasSpecialTermial = value;
                    this.RaisePropertyChanged(() => this.IsThisGroupHasSpecialTermial);
                }
            }
        }


        private Visibility _isHasSpecialVisual;

        /// <summary>
        /// 是否具有特殊设置时间表终端 可见
        /// </summary>
        public Visibility IsThisGroupHasSpecialVisual
        {
            get { return _isHasSpecialVisual; }
            set
            {
                if (_isHasSpecialVisual != value)
                {
                    _isHasSpecialVisual = value;
                    this.RaisePropertyChanged(() => this.IsThisGroupHasSpecialVisual);
                }
            }
        }
        #endregion

        #region  if  terminal
        private bool _isSpecialTerminal;

        /// <summary>
        /// 本终端是否为特殊设置时间表终端
        /// </summary>
        public bool IsThisTmlSpecialTerminal
        {
            get { return _isSpecialTerminal; }
            set
            {
                if (_isSpecialTerminal != value)
                {
                    _isSpecialTerminal = value;
                    this.RaisePropertyChanged(() => this.IsThisTmlSpecialTerminal);
                    if (value==false  && this .IsListTreeNodeGroup ==false  && _father != null)
                    {
                        K1TimeTalbe = _father.K1TimeTalbe;
                        K2TimeTalbe = _father.K2TimeTalbe;
                        K3TimeTalbe = _father.K3TimeTalbe;
                        K4TimeTalbe = _father.K4TimeTalbe;
                        K5TimeTalbe = _father.K5TimeTalbe;
                        K6TimeTalbe = _father.K6TimeTalbe;
                    }
                    CalShowContent();
                }
            }
        }

        private bool _isSpecialTerminalEnable;

        /// <summary>
        /// 本终端是否为特殊设置时间表终端
        /// </summary>
        public bool IsThisTmlSpecialTerminalEnable
        {
            get { return _isSpecialTerminalEnable; }
            set
            {
                if (_isSpecialTerminalEnable != value)
                {
                    _isSpecialTerminalEnable = value;
                    this.RaisePropertyChanged(() => this.IsThisTmlSpecialTerminalEnable);
                    CalShowContent();
                }
            }
        }

        private  Visibility _isSpecialVisual;

        /// <summary>
        /// 本终端是否为特殊设置时间表终端可见
        /// </summary>
        public Visibility IsThisTmlSpecialVisual
        {
            get { return _isSpecialVisual; }
            set
            {
                if (_isSpecialVisual != value)
                {
                    _isSpecialVisual = value;
                    this.RaisePropertyChanged(() => this.IsThisTmlSpecialVisual);
                }
            }
        }
        #endregion


        private void CalShowContent()
        {
            if (IsListTreeNodeGroup)
            {
                IsThisGroupHasSpecialVisual = Visibility.Visible;
                IsThisTmlSpecialVisual = Visibility.Collapsed;
                K1Show = K1TimeTableName;
                K1ShowTooltips = K1TimeTableNameDescriotion;
                K2Show = K2TimeTableName;
                K2ShowTooltips = K2TimeTableNameDescriotion;
                K3Show = K3TimeTableName;
                K3ShowTooltips = K3TimeTableNameDescriotion;
                K4Show = K4TimeTableName;
                K4ShowTooltips = K4TimeTableNameDescriotion;
                K5Show = K5TimeTableName;
                K5ShowTooltips = K5TimeTableNameDescriotion;
                K6Show = K6TimeTableName;
                K6ShowTooltips = K6TimeTableNameDescriotion;
            }
            else
            {
                IsThisGroupHasSpecialVisual = Visibility.Collapsed;
                IsThisTmlSpecialVisual = Visibility.Visible;
                if (IsThisTmlSpecialTerminal)
                {
                    K1Show = K1TimeTableName;
                    K1ShowTooltips = K1TimeTableNameDescriotion;
                    K2Show = K2TimeTableName;
                    K2ShowTooltips = K2TimeTableNameDescriotion;
                    K3Show = K3TimeTableName;
                    K3ShowTooltips = K3TimeTableNameDescriotion;
                    K4Show = K4TimeTableName;
                    K4ShowTooltips = K4TimeTableNameDescriotion;
                    K5Show = K5TimeTableName;
                    K5ShowTooltips = K5TimeTableNameDescriotion;
                    K6Show = K6TimeTableName;
                    K6ShowTooltips = K6TimeTableNameDescriotion;
                }
                else
                {
                    K1Show = "✓";
                    K1ShowTooltips = K1TimeTableName;
                    K2Show = "✓";
                    K2ShowTooltips = K2TimeTableName;
                    K3Show = "✓";
                    K3ShowTooltips = K3TimeTableName;
                    K4Show = "✓";
                    K4ShowTooltips = K4TimeTableName;
                    K5Show = "✓";
                    K5ShowTooltips = K5TimeTableName;
                    K6Show = "✓";
                    K6ShowTooltips = K6TimeTableName;
                }
            }
        }



        private ObservableCollection<ListTreeNodeBase> _childTreeItemsInfo;

        public ObservableCollection<ListTreeNodeBase> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                {
                    _childTreeItemsInfo = new ObservableCollection<ListTreeNodeBase>();
                }
                return _childTreeItemsInfo;
            }
        }


        /// <summary>
        /// 加载节点，第一次使用
        /// </summary>
        public virtual void AddChild()
        {

        }

        public virtual void ReUpdate(int updateArgu)
        {

        }

        /// <summary>
        /// 提供删除所有子节点功能，当选中的终端变化时，前选择的终端子节点全部删除
        /// </summary>
        public void CleanChildren()
        {
            for (int i = this.ChildTreeItems.Count - 1; i > -1; i--)
            {
                this.ChildTreeItems.RemoveAt(i);
            }
        }
        private int _iphyd;

        public int PhyId
        {
            get { return _iphyd; }
            set
            {
                if (_iphyd != value)
                {
                    _iphyd = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }
    }
}
