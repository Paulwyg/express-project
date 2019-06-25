using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.AppleWindowSet
{
    public partial class AppleWindowAttriDataContext : INotifyPropertyChanged
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

    public partial class AppleWindowAttriDataContext
    {
        #region BorderThickness

        private double _borderThickness;

        public double BorderThickness
        {
            get { return _borderThickness; }
            set
            {
                if (_borderThickness.Equals(value)) return;
                _borderThickness = value;
                OnPropertyChanged("BorderThickness");
            }
        }


        #endregion

        #region Background

        private string _background;

        public string Background
        {
            get { return _background; }
            set
            {
                if (_background == value) return;
                _background = value;
                OnPropertyChanged("Background");
            }
        }

        #endregion

        #region Foreground

        private string _foreground;

        public string Foreground
        {
            get { return _foreground; }
            set
            {
                if (_foreground == value) return;
                _foreground = value;
                OnPropertyChanged("Foreground");
            }
        }

        #endregion

        #region HeaderBrush

        private string _headerBrush;

        public string HeaderBrush
        {
            get { return _headerBrush; }
            set
            {
                if (_headerBrush == value) return;
                _headerBrush = value;
                OnPropertyChanged("HeaderBrush");
            }
        }

        #endregion

        #region BorderBrush

        private string _borderBrush;

        public string BorderBrush
        {
            get { return _borderBrush; }
            set
            {
                if (_borderBrush == value) return;
                _borderBrush = value;
                OnPropertyChanged("BorderBrush");
            }
        }

        #endregion

    }

    public partial class AppleWindowAttriDataContext
    {
        //private DependencyObject obj;
        public AppleWindowAttriDataContext()
        {
   
            Background = AppleWindowAttriXaml.Background.Color.ToString();
            Foreground = AppleWindowAttriXaml.Foreground.Color.ToString();
            HeaderBrush = AppleWindowAttriXaml.HeaderBrush.Color.ToString();
            BorderBrush = AppleWindowAttriXaml.BorderBrush.Color.ToString();
            BorderThickness = AppleWindowAttriXaml.BorderThickness.Left;
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
            var tmp = ColorConverter.ConvertFromString(Background);
            if (tmp != null)
                AppleWindowAttriXaml.Background = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(Foreground);
            if (tmp != null)
                AppleWindowAttriXaml.Foreground = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(HeaderBrush);
            if (tmp != null)
                AppleWindowAttriXaml.HeaderBrush = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(BorderBrush);
            if (tmp != null)
                AppleWindowAttriXaml.BorderBrush = new SolidColorBrush((Color)tmp);

            AppleWindowAttriXaml.BorderThickness = new Thickness(BorderThickness);

            AppleWindowAttriXaml.SaveStyle();
        }

        #endregion


        //#region CmdLook
        //private ICommand _CmdLook;

        //public ICommand CmdLook
        //{
        //    get
        //    {

        //        if (_CmdLook == null)
        //        {
        //            _CmdLook = new CommandRelay(ExLoop);
        //        }
        //        return _CmdLook;
        //    }
        //}

        //private void ExLoop()
        //{
        //    if (obj != null)
        //    {

        //        var tmp = ColorConverter.ConvertFromString(Background);
        //        if (tmp != null)
        //            AppleWindowAttriXaml.SetBackground(obj, new SolidColorBrush((Color)tmp));

        //        tmp = ColorConverter.ConvertFromString(Foreground);
        //        if (tmp != null)
        //            AppleWindowAttriXaml.SetForeground(obj, new SolidColorBrush((Color)tmp));

        //        tmp = ColorConverter.ConvertFromString(HeaderBrush);
        //        if (tmp != null)
        //            AppleWindowAttriXaml.SetHeaderBrush(obj, new SolidColorBrush((Color)tmp));

        //        tmp = ColorConverter.ConvertFromString(BorderBrush);
        //        if (tmp != null)
        //            AppleWindowAttriXaml.SetBorderBrush(obj, new SolidColorBrush((Color)tmp));

        //        AppleWindowAttriXaml.SetBorderThickness(obj, new Thickness(BorderThickness));

        //    }
        //}
        //#endregion

    }
}
