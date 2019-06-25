using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreOne.Models;
using System.Collections.ObjectModel;

namespace Wlst.Ux.Wj2090Module.ZcInfo.ZcConLocalArgs.ViewModels
{
    public class LocalArgs : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        /// <summary>
        /// 序号
        /// </summary>
        #region Index
        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                if (_index == value) return;
                _index = value;
                RaisePropertyChanged(() => Index);
            }
        }
        #endregion
        /// <summary>
        /// 有效日期
        /// </summary>
        #region ValidDate
        private string _validDate;
        public string ValidDate
        {
            get { return _validDate; }
            set
            {
                if (_validDate == value) return;
                _validDate = value;
                RaisePropertyChanged(() => ValidDate);
            }
        }
        #endregion

        /// <summary>
        /// 数据类型
        /// </summary>
        #region DataMode
        private string _dataMode;
        public string DataMode
        {
            get { return _dataMode; }
            set
            {
                if (_dataMode == value) return;
                _dataMode = value;
                RaisePropertyChanged(() => DataMode);
            }
        }
        #endregion

        /// <summary>
        /// 操作类型 
        /// </summary>
        #region OperateMode
        private string _operateMode;
        public string OperateMode
        {
            get { return _operateMode; }
            set
            {
                if (_operateMode == value) return;
                _operateMode = value;
                RaisePropertyChanged(() => OperateMode);
            }
        }
        #endregion

        /// <summary>
        /// 操作参数
        /// </summary>
        #region OperateArgs
        private string _operateArgs;
        public string OperateArgs
        {
            get { return _operateArgs; }
            set
            {
                if (_operateArgs == value) return;
                _operateArgs = value;
                RaisePropertyChanged(() => OperateArgs);
            }
        }
        #endregion

        /// <summary>
        /// 日出日落
        /// </summary>
        #region Sunrise_sunset
        private string _sunrise_sunset;
        public string Sunrise_sunset
        {
            get { return _sunrise_sunset; }
            set
            {
                if (_sunrise_sunset == value) return;
                _sunrise_sunset = value;
                RaisePropertyChanged(() => Sunrise_sunset);
            }
        }
        #endregion

        /// <summary>
        /// 输出类型
        /// </summary>
        #region OutputMode
        private string _outputMode;
        public string OutputMode
        {
            get { return _outputMode; }
            set
            {
                if (_outputMode == value) return;
                _outputMode = value;
                RaisePropertyChanged(() => OutputMode);
            }
        }
        #endregion

        /// <summary>
        /// 操作回路1-4
        /// </summary>
        ObservableCollection<NameValueInt> _lampOperate;
        public ObservableCollection<NameValueInt> LampOperate
        {
            get
            {
                if (_lampOperate == null)
                {
                    _lampOperate = new ObservableCollection<NameValueInt>();
                    for (int i = 1; i <= 4; i++)
                    {
                        _lampOperate.Add(new NameValueInt() { Name = "", Value = i });
                    }
                }
                return _lampOperate;
            }
        }

    }
}
