using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.iifx;
using Wlst.Ux.SinglePlan.Services;

namespace Wlst.Ux.SinglePlan.SinglePlan.ViewModel
{
    public partial class SluOneInstruction : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public SluOneInstruction(int areaId)
        {
            AreaId = areaId;
            InstructionId = 0;
            InstructionName = "新指令";
            InstructionDesc = "新指令";
            IsUsed = true;
        }

        public SluOneInstruction(Wlst.iifx.SluPlanBriefInfo.SluPlanItemBriefInfo item)
        {
            InstructionId = item.TimePlanId;
            InstructionName = item.TimePlanName;
            InstructionDesc = item.TimePlanDesc;
            InstructionTime = new DateTime( wlst.sr.iif.UtcTime.GetCsharpTime(item.DateCreate)).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public SluOneInstruction(Wlst.iifx.SluPlanDetailInfo info)
        {
            AreaId = info.AreaId;
            InstructionId = info.TimePlanId;
            InstructionName = info.TimePlanName;
            InstructionDesc = info.TimePlanDesc;
            IsUsed = info.TimePlanIsuesd;
            CmdType = info.CmdType;
            if (LightUsedRtuIdColl.Count > 0)
                CurrentSelectLightUsedRtuId = LightUsedRtuIdColl[0];
            foreach (var g in LightUsedRtuIdColl)
            {
                if (g.Value == info.LightUsedRtuId)
                {
                    CurrentSelectLightUsedRtuId = g;
                    break;
                }
            }
            CurrentSelectLightUsedRtuId.Value = info.LightUsedRtuId;
            OperationMethod = info.OpeMethod;
            if (info.OpeMethod == 1)
            {
                this.OperationArguHour = info.OpeArgu / 60;
                this.OperationArguMinute = info.OpeArgu % 60;
            }
            if (info.OpeMethod == 2 || info.OpeMethod == 3)
            {
                this.OperationArguOffset = info.OpeArgu;
            }
            if (info.OpeMethod == 11)
            {
                this.OperationArguLightStart = info.OpeArgu / 10000;

                this.OperationArguLightEnd = info.OpeArgu % 10000;

            }
            this.LightEndEffectHour = info.LuxEndEffect / 60;
            this.LightEndEffectMinute = info.LuxEndEffect % 60;

            this.LightStartEffectHour = info.LuxStartEffect / 60;
            this.LightStartEffectMinute = info.LuxStartEffect % 60;
            foreach (var g in this.OperationWeekSet)
            {
                if (info.UsedWeekSet.Contains(g.Value)) g.IsSelected = true;
                else g.IsSelected = false;
            }
            CmdPwmValue = info.Scale;
            for (int i = 0; i < 4; i++)
            {
                CmdPwmSel[i].IsSelected = info.LoopCanDo[i] == 1;
            }
        }

        private int _instructionId;

        /// <summary>
        ///方案Id  集中控制器的方案，方案地址由服务器设置，新增的方案地址全部为0提交服务器后服务器分配
        /// </summary>
        public int InstructionId
        {
            get { return _instructionId; }
            set
            {
                if (value != _instructionId)
                {
                    _instructionId = value;
                    this.RaisePropertyChanged(() => this.InstructionId);
                }
            }
        }

        private string _instructionName;

        /// <summary>
        /// 指令名称  
        /// </summary>

        [StringLength(30, ErrorMessage = "名称长度不能大于30")]
        [Required(ErrorMessage = "输入不得为空")]
        public string InstructionName
        {
            get { return _instructionName; }
            set
            {
                if (value != _instructionName)
                {
                    _instructionName = value;
                    this.RaisePropertyChanged(() => this.InstructionName);
                }
            }
        }

        [StringLength(30, ErrorMessage = "方案描述长度不能大于30")]
        [Required(ErrorMessage = "输入不得为空")]
        private string _instructionDesc;

        /// <summary>
        /// 指令描述
        /// </summary>
        public string InstructionDesc
        {
            get { return _instructionDesc; }
            set
            {
                if (value != _instructionDesc)
                {
                    _instructionDesc = value;
                    this.RaisePropertyChanged(() => this.InstructionDesc);
                }
            }
        }

        private string _instructionTime;
        /// <summary>
        /// 生成时间
        /// </summary>
        public string InstructionTime
        {
            get { return _instructionTime; }
            set
            {
                if (value != _instructionTime)
                {
                    _instructionTime = value;
                    this.RaisePropertyChanged(() => this.InstructionTime);
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

        private int _areaId;

        /// <summary>
        ///区域地址
        /// </summary>
        public int AreaId
        {
            get { return _areaId; }
            set
            {
                if (value != _areaId)
                {
                    _areaId = value;
                    this.RaisePropertyChanged(() => this.AreaId);
                }
            }
        }

        private void UpdateOperatorAboutTime()
        {
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
    /// 方案执行参数信息
    /// </summary>
    public partial class SluOneInstruction
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
                    if (value == 0)
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

                    _operationWeekSet.Add(new NameIntBool() { Name = "周一", Value = 1, IsSelected = false });
                    _operationWeekSet.Add(new NameIntBool() { Name = "周二", Value = 2, IsSelected = false });
                    _operationWeekSet.Add(new NameIntBool() { Name = "周三", Value = 3, IsSelected = false });
                    _operationWeekSet.Add(new NameIntBool() { Name = "周四", Value = 4, IsSelected = false });
                    _operationWeekSet.Add(new NameIntBool() { Name = "周五", Value = 5, IsSelected = false });
                    _operationWeekSet.Add(new NameIntBool() { Name = "周六", Value = 6, IsSelected = false });
                    _operationWeekSet.Add(new NameIntBool() { Name = "周日", Value = 0, IsSelected = false });

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


        private bool _isUseed;
        /// <summary>
        /// 使用状态
        /// </summary>
        public bool IsUsed
        {
            get { return _isUseed; }
            set
            {
                if (value != _isUseed)
                {
                    _isUseed = value;
                    this.RaisePropertyChanged(() => this.IsUsed);
                }
                IsUsedstr = value ? "启用" : "停用";
            }
        }

        private string _isUseeds;

        public string IsUsedstr
        {
            get { return _isUseeds; }
            set
            {
                if (value != _isUseeds)
                {
                    _isUseeds = value;
                    this.RaisePropertyChanged(() => this.IsUsedstr);
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
                    if (value < -60) value = -60;
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
                    _cLightUsedRtuIdColl.Add(new NameIntBool() { IsSelected = false, Name = "无", Value = 0 });
                    var nts =
                        (from t in
                             Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
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
                            Name = t.RtuPhyId.ToString("d4") + "-" + t.RtuName
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

            if (Mix2Enabl == false)
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
                if (CmdMix3 == 4) CmdMix3 = 1;
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


        private int _cCmdMixStr;

        /// <summary>
        ///  PWM操作 比例 1-11 ～0%-100%
        /// </summary>
        public int CmdPwmValue
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



        private string _cCmdMsdfixStr;

        /// <summary>
        ///  PWM操作 比例 1-11 ～0%-100%
        /// </summary>
        public string CmdPwmValueStrs
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

    /// <summary>
    /// Icommand
    /// </summary>
    public partial class SluOneInstruction
    {
        /// <summary>
        /// 保存指令
        /// </summary>
        #region CmdSaveInstruction

        private DateTime _dtCmdSaveInstruction;
        private ICommand _cmdSaveInstruction;

        public ICommand CmdSaveInstruction
        {
            get
            {
                return _cmdSaveInstruction ??
                       (_cmdSaveInstruction = new RelayCommand(ExCmdSaveInstruction, CanCmdSaveInstruction, true));
            }

        }

        private bool CanCmdSaveInstruction()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdSaveInstruction.Ticks > 10000000;
        }

        private void ExCmdSaveInstruction()
        {
            _dtCmdSaveInstruction = DateTime.Now;
            SaveInstruction();
        }

        #endregion

        /// <summary>
        /// 保存当前指令
        /// </summary>
        private void SaveInstruction()
        {
            var req = new SluPlanDetailInfo();
            req.AreaId = AreaId;
            req.TimePlanId = InstructionId;
            req.TimePlanName = InstructionName;
            req.TimePlanDesc = InstructionDesc;
            req.TimePlanIsuesd = IsUsed;
            req.DateCreate =wlst.sr.iif.UtcTime.GetUtcTime(DateTime.Now.Ticks);
            foreach (var week in OperationWeekSet)
            {
                if (week.IsSelected) req.UsedWeekSet.Add(week.Value);
            }
            req.OpeMethod = OperationMethod;

            if (OperationMethod == 1)
            {
                req.OpeArgu = OperationArguHour*60 + OperationArguMinute;
            }
            if (OperationMethod == 2 || OperationMethod == 3)
            {
                req.OpeArgu = OperationArguOffset;
            }
            if (OperationMethod == 11)
            {
                req.OpeArgu = OperationArguLightStart*10000 + OperationArguLightEnd;
            }
            req.LuxEndEffect = LightEndEffectHour*60 + LightEndEffectMinute;
            req.LuxStartEffect = LightStartEffectHour*60 + LightStartEffectMinute;
            req.CmdType = CmdType;
            req.Scale = CmdPwmValue;
            if (LightUsedRtuIdColl.Count > 0 && OperationMethod == 11)
                req.LightUsedRtuId = CurrentSelectLightUsedRtuId.Value;
            for (int i = 0; i < 4; i++)
            {
                req.LoopCanDo.Add(CmdPwmSel[i].IsSelected ? 1 : 0);;
            }
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp("post4075", System.Convert.ToBase64String(SluPlanDetailInfo.SerializeToBytes(req)));
            if (data == null) return;
            var res = CommAns.Deserialize(data);
            if (res.Head.IfSt != 1)
            {
                WlstMessageBox.Show("删除失败", "删除失败", WlstMessageBoxType.Ok);
                return;
            }
            WlstMessageBox.Show("删除成功", "删除成功", WlstMessageBoxType.Ok);
            SinglePlanViewModel._addOrModifyInstruction.Close();
            var args = new PublishEventArgs()
            {
                EventType = PublishEventType.Core,
                EventId = EventIdAssign.SaveInstructionId
            };
            EventPublish.PublishEvent(args);
        }
    }
}
