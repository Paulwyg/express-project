using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.client;

namespace Wlst.Ux.StateBarModule.OperatorDataQueryViewModel.ViewModel
{
    public class OperatorOneRecordViewModel : ObservableObject
    {
        #region 序号

        private int _recordIndex;
        public int RecordIndex
        {
            get { return _recordIndex; }
            set
            {
                if (value == _recordIndex) return;
                _recordIndex = value;
                RaisePropertyChanged(()=>RecordIndex);
            }
        }


        #endregion

        #region Time

        private string _time;
        public string Time
        {
            get { return _time; }
            set
            {
                if (_time == value) return;
                _time = value;
                RaisePropertyChanged(()=>Time);
            }
        }

        #endregion

        #region UserName
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName == value) return;
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }
        #endregion

        #region OperatorType
        private string _operatorType;
        public string OperatorType
        {
            get { return _operatorType; }
            set
            {
                if (_operatorType == value) return;
                _operatorType = value;
                RaisePropertyChanged(() => OperatorType);
            }
        }
        #endregion

        #region UpLoadOrDownLoad
        private string _upLoadOrDownLoad;
        public string UpLoadOrDownLoad
        {
            get { return _upLoadOrDownLoad; }
            set
            {
                if (_upLoadOrDownLoad == value) return;
                _upLoadOrDownLoad = value;
                RaisePropertyChanged(() => UpLoadOrDownLoad);
            }
        }
        #endregion

        #region Addresses
        private string _addresses;
        public string Addresses
        {
            get { return _addresses; }
            set
            {
                if (_addresses == value) return;
                _addresses = value;
                RaisePropertyChanged(() => Addresses);
            }
        }
        #endregion

        #region Content
        private string _content;
        public string Content
        {
            get { return _content; }
            set
            {
                if (_content == value) return;
                _content = value;
                RaisePropertyChanged(() => Content);
            }
        }


        private string Resdfs;
        public string Remark
        {
            get { return Resdfs; }
            set
            {
                if (Resdfs == value) return;
                Resdfs = value;
                RaisePropertyChanged(() => Remark);
            }
        }
        #endregion

        #region CmdWatchDetailInfo
        private ICommand _cmdWatchDetailInfo;
        public ICommand CmdWatchDetailInfo
        {
            get {
                return _cmdWatchDetailInfo ??
                       (_cmdWatchDetailInfo = new RelayCommand(ExCmdWatchDetailInfo, CanOnCmdWatchDetailInfo, true));
            }
        }
        private void ExCmdWatchDetailInfo()
        {
            if (CmdWatchDetailInfo == null) return;
            OnCmdWatchDetailInfo(this, EventArgs.Empty);
        }
        public event EventHandler OnCmdWatchDetailInfo;
        private bool CanOnCmdWatchDetailInfo()
        {
            return true;
        }
        #endregion

        #region BtnDetailVisible
        private Visibility _btnDetailVisible;
        public Visibility BtnDetailVisible
        {
            get { return _btnDetailVisible; }
            set
            {
                if (_btnDetailVisible == value) return;
                _btnDetailVisible = value;
                RaisePropertyChanged(() => BtnDetailVisible);
            }
        }
        #endregion

        public OperatorOneRecordViewModel(Wlst .client .OperatorRecord .OperatorRecordItem item, IEnumerable<OperatorTypeItem> typeItems, int index)
        {
            RecordIndex =index;
            Time = new DateTime( item .DateCreate).ToString("yyyy-MM-dd HH:mm:ss");

            UserName = item.UserName ;
            foreach (var typeItem in typeItems)
            {
                foreach (var one in typeItem.Value)
                {
                    if(item.OperatorType  ==one.Value)
                    {
                        OperatorType = one.Name;
                        break;
                    }
                }
            }
            
            switch (item.IsClientSnd )
            {
                case 0:
                    UpLoadOrDownLoad = "下发指令-设置指令-设备应答";
                    break;
                case 1:
                    UpLoadOrDownLoad = "下发指令";
                    break;
                case 2:
                    UpLoadOrDownLoad = "设置指令";
                    break;
                case 3:
                    UpLoadOrDownLoad = "设备应答";
                    break;
                default:
                    UpLoadOrDownLoad = "";
                    break;
            }
            Addresses = "";
            foreach (var id in item.TargetLst )
            {
                Addresses += id.ToString(CultureInfo.InvariantCulture)+" ";
            }
            BtnDetailVisible = Addresses.Length>=20 ? Visibility.Visible : Visibility.Collapsed;
            Content = item.Content ;
            Remark = item.Remark ;
        }
    }


}
