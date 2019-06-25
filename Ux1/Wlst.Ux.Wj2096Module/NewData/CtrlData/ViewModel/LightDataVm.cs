using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Wlst.Ux.Wj2096Module.NewData.CtrlData.ViewModel
{
  public   class LightDataVm:Wlst .Cr .Core .CoreServices .ObservableObject 
    {  
      
      #region attri



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



        private string  _indexsdf;

        public string  MaxVoltage
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

        private string _index;

        public string Electricity
        {
            get { return _index; }
            set
            {
                if (_index == value) return;
                _index = value;
                RaisePropertyChanged(() => Electricity);
            }
        }



        private string _indexx;

        public string ElectricityTotal
        {
            get { return _indexx; }
            set
            {
                if (_indexx == value) return;
                _indexx = value;
                RaisePropertyChanged(() => ElectricityTotal);
            }
        }


    
        #endregion

        public LightDataVm(Wlst.client.SluCtrlDataMeasureReply.AssistCtrlData.LightData info)
      {
          LampId = info.LampId;
          MaxCurrent = info.MaxCurrent.ToString("f2");
          MaxVoltage = info.MaxVoltage.ToString("f2");
          Electricity = info.Electricity.ToString("f2");
         
      //    ElectricityTotal =
      }
    }
}
