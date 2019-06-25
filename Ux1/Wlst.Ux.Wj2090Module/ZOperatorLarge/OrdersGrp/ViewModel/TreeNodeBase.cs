using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Ux.Wj2090Module.ZOperatorLarge.OrdersGrp.Services;

namespace Wlst.Ux.Wj2090Module.ZOperatorLarge.OrdersGrp.ViewModel
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

        private ObservableCollection<SluInfo> _childTreeItemsInfo; //TreeNodeBase

        public ObservableCollection<SluInfo> ChildTreeItems
        {
            get { return _childTreeItemsInfo ?? (_childTreeItemsInfo = new ObservableCollection<SluInfo>()); }
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
                        //item.IsChecked = IsChecked;
                        //if (IsSwitch1Checked) item.IsSwitch1Checked = IsChecked;
                        //if (IsSwitch2Checked) item.IsSwitch2Checked = IsChecked;
                        //if (IsSwitch3Checked) item.IsSwitch3Checked = IsChecked;
                        //if (IsSwitch4Checked) item.IsSwitch4Checked = IsChecked;
                        //if (IsSwitch5Checked) item.IsSwitch5Checked = IsChecked;
                        //if (IsSwitch6Checked) item.IsSwitch6Checked = IsChecked;
                        //if (IsSwitch7Checked) item.IsSwitch7Checked = IsChecked;
                        //if (IsSwitch8Checked) item.IsSwitch8Checked = IsChecked;
                    }
                }
                IsShowSelectedCheckBox = IsChecked;

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



        private ObservableCollection<NameIntBoolx> _operationOperatorType = null;

        /// <summary>
        ///  1 全部、2 单数、3 双数、4、自定义操作   其他11-99： 如32 地址除3余2的控制器操作 101 不操作
        /// </summary>
        public ObservableCollection<NameIntBoolx> OperatorType
        {
            get
            {
                if (_operationOperatorType == null)
                {
                    _operationOperatorType = new ObservableCollection<NameIntBoolx>();
                    _operationOperatorType.Add(new NameIntBoolx() { Name = "全部", Value = 10, IsGrp = false, IsVisi = Visibility.Visible, IsSelected = false });
                    _operationOperatorType.Add(new NameIntBoolx() { Name = "单数", Value = 21, IsGrp = false, IsVisi = Visibility.Visible, IsSelected = false });
                    _operationOperatorType.Add(new NameIntBoolx() { Name = "双数", Value = 20, IsGrp = false, IsVisi = Visibility.Visible, IsSelected = false });
                    _operationOperatorType.Add(new NameIntBoolx() { Name = "隔二亮一", Value = 31, IsGrp = false, IsVisi = Visibility.Visible, IsSelected = false });


                    for (int i = 4; i < 11; i++)
                    {
                        _operationOperatorType.Add(new NameIntBoolx()
                        {
                            Name = "无",
                            Value = -1,
                            IsGrp = true,
                            IsVisi = Visibility.Collapsed,
                            IsSelected = false
                        });
                    }



                }
                return _operationOperatorType;
            }
    
        }
    }

}
