using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.LabelSet
{
    public partial class LabelAttriDataContext : INotifyPropertyChanged
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

    public partial class LabelAttriDataContext
    {

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

    }

    public partial class LabelAttriDataContext
    {
        //private DependencyObject obj;
        public LabelAttriDataContext()
        {
            Foreground = LabelAttriXaml.Foreground.Color.ToString();
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
            var tmp = ColorConverter.ConvertFromString(Foreground);
            if (tmp != null)
                LabelAttriXaml.Foreground = new SolidColorBrush((Color)tmp);

            LabelAttriXaml.SaveStyle();
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
