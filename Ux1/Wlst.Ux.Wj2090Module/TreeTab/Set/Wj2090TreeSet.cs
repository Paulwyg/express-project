using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;

namespace Wlst.Ux.Wj2090Module.TreeTab.Set
{
    [Export(typeof (IIWj2090TreeSet))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Wj2090TreeSet : Wlst.Cr.Core.CoreServices.ObservableObject, IIWj2090TreeSet
    {
        public Wj2090TreeSet()
        {
            IsShowGrpInTreeModelShowId = Wj2090TreeSetLoad.Myself.IsShowGrpInTreeModelShowId;
            IsShowConOnNodeSelected = Wj2090TreeSetLoad.Myself.IsShowConOnNodeSelected;
            IsShowTreeOnTab = Wj2090TreeSetLoad.Myself.IsShowTreeOnTab;
            IsIconFollowTheRtu = Wj2090TreeSetLoad.Myself.IsIconFollowTheRtu;
            _dtApply = DateTime.Now.AddDays(-1);

            IsShowCtrlTime = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2090, 1, false);
            IsShowArgsOnNewData = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2090, 2, false);
            //this.NavOnLoad();
        }

        #region  define

        private bool _isIsShowConOnNodeSelected;

        /// <summary>
        /// 分组显示是否显示控制器
        /// </summary>
        public bool IsShowConOnNodeSelected
        {
            get { return _isIsShowConOnNodeSelected; }
            set
            {
                if (value != _isIsShowConOnNodeSelected)
                {
                    _isIsShowConOnNodeSelected = value;
                    this.RaisePropertyChanged(() => this.IsShowConOnNodeSelected);
                }
            }
        }


        private bool _isIconFollowTheRtu;

        /// <summary>
        /// 单灯图标跟随绑定的终端状态
        /// </summary>
        public bool IsIconFollowTheRtu
        {
            get { return _isIconFollowTheRtu; }
            set
            {
                if (value != _isIconFollowTheRtu)
                {
                    _isIconFollowTheRtu = value;
                    this.RaisePropertyChanged(() => this.IsIconFollowTheRtu);
                }
            }
        }

        private bool _isShowCtrlTime;

        /// <summary>
        /// 单灯方案显示控制器方案
        /// </summary>
        public bool IsShowCtrlTime
        {
            get { return _isShowCtrlTime; }
            set
            {
                if (value != _isShowCtrlTime)
                {
                    _isShowCtrlTime = value;
                    this.RaisePropertyChanged(() => this.IsShowCtrlTime);
                }
            }
        }

       
        private bool _isShowTreeOnTab;

        /// <summary>
        /// 是否在主界面显示 
        /// </summary>
        public bool IsShowTreeOnTab
        {
            get { return _isShowTreeOnTab; }
            set
            {
                if (value != _isShowTreeOnTab)
                {
                    _isShowTreeOnTab = value;
                    this.RaisePropertyChanged(() => this.IsShowTreeOnTab);
                }
            }
        }

        private bool _isShowArgsOnNewData;

        /// <summary>
        /// 是否在最新数据中显示参数
        /// </summary>
        public bool IsShowArgsOnNewData
        {
            get { return _isShowArgsOnNewData; }
            set
            {
                if (value != _isShowArgsOnNewData)
                {
                    _isShowArgsOnNewData = value;
                    this.RaisePropertyChanged(() => this.IsShowArgsOnNewData);
                }
            }
        }




        private bool _iIIsShowGrpInTreeModelShowIdiew;

        /// <summary>
        /// 是否在点击终端的时候 直接导航到设置界面
        /// </summary>
        public bool IsShowGrpInTreeModelShowId
        {
            get { return _iIIsShowGrpInTreeModelShowIdiew; }
            set
            {
                if (value != _iIIsShowGrpInTreeModelShowIdiew)
                {
                    _iIIsShowGrpInTreeModelShowIdiew = value;
                    this.RaisePropertyChanged(() => this.IsShowGrpInTreeModelShowId);
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
            Wj2090TreeSetLoad.Myself.IsShowConOnNodeSelected = IsShowConOnNodeSelected;
            Wj2090TreeSetLoad.Myself.IsShowTreeOnTab = IsShowTreeOnTab;
            Wj2090TreeSetLoad.Myself.IsShowGrpInTreeModelShowId = IsShowGrpInTreeModelShowId;
            Wj2090TreeSetLoad.Myself.IsIconFollowTheRtu = IsIconFollowTheRtu;
            Wj2090TreeSetLoad.Myself.SavConfig();

            var dicOp = new Dictionary<int, string>();
            var dicDesc = new Dictionary<int, string>();

            dicOp.Add(1, IsShowCtrlTime ? "1" : "0");
            dicDesc.Add(1, "单灯方案显示控制器方案");

            dicOp.Add(2, IsShowArgsOnNewData ? "1" : "0");
            dicDesc.Add(2, "最新数据显示参数信息");

            Wlst.Cr.CoreOne.Services.OptionXmlSvr.SaveXml(2090, dicOp, dicDesc); 

            
        }

        private bool CanEx()
        {
            return DateTime.Now.Ticks - _dtApply.Ticks > 30000000;
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            //LoadConfig();

            IsShowConOnNodeSelected = Wj2090TreeSetLoad.Myself.IsShowConOnNodeSelected;
            IsShowTreeOnTab = Wj2090TreeSetLoad.Myself.IsShowTreeOnTab;
            IsShowGrpInTreeModelShowId = Wj2090TreeSetLoad.Myself.IsShowGrpInTreeModelShowId;
            IsIconFollowTheRtu = Wj2090TreeSetLoad.Myself.IsIconFollowTheRtu;
            IsShowCtrlTime = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2090, 1, false);
            IsShowArgsOnNewData = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2090, 2, false);
        }
    }



}
