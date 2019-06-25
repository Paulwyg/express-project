using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Ux.Wj2090Module.NewData.ViewModel;

namespace Wlst.Ux.Wj2090Module.NewData.PartolView.Models
{
   public  class DataSluUnknow2 : UnknowCtrlVm
    {
       public DataSluUnknow2(Wlst .client .SluCtrlDataMeasureReply. UnknowCtrl info,int sluId)
           : base( info)
       {
           SluId = sluId;
           this.CtrlPhyId = NewDataViewModel.GetPhyIdByRtuId(sluId, info .CtrlId );
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
                   SluName = infos.RtuName;
                   if (infos.RtuFid  == 0)
                   {
                       SluShowId = string.Format("{0:D7}", infos.RtuPhyId );
                   }
                   else SluShowId = infos.RtuId + "";
               }
               else
               {
                   SluName = value + "";
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


    }
}
