using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj2090Module.HisDataQuery.ConcentratorDataQuery.ViewModel
{
    public class DataPhyItem : Wlst.Cr.Core.CoreServices.ObservableObject 
    {
       

        public DataPhyItem(Wlst.client.SluCtrlDataMeasureReply.CtrlPhyinfo tmp, int sluId, int ctrlId)
       {
           // SetSluNameId(sluId);
           this.CtrlId = ctrlId;
           this.CtrlPhyId = GetPhyIdByRtuId(sluId, ctrlId);
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
           Model = tmp.Model == 1 ? "wj2090j" : "未知";

           Routing = tmp.Routing;


            var datecreate = new DateTime(tmp.DtCreate);
            DateCreate = datecreate.ToString("yyyy-MM-dd HH:mm:ss");
       }

        public static int GetPhyIdByRtuId(int sluId, int ctrId)
        {
            var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(sluId);
            if (info == null) return 0;
            var tmps = info as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
            if (tmps == null) return 0;
            if (tmps.WjSluCtrls.ContainsKey(ctrId))
            {
                return tmps.WjSluCtrls[ctrId].CtrlPhyId;
            }
            return 0;
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


       private string _datcreate;
       public string DateCreate
       {
           get { return _datcreate; }
           set
           {
               if (_datcreate == value) return;
               _datcreate = value;
               RaisePropertyChanged(() => DateCreate);
           }
       }


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
