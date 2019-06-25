using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.Wj1050Module.Wj1050ManageSettingViewModel.Services;



namespace Wlst.Ux.Wj1050Module.Wj1050ManageSettingViewModel.ViewModel
{
    [Export(typeof (IIWj1050ManageSettingViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Wj1050ManageSettingViewModel : ObservableObject, IIWj1050ManageSettingViewModel
    {
        public Wj1050ManageSettingViewModel()
        {
            IsOnSelectNodeNavToParsSetView = Wj1050TreeSetLoad.Myself.IsOnSelectNodeNavToParsSetView;
            IsShowGrpInTreeModelShowId  = Wj1050TreeSetLoad.Myself.IsShowGrpInTreeModelShowId;
            IsShowTreeOnTab  = Wj1050TreeSetLoad.Myself.IsShowTreeOnTab;

            IsShowArea = Wj1050TreeSetLoad.Myself.IsShowArea;
            IsShowGrp = Wj1050TreeSetLoad.Myself.IsShowGrp;
            IsShowFid = Wj1050TreeSetLoad.Myself.IsShowFid;

            _dtApply = DateTime.Now.AddDays(-1);
            //this.NavOnLoad();
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

        private bool _isShowArea;

        /// <summary>
        /// 是否在显示区域
        /// </summary>
        public bool IsShowArea
        {
            get { return _isShowArea; }
            set
            {
                if (value != _isShowArea)
                {
                    _isShowArea = value;
                    this.RaisePropertyChanged(() => this.IsShowArea);
                }
            }
        }

        private bool _isShowGrp;

        /// <summary>
        /// 是否在显示分组
        /// </summary>
        public bool IsShowGrp
        {
            get { return _isShowGrp; }
            set
            {
                if (value != _isShowGrp)
                {
                    _isShowGrp = value;
                    this.RaisePropertyChanged(() => this.IsShowGrp);
                }
            }
        }

        private bool _isShowFid;

        /// <summary>
        /// 是否在显示终端信息
        /// </summary>
        public bool IsShowFid
        {
            get { return _isShowFid; }
            set
            {
                if (value != _isShowFid)
                {
                    _isShowFid = value;
                    this.RaisePropertyChanged(() => this.IsShowFid);
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

        private bool _iIsOnSelectNodeNavToParsSetView;

        /// <summary>
        /// 是否在点击终端的时候 直接导航到设置界面
        /// </summary>
        public bool IsOnSelectNodeNavToParsSetView
        {
            get { return _iIsOnSelectNodeNavToParsSetView; }
            set
            {
                if (value != _iIsOnSelectNodeNavToParsSetView)
                {
                    _iIsOnSelectNodeNavToParsSetView = value;
                    this.RaisePropertyChanged(() => this.IsOnSelectNodeNavToParsSetView);
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
            Wj1050TreeSetLoad.Myself.IsOnSelectNodeNavToParsSetView = IsOnSelectNodeNavToParsSetView;
            Wj1050TreeSetLoad.Myself.IsShowGrpInTreeModelShowId = IsShowGrpInTreeModelShowId;
            Wj1050TreeSetLoad.Myself.IsShowTreeOnTab = IsShowTreeOnTab;

            Wj1050TreeSetLoad.Myself.IsShowArea = IsShowArea;
            Wj1050TreeSetLoad.Myself.IsShowGrp = IsShowGrp;
            Wj1050TreeSetLoad.Myself.IsShowFid = IsShowFid;

            Wj1050TreeSetLoad.Myself.SavConfig();
        }

        private bool CanEx()
        {
            return DateTime.Now.Ticks - _dtApply.Ticks > 30000000;
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            //LoadConfig();
        }
    }

    public class Wj1050TreeSetLoad
    {
        public bool IsShowGrpInTreeModelShowId;
        public bool IsShowTreeOnTab;
        public bool IsOnSelectNodeNavToParsSetView;

        public bool IsShowArea;
        public bool IsShowGrp;
        public bool IsShowFid;

        static Wj1050TreeSetLoad myself;
        public static Wj1050TreeSetLoad Myself
        {
            get
            {
                if (myself == null) new Wj1050TreeSetLoad();
                return myself;
            }
        }


        public const string XmlConfigName = "Wj1050SetConfg";

        private  Wj1050TreeSetLoad()
        {

            if (myself == null) myself = this;



            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
            if (info.ContainsKey("IsShowGrpInTreeModelShowId"))
            {
                IsShowGrpInTreeModelShowId = info["IsShowGrpInTreeModelShowId"].Contains("yes");
            }
            else IsShowGrpInTreeModelShowId = true;

            if (info.ContainsKey("IsOnSelectNodeNavToParsSetView"))
            {
                IsOnSelectNodeNavToParsSetView = info["IsOnSelectNodeNavToParsSetView"].Contains("yes");
            }
            else IsOnSelectNodeNavToParsSetView = true;

            if (info.ContainsKey("IsShowTreeOnTab"))
            {
                IsShowTreeOnTab = info["IsShowTreeOnTab"].Contains("yes");
            }
            else IsShowTreeOnTab = true;


            if (info.ContainsKey("IsShowArea"))
            {
                IsShowArea = info["IsShowArea"].Contains("yes");
            }
            else IsShowArea = true;

            if (info.ContainsKey("IsShowGrp"))
            {
                IsShowGrp = info["IsShowGrp"].Contains("yes");
            }
            else IsShowArea = true;

            if (info.ContainsKey("IsShowFid"))
            {
                IsShowFid = info["IsShowFid"].Contains("yes");
            }
            else IsShowFid = true;


            Wj1050Module .Wj1050ManageViewModel .ViewModel .Wj1050ManageViewModel.OnSelectNodeChangeNavToParsSet =
                IsOnSelectNodeNavToParsSetView;
        }




        public void SavConfig()
        {
            var info = new Dictionary<string, string>();
            if (IsShowGrpInTreeModelShowId) info.Add("IsShowGrpInTreeModelShowId", "yes");
            else info.Add("IsShowGrpInTreeModelShowId", "no");

            if (IsOnSelectNodeNavToParsSetView) info.Add("IsOnSelectNodeNavToParsSetView", "yes");
            else info.Add("IsOnSelectNodeNavToParsSetView", "no");

            if (IsShowTreeOnTab) info.Add("IsShowTreeOnTab", "yes");
            else info.Add("IsShowTreeOnTab", "no");

            if (IsShowArea) info.Add("IsShowArea", "yes");
            else info.Add("IsShowArea", "no");

            if (IsShowGrp) info.Add("IsShowGrp", "yes");
            else info.Add("IsShowGrp", "no");

            if (IsShowFid) info.Add("IsShowFid", "yes");
            else info.Add("IsShowFid", "no");

            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, XmlConfigName);



            Wj1050Module.Wj1050ManageViewModel.ViewModel.Wj1050ManageViewModel.OnSelectNodeChangeNavToParsSet =
                IsOnSelectNodeNavToParsSetView;

            RegionManage.ShowViewByIdAttachRegion(
                Ux.Wj1050Module.Services.ViewIdAssign.Wj1050ManageViewId,
                IsShowTreeOnTab);
        }

    };

}
