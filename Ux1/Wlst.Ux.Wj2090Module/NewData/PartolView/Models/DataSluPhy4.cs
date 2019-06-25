using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Ux.Wj2090Module.NewData.ViewModel;

namespace Wlst.Ux.Wj2090Module.NewData.PartolView.Models
{
   public  class DataSluPhy4:Wlst .Cr .Core .CoreServices .ObservableObject 
    {
       public DataSluPhy4(Wlst.client.SluCtrlDataMeasureReply.CtrlPhyinfo tmp, int sluId, int ctrlId)
       {
           // SetSluNameId(sluId);
           this.CtrlId = ctrlId;
           this.CtrlPhyId = NewDataViewModel.GetPhyIdByRtuId(sluId, ctrlId);
           SluId = sluId;

           SignalStrength = tmp.SignalStrength;
           Phase = tmp.Phase == 1 ? "A相" : tmp.Phase == 2 ? "B相" : tmp.Phase == 3 ? "C相" : "未知";
           UsefulCommunicate = tmp.UsefulCommunicate;
           AllCommunicate = tmp.AllCommunicate;
           CtrlLoop = tmp.CtrlLoop;
           PowerSaving = tmp.PowerSaving == 0
                             ? "无控制"
                             : tmp.PowerSaving == 1
                                   ? "只有开关灯"
                                   : tmp.PowerSaving == 2
                                         ? "调档节能"
                                         : tmp.PowerSaving == 3
                                               ? "调光节能"
                                               : tmp.PowerSaving == 4 ? "RS485" : "调光";
           HasLeakage = tmp.HasLeakage ? "有" : "无";
           HasTemperature = tmp.HasTemperature ? "有" : "无";
           HasTimer = tmp.HasTimer ? "有" : "无";
           //Model = tmp.Model == 1 ? "wj2090j" : "未知";
           //lvf 2018年4月8日11:23:36  添加控制器型号解析
           switch (tmp.Model)
           {
               case 0:
                   Model = "未知";
                   break;
               case 1:
                   Model = "WJ2090J";
                   break;
               case 2:
                   Model = "WJ2090K";
                   break;
               case 3:
                   Model = "WJ2090C";
                   break;
               case 4:
                   Model = "WJ2090D";
                   break;
               case 5:
                   Model = "WJ2090L";
                   break;
               case 6:
                   Model = "WJ2090M";
                   break;
               case 7:
                   Model = "WJ4090";
                   break;
               default:
                   Model = "";
                   break;
           }

           Routing = tmp.Routing;

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
                   SluName = infos.RtuName  ;
                   SluShowId = infos.RtuPhyId  .ToString("D4") + "";
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

        #region attri

       private int _isdfsdfRoutingndexsdf;

       public int Routing
       {
           get { return _isdfsdfRoutingndexsdf; }
           set
           {
               if (_isdfsdfRoutingndexsdf == value) return;
               _isdfsdfRoutingndexsdf = value;
               RaisePropertyChanged(() => Routing);
           }
       }


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

        /// <summary>
        /// 序号
        /// </summary>

        #region SluId

        private int _indexsdf;

        public int SignalStrength
        {
            get { return _indexsdf; }
            set
            {
                if (_indexsdf == value) return;
                _indexsdf = value;
                RaisePropertyChanged(() => SignalStrength);
            }
        }

        private string _inPhasedexsdf;

        public string Phase
        {
            get { return _inPhasedexsdf; }
            set
            {
                if (_inPhasedexsdf == value) return;
                _inPhasedexsdf = value;
                RaisePropertyChanged(() => Phase);
            }
        }
        private int _iUsefulCommunicatendexsdf;

        public int UsefulCommunicate
        {
            get { return _iUsefulCommunicatendexsdf; }
            set
            {
                if (_iUsefulCommunicatendexsdf == value) return;
                _iUsefulCommunicatendexsdf = value;
                RaisePropertyChanged(() => UsefulCommunicate);
            }
        }

        private int _indAllCommunicateexsdf;

        public int AllCommunicate
        {
            get { return _indAllCommunicateexsdf; }
            set
            {
                if (_indAllCommunicateexsdf == value) return;
                _indAllCommunicateexsdf = value;
                RaisePropertyChanged(() => AllCommunicate);
            }
        }    private int _indeCtrlLoopxsdf;

        public int CtrlLoop
        {
            get { return _indeCtrlLoopxsdf; }
            set
            {
                if (_indeCtrlLoopxsdf == value) return;
                _indeCtrlLoopxsdf = value;
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


        private string _lHasTemperature;

        public string HasTemperature
        {
            get { return _lHasTemperature; }
            set
            {
                if (_lHasTemperature == value) return;
                _lHasTemperature = value;
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

        #endregion



    }
}
