using System;
using Wlst.client;

namespace Wlst.Ux.WJ4005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.ViewModel
{
    public class SwitchOutInfoViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        public SwitchOutInfoViewModel(Wlst.client.ZhaoCeInfo.RtuZhaoRtuPara1.SwitchOut switchOutInfo, int index)
        {
            this.SwitchOutId = "K" + Convert.ToString(index + 1);
            this.SwitchOutLoop = Convert.ToString(switchOutInfo.SwitchOutLoop[index]);
        }

        private string _switchOutId;

        /// <summary>
        /// 开关量输出地址序号 1~8
        /// </summary>
        public string SwitchOutId
        {
            get { return _switchOutId; }
            set
            {
                if (_switchOutId != value)
                {
                    _switchOutId = value;
                    this.RaisePropertyChanged(() => this.SwitchOutId);
                }
            }
        }

        private string _switchOutLoop;
        /// <summary>
        /// 每个开关量输出的回路数
        /// </summary>
        public string SwitchOutLoop
        {
            get { return _switchOutLoop; }
            set
            {
                if (_switchOutLoop != value)
                {
                    _switchOutLoop = value;
                    this.RaisePropertyChanged(() => this.SwitchOutLoop);
                }
            }
        }
    }
}
