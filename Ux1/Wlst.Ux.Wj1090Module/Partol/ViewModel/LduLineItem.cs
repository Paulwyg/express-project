using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;
using Wlst.client;


namespace Wlst.Ux.Wj1090Module.Partol.ViewModel
{

    public class LduLineItem : ObservableObject
    {

        #region attri

        #region 终端名称

        private int _rtuid;

        public int RtuId
        {
            get { return _rtuid; }
            set
            {
                if (value != _rtuid)
                {

                    _rtuid = value;
                    RaisePropertyChanged(() => RtuId);
                }
            }
        }
        private int _phyid;

        public int PhyId
        {
            get { return _phyid; }
            set
            {
                if (value != _phyid)
                {

                    _phyid = value;
                    RaisePropertyChanged(() => PhyId);
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
                    RaisePropertyChanged(() => RtuName);
                }
            }
        }

        private int _coneid;

        public int ConId
        {
            get { return _coneid; }
            set
            {
                if (value != _coneid)
                {

                    _coneid = value;
                    RaisePropertyChanged(() => ConId);
                }
            }
        }

        private string _lineLoopName;

        public string ConName
        {
            get { return _lineLoopName; }
            set
            {
                if (value != _lineLoopName)
                {

                    _lineLoopName = value;
                    RaisePropertyChanged(() => ConName);
                }
            }
        }

        private int _rtuLineNameid;

        public int LineId
        {
            get { return _rtuLineNameid; }
            set
            {
                if (value != _rtuLineNameid)
                {

                    _rtuLineNameid = value;
                    RaisePropertyChanged(() => LineId);
                }
            }
        }

        private int _rtuLinePhyid;

        public int LinePhyId
        {
            get { return _rtuLinePhyid; }
            set
            {
                if (value != _rtuLinePhyid)
                {

                    _rtuLinePhyid = value;
                    RaisePropertyChanged(() => LinePhyId);
                }
            }
        }

        private string _rtuLineName;

        public string LineName
        {
            get { return _rtuLineName; }
            set
            {
                if (value != _rtuLineName)
                {

                    _rtuLineName = value;
                    RaisePropertyChanged(() => LineName);
                }
            }
        }

        public bool IsDataBack = false;

        #endregion

        #region 采样时间

        private string _dateCreate;

        public string DateCreate
        {
            get { return _dateCreate; }
            set
            {
                if (value != _dateCreate)
                {

                    _dateCreate = value;
                    RaisePropertyChanged(() => DateCreate);
                }
            }
        }

        #endregion



        #region 电压

        private double _v;

        public double V
        {
            get { return _v; }
            set
            {
                if (!_v.Equals(value))
                {

                    _v = value;
                    RaisePropertyChanged(() => V);
                }
            }
        }

        #endregion

        #region 电流

        private double _a;

        public double A
        {
            get { return _a; }
            set
            {
                if (!value.Equals(_a))
                {

                    _a = value;
                    RaisePropertyChanged(() => A);
                }
            }
        }

        #endregion

        #region 有功功率

        private double _powerActive;

        public double PowerActive
        {
            get { return _powerActive; }
            set
            {
                if (!value.Equals(_powerActive))
                {

                    _powerActive = value;
                    RaisePropertyChanged(() => PowerActive);
                }
            }
        }

        #endregion

        #region 无功功率

        private double _powerReActive;

        public double PowerReActive
        {
            get { return _powerReActive; }
            set
            {
                if (!value.Equals(_powerReActive))
                {

                    _powerReActive = value;
                    RaisePropertyChanged(() => PowerReActive);
                }
            }
        }

        #endregion

        #region 功率因数

        private double _powerFactor;

        public double PowerFactor
        {
            get { return _powerFactor; }
            set
            {
                if (value > 1 && value < 1.2) value = 1;
                if (!value.Equals(_powerFactor))
                {

                    _powerFactor = value;
                    RaisePropertyChanged(() => PowerFactor);
                }
            }
        }

        #endregion

        #region 亮灯率

        private double _brightRate;

        public double BrightRate
        {
            get { return _brightRate; }
            set
            {
                if (!value.Equals(_brightRate))
                {

                    _brightRate = value;
                    RaisePropertyChanged(() => BrightRate);
                }
            }
        }

        #endregion

        #region 信号强度

        private int _single;

        public int Single
        {
            get { return _single; }
            set
            {
                if (!value.Equals(_single))
                {

                    _single = value;
                    RaisePropertyChanged(() => Single);
                }
            }
        }

        #endregion

