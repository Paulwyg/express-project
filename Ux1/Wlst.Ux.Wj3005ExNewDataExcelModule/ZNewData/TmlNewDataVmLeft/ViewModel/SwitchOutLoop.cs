using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.ViewModel
{
   
    public class SwitchOutLoopLeft : ObservableObject
    {
        private int RtuId;
        //private int SwitchOutCount;
        public SwitchOutLoopLeft(int rtuid, int switchoutid, string loopName, string backcolor, bool isOutInCloseState, string timeInfo, string timeTooltipst)
        {
            this.LoopId = switchoutid;
            LoopName = loopName;
            Backgroundx = backcolor;
            TimeInfo = timeInfo;
            TimeTooltipst = timeTooltipst;
            if(isOutInCloseState )
            {
                IsOutInCloseState = "开";
                TongDuan = "Red";
            }
            else
            {
                  IsOutInCloseState = "关";
                TongDuan = "Black";
            }

          //  IsOutInCloseState = isOutInCloseState;

            RtuId = rtuid;


        }


        private string _looTongDuanpId;
        /// <summary>
        /// 回路序号
        /// </summary>
        public string  TongDuan
        {
            get { return _looTongDuanpId; }
            set
            {
                if (value != _looTongDuanpId)
                {
                    _looTongDuanpId = value;
                    this.RaisePropertyChanged(() => this.TongDuan);
                }
            }
        }



        private int _loopId;
        /// <summary>
        /// 回路序号
        /// </summary>
        public int LoopId
        {
            get { return _loopId; }
            set
            {
                if (value != _loopId)
                {
                    _loopId = value;
                    this.RaisePropertyChanged(() => this.LoopId);
                }
            }
        }

        private string _loosdfpName;
        /// <summary>
        /// 回路名称
        /// </summary>
        public string Backgroundx
        {
            get { return _loosdfpName; }
            set
            {
                if (value != _loosdfpName)
                {
                    _loosdfpName = value;
                    this.RaisePropertyChanged(() => this.Backgroundx);
                }
            }
        }

        private string _loosdfpNasdfme;
        /// <summary>
        /// 回路名称
        /// </summary>
        public string BackgroundAttach
        {
            get { return _loosdfpNasdfme; }
            set
            {
                if (value != _loosdfpNasdfme)
                {
                    _loosdfpNasdfme = value;
                    this.RaisePropertyChanged(() => this.BackgroundAttach);
                }
            }
        }

 

        private string _loopName;
        /// <summary>
        /// 回路名称
        /// </summary>
        public string LoopName
        {
            get { return _loopName; }
            set
            {
                if (value != _loopName)
                {
                    _loopName = value;
                    this.RaisePropertyChanged(() => this.LoopName);
                }
            }
        }



        private string _v;
        /// <summary>
        /// 回路电压  或 所代表的门啥的状态
        /// </summary>
        public string TimeInfo
        {
            get { return _v; }
            set
            {
                if (value != _v)
                {
                    _v = value;
                    this.RaisePropertyChanged(() => this.TimeInfo);
                }
            }
        }

        private string _a;
        /// <summary>
        /// 回路电流
        /// </summary>
        public string TimeTooltipst
        {
            get { return _a; }
            set
            {
                if (value != _a)
                {
                    _a = value;
                    this.RaisePropertyChanged(() => this.TimeTooltipst);
                }
            }
        }


        private string    _ratio;

        /// <summary>
        /// 互感器比值
        /// </summary>
        public string  IsOutInCloseState
        {
            get { return _ratio; }
            set
            {
                if (value != _ratio)
                {
                    _ratio = value;
                    this.RaisePropertyChanged(() => this.IsOutInCloseState);
                }
            }
        }

    }
}
