using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.Wj2090Module.SrInfo;
using Wlst.client;

namespace Wlst.Ux.Wj2090Module.TimeInfo.TimeInfoSet.ViewModel
{
    public partial class TimeInfoOneVm : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public TimeInfoOneVm()
        {
            CmdPwmValueStrs = "0%";
            SchemeId = 0;
            SchemeName = "新方案";
            SchemeDesc = "新方案描述";
            CmdType = 4;
            OperationMethod = 1;
            IsSluOrCtrlScheme = 1;
            OperationOrder = 1;
            LightEndEffectHour = 18;
            LightEndEffectMinute = 59;
            LightStartEffectHour = 18;
            LightStartEffectMinute = 1;

            OperationArguOffset = 15;
            OperationArguLightStart = 15;
            OperationArguLightEnd = 45;

            OperationArguHour = 18;
            OperationArguMinute = 10;

            CmdMix1 = 1;
            CmdMix2 = 1;
            CmdMix3 = 1;
            CmdMix4 = 1;
            CmdPwmValue = 90;

            IsNotUsed = false;

            if (LightUsedRtuIdColl.Count > 0)
                CurrentSelectLightUsedRtuId = LightUsedRtuIdColl[0];

            Ctrls = new Dictionary<int, SluTimeScheme.SluTimeSchemeItem.SluTimeCtrlSluOne >();
        }

        public TimeInfoOneVm(int areaId,SluTimeScheme.SluTimeSchemeItem info)
        {
            CmdPwmValueStrs = "0%";
            UsedSluCount = info.SluCtrls.Count;
            this.SchemeDesc = info.SchemeDesc;
            this.SchemeId = info.SchemeId;
            this.SchemeName = info.SchemeName;
            this.CmdType = info.SluTimePlanInfo.CmdType;
            this.IsSluOrCtrlScheme = info.IsSluOrCtrlScheme;
            this.IsNotUsed = info.IsNotUsed;
            
            //this.OperationArgu = info.OperationArgu;

            if (LightUsedRtuIdColl.Count > 0)
                CurrentSelectLightUsedRtuId = LightUsedRtuIdColl[0];
            foreach (var g in LightUsedRtuIdColl)
            {
                if (g.Value == info.SluTimePlanInfo.LightUsedRtuId)
                {
                    CurrentSelectLightUsedRtuId = g;
                    break;
                }
            }

            this.OperationMethod = info.SluTimePlanInfo.OperationMethod;
            if (info.SluTimePlanInfo.OperationMethod == 1)
            {
                this.OperationArguHour = info.SluTimePlanInfo.OperationArgu / 60;
                this.OperationArguMinute = info.SluTimePlanInfo.OperationArgu % 60;
            }
            if (info.SluTimePlanInfo.OperationMethod == 2)
            {
                this.OperationArguOffset = info.SluTimePlanInfo.OperationArgu;
            }
            //if (info.OperationMethod == 12)
            //{
            //    this.OperationArguOffset = info.OperationArgu;
            //}
            if (info.SluTimePlanInfo.OperationMethod == 11)
            {
                this.OperationArguLightStart = info.SluTimePlanInfo.OperationArgu / 10000;

                this.OperationArguLightEnd = info.SluTimePlanInfo.OperationArgu % 10000;

            }

            this.OperationOrder = info.SluTimePlanInfo.OperationOrder + 1;
            this.LightEndEffectHour = info.SluTimePlanInfo.LightEndEffect / 60;
            this.LightEndEffectMinute = info.SluTimePlanInfo.LightEndEffect % 60;

            this.LightStartEffectHour = info.SluTimePlanInfo.LightStartEffect / 60;
            this.LightStartEffectMinute = info.SluTimePlanInfo.LightStartEffect % 60;

            //this.LightEndEffect = info.LightEndEffect;
            //this.LightStartEffect = info.LightStartEffect;
            // this.LightUsedRtuId = info.LightUsedRtuId;
            foreach (var g in this.OperationWeekSet)
            {
                if (info.SluTimePlanInfo.OperationWeekSet.Contains(g.Value)) g.IsSelected = true;
                else g.IsSelected = false;
            }

            if (info.SluTimePlanInfo.CmdMix.Count > 3)
            {
                CmdMix1 = info.SluTimePlanInfo.CmdMix[0] + 1;
                CmdMix2 = info.SluTimePlanInfo.CmdMix[1] + 1;
                CmdMix3 = info.SluTimePlanInfo.CmdMix[2] + 1;
                CmdMix4 = info.SluTimePlanInfo.CmdMix[3] + 1;
            }



            CmdPwmValue = info.SluTimePlanInfo.CmdPwmScaleValue;


            for (int i = 0; i < 4; i++)
            {
                CmdPwmSel[i].IsSelected = info.SluTimePlanInfo.CmdPwmScale.Contains(i + 1);

            }

            if (info.SluTimePlanInfo.OperationMethod == 2 && info.SluTimePlanInfo.CmdType == 4)
            {
                bool openlight = false;
                bool closelight = false;
                foreach (var g in info.SluTimePlanInfo.CmdMix)
                {
                    if (g == 4)
                    {
                        closelight = true;
                    }
                    if (g == 1 || g == 2 || g == 3)
                    {
                        openlight = true;
                    }
                }
                if(openlight ==closelight )
                {
                    this.OperationMethod = 2;
                }
                else
                {
                    if (openlight) this.OperationMethod = 2;
                    else this.OperationMethod = 12;
                }
            }


            OnChanged();


            Ctrls = new Dictionary<int, SluTimeScheme.SluTimeSchemeItem.SluTimeCtrlSluOne >();
            var tu = new Tuple<int, int>(areaId, info.SchemeId);//lvfff
            foreach (
                var t in TimeInfos.MySelf.Info[tu].SluCtrls)   //var tu = new Tuple<int, int>(AreaId , g.GrpId);
            {
                if (Ctrls.ContainsKey(t.SluId)) Ctrls[t.SluId] = t;
                else Ctrls.Add(t.SluId, t);
            }
            UsedSluCount = Ctrls.Count;
            UpdateOperatorAboutTime();
        }

