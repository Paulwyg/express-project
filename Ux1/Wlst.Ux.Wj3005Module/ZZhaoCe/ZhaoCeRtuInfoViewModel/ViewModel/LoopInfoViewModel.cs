
using Wlst.client;

namespace Wlst.Ux.WJ3005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.ViewModel
{
    public class LoopInfoViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        public LoopInfoViewModel(Wlst .client .ZhaoCeInfo .RtuZhaoRtuPara .RtuZhaoCeLoopInfo   loopInfo )
        {
            this.L36 = loopInfo.L36+"";
            this.L36Qlower = loopInfo.L36Qlower + "";
            this.L36Qupper = loopInfo.L36Qupper + "";
            this.LoopId = loopInfo.LoopId;
            this.Sin36 = loopInfo.Sin36 + "";
            
        }

        public LoopInfoViewModel(int   loopId)
        {
            this.L36 = "--";
            this.L36Qlower = "--";
            this.L36Qupper = "--";
            this.LoopId = loopId ;
            this.Sin36 = "--";

        }

        int _LoopId;
        /// <summary>
        /// 回路序号 1~6
        /// </summary>
        public int LoopId
        {
            get { return _LoopId; }
            set
            {
                if (_LoopId != value)
                {
                    _LoopId = value;
                    this.RaisePropertyChanged(() => this.LoopId);
                }
            }
        }

        string  _L36;
        /// <summary>
        /// 回路电流量程
        /// </summary>
        public string L36
        {
            get { return _L36; }
            set
            {
                if (_L36 != value)
                {
                    _L36 = value;
                    this.RaisePropertyChanged(() => this.L36);
                }
            }
        }

        string _L36Qupper;
        /// <summary>
        /// 回路电流上限
        /// </summary>
        public string L36Qupper
        {
            get { return _L36Qupper; }
            set
            {
                if (_L36Qupper != value)
                {
                    _L36Qupper = value;
                    this.RaisePropertyChanged(() => this.L36Qupper);
                }
            }
        }

        string _L36Qlower;
        /// <summary>
        /// 回路电流下限
        /// </summary>
        public string L36Qlower
        {
            get { return _L36Qlower; }
            set
            {
                if (_L36Qlower != value)
                {
                    _L36Qlower = value;
                    this.RaisePropertyChanged(() => this.L36Qlower);
                }
            }
        }

        string _Sin36;
        /// <summary>
        /// 模拟量输入矢量
        /// </summary>
        public string Sin36
        {
            get { return _Sin36; }
            set
            {
                if (_Sin36 != value)
                {
                    _Sin36 = value;
                    this.RaisePropertyChanged(() => this.Sin36);
                }
            }
        }

        private string kKK;
        /// <summary>
        /// 属于K几
        /// </summary>
        public string KKK
        {
            get { return kKK; }
            set
            {
                if (kKK != value)
                {
                    kKK = value;
                    this.RaisePropertyChanged(() => this.KKK);
                }
            }
        }

        private string switchinid;
        /// <summary>
        /// 输入地址 
        /// </summary>
        public string SwitchInId
        {
            get { return switchinid; }
            set
            {
                if (switchinid != value)
                {
                    switchinid = value;
                    this.RaisePropertyChanged(() => this.SwitchInId);
                }
            }
        }


        private string switchinidHop;
        /// <summary>
        /// 输入是否跳变报警 
        /// </summary>
        public string IsSwitchInHop
        {
            get { return switchinidHop; }
            set
            {
                if (switchinidHop != value)
                {
                    switchinidHop = value;
                    this.RaisePropertyChanged(() => this.IsSwitchInHop);
                }
            }
        }


        private string loopName;
        /// <summary>
        /// 回路名称 
        /// </summary>
        public string LoopName
        {
            get { return loopName; }
            set
            {
                if (loopName != value)
                {
                    loopName = value;
                    this.RaisePropertyChanged(() => this.LoopName);
                }
            }
        }
    }
}
