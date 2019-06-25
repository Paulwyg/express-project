using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.LhEquipemntTree.SettingViewModel.Services;

namespace Wlst.Ux.LhEquipemntTree.SettingViewModel.ViewModel
{
    [Export(typeof (IISettingViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SettingViewModel : ObservableObject, IISettingViewModel
    {
        public SettingViewModel()
        {
            this.NavOnLoad();
        }

        #region  define

        private bool _isShowGrpInTreeModelShowId;

        /// <summary>
        /// 分组显示是否显示ID
        /// </summary>
        public bool IsShowGrpInTreeModelShowId
        {
            get { return _isShowGrpInTreeModelShowId; }
            set
            {
                if (value != _isShowGrpInTreeModelShowId)
                {
                    _isShowGrpInTreeModelShowId = value;
                    this.RaisePropertyChanged(() => this.IsShowGrpInTreeModelShowId);
                }
            }
        }


        private bool _isShowGrpInTreeModelShowTmlChildNode;

        /// <summary>
        /// 分组终端是否显示附加设备
        /// </summary>
        public bool IsShowGrpInTreeModelShowTmlChildNode
        {
            get { return _isShowGrpInTreeModelShowTmlChildNode; }
            set
            {
                if (value != _isShowGrpInTreeModelShowTmlChildNode)
                {
                    _isShowGrpInTreeModelShowTmlChildNode = value;
                    this.RaisePropertyChanged(() => this.IsShowGrpInTreeModelShowTmlChildNode);
                }
            }
        }



        private bool _isIsShowTheSelectdNodeInTree;

        /// <summary>
        /// 分组终端提供定位功能
        /// </summary>
        public bool IsShowTheSelectdNodeInTree
        {
            get { return _isIsShowTheSelectdNodeInTree; }
            set
            {
                if (value != _isIsShowTheSelectdNodeInTree)
                {
                    _isIsShowTheSelectdNodeInTree = value;
                    this.RaisePropertyChanged(() => this.IsShowTheSelectdNodeInTree);
                }
            }
        }

        private bool _isShowSingleTreeOnTab;

        /// <summary>
        /// 是否在主界面显示 单终端树
        /// </summary>
        public bool IsShowSingleTreeOnTab
        {
            get { return _isShowSingleTreeOnTab; }
            set
            {
                if (value != _isShowSingleTreeOnTab)
                {
                    _isShowSingleTreeOnTab = value;
                    this.RaisePropertyChanged(() => this.IsShowSingleTreeOnTab);
                }
            }
        }

        private bool _isShowMulTreeOnTab;

        /// <summary>
        /// 是否在主界面显示多终端树
        /// </summary>
        public bool IsShowMulTreeOnTab
        {
            get { return _isShowMulTreeOnTab; }
            set
            {
                if (value != _isShowMulTreeOnTab)
                {
                    _isShowMulTreeOnTab = value;
                    this.RaisePropertyChanged(() => this.IsShowMulTreeOnTab);
                }
            }
        }


        private bool _isIsSelectGrpMapOnlyShow;

        /// <summary>
        /// 
        /// </summary>
        public bool IsSelectGrpMapOnlyShow
        {
            get { return _isIsSelectGrpMapOnlyShow; }
            set
            {
                if (value != _isIsSelectGrpMapOnlyShow)
                {
                    _isIsSelectGrpMapOnlyShow = value;
                    this.RaisePropertyChanged(() => this.IsSelectGrpMapOnlyShow);
                }
            }
        }


         private bool _isIsRutsNotShowError;

        /// <summary>
        /// 
        /// </summary>
        public bool IsRutsNotShowError
        {
            get { return _isIsRutsNotShowError; }
            set
            {
                if (value != _isIsRutsNotShowError)
                {
                    _isIsRutsNotShowError = value;
                    this.RaisePropertyChanged(() => this.IsRutsNotShowError);
                }
            }
        }

        private int _isIsRutsNotShowNullK;

        /// <summary>
        /// 
        /// </summary>
        public int IsRutsNotShowNullK
        {
            get { return _isIsRutsNotShowNullK; }
            set
            {
                if (value != _isIsRutsNotShowNullK)
                {
                    _isIsRutsNotShowNullK = value;
                    this.RaisePropertyChanged(() => this.IsRutsNotShowNullK);
                }
            }
        }

        private int _isShowRapidOp;

        /// <summary>
        /// IsShowRapidOp
        /// </summary>
        public int IsShowRapidOp
        {
            get { return _isShowRapidOp; }
            set
            {
                if (value != _isShowRapidOp)
                {
                    _isShowRapidOp = value;
                    this.RaisePropertyChanged(() => this.IsShowRapidOp);
                }
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _showRapidOp;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> ShowRapidOp
        {
            get
            {
                if (_showRapidOp == null)
                {
                    _showRapidOp = new ObservableCollection<NameValueInt>();
                    //for (int i = 1; i < 24; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() {Name = i + "小时", Value = i});
                    //}
                    //for (int i = 1; i < 94; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() {Name = i + " 天", Value = i*24});
                    //}

                    _showRapidOp.Add(new NameValueInt() { Name = "标准模式", Value = 0 });
                    _showRapidOp.Add(new NameValueInt() { Name = "高速模式", Value = 1 });


                }
                return _showRapidOp;
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _rutsNotShowNullK;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> RutsNotShowNullK
        {
            get
            {
                if (_rutsNotShowNullK == null)
                {
                    _rutsNotShowNullK = new ObservableCollection<NameValueInt>();
                    //for (int i = 1; i < 24; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() {Name = i + "小时", Value = i});
                    //}
                    //for (int i = 1; i < 94; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() {Name = i + " 天", Value = i*24});
                    //}

                    _rutsNotShowNullK.Add(new NameValueInt() { Name = "不显示", Value = 0 });
                    _rutsNotShowNullK.Add(new NameValueInt() { Name = "屏蔽无效", Value = 1 });
                    _rutsNotShowNullK.Add(new NameValueInt() { Name = "全部显示", Value = 2 });


                }
                return _rutsNotShowNullK;
            }
        }
        public int _indexsordtsss;
        /// <summary>
        /// 终端树排序  1 按照终端物理地址排序，2 按照拼音排序，3 按照分组内的地址排序，4 按照逻辑地址排序
        /// </summary>
        public int TreeSortBy
        {
            get { return _indexsordtsss; }
            set
            {
                if (value != _indexsordtsss)
                {
                    _indexsordtsss = value;
                    this.RaisePropertyChanged(() => this.TreeSortBy);
                }
            } 
        }

        private int _searchLimit;

        /// <summary>
        /// 是否在主界面显示 单终端树
        /// </summary>
        public int SearchLimit
        {
            get { return _searchLimit; }
            set
            {
                if (value != _searchLimit)
                {
                    _searchLimit = value;
                    this.RaisePropertyChanged(() => this.SearchLimit);
                }
            }
        }

        #endregion





        private DateTime _dtApply;
        private ICommand _cmdApply;

        public ICommand CmdApply
        {
            get
            {

                if (_cmdApply == null) _cmdApply = new RelayCommand(Ex, CanEx, false);
                return _cmdApply;
            }
        }

        //todo 目前未作对终端过滤  如停运不发送选测等
        private void Ex()
        {
            _dtApply = DateTime.Now;
            SettingExtend.Myself.UpdateSetting(this.IsShowGrpInTreeModelShowId,
                                              this.IsShowGrpInTreeModelShowTmlChildNode,
                                              this.IsShowSingleTreeOnTab,
                                              this.IsShowMulTreeOnTab, this.IsSelectGrpMapOnlyShow,this .IsShowTheSelectdNodeInTree,this .TreeSortBy ,this .IsRutsNotShowError,this.IsRutsNotShowNullK,this.IsShowRapidOp,this.SearchLimit  );
        }

        private bool CanEx()
        {
            if (
                this.IsShowGrpInTreeModelShowId == UxTreeSetting.IsShowGrpInTreeModelShowId &&
                this.IsShowGrpInTreeModelShowTmlChildNode == UxTreeSetting.IsShowGrpInTreeModelShowTmlChildNode &&
                this.IsShowSingleTreeOnTab == UxLhTreeSetting.IsShowSingleTreeOnTab &&
                this.IsShowMulTreeOnTab == UxTreeSetting.IsShowMulTreeOnTab &&
                  this.IsShowTheSelectdNodeInTree == SettingExtend.Myself.IsShowTheSelectdNodeInTree &&
                this.IsSelectGrpMapOnlyShow == UxTreeSetting.IsSelectGrpMapOnlyShow &&
                 this.IsRutsNotShowError == UxTreeSetting.IsRutsNotShowError &&
                 this.IsRutsNotShowNullK == UxLhTreeSetting.IsRutsNotShowNullK &&
                 this.IsShowRapidOp == UxLhTreeSetting.IsShowRapidOp &&
                this .TreeSortBy ==UxTreeSetting .TreeSortBy &&
                this.SearchLimit== UxLhTreeSetting.SearchLimit
            
                )
                return false;
            return DateTime.Now.Ticks - _dtApply.Ticks > 10000000;
        }

        public void NavOnLoad(params object[] parsObjects)
        {

            this.IsShowGrpInTreeModelShowId = UxTreeSetting.IsShowGrpInTreeModelShowId;
            this.IsShowGrpInTreeModelShowTmlChildNode = UxTreeSetting.IsShowGrpInTreeModelShowTmlChildNode;
            this.IsShowSingleTreeOnTab = UxLhTreeSetting.IsShowSingleTreeOnTab;
            this.IsShowMulTreeOnTab = UxTreeSetting.IsShowMulTreeOnTab;
            this.IsSelectGrpMapOnlyShow = UxTreeSetting.IsSelectGrpMapOnlyShow;
            this.IsShowTheSelectdNodeInTree = SettingExtend.Myself.IsShowTheSelectdNodeInTree;
            this.TreeSortBy = SettingExtend.Myself.TreeSortBy;
            this.IsRutsNotShowError = SettingExtend.Myself.IsRutsNotShowError;
            this.IsRutsNotShowNullK = SettingExtend.Myself.IsRutsNotShowNullK;
            this.IsShowRapidOp = SettingExtend.Myself.IsShowRapidOp;
            this.SearchLimit = SettingExtend.Myself.SearchLimit;
        }


    };

}
