using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.AppleWindowSet;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.MessageBoxOverrideSet
{
    public partial class MessageBoxOverrideAttriDataContext : INotifyPropertyChanged
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

    public partial class MessageBoxOverrideAttriDataContext
    {

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

    }

    public partial class MessageBoxOverrideAttriDataContext
    {
        //private DependencyObject obj;
        public MessageBoxOverrideAttriDataContext()
        {

            Background = MessageBoxOverrideAttriXaml.Background.Color.ToString();
            HeaderBrush = MessageBoxOverrideAttriXaml.HeaderBrush.Color.ToString();
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
                MessageBoxOverrideAttriXaml.Background = new SolidColorBrush((Color)tmp);

            tmp = ColorConverter.ConvertFromString(HeaderBrush);
            if (tmp != null)
                MessageBoxOverrideAttriXaml.HeaderBrush = new SolidColorBrush((Color)tmp);

            MessageBoxOverrideAttriXaml.SaveStyle();
        }

        #endregion

    }
}
