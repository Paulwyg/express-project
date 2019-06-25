using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.client;

namespace Wlst.Ux.Wj2090Module.HisDataQuery.DataMining2.ViewModel
{

    public class DataLampItemMining : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public DataLampItemMining(int sluId, int ctrId, int lampId, int index)
        {
            Index = index;
            SluId = sluId;
            CtrlId = Wj2090Module.Services.CommonSlu.GetPhyIdByCtrl(SluId, ctrId ); 
            LampId  = lampId;
        }

        public DataLampItemMining(SluOrCtrlData.DataSluCtrlLampEx tt, int index)
        {
            Index = index;
            CtrlId  = Wj2090Module.Services.CommonSlu.GetPhyIdByCtrl(tt.SluId, tt.CtrlId );  
            SluId = tt.SluId;
            LampId  = tt.LampId;
            DateCreate  = new DateTime(tt.DateCtrlCreate ).ToString("yyyy-MM-dd HH:mm:ss");

            Electricity = Math.Round(tt.Electricity, 2);
            ElectricityTotal = Math.Round(tt.ElectricityTotal, 2); // tt.ElectricityTotal.ToString("f2");

            ActiveTime = Math.Round(tt.ActiveTime / 60, 2); //tt.ActiveTime.ToString("f2");
            ActiveTimeTotal = Math.Round(tt.ActiveTimeTotal / 60, 2); //tt.ActiveTimeTotal.ToString("f2");

            TotalElec = 0;
            TotalTime = 0;
        }

        public DataLampItemMining(DataSluElec.DataSluCtrlElecItem tt, int index)
        {
            Index = index;
            CtrlId = Wj2090Module.Services.CommonSlu.GetPhyIdByCtrl(tt.SluId, tt.CtrlId);
            SluId = tt.SluId;
            LampId = tt.LampId;
            DateCreate = new DateTime(tt.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");

            ////if(isone)
            ////{
            //    Electricity = Math.Round(tt.OneDayUsePower, 2);
            //    ElectricityTotal = Math.Round(tt.OneDayUsePower, 2);

            //    ActiveTime = Math.Round(tt.OneDayOpenTime / 60, 2);
            //    ActiveTimeTotal = Math.Round(tt.OneDayOpenTime / 60, 2); 

            ////}
            ////else
            ////{
            Electricity = Math.Round(tt.OneDayUsePower, 2);
            ElectricityTotal = Math.Round(tt.BasePowerThatdayLastRecord, 2);

            ActiveTime = Math.Round(tt.OneDayOpenTime / 60, 2);
            ActiveTimeTotal = Math.Round(tt.BaseTimeThatdayLastRecord / 60, 2); 

            //}
                TotalElec = 0;
                TotalTime = 0;

            IsThatDayChangeControl = tt.IsThatdayChangedCtrl;
        }

        #region SluId

        private int   _indeSluIdx;

        public int    SluId
        {
            get { return _indeSluIdx; }
            set
            {
                if (_indeSluIdx == value) return;
                _indeSluIdx = value;
                RaisePropertyChanged(() => SluId);

                var  ntg = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( value);
               
                    SluName = ntg.RtuName ;


                    SluShowId = ntg.RtuPhyId .ToString("D4");
            }
        }

        private string _ssdfSluId;

        public string SluShowId
        {
            get { return _ssdfSluId; }
            set
            {
                if (value != _ssdfSluId)
                {
                    _ssdfSluId = value;
                    this.RaisePropertyChanged(() => this.SluShowId);
                }
            }
        }



        #endregion


        #region SluName

        private string _indeSlsdfuIdx;

        public string SluName
        {
            get { return _indeSlsdfuIdx; }
            set
            {
                if (_indeSlsdfuIdx == value) return;
                _indeSlsdfuIdx = value;
                RaisePropertyChanged(() => SluName);
            }
        }



        #endregion


        #region Index

        private int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                if (_index.Equals(value)) return;
                _index = value;
                RaisePropertyChanged(() => Index);
            }
        }

        #endregion

        #region CtrlId

        private int _controlId;

        public int CtrlId
        {
            get { return _controlId; }
            set
            {
                if (_controlId == value) return;
                _controlId = value;
                RaisePropertyChanged(() => CtrlId);
            }
        }

        #endregion

        #region LampId

        private int _lightNum;

        public int LampId
        {
            get { return _lightNum; }
            set
            {
                if (_lightNum.Equals(value)) return;
                _lightNum = value;
                RaisePropertyChanged(() => LampId);
            }
        }

        #endregion

        #region SampleTime

        private string  _sampleTime;

        public string  DateCreate
        {
            get { return _sampleTime; }
            set
            {
                if (_sampleTime == value) return;
                _sampleTime = value;
                RaisePropertyChanged(() => DateCreate);
            }
        }

        #endregion


        #region Electricity

        private double _electricity;

        public double Electricity
        {
            get { return _electricity; }
            set
            {
                if (_electricity == value) return;
                _electricity = value;
                //   _electricity = Math.Round(value, 2);
                RaisePropertyChanged(() => Electricity);
            }
        }

        #endregion

        #region ElectricityTotal

        private double _electricitytt;

        public double ElectricityTotal
        {
            get { return _electricitytt; }
            set
            {
                if (_electricitytt == value) return;
                _electricitytt = value;
                // _electricitytt = Math.Round(value, 2);
                RaisePropertyChanged(() => ElectricityTotal);
            }
        }

        #endregion

        #region IsThatDayChangeControl

        private int _isThatDayChangeControl;

        public int IsThatDayChangeControl
        {
            get { return _isThatDayChangeControl; }
            set
            {
                if (_isThatDayChangeControl == value) return;
                _isThatDayChangeControl = value;
                // _electricitytt = Math.Round(value, 2);
                RaisePropertyChanged(() => IsThatDayChangeControl);
            }
        }

        #endregion

        private double _activeTime;

        public double ActiveTime
        {
            get { return _activeTime; }
            set
            {
                if (_activeTime == value) return;
                _activeTime = value;
                RaisePropertyChanged(() => ActiveTime);
            }
        }


        private double _activeTimeTotal;

        public double ActiveTimeTotal
        {
            get { return _activeTimeTotal; }
            set
            {
                if (_activeTimeTotal == value) return;
                _activeTimeTotal = value;
                RaisePropertyChanged(() => ActiveTimeTotal);
            }
        }


        private double _TotalElec;

        public double TotalElec
        {
            get { return _TotalElec; }
            set
            {
                if (_TotalElec == value) return;
                _TotalElec = value;
                RaisePropertyChanged(() => TotalElec);
            }
        }


        private double _TotalTime;

        public double TotalTime
        {
            get { return _TotalTime; }
            set
            {
                if (_TotalTime == value) return;
                _TotalTime = value;
                RaisePropertyChanged(() => TotalTime);
            }
        }

    }
}
