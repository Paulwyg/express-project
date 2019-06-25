using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.TimeTableSystem.TimeTabletemporaryView.ViewModels
{
    public class OneItemTimeTable : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private int _timeid;

        /// <summary>
        /// 时间表ID
        /// </summary>
        public int TimeId
        {
            get { return _timeid; }
            set
            {
                if (_timeid != value)
                {
                    _timeid = value;
                    this.RaisePropertyChanged(() => this.TimeId);
                }
            }
        }

        private string _timename;

        /// <summary>
        /// 时间表名称
        /// </summary>
        public string TimeName
        {
            get { return _timename; }
            set
            {
                if (_timename != value)
                {
                    _timename = value;
                    this.RaisePropertyChanged(() => this.TimeName);
                }
            }
        }

        private string _timedesc;

        /// <summary>
        /// 时间表描述
        /// </summary>
        public string TimeDesc
        {
            get { return _timedesc; }
            set
            {
                if (_timedesc != value)
                {
                    _timedesc = value;
                    this.RaisePropertyChanged(() => this.TimeDesc);
                }
            }
        }

        private bool _isSelected;
        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                }
            }
        }

        private string _ownerScheme;
        /// <summary>
        /// 归属临时方案
        /// </summary>
        public string OwnerScheme
        {
            get { return _ownerScheme; }
            set
            {
                if (_ownerScheme != value)
                {
                    _ownerScheme = value;
                    this.RaisePropertyChanged(() => this.OwnerScheme);
                }
            }
        }

        private int _sectionNumber;
        /// <summary>
        /// 时间表段数
        /// </summary>
        public int SectionNumber
        {
            get { return _sectionNumber; }
            set
            {
                if (_sectionNumber != value)
                {
                    _sectionNumber = value;
                    this.RaisePropertyChanged(() => this.SectionNumber);
                }
            }
        }

        private bool _isEnableUsed;
        /// <summary>
        /// 能否使用
        /// </summary>
        public bool IsEnableUsed
        {
            get { return _isEnableUsed; }
            set
            {
                if (_isEnableUsed != value)
                {
                    _isEnableUsed = value;
                    this.RaisePropertyChanged(() => this.IsEnableUsed);
                }
            }
        }
    }
}
