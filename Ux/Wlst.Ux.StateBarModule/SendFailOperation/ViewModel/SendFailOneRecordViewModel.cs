using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.client;

namespace Wlst.Ux.StateBarModule.SendFailOperation.ViewModel
{
    public class SendFailOneRecordViewModel: ObservableObject
    {

        #region
        private int _index;
        /// <summary>
        /// 序号
        /// </summary>
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


        #region rtu1
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

        /// <summary>
        /// 终端名称
        /// </summary>
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

        #endregion

        #region rtu2
        private int _rtuId2;

        /// <summary>
        /// 终端地址
        /// </summary>
        public int RtuId2
        {
            get { return _rtuId2; }
            set
            {
                if (value != _rtuId2)
                {
                    _rtuId2 = value;
                    this.RaisePropertyChanged(() => this.RtuId2);

                    //todo
                }
            }
        }

        private string _rtuName2;

        /// <summary>
        /// 终端名称
        /// </summary>
        public string RtuName2
        {
            get { return _rtuName2; }
            set
            {
                if (value != _rtuName2)
                {
                    _rtuName2 = value;
                    this.RaisePropertyChanged(() => this.RtuName2);
                }
            }
        }


        private int _iphyd2;

        public int PhyId2
        {
            get { return _iphyd2; }
            set
            {
                if (_iphyd2 != value)
                {
                    _iphyd2 = value;
                    this.RaisePropertyChanged(() => this.PhyId2);
                }
            }
        }

        #endregion

        #region rtu3
        private int _rtuId3;

        /// <summary>
        /// 终端地址
        /// </summary>
        public int RtuId3
        {
            get { return _rtuId3; }
            set
            {
                if (value != _rtuId3)
                {
                    _rtuId3 = value;
                    this.RaisePropertyChanged(() => this.RtuId3);

                    //todo
                }
            }
        }

        private string _rtuName3;

        /// <summary>
        /// 终端名称
        /// </summary>
        public string RtuName3
        {
            get { return _rtuName3; }
            set
            {
                if (value != _rtuName3)
                {
                    _rtuName3 = value;
                    this.RaisePropertyChanged(() => this.RtuName3);
                }
            }
        }


        private int _iphyd3;

        public int PhyId3
        {
            get { return _iphyd3; }
            set
            {
                if (_iphyd3 != value)
                {
                    _iphyd3 = value;
                    this.RaisePropertyChanged(() => this.PhyId3);
                }
            }
        }

        #endregion

        private string _paraInfo;

        /// <summary>
        /// 参数信息
        /// </summary>
        public string ParaInfo
        {
            get { return _paraInfo; }
            set
            {
                if (value != _paraInfo)
                {
                    _paraInfo = value;
                    this.RaisePropertyChanged(() => this.ParaInfo);
                }
            }
        }

        #endregion


        //public SendFailOneRecordViewModel(Wlst.client.OperatorRecord.OperatorRecordItem item,int index)
        //{
         
        //}
    }
}
