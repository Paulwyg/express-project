using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.TabSet
{
    public partial class TabAttriDataContext : INotifyPropertyChanged
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
    public partial class TabAttriDataContext
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




        #region SelectedIndicateBrush

        private string _SelectedIndicateBrush;

        public string SelectedIndicateBrush
        {
            get { return _SelectedIndicateBrush; }
            set
            {
                if (_SelectedIndicateBrush != value)
                {
                    _SelectedIndicateBrush = value;
                    this.OnPropertyChanged("SelectedIndicateBrush");
                }
            }
        }


        #endregion
        #region UnSelectedIndicateBrush

        private string _UnSelectedIndicateBrush;

        public string UnSelectedIndicateBrush
        {
            get { return _UnSelectedIndicateBrush; }
            set
            {
                if (_UnSelectedIndicateBrush != value)
                {
                    _UnSelectedIndicateBrush = value;
                    this.OnPropertyChanged("UnSelectedIndicateBrush");
                }
            }
        }


        #endregion
        #region NormalTabForegroundBrush

        private string _NormalTabForegroundBrush;

        public string NormalTabForegroundBrush
        {
            get { return _NormalTabForegroundBrush; }
            set
            {
                if (_NormalTabForegroundBrush != value)
                {
                    _NormalTabForegroundBrush = value;
                    this.OnPropertyChanged("NormalTabForegroundBrush");
                }
            }
        }


        #endregion
        #region NormalTabBackgroundBrush

        private string _NormalTabBackgroundBrush;

        public string NormalTabBackgroundBrush
        {
            get { return _NormalTabBackgroundBrush; }
            set
            {
                if (_NormalTabBackgroundBrush != value)
                {
                    _NormalTabBackgroundBrush = value;
                    this.OnPropertyChanged("NormalTabBackgroundBrush");
                }
            }
        }


        #endregion
        #region NormalTabBorderBrush

        private string _NormalTabBorderBrush;

        public string NormalTabBorderBrush
        {
            get { return _NormalTabBorderBrush; }
            set
            {
                if (_NormalTabBorderBrush != value)
                {
                    _NormalTabBorderBrush = value;
                    this.OnPropertyChanged("NormalTabBorderBrush");
                }
            }
        }


        #endregion
        #region TabBorderThickness

        private double  _TabBorderThickness;

        public double TabBorderThickness
        {
            get { return _TabBorderThickness; }
            set
            {
                if (_TabBorderThickness != value)
                {
                    _TabBorderThickness = value;
                    this.OnPropertyChanged("TabBorderThickness");
                }
            }
        }


        #endregion
    }

    public partial class TabAttriDataContext
    {
        private DependencyObject objgroupbox;
        private DependencyObject objItem1;
        private DependencyObject objItem2;

        public TabAttriDataContext(DependencyObject tabContorl, DependencyObject tabItem1,
                                        DependencyObject tabItem2)
        {
            objgroupbox = tabContorl;
            objItem1 = tabItem1;
            objItem2 = tabItem2;

            this.NormalBackground = TabAttriXaml.NormalBackgrounBrush.Color.ToString();
            this.NormalBorderBrush = TabAttriXaml.NormalBorderBrush.Color.ToString(); //.Left ;
            this.NormalForeground = TabAttriXaml.NormalForegroundBrush.Color.ToString();


            this.MouseOverBackgroundBrush = TabAttriXaml.MouseOverBackgroundBrush.Color.ToString();
            this.MouseOverBorderBrush = TabAttriXaml.MouseOverBorderBrush.Color.ToString();
            this.MouseOverForegroundBrush = TabAttriXaml.MouseOverForegroundBrush.Color.ToString();

            this.PressedBackgroundBrush = TabAttriXaml.PressedBackgroundBrush.Color.ToString();
            this.PressedBorderBrush = TabAttriXaml.PressedBorderBrush.Color.ToString();
            this.PressedForegroundBrush = TabAttriXaml.PressedForegroundBrush.Color.ToString();

            this.DisableBackgroundBrush = TabAttriXaml.DisableBackgroundBrush.Color.ToString();
            this.DisableBorderBrush = TabAttriXaml.DisableBorderBrush.Color.ToString();
            this.DisableForegroundBrush = TabAttriXaml.DisableForegroundBrush.Color.ToString();


            this.SelectedIndicateBrush  = TabAttriXaml.SelectedIndicateBrush .Color.ToString();
            this.UnSelectedIndicateBrush  = TabAttriXaml.UnSelectedIndicateBrush .Color.ToString();

            this.NormalTabBackgroundBrush = TabAttriXaml.NormalTabBackgroundBrush.Color.ToString();
            this.NormalTabBorderBrush = TabAttriXaml.NormalTabBorderBrush.Color.ToString();
            this.NormalTabForegroundBrush = TabAttriXaml.NormalTabForegroundBrush.Color.ToString();

            this.BorderThickness = TabAttriXaml.BorderThickness.Left;
            this.TabBorderThickness = TabAttriXaml.TabBorderThickness.Left ;

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
                TabAttriXaml.NormalBackgrounBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
            if (tmp != null)
                TabAttriXaml.NormalBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.NormalForeground);
            if (tmp != null)
                TabAttriXaml.NormalForegroundBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.PressedBackgroundBrush);
            if (tmp != null)
                TabAttriXaml.PressedBackgroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.PressedBorderBrush);
            if (tmp != null)
                TabAttriXaml.PressedBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.PressedForegroundBrush);
            if (tmp != null)
                TabAttriXaml.PressedForegroundBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.MouseOverBackgroundBrush);
            if (tmp != null)
                TabAttriXaml.MouseOverBackgroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.MouseOverBorderBrush);
            if (tmp != null)
                TabAttriXaml.MouseOverBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.MouseOverForegroundBrush);
            if (tmp != null)
                TabAttriXaml.MouseOverForegroundBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.DisableBackgroundBrush);
            if (tmp != null)
                TabAttriXaml.DisableBackgroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
            if (tmp != null)
                TabAttriXaml.DisableBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.DisableForegroundBrush);
            if (tmp != null)
                TabAttriXaml.DisableForegroundBrush = new SolidColorBrush((Color) tmp);




            tmp = ColorConverter.ConvertFromString(this.SelectedIndicateBrush);
            if (tmp != null)
                TabAttriXaml.SelectedIndicateBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.UnSelectedIndicateBrush);
            if (tmp != null)
                TabAttriXaml.UnSelectedIndicateBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.NormalTabBackgroundBrush);
            if (tmp != null)
                TabAttriXaml.NormalTabBackgroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.NormalTabBorderBrush);
            if (tmp != null)
                TabAttriXaml.NormalTabBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.NormalTabForegroundBrush);
            if (tmp != null)
                TabAttriXaml.NormalTabForegroundBrush = new SolidColorBrush((Color) tmp);

            TabAttriXaml.BorderThickness = new Thickness(this.BorderThickness);
            TabAttriXaml.TabBorderThickness = new Thickness(this.TabBorderThickness);

            TabAttriXaml.SaveStyle();
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
            if (objItem1 != null)
            {

                var tmp = ColorConverter.ConvertFromString(this.NormalBackground);
                if (tmp != null)
                    TabAttriXaml.SetNormalBackgrounBrush(objItem1, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    TabAttriXaml.SetNormalBorderBrush(objItem1, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalForeground);
                if (tmp != null)
                    TabAttriXaml.SetNormalForegroundBrush(objItem1, new SolidColorBrush((Color) tmp));


                tmp = ColorConverter.ConvertFromString(this.PressedBackgroundBrush);
                if (tmp != null)
                    TabAttriXaml.SetPressedBackgroundBrush(objItem1, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.PressedBorderBrush);
                if (tmp != null)
                    TabAttriXaml.SetPressedBorderBrush(objItem1, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.PressedForegroundBrush);
                if (tmp != null)
                    TabAttriXaml.SetPressedForegroundBrush(objItem1, new SolidColorBrush((Color) tmp));


                tmp = ColorConverter.ConvertFromString(this.MouseOverBackgroundBrush);
                if (tmp != null)
                    TabAttriXaml.SetMouseOverBackgroundBrush(objItem1, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverBorderBrush);
                if (tmp != null)
                    TabAttriXaml.SetMouseOverBorderBrush(objItem1, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverForegroundBrush);
                if (tmp != null)
                    TabAttriXaml.SetMouseOverForegroundBrush(objItem1, new SolidColorBrush((Color) tmp));


                tmp = ColorConverter.ConvertFromString(this.DisableBackgroundBrush);
                if (tmp != null)
                    TabAttriXaml.SetDisableBackgroundBrush(objItem1, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
                if (tmp != null)
                    TabAttriXaml.SetDisableBorderBrush(objItem1, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableForegroundBrush);
                if (tmp != null)
                    TabAttriXaml.SetDisableForegroundBrush(objItem1, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.SelectedIndicateBrush);
                if (tmp != null)
                    TabAttriXaml.SetSelectedIndicateBrush(objItem1, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.UnSelectedIndicateBrush);
                if (tmp != null)
                    TabAttriXaml.SetUnSelectedIndicateBrush(objItem1, new SolidColorBrush((Color) tmp));
                TabAttriXaml.SetBorderThickness(objItem1, new Thickness(BorderThickness));

            }

            if (objItem2 != null)
            {

                var tmp = ColorConverter.ConvertFromString(this.NormalBackground);
                if (tmp != null)
                    TabAttriXaml.SetNormalBackgrounBrush(objItem2, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    TabAttriXaml.SetNormalBorderBrush(objItem2, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalForeground);
                if (tmp != null)
                    TabAttriXaml.SetNormalForegroundBrush(objItem2, new SolidColorBrush((Color) tmp));


                tmp = ColorConverter.ConvertFromString(this.PressedBackgroundBrush);
                if (tmp != null)
                    TabAttriXaml.SetPressedBackgroundBrush(objItem2, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.PressedBorderBrush);
                if (tmp != null)
                    TabAttriXaml.SetPressedBorderBrush(objItem2, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.PressedForegroundBrush);
                if (tmp != null)
                    TabAttriXaml.SetPressedForegroundBrush(objItem2, new SolidColorBrush((Color) tmp));


                tmp = ColorConverter.ConvertFromString(this.MouseOverBackgroundBrush);
                if (tmp != null)
                    TabAttriXaml.SetMouseOverBackgroundBrush(objItem2, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverBorderBrush);
                if (tmp != null)
                    TabAttriXaml.SetMouseOverBorderBrush(objItem2, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverForegroundBrush);
                if (tmp != null)
                    TabAttriXaml.SetMouseOverForegroundBrush(objItem2, new SolidColorBrush((Color) tmp));


                tmp = ColorConverter.ConvertFromString(this.DisableBackgroundBrush);
                if (tmp != null)
                    TabAttriXaml.SetDisableBackgroundBrush(objItem2, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
                if (tmp != null)
                    TabAttriXaml.SetDisableBorderBrush(objItem2, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableForegroundBrush);
                if (tmp != null)
                    TabAttriXaml.SetDisableForegroundBrush(objItem2, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.SelectedIndicateBrush);
                if (tmp != null)
                    TabAttriXaml.SetSelectedIndicateBrush(objItem2, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.UnSelectedIndicateBrush);
                if (tmp != null)
                    TabAttriXaml.SetUnSelectedIndicateBrush(objItem2, new SolidColorBrush((Color) tmp));
                TabAttriXaml.SetBorderThickness(objItem2, new Thickness(BorderThickness));

            }

            if (objgroupbox != null)
            {

                var tmp = ColorConverter.ConvertFromString(this.NormalTabBackgroundBrush);
                if (tmp != null)
                    TabAttriXaml.SetNormalTabBackgroundBrush(objgroupbox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalTabBorderBrush);
                if (tmp != null)
                    TabAttriXaml.SetNormalTabBorderBrush(objgroupbox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalTabForegroundBrush);
                if (tmp != null)
                    TabAttriXaml.SetNormalTabForegroundBrush(objgroupbox, new SolidColorBrush((Color) tmp));



                TabAttriXaml.SetTabBorderThickness( objgroupbox, new Thickness(TabBorderThickness));

            }
        }

        #endregion

    }

}