        #region 线路阻抗

        private int _impedance;

        public int Impedance
        {
            get { return _impedance; }
            set
            {
                if (!value.Equals(_impedance))
                {

                    _impedance = value;
                    RaisePropertyChanged(() => Impedance);
                }
            }
        }

        #endregion

        #region 12秒信号数

        private int _numOfUseFullSingleIn12Sec;

        public int NumOfUseFullSingleIn12Sec
        {
            get { return _numOfUseFullSingleIn12Sec; }
            set
            {
                if (!value.Equals(_numOfUseFullSingleIn12Sec))
                {

                    _numOfUseFullSingleIn12Sec = value;
                    RaisePropertyChanged(() => NumOfUseFullSingleIn12Sec);
                }
            }
        }

        #endregion

        #region 12秒跳变数

        private int _numOfSingleIn12Sec;

        public int NumOfSingleIn12Sec
        {
            get { return _numOfSingleIn12Sec; }
            set
            {
                if (!value.Equals(_numOfSingleIn12Sec))
                {

                    _numOfSingleIn12Sec = value;
                    RaisePropertyChanged(() => NumOfSingleIn12Sec);
                }
            }
        }

        #endregion

        #region 检测标识

        private string _flagDetection;

        public string FlagDetection
        {
            get { return _flagDetection; }
            set
            {
                if (!value.Equals(_flagDetection))
                {

                    _flagDetection = value;
                    RaisePropertyChanged(() => FlagDetection);
                }
            }
        }

        #endregion

        #region 报警标识/报警状态

        private string _flagAlarm;

        public string FlagAlarm
        {
            get { return _flagAlarm; }
            set
            {
                if (!value.Equals(_flagAlarm))
                {

                    _flagAlarm = value;
                    RaisePropertyChanged(() => FlagAlarm);
                }
            }
        }

        #endregion

        #region  报警否

        private string _isAlarm;

        public string IsAlarm
        {
            get { return _isAlarm; }
            set
            {
                if (!value.Equals(_isAlarm))
                {

                    _isAlarm = value;
                    RaisePropertyChanged(() => IsAlarm);
                }
            }
        }

        #endregion
        #region 信号数

        private string _ilSingleIn12Sec;

        public string SingleIn12Sec
        {
            get { return _ilSingleIn12Sec; }
            set
            {
                if (!value.Equals(_ilSingleIn12Sec))
                {

                    _ilSingleIn12Sec = value;
                    RaisePropertyChanged(() => SingleIn12Sec);
                }
            }
        }

