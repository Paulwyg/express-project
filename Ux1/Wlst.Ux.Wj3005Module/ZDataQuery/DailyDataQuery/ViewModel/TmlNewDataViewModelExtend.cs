using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;


namespace Wlst.Ux.WJ3005Module.ZDataQuery.DailyDataQuery.ViewModel
{
    public class TmlNewDataViewModelExtend : TmlNewDataViewModel
    {
        public TmlNewDataViewModelExtend ():base()
        {
            DataInfo = null;
        }


        public Wlst.client.TmlNewData DataInfo;

        public TmlNewDataViewModelExtend(Wlst .client .TmlNewData newDataInfo)
        {
            DataInfo = newDataInfo;
            var newData = new RtuNewDataInfo(newDataInfo);
            this.RtuId = newData.RtuId;
            //this.RtuName = newData.RtuName;
            this.RtuCurrentSumA = newData.RtuCurrentSumA.ToString("f2");
            this.RtuCurrentSumB = newData.RtuCurrentSumB.ToString("f2");
            this.RtuCurrentSumC = newData.RtuCurrentSumC.ToString("f2");
            this.RtuCurrentTotal = (newData.RtuCurrentSumA + newData.RtuCurrentSumB + newData.RtuCurrentSumC).ToString("f2");
            this.RtuVoltageA = newData.RtuVoltageA.ToString("f2");
            this.RtuVoltageB = newData.RtuVoltageB.ToString("f2");
            this.RtuVoltageC = newData.RtuVoltageC.ToString("f2");
            this.DtGetDataTime = newData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
            this.DtGetDataTimeFormat = newData.DateCreate;
            this.Temperature = newData.RtuTemperature.ToString("f0");

            double totalPower = (newData.RtuVoltageA * newData.RtuCurrentSumA + newData.RtuVoltageB * newData.RtuCurrentSumB +
                    newData.RtuVoltageC * newData.RtuCurrentSumC) / 1000;

            if (newData.RtuTemperature < -50)
            {
                this.RtuCurrentSumA = "--";
                this.RtuCurrentSumB = "--";
                this.RtuCurrentSumC = "--";
                this.RtuCurrentTotal = "--";
                this.RtuVoltageA = "--";
                this.RtuVoltageB = "--";
                this.RtuVoltageC = "--";
                this.RtuPowerSumA = "--";
                this.RtuPowerSumB = "--";
                this.RtuPowerSumC = "--";
                this.RtuPowerSum = "--";
                this.Temperature = "--";
                totalPower = 0;
            }
            int areaId = AreaInfoHold.MySlef.GetRtuBelongArea(RtuId);
            this.Area = AreaInfoHold.MySlef.GetAreaInfo(areaId).AreaName;
            int index = 1;
            foreach (var t in newData.IsSwitchOutAttraction)
            {
                this.LstIsSwitchOutAttraction.Add(new NameIntBool()  { IsSelected = t, Name = "", Value = index  });
                index++;
            }

            double PA = 0.00, PB = 0.00, PC = 0.00;
            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(RtuId))
            {
                var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[RtuId] as Wlst .Sr .EquipmentInfoHolding .Model .Wj3005Rtu ;


                foreach (var t in newData.LstNewLoopsData)
                {
                    if (info != null && (t.IsLoop && info.WjLoops.ContainsKey(t.LoopId)))
                    {
                        switch (info.WjLoops[t.LoopId].VoltagePhaseCode)
                        {
                            case Wlst.client.EnumVoltagePhase.Aphase:
                                PA += t.Power;
                                break;
                            case Wlst.client.EnumVoltagePhase.Bphase:
                                PB += t.Power;
                                break;
                            case Wlst.client.EnumVoltagePhase.Cphase:
                                PC += t.Power;
                                break;
                        }
                    }
                }
            }
            this.RtuPowerSumA = PA.ToString("f2");
            this.RtuPowerSumB = PB.ToString("f2");
            this.RtuPowerSumC = PC.ToString("f2");
            this.RtuPowerSum = (PA + PB + PC).ToString("f2");

