using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.ComboBoxListBoxSet
{
    public partial class ComboBoxListBoxAttriDataContext : INotifyPropertyChanged
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
    public partial class ComboBoxListBoxAttriDataContext
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

        #region InnerBorderThickness

        private double _innerBorderThickness;

        public double InnerBorderThickness
        {
            get { return _innerBorderThickness; }
            set
            {
                if (_innerBorderThickness != value)
                {
                    _innerBorderThickness = value;
                    this.OnPropertyChanged("InnerBorderThickness");
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


        #region CaretBrush

        private string _caretBrush;

        public string CaretBrush
        {
            get { return _caretBrush; }
            set
            {
                if (_caretBrush != value)
                {
                    _caretBrush = value;
                    this.OnPropertyChanged("CaretBrush");
                }
            }
        }


        #endregion

        #region SelectionBrush

        private string _selectionBrush;

        public string SelectionBrush
        {
            get { return _selectionBrush; }
            set
            {
                if (_selectionBrush != value)
                {
                    _selectionBrush = value;
                    this.OnPropertyChanged("SelectionBrush");
                }
            }
        }


        #endregion
    }

    public partial class ComboBoxListBoxAttriDataContext
    {
        private DependencyObject objCombox;
        private DependencyObject objListBox;
        public ComboBoxListBoxAttriDataContext(DependencyObject combox,DependencyObject listbox)
        {
            objCombox = combox;
            objListBox = listbox;
            this.NormalBackground = ComboBoxListBoxAttriXaml.NormalBackgrounBrush.Color.ToString();
            this.NormalBorderBrush = ComboBoxListBoxAttriXaml.NormalBorderBrush.Color.ToString(); //.Left ;
            this.NormalForeground = ComboBoxListBoxAttriXaml.NormalForegroundBrush.Color.ToString();


            this.MouseOverBackgroundBrush = ComboBoxListBoxAttriXaml.MouseOverBackgroundBrush.Color.ToString();
            this.MouseOverBorderBrush = ComboBoxListBoxAttriXaml.MouseOverBorderBrush.Color.ToString();
            this.MouseOverForegroundBrush = ComboBoxListBoxAttriXaml.MouseOverForegroundBrush.Color.ToString();

            this.PressedBackgroundBrush = ComboBoxListBoxAttriXaml.PressedBackgroundBrush.Color.ToString();
            this.PressedBorderBrush = ComboBoxListBoxAttriXaml.PressedBorderBrush.Color.ToString();
            this.PressedForegroundBrush = ComboBoxListBoxAttriXaml.PressedForegroundBrush.Color.ToString();

            this.DisableBackgroundBrush = ComboBoxListBoxAttriXaml.DisableBackgroundBrush.Color.ToString();
            this.DisableBorderBrush = ComboBoxListBoxAttriXaml.DisableBorderBrush.Color.ToString();
            this.DisableForegroundBrush = ComboBoxListBoxAttriXaml.DisableForegroundBrush.Color.ToString();

            this.SelectionBrush = ComboBoxListBoxAttriXaml.SelectionBrush.Color.ToString();
            this.CaretBrush = ComboBoxListBoxAttriXaml.CaretBrush.Color.ToString();

            this.BorderThickness = ComboBoxListBoxAttriXaml.BorderThickness.Left ;

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
                ComboBoxListBoxAttriXaml.NormalBackgrounBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
            if (tmp != null)
                ComboBoxListBoxAttriXaml.NormalBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.NormalForeground);
            if (tmp != null)
                ComboBoxListBoxAttriXaml.NormalForegroundBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.PressedBackgroundBrush);
            if (tmp != null)
                ComboBoxListBoxAttriXaml.PressedBackgroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.PressedBorderBrush);
            if (tmp != null)
                ComboBoxListBoxAttriXaml.PressedBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.PressedForegroundBrush);
            if (tmp != null)
                ComboBoxListBoxAttriXaml.PressedForegroundBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.MouseOverBackgroundBrush);
            if (tmp != null)
                ComboBoxListBoxAttriXaml.MouseOverBackgroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.MouseOverBorderBrush);
            if (tmp != null)
                ComboBoxListBoxAttriXaml.MouseOverBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.MouseOverForegroundBrush);
            if (tmp != null)
                ComboBoxListBoxAttriXaml.MouseOverForegroundBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.DisableBackgroundBrush);
            if (tmp != null)
                ComboBoxListBoxAttriXaml.DisableBackgroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
            if (tmp != null)
                ComboBoxListBoxAttriXaml.DisableBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.DisableForegroundBrush);
            if (tmp != null)
                ComboBoxListBoxAttriXaml.DisableForegroundBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.CaretBrush );
            if (tmp != null)
                ComboBoxListBoxAttriXaml.CaretBrush  = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(this.SelectionBrush );
            if (tmp != null)
                ComboBoxListBoxAttriXaml.SelectionBrush  = new SolidColorBrush((Color)tmp);

            ComboBoxListBoxAttriXaml.BorderThickness =new Thickness(  this.BorderThickness);
            ComboBoxListBoxAttriXaml.InnerBorderThickness  = new Thickness(this.InnerBorderThickness );
            ComboBoxListBoxAttriXaml.SaveStyle();
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
            if (objCombox != null)
            {

                var tmp = ColorConverter.ConvertFromString(this.NormalBackground);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetNormalBackgrounBrush(objCombox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetNormalBorderBrush(objCombox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalForeground);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetNormalForegroundBrush(objCombox, new SolidColorBrush((Color) tmp));


                tmp = ColorConverter.ConvertFromString(this.PressedBackgroundBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetPressedBackgroundBrush(objCombox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.PressedBorderBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetPressedBorderBrush(objCombox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.PressedForegroundBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetPressedForegroundBrush(objCombox, new SolidColorBrush((Color) tmp));


                tmp = ColorConverter.ConvertFromString(this.MouseOverBackgroundBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetMouseOverBackgroundBrush(objCombox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverBorderBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetMouseOverBorderBrush(objCombox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverForegroundBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetMouseOverForegroundBrush(objCombox, new SolidColorBrush((Color) tmp));


                tmp = ColorConverter.ConvertFromString(this.DisableBackgroundBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetDisableBackgroundBrush(objCombox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetDisableBorderBrush(objCombox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableForegroundBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetDisableForegroundBrush(objCombox, new SolidColorBrush((Color) tmp));


                tmp = ColorConverter.ConvertFromString(this.CaretBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetCaretBrush(objCombox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.SelectionBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetSelectionBrush(objCombox, new SolidColorBrush((Color) tmp));

                ComboBoxListBoxAttriXaml.SetBorderThickness(objCombox, new Thickness(BorderThickness));
                ComboBoxListBoxAttriXaml.SetInnerBorderThickness( objCombox, new Thickness(InnerBorderThickness ));

            }

            if (objListBox  != null)
            {

                var tmp = ColorConverter.ConvertFromString(this.NormalBackground);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetNormalBackgrounBrush(objListBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetNormalBorderBrush(objListBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalForeground);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetNormalForegroundBrush(objListBox, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(this.PressedBackgroundBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetPressedBackgroundBrush(objListBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.PressedBorderBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetPressedBorderBrush(objListBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.PressedForegroundBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetPressedForegroundBrush(objListBox, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(this.MouseOverBackgroundBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetMouseOverBackgroundBrush(objListBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverBorderBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetMouseOverBorderBrush(objListBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverForegroundBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetMouseOverForegroundBrush(objListBox, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(this.DisableBackgroundBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetDisableBackgroundBrush(objListBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetDisableBorderBrush(objListBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableForegroundBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetDisableForegroundBrush(objListBox, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(this.CaretBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetCaretBrush(objListBox, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.SelectionBrush);
                if (tmp != null)
                    ComboBoxListBoxAttriXaml.SetSelectionBrush(objListBox, new SolidColorBrush((Color)tmp));

                ComboBoxListBoxAttriXaml.SetBorderThickness(objListBox, new Thickness(BorderThickness));
                ComboBoxListBoxAttriXaml.SetInnerBorderThickness(objListBox, new Thickness(InnerBorderThickness));
            }
        }

        #endregion 

    }

}
