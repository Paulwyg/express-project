using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.DatePickerSet
{
    public partial class DatePickerAttriDataContext : INotifyPropertyChanged
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
    public partial class DatePickerAttriDataContext
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

        #region DisableForeground

        private string _disableForeground;

        public string DisableForeground
        {
            get { return _disableForeground; }
            set
            {
                if (_disableForeground != value)
                {
                    _disableForeground = value;
                    this.OnPropertyChanged("DisableForeground");
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

        #region NormalForeground
        private string _NormalForeground;

        public string NormalForeground
        {
            get { return _NormalForeground; }
            set
            {
                if (_NormalForeground == value) return;
                _NormalForeground = value;
                OnPropertyChanged("NormalForeground");
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

        #region ItemMouseOverBorderBrush
        private string _ItemMouseOverBorderBrush;

        public string ItemMouseOverBorderBrush
        {
            get { return _ItemMouseOverBorderBrush; }
            set
            {
                if (_ItemMouseOverBorderBrush == value) return;
                _ItemMouseOverBorderBrush = value;
                OnPropertyChanged("ItemMouseOverBorderBrush");
            }
        }
        #endregion

        #region ItemNormalForeground
        private string _ItemNormalForeground;

        public string ItemNormalForeground
        {
            get { return _ItemNormalForeground; }
            set
            {
                if (_ItemNormalForeground == value) return;
                _ItemNormalForeground = value;
                OnPropertyChanged("ItemNormalForeground");
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

        #region Calendar

        #region CalendarBackgrounBrush
        private string _calendarBackgrounBrush;

        public string CalendarBackgrounBrush
        {
            get { return _calendarBackgrounBrush; }
            set
            {
                if (_calendarBackgrounBrush == value) return;
                _calendarBackgrounBrush = value;
                OnPropertyChanged("CalendarBackgrounBrush");
            }
        }
        #endregion

        #region CalendarBorderBrush
        private string _calendarBorderBrush;

        public string CalendarBorderBrush
        {
            get { return _calendarBorderBrush; }
            set
            {
                if (_calendarBorderBrush == value) return;
                _calendarBorderBrush = value;
                OnPropertyChanged("CalendarBorderBrush");
            }
        }
        #endregion


        #region CalendarBorderThickness

        private double _calendarBorderThickness;

        public double CalendarBorderThickness
        {
            get { return _calendarBorderThickness; }
            set
            {
                if (_calendarBorderThickness != value)
                {
                    _calendarBorderThickness = value;
                    this.OnPropertyChanged("CalendarBorderThickness");
                }
            }
        }


        #endregion

        #endregion

        #region CalendarButton

        #region DatePickerButtonBackgrounBrush
        private string _datePickerButtonBackgrounBrush;

        public string DatePickerButtonBackgrounBrush
        {
            get { return _datePickerButtonBackgrounBrush; }
            set
            {
                if (_datePickerButtonBackgrounBrush == value) return;
                _datePickerButtonBackgrounBrush = value;
                OnPropertyChanged("DatePickerButtonBackgrounBrush");
            }
        }
        #endregion

        #region DatePickerButtonInUpBackgrounBrush
        private string _datePickerButtonInUpBackgrounBrush;

        public string DatePickerButtonInUpBackgrounBrush
        {
            get { return _datePickerButtonInUpBackgrounBrush; }
            set
            {
                if (_datePickerButtonInUpBackgrounBrush == value) return;
                _datePickerButtonInUpBackgrounBrush = value;
                OnPropertyChanged("DatePickerButtonInUpBackgrounBrush");
            }
        }
        #endregion

        #region DatePickerButtonForegrounBrush
        private string _datePickerButtonForegrounBrush;

        public string DatePickerButtonForegrounBrush
        {
            get { return _datePickerButtonForegrounBrush; }
            set
            {
                if (_datePickerButtonForegrounBrush == value) return;
                _datePickerButtonForegrounBrush = value;
                OnPropertyChanged("DatePickerButtonForegrounBrush");
            }
        }
        #endregion

        #endregion
        #region

        #endregion


    }

    public partial class DatePickerAttriDataContext
    {
        private DependencyObject obj;
        public DatePickerAttriDataContext(DependencyObject listview)
        {
            obj = listview;

            this.NormalBackground = DatePickerAttriXaml.NormalBackgrounBrush.Color.ToString();
            NormalBorderBrush = DatePickerAttriXaml.NormalBorderBrush.Color.ToString();
            this.BorderThickness = DatePickerAttriXaml.BorderThickness.Left;
            DisableBackground = DatePickerAttriXaml.DisableBackgrounBrush.Color.ToString();
            DisableBorderBrush = DatePickerAttriXaml.DisableBorderBrush.Color.ToString();
            DisableForeground = DatePickerAttriXaml.DisableForeground.Color.ToString();

            ItemNormalBackground = DatePickerAttriXaml.ItemNormalBackground.Color.ToString();
            NormalForeground = DatePickerAttriXaml.NormalForeground.Color.ToString();
            ItemNormalForeground = DatePickerAttriXaml.ItemNormalForeground.Color.ToString();
            ItemMouseOverBorderBrush = DatePickerAttriXaml.ItemMouseOverBorderBrush.Color.ToString();
            ItemBorderThickness = DatePickerAttriXaml.ItemBorderThickness.Left;
            
            ItemMouseOverBackground = DatePickerAttriXaml.ItemMouseOverBackground.Color.ToString();
            ItemNormalBorderBrush = DatePickerAttriXaml.ItemNormalBorderBrush.Color.ToString();


            ItemExpandBackground = DatePickerAttriXaml.ItemExpandBackground.Color.ToString();
            ItemExpandForeground = DatePickerAttriXaml.ItemExpandForeground.Color.ToString();
            HeaderMouseOverBackground = DatePickerAttriXaml.HeaderMouseOverBackground.Color.ToString();
            HeaderNormalBackground = DatePickerAttriXaml.HeaderNormalBackground.Color.ToString();

            CalendarBackgrounBrush = DatePickerAttriXaml.CalendarBackgrounBrush.Color.ToString();
            CalendarBorderBrush = DatePickerAttriXaml.CalendarBorderBrush.Color.ToString();
            CalendarBorderThickness = DatePickerAttriXaml.CalendarBorderThickness.Left;

            DatePickerButtonBackgrounBrush = DatePickerAttriXaml.DatePickerButtonBackgrounBrush.ToString();
            DatePickerButtonInUpBackgrounBrush = DatePickerAttriXaml.DatePickerButtonInUpBackgrounBrush.ToString();
            DatePickerButtonForegrounBrush = DatePickerAttriXaml.DatePickerButtonForegrounBrush.ToString();
            

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
                DatePickerAttriXaml.NormalBackgrounBrush = new SolidColorBrush((Color)tmp);

            DatePickerAttriXaml.BorderThickness = new Thickness(this.BorderThickness);

            tmp = ColorConverter.ConvertFromString(NormalBorderBrush);
            if (tmp != null)
                DatePickerAttriXaml.NormalBorderBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(DisableBorderBrush);
            if (tmp != null)
                DatePickerAttriXaml.DisableBorderBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(DisableBackground);
            if (tmp != null)
                DatePickerAttriXaml.DisableBackgrounBrush = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(DisableForeground);
            if (tmp != null)
                DatePickerAttriXaml.DisableForeground = new SolidColorBrush((Color)tmp);

            

            tmp = ColorConverter.ConvertFromString(ItemNormalBackground);
            if (tmp != null)
                DatePickerAttriXaml.ItemNormalBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemMouseOverBorderBrush);
            if (tmp != null)
                DatePickerAttriXaml.ItemMouseOverBorderBrush = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(NormalForeground);
            if (tmp != null)
                DatePickerAttriXaml.NormalForeground = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(ItemNormalForeground);
            if (tmp != null)
                DatePickerAttriXaml.ItemNormalForeground = new SolidColorBrush((Color)tmp);

            DatePickerAttriXaml.ItemBorderThickness = new Thickness(ItemBorderThickness);


            tmp = ColorConverter.ConvertFromString(ItemMouseOverBackground);
            if (tmp != null)
                DatePickerAttriXaml.ItemMouseOverBackground = new SolidColorBrush((Color)tmp);
         
            tmp = ColorConverter.ConvertFromString(ItemNormalBorderBrush);
            if (tmp != null)
                DatePickerAttriXaml.ItemNormalBorderBrush = new SolidColorBrush((Color)tmp);


            tmp = ColorConverter.ConvertFromString(ItemExpandBackground);
            if (tmp != null)
                DatePickerAttriXaml.ItemExpandBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(ItemExpandForeground);
            if (tmp != null)
                DatePickerAttriXaml.ItemExpandForeground = new SolidColorBrush((Color)tmp);


            tmp = ColorConverter.ConvertFromString(HeaderMouseOverBackground);
            if (tmp != null)
                DatePickerAttriXaml.HeaderMouseOverBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(HeaderNormalBackground);
            if (tmp != null)
                DatePickerAttriXaml.HeaderNormalBackground = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(CalendarBackgrounBrush);
            if (tmp != null)
                DatePickerAttriXaml.CalendarBackgrounBrush = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(CalendarBorderBrush);
            if (tmp != null)
                DatePickerAttriXaml.CalendarBorderBrush = new SolidColorBrush((Color)tmp);
            DatePickerAttriXaml.CalendarBorderThickness = new Thickness(this.CalendarBorderThickness);

            var color = ColorConverter.ConvertFromString(DatePickerButtonBackgrounBrush);
            if (color != null)
                DatePickerAttriXaml.DatePickerButtonBackgrounBrush = (Color)color;

            color = ColorConverter.ConvertFromString(DatePickerButtonInUpBackgrounBrush);
            if (color != null)
                DatePickerAttriXaml.DatePickerButtonInUpBackgrounBrush = (Color)color;
            color = ColorConverter.ConvertFromString(DatePickerButtonForegrounBrush);
            if (color != null)
                DatePickerAttriXaml.DatePickerButtonForegrounBrush = (Color)color;

            DatePickerAttriXaml.SaveStyle();
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
                    DatePickerAttriXaml.SetNormalBackgrounBrush(obj, new SolidColorBrush((Color) tmp));

                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    DatePickerAttriXaml.SetNormalBorderBrush(obj, new SolidColorBrush((Color) tmp));

                DatePickerAttriXaml.SetBorderThickness(obj, new Thickness(BorderThickness));

                tmp = ColorConverter.ConvertFromString(this.DisableBackground);
                if (tmp != null)
                    DatePickerAttriXaml.SetDisableBackgrounBrush(obj, new SolidColorBrush((Color) tmp));

                tmp = ColorConverter.ConvertFromString(this.DisableBorderBrush);
                if (tmp != null)
                    DatePickerAttriXaml.SetDisableBorderBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.DisableForeground);
                if (tmp != null)
                    DatePickerAttriXaml.SetDisableForeground(obj, new SolidColorBrush((Color)tmp));



                
                tmp = ColorConverter.ConvertFromString(ItemNormalBackground);
                if (tmp != null)
                    DatePickerAttriXaml.SetItemNormalBackground(obj, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(ItemMouseOverBorderBrush);
                if (tmp != null)
                    DatePickerAttriXaml.SetItemMouseOverBorderBrush(obj, new SolidColorBrush((Color) tmp));
                tmp = ColorConverter.ConvertFromString(NormalForeground);
                if (tmp != null)
                    DatePickerAttriXaml.SetNormalForeground(obj, new SolidColorBrush((Color) tmp));

                tmp = ColorConverter.ConvertFromString(ItemNormalForeground);
                if (tmp != null)
                    DatePickerAttriXaml.SetItemNormalForeground(obj, new SolidColorBrush((Color)tmp));

                DatePickerAttriXaml.SetItemBorderThickness(obj, new Thickness(ItemBorderThickness));


                tmp = ColorConverter.ConvertFromString(ItemMouseOverBackground);
                if (tmp != null)
                    DatePickerAttriXaml.SetItemMouseOverBackground(obj, new SolidColorBrush((Color) tmp));
               
                tmp = ColorConverter.ConvertFromString(ItemNormalBorderBrush);
                if (tmp != null)
                    DatePickerAttriXaml.SetItemNormalBorderBrush(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(ItemExpandBackground);
                if (tmp != null)
                    DatePickerAttriXaml.SetItemExpandBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(ItemExpandForeground);
                if (tmp != null)
                    DatePickerAttriXaml.SetItemExpandForeground(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(HeaderMouseOverBackground);
                if (tmp != null)
                    DatePickerAttriXaml.SetHeaderMouseOverBackground(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(HeaderNormalBackground);
                if (tmp != null)
                    DatePickerAttriXaml.SetHeaderNormalBackground(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(CalendarBackgrounBrush);
                if (tmp != null)
                    DatePickerAttriXaml.SetCalendarBackgrounBrush(obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(CalendarBorderBrush);
                if (tmp != null)
                    DatePickerAttriXaml.SetCalendarBorderBrush(obj, new SolidColorBrush((Color)tmp));
                DatePickerAttriXaml.SetCalendarBorderThickness(obj, new Thickness(CalendarBorderThickness));

                var color = ColorConverter.ConvertFromString(DatePickerButtonBackgrounBrush);
                
                if (color != null)
                    DatePickerAttriXaml.SetDatePickerButtonBackgrounBrush(obj, (Color)color);
                color = ColorConverter.ConvertFromString(DatePickerButtonInUpBackgrounBrush);
                if (color != null)
                    DatePickerAttriXaml.SetDatePickerButtonInUpBackgrounBrush(obj, (Color)color);
                color = ColorConverter.ConvertFromString(DatePickerButtonForegrounBrush);
                if (color != null)
                    DatePickerAttriXaml.SetDatePickerButtonForegrounBrush(obj, (Color)color);

            }
        }
        #endregion

    }
}
