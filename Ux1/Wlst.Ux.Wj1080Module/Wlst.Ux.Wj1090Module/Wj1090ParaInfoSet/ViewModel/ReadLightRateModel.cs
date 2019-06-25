using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Sr.ProtocolCnt.Wj1090;

namespace Wlst.Ux.Wj1090Module.Wj1090ParaInfoSet.ViewModel
{
   public class ReadLightRateModel: Cr.Core.CoreServices.ObservableObject
   {
       #region 回路标识
       /// <summary>
        /// 回路地址 1-6
        /// </summary>
        private int _lineLoopId; //回路标识，二进制转十进制

        public int LineLoopId
        {
            get { return _lineLoopId; }
            set
            {
                if(_lineLoopId.Equals(value)) return;
                _lineLoopId = value;
                RaisePropertyChanged("LineLoopId");
            }
        }
       #endregion

        #region 亮灯率
        /// <summary>
        /// 亮灯率
        /// </summary>
        private double _brightLightRate;
        public double BrightLightRate
        {
            get { return _brightLightRate; }
            set
            {
                if(_brightLightRate.Equals(value)) return;
                _brightLightRate = value;
                RaisePropertyChanged("BrightLightRate");
            }
        }
        #endregion

        public ReadLightRateModel(LduLineBrightLightData item)
        {
            LineLoopId = item.LineLoopId;
            BrightLightRate = item.BrightLightRate;
        }
    }
}
