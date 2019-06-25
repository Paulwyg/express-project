using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreOne.Models;
using System.Collections.ObjectModel;

namespace Wlst.Ux.Wj2096Module.ZcInfo.ZcConLocalArgs.ViewModels
{
    public class LocalArgs : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        
        #region Index
        private int _index;

        /// <summary>
        /// 序号
        /// </summary>
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

        #region ValidDate
        private string _validDate;

        /// <summary>
        /// 有效日期
        /// </summary>
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


        #region DataMode
        private string _dataMode;

        /// <summary>
        /// 数据类型
        /// </summary>
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


        #region OperateMode
        private string _operateMode;

        /// <summary>
        /// 操作类型 
        /// </summary>
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

       
        #region OperateArgs
        private string _operateArgs;

        /// <summary>
        /// 操作参数
        /// </summary>
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


        #region Sunrise_sunset
        private string _sunrise_sunset;

        /// <summary>
        /// 日出日落
        /// </summary>
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


        #region OutputMode
        private string _outputMode;

        /// <summary>
        /// 输出类型
        /// </summary>
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


        ObservableCollection<NameValueInt> _lampOperate;

        /// <summary>
        /// 操作回路1-4
        /// </summary>
        public ObservableCollection<NameValueInt> LampOperate
        {
            get
            {
                if (_lampOperate == null)
                {
                    _lampOperate = new ObservableCollection<NameValueInt>();
                    for (int i = 1; i <= 4; i++)
                    {
                        _lampOperate.Add(new NameValueInt() {Name = "", Value = i});
                    }
                }
                return _lampOperate;
            }
        }

    }
}
