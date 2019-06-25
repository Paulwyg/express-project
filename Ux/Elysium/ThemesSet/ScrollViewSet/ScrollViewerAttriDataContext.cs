using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.Common;

namespace Elysium.ThemesSet.ScrollViewSet
{

    public partial class ScrollViewerAttriDataContext : INotifyPropertyChanged
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
    public partial class ScrollViewerAttriDataContext
    {

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

        #region MyOpacity
        private double _myOpacity;

        public double MyOpacity
        {
            get { return _myOpacity; }
            set
            {
                if (_myOpacity == value) return;
                _myOpacity = value;
                this.OnPropertyChanged("MyOpacity");
            }
        }
        #endregion

    }

    public partial class ScrollViewerAttriDataContext
    {
        private DependencyObject obj;
        public ScrollViewerAttriDataContext(DependencyObject Scollview)
        {
            obj = Scollview;
            this.NormalBackground = ScrollViewerAttriXaml.NormalBackgrounBrush.Color.ToString();

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
                ScrollViewerAttriXaml.NormalBackgrounBrush = new SolidColorBrush((Color)tmp);

            ScrollViewerAttriXaml.MyOpacity = MyOpacity;

            ScrollViewerAttriXaml.SaveStyle();
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
                    ScrollViewerAttriXaml.SetNormalBackgrounBrush(obj, new SolidColorBrush((Color)tmp));

                ScrollViewerAttriXaml.SetMyOpacity(obj, MyOpacity);
            }
        }
        #endregion

    }
}
