using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.ButtonSet;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.TextBoxsSet
{
    public partial class TextBoxsAttriDataContext : INotifyPropertyChanged
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
    public partial class TextBoxsAttriDataContext
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



        #region InputingBackgroundBrush

        private string _InputingBackgroundBrush;

        public string InputingBackgroundBrush
        {
            get { return _InputingBackgroundBrush; }
            set
            {
                if (_InputingBackgroundBrush != value)
                {
                    _InputingBackgroundBrush = value;
                    this.OnPropertyChanged("InputingBackgroundBrush");
                }
            }
        }


        #endregion

        #region InputingForegroundBrush

        private string _InputingForegroundBrushBrushh;

        public string InputingForegroundBrush
        {
            get { return _InputingForegroundBrushBrushh; }
            set
            {
                if (_InputingForegroundBrushBrushh != value)
                {
                    _InputingForegroundBrushBrushh = value;
                    this.OnPropertyChanged("InputingForegroundBrush");
                }
            }
        }


        #endregion

        #region InputingBorderBrush

        private string _InputingBorderBrush;

        public string InputingBorderBrush
        {
            get { return _InputingBorderBrush; }
            set
            {
                if (_InputingBorderBrush != value)
                {
                    _InputingBorderBrush = value;
                    this.OnPropertyChanged("InputingBorderBrush");
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

    public partial class TextBoxsAttriDataContext
    {
        private DependencyObject textbox;
        private DependencyObject radionButton;
        public TextBoxsAttriDataContext(DependencyObject textboxobj)
        {
            textbox = textboxobj;

            this.NormalBackground = TextBoxsXaml.NormalBackgrounBrush.Color.ToString();
            this.NormalBorderBrush = TextBoxsXaml.NormalBorderBrush.Color.ToString(); //.Left ;
            this.NormalForeground = TextBoxsXaml.NormalForegroundBrush.Color.ToString();


            this.MouseOverBackgroundBrush = TextBoxsXaml.MouseOverBackgroundBrush.Color.ToString();
            this.MouseOverBorderBrush = TextBoxsXaml.MouseOverBorderBrush.Color.ToString();
            this.MouseOverForegroundBrush = TextBoxsXaml.MouseOverForegroundBrush.Color.ToString();

            this.InputingBackgroundBrush = TextBoxsXaml.InputingBackgroundBrush.Color.ToString();
            this.InputingBorderBrush = TextBoxsXaml.InputingBorderBrush.Color.ToString();
            this.InputingForegroundBrush = TextBoxsXaml.InputingForegroundBrush.Color.ToString();

            this.DisableBackgroundBrush = TextBoxsXaml.DisableBackgroundBrush.Color.ToString();
            this.DisableBorderBrush = TextBoxsXaml.DisableBorderBrush.Color.ToString();
            this.DisableForegroundBrush = TextBoxsXaml.DisableForegroundBrush.Color.ToString();

            this.CaretBrush  = TextBoxsXaml.CaretBrush .Color.ToString();
            this.SelectionBrush  = TextBoxsXaml.SelectionBrush .Color.ToString();

            this.BorderThickness = TextBoxsXaml.BorderThickness.Left ;

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
                TextBoxsXaml.NormalBackgrounBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
            if (tmp != null)
                TextBoxsXaml.NormalBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.NormalForeground);
            if (tmp != null)
                TextBoxsXaml.NormalForegroundBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.InputingBackgroundBrush);
            if (tmp != null)
                TextBoxsXaml.InputingBackgroundBrush  = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.InputingForegroundBrush);
            if (tmp != null)
                TextBoxsXaml.InputingForegroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.InputingBorderBrush);
            if (tmp != null)
                TextBoxsXaml.InputingBorderBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.MouseOverBackgroundBrush);
            if (tmp != null)
                TextBoxsXaml.MouseOverBackgroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.MouseOverBorderBrush);
            if (tmp != null)
                TextBoxsXaml.MouseOverBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.MouseOverForegroundBrush);
            if (tmp != null)
                TextBoxsXaml.MouseOverForegroundBrush = new SolidColorBrush((Color) tmp);


            tmp = ColorConverter.ConvertFromString(this.DisableBackgroundBrush);
            if (tmp != null)
                TextBoxsXaml.DisableBackgroundBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
            if (tmp != null)
                TextBoxsXaml.DisableBorderBrush = new SolidColorBrush((Color) tmp);
            tmp = ColorConverter.ConvertFromString(this.DisableForegroundBrush);
            if (tmp != null)
                TextBoxsXaml.DisableForegroundBrush = new SolidColorBrush((Color) tmp);

            tmp = ColorConverter.ConvertFromString(this.CaretBrush );
            if (tmp != null)
                TextBoxsXaml.CaretBrush  = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(this.SelectionBrush  );
            if (tmp != null)
                TextBoxsXaml.SelectionBrush  = new SolidColorBrush((Color)tmp);

            TextBoxsXaml.BorderThickness =new Thickness(  this.BorderThickness);

            TextBoxsXaml.SaveStyle();
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
            if (textbox != null)
            {

                var tmp = ColorConverter.ConvertFromString(this.NormalBackground);
                if (tmp != null)
                    TextBoxsXaml.SetNormalBackgrounBrush(textbox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    TextBoxsXaml.SetNormalBorderBrush(textbox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalForeground);
                if (tmp != null)
                    TextBoxsXaml.SetNormalForegroundBrush(textbox, new SolidColorBrush((Color) tmp));


                tmp = ColorConverter.ConvertFromString(this.InputingBackgroundBrush);
                if (tmp != null)
                    TextBoxsXaml.SetInputingBackgroundBrush(textbox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.InputingBorderBrush);
                if (tmp != null)
                    TextBoxsXaml.SetInputingBorderBrush(textbox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.InputingForegroundBrush);
                if (tmp != null)
                    TextBoxsXaml.SetInputingForegroundBrush(textbox, new SolidColorBrush((Color) tmp));


                tmp = ColorConverter.ConvertFromString(this.MouseOverBackgroundBrush);
                if (tmp != null)
                    TextBoxsXaml.SetMouseOverBackgroundBrush(textbox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverBorderBrush);
                if (tmp != null)
                    TextBoxsXaml.SetMouseOverBorderBrush(textbox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.MouseOverForegroundBrush);
                if (tmp != null)
                    TextBoxsXaml.SetMouseOverForegroundBrush(textbox, new SolidColorBrush((Color) tmp));


                tmp = ColorConverter.ConvertFromString(this.DisableBackgroundBrush);
                if (tmp != null)
                    TextBoxsXaml.SetDisableBackgroundBrush(textbox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
                if (tmp != null)
                    TextBoxsXaml.SetDisableBorderBrush(textbox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableForegroundBrush);
                if (tmp != null)
                    TextBoxsXaml.SetDisableForegroundBrush(textbox, new SolidColorBrush((Color) tmp));

                tmp = ColorConverter.ConvertFromString(this.CaretBrush);
                if (tmp != null)
                    TextBoxsXaml.SetCaretBrush(textbox, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(this.SelectionBrush);
                if (tmp != null)
                    TextBoxsXaml.SetSelectionBrush(textbox, new SolidColorBrush((Color) tmp));

                TextBoxsXaml.SetBorderThickness(textbox, new Thickness(BorderThickness));

            }
        }

        #endregion 

    }

}
