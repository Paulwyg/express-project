using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.Wj1090Module.LduTreeSettingViewModel.Services;
using Wlst.Ux.Wj1090Module.NewData.ViewModel;


namespace Wlst.Ux.Wj1090Module.LduTreeSettingViewModel.ViewModel
{
    [Export(typeof (IILduTreeSettingViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class LduTreeSettingViewModel : ObservableObject, IILduTreeSettingViewModel
    {
        public LduTreeSettingViewModel()
        {
            this.IsShowGrpInTreeModelShowId = Wj1090TreeSetLoad.Myself.IsShowGrpInTreeModelShowId;
            this.IsShowTreeOnTab = Wj1090TreeSetLoad.Myself.IsShowTreeOnTab;
            this.IsShowArea = Wj1090TreeSetLoad.Myself.IsShowArea;
            this.IsShowGrp = Wj1090TreeSetLoad.Myself.IsShowGrp;
            this.IsShowFid = Wj1090TreeSetLoad.Myself.IsShowFid;

            this.IsShowCJSJ = Wj1090TreeSetLoad.Myself.IsShowCJSJ;
            this.IsShowZT = Wj1090TreeSetLoad.Myself.IsShowZT;
            this.IsShowYGGL = Wj1090TreeSetLoad.Myself.IsShowYGGL;
            this.IsShowWGGL = Wj1090TreeSetLoad.Myself.IsShowWGGL;
            this.IsShowGLYS = Wj1090TreeSetLoad.Myself.IsShowGLYS;
            this.IsShowLDL = Wj1090TreeSetLoad.Myself.IsShowLDL;
            this.IsShowXHQD = Wj1090TreeSetLoad.Myself.IsShowXHQD;
            this.IsShowHLZK = Wj1090TreeSetLoad.Myself.IsShowHLZK;
            this.IsShowYYXH = Wj1090TreeSetLoad.Myself.IsShowYYXH;
            this.IsShowXHZS = Wj1090TreeSetLoad.Myself.IsShowXHZS;
            this.IsShowBJSZ = Wj1090TreeSetLoad.Myself.IsShowBJSZ;

            _dtApply = DateTime.Now.AddDays(-1);
            //this.NavOnLoad();
        }

        #region  define

        private bool _isShowGrpInTreeModelShowId;

        /// <summary>
        /// 分组显示是否显示ID
        /// </summary>
        public bool 
            IsShowGrpInTreeModelShowId
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

        //private bool _iIsOnSelectNodeNavToParsSetView;

        ///// <summary>
        ///// 是否在点击终端的时候 直接导航到设置界面
        ///// </summary>
        //public bool IsOnSelectNodeNavToParsSetView
        //{
        //    get { return _iIsOnSelectNodeNavToParsSetView; }
        //    set
        //    {
        //        if (value != _iIsOnSelectNodeNavToParsSetView)
        //        {
        //            _iIsOnSelectNodeNavToParsSetView = value;
        //            this.RaisePropertyChanged(() => this.IsOnSelectNodeNavToParsSetView);
        //        }
        //    }
        //}

        #endregion

        #region 采集时间是否显示
        private bool _isShowCJSJ;

        public bool IsShowCJSJ
        {
            get { return _isShowCJSJ; }
            set
            {
                if (_isShowCJSJ != value)
                {
                    _isShowCJSJ = value;
                    this.RaisePropertyChanged(() => this.IsShowCJSJ);

                }
            }
        }
        #endregion

        #region 状态是否显示
        private bool _isShowZT;

        public bool IsShowZT
        {
            get { return _isShowZT; }
            set
            {
                if (_isShowZT != value)
                {
                    _isShowZT = value;
                    this.RaisePropertyChanged(() => this.IsShowZT);

                }
            }
        }
        #endregion

        #region 有功功率是否显示
        private bool _isShowYGGL;

        public bool IsShowYGGL
        {
            get { return _isShowYGGL; }
            set
            {
                if (_isShowYGGL != value)
                {
                    _isShowYGGL = value;
                    this.RaisePropertyChanged(() => this.IsShowYGGL);

                }
            }
        }
        #endregion

        #region 无功功率是否显示
        private bool _isShowWGGL;

        public bool IsShowWGGL
        {
            get { return _isShowWGGL; }
            set
            {
                if (_isShowWGGL != value)
                {
                    _isShowWGGL = value;
                    this.RaisePropertyChanged(() => this.IsShowWGGL);

                }
            }
        }
        #endregion

        #region 功率因数是否显示
        private bool _isShowGLYS;

        public bool IsShowGLYS
        {
            get { return _isShowGLYS; }
            set
            {
                if (_isShowGLYS != value)
                {
                    _isShowGLYS = value;
                    this.RaisePropertyChanged(() => this.IsShowGLYS);

                }
            }
        }
        #endregion

        #region 亮灯率是否显示
        private bool _isShowLDL;

        public bool IsShowLDL
        {
            get { return _isShowLDL; }
            set
            {
                if (_isShowLDL != value)
                {
                    _isShowLDL = value;
                    this.RaisePropertyChanged(() => this.IsShowLDL);

                }
            }
        }
        #endregion

        #region 信号强度是否显示
        private bool _isShowXHQD;

        public bool IsShowXHQD
        {
            get { return _isShowXHQD; }
            set
            {
                if (_isShowXHQD != value)
                {
                    _isShowXHQD = value;
                    this.RaisePropertyChanged(() => this.IsShowXHQD);

                }
            }
        }
        #endregion

        #region 回路阻抗是否显示
        private bool _isShowHLZK;

        public bool IsShowHLZK
        {
            get { return _isShowHLZK; }
            set
            {
                if (_isShowHLZK != value)
                {
                    _isShowHLZK = value;
                    this.RaisePropertyChanged(() => this.IsShowHLZK);

                }
            }
        }
        #endregion

        #region 有用信号是否显示
        private bool _isShowYYXH;

        public bool IsShowYYXH
        {
            get { return _isShowYYXH; }
            set
            {
                if (_isShowYYXH != value)
                {
                    _isShowYYXH = value;
                    this.RaisePropertyChanged(() => this.IsShowYYXH);

                }
            }
        }
        #endregion

        #region 信号总数是否显示
        private bool _isShowXHZS;

        public bool IsShowXHZS
        {
            get { return _isShowXHZS; }
            set
            {
                if (_isShowXHZS != value)
                {
                    _isShowXHZS = value;
                    this.RaisePropertyChanged(() => this.IsShowXHZS);

                }
            }
        }
        #endregion

        #region 报警设置是否显示
        private bool _isShowBJSZ;

        public bool IsShowBJSZ
        {
            get { return _isShowBJSZ; }
            set
            {
                if (_isShowBJSZ != value)
                {
                    _isShowBJSZ = value;
                    this.RaisePropertyChanged(() => this.IsShowBJSZ);

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
            Wj1090TreeSetLoad.Myself.IsShowGrpInTreeModelShowId = this.IsShowGrpInTreeModelShowId;
            Wj1090TreeSetLoad.Myself.IsShowTreeOnTab = this.IsShowTreeOnTab;

            Wj1090TreeSetLoad.Myself.IsShowArea = this.IsShowArea;
            Wj1090TreeSetLoad.Myself.IsShowGrp = this.IsShowGrp;
            Wj1090TreeSetLoad.Myself.IsShowFid = this.IsShowFid;

            Wj1090TreeSetLoad.Myself.IsShowCJSJ = this.IsShowCJSJ;
            Wj1090TreeSetLoad.Myself.IsShowZT = this.IsShowZT;
            Wj1090TreeSetLoad.Myself.IsShowYGGL = this.IsShowYGGL;
            Wj1090TreeSetLoad.Myself.IsShowWGGL = this.IsShowWGGL;
            Wj1090TreeSetLoad.Myself.IsShowGLYS = this.IsShowGLYS;
            Wj1090TreeSetLoad.Myself.IsShowLDL = this.IsShowLDL;
            Wj1090TreeSetLoad.Myself.IsShowXHQD = this.IsShowXHQD;
            Wj1090TreeSetLoad.Myself.IsShowHLZK = this.IsShowHLZK;
            Wj1090TreeSetLoad.Myself.IsShowYYXH = this.IsShowYYXH;
            Wj1090TreeSetLoad.Myself.IsShowXHZS = this.IsShowXHZS;
            Wj1090TreeSetLoad.Myself.IsShowBJSZ = this.IsShowBJSZ;

            Wj1090TreeSetLoad.Myself.SavConfig();
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

    public class Wj1090TreeSetLoad
    {
        public bool IsShowGrpInTreeModelShowId;
        public bool IsShowTreeOnTab;

        public bool IsShowArea;
        public bool IsShowGrp;
        public bool IsShowFid;

        public bool IsShowCJSJ;
        public bool IsShowZT;
        public bool IsShowYGGL;
        public bool IsShowWGGL;
        public bool IsShowGLYS;
        public bool IsShowLDL;
        public bool IsShowXHQD;
        public bool IsShowHLZK;
        public bool IsShowYYXH;
        public bool IsShowXHZS;
        public bool IsShowBJSZ;

        static Wj1090TreeSetLoad myself;
        public static Wj1090TreeSetLoad Myself
        {
            get
            {
                if (myself == null) new Wj1090TreeSetLoad();
                return myself;
            }
        }
 
        public const string XmlConfigName = "Wj1090SetConfg";

        private Wj1090TreeSetLoad()
        {

            if (myself == null) myself = this;

            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
            if (info.ContainsKey("IsShowGrpInTreeModelShowId"))
            {
                IsShowGrpInTreeModelShowId = info["IsShowGrpInTreeModelShowId"].Contains("yes");
            }
            else IsShowGrpInTreeModelShowId = true;

            //if (info.ContainsKey("IsOnSelectNodeNavToParsSetView"))
            //{
            //    IsOnSelectNodeNavToParsSetView = info["IsOnSelectNodeNavToParsSetView"].Contains("yes");
            //}
            //else IsOnSelectNodeNavToParsSetView = true;

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





            if (info.ContainsKey("IsShowCJSJ"))
            {
                IsShowCJSJ = info["IsShowCJSJ"].Contains("yes");
            }
            else IsShowCJSJ = false;

            if (info.ContainsKey("IsShowZT"))
            {
                IsShowZT = info["IsShowZT"].Contains("yes");
            }
            else IsShowZT = false;

            if (info.ContainsKey("IsShowYGGL"))
            {
                IsShowYGGL = info["IsShowYGGL"].Contains("yes");
            }
            else IsShowYGGL = false;

            if (info.ContainsKey("IsShowWGGL"))
            {
                IsShowWGGL = info["IsShowWGGL"].Contains("yes");
            }
            else IsShowWGGL = false;

            if (info.ContainsKey("IsShowGLYS"))
            {
                IsShowGLYS = info["IsShowGLYS"].Contains("yes");
            }
            else IsShowGLYS = false;

            if (info.ContainsKey("IsShowLDL"))
            {
                IsShowLDL = info["IsShowLDL"].Contains("yes");
            }
            else IsShowLDL = false;

            if (info.ContainsKey("IsShowXHQD"))
            {
                IsShowXHQD = info["IsShowXHQD"].Contains("yes");
            }
            else IsShowXHQD = false;

            if (info.ContainsKey("IsShowHLZK"))
            {
                IsShowHLZK = info["IsShowHLZK"].Contains("yes");
            }
            else IsShowHLZK = false;

            if (info.ContainsKey("IsShowYYXH"))
            {
                IsShowYYXH = info["IsShowYYXH"].Contains("yes");
            }
            else IsShowYYXH = false;

            if (info.ContainsKey("IsShowXHZS"))
            {
                IsShowXHZS = info["IsShowXHZS"].Contains("yes");
            }
            else IsShowXHZS = false;

            if (info.ContainsKey("IsShowBJSZ"))
            {
                IsShowBJSZ = info["IsShowBJSZ"].Contains("yes");
            }
            else IsShowBJSZ = false;
        }




        public void SavConfig()
        {
            var info = new Dictionary<string, string>();
            if (IsShowGrpInTreeModelShowId) info.Add("IsShowGrpInTreeModelShowId", "yes");
            else info.Add("IsShowGrpInTreeModelShowId", "no");

            //if (IsOnSelectNodeNavToParsSetView) info.Add("IsOnSelectNodeNavToParsSetView", "yes");
            //else info.Add("IsOnSelectNodeNavToParsSetView", "no");

            if (IsShowTreeOnTab) info.Add("IsShowTreeOnTab", "yes");
            else info.Add("IsShowTreeOnTab", "no");

            if (IsShowArea) info.Add("IsShowArea", "yes");
            else info.Add("IsShowArea", "no");

            if (IsShowGrp) info.Add("IsShowGrp", "yes");
            else info.Add("IsShowGrp", "no");

            if (IsShowFid) info.Add("IsShowFid", "yes");
            else info.Add("IsShowFid", "no");


            if (IsShowCJSJ) info.Add("IsShowCJSJ", "yes");
            else info.Add("IsShowCJSJ", "no");

            if (IsShowZT) info.Add("IsShowZT", "yes");
            else info.Add("IsShowZT", "no");

            if (IsShowYGGL) info.Add("IsShowYGGL", "yes");
            else info.Add("IsShowYGGL", "no");

            if (IsShowWGGL) info.Add("IsShowWGGL", "yes");
            else info.Add("IsShowWGGL", "no");

            if (IsShowGLYS) info.Add("IsShowGLYS", "yes");
            else info.Add("IsShowGLYS", "no");

            if (IsShowLDL) info.Add("IsShowLDL", "yes");
            else info.Add("IsShowLDL", "no");

            if (IsShowXHQD) info.Add("IsShowXHQD", "yes");
            else info.Add("IsShowXHQD", "no");

            if (IsShowHLZK) info.Add("IsShowHLZK", "yes");
            else info.Add("IsShowHLZK", "no");

            if (IsShowYYXH) info.Add("IsShowYYXH", "yes");
            else info.Add("IsShowYYXH", "no");

            if (IsShowXHZS) info.Add("IsShowXHZS", "yes");
            else info.Add("IsShowXHZS", "no");

            if (IsShowBJSZ) info.Add("IsShowBJSZ", "yes");
            else info.Add("IsShowBJSZ", "no");

            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, XmlConfigName);



            //RegionManage.ShowViewByIdAttachRegion(
            //    Ux.Wj1090Module.Services.ViewIdAssign.LduTreeInfoViewId,
            //    Ux.Wj1090Module.Services.ViewIdAssign.LduTreeInfoViewAttachRegion,
            //    IsShowTreeOnTab);
            RegionManage.ShowViewByIdAttachRegion(
           Ux.Wj1090Module.Services.ViewIdAssign.Wj1090ManageViewId,
   
           IsShowTreeOnTab);
        }

    };

}
