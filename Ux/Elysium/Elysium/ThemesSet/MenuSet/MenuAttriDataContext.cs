using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.MenuSet
{
    public partial class MenuAttriDataContext : INotifyPropertyChanged
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
    public partial class MenuAttriDataContext
    {
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

        #region NormalItemBackground

        private string _normalItemBackground;

        public string NormalItemBackground
        {
            get { return _normalItemBackground; }
            set
            {
                if (_normalItemBackground != value)
                {
                    _normalItemBackground = value;
                    this.OnPropertyChanged("NormalItemBackground");
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


        #region MouseOverItemBackground

        private string _mouseOverItemBackground;

        public string MouseOverItemBackground
        {
            get { return _mouseOverItemBackground; }
            set
            {
                if (_mouseOverItemBackground != value)
                {
                    _mouseOverItemBackground = value;
                    this.OnPropertyChanged("MouseOverItemBackground");
                }
            }
        }


        #endregion

        #region MouseOverForeground

        private string _mouseOverForeground;

        public string MouseOverForeground
        {
            get { return _mouseOverForeground; }
            set
            {
                if (_mouseOverForeground != value)
                {
                    _mouseOverForeground = value;
                    this.OnPropertyChanged("MouseOverForeground");
                }
            }
        }


        #endregion

        #region DisableItemBackground

        private string _disableItemBackground;

        public string DisableItemBackground
        {
            get { return _disableItemBackground; }
            set
            {
                if (_disableItemBackground != value)
                {
                    _disableItemBackground = value;
                    this.OnPropertyChanged("DisableItemBackground");
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

    }

    public partial class MenuAttriDataContext
    {
        private DependencyObject obj;
        public MenuAttriDataContext(DependencyObject menu)
        {
            obj = menu;
            this.NormalBackground = MenuAttriXaml.NormalBackgrounBrush.Color.ToString();
            NormalItemBackground = MenuAttriXaml.NormalItemBackgrounBrush.Color.ToString();
            NormalForeground = MenuAttriXaml.NormalForegrounBrush.Color.ToString();
            NormalBorderBrush = MenuAttriXaml.NormalBorderBrush.Color.ToString();
            MouseOverItemBackground = MenuAttriXaml.MouseOverItemBackgrounBrush.Color.ToString();
            MouseOverForeground = MenuAttriXaml.MouseOverForegrounBrush.Color.ToString();

            DisableForeground = MenuAttriXaml.DisableForegrounBrush.Color.ToString();
            DisableItemBackground = MenuAttriXaml.DisableItemBackgrounBrush.Color.ToString();

            this.BorderThickness = MenuAttriXaml.BorderThickness.Left;
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
                MenuAttriXaml.NormalBackgrounBrush = new SolidColorBrush((Color)tmp);

             tmp = ColorConverter.ConvertFromString(NormalItemBackground);
            if (tmp != null)
                MenuAttriXaml.NormalItemBackgrounBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(NormalForeground);
            if(tmp!=null)
                MenuAttriXaml.NormalForegrounBrush=new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(NormalBorderBrush);
            if (tmp != null)
                MenuAttriXaml.NormalBorderBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(MouseOverItemBackground);
            if (tmp != null)
                MenuAttriXaml.MouseOverItemBackgrounBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(MouseOverForeground);
            if (tmp != null)
                MenuAttriXaml.MouseOverForegrounBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(DisableItemBackground);
            if (tmp != null)
                MenuAttriXaml.DisableItemBackgrounBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(DisableForeground);
            if (tmp != null)
                MenuAttriXaml.DisableForegrounBrush = new SolidColorBrush((Color)tmp);

            MenuAttriXaml.BorderThickness = new Thickness(this.BorderThickness);
            MenuAttriXaml.SaveStyle();
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
            if (obj != null)
            {

                var tmp = ColorConverter.ConvertFromString(this.NormalBackground);
                if (tmp != null)
                    MenuAttriXaml.SetNormalBackgrounBrush(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(NormalItemBackground);
                if (tmp != null)
                    MenuAttriXaml.SetNormalItemBackgrounBrush(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(this.NormalForeground);
                if (tmp != null)
                    MenuAttriXaml.SetNormalForegrounBrush(obj, new SolidColorBrush((Color)tmp));

                //tmp = ColorConverter.ConvertFromString(this.NormalForeground);
                //if (tmp != null)
                //    MenuAttriXaml.SetNormalForegrounBrush(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    MenuAttriXaml.SetNormalBorderBrush(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(MouseOverItemBackground);
                if (tmp != null)
                    MenuAttriXaml.SetMouseOverItemBackgrounBrush(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(MouseOverForeground);
                if (tmp != null)
                    MenuAttriXaml.SetMouseOverForegrounBrush(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(DisableItemBackground);
                if (tmp != null)
                    MenuAttriXaml.SetDisableItemBackgrounBrush(obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(DisableForeground);
                if (tmp != null)
                    MenuAttriXaml.SetDisableForegrounBrush(obj, new SolidColorBrush((Color)tmp));

                MenuAttriXaml.SetBorderThickness(obj,new Thickness(BorderThickness));

            }
        }
        #endregion

    }
}
