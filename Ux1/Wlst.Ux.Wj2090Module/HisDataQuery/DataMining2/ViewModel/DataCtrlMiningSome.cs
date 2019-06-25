using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj2090Module.HisDataQuery.DataMining2.ViewModel
{
    public class DataCtrlMiningSome : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        #region Index

        private int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                if (_index == value) return;
                _index = value;
                RaisePropertyChanged(() => Index);
            }
        }

        #endregion

        #region SluId

        private int  _indeSluIdx;

        public int SluId
        {
            get { return _indeSluIdx; }
            set
            {
                if (_indeSluIdx==value) return;
                _indeSluIdx = value;
                RaisePropertyChanged(() => SluId);

                var
                ntg = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( value);
                SluName = ntg.RtuName ;
                SluShowId = ntg.RtuPhyId.ToString("D4");
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
                if (_indeSlsdfuIdx==value) return;
                _indeSlsdfuIdx = value;
                RaisePropertyChanged(() => SluName);
            }
        }



        #endregion

        #region CtrlId

        private string _controlId;

        public string CtrlId
        {
            get { return _controlId; }
            set
            {
                int ctrlid;
                if(Int32.TryParse(value, out ctrlid))
                {
                    value = Wj2090Module.Services.CommonSlu.GetPhyIdByCtrl(SluId, ctrlid).ToString("d4");
                    if (_controlId == value) return;
                    _controlId = value;
                    RaisePropertyChanged(() => CtrlId);
                }
                else
                {
                    value = "--";
                    if (_controlId == value) return;
                    _controlId = value;
                    RaisePropertyChanged(() => CtrlId);
                }
            }
        }

        #endregion

        #region ElectricityTotalLamp

        private string _lectricityTotalLamp1;

        public string ElectricityTotalLamp1
        {
            get { return _lectricityTotalLamp1; }
            set
            {
                if (_lectricityTotalLamp1==value) return;
                _lectricityTotalLamp1 = value;
                RaisePropertyChanged(() => ElectricityTotalLamp1);
            }
        }


        private string _lectricityTotalLamp2;

        public string ElectricityTotalLamp2
        {
            get { return _lectricityTotalLamp2; }
            set
            {
                if (_lectricityTotalLamp2==value) return;
                _lectricityTotalLamp2 = value;
                RaisePropertyChanged(() => ElectricityTotalLamp2);
            }
        }



        private string _lectricityTotalLamp3;

        public string ElectricityTotalLamp3
        {
            get { return _lectricityTotalLamp3; }
            set
            {
                if (_lectricityTotalLamp3==value) return;
                _lectricityTotalLamp3 = value;
                RaisePropertyChanged(() => ElectricityTotalLamp3);
            }
        }


        private string _lectricityTotalLamp4;

        public string ElectricityTotalLamp4
        {
            get { return _lectricityTotalLamp4; }
            set
            {
                if (_lectricityTotalLamp4==value) return;
                _lectricityTotalLamp4 = value;
                RaisePropertyChanged(() => ElectricityTotalLamp4);
            }
        }
        private string _lectricityTotalLampx;

        public string ElectricityTotalLampx
        {
            get { return _lectricityTotalLampx; }
            set
            {
                if (_lectricityTotalLampx == value) return;
                _lectricityTotalLampx = value;
                RaisePropertyChanged(() => ElectricityTotalLampx);
            }
        }
        #endregion

        #region ActiveTimeTotal

        private string _activeTimeTotal1;

        public string ActiveTimeTotal1
        {
            get { return _activeTimeTotal1; }
            set
            {
                if (_activeTimeTotal1==value) return;
                _activeTimeTotal1 = value;
                RaisePropertyChanged(() => ActiveTimeTotal1);
            }
        }


        private string _activeTimeTotal2;

        public string ActiveTimeTotal2
        {
            get { return _activeTimeTotal2; }
            set
            {
                if (_activeTimeTotal2==value) return;
                _activeTimeTotal2 = value;
                RaisePropertyChanged(() => ActiveTimeTotal2);
            }
        }



        private string _activeTimeTotal3;

        public string ActiveTimeTotal3
        {
            get { return _activeTimeTotal3; }
            set
            {
                if (_activeTimeTotal3==value) return;
                _activeTimeTotal3 = value;
                RaisePropertyChanged(() => ActiveTimeTotal3);
            }
        }


        private string _activeTimeTotal4;

        public string ActiveTimeTotal4
        {
            get { return _activeTimeTotal4; }
            set
            {
                if (_activeTimeTotal4==value) return;
                _activeTimeTotal4 = value;
                RaisePropertyChanged(() => ActiveTimeTotal4);
            }
        }

        private string _activeTimeTotalx;

        public string ActiveTimeTotax
        {
            get { return _activeTimeTotalx; }
            set
            {
                if (_activeTimeTotalx == value) return;
                _activeTimeTotalx = value;
                RaisePropertyChanged(() => ActiveTimeTotax);
            }
        }
        #endregion

        private ObservableCollection<DataCtrlMiningSome> _concentratorItemsdfsss;

        public ObservableCollection<DataCtrlMiningSome> ItemsCtrlSome
        {
            get
            {
                return _concentratorItemsdfsss ??
                       (_concentratorItemsdfsss = new ObservableCollection<DataCtrlMiningSome>());
            }
            set
            {
                if (value == _concentratorItemsdfsss) return;
                _concentratorItemsdfsss = value;
                this.RaisePropertyChanged(() => this.ItemsCtrlSome);
            }
        }


        public double t1;
        public double t2;
        public double t3;
        public double t4;
        public double e1;
        public double e2;
        public double e3;
        public double e4;
    }
}
