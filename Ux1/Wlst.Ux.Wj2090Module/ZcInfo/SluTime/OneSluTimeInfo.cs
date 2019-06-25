using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj2090Module.ZcInfo.SluTime
{
    public class OneSluTimeInfo : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        public int Index;
        public int Count;

        #region

       

        private int sdSluId;

        /// <summary>
        /// 召测序号  ~~~~
        /// </summary>
        public int  SluId
        {
            get { return sdSluId; }
            set
            {
                if (value == sdSluId) return;
                sdSluId = value;
                this.RaisePropertyChanged(() => this.SluId);
            }
        }


        private int sdSsdfluId;

        /// <summary>
        /// 召测序号  ~~~~
        /// </summary>
        public int SluPhyId
        {
            get { return sdSsdfluId; }
            set
            {
                if (value == sdSsdfluId) return;
                sdSsdfluId = value;
                this.RaisePropertyChanged(() => this.SluPhyId);
            }
        }

        private string sdfsdfds;

        /// <summary>
        /// 召测序号  ~~~~
        /// </summary>
        public string ZcTime
        {
            get { return sdfsdfds; }
            set
            {
                if (value == sdfsdfds) return;
                sdfsdfds = value;
                this.RaisePropertyChanged(() => this.ZcTime);
            }
        }

        private string indexcount;

        /// <summary>
        /// 召测序号  ~~~~
        /// </summary>
        public string IndexCount
        {
            get { return indexcount; }
            set
            {
                if (value == indexcount) return;
                indexcount = value;
                this.RaisePropertyChanged(() => this.IndexCount);
            }
        }

        private string ordertype;

        /// <summary>
        /// 指令类型 经纬度、定时等
        /// </summary>
        public string OrderType
        {
            get { return ordertype; }
            set
            {
                if (value == ordertype) return;
                ordertype = value;
                this.RaisePropertyChanged(() => this.OrderType);
            }
        }



        private string operatortype;

        /// <summary>
        /// 操作类型 cmd或 pwm
        /// </summary>
        public string OperatorType
        {
            get { return operatortype; }
            set
            {
                if (value == operatortype) return;
                operatortype = value;
                this.RaisePropertyChanged(() => this.OperatorType);
            }
        }

        private string weekset;

        /// <summary>
        /// 周期
        /// </summary>
        public string WeekSet
        {
            get { return weekset; }
            set
            {
                if (value == weekset) return;
                weekset = value;
                this.RaisePropertyChanged(() => this.WeekSet);
            }
        }


        private string orderSunxu;

        /// <summary>
        /// 执行顺序 广播或依次
        /// </summary>
        public string OrderSunxu
        {
            get { return orderSunxu; }
            set
            {
                if (value == orderSunxu) return;
                orderSunxu = value;
                this.RaisePropertyChanged(() => this.OrderSunxu);
            }
        }

        private string orderargu;

        /// <summary>
        /// 操作参数 定时则为操作时间  经纬度则为 偏移分钟数
        /// </summary>
        public string OrderArgu
        {
            get { return orderargu; }
            set
            {
                if (value == orderargu) return;
                orderargu = value;
                this.RaisePropertyChanged(() => this.OrderArgu);
            }
        }


        private string lampp1;

        /// <summary>
        /// Mix操作 灯头的操作 ，pwm操作则为灯头的操作百分比
        /// </summary>
        public string Lamp1
        {
            get { return lampp1; }
            set
            {
                if (value == lampp1) return;
                lampp1 = value;
                this.RaisePropertyChanged(() => this.Lamp1);
            }
        }

        private string lampp2;

        /// <summary>
        ///  Mix操作 灯头的操作 ，pwm操作则为灯头的操作百分比
        /// </summary>
        public string Lamp2
        {
            get { return lampp2; }
            set
            {
                if (value == lampp2) return;
                lampp2 = value;
                this.RaisePropertyChanged(() => this.Lamp2);
            }
        }


        private string lampp3;

        /// <summary>
        ///  Mix操作 灯头的操作 ，pwm操作则为灯头的操作百分比
        /// </summary>
        public string Lamp3
        {
            get { return lampp3; }
            set
            {
                if (value == lampp3) return;
                lampp3 = value;
                this.RaisePropertyChanged(() => this.Lamp3);
            }
        }

        private string lampp4;

        /// <summary>
        ///  Mix操作 灯头的操作 ，pwm操作则为灯头的操作百分比
        /// </summary>
        public string Lamp4
        {
            get { return lampp4; }
            set
            {
                if (value == lampp4) return;
                lampp4 = value;
                this.RaisePropertyChanged(() => this.Lamp4);
            }
        }


        private string addrs;

        /// <summary>
        ///  控制器地址
        /// </summary>
        public string Addrs
        {
            get { return addrs; }
            set
            {
                if (value == addrs) return;
                addrs = value;
                this.RaisePropertyChanged(() => this.Addrs);
            }
        }

        #endregion


        public OneSluTimeInfo(int sluId, Wlst.client.SluTimeRead.SluTimeItem info)// .SluTimeZcShort .ReadConcentratorShortOperationData
        {
            var inforx = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(sluId);
            if(inforx !=null )
            {
                SluId = sluId;

                SluPhyId = inforx.RtuPhyId;


            }
            else
            {
                SluId = sluId;
                SluPhyId = sluId;
            }
           
            Index = info.Index;
            Count = info.DataCount;

            ZcTime = DateTime.Now.ToString("HH:mm:ss");
            IndexCount = Index.ToString("D2") + "/" + Count.ToString("D2");
            this.OrderType = info.OperationType == 1 ? "定时" : info.OperationType == 2 ? "经纬度" : "未知";
            this.OrderSunxu = info.OperationOrder == 1 ? "依次执行" : "广播执行";
            bool ishunhe = info.CmdType == 4;
            this.OperatorType = info.CmdType == 4 ? "混合控制" : info.CmdType == 3?"经纬度控制": "调光节能";

            if(info .OperationType ==1)
            {
                  int hour = info.TimerOrOff/60;
                int min = info.TimerOrOff%60;
                this.OrderArgu = hour.ToString("D2") + ":" + min.ToString("D2");
            }
            else
            {
                 this.OrderArgu = "偏移:" + info.TimerOrOff.ToString("D2");

            }
            //if (info.OperationType == 2 && info.CmdType == 4)
            //{
            //    bool openlight = false;
            //    bool closelight = false;
            //    foreach (var g in info.CmdMix)
            //    {
            //        if (g == 4)
            //        {
            //            closelight = true;
            //        }
            //        if (g == 1 || g == 2 || g == 3)
            //        {
            //            openlight = true;
            //        }
            //    }
            //    if (openlight == closelight)
            //    {
            //        this.OperatorType = 2;
            //    }
            //    else
            //    {
            //        if (openlight) this.OperationMethod = 2;
            //        else this.OperationMethod = 12;
            //    }
            //}

            if (ishunhe)
            {
              

                if (info.CmdMix.Count > 0)
                    this.Lamp1 = GetMixValue(info.CmdMix[0].Handle);
                else this.Lamp1 = "无";

                if (info.CmdMix.Count > 1)
                    this.Lamp2 = GetMixValue(info.CmdMix[1].Handle);
                else this.Lamp2 = "无";

                if (info.CmdMix.Count > 2)
                    this.Lamp3 = GetMixValue(info.CmdMix[2].Handle);
                else this.Lamp3 = "无";

                if (info.CmdMix.Count > 3)
                    this.Lamp4 = GetMixValue(info.CmdMix[3].Handle);
                else this.Lamp4 = "无";
            }
            else
            {
               
                if (info.CmdMix.Count > 0)
                    this.Lamp1 = GetMixValue(info.CmdMix[0].Handle);
                else this.Lamp1 = "无";

                if (info.CmdMix.Count > 1)
                    this.Lamp2 = GetMixValue(info.CmdMix[1].Handle);
                else this.Lamp2 = "无";

                if (info.CmdMix.Count > 2)
                    this.Lamp3 = GetMixValue(info.CmdMix[2].Handle);
                else this.Lamp3 = "无";

                if (info.CmdMix.Count > 3)
                    this.Lamp4 = GetMixValue(info.CmdMix[3].Handle);
                else this.Lamp4 = "无";
            }

            if (info.AddrType == 0) this.Addrs = "全部";
            if (info.AddrType == 1)
            {
                this.Addrs = "组：";
                foreach (var f in info.Addr)
                    this.Addrs += f + "-";
                

                this.Addrs = Addrs.Substring(0, Addrs.Length - 1);
            }
            if (info.AddrType == 2&& info.Addr.Count >0)
            {
                int x1 = info.Addr[0]/10;
                int x2 = info.Addr[0]%10;
                x1 = x1 - 1;
                if (x1 == 0) this.Addrs = "全部";
                else this.Addrs =  "隔" + x1 + "亮1";
                if (x1 < 0) this.Addrs = "隔1亮1";
            }
            if (info.AddrType == 3) this.Addrs = "控制器：" + info.Addr;
            if (info.AddrType == 4)
            {
                this.Addrs = "Gprs：" ;
                var ntgs = (from s in info.Addr orderby s ascending select s).ToList();
                foreach (var ss in ntgs)
                {
                    this.Addrs += ss + "-";
                }
            }


            WeekSet = "";
            string[] bt = new string[7] { "日", "一", "二", "三", "四", "五", "六" };
            for (int i = 1; i < info.WeekSet.Count; i++)
            {
                if (info.WeekSet[i])
                {
                    WeekSet += bt[i] + "、";
                }
            }
            if (info.WeekSet[0])
            {
                WeekSet += "日、";
            }


            if (WeekSet.Length > 1)
            {
                WeekSet = WeekSet.Substring(0, WeekSet.Length - 1);
            }
            else
            {
                WeekSet = "无";
            }

        }

        // 回路1-4操作，-1-不操作，0-开灯，1-一档节能，2-二档节能，3-关灯，100～200-pwm0%～100%
        private string GetMixValue(int x)
        {
            if (x == -1) return "不操作";
            if (x == 0) return "开灯";
            if (x == 1) return "调档节能";
            if (x == 2) return "调光节能";
            if (x == 3) return "关灯";

            int y = x - 100;
            if (y > -1 && y < 101) return y + " %";
            return "未知操作 "+x ;

            return "未知操作";
        }

        // 回路1-4操作，-1-不操作，0-开灯，1-一档节能，2-二档节能，3-关灯，100～200-pwm0%～100%
        //private string GetPwmValue(int x)
        //{
        //    int y = x - 100;
        //    if (y > -1 && y < 101) return y + " %";
        //    return "未知操作";
        //}
    }
}
