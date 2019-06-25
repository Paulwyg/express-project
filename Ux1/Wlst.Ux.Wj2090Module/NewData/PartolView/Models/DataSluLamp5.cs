using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Ux.Wj2090Module.NewData.ViewModel;

namespace Wlst.Ux.Wj2090Module.NewData.PartolView.Models
{
    public class DataSluLamp5 : DataSluCtrlLampVm
    {
        public DataSluLamp5(Wlst.client.SluCtrlDataMeasureReply.DataSluCtrlLamp info, int sluId, int ctrlId, int state)
            : base(info, state)
        {
            SluId = sluId;
            CtrlId = ctrlId;
            this.CtrlPhyId = NewDataViewModel.GetPhyIdByRtuId(sluId, ctrlId);
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
                var infos = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById(  value);
                if (infos != null)
                {
                    SluName = infos.RtuName ;
                    SluShowId = infos.RtuPhyId .ToString("D4") + "";
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
    }
}
