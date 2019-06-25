using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.Wj6005Module.Jd601ManageSettingViewModel.Services;


namespace Wlst.Ux.Wj6005Module.Jd601ManageSettingViewModel.ViewModel
{
    [Export(typeof (IIJd601ManageSettingViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Jd601ManageSettingViewModel : ObservableObject, IIJd601ManageSettingViewModel
    {
        public Jd601ManageSettingViewModel()
        {
             this.IsOnSelectNodeNavToParsSetView=Jd601LoadSet.Myself.IsOnSelectNodeNavToParsSetView ;//();
            this.IsShowGrpInTreeModelShowId = Jd601LoadSet.Myself.IsShowGrpInTreeModelShowId;
            this.IsShowTreeOnTab = Jd601LoadSet.Myself.IsShowTreeOnTab;
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
            Jd601LoadSet.Myself.IsOnSelectNodeNavToParsSetView = this.IsOnSelectNodeNavToParsSetView;//();
            Jd601LoadSet.Myself.IsShowGrpInTreeModelShowId = this.IsShowGrpInTreeModelShowId;
            Jd601LoadSet.Myself.IsShowTreeOnTab = this.IsShowTreeOnTab;
            Jd601LoadSet.Myself.SavConfig();
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

    public class Jd601LoadSet
    {
        public bool IsShowGrpInTreeModelShowId;
        public bool IsShowTreeOnTab;
        public bool IsOnSelectNodeNavToParsSetView;


        static Jd601LoadSet _myself;
        public static Jd601LoadSet Myself
        {
            get
            {
                if (_myself == null) new Jd601LoadSet();
                return _myself;
            }
        }

        private  const string XmlConfigName = "Jd601SetConfg";



        private Jd601LoadSet()
        {

            if (_myself == null) _myself = this;

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

            Jd601ManageViewModel.ViewModel.Jd601ManageViewModel.OnSelectNodeChangeNavToParsSet =
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
            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, XmlConfigName);


            Jd601ManageViewModel.ViewModel.Jd601ManageViewModel.OnSelectNodeChangeNavToParsSet =
                IsOnSelectNodeNavToParsSetView;

            RegionManage.ShowViewByIdAttachRegion(
                Ux.Wj6005Module.Services.ViewIdAssign.Jd601ManageViewId,
                IsShowTreeOnTab);
        }

    };

}
