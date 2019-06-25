using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Ux.Wj2090Module.NewData.ViewModel;

namespace Wlst.Ux.Wj2090Module.NewData.PartolView.Models
{
    public class DataSluAssis6 : LightDataVm
    {
        public DataSluAssis6(Wlst.client.SluCtrlDataMeasureReply.AssistCtrlData.LightData data, int sluId, int ctrlId, double leakageCurrent)
            : base(data)
        {
            SluId = sluId;

            this.CtrlId = ctrlId;

            LeakageCurrent = leakageCurrent.ToString("f2");
            this.CtrlPhyId = NewDataViewModel.GetPhyIdByRtuId(sluId, ctrlId);
            this.CtrlLampCode = NewDataViewModel.GetLampCode(sluId, ctrlId);
            this.BarCode =NewDataViewModel. GetBarCode(sluId, ctrlId).PadLeft(13, '0');
        }
        private int _issdfsddfsdfndexsdf;

        public int CtrlPhyId
        {
            get { return _issdfsddfsdfndexsdf; }
            set
            {
                if (_issdfsddfsdfndexsdf == value) return;
                _issdfsddfsdfndexsdf = value;
                RaisePropertyChanged(() => CtrlPhyId);
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
                var infos = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( value);
                if (infos != null)
                {
                    SluName = infos.RtuName ;
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


        #region attri

        private string _iLeakageCurrent;

        public string LeakageCurrent
        {
            get { return _iLeakageCurrent; }
            set
            {
                if (_iLeakageCurrent == value) return;
                _iLeakageCurrent = value;
                RaisePropertyChanged(() => LeakageCurrent);
            }
        }

        #endregion
    }
}
