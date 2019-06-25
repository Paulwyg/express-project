using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Statistics.UxStatistics.ViewModel
{
    public class Data
    {
        private DateTime _timeStamp;
        private double _value;
        private int _value1;
        private int _rfid;
        private string _category;
        private int _isRtu;
        private string _date;

        //public Data(DateTime timeStamp, double value)
        //{
        //    this._timeStamp = timeStamp;
        //    this._value = value;
        //}
          
        public Data(string category, double value,int rfid = 0, int isRtu= 0,string date =null )
        {
            this._category = category;
            this._value = value;
            this._rfid = rfid;
            this._isRtu = isRtu;
            this._date = date;
        }

        public Data(string category, int value1, int rfid = 0, int isRtu = 0, string date = null)
        {
            this._category = category;
            this._value = value1;
            this._rfid = rfid;
            this._isRtu = isRtu;
            this._date = date;
        }

        //public DateTime TimeStamp
        //{
        //    get
        //    {
        //        return this._timeStamp;
        //    }
        //    set
        //    {
        //        if (this._timeStamp != value)
        //        {
        //            this._timeStamp = value;
        //        }
        //    }
        //}

        public string Category
        {
            get
            {
                return this._category;
            }
            set
            {
                if (this._category != value)
                {
                    this._category = value;
                }
            }
        }

        public double Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if (this._value != value)
                {
                    this._value = value;
                }
            }
        }

        public int Value1
        {
            get
            {
                return this._value1;
            }
            set
            {
                if (this._value1 != value)
                {
                    this._value1 = value;
                }
            }
        }

        public int Rfid
        {
            get
            {
                return this._rfid;
            }
            set
            {
                if (this._rfid != value)
                {
                    this._rfid = value;
                }
            }
        }

        public int IsRtu
        {
            get
            {
                return this._isRtu;
            }
            set
            {
                if (this._isRtu != value)
                {
                    this._isRtu = value;
                }
            }
        }

        public string Date
        {
            get
            {
                return this._date;
            }
            set
            {
                if (this._date != value)
                {
                    this._date = value;
                }
            }
        }
    }
}
