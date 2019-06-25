using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Ux.Wj2096Module.TimeInfoSet.ViewModel;
using Wlst.client;

namespace Wlst.Ux.Wj2096Module.TimeInfoQuery
{
    public class TimeInfoOneQueryVm : TimeInfoOneVm
    {
        public TimeInfoOneQueryVm(int areaId, VSluTimeScheme.VSluTimeSchemeItem info, VSluTimeScheme.VSluTimeSchemeItem.VSluTimeCtrlSluOne ctrls)
            : base(areaId,info)
        {

            if(info.SluTimePlanInfo.IsNotUsed )
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
            if (ctrls.OperatorType == 1)
            {
                CtrlInfos = "全部节点";
            }

            if (ctrls.OperatorType == 2)
            {

                CtrlInfos = "分组：";

                foreach (var g in ctrls.CtrlOrGrp)
                {
                    CtrlInfos += g + "  ";
                }
            }
            if (ctrls.OperatorType == 3)
            {
                CtrlInfos = "控制器：";

                foreach (var g in ctrls.CtrlOrGrp)
                {
                    CtrlInfos += g + "  ";
                }
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
