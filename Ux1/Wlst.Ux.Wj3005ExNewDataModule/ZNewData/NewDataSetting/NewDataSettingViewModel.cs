using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Ux.Wj3005ExNewDataModule.ZNewData.TmlNewDataViewModel.ViewModel;

namespace Wlst.Ux.Wj3005ExNewDataModule.ZNewData.NewDataSetting
{
    [Export(typeof(IINewDataSetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewDataSettingViewModel : ObservableObject, IINewDataSetting
    {
        public NewDataSettingViewModel()
        {
            //this.NavOnLoad();

            this.RowHeight = TmlNewDataViewModel.ViewModel.NewDataViewModel.RowHeight;
            this.TimeNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.TimeNameLength;
            LoopNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.LoopNameLength;
            VaNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.VaNameLength;
            BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.BackgroundColor;
            K1BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K1BackgroundColor;
            K2BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K2BackgroundColor;
            K3BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K3BackgroundColor;
            K4BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K4BackgroundColor;
            K5BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K5BackgroundColor;
            K6BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K6BackgroundColor;
         //   ShowDw = true;
        }

        //public static int RowHeight = 25;
        //public static int LoopNameLength = 120;
        //public static int TimeNameLength = 120;
        //public static int VaNameLength = 80;


        #region  define

        private int _rowHeight;

        public int RowHeight
        {
            get { return _rowHeight; }
            set
            {
                if (value != _rowHeight)
                {
                    _rowHeight = value;
                    this.RaisePropertyChanged(() => this.RowHeight);
                }
            }
        }


        private int _loopNameLength;


        public int LoopNameLength
        {
            get { return _loopNameLength; }
            set
            {
                if (value != _loopNameLength)
                {
                    _loopNameLength = value;
                    this.RaisePropertyChanged(() => this.LoopNameLength);
                }
            }
        }


        private int _timeNameLength;

        public int TimeNameLength
        {
            get { return _timeNameLength; }
            set
            {
                if (value != _timeNameLength)
                {
                    _timeNameLength = value;
                    this.RaisePropertyChanged(() => this.TimeNameLength);
                }
            }
        }

        private int _vaNameLength;

        public int VaNameLength
        {
            get { return _vaNameLength; }
            set
            {
                if (value != _vaNameLength)
                {
                    _vaNameLength = value;
                    this.RaisePropertyChanged(() => this.VaNameLength);
                }
            }
        }


        private bool   _vasdgd;

        public bool IsShowLoopId
        {
            get { return _vasdgd; }
            set
            {
                if (value != _vasdgd)
                {
                    _vasdgd = value;
                    this.RaisePropertyChanged(() => this.IsShowLoopId);
                }
            }
        }


        private int _vasRtuNameLength;

        public int RtuNameLength
        {
            get { return _vasRtuNameLength; }
            set
            {
                if (value != _vasRtuNameLength)
                {
                    _vasRtuNameLength = value;
                    this.RaisePropertyChanged(() => this.RtuNameLength);
                }
            }
        }
        #region BackgroundColor
        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                if(value!=_backgroundColor)
                {
                    _backgroundColor = value;
                    RaisePropertyChanged(()=>this.BackgroundColor);
                }
            }
        }
        #endregion

        #region K1BackgroundColor
        private string _k1BackgroundColor;
        public string K1BackgroundColor
        {
            get { return _k1BackgroundColor; }
            set
            {
                if (value != _k1BackgroundColor)
                {
                    _k1BackgroundColor = value;
                    RaisePropertyChanged(() => K1BackgroundColor);
                }
            }
        }
        #endregion

        #region K2BackgroundColor
        private string _k2BackgroundColor;
        public string K2BackgroundColor
        {
            get { return _k2BackgroundColor; }
            set
            {
                if (value != _k2BackgroundColor)
                {
                    _k2BackgroundColor = value;
                    RaisePropertyChanged(() => this.K2BackgroundColor);
                }
            }
        }
        #endregion

        #region K3BackgroundColor
        private string _k3BackgroundColor;
        public string K3BackgroundColor
        {
            get { return _k3BackgroundColor; }
            set
            {
                if (value != _k3BackgroundColor)
                {
                    _k3BackgroundColor = value;
                    RaisePropertyChanged(() => K3BackgroundColor);
                }
            }
        }
        #endregion

        #region K4BackgroundColor
        private string _k4BackgroundColor;
        public string K4BackgroundColor
        {
            get { return _k4BackgroundColor; }
            set
            {
                if (value != _k4BackgroundColor)
                {
                    _k4BackgroundColor = value;
                    RaisePropertyChanged(() => K4BackgroundColor);
                }
            }
        }
        #endregion

        #region K5BackgroundColor
        private string _k5BackgroundColor;
        public string K5BackgroundColor
        {
            get { return _k5BackgroundColor; }
            set
            {
                if (value != _k5BackgroundColor)
                {
                    _k5BackgroundColor = value;
                    RaisePropertyChanged(() => this.K5BackgroundColor);
                }
            }
        }
        #endregion

        #region K6BackgroundColor
        private string _k6BackgroundColor;
        public string K6BackgroundColor
        {
            get { return _k6BackgroundColor; }
            set
            {
                if (value != _k6BackgroundColor)
                {
                    _k6BackgroundColor = value;
                    RaisePropertyChanged(() => K6BackgroundColor);
                }
            }
        }


        
        #endregion

                #region OnMeasureShowData
        private bool   _OnMeasureShowData;
        public bool OnMeasureShowData
        {
            get { return _OnMeasureShowData; }
            set
            {
                if (value != _OnMeasureShowData)
                {
                    _OnMeasureShowData = value;
                    RaisePropertyChanged(() => OnMeasureShowData);
                }
            }
        }
          private bool   _OnShowDw;
        public bool ShowDw
        {
            get { return _OnShowDw; }
            set
            {
                if (value != _OnShowDw)
                {
                    _OnShowDw = value;
                    RaisePropertyChanged(() => ShowDw);
                }
            }
        }
        
        
        #endregion
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
            TmlNewDataViewModel.ViewModel.NewDataViewModel.RowHeight = this.RowHeight;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.LoopNameLength = this.LoopNameLength;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.TimeNameLength = this.TimeNameLength;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.VaNameLength = this.VaNameLength;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.IsShowLoopId = this.IsShowLoopId;
             TmlNewDataViewModel.ViewModel.NewDataViewModel.RtuNameLength = this.RtuNameLength;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.BackgroundColor = this.BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.K1BackgroundColor = this.K1BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.K2BackgroundColor = this.K2BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.K3BackgroundColor = this.K3BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.K4BackgroundColor = this.K4BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.K5BackgroundColor = this.K5BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.K6BackgroundColor = this.K6BackgroundColor;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.OnMeasureShowData = this.OnMeasureShowData ;
            TmlNewDataViewModel.ViewModel.NewDataViewModel.ShowDw = this.ShowDw;
           // RtuLoopInfoVm._constColor = new string[] { BackgroundColor, K1BackgroundColor, K2BackgroundColor, K3BackgroundColor, K4BackgroundColor, K5BackgroundColor, K6BackgroundColor };
            NewDataViewModel.ConstColor = new string[] { BackgroundColor, K1BackgroundColor, K2BackgroundColor, K3BackgroundColor, K4BackgroundColor, K5BackgroundColor, K6BackgroundColor };
            this.SavConfig();

        }

        private bool CanEx()
        {
            if (
                this.RowHeight == TmlNewDataViewModel.ViewModel.NewDataViewModel.RowHeight &&
                this.TimeNameLength == TmlNewDataViewModel.ViewModel.NewDataViewModel.TimeNameLength &&
                this.LoopNameLength == TmlNewDataViewModel.ViewModel.NewDataViewModel.LoopNameLength &&
                this.VaNameLength == TmlNewDataViewModel.ViewModel.NewDataViewModel.VaNameLength &&
                this.RtuNameLength == TmlNewDataViewModel.ViewModel.NewDataViewModel.RtuNameLength &&
                this.IsShowLoopId == TmlNewDataViewModel.ViewModel.NewDataViewModel.IsShowLoopId &&
                this.BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.BackgroundColor &&
                this.K1BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.K1BackgroundColor &&
                this.K2BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.K2BackgroundColor &&
                this.K3BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.K3BackgroundColor &&
                this.K4BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.K4BackgroundColor &&
                this.K5BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.K5BackgroundColor &&
                this.K6BackgroundColor == TmlNewDataViewModel.ViewModel.NewDataViewModel.K6BackgroundColor &&
                this.OnMeasureShowData == TmlNewDataViewModel.ViewModel.NewDataViewModel.OnMeasureShowData &&
                this.ShowDw == TmlNewDataViewModel.ViewModel.NewDataViewModel.ShowDw 
                )
                return false;
            return DateTime.Now.Ticks - _dtApply.Ticks > 30000000;
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            this.RowHeight = TmlNewDataViewModel.ViewModel.NewDataViewModel.RowHeight;
            this.TimeNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.TimeNameLength;
            LoopNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.LoopNameLength;
            VaNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.VaNameLength;
            IsShowLoopId = TmlNewDataViewModel.ViewModel.NewDataViewModel.IsShowLoopId ;
            RtuNameLength = TmlNewDataViewModel.ViewModel.NewDataViewModel.RtuNameLength;
            BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.BackgroundColor;
            K1BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K1BackgroundColor;
            K2BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K2BackgroundColor;
            K3BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K3BackgroundColor;
            K4BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K4BackgroundColor;
            K5BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K5BackgroundColor;
            K6BackgroundColor = TmlNewDataViewModel.ViewModel.NewDataViewModel.K6BackgroundColor;
            OnMeasureShowData = TmlNewDataViewModel.ViewModel.NewDataViewModel.OnMeasureShowData ;
            ShowDw = TmlNewDataViewModel.ViewModel.NewDataViewModel.ShowDw;

        }



    }


