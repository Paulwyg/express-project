using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Wlst.Ux.Nr6005Module.ZDataQuery.RtuOpenCloseLightQuery.Services
{
   public  class RtuOpenCloseItemThree : Wlst.Cr.Core.CoreServices.ObservableObject
    {
       public RtuOpenCloseItemThree (int rutid)
       {
           this.RtuId = rutid;
           for (int i = 0; i < 17; i++) Time.Add("--");
           //Time1 = "--";
           //Time2 = "--";
           //Time3 = "--";
           //Time4 = "--";
           //Time5 = "--";
           //Time6 = "--";
       }

        private int _index;
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                if (_index != value)
                {
                    _index = value;
                    this.RaisePropertyChanged(() => this.Index);
                }
            }
        }
        /// <summary>
        /// 终端地址
        /// </summary>
        private int rtu_id;

        public int RtuId
        {
            get { return rtu_id; }
            set
            {
                if (rtu_id != value)
                {
                    rtu_id = value;
                    PhyId = value;
                    this.RaisePropertyChanged(() => this.RtuId);


                    if (
                       !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                            InfoItems .ContainsKey
                            (rtu_id))
                        return;
                    var tml =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems 
                            [rtu_id];
                    this.RtuName = tml.RtuName;
                    PhyId =tml.RtuPhyId ;
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
        /// <summary>
        /// 终端地址
        /// </summary>
        private string rtu_name;

        public string RtuName
        {
            get { return rtu_name; }
            set
            {
                if (rtu_name != value)
                {
                    rtu_name = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        private ObservableCollection<string> _time;

        public ObservableCollection<string> Time
        {
            get
            {

                if (_time == null)
                    _time = new ObservableCollection<string>();
                return _time;
            }
        }
        ///// <summary>
        ///// 计算出来的时间
        ///// </summary>
        //private string _time1;

        //public string Time1
        //{
        //    get { return _time1; }
        //    set
        //    {
        //        if (_time1 != value)
        //        {
        //            _time1 = value;
        //            this.RaisePropertyChanged(() => this.Time1);
        //        }
        //    }
        //}



        ///// <summary>
        ///// 计算出来的时间
        ///// </summary>
        //private string _time2;

        //public string Time2
        //{
        //    get { return _time2; }
        //    set
        //    {
        //        if (_time2 != value)
        //        {
        //            _time2 = value;
        //            this.RaisePropertyChanged(() => this.Time2);
        //        }
        //    }
        //}


        ///// <summary>
        ///// 计算出来的时间
        ///// </summary>
        //private string _time3;

        //public string Time3
        //{
        //    get { return _time3; }
        //    set
        //    {
        //        if (_time3 != value)
        //        {
        //            _time3 = value;
        //            this.RaisePropertyChanged(() => this.Time3);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 计算出来的时间
        ///// </summary>
        //private string _time4;

        //public string Time4
        //{
        //    get { return _time4; }
        //    set
        //    {
        //        if (_time4 != value)
        //        {
        //            _time4 = value;
        //            this.RaisePropertyChanged(() => this.Time4);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 计算出来的时间
        ///// </summary>
        //private string _time5;

        //public string Time5
        //{
        //    get { return _time5; }
        //    set
        //    {
        //        if (_time5 != value)
        //        {
        //            _time5 = value;
        //            this.RaisePropertyChanged(() => this.Time5);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 计算出来的时间
        ///// </summary>
        //private string _time6;

        //public string Time6
        //{
        //    get { return _time6; }
        //    set
        //    {
        //        if (_time6 != value)
        //        {
        //            _time6 = value;
        //            this.RaisePropertyChanged(() => this.Time6);
        //        }
        //    }
        //}
   
   }
}