            if (totalPower != 0)
            {
                this.RtuTotalPowerFactor = ((PA + PB + PC) / totalPower).ToString("f2");
            }
            else
            {
                this.RtuTotalPowerFactor = "--";
            }


            foreach (var t in newData.LstNewLoopsData)
            {
                //var ggg = new TmlAmpLoopViewModel(newData.RtuId, t);
                this.LstNewLoopsData.Add(new TmlAmpLoopViewModel(newData.RtuId, t));
            }






        }

        private  int _index;
        public int Index
        {
            get { return _index;
            }
            set
            {
                if(_index !=value )
                {
                    _index = value;
                    this.RaisePropertyChanged(() => this.Index);
                }
            }
        }

        
    }

    public class TmlNewDataViewModel : ObservableObject
    {


        //    public int RtuIdNeedUpdate;



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
                    if (
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .
                            ContainsKey(_rtuId))
                    {

                        this.RtuName = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems [
                            _rtuId].RtuName;
                        this.PhyIdd = string.Format("{0:D4}", Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                            _rtuId].RtuPhyId);
                    }
                }
            }
        }

        private string _phyIdd;

        /// <summary>
        /// 终端地址(表格）
        /// </summary>
        public string PhyIdd
        {
            get { return _phyIdd; }
            set
            {
                if (value != _phyIdd)
                {
                    _phyIdd = value;
                    //RtuName = "";
                    this.RaisePropertyChanged(() => this.PhyIdd);
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

        private string _area;

        public string Area
        {
            get { return _area; }
            set
            {
                if (value != _area)
                {
                    _area = value;
                    this.RaisePropertyChanged(() => this.Area);
                }
            }
        }

        /// <summary>
        /// 电压
        /// </summary>
        private string _RtuVoltageA;

        public string RtuVoltageA
        {
            get { return _RtuVoltageA; }
            set
            {
                if (value != _RtuVoltageA)
                {
                    _RtuVoltageA = value;
                    this.RaisePropertyChanged(() => this.RtuVoltageA);
                }
            }
        }

        private string _RtuVoltageB;

        /// <summary>
        /// 电压
        /// </summary>
        public string RtuVoltageB
        {
            get { return _RtuVoltageB; }
            set
            {
                if (value != _RtuVoltageB)
                {
                    _RtuVoltageB = value;
                    this.RaisePropertyChanged(() => this.RtuVoltageB);
                }
            }
        }

        private string _RtuVoltageC;

        /// <summary>
        /// 电压
        /// </summary>
        public string RtuVoltageC
        {
            get { return _RtuVoltageC; }
            set
            {
                if (value != _RtuVoltageC)
                {
                    _RtuVoltageC = value;
                    this.RaisePropertyChanged(() => this.RtuVoltageC);
                }
            }
        }

        private string _RtuCurrentSumA;

        /// <summary>
        /// 电流
        /// </summary>
        public string RtuCurrentSumA
        {
            get { return _RtuCurrentSumA; }
            set
            {
                if (value != _RtuCurrentSumA)
                {
                    _RtuCurrentSumA = value;
                    this.RaisePropertyChanged(() => this.RtuCurrentSumA);
                }
            }
        }

        private string _RtuCurrentSumB;

        /// <summary>
        /// 电流
        /// </summary>
        public string RtuCurrentSumB
        {
            get { return _RtuCurrentSumB; }
            set
            {
                if (value != _RtuCurrentSumB)
                {
                    _RtuCurrentSumB = value;
                    this.RaisePropertyChanged(() => this.RtuCurrentSumB);
                }
            }
        }

        private string _RtuCurrentSumC;

        /// <summary>
        /// 电流
        /// </summary>
        public string RtuCurrentSumC
        {
            get { return _RtuCurrentSumC; }
            set
            {
                if (value != _RtuCurrentSumC)
                {
                    _RtuCurrentSumC = value;
                    this.RaisePropertyChanged(() => this.RtuCurrentSumC);
                }
            }
        }

        private string _rtuCurrentTotal;

        /// <summary>
        /// 总电流
        /// </summary>
        public string RtuCurrentTotal
        {
            get { return _rtuCurrentTotal; }
            set
            {
                if (value != _rtuCurrentTotal)
                {
                    _rtuCurrentTotal = value;
                    this.RaisePropertyChanged(() => this.RtuCurrentTotal);
                }
            }
        }


        private string _dtGetDataTime;

        /// <summary>
        /// 客户的接收到数据的时间
        /// </summary>
        public string DtGetDataTime
        {
            get { return _dtGetDataTime; }
            set
            {
                if (value != _dtGetDataTime)
                {
                    _dtGetDataTime = value;
                    this.RaisePropertyChanged(() => this.DtGetDataTime);
                }
            }
        }

        private DateTime _dtGetDataTimeFormat;

        /// <summary>
        /// 客户的接收到数据的时间
        /// </summary>
        public DateTime DtGetDataTimeFormat
        {
            get { return _dtGetDataTimeFormat; }
            set
            {
                if (value != _dtGetDataTimeFormat)
                {
                    _dtGetDataTimeFormat = value;
                    this.RaisePropertyChanged(() => this.DtGetDataTimeFormat);
                }
            }
        }

    

        private ObservableCollection<TmlAmpLoopViewModel> _lstNewLoopsData;

        /// <summary>
        /// 回路最新数据
        /// 其回路信息存放结构为 NewDataforOneLoop
        /// </summary>
        public ObservableCollection<TmlAmpLoopViewModel> LstNewLoopsData
        {
            get
            {
                if (_lstNewLoopsData == null) _lstNewLoopsData = new ObservableCollection<TmlAmpLoopViewModel>();
                return _lstNewLoopsData;
            }
        }

        private ObservableCollection<bool> _lstIsSwithInAttraction;




        private ObservableCollection<NameIntBool> _lstIsSwitchOutAttraction;

        /// <summary>
        /// 开关量输出状态数据 是否每个开关量输出吸合连接 
        /// </summary>
        public ObservableCollection<NameIntBool> LstIsSwitchOutAttraction
        {
            get
            {

                if (_lstIsSwitchOutAttraction == null)
                    _lstIsSwitchOutAttraction = new ObservableCollection<NameIntBool>();
                return _lstIsSwitchOutAttraction;
            }
        }

        private string _temperature;

        /// <summary>
        /// 3006温度
        /// </summary>
        public string  Temperature
        {
            get { return _temperature; }
            set
            {
                if (value != _temperature)
                {
                    _temperature = value;
                    this.RaisePropertyChanged(() => this.Temperature);

                }
            }
        }

        private string _RtuPowerSumA;

        /// <summary>
        /// 功率
        /// </summary>
        public string RtuPowerSumA
        {
            get { return _RtuPowerSumA; }
            set
            {
                if (value != _RtuPowerSumA)
                {
                    _RtuPowerSumA = value;
                    this.RaisePropertyChanged(() => this.RtuPowerSumA);
                }
            }
        }

        private string _RtuPowerSumB;

        /// <summary>
        /// 功率
        /// </summary>
        public string RtuPowerSumB
        {
            get { return _RtuPowerSumB; }
            set
            {
                if (value != _RtuPowerSumB)
                {
                    _RtuPowerSumB = value;
                    this.RaisePropertyChanged(() => this.RtuPowerSumB);
                }
            }
        }

        private string _RtuPowerSumC;

        /// <summary>
        /// 功率
        /// </summary>
        public string RtuPowerSumC
        {
            get { return _RtuPowerSumC; }
            set
            {
                if (value != _RtuPowerSumC)
                {
                    _RtuPowerSumC = value;
                    this.RaisePropertyChanged(() => this.RtuPowerSumC);
                }
            }
        }

        private string _RtuPowerSum;

        /// <summary>
        /// 功率
        /// </summary>
        public string RtuPowerSum
        {
            get { return _RtuPowerSum; }
            set
            {
                if (value != _RtuPowerSum)
                {
                    _RtuPowerSum = value;
                    this.RaisePropertyChanged(() => this.RtuPowerSum);
                }
            }
        }

        private string _RtuTotalPowerFactor;

        /// <summary>
        /// 总功率因数
        /// </summary>
        public string RtuTotalPowerFactor
        {
            get { return _RtuTotalPowerFactor; }
            set
            {
                if (value != _RtuTotalPowerFactor)
                {
                    _RtuTotalPowerFactor = value;
                    this.RaisePropertyChanged(() => this.RtuTotalPowerFactor);
                }
            }
        }

    }

    public class TmlAmpLoopViewModel : ObservableObject
    {
        public TmlAmpLoopViewModel(int rtuId, RtuNewDataLoopItem tmlNewOneLoopDataInfo)
        {
            this.LoopId = tmlNewOneLoopDataInfo.LoopId;
            //this.LoopName = tmlNewOneLoopDataInfo.LoopName;
            this.A = tmlNewOneLoopDataInfo.A.ToString("f2");
            this.Power = tmlNewOneLoopDataInfo.Power+"";
            this.V = tmlNewOneLoopDataInfo.V.ToString("f2");
            this.PowerFactor = tmlNewOneLoopDataInfo.PowerFactor+"";
            this.SwitchInState = "";
            this.BrightRate = (tmlNewOneLoopDataInfo.BrightRate).ToString( "f2");
            this.LoopName = tmlNewOneLoopDataInfo.LoopName;
            this.SwitchInState = tmlNewOneLoopDataInfo.BolSwitchInState?"吸合":"断开";
        }


        private int _loopId;
        /// <summary>
        /// 回路序号
        /// </summary>
        public int LoopId
        {
            get { return _loopId; }
            set
            {
                if (value != _loopId)
                {
                    _loopId = value;
                    this.RaisePropertyChanged(() => this.LoopId);
                }
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
                if (value != _loopName)
                {
                    _loopName = value;
                    this.RaisePropertyChanged(() => this.LoopName);
                }
            }
        }

        private string _v;
        /// <summary>
        /// 回路电压  或 所代表的门啥的状态
        /// </summary>
        public string V
        {
            get { return _v; }
            set
            {
                if (value != _v)
                {
                    _v = value;
                    this.RaisePropertyChanged(() => this.V);
                }
            }
        }

        private string _a;
        /// <summary>
        /// 回路电流
        /// </summary>
        public string A
        {
            get { return _a; }
            set
            {
                if (value != _a)
                {
                    _a = value;
                    this.RaisePropertyChanged(() => this.A);
                }
            }
        }

        private string _Power;
        /// <summary>
        /// 回路有功功率
        /// </summary>
        public string Power
        {
            get { return _Power; }
            set
            {
                if (value != _Power)
                {
                    _Power = value;
                    this.RaisePropertyChanged(() => this.Power);
                }
            }
        }



        private string _PowerFactor;
        /// <summary>
        /// 回路~~~
        /// </summary>
        public string PowerFactor
        {
            get { return _PowerFactor; }
            set
            {
                if (value != _PowerFactor)
                {
                    _PowerFactor = value;
                    this.RaisePropertyChanged(() => this.PowerFactor);
                }
            }
        }

        private string _BrightRate;
        /// <summary>
        /// 亮灯率~~~
        /// </summary>
        public string BrightRate
        {
            get { return _BrightRate; }
            set
            {
                if (value != _BrightRate)
                {
                    _BrightRate = value;
                    this.RaisePropertyChanged(() => this.BrightRate);
                }
            }
        }

        private string _SwitchInState;

        /// <summary>
        /// ~~~
        /// </summary>
        public string SwitchInState
        {
            get { return _SwitchInState; }
            set
            {
                if (value != _SwitchInState)
                {
                    _SwitchInState = value;
                    this.RaisePropertyChanged(() => this.SwitchInState);
                }
            }
        }

        
    }
}
