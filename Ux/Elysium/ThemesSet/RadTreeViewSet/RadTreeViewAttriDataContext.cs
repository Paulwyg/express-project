using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.RadTreeViewSet
{
    public partial class RadTreeViewAttriDataContext : INotifyPropertyChanged
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
    public partial class RadTreeViewAttriDataContext
    {

        #region 内容面板及其边框

        #region RadTreeViewBorderThickness

        private double _radTreeViewBorderThickness;

        public double RadTreeViewBorderThickness
        {
            get { return _radTreeViewBorderThickness; }
            set
            {
                if (_radTreeViewBorderThickness != value)
                {
                    _radTreeViewBorderThickness = value;
                    this.OnPropertyChanged("RadTreeViewBorderThickness");
                }
            }
        }


        #endregion

        #region RadTreeViewBackground

        private string _radTreeViewBackground;

        public string RadTreeViewBackground
        {
            get { return _radTreeViewBackground; }
            set
            {
                if (_radTreeViewBackground != value)
                {
                    _radTreeViewBackground = value;
                    this.OnPropertyChanged("RadTreeViewBackground");
                }
            }
        }


        #endregion

        #region RadTreeViewBackgroundMouseOver

        private string _radTreeViewBackgroundMouseOver;

        public string RadTreeViewBackgroundMouseOver
        {
            get { return _radTreeViewBackgroundMouseOver; }
            set
            {
                if (_radTreeViewBackgroundMouseOver != value)
                {
                    _radTreeViewBackgroundMouseOver = value;
                    this.OnPropertyChanged("RadTreeViewBackgroundMouseOver");
                }
            }
        }


        #endregion

        #region RadTreeViewBorderBrush

        private string _radTreeViewBorderBrush;

        public string RadTreeViewBorderBrush
        {
            get { return _radTreeViewBorderBrush; }
            set
            {
                if (_radTreeViewBorderBrush != value)
                {
                    _radTreeViewBorderBrush = value;
                    OnPropertyChanged("RadTreeViewBorderBrush");
                }
            }
        }


        #endregion

        #region ItemRadTreeViewBackground

        private string _itemRadTreeViewBackground;

        public string ItemRadTreeViewBackground
        {
            get { return _itemRadTreeViewBackground; }
            set
            {
                if (_itemRadTreeViewBackground != value)
                {
                    _itemRadTreeViewBackground = value;
                    OnPropertyChanged("ItemRadTreeViewBackground");
                }
            }
        }


        #endregion

        #region ItemRadTreeViewForeground

        private string _itemRadTreeViewForeground;

        public string ItemRadTreeViewForeground
        {
            get { return _itemRadTreeViewForeground; }
            set
            {
                if (_itemRadTreeViewForeground != value)
                {
                    _itemRadTreeViewForeground = value;
                    OnPropertyChanged("ItemRadTreeViewForeground");
                }
            }
        }


        #endregion

        #region ItemRadTreeViewBackgroundMouseOver

        private string _itemRadTreeViewBackgroundMouseOver;

        public string ItemRadTreeViewBackgroundMouseOver
        {
            get { return _itemRadTreeViewBackgroundMouseOver; }
            set
            {
                if (_itemRadTreeViewBackgroundMouseOver != value)
                {
                    _itemRadTreeViewBackgroundMouseOver = value;
                    OnPropertyChanged("ItemRadTreeViewBackgroundMouseOver");
                }
            }
        }


        #endregion

        #region ItemRadTreeViewForegroundMouseOver

        private string _itemRadTreeViewForegroundMouseOver;

        public string ItemRadTreeViewForegroundMouseOver
        {
            get { return _itemRadTreeViewForegroundMouseOver; }
            set
            {
                if (_itemRadTreeViewForegroundMouseOver != value)
                {
                    _itemRadTreeViewForegroundMouseOver = value;
                    OnPropertyChanged("ItemRadTreeViewForegroundMouseOver");
                }
            }
        }


        #endregion

        #region ItemRadTreeViewBackgroundSelected

        private string _itemRadTreeViewBackgroundSelected;

        public string ItemRadTreeViewBackgroundSelected
        {
            get { return _itemRadTreeViewBackgroundSelected; }
            set
            {
                if (_itemRadTreeViewBackgroundSelected != value)
                {
                    _itemRadTreeViewBackgroundSelected = value;
                    OnPropertyChanged("ItemRadTreeViewBackgroundSelected");
                }
            }
        }


        #endregion

        #region ItemRadTreeViewForegroundSelected

        private string _itemRadTreeViewForegroundSelected;

        public string ItemRadTreeViewForegroundSelected
        {
            get { return _itemRadTreeViewForegroundSelected; }
            set
            {
                if (_itemRadTreeViewForegroundSelected != value)
                {
                    _itemRadTreeViewForegroundSelected = value;
                    OnPropertyChanged("ItemRadTreeViewForegroundSelected");
                }
            }
        }


        #endregion

        #region TitleExplandRadTreeViewBackground

        private string _titleExplandRadTreeViewBackground;

        public string TitleExplandRadTreeViewBackground
        {
            get { return _titleExplandRadTreeViewBackground; }
            set
            {
                if (_titleExplandRadTreeViewBackground != value)
                {
                    _titleExplandRadTreeViewBackground = value;
                    OnPropertyChanged("TitleExplandRadTreeViewBackground");
                }
            }
        }


        #endregion

        #region TitleExplandRadTreeViewBackgroundMouseOver

        private string _titleExplandRadTreeViewBackgroundMouseOver;

        public string TitleExplandRadTreeViewBackgroundMouseOver
        {
            get { return _titleExplandRadTreeViewBackgroundMouseOver; }
            set
            {
                if (_titleExplandRadTreeViewBackgroundMouseOver != value)
                {
                    _titleExplandRadTreeViewBackgroundMouseOver = value;
                    OnPropertyChanged("TitleExplandRadTreeViewBackgroundMouseOver");
                }
            }
        }


        #endregion

        #region TitleRadTreeViewBackgroundMouseOver

        private string _titleRadTreeViewBackgroundMouseOver;

        public string TitleRadTreeViewBackgroundMouseOver
        {
            get { return _titleRadTreeViewBackgroundMouseOver; }
            set
            {
                if (_titleRadTreeViewBackgroundMouseOver != value)
                {
                    _titleRadTreeViewBackgroundMouseOver = value;
                    OnPropertyChanged("TitleRadTreeViewBackgroundMouseOver");
                }
            }
        }


        #endregion

        #region TitleRadTreeViewBackground

        private string _titleRadTreeViewBackground;

        public string TitleRadTreeViewBackground
        {
            get { return _titleRadTreeViewBackground; }
            set
            {
                if (_titleRadTreeViewBackground != value)
                {
                    _titleRadTreeViewBackground = value;
                    OnPropertyChanged("TitleRadTreeViewBackground");
                }
            }
        }


        #endregion

        #endregion


    }

    public partial class RadTreeViewAttriDataContext
    {
        private readonly DependencyObject obj;
        public RadTreeViewAttriDataContext(DependencyObject listview)
        {

            obj = listview;
            RadTreeViewBorderThickness = RadTreeViewAttriXaml.RadTreeViewBorderThickness.Left;
            RadTreeViewBackground = RadTreeViewAttriXaml.RadTreeViewBackground.Color.ToString();
            RadTreeViewBackgroundMouseOver = RadTreeViewAttriXaml.RadTreeViewBackgroundMouseOver.Color.ToString();
            RadTreeViewBorderBrush = RadTreeViewAttriXaml.RadTreeViewBorderBrush.Color.ToString();

            ItemRadTreeViewBackground = RadTreeViewAttriXaml.ItemRadTreeViewBackground.Color.ToString();
            ItemRadTreeViewForeground = RadTreeViewAttriXaml.ItemRadTreeViewForeground.Color.ToString();
            ItemRadTreeViewBackgroundMouseOver = RadTreeViewAttriXaml.ItemRadTreeViewBackgroundMouseOver.Color.ToString();
            ItemRadTreeViewForegroundMouseOver = RadTreeViewAttriXaml.ItemRadTreeViewForegroundMouseOver.Color.ToString();
            ItemRadTreeViewBackgroundSelected = RadTreeViewAttriXaml.ItemRadTreeViewBackgroundSelected.Color.ToString();
            ItemRadTreeViewForegroundSelected = RadTreeViewAttriXaml.ItemRadTreeViewForegroundSelected.Color.ToString();

            TitleExplandRadTreeViewBackground = RadTreeViewAttriXaml.TitleExplandRadTreeViewBackground.Color.ToString();
            TitleExplandRadTreeViewBackgroundMouseOver = RadTreeViewAttriXaml.TitleExplandRadTreeViewBackgroundMouseOver.Color.ToString();
            TitleRadTreeViewBackgroundMouseOver = RadTreeViewAttriXaml.TitleRadTreeViewBackgroundMouseOver.Color.ToString();
            TitleRadTreeViewBackground = RadTreeViewAttriXaml.TitleRadTreeViewBackground.Color.ToString();

        }
        #region save
        private ICommand _cmdSave;

        public ICommand CmdSave
        {
            get { return _cmdSave ?? (_cmdSave = new CommandRelay(Ex)); }
        }

        private void Ex()
        {

            RadTreeViewAttriXaml.RadTreeViewBorderThickness = new Thickness(RadTreeViewBorderThickness);
            var tmp = ColorConverter.ConvertFromString(RadTreeViewBackground);
            if (tmp != null)
                RadTreeViewAttriXaml.RadTreeViewBackground = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(RadTreeViewBackgroundMouseOver);
            if (tmp != null)
                RadTreeViewAttriXaml.RadTreeViewBackgroundMouseOver = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(RadTreeViewBorderBrush);
            if (tmp != null)
                RadTreeViewAttriXaml.RadTreeViewBorderBrush = new SolidColorBrush((Color)tmp);
            
            
            tmp = ColorConverter.ConvertFromString(ItemRadTreeViewBackground);
            if (tmp != null)
                RadTreeViewAttriXaml.ItemRadTreeViewBackground = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(ItemRadTreeViewForeground);
            if (tmp != null)
                RadTreeViewAttriXaml.ItemRadTreeViewForeground = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(ItemRadTreeViewBackgroundMouseOver);
            if (tmp != null)
                RadTreeViewAttriXaml.ItemRadTreeViewBackgroundMouseOver = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(ItemRadTreeViewForegroundMouseOver);
            if (tmp != null)
                RadTreeViewAttriXaml.ItemRadTreeViewForegroundMouseOver = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(ItemRadTreeViewBackgroundSelected);
            if (tmp != null)
                RadTreeViewAttriXaml.ItemRadTreeViewBackgroundSelected = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(ItemRadTreeViewForegroundSelected);
            if (tmp != null)
                RadTreeViewAttriXaml.ItemRadTreeViewForegroundSelected = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(TitleExplandRadTreeViewBackground);
            if (tmp != null)
                RadTreeViewAttriXaml.TitleExplandRadTreeViewBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(TitleExplandRadTreeViewBackgroundMouseOver);
            if (tmp != null)
                RadTreeViewAttriXaml.TitleExplandRadTreeViewBackgroundMouseOver = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(TitleRadTreeViewBackgroundMouseOver);
            if (tmp != null)
                RadTreeViewAttriXaml.TitleRadTreeViewBackgroundMouseOver = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(TitleRadTreeViewBackground);
            if (tmp != null)
                RadTreeViewAttriXaml.TitleRadTreeViewBackground = new SolidColorBrush((Color)tmp);
            RadTreeViewAttriXaml.SaveStyle();
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
                RadTreeViewAttriXaml.SetRadTreeViewBorderThickness(obj, new Thickness(RadTreeViewBorderThickness));

                var tmp = ColorConverter.ConvertFromString(RadTreeViewBackground);
                if (tmp != null)
                    RadTreeViewAttriXaml.SetRadTreeViewBackground(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(RadTreeViewBackgroundMouseOver);
                if (tmp != null)
                    RadTreeViewAttriXaml.SetRadTreeViewBackgroundMouseOver(obj, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(RadTreeViewBorderBrush);
                if (tmp != null)
                    RadTreeViewAttriXaml.SetRadTreeViewBorderBrush(obj, new SolidColorBrush((Color)tmp));     

                tmp = ColorConverter.ConvertFromString(ItemRadTreeViewBackground);
                if (tmp != null)
                    RadTreeViewAttriXaml.SetItemRadTreeViewBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemRadTreeViewForeground);
                if (tmp != null)
                    RadTreeViewAttriXaml.SetItemRadTreeViewForeground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemRadTreeViewBackgroundMouseOver);
                if (tmp != null)
                    RadTreeViewAttriXaml.SetItemRadTreeViewBackgroundMouseOver(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemRadTreeViewForegroundMouseOver);
                if (tmp != null)
                    RadTreeViewAttriXaml.SetItemRadTreeViewForegroundMouseOver(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemRadTreeViewBackgroundSelected);
                if (tmp != null)
                    RadTreeViewAttriXaml.SetItemRadTreeViewBackgroundSelected(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemRadTreeViewForegroundSelected);
                if (tmp != null)
                    RadTreeViewAttriXaml.SetItemRadTreeViewForegroundSelected(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(TitleExplandRadTreeViewBackground);
                if (tmp != null)
                    RadTreeViewAttriXaml.SetTitleExplandRadTreeViewBackground(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(TitleExplandRadTreeViewBackgroundMouseOver);
                if (tmp != null)
                    RadTreeViewAttriXaml.SetTitleExplandRadTreeViewBackgroundMouseOver(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(TitleRadTreeViewBackgroundMouseOver);
                if (tmp != null)
                    RadTreeViewAttriXaml.SetTitleRadTreeViewBackgroundMouseOver(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(TitleRadTreeViewBackground);
                if (tmp != null)
                    RadTreeViewAttriXaml.SetTitleRadTreeViewBackground(obj, new SolidColorBrush((Color)tmp));
            }
        }
        #endregion

    }
}
