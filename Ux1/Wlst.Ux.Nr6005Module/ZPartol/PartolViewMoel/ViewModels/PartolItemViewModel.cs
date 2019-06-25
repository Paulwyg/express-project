using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Nr6005Module.ZPartol.PartolViewMoel.ViewModels
{

    /// <summary>
    /// 巡测时 一个终端的数据vm
    /// </summary>
    public class PartolItemViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public const long OneSecond = 10000000;
        private const int SwitchOutLoopSum = 8;

        public long DateCreateTime;

        public int AlarmCount;

        /// <summary>
        /// 电压
        /// </summary>
        public double RtuVoltageA;

        /// <summary>
        /// 电压
        /// </summary>
        public double RtuVoltageB;

        /// <summary>
        /// 电压
        /// </summary>
        public double RtuVoltageC;

        /// <summary>
        /// 电流
        /// </summary>
        public Double RtuCurrentSumA;

        /// <summary>
        /// 电流
        /// </summary>
        public Double RtuCurrentSumB;

        /// <summary>
        /// 电流
        /// </summary>
        public Double RtuCurrentSumC;

        #region data

        /// <summary>
        /// 开关量输出路数 最多支持16路 默认6路
        /// </summary>

        private int _rtuId;

        private string _rtuName;
        private string _timeSpan;

        /// <summary>
        /// 终端序号
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (_rtuId == value) return;
                _rtuId = value;
                this.RaisePropertyChanged(() => this.RtuId);
            }
        }    private int _Reqerror;
        /// <summary>
        /// 请求终端最新数据时间
        /// </summary>
        public int ErrorCount
        {
            get { return _Reqerror; }
            set
            {
                if (_Reqerror == value) return;
                _Reqerror = value;
                this.RaisePropertyChanged(() => this.ErrorCount);
            }
        }

        private int _physicalId;
        public int PhysicalId
        {
            get { return _physicalId; }
            set
            {
                if(_physicalId==value) return;
                _physicalId = value;
                RaisePropertyChanged(()=>PhysicalId);
            }
        }
        /// <summary>
        /// 终端名称 设置好地址后会自动查找
        /// </summary>
        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (_rtuName == value) return;
                _rtuName = value;
                this.RaisePropertyChanged(() => this.RtuName);
            }
        }


        private string _grpname;
        /// <summary>
        /// 终端名称 设置好地址后会自动查找
        /// </summary>
        public string GrpName
        {
            get { return _grpname; }
            set
            {
                if (_grpname == value) return;
                _grpname = value;
                this.RaisePropertyChanged(() => this.GrpName);
            }
        }

        /// <summary>
        ///  数据 0、不用,1、 停运，2、 使用等
        /// </summary>
        public int RtuStateInt = 0;
        public void OnRtudataArvUpState(int state)
        {
            if (state != RtuStateInt)
            {
                string str1 = RtuState;
                string str2 = state == 2 ? "使用" : "停运";
                RtuState = "设置:" + str1 + " 实际:" + str2;
            }
        }

        private string _rtuState;
        /// <summary>
        /// 终端名称 设置好地址后会自动查找
        /// </summary>
        public string RtuState
        {
            get { return _rtuState; }
            set
            {
                if (_rtuState == value) return;
                _rtuState = value;
                this.RaisePropertyChanged(() => this.RtuState);
            }
        }

        private string _RequestNewDataTime;

        /// <summary>
        /// 请求终端最新数据时间
        /// </summary>
        public string RequestNewDataTime
        {
            get { return _RequestNewDataTime; }
            set
            {
                if (_RequestNewDataTime == value) return;
                _RequestNewDataTime = value;
                this.RaisePropertyChanged(() => this.RequestNewDataTime);
            }
        }



        private string _ReceiveNewDataTime;
        //需要显示的接收最新数据时间
        public string ReceiveNewDataTime
        {
            get { return _ReceiveNewDataTime; }
            set
            {
                if (_ReceiveNewDataTime == value) return;
                _ReceiveNewDataTime = value;
                this.RaisePropertyChanged(() => this.ReceiveNewDataTime);
            }
        }


        private string _ssssssssssss;
        //是否在线
        public string OnLine
        {
            get { return _ssssssssssss; }
            set
            {
                if (_ssssssssssss == value) return;
                _ssssssssssss = value;
                this.RaisePropertyChanged(() => this.OnLine);
            }
        }

        /// <summary>
        /// 发送与接收时间长
        /// </summary>
        public string TimeSpan
        {
            get { return _timeSpan; }
             set
            {
                if (_timeSpan == value) return;
                _timeSpan = value;
                this.RaisePropertyChanged(() => this.TimeSpan);
            }
        }


        private ObservableCollection<NameIntBool> _switchOutState;

        /// <summary>
        /// 回路是否开启 IsSelected==true 开启 Value为回路编号 1为输出1 
        /// </summary>
        public ObservableCollection<NameIntBool> SwitchOutState
        {
            get
            {
                if (_switchOutState == null)
                {
                    _switchOutState = new ObservableCollection<NameIntBool>();
                    for (int i = 1; i < SwitchOutLoopSum + 1; i++)
                    {
                        _switchOutState.Add(new NameIntBool() {IsSelected = false, Name = "", Value = i});
                    }
                }
                return _switchOutState;
            }
        }

        private string _rtuPowerSumA;
        //功率
        public string RtuPowerSumA
        {
            get { return _rtuPowerSumA; }
            set
            {
                if (_rtuPowerSumA == value) return;
                _rtuPowerSumA = value;
                this.RaisePropertyChanged(() => this.RtuPowerSumA);
            }
        }
        
        private string _rtuPowerSumB;
        //功率
        public string RtuPowerSumB
        {
            get { return _rtuPowerSumB; }
            set
            {
                if (_rtuPowerSumB == value) return;
                _rtuPowerSumB = value;
                this.RaisePropertyChanged(() => this.RtuPowerSumB);
            }
        }
       
        private string _rtuPowerSumC;
        //功率
        public string RtuPowerSumC
        {
            get { return _rtuPowerSumC; }
            set
            {
                if (_rtuPowerSumC == value) return;
                _rtuPowerSumC = value;
                this.RaisePropertyChanged(() => this.RtuPowerSumC);
            }
        } 
      
        private string _rtuPowerSum;
        //功率
        public string RtuPowerSum
        {
            get { return _rtuPowerSum; }
            set
            {
                if (_rtuPowerSum == value) return;
                _rtuPowerSum = value;
                this.RaisePropertyChanged(() => this.RtuPowerSum);
            }
        }


        #endregion


        /// <summary>
        /// 上一次手动发送选测数据时间
        /// </summary>
        private long _lastdatasndtime;

        public PartolItemViewModel()
        {
            RequestNewDataTime = "--";
            ReceiveNewDataTime = "--";
            TimeSpan = "--";
            OnLine = "--";

        }


        /// <summary>
        /// 设置开关量输出吸合状态
        /// </summary>
        /// <param name="state">开关量1~16</param>
        public void SetSwitchOutState(List<bool> state)
        {
            for (int i = 1; i < state.Count() + 1; i++)
            {
                foreach (var t in SwitchOutState)
                {
                    if (t.Value == i) t.IsSelected = state[i - 1];
                }
            }
        }





        ////E:\CETC50\Programs\CETC50_Demo\BasicFunctionModule\Resource\on.bmp

        //public void SetRcvEquipmentNewDataInfo(Sr.EquipmentNewData.Model.RtuNewDataInfo newdata)
        //{

        //    if (newdata == null) return;
        //    if (newdata.RtuId != RtuId) return;
        //    _lastdatarcvtime = DateTime.Now.Ticks;

        //    ReceiveNewDataTime = newdata.DateCreate.ToString(CultureInfo.InvariantCulture);

        //    for (int i = 0; i < newdata.IsSwitchOutAttraction.Count; i++)
        //    {
        //        SetSwitchOutState(i + 1, newdata.IsSwitchOutAttraction[i]);
        //    }

        //    if (_lastdatasndtime == 0) TimeSpan = "--";
        //    else
        //    {
        //        var span = (_lastdatarcvtime - _lastdatasndtime)/OneSecond;
        //        TimeSpan = span + " 秒";

        //    }
        //}

        public void SetOnLine(bool isOnLine)
        {
            this.OnLine = isOnLine ? "√" : "--";
        }


    }

    public class ShieldItemViewModel:Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private int _rtuId;
        /// <summary>
        /// 终端id
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (_rtuId == value) return;
                _rtuId = value;
                this.RaisePropertyChanged(() => this.RtuId);
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
                if (_rtuName == value) return;
                _rtuName = value;
                this.RaisePropertyChanged(() => this.RtuName);
            }
        }

        private int _loopId;
        /// <summary>
        /// 回路id
        /// </summary>
        public int LoopId
        {
            get { return _loopId; }
            set
            {
                if (_loopId == value) return;
                _loopId = value;
                this.RaisePropertyChanged(() => this.LoopId);
            }
        }

        private string _loopName;
        /// <summary>
        /// 回路名称
        /// </summary>
        public string LoopName
        {
            get { return _loopName; }
            set
            {
                if (_loopName == value) return;
                _loopName = value;
                this.RaisePropertyChanged(() => this.LoopName);
            }
        }

        private string _v;
        /// <summary>
        /// 电压
        /// </summary>
        public string V
        {
            get { return _v; }
            set
            {
                if (_v == value) return;
                _v = value;
                this.RaisePropertyChanged(() => this.V);
            }
        }

        private string _a;
        /// <summary>
        /// 电流
        /// </summary>
        public string A
        {
            get { return _a; }
            set
            {
                if (_a == value) return;
                _a = value;
                this.RaisePropertyChanged(() => this.A);
            }
        }

        private string _power;
        /// <summary>
        /// 功率
        /// </summary>
        public string Power
        {
            get { return _power; }
            set
            {
                if (_power == value) return;
                _power = value;
                this.RaisePropertyChanged(() => this.Power);
            }
        }
    }

}
