using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.DataGridSet
{
    public partial class DataGridAttriDataContext : INotifyPropertyChanged
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
    public partial class DataGridAttriDataContext
    {
        #region 内容面板及其边框
        #region BorderThickness

        private double _borderThickness;

        public double BorderThickness
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

        #region DisableBackground

        private string _disableBackground;

        public string DisableBackground
        {
            get { return _disableBackground; }
            set
            {
                if (_disableBackground != value)
                {
                    _disableBackground = value;
                    this.OnPropertyChanged("DisableBackground");
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
        #endregion

        #region Item项

        #region ItemNormalBackground

        private string _itemNormalBackground;

        public string ItemNormalBackground
        {
            get { return _itemNormalBackground; }
            set
            {
                if (_itemNormalBackground == value) return;
                _itemNormalBackground = value;
                OnPropertyChanged("ItemNormalBackground");
            }
        }

        #endregion

        #region ItemNormalForeground
        private string _itemNormalForeground;

        public string ItemNormalForeground
        {
            get { return _itemNormalForeground; }
            set
            {
                if (_itemNormalForeground == value) return;
                _itemNormalForeground = value;
                OnPropertyChanged("ItemNormalForeground");
            }
        }
        #endregion

        #region ItemNormalBorderBrush
        private string _itemNormalBorderBrush;

        public string ItemNormalBorderBrush
        {
            get { return _itemNormalBorderBrush; }
            set
            {
                if (_itemNormalBorderBrush == value) return;
                _itemNormalBorderBrush = value;
                OnPropertyChanged("ItemNormalBorderBrush");
            }
        }
        #endregion

        #region ItemBorderThickness
        private double _itemBorderThickness;

        public double ItemBorderThickness
        {
            get { return _itemBorderThickness; }
            set
            {
                if (_itemBorderThickness != value)
                {
                    _itemBorderThickness = value;
                    this.OnPropertyChanged("ItemBorderThickness");
                }
            }
        }
        #endregion



        #region ItemMouseOverBackground
        private string _itemMouseOverBackground;

        public string ItemMouseOverBackground
        {
            get { return _itemMouseOverBackground; }
            set
            {
                if (_itemMouseOverBackground == value) return;
                _itemMouseOverBackground = value;
                OnPropertyChanged("ItemMouseOverBackground");
            }
        }
        #endregion

        #region ItemMouseOverForeground
        private string _itemMouseOverForeground;

        public string ItemMouseOverForeground
        {
            get { return _itemMouseOverForeground; }
            set
            {
                if (_itemMouseOverForeground == value) return;
                _itemMouseOverForeground = value;
                OnPropertyChanged("ItemMouseOverForeground");
            }
        }
        #endregion

        #region ItemSelectedBackground
        private string _itemSelectedBackground;

        public string ItemSelectedBackground
        {
            get { return _itemSelectedBackground; }
            set
            {
                if (_itemSelectedBackground == value) return;
                _itemSelectedBackground = value;
                OnPropertyChanged("ItemSelectedBackground");
            }
        }
        #endregion

        #region ItemSelectedForeground
        private string _itemSelectedForeground;

        public string ItemSelectedForeground
        {
            get { return _itemSelectedForeground; }
            set
            {
                if (_itemSelectedForeground == value) return;
                _itemSelectedForeground = value;
                OnPropertyChanged("ItemSelectedForeground");
            }
        }
        #endregion

        #endregion

        #region Header项
        #region HeaderNormalBackground
        private string _headerNormalBackground;

        public string HeaderNormalBackground
        {
            get { return _headerNormalBackground; }
            set
            {
                if (_headerNormalBackground == value) return;
                _headerNormalBackground = value;
                OnPropertyChanged("HeaderNormalBackground");
            }
        }
        #endregion
        #region HeaderNormalForeground
        private string _headerNormalForeground;

        public string HeaderNormalForeground
        {
            get { return _headerNormalForeground; }
            set
            {
                if (_headerNormalForeground == value) return;
                _headerNormalForeground = value;
                OnPropertyChanged("HeaderNormalForeground");
            }
        }
        #endregion
        #region HeaderMouseOverBackground
        private string _headerMouseOverBackground;

        public string HeaderMouseOverBackground
        {
            get { return _headerMouseOverBackground; }
            set
            {
                if (_headerMouseOverBackground == value) return;
                _headerMouseOverBackground = value;
                OnPropertyChanged("HeaderMouseOverBackground");
            }
        }
        #endregion
        #region HeaderMouseOverForeground
        private string _headerMouseOverForeground;

        public string HeaderMouseOverForeground
        {
            get { return _headerMouseOverForeground; }
            set
            {
                if (_headerMouseOverForeground == value) return;
                _headerMouseOverForeground = value;
                OnPropertyChanged("HeaderMouseOverForeground");
            }
        }
        #endregion
        #region HeaderPressedBackground
        private string _headerPressedBackground;

        public string HeaderPressedBackground
        {
            get { return _headerPressedBackground; }
            set
            {
                if (_headerPressedBackground == value) return;
                _headerPressedBackground = value;
                OnPropertyChanged("HeaderPressedBackground");
            }
        }
        #endregion
        #region HeaderPressedForeground
        private string _headerPressedForeground;

        public string HeaderPressedForeground
        {
            get { return _headerPressedForeground; }
            set
            {
                if (_headerPressedForeground == value) return;
                _headerPressedForeground = value;
                OnPropertyChanged("HeaderPressedForeground");
            }
        }
        #endregion
        #endregion

        #region 表头分隔符

        #region HeaderSeparatedColor
        private string _headerSeparatedColor;

        public string HeaderSeparatedColor
        {
            get { return _headerSeparatedColor; }
            set
            {
                if (_headerSeparatedColor == value) return;
                _headerSeparatedColor = value;
                OnPropertyChanged("HeaderSeparatedColor");
            }
        }
        #endregion

        #region HeaderSeparatedWidth
        private double _headerSeparatedWidth;

        public double HeaderSeparatedWidth
        {
            get { return _headerSeparatedWidth; }
            set
            {
                if (_headerSeparatedWidth == value) return;
                _headerSeparatedWidth = value;
                OnPropertyChanged("HeaderSeparatedWidth");
            }
        }
        #endregion

        #endregion

    }

    public partial class DataGridAttriDataContext
    {
        private DependencyObject obj;
        private DependencyObject objdisable;
        public DataGridAttriDataContext(DependencyObject listview,DependencyObject listdisable)
        {
            obj = listview;
            objdisable = listdisable;

            this.NormalBackground = DataGridAttriXaml.NormalBackgrounBrush.Color.ToString();
            NormalBorderBrush = DataGridAttriXaml.NormalBorderBrush.Color.ToString();
            this.BorderThickness = DataGridAttriXaml.BorderThickness.Left;
            DisableBackground = DataGridAttriXaml.DisableBackgrounBrush.Color.ToString();
            DisableBorderBrush = DataGridAttriXaml.DisableBorderBrush.Color.ToString();

            ItemNormalBackground = DataGridAttriXaml.ItemNormalBackground.Color.ToString();
            ItemNormalForeground = DataGridAttriXaml.ItemNormalForeground.Color.ToString();
            ItemNormalBorderBrush = DataGridAttriXaml.ItemNormalBorderBrush.Color.ToString();
            ItemBorderThickness = DataGridAttriXaml.ItemBorderThickness.Left;

            ItemMouseOverBackground= DataGridAttriXaml.ItemMouseOverBackground.Color.ToString();
            ItemMouseOverForeground= DataGridAttriXaml.ItemMouseOverForeground.Color.ToString();
            ItemSelectedBackground = DataGridAttriXaml.ItemSelectedBackground.Color.ToString();
            ItemSelectedForeground = DataGridAttriXaml.ItemSelectedForeground.Color.ToString();


            HeaderNormalBackground = DataGridAttriXaml.HeaderNormalBackground.Color.ToString();
            HeaderNormalForeground = DataGridAttriXaml.HeaderNormalForeground.Color.ToString();
            HeaderMouseOverBackground = DataGridAttriXaml.HeaderMouseOverBackground.Color.ToString();
            HeaderMouseOverForeground = DataGridAttriXaml.HeaderMouseOverForeground.Color.ToString();
            HeaderPressedBackground = DataGridAttriXaml.HeaderPressedBackground.Color.ToString();
            HeaderPressedForeground = DataGridAttriXaml.HeaderPressedForeground.Color.ToString();

            HeaderSeparatedWidth = DataGridAttriXaml.HeaderSeparatedWidth;
            HeaderSeparatedColor = DataGridAttriXaml.HeaderSeparatedColor.Color.ToString();
            




        }
        #region save
        private ICommand _cmdSave;

        public ICommand CmdSave
        {
            get { return _cmdSave ?? (_cmdSave = new CommandRelay(Ex)); }
        }

        private void Ex()
        {
            var tmp = ColorConverter.ConvertFromString(this.NormalBackground);
            if (tmp != null)
                DataGridAttriXaml.NormalBackgrounBrush = new SolidColorBrush((Color)tmp);

            DataGridAttriXaml.BorderThickness = new Thickness(this.BorderThickness);

            tmp = ColorConverter.ConvertFromString(NormalBorderBrush);
            if (tmp != null)
                DataGridAttriXaml.NormalBorderBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(DisableBorderBrush);
            if (tmp != null)
                DataGridAttriXaml.DisableBorderBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(DisableBackground);
            if (tmp != null)
                DataGridAttriXaml.DisableBackgrounBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(ItemNormalBackground);
            if (tmp != null)
                DataGridAttriXaml.ItemNormalBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemNormalBorderBrush);
            if (tmp != null)
                DataGridAttriXaml.ItemNormalBorderBrush = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemNormalForeground);
            if (tmp != null)
                DataGridAttriXaml.ItemNormalForeground = new SolidColorBrush((Color)tmp);

            DataGridAttriXaml.ItemBorderThickness = new Thickness(ItemBorderThickness);


            tmp = ColorConverter.ConvertFromString(ItemMouseOverBackground);
            if (tmp != null)
                DataGridAttriXaml.ItemMouseOverBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemMouseOverForeground);
            if (tmp != null)
                DataGridAttriXaml.ItemMouseOverForeground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemSelectedBackground);
            if (tmp != null)
                DataGridAttriXaml.ItemSelectedBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemSelectedForeground);
            if (tmp != null)
                DataGridAttriXaml.ItemSelectedForeground = new SolidColorBrush((Color)tmp);


            tmp = ColorConverter.ConvertFromString(HeaderNormalBackground);
            if (tmp != null)
                DataGridAttriXaml.HeaderNormalBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(HeaderNormalForeground);
            if (tmp != null)
                DataGridAttriXaml.HeaderNormalForeground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(HeaderMouseOverBackground);
            if (tmp != null)
                DataGridAttriXaml.HeaderMouseOverBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(HeaderMouseOverForeground);
            if (tmp != null)
                DataGridAttriXaml.HeaderMouseOverForeground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(HeaderPressedBackground);
            if (tmp != null)
                DataGridAttriXaml.HeaderPressedBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(HeaderPressedForeground);
            if (tmp != null)
                DataGridAttriXaml.HeaderPressedForeground = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(HeaderSeparatedColor);
            if (tmp != null)
                DataGridAttriXaml.HeaderSeparatedColor = new SolidColorBrush((Color)tmp);

            DataGridAttriXaml.HeaderSeparatedWidth = HeaderSeparatedWidth;
            DataGridAttriXaml.SaveStyle();
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

                var tmp = ColorConverter.ConvertFromString(this.NormalBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetNormalBackgrounBrush(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    DataGridAttriXaml.SetNormalBorderBrush(obj, new SolidColorBrush((Color)tmp));

                DataGridAttriXaml.SetBorderThickness(obj, new Thickness(BorderThickness));

                 tmp = ColorConverter.ConvertFromString(this.DisableBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetDisableBackgrounBrush(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
                if (tmp != null)
                    DataGridAttriXaml.SetDisableBorderBrush(obj, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(ItemNormalBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetItemNormalBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemNormalBorderBrush);
                if (tmp != null)
                    DataGridAttriXaml.SetItemNormalBorderBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemNormalForeground);
                if (tmp != null)
                    DataGridAttriXaml.SetItemNormalForeground(obj, new SolidColorBrush((Color)tmp));
                DataGridAttriXaml.SetItemBorderThickness(obj, new Thickness(ItemBorderThickness));


                tmp = ColorConverter.ConvertFromString(ItemMouseOverBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetItemMouseOverBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemMouseOverForeground);
                if (tmp != null)
                    DataGridAttriXaml.SetItemMouseOverForeground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemSelectedBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetItemSelectedBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemSelectedForeground);
                if (tmp != null)
                    DataGridAttriXaml.SetItemSelectedForeground(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(HeaderNormalBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetHeaderNormalBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderNormalForeground);
                if (tmp != null)
                    DataGridAttriXaml.SetHeaderNormalForeground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderMouseOverBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetHeaderMouseOverBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderMouseOverForeground);
                if (tmp != null)
                    DataGridAttriXaml.SetHeaderMouseOverForeground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderPressedBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetHeaderPressedBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderPressedForeground);
                if (tmp != null)
                    DataGridAttriXaml.SetHeaderPressedForeground(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(HeaderSeparatedColor);
                if (tmp != null)
                    DataGridAttriXaml.SetHeaderSeparatedColor(obj, new SolidColorBrush((Color)tmp));

                DataGridAttriXaml.SetHeaderSeparatedWidth(obj, HeaderSeparatedWidth);

            }

            #region disable
            if(objdisable !=null)
            {
                var tmp = ColorConverter.ConvertFromString(this.NormalBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetNormalBackgrounBrush(objdisable, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    DataGridAttriXaml.SetNormalBorderBrush(objdisable, new SolidColorBrush((Color)tmp));

                DataGridAttriXaml.SetBorderThickness(objdisable, new Thickness(BorderThickness));

                tmp = ColorConverter.ConvertFromString(this.DisableBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetDisableBackgrounBrush(objdisable, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
                if (tmp != null)
                    DataGridAttriXaml.SetDisableBorderBrush(objdisable, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(ItemNormalBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetItemNormalBackground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemNormalBorderBrush);
                if (tmp != null)
                    DataGridAttriXaml.SetItemNormalBorderBrush(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemNormalForeground);
                if (tmp != null)
                    DataGridAttriXaml.SetItemNormalForeground(objdisable, new SolidColorBrush((Color)tmp));
                DataGridAttriXaml.SetItemBorderThickness(objdisable, new Thickness(ItemBorderThickness));

                tmp = ColorConverter.ConvertFromString(ItemMouseOverBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetItemMouseOverBackground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemMouseOverForeground);
                if (tmp != null)
                    DataGridAttriXaml.SetItemMouseOverForeground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemSelectedBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetItemSelectedBackground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemSelectedForeground);
                if (tmp != null)
                    DataGridAttriXaml.SetItemSelectedForeground(objdisable, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(HeaderNormalBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetHeaderNormalBackground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderNormalForeground);
                if (tmp != null)
                    DataGridAttriXaml.SetHeaderNormalForeground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderMouseOverBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetHeaderMouseOverBackground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderMouseOverForeground);
                if (tmp != null)
                    DataGridAttriXaml.SetHeaderMouseOverForeground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderPressedBackground);
                if (tmp != null)
                    DataGridAttriXaml.SetHeaderPressedBackground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderPressedForeground);
                if (tmp != null)
                    DataGridAttriXaml.SetHeaderPressedForeground(objdisable, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(HeaderSeparatedColor);
                if (tmp != null)
                    DataGridAttriXaml.SetHeaderSeparatedColor(objdisable, new SolidColorBrush((Color)tmp));

                DataGridAttriXaml.SetHeaderSeparatedWidth(objdisable, HeaderSeparatedWidth);
            }

            #endregion
        }
        #endregion

    }
}
