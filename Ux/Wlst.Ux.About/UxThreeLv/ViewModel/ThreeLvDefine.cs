using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.About.UxThreeLv.ViewModel
{
    public class ThreeLvDefine : ObservableObject
    {




        private int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                if (_index != value)
                {
                    _index = value;
                    this.RaisePropertyChanged(() => this.Index);
                }
            }
        }

        private string _dtCreateTime;

        public string DtCreateTime
        {
            get { return _dtCreateTime; }
            set
            {
                if (_dtCreateTime != value)
                {
                    _dtCreateTime = value;
                    this.RaisePropertyChanged(() => this.DtCreateTime);
                }
            }
        }

        private int _iphyd;

        public int PhyId
        {
            get { return _iphyd; }
            set
            {
                if (_iphyd != value)
                {
                    _iphyd = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }


        private int _rtuId;

        /// <summary>
        /// 终端地址
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value != _rtuId)
                {
                    _rtuId = value;
                    this.RaisePropertyChanged(() => this.RtuId);
                    //todo
                }
            }
        }

        private string _rtuName;

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        private string _zdt;

        public string ZDT
        {
            get { return _zdt; }
            set
            {
                if (value != _zdt)
                {
                    _zdt = value;
                    this.RaisePropertyChanged(() => this.ZDT);
                }
            }
        }

        private string _Count;

        public string Count
        {
            get { return _Count; }
            set
            {
                if (value != _Count)
                {
                    _Count = value;
                    this.RaisePropertyChanged(() => this.Count);
                }
            }
        }

        private string _Ratio;

        public string Ratio
        {
            get { return _Ratio; }
            set
            {
                if (value != _Ratio)
                {
                    _Ratio = value;
                    this.RaisePropertyChanged(() => this.Ratio);
                }
            }
        }

        private string _loopname;

        public string LoopName
        {
            get { return _loopname; }
            set
            {
                if (value != _loopname)
                {
                    _loopname = value;
                    this.RaisePropertyChanged(() => this.LoopName);
                }
            }
        }

        private string _power;

        public string Power
        {
            get { return _power; }
            set
            {
                if (value != _power)
                {
                    _power = value;
                    this.RaisePropertyChanged(() => this.Power);
                }
            }
        }

    }
}