        public Dictionary<int, SluTimeScheme.SluTimeSchemeItem.SluTimeCtrlSluOne> Ctrls;

        public SluTimeScheme.SluTimeSchemeItem BackToSluTimeSchemeOne()
        {
            var infof = new SluTimeScheme.SluTimeSchemeItem()
                            {
                                SluTimePlanInfo=new SluTimeScheme.SluTimeSchemeItem.SluTimePlan()   
                                                    {
                                                             CmdType = this.CmdType,
                                LightEndEffect = LightEndEffectHour*60 + LightEndEffectMinute,
                                LightStartEffect = LightStartEffectHour*60 + LightStartEffectMinute,
                                LightUsedRtuId =
                                    CurrentSelectLightUsedRtuId == null ? 0 : CurrentSelectLightUsedRtuId.Value,
                                OperationOrder = OperationOrder - 1,
                                CmdMix = new List<int>(),
                                CmdPwmScale = new List<int>(),
                                OperationWeekSet = new List<int>(),
                                OperationArgu = 0,
                                OperationMethod = OperationMethod
                                                    },
                                IsSluOrCtrlScheme = IsSluOrCtrlScheme,
                                Nindex = 0,
                                // SluCtrls =Ctrls ,
                                SchemeName = SchemeName,
                                SchemeDesc = SchemeDesc,
                                SchemeId = SchemeId,
                                SchemeDescSec = "",
                                IsNotUsed =this .IsNotUsed ,
                                SluCtrls = new List<SluTimeScheme.SluTimeSchemeItem.SluTimeCtrlSluOne>(),
                            };
            if(OperationMethod ==12)
            {
                infof.SluTimePlanInfo.OperationMethod = 2;
            }
            infof.SluTimePlanInfo.CmdMix.Add(CmdMix1 - 1);
            infof.SluTimePlanInfo.CmdMix.Add(CmdMix2 - 1);
            infof.SluTimePlanInfo.CmdMix.Add(CmdMix3 - 1);
            infof.SluTimePlanInfo.CmdMix.Add(CmdMix4 - 1);
            //   int pwmvalue = CmdPwmValue - 1;
            foreach (var g in CmdPwmSel)
            {
                if (g.IsSelected) infof.SluTimePlanInfo.CmdPwmScale.Add(g.Value);
            }
            infof.SluTimePlanInfo.CmdPwmScaleValue = CmdPwmValue;

            foreach (var g in OperationWeekSet)
            {
                if (g.IsSelected) infof.SluTimePlanInfo.OperationWeekSet.Add(g.Value);
            }
            if (OperationMethod == 1)
            {
                infof.SluTimePlanInfo.OperationArgu = OperationArguHour * 60 + OperationArguMinute;
            }
            if (OperationMethod == 2||OperationMethod==12)
            {
                infof.SluTimePlanInfo.OperationArgu = OperationArguOffset;
            }
            if (OperationMethod == 11)
            {
                infof.SluTimePlanInfo.OperationArgu = OperationArguLightStart * 10000 + OperationArguLightEnd;
            }
            foreach (var g in Ctrls)
            {
                infof.SluCtrls.Add(g.Value);
            }


            return infof;
        }


