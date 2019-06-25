using System;
using System.Collections.Generic;
using System.Globalization;
using Wlst.Cr.Core.CoreServices;
using Wlst.Sr.ProtocolCnt.RecordEvent.Models;

namespace Wlst.Ux.EquipmentDataQuery.RecordEventQueryViewModel.ViewModel
{
    public class OneRecordEventViewModel : ObservableObject
    {
        #region attri

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


        private string _SndTime;

        /// <summary>
        /// 操作事件
        /// </summary>
        public string CreateTime
        {
            get { return _SndTime; }
            set
            {
                if (value != _SndTime)
                {
                    _SndTime = value;
                    this.RaisePropertyChanged(() => this.CreateTime);
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
                    RtuName = "" + _rtuId;
                    if (
                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.
                            EquipmentInfoDictionary .ContainsKey(_rtuId))
                    {
                        var info =
                            Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.
                                EquipmentInfoDictionary [_rtuId];
                            this.RtuName = info.RtuName;
                        
                    }
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

        private string _loop;

        /// <summary>
        /// 终端名称
        /// </summary>
        public string Loop
        {
            get { return _loop; }
            set
            {
                if (value != _loop)
                {
                    _loop = value;
                    this.RaisePropertyChanged(() => this.Loop);
                }
            }
        }


        private string _K1K3RcvTime;

        /// <summary>
        /// 操作名称
        /// </summary>
        public string Operator
        {
            get { return _K1K3RcvTime; }
            set
            {
                if (value != _K1K3RcvTime)
                {
                    _K1K3RcvTime = value;
                    this.RaisePropertyChanged(() => this.Operator);
                }
            }
        }

        private string _K4K6RcvTime;

        /// <summary>
        /// 操作者
        /// </summary>
        public string Operatorer
        {
            get { return _K4K6RcvTime; }
            set
            {
                if (value != _K4K6RcvTime)
                {
                    _K4K6RcvTime = value;
                    this.RaisePropertyChanged(() => this.Operatorer);
                }
            }
        }

        #endregion


        public static List<OneRecordEventViewModel> GetOneRecordVm(RecordEvent record)
        {
            List<OneRecordEventViewModel> lstRtn = new List<OneRecordEventViewModel>();
            List<int> ids = new List<int>();
            var sp = record.DeviceIds.Split(';');
            foreach (var t in sp)
            {
                try
                {
                    int x = Convert.ToInt32(t);
                    if (x > 0) ids.Add(x);
                }
                catch (Exception ex)
                {
                }
            }
            string operatorname = GetOperatorName(record.OperatorType);
            string createtime = new DateTime(record.DateCreate).ToString(CultureInfo.InvariantCulture);
            foreach (var t in ids)
            {
                var tmp = new OneRecordEventViewModel()
                              {
                                  CreateTime = createtime,
                                  Operatorer = record.UserName,
                                  RecordIndex = record.EventId,
                                  Operator = operatorname,
                                  RtuId = t,
                                  Loop = record.LoopId == 0 ? "-" : record.LoopId.ToString(CultureInfo.InvariantCulture)
                              };
                lstRtn.Add(tmp);

            }
            return lstRtn;
        }


        public static string GetOperatorName(int operatorid)
        {
            switch (operatorid)
            {
                case 1:
                    return "手动开灯";
                case 2:
                    return "自动开掉";
                case 3:
                    return "手动关灯";
                case 4:
                    return "自动关灯";
                case 5:
                    return "抄表";
                case 6:
                    return "发送周设置K1K3";
                case 7:
                    return "发送州设置K4K6[自动]";
                case 8:
                    return "发送时间";
                case 9:
                    return "更新时间表";
                case 10:
                    return "更新分组设置";
                case 11:
                    return "设置终端参数";
                case 12:
                    return "设置自定义故障";
                case 13:
                    return "设置故障与终端绑定";
                case 14:
                    return "召测终端参数";
                case 15:
                    return "召测周设置";
                case 16:
                    return "用户登陆";
            }
            return "未知操作";
        }
    }


}
