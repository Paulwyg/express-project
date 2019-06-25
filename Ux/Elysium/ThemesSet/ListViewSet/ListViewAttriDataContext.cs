using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.ListViewSet
{
    public partial class ListViewAttriDataContext : INotifyPropertyChanged
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
    public partial class ListViewAttriDataContext
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

    public partial class ListViewAttriDataContext
    {
        private DependencyObject obj;
        private DependencyObject objdisable;
        public ListViewAttriDataContext(DependencyObject listview,DependencyObject listdisable)
        {
            obj = listview;
            objdisable = listdisable;

            this.NormalBackground = ListViewAttriXaml.NormalBackgrounBrush.Color.ToString();
            NormalBorderBrush = ListViewAttriXaml.NormalBorderBrush.Color.ToString();
            this.BorderThickness = ListViewAttriXaml.BorderThickness.Left;
            DisableBackground = ListViewAttriXaml.DisableBackgrounBrush.Color.ToString();
            DisableBorderBrush = ListViewAttriXaml.DisableBorderBrush.Color.ToString();

            ItemNormalBackground = ListViewAttriXaml.ItemNormalBackground.Color.ToString();
            ItemNormalForeground = ListViewAttriXaml.ItemNormalForeground.Color.ToString();
            ItemNormalBorderBrush = ListViewAttriXaml.ItemNormalBorderBrush.Color.ToString();
            ItemBorderThickness = ListViewAttriXaml.ItemBorderThickness.Left;

            ItemMouseOverBackground= ListViewAttriXaml.ItemMouseOverBackground.Color.ToString();
            ItemMouseOverForeground= ListViewAttriXaml.ItemMouseOverForeground.Color.ToString();
            ItemSelectedBackground = ListViewAttriXaml.ItemSelectedBackground.Color.ToString();
            ItemSelectedForeground = ListViewAttriXaml.ItemSelectedForeground.Color.ToString();


            HeaderNormalBackground = ListViewAttriXaml.HeaderNormalBackground.Color.ToString();
            HeaderNormalForeground = ListViewAttriXaml.HeaderNormalForeground.Color.ToString();
            HeaderMouseOverBackground = ListViewAttriXaml.HeaderMouseOverBackground.Color.ToString();
            HeaderMouseOverForeground = ListViewAttriXaml.HeaderMouseOverForeground.Color.ToString();
            HeaderPressedBackground = ListViewAttriXaml.HeaderPressedBackground.Color.ToString();
            HeaderPressedForeground = ListViewAttriXaml.HeaderPressedForeground.Color.ToString();

            HeaderSeparatedWidth = ListViewAttriXaml.HeaderSeparatedWidth;
            HeaderSeparatedColor = ListViewAttriXaml.HeaderSeparatedColor.Color.ToString();
            




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
                ListViewAttriXaml.NormalBackgrounBrush = new SolidColorBrush((Color)tmp);

            ListViewAttriXaml.BorderThickness = new Thickness(this.BorderThickness);

            tmp = ColorConverter.ConvertFromString(NormalBorderBrush);
            if (tmp != null)
                ListViewAttriXaml.NormalBorderBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(DisableBorderBrush);
            if (tmp != null)
                ListViewAttriXaml.DisableBorderBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(DisableBackground);
            if (tmp != null)
                ListViewAttriXaml.DisableBackgrounBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(ItemNormalBackground);
            if (tmp != null)
                ListViewAttriXaml.ItemNormalBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemNormalBorderBrush);
            if (tmp != null)
                ListViewAttriXaml.ItemNormalBorderBrush = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemNormalForeground);
            if (tmp != null)
                ListViewAttriXaml.ItemNormalForeground = new SolidColorBrush((Color)tmp);

            ListViewAttriXaml.ItemBorderThickness = new Thickness(ItemBorderThickness);


            tmp = ColorConverter.ConvertFromString(ItemMouseOverBackground);
            if (tmp != null)
                ListViewAttriXaml.ItemMouseOverBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemMouseOverForeground);
            if (tmp != null)
                ListViewAttriXaml.ItemMouseOverForeground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemSelectedBackground);
            if (tmp != null)
                ListViewAttriXaml.ItemSelectedBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemSelectedForeground);
            if (tmp != null)
                ListViewAttriXaml.ItemSelectedForeground = new SolidColorBrush((Color)tmp);


            tmp = ColorConverter.ConvertFromString(HeaderNormalBackground);
            if (tmp != null)
                ListViewAttriXaml.HeaderNormalBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(HeaderNormalForeground);
            if (tmp != null)
                ListViewAttriXaml.HeaderNormalForeground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(HeaderMouseOverBackground);
            if (tmp != null)
                ListViewAttriXaml.HeaderMouseOverBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(HeaderMouseOverForeground);
            if (tmp != null)
                ListViewAttriXaml.HeaderMouseOverForeground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(HeaderPressedBackground);
            if (tmp != null)
                ListViewAttriXaml.HeaderPressedBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(HeaderPressedForeground);
            if (tmp != null)
                ListViewAttriXaml.HeaderPressedForeground = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(HeaderSeparatedColor);
            if (tmp != null)
                ListViewAttriXaml.HeaderSeparatedColor = new SolidColorBrush((Color)tmp);

            ListViewAttriXaml.HeaderSeparatedWidth = HeaderSeparatedWidth;
            ListViewAttriXaml.SaveStyle();
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
                    ListViewAttriXaml.SetNormalBackgrounBrush(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    ListViewAttriXaml.SetNormalBorderBrush(obj, new SolidColorBrush((Color)tmp));

                ListViewAttriXaml.SetBorderThickness(obj, new Thickness(BorderThickness));

                 tmp = ColorConverter.ConvertFromString(this.DisableBackground);
                if (tmp != null)
                    ListViewAttriXaml.SetDisableBackgrounBrush(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
                if (tmp != null)
                    ListViewAttriXaml.SetDisableBorderBrush(obj, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(ItemNormalBackground);
                if (tmp != null)
                    ListViewAttriXaml.SetItemNormalBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemNormalBorderBrush);
                if (tmp != null)
                    ListViewAttriXaml.SetItemNormalBorderBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemNormalForeground);
                if (tmp != null)
                    ListViewAttriXaml.SetItemNormalForeground(obj, new SolidColorBrush((Color)tmp));
                ListViewAttriXaml.SetItemBorderThickness(obj, new Thickness(ItemBorderThickness));


                tmp = ColorConverter.ConvertFromString(ItemMouseOverBackground);
                if (tmp != null)
                    ListViewAttriXaml.SetItemMouseOverBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemMouseOverForeground);
                if (tmp != null)
                    ListViewAttriXaml.SetItemMouseOverForeground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemSelectedBackground);
                if (tmp != null)
                    ListViewAttriXaml.SetItemSelectedBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemSelectedForeground);
                if (tmp != null)
                    ListViewAttriXaml.SetItemSelectedForeground(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(HeaderNormalBackground);
                if (tmp != null)
                    ListViewAttriXaml.SetHeaderNormalBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderNormalForeground);
                if (tmp != null)
                    ListViewAttriXaml.SetHeaderNormalForeground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderMouseOverBackground);
                if (tmp != null)
                    ListViewAttriXaml.SetHeaderMouseOverBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderMouseOverForeground);
                if (tmp != null)
                    ListViewAttriXaml.SetHeaderMouseOverForeground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderPressedBackground);
                if (tmp != null)
                    ListViewAttriXaml.SetHeaderPressedBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderPressedForeground);
                if (tmp != null)
                    ListViewAttriXaml.SetHeaderPressedForeground(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(HeaderSeparatedColor);
                if (tmp != null)
                    ListViewAttriXaml.SetHeaderSeparatedColor(obj, new SolidColorBrush((Color)tmp));

                ListViewAttriXaml.SetHeaderSeparatedWidth(obj, HeaderSeparatedWidth);

            }

            #region disable
            if(objdisable !=null)
            {
                var tmp = ColorConverter.ConvertFromString(this.NormalBackground);
                if (tmp != null)
                    ListViewAttriXaml.SetNormalBackgrounBrush(objdisable, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    ListViewAttriXaml.SetNormalBorderBrush(objdisable, new SolidColorBrush((Color)tmp));

                ListViewAttriXaml.SetBorderThickness(objdisable, new Thickness(BorderThickness));

                tmp = ColorConverter.ConvertFromString(this.DisableBackground);
                if (tmp != null)
                    ListViewAttriXaml.SetDisableBackgrounBrush(objdisable, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
                if (tmp != null)
                    ListViewAttriXaml.SetDisableBorderBrush(objdisable, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(ItemNormalBackground);
                if (tmp != null)
                    ListViewAttriXaml.SetItemNormalBackground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemNormalBorderBrush);
                if (tmp != null)
                    ListViewAttriXaml.SetItemNormalBorderBrush(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemNormalForeground);
                if (tmp != null)
                    ListViewAttriXaml.SetItemNormalForeground(objdisable, new SolidColorBrush((Color)tmp));
                ListViewAttriXaml.SetItemBorderThickness(objdisable, new Thickness(ItemBorderThickness));

                tmp = ColorConverter.ConvertFromString(ItemMouseOverBackground);
                if (tmp != null)
                    ListViewAttriXaml.SetItemMouseOverBackground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemMouseOverForeground);
                if (tmp != null)
                    ListViewAttriXaml.SetItemMouseOverForeground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemSelectedBackground);
                if (tmp != null)
                    ListViewAttriXaml.SetItemSelectedBackground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemSelectedForeground);
                if (tmp != null)
                    ListViewAttriXaml.SetItemSelectedForeground(objdisable, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(HeaderNormalBackground);
                if (tmp != null)
                    ListViewAttriXaml.SetHeaderNormalBackground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderNormalForeground);
                if (tmp != null)
                    ListViewAttriXaml.SetHeaderNormalForeground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderMouseOverBackground);
                if (tmp != null)
                    ListViewAttriXaml.SetHeaderMouseOverBackground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderMouseOverForeground);
                if (tmp != null)
                    ListViewAttriXaml.SetHeaderMouseOverForeground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderPressedBackground);
                if (tmp != null)
                    ListViewAttriXaml.SetHeaderPressedBackground(objdisable, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderPressedForeground);
                if (tmp != null)
                    ListViewAttriXaml.SetHeaderPressedForeground(objdisable, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(HeaderSeparatedColor);
                if (tmp != null)
                    ListViewAttriXaml.SetHeaderSeparatedColor(objdisable, new SolidColorBrush((Color)tmp));

                ListViewAttriXaml.SetHeaderSeparatedWidth(objdisable, HeaderSeparatedWidth);
            }

            #endregion
        }
        #endregion

    }
}