        public Tuple<bool, string> OnChanged()
        {
            // var nts = (from t in OperationWeekSet where t.IsSelected select t.Value).ToList();
            OperationWeekSetStr = "";
            string[] bt = new string[7] { "一", "二", "三", "四", "五", "六", "日"};
            bool noselected = true;
            for (int i = 0; i < OperationWeekSet.Count; i++)
            {
                if (OperationWeekSet[i].IsSelected)
                {
                    OperationWeekSetStr += bt[i] + "、";
                    noselected = false;
                }
            }
            if (noselected)
            {
                return new Tuple<bool, string>(false, "未选择执行周期....");
            }
            if (OperationWeekSetStr.Length > 1)
            {
                OperationWeekSetStr = OperationWeekSetStr.Substring(0, OperationWeekSetStr.Length - 1);
            }

            if (OperationMethod == 11)
            {
                if (CurrentSelectLightUsedRtuId == null || CurrentSelectLightUsedRtuId.Value == 0)
                {
                    return new Tuple<bool, string>(false, "设置为光控操作，但未选择光控设备....");
                }
                int xst = LightStartEffectHour*60 + LightStartEffectMinute;
                int xed = LightEndEffectHour*60 + LightEndEffectMinute;
                if (xst >= xed)
                {
                    return new Tuple<bool, string>(false, "设置为光控操作，但光控有效起始时间大于结束时间....");
                }
                int xgt = xed - xst;
                if (xgt < 10)
                {
                    return new Tuple<bool, string>(false, "设置为光控操作，但光控有效作用时间太短不到10分钟....");
                }

            }
            if (CmdType == 4)
            {
                if (CmdMix1 == 1 && CmdMix2 == 1 && CmdMix3 == 1 && CmdMix4 == 1)
                {
                    return new Tuple<bool, string>(false, "设置为混合控制，但所有灯头均无操作....");
                }
            }
            if (CmdType == 5)
            {
                var noselecteds = true;
                foreach (var t in this.CmdPwmSel)
                {
                    if (t.IsSelected) noselecteds = false;
                }
                if (noselecteds)
                {
                    return new Tuple<bool, string>(false, "设置为调光调节，但所有灯头均无操作....");
                }
            }

            return new Tuple<bool, string>(true, "");
        }



        private string _markeds;


        public string Marked
        {
            get { return _markeds; }
            set
            {
                if (value != _markeds)
                {
                    _markeds = value;
                    this.RaisePropertyChanged(() => this.Marked);
                }
            }
        }

      

        private string _maOperatorAboutTimerkeds;


        public string OperatorAboutTime
        {
            get { return _maOperatorAboutTimerkeds; }
            set
            {
                if (value != _maOperatorAboutTimerkeds)
                {
                    _maOperatorAboutTimerkeds = value;
                    this.RaisePropertyChanged(() => this.OperatorAboutTime);
                }
            }
        }

