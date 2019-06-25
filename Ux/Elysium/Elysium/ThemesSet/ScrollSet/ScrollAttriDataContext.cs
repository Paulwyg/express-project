using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.ScrollSet
{
    public partial class ScrollAttriDataContext : INotifyPropertyChanged
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
    public partial class ScrollAttriDataContext
    {

        #region MyHeight

        private double _height;

        public double MyHeight
        {
            get { return _height; }
            set
            {
                if (_height == value) return;
                _height = value;
                this.OnPropertyChanged("MyHeight");
            }
        }


        #endregion

        #region MyWidth

        private double _width;

        public double MyWidth
        {
            get { return _width; }
            set
            {
                if (_width == value) return;
                _width = value;
                this.OnPropertyChanged("MyWidth");
            }
        }


        #endregion

        #region NormalBackground

        private string _normalBackground;

        public string NormalBackground
        {
            get { return _normalBackground; }
            set
            {
                if (_normalBackground != value)
                {
                    _normalBackground = value;
                    this.OnPropertyChanged("NormalBackground");
                }
            }
        }


        #endregion

        #region NormalBorderBrush

        private string _normalBorderBrush;

        public string NormalBorderBrush
        {
            get { return _normalBorderBrush; }
            set
            {
                if (_normalBorderBrush != value)
                {
                    _normalBorderBrush = value;
                    this.OnPropertyChanged("NormalBorderBrush");
                }
            }
        }


        #endregion




        #region MouseOverBackgroundBrush

        private string _mouseOverBackgroundBrush;

        public string MouseOverBackgroundBrush
        {
            get { return _mouseOverBackgroundBrush; }
            set
            {
                if (_mouseOverBackgroundBrush != value)
                {
                    _mouseOverBackgroundBrush = value;
                    this.OnPropertyChanged("MouseOverBackgroundBrush");
                }
            }
        }


        #endregion

        #region MouseOverBorderBrush

        private string _mouseOverBorderBrush;

        public string MouseOverBorderBrush
        {
            get { return _mouseOverBorderBrush; }
            set
            {
                if (_mouseOverBorderBrush != value)
                {
                    _mouseOverBorderBrush = value;
                    this.OnPropertyChanged("MouseOverBorderBrush");
                }
            }
        }


        #endregion



        #region PressedBorderBrush

        private string _pressedBorderBrush;

        public string PressedBorderBrush
        {
            get { return _pressedBorderBrush; }
            set
            {
                if (_pressedBorderBrush != value)
                {
                    _pressedBorderBrush = value;
                    this.OnPropertyChanged("PressedBorderBrush");
                }
            }
        }


        #endregion

        #region PressedBackgroundBrush

        private string _pressedBackgroundBrush;

        public string PressedBackgroundBrush
        {
            get { return _pressedBackgroundBrush; }
            set
            {
                if (_pressedBackgroundBrush != value)
                {
                    _pressedBackgroundBrush = value;
                    this.OnPropertyChanged("PressedBackgroundBrush");
                }
            }
        }


        #endregion



        #region DisableBackgroundBrush

        private string _disableBackgroundBrush;

        public string DisableBackgroundBrush
        {
            get { return _disableBackgroundBrush; }
            set
            {
                if (_disableBackgroundBrush != value)
                {
                    _disableBackgroundBrush = value;
                    this.OnPropertyChanged("DisableBackgroundBrush");
                }
            }
        }


        #endregion

        #region DisableBorderBrush

        private string _disableBorderBrush;

        public string DisableBorderBrush
        {
            get { return _disableBorderBrush; }
            set
            {
                if (_disableBorderBrush != value)
                {
                    _disableBorderBrush = value;
                    this.OnPropertyChanged("DisableBorderBrush");
                }
            }
        }


        #endregion

    }

    public partial class ScrollAttriDataContext
    {
        private DependencyObject obj;
        public ScrollAttriDataContext(DependencyObject ScollBar)
        {
            obj = ScollBar;
            this.NormalBackground = ScrollAttriXaml.NormalBackgrounBrush.Color.ToString();
            this.NormalBorderBrush = ScrollAttriXaml.NormalBorderBrush.Color.ToString(); //.Left ;


            this.MouseOverBackgroundBrush = ScrollAttriXaml.MouseOverBackgroundBrush.Color.ToString();
            this.MouseOverBorderBrush = ScrollAttriXaml.MouseOverBorderBrush.Color.ToString();

            this.PressedBackgroundBrush = ScrollAttriXaml.PressedBackgroundBrush.Color.ToString();
            this.PressedBorderBrush = ScrollAttriXaml.PressedBorderBrush.Color.ToString();

            this.DisableBackgroundBrush = ScrollAttriXaml.DisableBackgroundBrush.Color.ToString();
            this.DisableBorderBrush = ScrollAttriXaml.DisableBorderBrush.Color.ToString();

            this.MyHeight = ScrollAttriXaml.MyHeight;
            MyWidth = ScrollAttriXaml.MyWidth;

        }
        #region save
        private ICommand _cmdSave;

        public ICommand CmdSave
        {
            get
            {

                if (_cmdSave == null)
                {
                    _cmdSave = new CommandRelay(Ex);
                }
                return _cmdSave;
            }
        }

        private void Ex()
        {
            var tmp = ColorConverter.ConvertFromString(this.NormalBackground);
            if (tmp != null)
                ScrollAttriXaml.NormalBackgrounBrush = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
            if (tmp != null)
                ScrollAttriXaml.NormalBorderBrush = new SolidColorBrush((Color)tmp);


            tmp = ColorConverter.ConvertFromString(this.PressedBackgroundBrush);
            if (tmp != null)
                ScrollAttriXaml.PressedBackgroundBrush = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(this.PressedBorderBrush);
            if (tmp != null)
                ScrollAttriXaml.PressedBorderBrush = new SolidColorBrush((Color)tmp);


            tmp = ColorConverter.ConvertFromString(this.MouseOverBackgroundBrush);
            if (tmp != null)
                ScrollAttriXaml.MouseOverBackgroundBrush = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(this.MouseOverBorderBrush);
            if (tmp != null)
                ScrollAttriXaml.MouseOverBorderBrush = new SolidColorBrush((Color)tmp);


            tmp = ColorConverter.ConvertFromString(this.DisableBackgroundBrush);
            if (tmp != null)
                ScrollAttriXaml.DisableBackgroundBrush = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
            if (tmp != null)
                ScrollAttriXaml.DisableBorderBrush = new SolidColorBrush((Color)tmp);


            ScrollAttriXaml.MyHeight = MyHeight;
            ScrollAttriXaml.MyWidth = MyWidth;

            ScrollAttriXaml.SaveStyle();
        }

        #endregion


        #region CmdLook
        private ICommand _CmdLook;

        public ICommand CmdLook
        {
            get
            {

                if (_CmdLook == null)
                {
                    _CmdLook = new CommandRelay(ExLoop);
                }
                return _CmdLook;
            }
        }

        private void ExLoop()
        {
            if (obj != null)
            {

                var tmp = ColorConverter.ConvertFromString(this.NormalBackground);
                if (tmp != null)
                    ScrollAttriXaml.SetNormalBackgrounBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    ScrollAttriXaml.SetNormalBorderBrush(obj, new SolidColorBrush((Color)tmp));



                tmp = ColorConverter.ConvertFromString(this.PressedBackgroundBrush);
                if (tmp != null)
                    ScrollAttriXaml.SetPressedBackgroundBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.PressedBorderBrush);
                if (tmp != null)
                    ScrollAttriXaml.SetPressedBorderBrush(obj, new SolidColorBrush((Color)tmp));

                

                tmp = ColorConverter.ConvertFromString(this.MouseOverBackgroundBrush);
                if (tmp != null)
                    ScrollAttriXaml.SetMouseOverBackgroundBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverBorderBrush);
                if (tmp != null)
                    ScrollAttriXaml.SetMouseOverBorderBrush(obj, new SolidColorBrush((Color)tmp));



                tmp = ColorConverter.ConvertFromString(this.DisableBackgroundBrush);
                if (tmp != null)
                    ScrollAttriXaml.SetDisableBackgroundBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
                if (tmp != null)
                    ScrollAttriXaml.SetDisableBorderBrush(obj, new SolidColorBrush((Color)tmp));

                ScrollAttriXaml.SetMyHeight(obj, MyHeight);
                ScrollAttriXaml.SetMyWidth(obj, MyWidth);

            }
        }
        #endregion

    }
}
