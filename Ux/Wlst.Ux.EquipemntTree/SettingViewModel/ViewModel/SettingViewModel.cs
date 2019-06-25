using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.EquipemntTree.SettingViewModel.Services;

namespace Wlst.Ux.EquipemntTree.SettingViewModel.ViewModel
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



        private bool _isShowMapPoint;

        /// <summary>
        /// 电子地图点击后，终端树呈现选中设备
        /// </summary>
        public bool IsShowMapPoint
        {
            get { return _isShowMapPoint; }
            set
            {
                if (value != _isShowMapPoint)
                {
                    _isShowMapPoint = value;
                    this.RaisePropertyChanged(() => this.IsShowMapPoint);
                }
            }
        }

        private bool _canCheckMapPoint;

        /// <summary>
        /// 可以勾选    电子地图点击后，终端树呈现选中设备
        /// </summary>
        public bool CanCheckMapPoint
        {
            get { return _canCheckMapPoint; }
            set
            {
                if (value != _canCheckMapPoint)
                {
                    _canCheckMapPoint = value;
                    this.RaisePropertyChanged(() => this.CanCheckMapPoint);
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
                    //如果勾选了联动，则不让勾选电子地图点击联动树呈现
                    if (_isIsShowTheSelectdNodeInTree) IsShowMapPoint = false;
                    CanCheckMapPoint = !_isIsShowTheSelectdNodeInTree;
                   
                    this.RaisePropertyChanged(() => this.IsShowTheSelectdNodeInTree);
                }
            }
        }

        private bool _isShowKeyWords;

        /// <summary>
        /// 快速查询显示关键字
        /// </summary>
        public bool IsShowKeyWords
        {
            get { return _isShowKeyWords; }
            set
            {
                if (value != _isShowKeyWords)
                {
                    _isShowKeyWords = value;
                    this.RaisePropertyChanged(() => this.IsShowKeyWords);
                }
            }
        }


        private bool _isShowTreeChk;

        /// <summary>
        /// 终端树显示多选框
        /// </summary>
        public bool IsShowTreeChk
        {
            get { return _isShowTreeChk; }
            set
            {
                if (value != _isShowTreeChk)
                {
                    _isShowTreeChk = value;
                    this.RaisePropertyChanged(() => this.IsShowTreeChk);
                }
            }
        }


        private bool _isFastControl;

        /// <summary>
        /// 控制中心快速查询
        /// </summary>
        public bool IsFastControl
        {
            get { return _isFastControl; }
            set
            {
                if (value != _isFastControl)
                {
                    _isFastControl = value;
                    this.RaisePropertyChanged(() => this.IsFastControl);
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


        private bool _isTmlRightMenuShowTimeTable;

        /// <summary>
        /// 右击开关灯显示时间表
        /// </summary>
        public bool IsTmlRightMenuShowTimeTable
        {
            get { return _isTmlRightMenuShowTimeTable; }
            set
            {
                if (value != _isTmlRightMenuShowTimeTable)
                {
                    _isTmlRightMenuShowTimeTable = value;
                    this.RaisePropertyChanged(() => this.IsTmlRightMenuShowTimeTable);
                }
            }
        }


        private bool _isTmlRightMenuShowLoopName;

        /// <summary>
        /// 右击菜单开关灯显示回路名称
        /// </summary>
        public bool IsTmlRightMenuShowLoopName
        {
            get { return _isTmlRightMenuShowLoopName; }
            set
            {
                if (value != _isTmlRightMenuShowLoopName)
                {
                    _isTmlRightMenuShowLoopName = value;
                    this.RaisePropertyChanged(() => this.IsTmlRightMenuShowLoopName);
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
        /// 查询限制   0 不限制   1 回车查询   >1 呈现上限
        /// </summary>
        public int SearchLimit
        {
            get { return _searchLimit; }
            set
            {
                if (value != _searchLimit)
                {
                    if (value < 5) value = 5;
                    _searchLimit = value;
                    this.RaisePropertyChanged(() => this.SearchLimit);
                }
            }
        }


        private int _searchSearchLimitOpLimit;

        /// <summary>
        /// 查询限制   1 不限制   2 回车查询   >3 呈现上限
        /// </summary>
        public int SearchLimitOp
        {
            get { return _searchSearchLimitOpLimit; }
            set
            {
                if (value != _searchSearchLimitOpLimit)
                {
                    _searchSearchLimitOpLimit = value;
                    this.RaisePropertyChanged(() => this.SearchLimitOp);
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


            var emtpSearchLimit = SearchLimit;
            if (SearchLimitOp == 1) emtpSearchLimit = 0;
            if (SearchLimitOp == 2) emtpSearchLimit = 1;
            if (SearchLimitOp == 3)
            {
                if (SearchLimit < 5) SearchLimit = 5;
                emtpSearchLimit = SearchLimit;
            }

            SettingExtend.Myself.UpdateSetting(this.IsShowGrpInTreeModelShowId,
                                              this.IsShowGrpInTreeModelShowTmlChildNode,
                                              this.IsShowSingleTreeOnTab,
                                              this.IsShowMulTreeOnTab, this.IsSelectGrpMapOnlyShow, this.IsShowTheSelectdNodeInTree, this.TreeSortBy, this.IsRutsNotShowError, this.IsRutsNotShowNullK - 1, this.IsShowRapidOp, emtpSearchLimit);




            var dicOp = new Dictionary<int, string>();
            var dicDesc = new Dictionary<int, string>();
            dicOp.Add(1, IsShowKeyWords ? "1" : "0");
            dicDesc.Add(1, "快速查询显示关键字");

            dicOp.Add(2, IsShowTreeChk ? "1" : "0");
            dicDesc.Add(2, "终端树显示多选框");

            dicOp.Add(3, IsTmlRightMenuShowTimeTable ? "1" : "0");
            dicDesc.Add(3, "右击菜单开关灯显示绑定时间表名称");

            dicOp.Add(4, IsTmlRightMenuShowLoopName ? "1" : "0");
            dicDesc.Add(4, "右击菜单开关灯显示回路名称");

            dicOp.Add(5, IsFastControl ? "1" : "0");
            dicDesc.Add(5, "控制中心快速查询");

            dicOp.Add(6, IsShowMapPoint ? "1" : "0");
            dicDesc.Add(6, "电子地图点选后，终端树呈现");


            //      NewDataWidth = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 13, 600);//最新数据 宽度
            //IsShowAbc=Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 14, true);//是否显示 ABC 电流
            Wlst.Cr.CoreOne.Services.OptionXmlSvr.SaveXml(4001, dicOp, dicDesc); //3005模块，数据查询倒序


        }

        private bool CanEx()
        {
            //if (
            //    this.IsShowGrpInTreeModelShowId == UxTreeSetting.IsShowGrpInTreeModelShowId &&
            //    this.IsShowGrpInTreeModelShowTmlChildNode == UxTreeSetting.IsShowGrpInTreeModelShowTmlChildNode &&
            //    this.IsShowSingleTreeOnTab == UxTreeSetting.IsShowSingleTreeOnTab &&
            //    this.IsShowMulTreeOnTab == UxTreeSetting.IsShowMulTreeOnTab &&
            //      this.IsShowTheSelectdNodeInTree == SettingExtend.Myself.IsShowTheSelectdNodeInTree &&
            //    this.IsSelectGrpMapOnlyShow == UxTreeSetting.IsSelectGrpMapOnlyShow &&
            //     this.IsRutsNotShowError == UxTreeSetting.IsRutsNotShowError &&
            //     this.IsRutsNotShowNullK==UxTreeSetting.IsRutsNotShowNullK &&
            //     this.IsShowRapidOp == UxTreeSetting.IsShowRapidOp &&
            //    this .TreeSortBy ==UxTreeSetting .TreeSortBy &&
            //    this.SearchLimit== UxTreeSetting.SearchLimit &&
            //    this.IsShowKeyWords == Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 1) 
            
            //    )
            //    return false;
            return DateTime.Now.Ticks - _dtApply.Ticks > 10000000;
        }

        public void NavOnLoad(params object[] parsObjects)
        {

            this.IsShowGrpInTreeModelShowId = UxTreeSetting.IsShowGrpInTreeModelShowId;
            this.IsShowGrpInTreeModelShowTmlChildNode = UxTreeSetting.IsShowGrpInTreeModelShowTmlChildNode;
            this.IsShowSingleTreeOnTab = UxTreeSetting.IsShowSingleTreeOnTab;
            this.IsShowMulTreeOnTab = UxTreeSetting.IsShowMulTreeOnTab;
            this.IsSelectGrpMapOnlyShow = UxTreeSetting.IsSelectGrpMapOnlyShow;
            this.IsShowTheSelectdNodeInTree = SettingExtend.Myself.IsShowTheSelectdNodeInTree;
            this.TreeSortBy = SettingExtend.Myself.TreeSortBy;
            this.IsRutsNotShowError = SettingExtend.Myself.IsRutsNotShowError;
            this.IsRutsNotShowNullK = SettingExtend.Myself.IsRutsNotShowNullK+1;
            this.IsShowRapidOp = SettingExtend.Myself.IsShowRapidOp;

            //var emtpSearchLimit = SearchLimit;
            //if (SearchLimitOp == 1) emtpSearchLimit = 0;
            //if (SearchLimitOp == 2) emtpSearchLimit = 1;
            //if (SearchLimitOp == 3)
            //{
            //    if (SearchLimit < 5) SearchLimit = 5;
            //    emtpSearchLimit = SearchLimit;
            //}
           var tmp = SettingExtend.Myself.SearchLimit;
            if(tmp ==0 )
            {
                SearchLimitOp = 1;
                SearchLimit = 0;
            }
            else if (tmp == 1)
            {
                SearchLimitOp = 2;
                SearchLimit = 0;
            }
            else
            {
                SearchLimitOp = 3;
                SearchLimit = tmp;
            }

            this.IsShowKeyWords = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001,1,false ) ;
            this.IsShowTreeChk = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 2, false);

            this.IsTmlRightMenuShowTimeTable = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 3,false ) ;
            this.IsTmlRightMenuShowLoopName = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 4, false);

            this.IsFastControl = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 5, false);

            this.IsShowMapPoint = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 6, false);
            CanCheckMapPoint = true;
        }


    };

}
