using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel
{
    public class WlstMessageBoxViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }


        private string _title;
        string _message;
        string _innerMessageDetails;

        Visibility _yesNoVisibility;
        Visibility _cancelVisibility;
        Visibility _okVisibility;
        Visibility _closeVisibility;
        Visibility _showDetails;


        ICommand _noCommand;
        ICommand _cancelCommand;
        ICommand _closeCommand;
        ICommand _okCommand;
        ICommand _btnCloseWindows;
        private readonly View.WlstMessageBox _view;

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChange("Title");
                }
            }
        }
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    NotifyPropertyChange("Message");
                }
            }
        }
        public string InnerMessageDetails
        {
            get { return _innerMessageDetails; }
            set
            {
                if (_innerMessageDetails != value)
                {
                    _innerMessageDetails = value;
                    NotifyPropertyChange("InnerMessageDetails");
                }
            }
        }
        public Visibility YesNoVisibility
        {
            get { return _yesNoVisibility; }
            set
            {
                if (_yesNoVisibility != value)
                {
                    _yesNoVisibility = value;
                    NotifyPropertyChange("YesNoVisibility");
                }
            }
        }

        public Visibility CancelVisibility
        {
            get { return _cancelVisibility; }
            set
            {
                if (_cancelVisibility != value)
                {
                    _cancelVisibility = value;
                    NotifyPropertyChange("CancelVisibility");
                }
            }
        }

        public Visibility OkVisibility
        {
            get { return _okVisibility; }
            set
            {
                if (_okVisibility != value)
                {
                    _okVisibility = value;
                    NotifyPropertyChange("OkVisibility");
                }
            }
        }

        public Visibility CloseVisibility
        {
            get { return _closeVisibility; }
            set
            {
                if (_closeVisibility != value)
                {
                    _closeVisibility = value;
                    NotifyPropertyChange("CloseVisibility");
                }
            }
        }

        public Visibility ShowDetails
        {
            get { return _showDetails; }
            set
            {
                if (_showDetails != value)
                {
                    _showDetails = value;
                    NotifyPropertyChange("ShowDetails");
                }
            }
        }
        private ICommand _yesCommand;
        public ICommand YesCommand
        {

            get { return _yesCommand ?? (_yesCommand = new DelegateCommand(YesExecuteMethod)); }

        }
        private void YesExecuteMethod()
        {
            _view.Result = WlstMessageBoxResults.Yes;
            _view.Close();
        }

        public ICommand NoCommand
        {
            get { return _noCommand ?? (_noCommand = new DelegateCommand(NoExecuteMethod)); }
        }
        public void NoExecuteMethod()
        {
            _view.Result = WlstMessageBoxResults.No;
            
            _view.Close();
        }
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new DelegateCommand(() =>
                {
                    _view.Result =
                        WlstMessageBoxResults.Cancel;
                    _view.Close();
                }));
            }
        }


        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new DelegateCommand(() =>
                {
                    _view.Result =
                        WlstMessageBoxResults.Close;
                    _view.Close();
                }));
            }
        }

        public ICommand OkCommand
        {
            get
            {
                return _okCommand ?? (_okCommand = new DelegateCommand(() =>
                {
                    _view.Result = WlstMessageBoxResults.Ok;
                    _view.Close();
                }));
            }
        }
        public ICommand BtnCloseWindows
        {
            get { return _btnCloseWindows ?? (_btnCloseWindows = new DelegateCommand(BtnCloseWindowsMethod)); }
        }
        private void BtnCloseWindowsMethod()
        {
            _view.Close();
        }
        public WlstMessageBoxViewModel(View.WlstMessageBox view, string title, string message, string innerMessage, WlstMessageBoxType style)
        {
            Title = title;
            Message = message;
            InnerMessageDetails = innerMessage;
            SetWpfMessageBoxStyle(style);
            _view = view;

        }
        private void SetWpfMessageBoxStyle(WlstMessageBoxType style)
        {
            switch (style)
            {
                case WlstMessageBoxType.YesNo:
                    OkVisibility = CancelVisibility = CloseVisibility = Visibility.Collapsed;
                    break;
                case WlstMessageBoxType.YesNoCancel:
                    OkVisibility = CloseVisibility = Visibility.Collapsed;
                    break;
                case WlstMessageBoxType.Ok:
                    YesNoVisibility = CancelVisibility = CloseVisibility = Visibility.Collapsed;
                    break;
                case WlstMessageBoxType.OkClose:
                    YesNoVisibility = CancelVisibility = Visibility.Collapsed;
                    break;
                case WlstMessageBoxType.OkCancel:
                    YesNoVisibility = CloseVisibility = Visibility.Collapsed;
                    break;
                default:
                    OkVisibility = CancelVisibility = YesNoVisibility = Visibility.Collapsed;
                    break;
            }
            ShowDetails = string.IsNullOrEmpty(InnerMessageDetails) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
