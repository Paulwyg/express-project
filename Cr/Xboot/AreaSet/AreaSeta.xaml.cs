using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.CommandCore;

namespace Xboot.AreaSet
{
    /// <summary>
    /// AreaSeta.xaml 的交互逻辑
    /// </summary>
    //[ViewExport(
    //AttachNow = false,
    //AttachRegion = Wlst.Cr.CoreOne.CoreIdAssign.ViewIdAssgin.MainViewSettingViewAttachRegion,
    //ID = Wlst.Cr.CoreOne.CoreIdAssign.ViewIdAssgin.MainViewSettingViewId)]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public partial class AreaSeta
    {
        public AreaSeta()
        {
            InitializeComponent();

            this.DataContext = this;

            OnLoad();
        }

        private void OnLoad()
        {
            this.Color1 = AreaSet.AreaSets.MySelf.Color1;
            this.Color2 = AreaSet.AreaSets.MySelf.Color2;
            this.Color3 = AreaSet.AreaSets.MySelf.Color3;
            this.Color4 = AreaSet.AreaSets.MySelf.Color4;
            this.Color5 = AreaSet.AreaSets.MySelf.Color5;
            ColorBottom = AreaSet.AreaSets.MySelf.ColorBottom;

            this.TreeIsHide = AreaSet.AreaSets.MySelf.TreeIsHide;
            this.DataIsHide = AreaSet.AreaSets.MySelf.DataIsHide;
            this.MsgIsHide = AreaSet.AreaSets.MySelf.MsgIsHide;
            this.DataArea = AreaSet.AreaSets.MySelf.DataArea;
            this.MsgArea = AreaSet.AreaSets.MySelf.MsgArea;
            this.Area1Wide = AreaSet.AreaSets.MySelf.Area1Wide;
            this.Area35Wide = AreaSet.AreaSets.MySelf.Area35Wide;
            this.Area45Height = AreaSet.AreaSets.MySelf.Area45Height;
            this.MainArea = AreaSet.AreaSets.MySelf.MainArea;
        }

        private void label7_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                GC.Collect();
            }
            catch (Exception ex)
            {
                
            }
        }

        private void radioButton111_Checked(object sender, RoutedEventArgs e)
        {

        }
    }

    public partial class AreaSeta : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public partial class AreaSeta
    {

        #region Color1

        private string _Color1;

        public string Color1
        {
            get { return _Color1; }
            set
            {
                if (_Color1 != value)
                {
                    _Color1 = value;
                    this.OnPropertyChanged("Color1");
                }
            }
        }


        #endregion

        #region Color2

        private string _Color2;

        public string Color2
        {
            get { return _Color2; }
            set
            {
                if (_Color2 != value)
                {
                    _Color2 = value;
                    this.OnPropertyChanged("Color2");
                }
            }
        }


        #endregion

        #region Color3

        private string _Color3;

        public string Color3
        {
            get { return _Color3; }
            set
            {
                if (_Color3 != value)
                {
                    _Color3 = value;
                    this.OnPropertyChanged("Color3");
                }
            }
        }


        #endregion

        #region Color4

        private string _Color4;

        public string Color4
        {
            get { return _Color4; }
            set
            {
                if (_Color4 != value)
                {
                    _Color4 = value;
                    this.OnPropertyChanged("Color4");
                }
            }
        }


        #endregion

        #region Color5

        private string _Color5;

        public string Color5
        {
            get { return _Color5; }
            set
            {
                if (_Color5 != value)
                {
                    _Color5 = value;
                    this.OnPropertyChanged("Color5");
                }
            }
        }


        #endregion

        #region ColorBottom
        private string _colorBottom;

        public string ColorBottom
        {
            get { return _colorBottom; }
            set
            {
                if (_colorBottom != value)
                {
                    _colorBottom = value;
                    OnPropertyChanged("ColorBottom");
                }
            }
        }
        #endregion

        #region TreeIsHide

        private bool _TreeIsHide;

        public bool TreeIsHide
        {
            get { return _TreeIsHide; }
            set
            {
                if (_TreeIsHide != value)
                {
                    _TreeIsHide = value;
                    this.OnPropertyChanged("TreeIsHide");
                }
            }
        }


        #endregion

        #region DataIsHide

        private bool _DataIsHide;

        public bool DataIsHide
        {
            get { return _DataIsHide; }
            set
            {
                if (_DataIsHide != value)
                {
                    _DataIsHide = value;
                    this.OnPropertyChanged("DataIsHide");
                }
            }
        }


        #endregion

        #region MsgIsHide

        private bool _MsgIsHide;

        public bool MsgIsHide
        {
            get { return _MsgIsHide; }
            set
            {
                if (_MsgIsHide != value)
                {
                    _MsgIsHide = value;
                    this.OnPropertyChanged("MsgIsHide");
                }
            }
        }


        #endregion

        #region DataArea 4 5 45

        private int _DataArea;

        public int DataArea
        {
            get { return _DataArea; }
            set
            {
                if (_DataArea != value)
                {
                    _DataArea = value;
                    this.OnPropertyChanged("DataArea");
                }
            }
        }


        #endregion

        #region MsgArea 3 4 5 35 45

        private int _MsgArea;

        public int MsgArea
        {
            get { return _MsgArea; }
            set
            {
                if (_MsgArea != value)
                {
                    _MsgArea = value;
                    this.OnPropertyChanged("MsgArea");
                }
            }
        }


        #endregion

        #region MainArea 2 23 2345

        private int _mainArea;

        public int MainArea
        {
            get { return _mainArea; }
            set
            {
                if (_mainArea != value)
                {
                    _mainArea = value;
                    this.OnPropertyChanged("MainArea");
                }
            }
        }


        #endregion

        #region Area1Wide

        private int _Area1Wide;

        public int Area1Wide
        {
            get { return _Area1Wide; }
            set
            {
                if (_Area1Wide != value)
                {
                    if (value < 0) value = 0;
                    if (value > 500) value = 500;
                    _Area1Wide = value;
                    this.OnPropertyChanged("Area1Wide");
                }
            }
        }


        #endregion

        #region Area45Height

        private int _Area45Height;

        public int Area45Height
        {
            get { return _Area45Height; }
            set
            {
                if (_Area45Height != value)
                {
                    if (value < 100) value = 100;
                    if (value > 950) value = 950;
                    _Area45Height = value;
                    this.OnPropertyChanged("Area45Height");
                }
            }
        }


        #endregion

        #region Area35Wide

        private int _Area35Wide;

        public int Area35Wide
        {
            get { return _Area35Wide; }
            set
            {
                if (_Area35Wide != value)
                {
                    if (value < 100) value = 100;
                    if (value > 950) value = 950;
                    _Area35Wide = value;
                    this.OnPropertyChanged("Area35Wide");
                }
            }
        }


        #endregion

        #region CmdApply

        private ICommand _CmdApply;

        public ICommand CmdApply
        {
            get
            {

                if (_CmdApply == null)
                {
                    _CmdApply = new CommandRelay(ExCmdApply);
                }
                return _CmdApply;
            }
        }

        private void ExCmdApply()
        {

            AreaSet.AreaSets.MySelf.Color1 = this.Color1;
            AreaSet.AreaSets.MySelf.Color2 = this.Color2;
            AreaSet.AreaSets.MySelf.Color3 = this.Color3;
            AreaSet.AreaSets.MySelf.Color4 = this.Color4;
            AreaSet.AreaSets.MySelf.Color5 = this.Color5;
            AreaSet.AreaSets.MySelf.ColorBottom = this.ColorBottom;
            AreaSet.AreaSets.MySelf.TreeIsHide = this.TreeIsHide;
            AreaSet.AreaSets.MySelf.DataIsHide = this.DataIsHide;
            AreaSet.AreaSets.MySelf.MsgIsHide = this.MsgIsHide;
            AreaSet.AreaSets.MySelf.DataArea = this.DataArea;
            AreaSet.AreaSets.MySelf.MsgArea = this.MsgArea;
            AreaSet.AreaSets.MySelf.Area1Wide = this.Area1Wide;
            AreaSet.AreaSets.MySelf.Area35Wide = this.Area35Wide;
            AreaSet.AreaSets.MySelf.Area45Height = this.Area45Height;
            AreaSet.AreaSets.MySelf.MainArea = this.MainArea;

            AreaSet.AreaSets.MySelf.UpdateCurrentSet();
            AreaSet.AreaSets.MySelf.Save();
        }

        #endregion
    }
}
