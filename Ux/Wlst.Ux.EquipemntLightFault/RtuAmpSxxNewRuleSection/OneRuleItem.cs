using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.client;

namespace Wlst.Ux.EquipemntLightFault.RtuAmpSxxNewRuleSection
{
    public partial class OneRuleItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        /// <summary>
        /// 全部为终端
        /// </summary>
        public List<Wlst.client.RtuSetsNew.RtuLoopSxxSectionInfo.RtuLoopSxx > SelectedRtus = new List<Wlst.client.RtuSetsNew.RtuLoopSxxSectionInfo.RtuLoopSxx>();

        public int AreaId = 0;

        private int ididid;

        public int Id
        {
            get { return ididid; }
            set
            {
                if (value == ididid) return;
                ididid = value;
                this.RaisePropertyChanged(() => this.Id);
            }
        }

        private string IdName;

        public string Name
        {
            get { return IdName; }
            set
            {
                if (value == IdName) return;
                IdName = value;
                this.RaisePropertyChanged(() => this.Name);
            }
        }

        private int idididx;

        public int RtuCount
        {
            get { return idididx; }
            set
            {
                if (value == idididx) return;
                idididx = value;
                this.RaisePropertyChanged(() => this.RtuCount);
            }
        }

        private int IdNaOpStartme;

        /// <summary>
        ///  1、开灯后，2、定时
        /// </summary>
        public int OpStart
        {
            get { return IdNaOpStartme; }
            set
            {
                if (value == IdNaOpStartme) return;
                IdNaOpStartme = value;
                this.RaisePropertyChanged(() => this.OpStart);
                Onupdaete();
            }
        }

        private int OOpEndpStart;

        /// <summary>
        /// 1、开灯后，2、定时
        /// </summary>
        public int OpEnd
        {
            get { return OOpEndpStart; }
            set
            {
                if (value == OOpEndpStart) return;
                OOpEndpStart = value;
                this.RaisePropertyChanged(() => this.OpEnd); Onupdaete();
            }
        }


        private bool OOpEndOpEndDtIsEnableOpEndDtpStart;
        public bool IsEnableOpEndDt
        {
            get { return OOpEndOpEndDtIsEnableOpEndDtpStart; }
            set
            {
                if (value == OOpEndOpEndDtIsEnableOpEndDtpStart) return;
                OOpEndOpEndDtIsEnableOpEndDtpStart = value;
                this.RaisePropertyChanged(() => this.IsEnableOpEndDt);
            }
        }
        private int OOpEndOpEndDtpStart;

        /// <summary>
        /// 1、当天，2、第二天
        /// </summary>
        public int OpEndDt
        {
            get { return OOpEndOpEndDtpStart; }
            set
            {
                if (value == OOpEndOpEndDtpStart) return;
                OOpEndOpEndDtpStart = value;
                this.RaisePropertyChanged(() => this.OpEndDt);
                Onupdaete();
            }
        }

        private string IdNDtStartame;

        /// <summary>
        /// 报警起始时间
        /// </summary>
        public string DtStart
        {
            get { return IdNDtStartame; }
            set
            {
                if (value == IdNDtStartame) return;
                //   IdNDtStartame = value;

                IdNDtStartame = getstr(value);
                this.RaisePropertyChanged(() => this.DtStart); Onupdaete();
            }
        }

        private string IdNDtEndame;

        /// <summary>
        /// 报警结束时间
        /// </summary>
        public string DtEnd
        {
            get { return IdNDtEndame; }
            set
            {
                if (value == IdNDtEndame) return;
                //  IdNDtEndame = value;

                IdNDtEndame = getstr(value);
                this.RaisePropertyChanged(() => this.DtEnd); Onupdaete();
            }
        }


        private DateTime IdNDtReq1DtEndame;

        /// <summary>
        /// 请求的起始时间
        /// </summary>
        public DateTime DtReq1
        {
            get { return IdNDtReq1DtEndame; }
            set
            {
                if (value == IdNDtReq1DtEndame) return;
                IdNDtReq1DtEndame = value;
                this.RaisePropertyChanged(() => this.DtReq1);
            }
        }

        private DateTime IdNDDtReq2tEndame;

        /// <summary>
        /// 请求的结束时间
        /// </summary>
        public DateTime DtReq2
        {
            get { return IdNDDtReq2tEndame; }
            set
            {
                if (value == IdNDDtReq2tEndame) return;
                IdNDDtReq2tEndame = value;
                this.RaisePropertyChanged(() => this.DtReq2);
            }
        }


        private string getstr(string data)
        {
            var xr = getStrInt(data);
            return (xr/60) + ":" + (xr%60);

        }

        private int getStrInt(string data)
        {
            if (string.IsNullOrEmpty(data)) return 0;
            int xr = 0;
            if (data.Contains(":") || data.Contains("：") || data.Contains(".") || data.Contains("。") ||
                data.Contains("，") || data.Contains(","))
            {
                var sps = data.Split(':', '：', ',', '.', '，', '。');
                int x1 = 0;
                int x2 = 0;

                if (Int32.TryParse(sps[0], out x1)) xr = x1*60;
                if (Int32.TryParse(sps[1], out x2)) xr += x2;
            }
            else
            {
                int xr1 = 0;
                if (Int32.TryParse(data, out xr1)) xr = xr1*60;
            }
            return xr;
            //return (xr / 60) + ":" + (xr % 60);

        }