    public partial class NewDataSettingViewModel
    {

        public const string XmlConfigName = "NewDataLenghtSetConfg";

        /// <summary>
        /// RowHeight LoopNameLength TimeNameLength VaNameLength
        /// </summary>
        /// <returns></returns>
        public static Tuple<int, int, int, int, int, bool, BackgroundSet> LoadNewDataLenghtSetConfg()
        {
            //public static int RowHeight = 25;
            //public static int LoopNameLength = 120;
            //public static int TimeNameLength = 120;
            //public static int VaNameLength = 80;

            int x1 = 0, x2 = 0, x3 = 0, x4 = 0, x5 = 0;
            int x6 = 0;
            int x7 = 0;
            string background = "",
                   k1background = "",
                   k2background = "",
                   k3background = "",
                   k4background = "",
                   k5background = "",
                   k6background = "";
            bool isshow = false;
            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
            if (info.ContainsKey("NewDataRowHeight"))
            {
                try
                {
                    x1 = Convert.ToInt32(info["NewDataRowHeight"]);
                }
                catch (Exception ex)
                {
                }
            }


            if (info.ContainsKey("NewDataLoopNameLength"))
            {
                try
                {
                    x2 = Convert.ToInt32(info["NewDataLoopNameLength"]);
                }
                catch (Exception ex)
                {
                }
            }


            if (info.ContainsKey("NewDataTimeNameLength"))
            {
                try
                {
                    x3 = Convert.ToInt32(info["NewDataTimeNameLength"]);
                }
                catch (Exception ex)
                {
                }
            }



            if (info.ContainsKey("NewDataVaNameLength"))
            {
                try
                {
                    x4 = Convert.ToInt32(info["NewDataVaNameLength"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("IsShowLoopId"))
            {
                try
                {
                    isshow = Convert.ToInt32(info["IsShowLoopId"]) == 1 ? true : false;
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("RtuNameLength"))
            {
                try
                {
                    x5 = Convert.ToInt32(info["RtuNameLength"]);
                }
                catch (Exception ex)
                {
                }
            }

            if (info.ContainsKey("OnMeasureShowData"))
            {
                try
                {
                    x6 = Convert.ToInt32(info["OnMeasureShowData"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("ShowDw"))
            {
                try
                {
                    x7 = Convert.ToInt32(info["ShowDw"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("BackgroundColor"))
            {
                try
                {
                    background = info["BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                background = "Black";
            }

            if (info.ContainsKey("K1BackgroundColor"))
            {
                try
                {
                    k1background = info["K1BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k1background = "Black";
            }
            if (info.ContainsKey("K2BackgroundColor"))
            {
                try
                {
                    k2background = info["K2BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k2background = "Black";
            }
            if (info.ContainsKey("K3BackgroundColor"))
            {
                try
                {
                    k3background = info["K3BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k3background = "Black";
            }
            if (info.ContainsKey("K4BackgroundColor"))
            {
                try
                {
                    k4background = info["K4BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k4background = "Black";
            }
            if (info.ContainsKey("K5BackgroundColor"))
            {
                try
                {
                    k5background = info["K5BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k5background = "Black";
            }
            if (info.ContainsKey("K6BackgroundColor"))
            {
                try
                {
                    k6background = info["K6BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k6background = "Black";
            }
            if (x1 < 15) x1 = 15;
            if (x2 < 60) x2 = 60;
            if (x3 < 60) x3 = 60;
            if (x4 < 60) x4 = 60;
            if (x5 < 250) x5 = 250;
            return new Tuple<int, int, int, int, int, bool, BackgroundSet>(x1, x2, x3, x4, x5, isshow,
                                                                                 new BackgroundSet()
                                                                                     {
                                                                                         Background = background,
                                                                                         K1Background = k1background,
                                                                                         K2Background = k2background,
                                                                                         K3Background = k3background,
                                                                                         K4Background = k4background,
                                                                                         K5Background = k5background,
                                                                                         K6Background = k6background,
                                                                                         OnMeasureShowData =x6 ==1,
                                                                                         ShowDw =x7 ==0
                                                                                     });

        }




        public void SavConfig()
        {
            var info = new Dictionary<string, string>();
            info.Add("NewDataRowHeight", this.RowHeight + "");
            info.Add("NewDataLoopNameLength", this.LoopNameLength + "");

            info.Add("NewDataTimeNameLength", this.TimeNameLength + "");
            info.Add("NewDataVaNameLength", this.VaNameLength + "");
            info.Add("RtuNameLength", this.RtuNameLength + "");
            info.Add("IsShowLoopId", (this.IsShowLoopId ? 1 : 0) + "");
            info.Add("BackgroundColor", BackgroundColor);
            info.Add("K1BackgroundColor", K1BackgroundColor);
            info.Add("K2BackgroundColor", K2BackgroundColor);
            info.Add("K3BackgroundColor", K3BackgroundColor);
            info.Add("K4BackgroundColor", K4BackgroundColor);
            info.Add("K5BackgroundColor", K5BackgroundColor);
            info.Add("K6BackgroundColor", K6BackgroundColor);

            info.Add("OnMeasureShowData",( OnMeasureShowData?1:0 )+ "");
            info.Add("ShowDw", (ShowDw ? 0: 1) + "");
            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, XmlConfigName);
        }

    }

    public class BackgroundSet
    {
        public string Background { get; set; }
        public string K1Background { get; set; }
        public string K2Background { get; set; }
        public string K3Background { get; set; }
        public string K4Background { get; set; }
        public string K5Background { get; set; }
        public string K6Background { get; set; }
        public bool OnMeasureShowData { get; set; }
        public bool ShowDw { get; set; }
    }
}
