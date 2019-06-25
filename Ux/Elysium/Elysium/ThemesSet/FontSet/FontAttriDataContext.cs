using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.FontSet
{
    public partial class FontAttriDataContext : INotifyPropertyChanged
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

    //Notify
    public partial class FontAttriDataContext
    { 
        #region RowHeadHeightt
        private double  _myFonRowHeadHeighttRowHeightttSize;

        public double  RowHeadHeightt
        {
            get { return _myFonRowHeadHeighttRowHeightttSize; }
            set
            {
                if (_myFonRowHeadHeighttRowHeightttSize.Equals(value)) return;
                _myFonRowHeadHeighttRowHeightttSize = value;
                OnPropertyChanged("RowHeadHeightt");
            }
        }
        #endregion

        #region RowHeightt
        private double  _myFonRowHeightttSize;

        public double  RowHeightt
        {
            get { return _myFonRowHeightttSize; }
            set
            {
                if (_myFonRowHeightttSize.Equals(value)) return;
                _myFonRowHeightttSize = value;
                OnPropertyChanged("RowHeightt");
            }
        }
        #endregion

        #region RowHeightTree
        private double _myFonRowHeightTree;

        public double RowHeightTree
        {
            get { return _myFonRowHeightTree; }
            set
            {
                if (_myFonRowHeightTree.Equals(value)) return;
                _myFonRowHeightTree = value;
                OnPropertyChanged("RowHeightTree");
            }
        }
        #endregion

        #region MyFontSize
        private double _myFontSize;

        public double MyFontSize
        {
            get { return _myFontSize; }
            set
            {
                if (_myFontSize.Equals(value)) return;
                _myFontSize = value;
                OnPropertyChanged("MyFontSize");
            }
        }
        #endregion

        #region MyFontWeight
        private string _myFontWeight;

        public string MyFontWeight
        {
            get { return _myFontWeight; }
            set
            {
                if (_myFontWeight==value) return;
                _myFontWeight = value;
                OnPropertyChanged("MyFontWeight");
            }
        }
        #endregion

        #region MyFontStyle
        private string _myFontStyle;

        public string MyFontStyle
        {
            get { return _myFontStyle; }
            set
            {
                if (_myFontStyle == value) return;
                _myFontStyle = value;
                OnPropertyChanged("MyFontStyle");
            }
        }
        #endregion

        #region MyFontStretch
        private string _myFontStretch;

        public string MyFontStretch
        {
            get { return _myFontStretch; }
            set
            {
                if (_myFontStretch == value) return;
                _myFontStretch = value;
                OnPropertyChanged("MyFontStretch");
            }
        }
        #endregion

        #region MyFontFamily
        private string _myFontFamily;

        public string MyFontFamily
        {
            get { return _myFontFamily; }
            set
            {
                if (_myFontFamily == value) return;
                _myFontFamily = value;
                OnPropertyChanged("MyFontFamily");
            }
        }
        #endregion

        #region Remind

        private string _remind;
        public string Remind
        {
            get { return _remind; }
            set
            {
                if(_remind==value) return;
                _remind = value;
                OnPropertyChanged("Remind");
            }
        }

        #endregion
    }

    public partial class FontAttriDataContext
    {
        private DependencyObject obj;
        public FontAttriDataContext(DependencyObject font)
        {
            obj = font;
            RowHeadHeightt = FontAttriXaml.RowHeadHeightt;
            RowHeightt = FontAttriXaml.RowHeightt;
            RowHeightTree = FontAttriXaml.RowHeightTree; 

            MyFontSize = FontAttriXaml.MyFontSize;
            MyFontWeight = FontAttriXaml.MyFontWeight.ToString();


            MyFontStyle = FontAttriXaml.MyFontStyle.ToString();
            MyFontStretch = FontAttriXaml.MyFontStretch.ToString();
            MyFontFamily = FontAttriXaml.MyFontFamily.ToString();
        }
        #region save
        private ICommand _cmdSave;

        public ICommand CmdSave
        {
            get { return _cmdSave ?? (_cmdSave = new CommandRelay(Ex)); }
        }

        private void Ex()
        {
            if (MyFontSize < 10 || MyFontSize > 18)
            {
                Remind = "字体大小需在10-18号之间,请重新输入字体大小";
                MyFontSize = 12;
                return;
            }
            Remind = string.Empty;
            FontAttriXaml.RowHeadHeightt = RowHeadHeightt;
            FontAttriXaml.RowHeightt = RowHeightt;
            FontAttriXaml.RowHeightTree = RowHeightTree;

            FontAttriXaml.MyFontSize = MyFontSize;
            FontAttriXaml.MyFontWeight =FontAttriXaml.GetFontWeight(MyFontWeight);
            FontAttriXaml.MyFontStyle = FontAttriXaml.GetFontStyle(MyFontStyle);
            FontAttriXaml.MyFontStretch = FontAttriXaml.GetFontStretch(MyFontStretch);
            FontAttriXaml.MyFontFamily = new FontFamily(MyFontFamily);
            FontAttriXaml.SaveStyle();
        }

        #endregion


        #region CmdLook
        private ICommand _CmdLook;

        public ICommand CmdLook
        {
            get { return _CmdLook ?? (_CmdLook = new CommandRelay(ExLoop)); }
        }

        private void ExLoop()
        {
            if (obj != null)
            {
                if(MyFontSize<10 || MyFontSize>25)
                {
                    Remind = "字体大小需在10-25号之间,请重新输入字体大小";
                    MyFontSize = 12;
                    return;
                }
                if(RowHeightt <10 ||RowHeightt >30)
                {
                    Remind = "表格行高大小需在10-30号之间,请重新输入表格行高";
                    RowHeightt = 12;
                    return;
                } if (RowHeadHeightt < 10 || RowHeadHeightt > 30)
                {
                    Remind = "表格行高大小需在10-30号之间,请重新输入表格行高";
                    RowHeadHeightt = 15;
                    return;
                }
                if (RowHeightTree < 10 || RowHeightTree > 30)
                {
                    Remind = "树控件行高大小需在10-30号之间,请重新输入树控件行高";
                    RowHeightTree = 15;
                    return;
                }


                Remind = string.Empty;
                FontAttriXaml.SetRowHeadHeightt(obj, RowHeadHeightt);
                FontAttriXaml .SetRowHeightt(obj ,RowHeightt  );
                FontAttriXaml.SetMyFontSize(obj, MyFontSize);
                var tmp = FontAttriXaml.GetFontWeight(MyFontWeight);
                if(tmp!=null)
                FontAttriXaml.SetMyFontWeight(obj,tmp);

                var tmpstyle = FontAttriXaml.GetFontStyle(MyFontStyle);
                if (tmpstyle != null)
                    FontAttriXaml.SetMyFontStyle(obj, tmpstyle);

                var tmpStretch = FontAttriXaml.GetFontStretch(MyFontStretch);
                if (tmpStretch != null)
                    FontAttriXaml.SetMyFontStretch(obj, tmpStretch);
                FontAttriXaml.SetMyFontFamily(obj,new FontFamily(MyFontFamily));
            }

        }
        #endregion

    }
}