        private void UpdateOperatorAboutTime()
        {
            //this.OperationMethod = info.OperationMethod;
            //if (info.OperationMethod == 1)
            //{
            //    this.OperationArguHour = info.OperationArgu / 60;
            //    this.OperationArguMinute = info.OperationArgu % 60;
            //}
            //if (info.OperationMethod == 2)
            //{
            //    this.OperationArguOffset = info.OperationArgu;
            //}
            //if (info.OperationMethod == 11)
            //{
            //    this.OperationArguLight = info.OperationArgu;
            //}

            //this.OperationOrder = info.OperationOrder + 1;
            //this.LightEndEffectHour = info.LightEndEffect / 60;
            //this.LightEndEffectMinute = info.LightEndEffect % 60;

            //this.LightStartEffectHour = info.LightStartEffect / 60;
            //this.LightStartEffectMinute = info.LightStartEffect % 60;

            if (OperationMethod == 1)
            {
                OperatorAboutTime = OperationArguHour.ToString("D2") + ":" + OperationArguMinute.ToString("D2");
            }
            else if (OperationMethod == 2)
            {
                OperatorAboutTime = "日落偏移:" + OperationArguOffset;
            }
            else if (OperationMethod == 11)
            {
                OperatorAboutTime = LightStartEffectHour.ToString("D2") + ":" + LightStartEffectMinute.ToString("D2") +
                                    " - "
                                    + LightEndEffectHour.ToString("D2") + ":" + LightEndEffectMinute.ToString("D2");
            }
            else if (OperationMethod == 12)
            {
                OperatorAboutTime = "日出偏移:" + OperationArguOffset;
            }
            else OperatorAboutTime = "--";
        }
    }

    /// <summary>
    /// 方案控制终端信息
    /// </summary>
    public partial class TimeInfoOneVm
    {
       
        private int _usedSluCount;

        /// <summary>
        ///使用该方案的控制器的个数
        /// </summary>
        public int UsedSluCount
        {
            get { return _usedSluCount; }
            set
            {
                if (value != _usedSluCount)
                {
                    _usedSluCount = value;
                    this.RaisePropertyChanged(() => this.UsedSluCount);
                }
            }
        }
    }

    /// <summary>
    /// 方案扼要信息
    /// </summary>
    public partial class TimeInfoOneVm
    {

        private int _schemeId;

        /// <summary>
        ///方案Id  集中控制器的方案，方案地址由服务器设置，新增的方案地址全部为0提交服务器后服务器分配
        /// </summary>
        public int SchemeId
        {
            get { return _schemeId; }
            set
            {
                if (value != _schemeId)
                {
                    _schemeId = value;
                    this.RaisePropertyChanged(() => this.SchemeId);
                }
            }
        }


        private int _isSluOrCtrlScheme;

        /// <summary>
        ///1、集中器方案，2、控制器方案，3、同为集中器和控制器的方案  其他不处理 直接删除
        /// </summary>
        public int IsSluOrCtrlScheme
        {
            get { return _isSluOrCtrlScheme; }
            set
            {
                if (value != _isSluOrCtrlScheme)
                {
                    _isSluOrCtrlScheme = value;
                    this.RaisePropertyChanged(() => this.IsSluOrCtrlScheme);
                    IsSluOrCtrlSchemeStr = "未知";
                    if (value == 1) IsSluOrCtrlSchemeStr = "集中器";
                    if (value == 2) IsSluOrCtrlSchemeStr = "控制器";
                    if (value == 3) IsSluOrCtrlSchemeStr = "集中&控制";
                }
            }
        }

        private string isSluOrCtrlSchemeStr;
        public string IsSluOrCtrlSchemeStr
        {
            get { return isSluOrCtrlSchemeStr; }
            set
            {
                if (value != isSluOrCtrlSchemeStr)
                {
                    isSluOrCtrlSchemeStr = value;
                    this.RaisePropertyChanged(() => this.IsSluOrCtrlSchemeStr);
                }
            }
        }

        private string _schemeName;

        /// <summary>
        /// 方案名称  
        /// </summary>
        
        [StringLength(30,ErrorMessage="名称长度不能大于30")]
        [Required (ErrorMessage ="输入不得为空")]
        public string SchemeName
        {
            get { return _schemeName; }
            set
            {
                if (value != _schemeName)
                {
                    _schemeName = value;
                    this.RaisePropertyChanged(() => this.SchemeName);
                }
            }
        }

        [StringLength(30, ErrorMessage = "方案描述长度不能大于30")]
        [Required(ErrorMessage = "输入不得为空")]
        private string _schemeDesc;

        /// <summary>
        /// 方案描述
        /// </summary>
        public string SchemeDesc
        {
            get { return _schemeDesc; }
            set
            {
                if (value != _schemeDesc)
                {
                    _schemeDesc = value;
                    this.RaisePropertyChanged(() => this.SchemeDesc);
                }
            }
        }



        ///// <summary>
        ///// 方案描述
        ///// </summary>
        //public string SchemeDescSec
        //{
        //    get { return _attachRtuId; }
        //    set
        //    {
        //        if (value != _attachRtuId)
        //        {
        //            _attachRtuId = value;
        //            this.RaisePropertyChanged(() => this.AttachRtuId);
        //        }
        //    }
        //}






    }

    /// <summary>
    /// 方案执行参数信息
    /// </summary>
    public partial class TimeInfoOneVm
    {
        private int _operationOrder;

        /// <summary>
        /// （集中器：操作顺序 0-广播，1-依次）（控制器：数据类型 0-base，1-adv）
        /// </summary>
        public int OperationOrder
        {
            get { return _operationOrder; }
            set
            {
                if (value != _operationOrder)
                {
                    _operationOrder = value;
                    this.RaisePropertyChanged(() => this.OperationOrder);
                }
            }
        }

        private int _operationMethod;

        /// <summary>
        /// 指令类型 0-清除(发送到控制器时无此值)，1-定时，2-经纬度，3-即时 ,11 - 光控  12经纬度关灯
        /// </summary>
        public int OperationMethod
        {
            get { return _operationMethod; }
            set
            {
                if (value != _operationMethod)
                {
                    _operationMethod = value;
                    this.RaisePropertyChanged(() => this.OperationMethod);

                    OperationMethodStr = "未知";
                    if(value ==0)
                    {
                        OperationMethodStr = "清除";
                    }
                    if (value == 1)
                    {
                        OperationMethodStr = "定时";
                    }
                    if (value == 2)
                    {
                        OperationMethodStr = "日落时间";
                    }
                    if (value == 3)
                    {
                        OperationMethodStr = "即时";
                    }
                    if (value == 11)
                    {
                        OperationMethodStr = "光控";
                    }
                    if (value == 12)
                    {
                        OperationMethodStr = "日出时间";
                    }
                    UpdateOperatorAboutTime();
                    UpdatEnable();
                }
            }
        }


        private string operationMethodStr;
        public string OperationMethodStr
        {
            get { return operationMethodStr; }
            set
            {
                if (value != operationMethodStr)
                {
                    operationMethodStr = value;
                    this.RaisePropertyChanged(() => this.OperationMethodStr);
                }
            }
        }

        private ObservableCollection<NameIntBool> _operationWeekSet = null;

        /// <summary>
        /// 一周需要操作的时间 0周日 1 周一  如果包含就操作 不包含就不操作
        /// </summary>
        public ObservableCollection<NameIntBool> OperationWeekSet
        {
            get
            {
                if (_operationWeekSet == null)
                {
                    _operationWeekSet = new ObservableCollection<NameIntBool>();

                    _operationWeekSet.Add(new NameIntBool() {Name = "周一", Value = 1, IsSelected = false});
                    _operationWeekSet.Add(new NameIntBool() {Name = "周二", Value = 2, IsSelected = false});
                    _operationWeekSet.Add(new NameIntBool() {Name = "周三", Value = 3, IsSelected = false});
                    _operationWeekSet.Add(new NameIntBool() {Name = "周四", Value = 4, IsSelected = false});
                    _operationWeekSet.Add(new NameIntBool() {Name = "周五", Value = 5, IsSelected = false});
                    _operationWeekSet.Add(new NameIntBool() {Name = "周六", Value = 6, IsSelected = false});
                    _operationWeekSet.Add(new NameIntBool() {Name = "周日", Value = 0, IsSelected = false});

                }
                return _operationWeekSet;
            }
        }


        private string cOperationWeekSetStr;
        public string OperationWeekSetStr
        {
            get { return cOperationWeekSetStr; }
            set
            {
                if (value != cOperationWeekSetStr)
                {
                    cOperationWeekSetStr = value;
                    this.RaisePropertyChanged(() => this.OperationWeekSetStr);
                }
            }
        }

        #region  操作参数
        private int _operationArguHour;

        /// <summary>
        /// 定时则为操作时间，经纬度则为偏移，光控则为光控值
        /// </summary>
        public int OperationArguHour
        {
            get { return _operationArguHour; }
            set
            {
                if (value != _operationArguHour)
                {
                    if (value < 0) value = 0;
                    if (value > 23) value = 23;
                    _operationArguHour = value;
                    this.RaisePropertyChanged(() => this.OperationArguHour); UpdateOperatorAboutTime();
                }
            }
        }


        private bool  _isNOtUseed;

        public bool IsNotUsed
        {
            get { return _isNOtUseed; }
            set
            {
                if (value != _isNOtUseed)
                {
                    _isNOtUseed = value;
                    this.RaisePropertyChanged(() => this.IsNotUsed);
                }
                IsNotUsedstr = value ? "停用" : "启用";
            }
        }

        private string  _isNOtUseeds;

        public string  IsNotUsedstr
        {
            get { return _isNOtUseeds; }
            set
            {
                if (value != _isNOtUseeds)
                {
                    _isNOtUseeds = value;
                    this.RaisePropertyChanged(() => this.IsNotUsedstr);
                }
            }
        }


        private int _operationArguMinute;

        /// <summary>
        /// 定时则为操作时间，经纬度则为偏移，光控则为光控值
        /// </summary>
        public int OperationArguMinute
        {
            get { return _operationArguMinute; }
            set
            {
                if (value != _operationArguMinute)
                {
                    if (value < 0) value = 0;
                    if (value > 59) value = 59;
                    _operationArguMinute = value;
                    this.RaisePropertyChanged(() => this.OperationArguMinute); UpdateOperatorAboutTime();
                }
            }
        }
        private int _operationArguOffset;

        /// <summary>
        /// 定时则为操作时间，经纬度则为偏移，光控则为光控值
        /// </summary>
        public int OperationArguOffset
        {
            get { return _operationArguOffset; }
            set
            {
                if (value != _operationArguOffset)
                {
                    if (value < -60) value =-60;
                    if (value > 60) value = 60;

                    _operationArguOffset = value;
                    this.RaisePropertyChanged(() => this.OperationArguOffset); UpdateOperatorAboutTime();
                }
            }
        }
        private int _operationArguLights;

        /// <summary>
        /// 定时则为操作时间，经纬度则为偏移，光控则为光控值
        /// </summary>
        public int OperationArguLightStart
        {
            get { return _operationArguLights; }
            set
            {
                if (value != _operationArguLights)
                {
                    if (value < 3) value = 3;
                    if (value > 9999) value = 9999;
                    _operationArguLights = value;
                    this.RaisePropertyChanged(() => this.OperationArguLightStart); UpdateOperatorAboutTime();
                }
            }
        }


        private int _operationArguLightee;

        /// <summary>
        /// 定时则为操作时间，经纬度则为偏移，光控则为光控值
        /// </summary>
        public int OperationArguLightEnd
        {
            get { return _operationArguLightee; }
            set
            {
                if (value != _operationArguLightee)
                {
                    if (value < 3) value = 3;
                    if (value > 9999) value = 9999;
                    _operationArguLightee = value;
                    this.RaisePropertyChanged(() => this.OperationArguLightEnd);
                    UpdateOperatorAboutTime();
                }
            }
        }

        #endregion

        private int _lightStartEffecthour;

        /// <summary>
        /// 光控有效开始时间 
        /// </summary>
        public int LightStartEffectHour
        {
            get { return _lightStartEffecthour; }
            set
            {
                if (value != _lightStartEffecthour)
                {
                    if (value < 0) value = 0;
                    if (value > 23) value = 23;
                    _lightStartEffecthour = value;
                    this.RaisePropertyChanged(() => this.LightStartEffectHour); UpdateOperatorAboutTime();
                }
            }
        }
        private int _lightStartEffectminu;

        /// <summary>
        /// 光控有效开始时间 
        /// </summary>
        public int LightStartEffectMinute
        {
            get { return _lightStartEffectminu; }
            set
            {
                if (value != _lightStartEffectminu)
                {
                    if (value < 0) value = 0;
                    if (value > 59) value = 59;
                    _lightStartEffectminu = value;
                    this.RaisePropertyChanged(() => this.LightStartEffectMinute); UpdateOperatorAboutTime();
                }
            }
        }

        private int _lightEndEffecthour;

        /// <summary>
        /// 光控有效结束时间 
        /// </summary>
        public int LightEndEffectHour
        {
            get { return _lightEndEffecthour; }
            set
            {
                if (value != _lightEndEffecthour)
                {
                    if (value < 1) value = 1;
                    if (value > 23) value = 23;
                    _lightEndEffecthour = value;
                    this.RaisePropertyChanged(() => this.LightEndEffectHour); UpdateOperatorAboutTime();
                }
            }
        } private int _lightEndEffectminute;

        /// <summary>
        /// 光控有效结束时间 
        /// </summary>
        public int LightEndEffectMinute
        {
            get { return _lightEndEffectminute; }
            set
            {
                if (value != _lightEndEffectminute)
                {
                    if (value < 0) value = 0;
                    if (value > 59) value = 59;
                    _lightEndEffectminute = value;
                    this.RaisePropertyChanged(() => this.LightEndEffectMinute); UpdateOperatorAboutTime();
                }
            }
        }


        private ObservableCollection<NameIntBool> _cLightUsedRtuIdColl;

        /// <summary>
        ///光控开灯所使用的光控地址
        /// </summary>
        public ObservableCollection<NameIntBool> LightUsedRtuIdColl
        {
            get
            {
                if (_cLightUsedRtuIdColl == null)
                {
                    _cLightUsedRtuIdColl = new ObservableCollection<NameIntBool>();
                    _cLightUsedRtuIdColl.Add(new NameIntBool() {IsSelected = false, Name = "无", Value = 0});
                    var nts =
                        (from t in
                             Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems 
                         where
                             t.Key > Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.LuxStrt &&
                             t.Key < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.LuxEnd
                         orderby t.Key ascending
                         select t.Value).ToList();
                    foreach (var t in nts)
                    {
                        _cLightUsedRtuIdColl.Add(new NameIntBool()
                                                     {
                                                         IsSelected = false,
                                                         Value = t.RtuId,
                                                         Name =t.RtuPhyId .ToString( "d4")+"-"+ t.RtuName 
                                                     });
                    }


                }
                return _cLightUsedRtuIdColl;
            }
        }

        private NameIntBool _lightUsedRtuId;

        /// <summary>
        /// 光控开灯所使用的光控地址 
        /// </summary>
        public NameIntBool CurrentSelectLightUsedRtuId
        {
            get { return _lightUsedRtuId; }
            set
            {
                if (value != _lightUsedRtuId)
                {
                    _lightUsedRtuId = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectLightUsedRtuId);
                }
            }
        }

        void UpdatEnable()
        {
            if (this.OperationMethod == 1)
            {
                this.Mix1Enabl = true;
                this.Mix2Enabl = true;
                this.Mix3Enabl = true;
                this.Mix4Enabl = true;
                this.Mix5Enabl = true;
            }
            if (this.OperationMethod == 2)
            {
                this.Mix1Enabl = true;
                this.Mix2Enabl = true;
                this.Mix3Enabl = true;
                this.Mix4Enabl = true;
                this.Mix5Enabl = false;
            }
            if (this.OperationMethod == 12)
            {
                this.Mix1Enabl = true;
                this.Mix2Enabl = false;
                this.Mix3Enabl = false;
                this.Mix4Enabl = false;
                this.Mix5Enabl = true;
            }
            if (this.OperationMethod == 11)
            {
                this.Mix1Enabl = true;
                this.Mix2Enabl = true;
                this.Mix3Enabl = false;
                this.Mix4Enabl = false;
                this.Mix5Enabl = true;
            }

            if(Mix2Enabl ==false )
            {
                if (CmdMix1 == 2) CmdMix1 = 1;
                if (CmdMix2 == 2) CmdMix2 = 1;
                if (CmdMix3 == 2) CmdMix3 = 1;
                if (CmdMix4 == 2) CmdMix4 = 1;
            }
            if (Mix3Enabl == false)
            {
                if (CmdMix1 == 3) CmdMix1 = 1;
                if (CmdMix2 == 3) CmdMix2 = 1;
                if (CmdMix3 == 3) CmdMix3 = 1;
                if (CmdMix4 == 3) CmdMix4 = 1;
            }
            if (Mix4Enabl == false)
            {
                if (CmdMix1 == 4) CmdMix1 = 1;
                if (CmdMix2 == 4) CmdMix2 = 1;
                if (CmdMix3 ==4) CmdMix3 = 1;
                if (CmdMix4 == 4) CmdMix4 = 1;
            }
            if (Mix5Enabl == false)
            {
                if (CmdMix1 == 5) CmdMix1 = 1;
                if (CmdMix2 == 5) CmdMix2 = 1;
                if (CmdMix3 == 5) CmdMix3 = 1;
                if (CmdMix4 == 5) CmdMix4 = 1;
            }
        }

        #region sienable mix

        private bool mix1enable;
        public bool Mix1Enabl
        {
            get { return mix1enable; }
            set
            {
                if (value == mix1enable) return;
                mix1enable = value;
                this.RaisePropertyChanged(() => this.Mix1Enabl);
            }
        }



        private bool mix2enable;
        public bool Mix2Enabl
        {
            get { return mix2enable; }
            set
            {
                if (value == mix2enable) return;
                mix2enable = value;
                this.RaisePropertyChanged(() => this.Mix2Enabl);
            }
        }

        private bool mix3enable;
        public bool Mix3Enabl
        {
            get { return mix3enable; }
            set
            {
                if (value == mix3enable) return;
                mix3enable = value;
                this.RaisePropertyChanged(() => this.Mix3Enabl);
            }
        }

        private bool mix4enable;
        public bool Mix4Enabl
        {
            get { return mix4enable; }
            set
            {
                if (value == mix4enable) return;
                mix4enable = value;
                this.RaisePropertyChanged(() => this.Mix4Enabl);
            }
        }

        private bool mix5enable;
        public bool Mix5Enabl
        {
            get { return mix5enable; }
            set
            {
                if (value == mix5enable) return;
                mix5enable = value;
                this.RaisePropertyChanged(() => this.Mix5Enabl);
            }
        }
        #endregion

        private int _cmdtype;

        /// <summary>
        ///操作类型 4-混合控制，5-pwm调节，6-485调节 不用
        /// </summary>
        public int CmdType
        {
            get { return _cmdtype; }
            set
            {
                if (value != _cmdtype)
                {
                    _cmdtype = value;
                    this.RaisePropertyChanged(() => this.CmdType);
                    CmdTypeStr = "未知";
                    if (value == 4) CmdTypeStr = "开关灯控制";
                    if (value == 5) CmdTypeStr = "调光控制";
                }
            }
        }
        private string cmdTypeStr;
        public string CmdTypeStr
        {
            get { return cmdTypeStr; }
            set
            {
                if (value != cmdTypeStr)
                {
                    cmdTypeStr = value;
                    this.RaisePropertyChanged(() => this.CmdTypeStr);
                }
            }
        }

        #region CmdMix1 混合控制

        private int _cmdMixOperator1;

        /// <summary>
        /// 混合控制 回路操作 1-不操作，2-开灯，3-1档节能，4-2档节能，5-关灯 ,值都加1
        /// </summary>
        public int CmdMix1
        {
            get { return _cmdMixOperator1; }
            set
            {
                if (value != _cmdMixOperator1)
                {
                    _cmdMixOperator1 = value;
                    this.RaisePropertyChanged(() => this.CmdMix1);
                }
            }
        }

        private int _cmdMixOperator2;

        /// <summary>
        /// 混合控制 回路操作 1-不操作，2-开灯，3-1档节能，4-2档节能，5-关灯 ,值都加1
        /// </summary>
        public int CmdMix2
        {
            get { return _cmdMixOperator2; }
            set
            {
                if (value != _cmdMixOperator2)
                {
                    _cmdMixOperator2 = value;
                    this.RaisePropertyChanged(() => this.CmdMix2);
                }
            }
        }

        private int _cmdMixOperator3;

        /// <summary>
        /// 混合控制 回路操作 1-不操作，2-开灯，3-1档节能，4-2档节能，5-关灯 ,值都加1
        /// </summary>
        public int CmdMix3
        {
            get { return _cmdMixOperator3; }
            set
            {
                if (value != _cmdMixOperator3)
                {
                    _cmdMixOperator3 = value;
                    this.RaisePropertyChanged(() => this.CmdMix3);
                }
            }
        }

        private int _cmdMixOperator4;

        /// <summary>
        /// 混合控制 回路操作 1-不操作，2-开灯，3-1档节能，4-2档节能，5-关灯 ,值都加1
        /// </summary>
        public int CmdMix4
        {
            get { return _cmdMixOperator4; }
            set
            {
                if (value != _cmdMixOperator4)
                {
                    _cmdMixOperator4 = value;
                    this.RaisePropertyChanged(() => this.CmdMix4);
                }
            }
        }
        #endregion


        #region CmdPwmScale 

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool> _CmdPwmSel = null;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameIntBool> CmdPwmSel
        {
            get
            {
                if (_CmdPwmSel == null)
                {
                    _CmdPwmSel = new ObservableCollection<NameIntBool>();
                    for (int i = 1; i < 5; i++)
                        _CmdPwmSel.Add(new NameIntBool() { IsSelected = false, Name = "灯头 " + i, Value = i });

                }
                return _CmdPwmSel;
            }
        }


        private int  _cCmdMixStr;
        
        /// <summary>
        ///  PWM操作 比例 1-11 ～0%-100%
        /// </summary>
        public int  CmdPwmValue
        {
            get { return _cCmdMixStr; }
            set
            {
                if (value != _cCmdMixStr)
                {
                    if (value < 0) value = 0;
                    _cCmdMixStr = value;
                    this.RaisePropertyChanged(() => this.CmdPwmValue);
                    if (value == 0)
                    {
                        CmdPwmValueStrs = "0%";
                    }
                    else
                    {
                        CmdPwmValueStrs = value + "%";
                    }
                }
            }
        }



        private string  _cCmdMsdfixStr;

        /// <summary>
        ///  PWM操作 比例 1-11 ～0%-100%
        /// </summary>
        public string  CmdPwmValueStrs
        {
            get { return _cCmdMsdfixStr; }
            set
            {
                if (value != _cCmdMsdfixStr)
                {
                    _cCmdMsdfixStr = value;
                    this.RaisePropertyChanged(() => this.CmdPwmValueStrs);
                }
            }
        }

        #endregion

    }




}