        #endregion
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item">LduLineData 数据</param>
        /// 
        public void SetLduLineItemInfo(LduLineData item)
        {
            DateCreate =new DateTime( item.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
            // ResolveLduLineName(item.LineLoopId,item.RtuId);
            V = item.V;
            A = item.A;
            PowerActive = item.PowerActive;
            PowerReActive = item.PowerReActive;
            PowerFactor = item.PowerFactor;
            BrightRate = item.BrightRate;
            Single = item.Single;
            Impedance = item.Impedance;
            NumOfUseFullSingleIn12Sec = item.NumofUsefullSingleIn12Sec;
            NumOfSingleIn12Sec = item.NumofSingleIn12Sec;
            string temp = Convert.ToString(item.FlagAlarm, 2);
            if (temp.Length < 8)
            {
                int i = 0;
                while (8 - temp.Length > 0)
                {
                    temp = "0" + temp;
                    i++;
                }
            }
            FlagAlarm = temp;
            temp = Convert.ToString(item.FlagDetection, 2);
            if (temp.Length < 8)
            {
                int i = 0;
                while (8 - temp.Length > 0)
                {
                    temp = "0" + temp;
                    i++;
                }
            }

            IsAlarm = GetInfo(item.FlagAlarm, item.FlagDetection);
            SingleIn12Sec = NumOfUseFullSingleIn12Sec + "/" + NumOfSingleIn12Sec;
            FlagDetection = temp;
            IsDataBack = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fslagAlarm">上传数据</param>
        /// <param name="fslagDetection">检测数据</param>
        /// <returns></returns>
        private string GetInfo(int fslagAlarm, int fslagDetection)
        {
            string rtn = "正常";
            if ((fslagAlarm >> 3 & 1) == 1) //1 关灯 0 开灯
            {
                if ((fslagAlarm >> 6 & 1) == 1 && (fslagDetection >> 6 & 1) == 1)
                {
                    //if (tmp.Contains(42))
                    {
                        //   occrrors.Add(41);
                        rtn = "短路";
                    }
                }

                if ((fslagDetection >> 4 & 1) == 1 && (fslagDetection >> 5 & 1) == 1)
                {
                    if (((fslagAlarm >> 4 & 1) == 1 && (fslagDetection >> 4 & 1) == 1) &&
                        ((fslagAlarm >> 5 & 1) == 1 && (fslagDetection >> 5 & 1) == 1))
                    {
                        // if (tmp.Contains(41))
                        {
                            // occrrors.Add(41);
                            rtn = "被盗";
                        }
                    }
                }
                else
                {
                    if (((fslagAlarm >> 4 & 1) == 1 && (fslagDetection >> 4 & 1) == 1) ||
                        ((fslagAlarm >> 5 & 1) == 1 && (fslagDetection >> 5 & 1) == 1))
                    {
                        // if (tmp.Contains(41))
                        {
                            rtn = "被盗";
                        }
                    }
                }
            }
            else
            {
                bool checkeded = false;
                bool bolerror = false;
                if ((fslagDetection & 1) == 1)
                {
                    checkeded = true;
                    if ((fslagAlarm & 1) == 0) bolerror = true;
                }
                if ((fslagDetection >> 1 & 1) == 1)
                {
                    checkeded = true;
                    if ((fslagAlarm >> 1 & 1) == 0) bolerror = true;
                }
                if ((fslagDetection >> 2 & 1) == 1)
                {
                    checkeded = true;
                    if ((fslagAlarm >> 2 & 1) == 0) bolerror = true;
                }

                if (bolerror == false && checkeded)
                {
                    // if (tmp.Contains(41))
                    {
                        rtn = "被盗";
                    }
                }
            }
            return rtn;
        }

        public LduLineItem(int conId, int lineid)
        {
            IsDataBack = false;
            DateCreate = "--";
            IsAlarm = "--";
            ConId = conId;
            LineId = lineid;
            
            var coninfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(conId );//.GetEquipmentInfo(conId);
            if (coninfo != null)
            {
                ConName = coninfo.RtuName;
                var fatherinfo =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(coninfo.RtuFid);//.GetEquipmentInfo(coninfo.AttachRtuId);
                if (fatherinfo != null)
                {
                    RtuId = coninfo.RtuFid;
                    RtuName = fatherinfo.RtuName;
                }
            }



            var info = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(conId );//.GetEquipmentInfo(conId);
            if (info == null) return;
            var lines = info as Sr.EquipmentInfoHolding.Model.Wj1090Ldu;// Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
            if (lines == null) return;
            if (lines.WjLduLines == null) return;
            LinePhyId = lines.RtuPhyId;
            foreach (var t in lines.WjLduLines.Values )
            {
                if (t .LduLineId  == lineid)
                {
                    LineName = t.LduLineName;
                }
            }
        }

        public LduLineItem(int rtuId,int phyId, int conId, int lineid, string rtuName, string conName, string lineName,int linePhyId)
        {
            IsDataBack = false;
            DateCreate = "--";
            RtuId = rtuId;
            ConId = conId;
            LineId = lineid;
            RtuName = rtuName;
            ConName = conName;
            LineName = lineName;
            PhyId  = phyId;
            IsAlarm = "--";
            LinePhyId = linePhyId;
        }

        public static List<LduLineItem> GetLduLineItem(int conId)
        {
            var coninfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(conId);//.GetEquipmentInfo(conId);
            int rtuIds = 0;
            int rtuphyid = 0;
            string rtunames = "";
            string connames = "";
            if (coninfo != null)
            {
                connames = coninfo.RtuName;
                var fatherinfo =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(coninfo.RtuFid);//.GetEquipmentInfo(coninfo.AttachRtuId);
                if (fatherinfo != null)
                {
                    rtuIds = coninfo.RtuFid;
                    rtunames = fatherinfo.RtuName;
                    rtuphyid = fatherinfo.RtuPhyId;
                }
            }




            var lines = coninfo as Sr.EquipmentInfoHolding.Model.Wj1090Ldu;//Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
            if (lines == null) return new List<LduLineItem>();
            if (lines.WjLduLines == null) return new List<LduLineItem>();

            var rtn = new List<LduLineItem>();
            foreach (var t in lines.WjLduLines.Values)
            {
                
                if (t.IsUsed)
                    rtn.Add(new LduLineItem(rtuIds,rtuphyid , conId, t.LduLineId, rtunames, connames, t.LduLineName,lines.RtuPhyId));
            }
            return rtn;
        }



    }
}
