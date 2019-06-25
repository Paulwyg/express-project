using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Wlst.Ux.Wj2090Module.NewData.ViewModel
{
    public class UnknowCtrlVm : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        #region attri



        private int _isdfsdfndexsdf;

        public int CtrlId
        {
            get { return _isdfsdfndexsdf; }
            set
            {
                if (_isdfsdfndexsdf == value) return;
                _isdfsdfndexsdf = value;
                RaisePropertyChanged(() => CtrlId);
            }
        }

        /// <summary>
        /// 序号
        /// </summary>

        #region SluId

        private int _indexsdf;

        public int CtrlLoop
        {
            get { return _indexsdf; }
            set
            {
                if (_indexsdf == value) return;
                _indexsdf = value;
                RaisePropertyChanged(() => CtrlLoop);
            }
        }

        private string _indsdfsdfdf;

        public string PowerSaving
        {
            get { return _indsdfsdfdf; }
            set
            {
                if (_indsdfsdfdf == value) return;
                _indsdfsdfdf = value;
                RaisePropertyChanged(() => PowerSaving);
            }
        }

        private string _index;

        public string HasLeakage
        {
            get { return _index; }
            set
            {
                if (_index == value) return;
                _index = value;
                RaisePropertyChanged(() => HasLeakage);
            }
        }

        #endregion


        private string _lDateCreate;

        public string HasTemperature
        {
            get { return _lDateCreate; }
            set
            {
                if (_lDateCreate == value) return;
                _lDateCreate = value;
                RaisePropertyChanged(() => HasTemperature);
            }
        }


        private string _lDateReply;

        public string HasTimer
        {
            get { return _lDateReply; }
            set
            {
                if (_lDateReply == value) return;
                _lDateReply = value;
                RaisePropertyChanged(() => HasTimer);
            }
        }


        private string _liUserName;

        public string Model
        {
            get { return _liUserName; }
            set
            {
                if (_liUserName == value) return;
                _liUserName = value;
                RaisePropertyChanged(() => Model);
            }
        }


        private string _lRemark;

        public string CtrlBarcode
        {
            get { return _lRemark; }
            set
            {
                if (_lRemark == value) return;
                _lRemark = value;
                RaisePropertyChanged(() => CtrlBarcode);
            }
        }

        #endregion

        public UnknowCtrlVm(Wlst.client.SluCtrlDataMeasureReply.UnknowCtrl info)
        {
            CtrlId = info.CtrlId;
            //this.CtrlPhyId = NewDataViewModel.GetPhyIdByRtuId(i, CtrlId);
            CtrlLoop = info.CtrlLoop;
            PowerSaving = info.PowerSaving == 0
                              ? "无控制"
                              : info.PowerSaving == 1
                                    ? "只有开关灯"
                                    : info.PowerSaving == 2
                                          ? "调档节能"
                                          : info.PowerSaving == 3
                                                ? "调光节能"
                                                : info.PowerSaving == 4 ? "RS485" : "调光";
            HasLeakage = info.HasLeakage ? "有" : "无";
            HasTemperature = info.HasTemperature ? "有" : "无";
            HasTimer = info.HasTimer ? "有" : "无";
            Model = info.Model == 1 ? "wj2090j" : "未知";
            CtrlBarcode = info.CtrlBarcode + "";
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
    }
}
