using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.client;

namespace Wlst.Ux.Wj1080Module.Wj1080InfoSet.ViewModel
{
    public  class LuxRecoredViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        public LuxRecoredViewModel ()
        {
            this.DateCreate = "--";
            this.Id = 0;
            this.
            RtuId = 0;
            this.LuxData = 0;
        }


        public LuxRecoredViewModel(LuxData.LuxDataItem info)
        {
            this.DtDateCreate = new DateTime(info.DateCreate);
            this.DateCreate = new DateTime(info.DateCreate).ToString("yyyy-MM-dd HH:mm:ss") ;
            this.Id = 0;
            this.RtuId = info.LuxId;
            this.LuxData = info.LuxValue ;
        }


        private int _id;

        /// <summary>
        /// 记录序号
        /// </summary>

        public int Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    this.RaisePropertyChanged(() => this.Id);
                }
            }
        }


        private int _rtuId;

        /// <summary>
        /// 地址
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
                }
            }
        }


        private string  _dateCreate;

        /// <summary>
        /// 时间
        /// </summary>

        public string  DateCreate
        {
            get { return _dateCreate; }
            set
            {
                if (value != _dateCreate)
                {
                    _dateCreate = value;
                    this.RaisePropertyChanged(() => this.DateCreate);
                }
            }
        }

        private DateTime _dtdateCreate;

        /// <summary>
        /// 时间
        /// </summary>

        public DateTime DtDateCreate
        {
            get { return _dtdateCreate; }
            set
            {
                if (value != _dtdateCreate)
                {
                    _dtdateCreate = value;
                    this.RaisePropertyChanged(() => this.DtDateCreate);
                }
            }
        }

        private double _luxData;

        /// <summary>
        /// 
        /// </summary>

        public double LuxData
        {
            get { return _luxData; }
            set
            {
                if (value != _luxData)
                {
                    _luxData = value;
                    this.RaisePropertyChanged(() => this.LuxData);
                }
            }
        }
    }
}
