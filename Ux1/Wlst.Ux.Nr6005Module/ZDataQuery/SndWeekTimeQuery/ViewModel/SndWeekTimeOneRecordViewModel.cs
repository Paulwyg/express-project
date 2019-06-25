using System;
using System.Globalization;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.Nr6005Module.ZDataQuery.SndWeekTimeQuery.ViewModel
{
    public class SndWeekTimeOneRecordViewModel : ObservableObject
    {
       private int _recordIndex;

        /// <summary>
        /// 记录编号  顺序
        /// </summary>
        public int RecordIndex
        {
            get { return _recordIndex; }
            set
            {
                if (value != _recordIndex)
                {
                    _recordIndex = value;
                    this.RaisePropertyChanged(() => this.RecordIndex);
                }
            }
        }
        #region
 

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
                    // _rtuId = value;

                    RtuName = "" + value;
                    if (
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                            InfoItems .ContainsKey(value))
                    {
                        var info =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                                InfoItems [value];
                        this.RtuName = info.RtuName;
                        _rtuId = info.RtuPhyId ;
                    }
                    else
                    {
                        _rtuId = value;
                    }
                    this.RaisePropertyChanged(() => this.RtuId);
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

        private string _SndTime;

        /// <summary>
        /// 发送周设置时间
        /// </summary>
        public string SndTime
        {
            get { return _SndTime; }
            set
            {
                if (value != _SndTime)
                {
                    _SndTime = value;
                    this.RaisePropertyChanged(() => this.SndTime);
                }
            }
        }

        private string _K1K3RcvTime;

        /// <summary>
        /// 回路一到回路三收到应答时间
        /// </summary>
        public string RcvTime
        {
            get { return _K1K3RcvTime; }
            set
            {
                if (value != _K1K3RcvTime)
                {
                    _K1K3RcvTime = value;
                    this.RaisePropertyChanged(() => this.RcvTime);
                }
            }
        }


        private string _UserName;

        /// <summary>
        /// 回路一到回路三收到应答时间
        /// </summary>
        public string UserName
        {
            get { return _UserName; }
            set
            {
                if (value != _UserName)
                {
                    _UserName = value;
                    this.RaisePropertyChanged(() => this.UserName);
                }
            }
        }

        private string _K4K6RcvTime;

        /// <summary>
        /// 本条记录为K1K3 还是K4K6
        /// </summary>
        public string RecordType
        {
            get { return _K4K6RcvTime; }
            set
            {
                if (value != _K4K6RcvTime)
                {
                    _K4K6RcvTime = value;
                    this.RaisePropertyChanged(() => this.RecordType);
                }
            }
        }
        #endregion


        public SndWeekTimeOneRecordViewModel(Wlst.client.RecordWeekTime.RecordWeekTimeItem record, int id)
        {
            this.RecordIndex = id;
            this.RtuId = record.RtuId;
            //  Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetPhysicalIdByLogicalId(record.RtuId);// record.RtuId;
            this.SndTime = new DateTime(record.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
                //string.Format("{0:G}", new DateTime(record.DateCreate));
            this.RcvTime = record.DateReply == 0
                               ? "未应答"
                               : new DateTime(record.DateReply).ToString("yyyy-MM-dd HH:mm:ss");
            if (record.WeekTimeTypex > 100)
            {
                //if (record.WeekTimeTypex.ToString("d8").Length == 8)
                //{
                //    if (record.WeekTimeTypex.ToString("d8").Substring(7, 1) == "1")
                //        this.RecordType = record.WeekTimeTypex.ToString("d8").Substring(0, 6) + "上旬";
                //    else if (record.WeekTimeTypex.ToString("d8").Substring(7, 1) == "2")
                //        this.RecordType = record.WeekTimeTypex.ToString("d8").Substring(0, 6) + "中旬";
                //    else if (record.WeekTimeTypex.ToString("d8").Substring(7, 1) == "3")
                //        this.RecordType = record.WeekTimeTypex.ToString("d8").Substring(0, 6) + "下旬";
                //}
                //else
                //{
                    this.RecordType = record.WeekTimeTypex + "";
                //}
            }
            else
            {
                if (record.WeekTimeTypex == 13)
                    this.RecordType = "K1-K3";
                else if (record.WeekTimeTypex == 46) this.RecordType = "K4-K6";
                else if (record.WeekTimeTypex == 78) this.RecordType = "K7-K8";
                else if (record.WeekTimeTypex == 14) this.RecordType = "S1-S4(假)";
                else if (record.WeekTimeTypex == 58) this.RecordType = "S5-S8(假)";
                
            }
            this.UserName = record.UserName;
        }




    }
}
