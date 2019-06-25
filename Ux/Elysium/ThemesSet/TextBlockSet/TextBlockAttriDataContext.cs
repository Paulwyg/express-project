using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.TextBlockSet
{
    public partial class TextBlockAttriDataContext : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
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
    public partial class TextBlockAttriDataContext
    {

        #region NormalBackground

        private string _normalBackground;

        /// <summary>
        /// 
        /// </summary>
        public string NormalBackground
        {
            get { return _normalBackground; }
            set
            {
                if (_normalBackground != value)
                {
                    _normalBackground = value;
                    OnPropertyChanged("NormalBackground");
                }
            }
        }


        #endregion

        #region NormalForeground

        private string _normalForeground;

        /// <summary>
        /// 
        /// </summary>
        public string NormalForeground
        {
            get { return _normalForeground; }
            set
            {
                if (_normalForeground != value)
                {
                    _normalForeground = value;
                    OnPropertyChanged("NormalForeground");
                }
            }
        }


        #endregion

        #region MouseOverBackground

        private string _mouseOverBackground;

        /// <summary>
        /// 
        /// </summary>
        public string MouseOverBackground
        {
            get { return _mouseOverBackground; }
            set
            {
                if (_mouseOverBackground != value)
                {
                    _mouseOverBackground = value;
                    OnPropertyChanged("MouseOverBackground");
                }
            }
        }


        #endregion

        #region MouseOverForeground

        private string _mouseOverForeground;

        /// <summary>
        /// 
        /// </summary>
        public string MouseOverForeground
        {
            get { return _mouseOverForeground; }
            set
            {
                if (_mouseOverForeground != value)
                {
                    _mouseOverForeground = value;
                    OnPropertyChanged("MouseOverForeground");
                }
            }
        }


        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class TextBlockAttriDataContext
    {
        private readonly DependencyObject _obj;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public TextBlockAttriDataContext(DependencyObject obj)
        {
            _obj = obj;

            NormalBackground = TextBlockAttriXaml.NormalBackground.Color.ToString();
            NormalForeground = TextBlockAttriXaml.NormalForeground.Color.ToString(); //.Left ;
            MouseOverBackground = TextBlockAttriXaml.MouseOverBackground.Color.ToString();
            MouseOverForeground = TextBlockAttriXaml.MouseOverForeground.Color.ToString();

        }

        #region save

        private ICommand _cmdSave;

        /// <summary>
        /// 
        /// </summary>
        public ICommand CmdSave
        {
            get { return _cmdSave ?? (_cmdSave = new CommandRelay(Ex)); }
        }

        private void Ex()
        {
            var tmp = ColorConverter.ConvertFromString(NormalBackground);
            if (tmp != null)
                TextBlockAttriXaml.NormalBackground = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(NormalForeground);
            if (tmp != null)
                TextBlockAttriXaml.NormalForeground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(MouseOverBackground);
            if (tmp != null)
                TextBlockAttriXaml.MouseOverBackground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(MouseOverForeground);
            if (tmp != null)
                TextBlockAttriXaml.MouseOverForeground = new SolidColorBrush((Color)tmp);
            TextBlockAttriXaml.SaveStyle();
        }

        #endregion


        #region CmdLook

        private ICommand _cmdLook;

        /// <summary>
        /// 
        /// </summary>
        public ICommand CmdLook
        {
            get { return _cmdLook ?? (_cmdLook = new CommandRelay(ExLoop)); }
        }

        private void ExLoop()
        {
            if (_obj != null)
            {

                var tmp = ColorConverter.ConvertFromString(NormalBackground);
                if (tmp != null)
                    TextBlockAttriXaml.SetNormalBackground(_obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(NormalForeground);
                if (tmp != null)
                    TextBlockAttriXaml.SetNormalForeground(_obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(MouseOverBackground);
                if (tmp != null)
                    TextBlockAttriXaml.SetMouseOverBackground(_obj, new SolidColorBrush((Color)tmp));

                tmp = ColorConverter.ConvertFromString(MouseOverForeground);
                if (tmp != null)
                    TextBlockAttriXaml.SetMouseOverForeground(_obj, new SolidColorBrush((Color)tmp));

            }

        }

        #endregion

    }

}
