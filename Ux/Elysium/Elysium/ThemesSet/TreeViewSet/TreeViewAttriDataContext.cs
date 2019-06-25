using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.TreeViewSet
{
    public partial class TreeViewAttriDataContext : INotifyPropertyChanged
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
    public partial class TreeViewAttriDataContext
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
        #region ItemExpandBackground
        private string _itemExpandBackground;

        public string ItemExpandBackground
        {
            get { return _itemExpandBackground; }
            set
            {
                if (_itemExpandBackground == value) return;
                _itemExpandBackground = value;
                OnPropertyChanged("ItemExpandBackground");
            }
        }
        #endregion
        #region ItemExpandForeground
        private string _itemExpandForeground;

        public string ItemExpandForeground
        {
            get { return _itemExpandForeground; }
            set
            {
                if (_itemExpandForeground == value) return;
                _itemExpandForeground = value;
                OnPropertyChanged("ItemExpandForeground");
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
        #endregion


    }

    public partial class TreeViewAttriDataContext
    {
        private DependencyObject obj;
        public TreeViewAttriDataContext(DependencyObject listview)
        {
            obj = listview;

            this.NormalBackground = TreeViewAttriXaml.NormalBackgrounBrush.Color.ToString();
            NormalBorderBrush = TreeViewAttriXaml.NormalBorderBrush.Color.ToString();
            this.BorderThickness = TreeViewAttriXaml.BorderThickness.Left;
            DisableBackground = TreeViewAttriXaml.DisableBackgrounBrush.Color.ToString();
            DisableBorderBrush = TreeViewAttriXaml.DisableBorderBrush.Color.ToString();

            ItemNormalBackground = TreeViewAttriXaml.ItemNormalBackground.Color.ToString();
            ItemNormalForeground = TreeViewAttriXaml.ItemNormalForeground.Color.ToString();
            ItemNormalBorderBrush = TreeViewAttriXaml.ItemNormalBorderBrush.Color.ToString();
            ItemBorderThickness = TreeViewAttriXaml.ItemBorderThickness.Left;

            ItemMouseOverBackground = TreeViewAttriXaml.ItemMouseOverBackground.Color.ToString();
            ItemMouseOverForeground = TreeViewAttriXaml.ItemMouseOverForeground.Color.ToString();
            ItemSelectedBackground = TreeViewAttriXaml.ItemSelectedBackground.Color.ToString();
            ItemSelectedForeground = TreeViewAttriXaml.ItemSelectedForeground.Color.ToString();


            ItemExpandBackground = TreeViewAttriXaml.ItemExpandBackground.Color.ToString();
            ItemExpandForeground = TreeViewAttriXaml.ItemExpandForeground.Color.ToString();
            HeaderMouseOverBackground = TreeViewAttriXaml.HeaderMouseOverBackground.Color.ToString();
            HeaderNormalBackground = TreeViewAttriXaml.HeaderNormalBackground.Color.ToString();

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
                TreeViewAttriXaml.NormalBackgrounBrush = new SolidColorBrush((Color)tmp);

            TreeViewAttriXaml.BorderThickness = new Thickness(this.BorderThickness);

            tmp = ColorConverter.ConvertFromString(NormalBorderBrush);
            if (tmp != null)
                TreeViewAttriXaml.NormalBorderBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(DisableBorderBrush);
            if (tmp != null)
                TreeViewAttriXaml.DisableBorderBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(DisableBackground);
            if (tmp != null)
                TreeViewAttriXaml.DisableBackgrounBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(ItemNormalBackground);
            if (tmp != null)
                TreeViewAttriXaml.ItemNormalBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemNormalBorderBrush);
            if (tmp != null)
                TreeViewAttriXaml.ItemNormalBorderBrush = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemNormalForeground);
            if (tmp != null)
                TreeViewAttriXaml.ItemNormalForeground = new SolidColorBrush((Color)tmp);

            TreeViewAttriXaml.ItemBorderThickness = new Thickness(ItemBorderThickness);


            tmp = ColorConverter.ConvertFromString(ItemMouseOverBackground);
            if (tmp != null)
                TreeViewAttriXaml.ItemMouseOverBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemMouseOverForeground);
            if (tmp != null)
                TreeViewAttriXaml.ItemMouseOverForeground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemSelectedBackground);
            if (tmp != null)
                TreeViewAttriXaml.ItemSelectedBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemSelectedForeground);
            if (tmp != null)
                TreeViewAttriXaml.ItemSelectedForeground = new SolidColorBrush((Color)tmp);


            tmp = ColorConverter.ConvertFromString(ItemExpandBackground);
            if (tmp != null)
                TreeViewAttriXaml.ItemExpandBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemExpandForeground);
            if (tmp != null)
                TreeViewAttriXaml.ItemExpandForeground = new SolidColorBrush((Color)tmp);


            tmp = ColorConverter.ConvertFromString(HeaderMouseOverBackground);
            if (tmp != null)
                TreeViewAttriXaml.HeaderMouseOverBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(HeaderNormalBackground);
            if (tmp != null)
                TreeViewAttriXaml.HeaderNormalBackground = new SolidColorBrush((Color)tmp);

            TreeViewAttriXaml.SaveStyle();
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
                    TreeViewAttriXaml.SetNormalBackgrounBrush(obj, new SolidColorBrush((Color) tmp));

                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    TreeViewAttriXaml.SetNormalBorderBrush(obj, new SolidColorBrush((Color) tmp));

                TreeViewAttriXaml.SetBorderThickness(obj, new Thickness(BorderThickness));

                tmp = ColorConverter.ConvertFromString(this.DisableBackground);
                if (tmp != null)
                    TreeViewAttriXaml.SetDisableBackgrounBrush(obj, new SolidColorBrush((Color) tmp));

                tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
                if (tmp != null)
                    TreeViewAttriXaml.SetDisableBorderBrush(obj, new SolidColorBrush((Color) tmp));


                tmp = ColorConverter.ConvertFromString(ItemNormalBackground);
                if (tmp != null)
                    TreeViewAttriXaml.SetItemNormalBackground(obj, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(ItemNormalBorderBrush);
                if (tmp != null)
                    TreeViewAttriXaml.SetItemNormalBorderBrush(obj, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(ItemNormalForeground);
                if (tmp != null)
                    TreeViewAttriXaml.SetItemNormalForeground(obj, new SolidColorBrush((Color) tmp));
                TreeViewAttriXaml.SetItemBorderThickness(obj, new Thickness(ItemBorderThickness));


                tmp = ColorConverter.ConvertFromString(ItemMouseOverBackground);
                if (tmp != null)
                    TreeViewAttriXaml.SetItemMouseOverBackground(obj, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(ItemMouseOverForeground);
                if (tmp != null)
                    TreeViewAttriXaml.SetItemMouseOverForeground(obj, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(ItemSelectedBackground);
                if (tmp != null)
                    TreeViewAttriXaml.SetItemSelectedBackground(obj, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(ItemSelectedForeground);
                if (tmp != null)
                    TreeViewAttriXaml.SetItemSelectedForeground(obj, new SolidColorBrush((Color) tmp));

                tmp = ColorConverter.ConvertFromString(ItemExpandBackground);
                if (tmp != null)
                    TreeViewAttriXaml.SetItemExpandBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemExpandForeground);
                if (tmp != null)
                    TreeViewAttriXaml.SetItemExpandForeground(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(HeaderMouseOverBackground);
                if (tmp != null)
                    TreeViewAttriXaml.SetHeaderMouseOverBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderNormalBackground);
                if (tmp != null)
                    TreeViewAttriXaml.SetHeaderNormalBackground(obj, new SolidColorBrush((Color)tmp));
               

            }
        }
        #endregion

    }
}
