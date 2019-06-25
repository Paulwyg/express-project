using System;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;

namespace Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.ViewModel
{
    public class FrmSelectTimeTableViewModel : Services.FrmSelectTimeTableViewModelBase
    {
        #region if group data
        private bool _isGroup;
        /// <summary>
        /// 当前设置的是否为分组
        /// </summary>
        public bool IsGroup
        {
            get { return _isGroup; }
            set
            {
                if (value != _isGroup)
                {
                    _isGroup = value;
                    RaisePropertyChanged(() => IsGroup);
                    IsAllChildApplyThisTimeTableVisi = value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }




        private Visibility _isAllChildApplyThisTimeTableVisi;
        /// <summary>
        /// 如果为组的话  是否当前选择的时间表应用到该分组下的所有递归终端
        /// </summary>
        public Visibility IsAllChildApplyThisTimeTableVisi
        {
            get { return _isAllChildApplyThisTimeTableVisi; }
            set
            {
                if (value != _isAllChildApplyThisTimeTableVisi)
                {
                    _isAllChildApplyThisTimeTableVisi = value;
                    RaisePropertyChanged(() => IsAllChildApplyThisTimeTableVisi);
                }
            }
        }

        private int  _isAllChildApplyThisTimeTable;

        /// <summary>
        ///  1; 组下正常
        ///  2组下所有正常的终端            递归正常
        /// 3; //组下所有终端  一级终端  包括特殊终端
        ///4; //组下所有递归分组下的 所有终端 包括特殊终端
        /// </summary>
        public int ApplyRtusType
        {
            get { return _isAllChildApplyThisTimeTable; }
            set
            {
                if (value != _isAllChildApplyThisTimeTable)
                {
                    _isAllChildApplyThisTimeTable = value;
                    RaisePropertyChanged(() => this.ApplyRtusType);
                }
            }
        }


        #endregion

        #region cmd save

        private DateTime _dtSave;
        private ICommand _cmdSave;

        public ICommand CmdSave
        {
            get { return _cmdSave ?? (_cmdSave = new RelayCommand(Ex, CanEx, true)); }
        }

        public event EventHandler OnBtnOkClick;
        void Ex()
        {
            _dtSave = DateTime.Now;
            if (OnBtnOkClick != null)
            {
                OnBtnOkClick(this, new EventArgs());
            }
        }
        bool CanEx()
        {
            if (CurrentSelectItem == null) return false;
            if (OldSelectTimeTableId != CurrentSelectItem.TimeTableId)
            {
                return DateTime.Now.Ticks-_dtSave.Ticks>30000000;
            }
            return false;
        }
        #endregion
    }
}
