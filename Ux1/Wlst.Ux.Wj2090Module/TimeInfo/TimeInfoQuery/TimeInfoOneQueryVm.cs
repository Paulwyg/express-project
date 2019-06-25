using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Ux.Wj2090Module.TimeInfo.TimeInfoSet.ViewModel;
using Wlst.client;

namespace Wlst.Ux.Wj2090Module.TimeInfo.TimeInfoQuery
{
    public class TimeInfoOneQueryVm : TimeInfoOneVm
    {
        public TimeInfoOneQueryVm(int areaId ,SluTimeScheme.SluTimeSchemeItem info, SluTimeScheme.SluTimeSchemeItem.SluTimeCtrlSluOne  ctrls)
            : base(areaId,info)
        {
            bool is485 = false;
             
            var holdinf =
                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( ctrls.SluId);
            if (holdinf != null)
            {
                if (holdinf.RtuFid  == 0)
                {
                    is485 = false;
                }
                else
                {
                    is485 = true;
                }


            }

            if(info.IsNotUsed )
            {
                Status = "停用";
            }
            else
            {
                Status = "使用";
            }


            if (ctrls.OperatorType == 101)
            {
                CtrlInfos = "无操作";
            }
            if (ctrls.OperatorType == 10)
            {
                CtrlInfos = "全部节点";
            }
            if (ctrls.OperatorType == 21)
            {
                CtrlInfos = "单数节点";
            }
            if (ctrls.OperatorType == 20)
            {
                CtrlInfos = "双数节点";
            }
            if (ctrls.OperatorType == 31)
            {
                CtrlInfos = "隔二亮一";
            }
            if (ctrls.OperatorType == 41)
            {
                CtrlInfos = "隔三亮一";
            }
            if (ctrls.OperatorType == 51)
            {
                CtrlInfos = "隔四亮一";
            }
            if (ctrls.OperatorType == 4)
            {

                CtrlInfos = "分组：";

                foreach (var g in ctrls.CtrlPhys)
                {
                    CtrlInfos += g + "-";
                }

                CtrlInfos = CtrlInfos.Substring(0, CtrlInfos.Length - 1);
            }
        }



        private string _ctrlInfos;

        public string CtrlInfos
        {
            get { return _ctrlInfos; }
            set
            {
                if (value != _ctrlInfos)
                {
                    _ctrlInfos = value;
                    this.RaisePropertyChanged(() => this.CtrlInfos);
                }
            }
        }


        private string _Status;

        public string Status
        {
            get { return _Status; }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    this.RaisePropertyChanged(() => this.Status);
                }
            }
        }
        

    }
}
