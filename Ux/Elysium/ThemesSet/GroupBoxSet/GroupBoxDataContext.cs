using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;
using Elysium.ThemesSet.TextBoxsSet;

namespace Elysium.ThemesSet.GroupBoxSet
{
    public partial class GroupBoxAttriDataContext : INotifyPropertyChanged
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
    public partial class GroupBoxAttriDataContext
    {

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

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class GroupBoxAttriDataContext
    {
        private readonly DependencyObject _obj;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public GroupBoxAttriDataContext(DependencyObject obj)
        {
            _obj = obj;
            NormalForeground = GroupBoxAttriXaml.NormalForeground.Color.ToString();
            NormalBorderBrush = GroupBoxAttriXaml.NormalBorderBrush.Color.ToString();
            BorderThickness = GroupBoxAttriXaml.BorderThickness.Left;
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
            
            var tmp = ColorConverter.ConvertFromString(NormalForeground);
            if (tmp != null)
                GroupBoxAttriXaml.NormalForeground = new SolidColorBrush((Color)tmp);
            tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
            if (tmp != null)
                GroupBoxAttriXaml.NormalBorderBrush = new SolidColorBrush((Color)tmp);
            GroupBoxAttriXaml.BorderThickness = new Thickness(this.BorderThickness);

            GroupBoxAttriXaml.SaveStyle();
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
                var tmp = ColorConverter.ConvertFromString(NormalForeground);
                if (tmp != null)
                    GroupBoxAttriXaml.SetNormalForeground(_obj, new SolidColorBrush((Color)tmp));
                tmp = ColorConverter.ConvertFromString(this.NormalBorderBrush);
                if (tmp != null)
                    GroupBoxAttriXaml.SetNormalBorderBrush(_obj, new SolidColorBrush((Color)tmp));
                GroupBoxAttriXaml.SetBorderThickness(_obj, new Thickness(BorderThickness));
            }

        }

        #endregion

    }

}
