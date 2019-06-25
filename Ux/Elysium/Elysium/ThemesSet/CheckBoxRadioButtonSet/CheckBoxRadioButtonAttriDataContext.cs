using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.ButtonSet;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.CheckBoxRadioButtonSet
{
    public partial class CheckBoxRadioButtonAttriDataContext : INotifyPropertyChanged
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
    public partial class CheckBoxRadioButtonAttriDataContext
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

    public partial class CheckBoxRadioButtonAttriDataContext
    {
        private DependencyObject checkBox;
        private DependencyObject radionButton;
        public CheckBoxRadioButtonAttriDataContext(DependencyObject checkBoxobj,DependencyObject radionButtonobj)
        {
            checkBox = checkBoxobj ;
            radionButton = radionButtonobj; 
            this.NormalBackground = CheckBoxRadioButtonXaml.NormalBackgrounBrush.Color.ToString();
            this.NormalBorderBrush = CheckBoxRadioButtonXaml.NormalBorderBrush.Color.ToString(); //.Left ;
            this.NormalForeground = CheckBoxRadioButtonXaml.NormalForegroundBrush.Color.ToString();


            this.MouseOverBackgroundBrush = CheckBoxRadioButtonXaml.MouseOverBackgroundBrush.Color.ToString();
            this.MouseOverBorderBrush = CheckBoxRadioButtonXaml.MouseOverBorderBrush.Color.ToString();
            this.MouseOverForegroundBrush = CheckBoxRadioButtonXaml.MouseOverForegroundBrush.Color.ToString();

            this.PressedBackgroundBrush = CheckBoxRadioButtonXaml.PressedBackgroundBrush.Color.ToString();
            this.PressedBorderBrush = CheckBoxRadioButtonXaml.PressedBorderBrush.Color.ToString();
            this.PressedForegroundBrush = CheckBoxRadioButtonXaml.PressedForegroundBrush.Color.ToString();

            this.DisableBackgroundBrush = CheckBoxRadioButtonXaml.DisableBackgroundBrush.Color.ToString();
            this.DisableBorderBrush = CheckBoxRadioButtonXaml.DisableBorderBrush.Color.ToString();
            this.DisableForegroundBrush = CheckBoxRadioButtonXaml.DisableForegroundBrush.Color.ToString();

            this.BorderThickness = CheckBoxRadioButtonXaml.BorderThickness.Left ;

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
                CheckBoxRadioButtonXaml.NormalBackgrounBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
            if (tmp != null)
                CheckBoxRadioButtonXaml.NormalBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.NormalForeground);
            if (tmp != null)
                CheckBoxRadioButtonXaml.NormalForegroundBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.PressedBackgroundBrush);
            if (tmp != null)
                CheckBoxRadioButtonXaml.PressedBackgroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.PressedBorderBrush);
            if (tmp != null)
                CheckBoxRadioButtonXaml.PressedBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.PressedForegroundBrush);
            if (tmp != null)
                CheckBoxRadioButtonXaml.PressedForegroundBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.MouseOverBackgroundBrush);
            if (tmp != null)
                CheckBoxRadioButtonXaml.MouseOverBackgroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.MouseOverBorderBrush);
            if (tmp != null)
                CheckBoxRadioButtonXaml.MouseOverBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.MouseOverForegroundBrush);
            if (tmp != null)
                CheckBoxRadioButtonXaml.MouseOverForegroundBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.DisableBackgroundBrush);
            if (tmp != null)
                CheckBoxRadioButtonXaml.DisableBackgroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
            if (tmp != null)
                CheckBoxRadioButtonXaml.DisableBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.DisableForegroundBrush);
            if (tmp != null)
                CheckBoxRadioButtonXaml.DisableForegroundBrush = new SolidColorBrush((Color) tmp);

            CheckBoxRadioButtonXaml.BorderThickness =new Thickness(  this.BorderThickness);

            CheckBoxRadioButtonXaml.SaveStyle();
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
            if(checkBox  !=null)
            {
                
                var tmp = ColorConverter.ConvertFromString(this.NormalBackground);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetNormalBackgrounBrush(checkBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetNormalBorderBrush(checkBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalForeground);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetNormalForegroundBrush(checkBox, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(this.PressedBackgroundBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetPressedBackgroundBrush(checkBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.PressedBorderBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetPressedBorderBrush(checkBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.PressedForegroundBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetPressedForegroundBrush(checkBox, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(this.MouseOverBackgroundBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetMouseOverBackgroundBrush(checkBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverBorderBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetMouseOverBorderBrush(checkBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverForegroundBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetMouseOverForegroundBrush(checkBox, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(this.DisableBackgroundBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetDisableBackgroundBrush(checkBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetDisableBorderBrush(checkBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableForegroundBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetDisableForegroundBrush(checkBox, new SolidColorBrush((Color)tmp));

                CheckBoxRadioButtonXaml.SetBorderThickness(checkBox,new Thickness(  BorderThickness));

            }

            if (radionButton  != null)
            {

                var tmp = ColorConverter.ConvertFromString(this.NormalBackground);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetNormalBackgrounBrush(radionButton, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetNormalBorderBrush(radionButton, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalForeground);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetNormalForegroundBrush(radionButton, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(this.PressedBackgroundBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetPressedBackgroundBrush(radionButton, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.PressedBorderBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetPressedBorderBrush(radionButton, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.PressedForegroundBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetPressedForegroundBrush(radionButton, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(this.MouseOverBackgroundBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetMouseOverBackgroundBrush(radionButton, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverBorderBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetMouseOverBorderBrush(radionButton, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverForegroundBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetMouseOverForegroundBrush(radionButton, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(this.DisableBackgroundBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetDisableBackgroundBrush(radionButton, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetDisableBorderBrush(radionButton, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableForegroundBrush);
                if (tmp != null)
                    CheckBoxRadioButtonXaml.SetDisableForegroundBrush(radionButton, new SolidColorBrush((Color)tmp));

                CheckBoxRadioButtonXaml.SetBorderThickness(radionButton, new Thickness(BorderThickness));

            }
        }
        #endregion 

    }

}
