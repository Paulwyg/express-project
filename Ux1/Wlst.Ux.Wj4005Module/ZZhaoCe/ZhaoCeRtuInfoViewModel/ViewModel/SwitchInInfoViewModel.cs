using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj4005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.ViewModel
{
    public class SwitchInInfoViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public SwitchInInfoViewModel(Wlst.client.ZhaoCeInfo.RtuZhaoRtuPara1.SwitchIn switchInInfo, int index)
        {
            this.LoopId = Convert.ToString(index + 1);

            if (switchInInfo.CurrentPhase[index] == 0)
            {
                this.CurrentPhase = "A相";
            }
            else if (switchInInfo.CurrentPhase[index] == 1)
            {
                this.CurrentPhase = "B相";
            }
            else if (switchInInfo.CurrentPhase[index] == 2)
            {
                this.CurrentPhase = "C相";
            }

            this.CurrentTransformer = Convert.ToString(switchInInfo.CurrentTransformer[index] * 5 + " / 5");
        }

        string _loopId;
        /// <summary>
        /// 回路序号 1~6
        /// </summary>
        public string LoopId
        {
            get { return _loopId; }
            set
            {
                if (_loopId != value)
                {
                    _loopId = value;
                    this.RaisePropertyChanged(() => this.LoopId);
                }
            }
        }

        string _currentPhase;
        /// <summary>
        /// 电流回路相位,0-a,1-b,2-c
        /// </summary>
        public string CurrentPhase
        {
            get { return _currentPhase; }
            set
            {
                if (_currentPhase != value)
                {
                    _currentPhase = value;
                    this.RaisePropertyChanged(() => this.CurrentPhase);
                }
            }
        }

        string _currentTransformer;
        /// <summary>
        /// 电流回路互感比,按真实值提交，通信下发时/5
        /// </summary>
        public string CurrentTransformer 
        {
            get { return _currentTransformer; }
            set
            {
                if (_currentTransformer != value)
                {
                    _currentTransformer = value;
                    this.RaisePropertyChanged(() => this.CurrentTransformer);
                }
            }
        }
    }
}
