using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.ButtonSet
{
    public partial class ButtonAttriDataContext : INotifyPropertyChanged
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
    public partial class ButtonAttriDataContext
    {

        #region BorderThickness

        private double  _borderThickness;

        public double  BorderThickness
        {
            get { return _borderThickness; }
            set
            {
                if (_borderThickness != value)
                {
                    _borderThickness = value;
                    this.OnPropertyChanged("BorderThickness");
                }
            }
        }


        #endregion


        #region CornerRadiusValue

        private double _cornerRadiusValue;

        public double CornerRadiusValue
        {
            get { return _cornerRadiusValue; }
            set
            {
                if (_cornerRadiusValue == value) return;
                _cornerRadiusValue = value;
                this.OnPropertyChanged("CornerRadiusValue");
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

        #region NormalForeground

        private string _normalForeground;

        public string NormalForeground
        {
            get { return _normalForeground; }
            set
            {
                if (_normalForeground != value)
                {
                    _normalForeground = value;
                    this.OnPropertyChanged("NormalForeground");
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

        #region MouseOverForegroundBrush

        private string _mouseOverForegroundBrush;

        public string MouseOverForegroundBrush
        {
            get { return _mouseOverForegroundBrush; }
            set
            {
                if (_mouseOverForegroundBrush != value)
                {
                    _mouseOverForegroundBrush = value;
                    this.OnPropertyChanged("MouseOverForegroundBrush");
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

        #region PressedForegroundBrush

        private string _pressedForegroundBrush;

        public string PressedForegroundBrush
        {
            get { return _pressedForegroundBrush; }
            set
            {
                if (_pressedForegroundBrush != value)
                {
                    _pressedForegroundBrush = value;
                    this.OnPropertyChanged("PressedForegroundBrush");
                }
            }
        }


        #endregion


        #region DisableForegroundBrush

        private string _disableForegroundBrush;

        public string DisableForegroundBrush
        {
            get { return _disableForegroundBrush; }
            set
            {
                if (_disableForegroundBrush != value)
                {
                    _disableForegroundBrush = value;
                    this.OnPropertyChanged("DisableForegroundBrush");
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

    public partial class ButtonAttriDataContext
    {
        private DependencyObject obj;
        public ButtonAttriDataContext(DependencyObject button)
        {
            obj = button;
            this.NormalBackground = ButtonAttriXaml.NormalBackgrounBrush.Color.ToString();
            this.NormalBorderBrush = ButtonAttriXaml.NormalBorderBrush.Color.ToString(); //.Left ;
            this.NormalForeground = ButtonAttriXaml.NormalForegroundBrush.Color.ToString();


            this.MouseOverBackgroundBrush = ButtonAttriXaml.MouseOverBackgroundBrush.Color.ToString();
            this.MouseOverBorderBrush = ButtonAttriXaml.MouseOverBorderBrush.Color.ToString();
            this.MouseOverForegroundBrush = ButtonAttriXaml.MouseOverForegroundBrush.Color.ToString();

            this.PressedBackgroundBrush = ButtonAttriXaml.PressedBackgroundBrush.Color.ToString();
            this.PressedBorderBrush = ButtonAttriXaml.PressedBorderBrush.Color.ToString();
            this.PressedForegroundBrush = ButtonAttriXaml.PressedForegroundBrush.Color.ToString();

            this.DisableBackgroundBrush = ButtonAttriXaml.DisableBackgroundBrush.Color.ToString();
            this.DisableBorderBrush = ButtonAttriXaml.DisableBorderBrush.Color.ToString();
            this.DisableForegroundBrush = ButtonAttriXaml.DisableForegroundBrush.Color.ToString();

            this.BorderThickness = ButtonAttriXaml.BorderThickness.Left ;
            this.CornerRadiusValue = ButtonAttriXaml.CornerRadiusValue;
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
                ButtonAttriXaml.NormalBackgrounBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
            if (tmp != null)
                ButtonAttriXaml.NormalBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.NormalForeground);
            if (tmp != null)
                ButtonAttriXaml.NormalForegroundBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.PressedBackgroundBrush);
            if (tmp != null)
                ButtonAttriXaml.PressedBackgroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.PressedBorderBrush);
            if (tmp != null)
                ButtonAttriXaml.PressedBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.PressedForegroundBrush);
            if (tmp != null)
                ButtonAttriXaml.PressedForegroundBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.MouseOverBackgroundBrush);
            if (tmp != null)
                ButtonAttriXaml.MouseOverBackgroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.MouseOverBorderBrush);
            if (tmp != null)
                ButtonAttriXaml.MouseOverBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.MouseOverForegroundBrush);
            if (tmp != null)
                ButtonAttriXaml.MouseOverForegroundBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.DisableBackgroundBrush);
            if (tmp != null)
                ButtonAttriXaml.DisableBackgroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
            if (tmp != null)
                ButtonAttriXaml.DisableBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.DisableForegroundBrush);
            if (tmp != null)
                ButtonAttriXaml.DisableForegroundBrush = new SolidColorBrush((Color) tmp);

            ButtonAttriXaml.BorderThickness =new Thickness(  this.BorderThickness);
            ButtonAttriXaml.CornerRadiusValue = this.CornerRadiusValue;
            
            ButtonAttriXaml.SaveStyle();
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
            if(obj !=null)
            {
                
                var tmp = ColorConverter.ConvertFromString(this.NormalBackground);
                if (tmp != null)
                    ButtonAttriXaml.SetNormalBackgrounBrush(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    ButtonAttriXaml.SetNormalBorderBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalForeground);
                if (tmp != null)
                    ButtonAttriXaml.SetNormalForegroundBrush(obj, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(this.PressedBackgroundBrush);
                if (tmp != null)
                    ButtonAttriXaml.SetPressedBackgroundBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.PressedBorderBrush);
                if (tmp != null)
                    ButtonAttriXaml.SetPressedBorderBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.PressedForegroundBrush);
                if (tmp != null)
                    ButtonAttriXaml.SetPressedForegroundBrush(obj, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(this.MouseOverBackgroundBrush);
                if (tmp != null)
                    ButtonAttriXaml.SetMouseOverBackgroundBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverBorderBrush);
                if (tmp != null)
                    ButtonAttriXaml.SetMouseOverBorderBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverForegroundBrush);
                if (tmp != null)
                    ButtonAttriXaml.SetMouseOverForegroundBrush(obj, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(this.DisableBackgroundBrush);
                if (tmp != null)
                    ButtonAttriXaml.SetDisableBackgroundBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
                if (tmp != null)
                    ButtonAttriXaml.SetDisableBorderBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableForegroundBrush);
                if (tmp != null)
                    ButtonAttriXaml.SetDisableForegroundBrush(obj, new SolidColorBrush((Color)tmp));

                ButtonAttriXaml.SetBorderThickness(obj,new Thickness(BorderThickness));
                ButtonAttriXaml.SetCornerRadiusValue(obj, CornerRadiusValue);
                
            }
        }
        #endregion 

    }

}