        private string gettostr(int xr)
        {
            return (xr/60) + ":" + (xr%60);
        }

    }

    public partial class OneRuleItem
    {
        public OneRuleItem(Wlst.client.RtuSetsNew.RtuLoopSxxSectionInfo data)
        {
            Id = data.SectionId;
            Name = data.SectionName;
            OpStart = data.OpTimeStart;
            OpEnd = data.OpTimeEnd;

            DtStart = gettostr(data.TimeStart);

            if (data.TimeEnd > 9999)
            {
                OpEndDt = 2;
                DtEnd = gettostr(data.TimeEnd - 10000);
            }
            else
            {
                OpEndDt = 1;
                DtEnd = gettostr(data.TimeEnd );
            }
            
            //DtEnd = gettostr(data.TimeEnd);

            SelectedRtus = data.Items;
            AreaId = data.AreaId;
            RtuCount = (from t in SelectedRtus select t.RtuId).Distinct().Count();


            //var dtx = DateTime.Now.AddDays(-1);
            DtReq1 = new DateTime(data.DtRequestStart);
            DtReq2 = new DateTime(data.DtRequestEnd);

            //    new DateTime(dtx.Year, dtx.Month, dtx.Day, data.TimeStart/60, data.TimeStart%60, 0);
            //if (data.TimeEnd > data.TimeStart)
            //    DtReq2 = new DateTime(dtx.Year, dtx.Month, dtx.Day, data.TimeEnd/60, data.TimeEnd%60, 0);
            //else
            //    DtReq2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, data.TimeEnd/60,
            //                          data.TimeEnd%60, 0);
        }

        public OneRuleItem(int areaid, int id)
        {
            Id = id;
            Name = "新增规则";
            OpStart = 2;
            OpEnd = 2;

            DtStart = "22:00";
            DtEnd = "23:30";
            OpEndDt = 1;

            SelectedRtus = new List<RtuSetsNew.RtuLoopSxxSectionInfo.RtuLoopSxx>();
            AreaId = areaid;


            var dtx = DateTime.Now.AddDays(-1);
            DtReq1 = new DateTime(dtx.Year, dtx.Month, dtx.Day, 22, 0, 0);
            DtReq2 = new DateTime(dtx.Year, dtx.Month, dtx.Day, 23, 30, 0);
        }


        public Wlst.client.RtuSetsNew.RtuLoopSxxSectionInfo BackTo()
        {
            int baseend = 0;
            if (OpStart ==1 && OpEnd ==2 && OpEndDt == 2) baseend = 10000;
            return new RtuSetsNew.RtuLoopSxxSectionInfo()
                       {
                           AreaId = AreaId,
                           Items = SelectedRtus ,
                           OpTimeEnd = OpEnd,
                           OpTimeStart = OpStart,
                           SectionId = Id,
                           SectionName = Name,
                           TimeEnd = getStrInt(DtEnd) + baseend,
                           TimeStart = getStrInt(DtStart),
                           DtRequestEnd =DtReq2 .Ticks ,
                           DtRequestStart =DtReq1 .Ticks ,
                           //少 设定取数据的时间
                       };
        }



        void Onupdaete()
        {
            string ShowInfo = "方案-"+Name +" 上下限报警时间为:";

            var str = "";
            if (OpStart == 1)
            {
                str = "开灯后:";
                ShowInfo += str + getStrInt(DtStart) + " 分钟";
            }
            else
            {
                str = "定时:";
                ShowInfo = str + DtStart + "";
            }


            str += DtStart;


            var str1 = "";
            if (OpEnd == 1)
            {
                str1 = "开灯后:";
                ShowInfo += "  -  " + str1 + getStrInt(DtEnd) + " 分钟";
            }
            else
            {
                str1 = "定时:";
                ShowInfo += "  -  " + str1 + DtEnd + "";
            }
            str1 += DtEnd;

            int x1 = getStrInt(DtStart);
            int x2 = getStrInt(DtEnd);
            if (OpStart == OpEnd && x1 > x2)
            {
                if (OpStart == 1)
                {
                    ShowInfo += "  报警起始时间大于报警结束时间-错误设置";
                }
                else
                {
                    ShowInfo += " [第二天]";
                }

            }
            if (OpStart == 1 && OpEnd == 2)
            {
                if (OpEndDt == 2)
                    ShowInfo += " [第二天]";
                else
                    ShowInfo += " [开灯当天]";
            }
            if (NewRuleSectionVm.MySelf != null)
                NewRuleSectionVm.MySelf.Remark = ShowInfo;

            StrStarttime = str;
            StrEndtime = str1;

            if (OpStart == 1 && OpEnd == 2) IsEnableOpEndDt = true;
            else IsEnableOpEndDt = false;


        }


        //private string IdNDtStrStarttShowInfoimeEndame;
 
        //public string ShowInfo
        //{
        //    get { return IdNDtStrStarttShowInfoimeEndame; }
        //    set
        //    {
        //        if (value == IdNDtStrStarttShowInfoimeEndame) return;
        //        //  IdNDtEndame = value;

        //        IdNDtStrStarttShowInfoimeEndame = value;
        //        this.RaisePropertyChanged(() => this.ShowInfo);
        //    }
        //}

        private string IdNDtStrStarttimeEndame;

        /// <summary>
        /// 报警结束时间
        /// </summary>
        public string StrStarttime
        {
            get { return IdNDtStrStarttimeEndame; }
            set
            {
                if (value == IdNDtStrStarttimeEndame) return;
                //  IdNDtEndame = value;

                IdNDtStrStarttimeEndame = value;
                this.RaisePropertyChanged(() => this.StrStarttime);
            }
        }


        private string IdNDtEndamStrEndtimee;

        /// <summary>
        /// 报警结束时间
        /// </summary>
        public string StrEndtime
        {
            get { return IdNDtEndamStrEndtimee; }
            set
            {
                if (value == IdNDtEndamStrEndtimee) return;
                //  IdNDtEndame = value;

                IdNDtEndamStrEndtimee = value;
                this.RaisePropertyChanged(() => this.StrEndtime);
            }
        }
    }
}
