using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
using Wlst.Ux.Wj2090Module.NewData.ViewModel;

namespace Wlst.Ux.Wj2090Module.NewData.DataSluAssisQuery
{
    public class SingleMaxMinData  :Wlst .Cr .Core .CoreServices .ObservableObject 
    {

        public SingleMaxMinData(int sluid,int ctrlid, Wlst.client.SluMaxMinData.SingleMaxMinData  info)
        {
            SluId = sluid;

            this.CtrlId = ctrlid;
            //this.CtrlPhyId = NewDataViewModel.GetPhyIdByRtuId(sluid, ctrlid);
            this.CtrlLampCode = NewDataViewModel.GetLampCode(sluid, ctrlid);
            this.BarCode = NewDataViewModel.GetBarCode(sluid, ctrlid).PadLeft(13, '0');
            MinVoltage = info.MinVoltage.ToString("f2");
            MaxVoltage = info.MaxVoltage.ToString("f2");
            MinCurrent = info.MinCurrent.ToString("f2");
            MaxCurrent = info.MaxCurrent.ToString("f2");
            LampId = info.LampId;

        }




        #region attri




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


        private int _isdfsdfndexsdf;

        public int LampId
        {
            get { return _isdfsdfndexsdf; }
            set
            {
                if (_isdfsdfndexsdf == value) return;
                _isdfsdfndexsdf = value;
                RaisePropertyChanged(() => LampId);
            }
        }



        private string _indexsdf;

        public string MaxVoltage
        {
            get { return _indexsdf; }
            set
            {
                if (_indexsdf == value) return;
                _indexsdf = value;
                RaisePropertyChanged(() => MaxVoltage);
            }
        }

        private string _indsdfsdfdf;

        public string MaxCurrent
        {
            get { return _indsdfsdfdf; }
            set
            {
                if (_indsdfsdfdf == value) return;
                _indsdfsdfdf = value;
                RaisePropertyChanged(() => MaxCurrent);
            }
        }

        private string _minVoltage;

        public string MinVoltage
        {
            get { return _minVoltage; }
            set
            {
                if (_minVoltage == value) return;
                _minVoltage = value;
                RaisePropertyChanged(() => MinVoltage);
            }
        }

        private string _minCurrent;

        public string MinCurrent
        {
            get { return _minCurrent; }
            set
            {
                if (_minCurrent == value) return;
                _minCurrent = value;
                RaisePropertyChanged(() => MinCurrent);
            }
        }



        #region SluId

        private int _indeSluIdx;

        public int SluId
        {
            get { return _indeSluIdx; }
            set
            {
                if (_indeSluIdx == value) return;
                _indeSluIdx = value;
                RaisePropertyChanged(() => SluId);
                var infos = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value);
                if (infos != null)
                {
                    SluName = infos.RtuName;
                    SluShowId = infos.RtuPhyId.ToString("D4") + "";
                }
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


        /// <summary>
        /// 灯杆编码
        /// </summary>
        private string _ctrlLampCode;

        public string CtrlLampCode
        {
            get { return _ctrlLampCode; }
            set
            {
                if (_ctrlLampCode == value) return;
                _ctrlLampCode = value;
                RaisePropertyChanged(() => CtrlLampCode);
            }
        }

        private string _barCode;
        /// <summary>
        /// 控制器条形码
        /// </summary>
        public string BarCode
        {
            get { return _barCode; }
            set
            {
                if (_barCode == value) return;
                _barCode = value;
                RaisePropertyChanged(() => BarCode);
            }
        }

        private int _insdfsdfdexsdf;

        public int CtrlId
        {
            get { return _insdfsdfdexsdf; }
            set
            {
                if (_insdfsdfdexsdf == value) return;
                _insdfsdfdexsdf = value;
                RaisePropertyChanged(() => CtrlId);
            }
        }

        private string _ctrlName;
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string CtrlName
        {
            get { return _ctrlName; }
            set
            {
                if (_ctrlName == value) return;
                _ctrlName = value;
                RaisePropertyChanged(() => CtrlName);
            }
        }

        #endregion




    }
}
