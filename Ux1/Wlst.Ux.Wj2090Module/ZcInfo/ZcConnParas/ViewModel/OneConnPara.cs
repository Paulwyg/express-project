using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.client;

namespace Wlst.Ux.Wj2090Module.ZcInfo.ZcConnParas.ViewModel
{
    public class OneConnPara : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        #region



        private string sdSluId;

        /// <summary>
        /// 控制器状态 false-停运，true-投运
        /// </summary>
        public string CtrlStatus
        {
            get { return sdSluId; }
            set
            {
                if (value == sdSluId) return;
                sdSluId = value;
                this.RaisePropertyChanged(() => this.CtrlStatus);
            }
        }


        private string sdSsdfluId;

        /// <summary>
        /// 控制器条码
        /// </summary>
        public string CtrlBarcode
        {
            get { return sdSsdfluId; }
            set
            {
                if (value == sdSsdfluId) return;
                sdSsdfluId = value;
                this.RaisePropertyChanged(() => this.CtrlBarcode);
            }
        }

        private string sdfsdfds;

        /// <summary>
        /// 控制器物理地址
        /// </summary>
        public string PhyId
        {
            get { return sdfsdfds; }
            set
            {
                if (value == sdfsdfds) return;
                sdfsdfds = value;
                this.RaisePropertyChanged(() => this.PhyId);
            }
        }

        private string indexcount;

        /// <summary>
        /// 控制器地址
        /// </summary>
        public string RtuId
        {
            get { return indexcount; }
            set
            {
                if (value == indexcount) return;
                indexcount = value;
                this.RaisePropertyChanged(() => this.RtuId);
            }
        }

        private string ordertype;

        /// <summary>
        /// 控制器所属组（5个）
        /// </summary>
        public string CtrlGroup
        {
            get { return ordertype; }
            set
            {
                if (value == ordertype) return;
                ordertype = value;
                this.RaisePropertyChanged(() => this.CtrlGroup);
            }
        }



        private string operatortype;

        /// <summary>
        /// 控制器路由（前4级通信控制器编号1,2,3...)
        /// </summary>
        public string CtrlRoute
        {
            get { return operatortype; }
            set
            {
                if (value == operatortype) return;
                operatortype = value;
                this.RaisePropertyChanged(() => this.CtrlRoute);
            }
        }

        private string weekset;

        /// <summary>
        /// 开灯序号
        /// </summary>
        public string CtrlOrder
        {
            get { return weekset; }
            set
            {
                if (value == weekset) return;
                weekset = value;
                this.RaisePropertyChanged(() => this.CtrlOrder);
            }
        }


        private string orderSunxu;

        /// <summary>
        /// 功率上限
        /// </summary>
        public string UppperPowerLimit
        {
            get { return orderSunxu; }
            set
            {
                if (value == orderSunxu) return;
                orderSunxu = value;
                this.RaisePropertyChanged(() => this.UppperPowerLimit);
            }
        }

        private string orderargu;

        /// <summary>
        /// 功率下限
        /// </summary>
        public string LowerPowerLimit
        {
            get { return orderargu; }
            set
            {
                if (value == orderargu) return;
                orderargu = value;
                this.RaisePropertyChanged(() => this.LowerPowerLimit);
            }
        }


        private string lampp1;

        /// <summary>
        /// 控制器上电开灯 true-开灯，false-关灯 依次为灯一灯二，，，
        /// </summary>
        public string CtrlPowerTurnon
        {
            get { return lampp1; }
            set
            {
                if (value == lampp1) return;
                lampp1 = value;
                this.RaisePropertyChanged(() => this.CtrlPowerTurnon);
            }
        }

        private string lampp2;

        /// <summary>
        ///  M控制器主报 false-禁止主报，true-允许主报
        /// </summary>
        public string CtrlEnableAlarm
        {
            get { return lampp2; }
            set
            {
                if (value == lampp2) return;
                lampp2 = value;
                this.RaisePropertyChanged(() => this.CtrlEnableAlarm);
            }
        }


        private string lampp3;

