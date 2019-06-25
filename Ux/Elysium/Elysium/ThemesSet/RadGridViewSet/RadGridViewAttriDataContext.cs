using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.RadGridViewSet
{

    public class Model
    {
        public int RecordIndex { get; set; }
        public string Time { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }


    }
    public partial class RadGridViewAttriDataContext : INotifyPropertyChanged
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
    public partial class RadGridViewAttriDataContext
    {
        #region Data
        private List<Model> _items;
        public List<Model> Items
        {
            get { return _items ?? (_items = new List<Model>()); }
        }
        #endregion

        #region 内容面板及其边框

        #region RadGridViewBorderThickness

        private double _radGridViewBorderThickness;

        public double RadGridViewBorderThickness
        {
            get { return _radGridViewBorderThickness; }
            set
            {
                if (_radGridViewBorderThickness != value)
                {
                    _radGridViewBorderThickness = value;
                    this.OnPropertyChanged("RadGridViewBorderThickness");
                }
            }
        }


        #endregion

        #region RadGridViewBackground

        private string _radGridViewBackground;

        public string RadGridViewBackground
        {
            get { return _radGridViewBackground; }
            set
            {
                if (_radGridViewBackground != value)
                {
                    _radGridViewBackground = value;
                    this.OnPropertyChanged("RadGridViewBackground");
                }
            }
        }


        #endregion

        #region RadGridViewBorderBrush

        private string _radGridViewBorderBrush;

        public string RadGridViewBorderBrush
        {
            get { return _radGridViewBorderBrush; }
            set
            {
                if (_radGridViewBorderBrush != value)
                {
                    _radGridViewBorderBrush = value;
                    this.OnPropertyChanged("RadGridViewBorderBrush");
                }
            }
        }


        #endregion

        #region RadGridViewForeground

        private string _radGridViewForeground;

        public string RadGridViewForeground
        {
            get { return _radGridViewForeground; }
            set
            {
                if (_radGridViewForeground != value)
                {
                    _radGridViewForeground = value;
                    this.OnPropertyChanged("RadGridViewForeground");
                }
            }
        }


        #endregion

        #endregion

        #region 指示器

         #region SortIndicatorAscBrush

        private string _sortIndicatorAscBrush;

        public string SortIndicatorAscBrush
        {
            get { return _sortIndicatorAscBrush; }
            set
            {
                if (_sortIndicatorAscBrush != value)
                {
                    _sortIndicatorAscBrush = value;
                    this.OnPropertyChanged("SortIndicatorAscBrush");
                }
            }
        }


        #endregion
        
        #region SortIndicatorDesBrush

        private string _sortIndicatorDesBrush;

        public string SortIndicatorDesBrush
        {
            get { return _sortIndicatorDesBrush; }
            set
            {
                if (_sortIndicatorDesBrush != value)
                {
                    _sortIndicatorDesBrush = value;
                    OnPropertyChanged("SortIndicatorDesBrush");
                }
            }
        }


        #endregion
            
            
        #endregion

        #region 表头
            #region TitleNormalBackground

            private string _titleNormalBackground;

            public string TitleNormalBackground
            {
                get { return _titleNormalBackground; }
                set
                {
                    if (_titleNormalBackground != value)
                    {
                        _titleNormalBackground = value;
                        OnPropertyChanged("TitleNormalBackground");
                    }
                }
            }


            #endregion

            #region TitleCellMouseOverBrush

            private string _titleCellMouseOverBrush;

            public string TitleCellMouseOverBrush
            {
                get { return _titleCellMouseOverBrush; }
                set
                {
                    if (_titleCellMouseOverBrush != value)
                    {
                        _titleCellMouseOverBrush = value;
                        OnPropertyChanged("TitleCellMouseOverBrush");
                    }
                }
            }


        #endregion

            #region TitleCellSelectedBrush

            private string _titleCellSelectedBrush;

            public string TitleCellSelectedBrush
            {
                get { return _titleCellSelectedBrush; }
                set
                {
                    if (_titleCellSelectedBrush != value)
                    {
                        _titleCellSelectedBrush = value;
                        OnPropertyChanged("TitleCellSelectedBrush");
                    }
                }
            }


            #endregion
            #region TitleNormalForeground

            private string _titleNormalForeground;

            public string TitleNormalForeground
            {
                get { return _titleNormalForeground; }
                set
                {
                    if (_titleNormalForeground != value)
                    {
                        _titleNormalForeground = value;
                        OnPropertyChanged("TitleNormalForeground");
                    }
                }
            }


            #endregion
            #region TitleNormalForegroundSelected

            private string _titleNormalForegroundSelected;

            public string TitleNormalForegroundSelected
            {
                get { return _titleNormalForegroundSelected; }
                set
                {
                    if (_titleNormalForegroundSelected != value)
                    {
                        _titleNormalForegroundSelected = value;
                        OnPropertyChanged("TitleNormalForegroundSelected");
                    }
                }
            }


            #endregion
            
        #endregion

        #region Item
            #region ItemBackground

            private string _itemBackground;

            public string ItemBackground
            {
                get { return _itemBackground; }
                set
                {
                    if (_itemBackground != value)
                    {
                        _itemBackground = value;
                        OnPropertyChanged("ItemBackground");
                    }
                }
            }


            #endregion

            #region ItemBackgroundMouseOver

            private string _itemBackgroundMouseOver;

            public string ItemBackgroundMouseOver
            {
                get { return _itemBackgroundMouseOver; }
                set
                {
                    if (_itemBackgroundMouseOver != value)
                    {
                        _itemBackgroundMouseOver = value;
                        OnPropertyChanged("ItemBackgroundMouseOver");
                    }
                }
            }


            #endregion

            #region ItemBackgroundSelected

            private string _itemBackgroundSelected;

            public string ItemBackgroundSelected
            {
                get { return _itemBackgroundSelected; }
                set
                {
                    if (_itemBackgroundSelected != value)
                    {
                        _itemBackgroundSelected = value;
                        OnPropertyChanged("ItemBackgroundSelected");
                    }
                }
            }


            #endregion
        #endregion


    }

    public partial class RadGridViewAttriDataContext
    {
        private DependencyObject obj;
        public RadGridViewAttriDataContext(DependencyObject listview)
        {
            #region Data
            for (int i = 0; i < 20; i++)
            {
                Items.Add(new Model { RecordIndex = i, Time = "ss", UserName = "ssss", Content = "msls" });
            }
            #endregion

            obj = listview;
            RadGridViewBorderThickness = RadGridViewAttriXaml.RadGridViewBorderThickness.Left;
            RadGridViewBackground = RadGridViewAttriXaml.RadGridViewBackground.Color.ToString();
            RadGridViewBorderBrush = RadGridViewAttriXaml.RadGridViewBorderBrush.Color.ToString();
            RadGridViewForeground = RadGridViewAttriXaml.RadGridViewForeground.Color.ToString();
            SortIndicatorAscBrush=RadGridViewAttriXaml.SortIndicatorAscBrush.Color.ToString();
            SortIndicatorDesBrush=RadGridViewAttriXaml.SortIndicatorDesBrush.Color.ToString();
            TitleNormalBackground = RadGridViewAttriXaml.TitleNormalBackground.Color.ToString();
            TitleCellMouseOverBrush = RadGridViewAttriXaml.TitleCellMouseOverBrush.Color.ToString();
            TitleCellSelectedBrush = RadGridViewAttriXaml.TitleCellSelectedBrush.Color.ToString();
            TitleNormalForeground = RadGridViewAttriXaml.TitleNormalForeground.Color.ToString();
            TitleNormalForegroundSelected = RadGridViewAttriXaml.TitleNormalForegroundSelected.Color.ToString();

            ItemBackground = RadGridViewAttriXaml.ItemBackground.Color.ToString();
            ItemBackgroundMouseOver = RadGridViewAttriXaml.ItemBackgroundMouseOver.Color.ToString();
            ItemBackgroundSelected = RadGridViewAttriXaml.ItemBackgroundSelected.Color.ToString();
        }
        #region save
        private ICommand _cmdSave;

        public ICommand CmdSave
        {
            get { return _cmdSave ?? (_cmdSave = new CommandRelay(Ex)); }
        }

        private void Ex()
        {

            RadGridViewAttriXaml.RadGridViewBorderThickness = new Thickness(this.RadGridViewBorderThickness);
            var tmp = ColorConverter.ConvertFromString(this.RadGridViewBackground);
            if (tmp != null)
                RadGridViewAttriXaml.RadGridViewBackground = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(RadGridViewBorderBrush);
            if (tmp != null)
                RadGridViewAttriXaml.RadGridViewBorderBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(RadGridViewForeground);
            if (tmp != null)
                RadGridViewAttriXaml.RadGridViewForeground = new SolidColorBrush((Color)tmp);

             tmp = ColorConverter.ConvertFromString(SortIndicatorAscBrush);
            if (tmp != null)
                RadGridViewAttriXaml.SortIndicatorAscBrush = new SolidColorBrush((Color)tmp);

             tmp = ColorConverter.ConvertFromString(SortIndicatorDesBrush);
            if (tmp != null)
                RadGridViewAttriXaml.SortIndicatorDesBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(TitleNormalBackground);
            if (tmp != null)
                RadGridViewAttriXaml.TitleNormalBackground = new SolidColorBrush((Color)tmp);

             tmp = ColorConverter.ConvertFromString(TitleCellMouseOverBrush);
            if (tmp != null)
                RadGridViewAttriXaml.TitleCellMouseOverBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(TitleCellSelectedBrush);
            if (tmp != null)
                RadGridViewAttriXaml.TitleCellSelectedBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(TitleNormalForeground);
            if (tmp != null)
                RadGridViewAttriXaml.TitleNormalForeground = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(TitleNormalForegroundSelected);
            if (tmp != null)
                RadGridViewAttriXaml.TitleNormalForegroundSelected = new SolidColorBrush((Color)tmp);


            
            tmp = ColorConverter.ConvertFromString(ItemBackground);
            if (tmp != null)
                RadGridViewAttriXaml.ItemBackground = new SolidColorBrush((Color)tmp);
              
            tmp = ColorConverter.ConvertFromString(ItemBackgroundMouseOver);
            if (tmp != null)
                RadGridViewAttriXaml.ItemBackgroundMouseOver = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(ItemBackgroundSelected);
            if (tmp != null)
                RadGridViewAttriXaml.ItemBackgroundSelected = new SolidColorBrush((Color)tmp);
                
                    
                    

            RadGridViewAttriXaml.SaveStyle();
            
                
                
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
                RadGridViewAttriXaml.SetRadGridViewBorderThickness(obj, new Thickness(RadGridViewBorderThickness));

                var tmp = ColorConverter.ConvertFromString(this.RadGridViewBackground);
                if (tmp != null)
                    RadGridViewAttriXaml.SetRadGridViewBackground(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(this.RadGridViewBorderBrush);
                if (tmp != null)
                    RadGridViewAttriXaml.SetRadGridViewBorderBrush(obj, new SolidColorBrush((Color)tmp));


                tmp = ColorConverter.ConvertFromString(this.RadGridViewForeground);
                if (tmp != null)
                    RadGridViewAttriXaml.SetRadGridViewForeground(obj, new SolidColorBrush((Color)tmp));

                                tmp = ColorConverter.ConvertFromString(this.SortIndicatorAscBrush);
                if (tmp != null)
                    RadGridViewAttriXaml.SetSortIndicatorAscBrush(obj, new SolidColorBrush((Color)tmp));
                                tmp = ColorConverter.ConvertFromString(this.SortIndicatorDesBrush);
                if (tmp != null)
                    RadGridViewAttriXaml.SetSortIndicatorDesBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.TitleNormalBackground);
                if (tmp != null)
                    RadGridViewAttriXaml.SetTitleNormalBackground(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(this.TitleCellSelectedBrush);
                if (tmp != null)
                    RadGridViewAttriXaml.SetTitleCellSelectedBrush(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(this.TitleCellMouseOverBrush);
                if (tmp != null)
                    RadGridViewAttriXaml.SetTitleCellMouseOverBrush(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(TitleNormalForeground);
                if (tmp != null)
                    RadGridViewAttriXaml.SetTitleNormalForeground(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(TitleNormalForegroundSelected);
                if (tmp != null)
                    RadGridViewAttriXaml.SetTitleNormalForegroundSelected(obj, new SolidColorBrush((Color)tmp));

                
                

                tmp = ColorConverter.ConvertFromString(this.ItemBackground);
                if (tmp != null)
                    RadGridViewAttriXaml.SetItemBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemBackgroundMouseOver);
                if (tmp != null)
                    RadGridViewAttriXaml.SetItemBackgroundMouseOver(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemBackgroundSelected);
                if (tmp != null)
                    RadGridViewAttriXaml.SetItemBackgroundSelected(obj, new SolidColorBrush((Color)tmp));
                
                      
                
                    
            }
        }
        #endregion

    }
}