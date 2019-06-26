using System;
using System.ComponentModel;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
 

namespace Wlst.Ux.CoreModuelConfig.ViewModel
{
    public class ModuleItemInfoModel : INotifyPropertyChanged
    {
        private int _moduleId;
        /// <summary>
        /// 模块Id
        /// </summary>
        public int ModuleId
        {
            get { return _moduleId; }
            set
            {
                if (_moduleId != value)
                {
                    _moduleId = value;
                    this.RiseProperyChange("ModuleId");
                }
            }
        }

        private string _catalogPath;
        /// <summary>
        /// 模块路径
        /// </summary>
        public string CatalogPath
        {
            get { return _catalogPath; }
            set
            {
                if (_catalogPath != value)
                {
                    _catalogPath = value;
                    this.RiseProperyChange("CatalogPath");
                }
            }
        }

        private string _name;
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.RiseProperyChange("Name");
                }
            }
        }

        private string _version;
        /// <summary>
        /// 通过路径加载后的程序集版本
        /// </summary>
        public string Version
        {
            get { return _version; }
            set
            {
                if (_version != value)
                {
                    _version = value;
                    this.RiseProperyChange("Version");
                }
            }
        }

        private string _company;
        /// <summary>
        /// 通过路径加载后的程序集包含的注册公司
        /// </summary>
        public string Company
        {
            get { return _company; }
            set
            {
                if (_company != value)
                {
                    _company = value;
                    this.RiseProperyChange("Company");
                }
            }
        }

        private string _moduleType;
        /// <summary>
        /// 模块类别 是Core、Base、Model
        /// </summary>
        public string  ModuleType
        {
            get { return _moduleType; }
            set
            {
                if (_moduleType != value)
                {
                    _moduleType = value;
                    this.RiseProperyChange("ModuleType");
                }
            }
        }

        private bool _shouldLoadOnFirstRun;
        /// <summary>
        /// 若非基础性模块  是否需要在程序运行时加载
        /// </summary>
        public bool ShouldLoadOnFirstRun
        {
            get { return _shouldLoadOnFirstRun; }
            set
            {
                if (_shouldLoadOnFirstRun != value)
                {
                    _shouldLoadOnFirstRun = value;
                    this.RiseProperyChange("ShouldLoadOnFirstRun");
                    if (OnShouldLoadOnFirstRunChange == null) return;
                    OnShouldLoadOnFirstRunChange(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler OnShouldLoadOnFirstRunChange;// = null;//= delegate { };

        private bool _isLoad;
        /// <summary>
        /// 模块是否已经加载
        /// </summary>
        public bool IsLoad
        {
            get { return _isLoad; }
            set
            {
                if (_isLoad != value)
                {
                    _isLoad = value;
                    this.RiseProperyChange("IsLoad");
                }
            }
        }

        private bool _isLoadBySystem;

        /// <summary>
        /// 是否为系统自动加载
        /// </summary>
        public bool IsLoadBySystemAuto
        {
            get { return _isLoadBySystem; }
            set
            {
                _isLoadBySystem = value;
                IsCanCheckBoxEnable = !_isLoadBySystem;
            }
        }


        private bool _isCanCheckBoxEnable;
        public bool IsCanCheckBoxEnable
        {
            get { return _isCanCheckBoxEnable; }
            set
            {
                if (_isCanCheckBoxEnable != value)
                {
                    _isCanCheckBoxEnable = value;
                    this.RiseProperyChange("IsCanCheckBoxEnable");
                }
            }
        }

        #region event

        #region load command

        private ICommand _iCmdLoad;

        public ICommand CmdLoad
        {
            get
            {
                if (_iCmdLoad == null) _iCmdLoad = new RelayCommand(ExLoad, CanExLoad,false );
                return _iCmdLoad;
            }
        }

        private void ExLoad()
        {
            if (OnCmdLoad == null) return;
            OnCmdLoad(this, EventArgs.Empty);
        }

        public event EventHandler OnCmdLoad;// = null;//= delegate { };

        private bool CanExLoad()
        {
            return !IsLoad && !IsLoadBySystemAuto ;
        }

        #endregion

        #region unload command

        private ICommand _iCmdUnLoad;

        public ICommand CmdUnLoad
        {
            get
            {
                if (_iCmdUnLoad == null) _iCmdUnLoad = new RelayCommand(ExUnLoad, CanExUnLoad,false );
                return _iCmdUnLoad;
            }
        }

        private void ExUnLoad()
        {
            if (OnCmdUnLoad == null) return;
            OnCmdUnLoad(this, EventArgs.Empty);
        }

        public event EventHandler OnCmdUnLoad;// = null;

        private bool CanExUnLoad()
        {
            return IsLoad && !IsLoadBySystemAuto;
        }

        #endregion


        #region delete command

        private ICommand _iCmdDelete;

        public ICommand CmdDelete
        {
            get
            {
                if (_iCmdDelete == null) _iCmdDelete = new RelayCommand(ExDelete,CanExDelete ,false );
                return _iCmdDelete;
            }
        }

        private void ExDelete()
        {
            if (OnCmdDelete == null) return;
            OnCmdDelete(this, EventArgs.Empty  );
        }

        private bool CanExDelete()
        {
            return !IsLoad && !IsLoadBySystemAuto;
        }

        public event EventHandler OnCmdDelete;//= delegate { };


        #endregion

        #endregion

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        public void RiseProperyChange(string properyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(properyName));

        }

        #endregion
    };

}