        /// <summary>
        /// 控制器物理矢量
        /// </summary>
        public string CtrlVector
        {
            get { return lampp3; }
            set
            {
                if (value == lampp3) return;
                lampp3 = value;
                this.RaisePropertyChanged(() => this.CtrlVector);
            }
        }

        private string lampp4;

        /// <summary>
        ///  额定功率
        /// </summary>
        public string RatedPower
        {
            get { return lampp4; }
            set
            {
                if (value == lampp4) return;
                lampp4 = value;
                this.RaisePropertyChanged(() => this.RatedPower);
            }
        }



        /// <summary>
        /// 灯杆编码
        /// </summary>

        #region LampCode
        private string _lampCode;

        public string LampCode
        {
            get { return _lampCode; }
            set
            {
                if (_lampCode == value) return;
                _lampCode = value;
                RaisePropertyChanged(() => LampCode);
            }
        }

        #endregion

        

        #endregion


        public OneConnPara(Wlst.client.SluCtrlParaRead.SluCtrlParaReadItem info, int sluId)
        {

            //lvf 2018年4月2日11:28:29  额定功率 对应关系 只呈现上限数字
            var dir = new Dictionary<int, string >();
            dir.Add(0,"未设置");
            dir.Add(1,"20");
            dir.Add(2, "100");
            dir.Add(3, "120");
            dir.Add(4, "150");
            dir.Add(5, "200");
            dir.Add(6, "250");
            dir.Add(7, "300");
            dir.Add(8, "400");
            dir.Add(9, "600");
            dir.Add(10, "800");
            dir.Add(11, "1000");
            dir.Add(12, "1500");
            dir.Add(13, "2000");
            dir.Add(14, "50");
            dir.Add(15, "75");


            this.RtuId = sluId + "";
            this.PhyId = info.PhyId + "";
            CtrlBarcode = info.CtrlBarcode + "";
            CtrlOrder = info.CtrlOrder + "";
            UppperPowerLimit = info.UppperPowerLimit + "";
            LowerPowerLimit = info.LowerPowerLimit + "";
            CtrlStatus = info.CtrlStatus ? "投运" : "停运";
            CtrlEnableAlarm = info.CtrlEnableAlarm ? "允许主报" : "禁止主报";

            LampCode = "---";
            //lvf 2018年4月2日10:53:27  添加灯杆编码
            var t = EquipmentDataInfoHold.InfoItems[sluId] as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;

            if (t == null || t.WjSluCtrls == null)
                return;

            foreach (var g in t.WjSluCtrls)
            {
                if ( g.Value.CtrlPhyId ==info.PhyId)
                {
                    LampCode = g.Value.LampCode;
                    break;
                }
            }

            CtrlGroup = "";
            foreach (var g in info.CtrlGroup)
            {
                CtrlGroup += g + "-";
            }
            if (CtrlGroup.Length > 1) CtrlGroup = CtrlGroup.Substring(0, CtrlGroup.Length - 1);

            CtrlRoute = "";
            foreach (var g in info.CtrlRoute)
            {
                CtrlRoute += g + "-";
            }
            if (CtrlRoute.Length > 1) CtrlRoute = CtrlRoute.Substring(0, CtrlRoute.Length - 1);

            CtrlPowerTurnon = "";
            foreach (var g in info.CtrlPowerTurnon)
            {
                CtrlPowerTurnon += g ? "是" + "-" : "否" + "-";
            }
            if (CtrlPowerTurnon.Length > 1) CtrlPowerTurnon = CtrlPowerTurnon.Substring(0, CtrlPowerTurnon.Length - 1);

            CtrlVector = "";
            foreach (var g in info.CtrlVector)
            {
                CtrlVector += g + "-";
            }
            if (CtrlVector.Length > 1) CtrlVector = CtrlVector.Substring(0, CtrlVector.Length - 1);

            //  lvf 2018年4月2日11:05:32  改成上限数字
            RatedPower = "";
            foreach (var g in info.RatedPower)
            {
                RatedPower += dir[g] + "-";
            }



            if (RatedPower.Length > 1) RatedPower = RatedPower.Substring(0, RatedPower.Length - 1);
        }
    }
}
