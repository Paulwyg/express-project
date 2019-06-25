// This is the unsafe code for reading and writing


// DO EDIT IF ANY ERROR.
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Wlst.iif
{
    public partial class Head
    {
    public Head()
    {
    Idx = 0;
    Ver = 0;
    IfName = string .Empty ;
    IfDt = 0;
    IfSt = 0;
    IfMsg = string .Empty ;
    AddrIds = new List<int>();
    AddrLds = new List<long>();
    PagingNum = 0;
    PagingIdx = 0;
    PagingTotal = 0;
    PagingRecordTotal = 0;
    PagingFlag = 0;
    }
    }
    public partial class CommAns
    {
    public CommAns()
    {
    Head = new Wlst.iif.Head();
    }
    }
    public partial class RtuRunningInfo
    {
    public RtuRunningInfo()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.RtuRunningInfo.RtuRunningInfoOne>();
    RunningError = new Wlst.iif.RtuRunningInfo.RtuRunningError();
    }
    }
    public partial class GtOnline
    {
    public GtOnline()
    {
    Head = new Wlst.iif.Head();
    DateStart = 0;
    DateEnd = 0;
    RtuIds = new List<int>();
    }
    }
    public partial class Online
    {
    public Online()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.Online.OnlineItem>();
    }
    }
    public partial class UserInfoBk
    {
    public UserInfoBk()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.UserInfoBk.UserInfo>();
    RequestUserIds = new List<string>();
    }
    }
    public partial class UserInfoRq
    {
    public UserInfoRq()
    {
    RequestUserIds = new List<string>();
    }
    }
    public partial class AddOrUpdateUserInfo
    {
    public AddOrUpdateUserInfo()
    {
    Head = new Wlst.iif.Head();
    UserId = string .Empty ;
    UserName = string .Empty ;
    UserTel = string .Empty ;
    UserDept = string .Empty ;
    UserRole = string .Empty ;
    UserPw = string .Empty ;
    UserRoleId = 0;
    }
    }
    public partial class DelUserInfo
    {
    public DelUserInfo()
    {
    Head = new Wlst.iif.Head();
    UserId = string .Empty ;
    }
    }
    public partial class ResetUserPsw
    {
    public ResetUserPsw()
    {
    Head = new Wlst.iif.Head();
    UserId = string .Empty ;
    UserPw = string .Empty ;
    }
    }
    public partial class UpdateUserPsw
    {
    public UpdateUserPsw()
    {
    UserId = string .Empty ;
    UserPwOld = string .Empty ;
    UserPwNew = string .Empty ;
    }
    }
    public partial class RoleInfoBk
    {
    public RoleInfoBk()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.RoleInfoBk.RoleInfo>();
    RequestRoleIds = new List<int>();
    }
    }
    public partial class RoleInfoRq
    {
    public RoleInfoRq()
    {
    RequestRoleIds = new List<int>();
    }
    }
    public partial class AddRoleInfo
    {
    public AddRoleInfo()
    {
    Head = new Wlst.iif.Head();
    Info = new Wlst.iif.RoleInfoBk.RoleInfo();
    }
    }
    public partial class DelRoleInfo
    {
    public DelRoleInfo()
    {
    Head = new Wlst.iif.Head();
    RoleId = 0;
    }
    }
    public partial class RoleUserInfoRq
    {
    public RoleUserInfoRq()
    {
    RequestUserName = string .Empty ;
    }
    }
    public partial class RoleUserInfoBk
    {
    public RoleUserInfoBk()
    {
    Head = new Wlst.iif.Head();
    UserRole = new Wlst.iif.RoleInfoBk.RoleInfo();
    }
    }
    public partial class DelPlanBatchOperationInfo
    {
    public DelPlanBatchOperationInfo()
    {
    PlanId = 0;
    }
    }
    public partial class AddPlanBatchOperationInfo
    {
    public AddPlanBatchOperationInfo()
    {
    PlanName = string .Empty ;
    ItemsPlan = new List<Wlst.iif.AddPlanBatchOperationInfo.PlanInfoAdd>();
    }
    }
    public partial class PlanBatchOperationInfo
    {
    public PlanBatchOperationInfo()
    {
    PlanId = 0;
    PlanName = string .Empty ;
    ItemsPlan = new List<Wlst.iif.PlanBatchOperationInfo.PlanInfo>();
    }
    }
    public partial class PlanBatchOperationInfoBk
    {
    public PlanBatchOperationInfoBk()
    {
    Head = new Wlst.iif.Head();
    PlanIds = new List<int>();
    ItemsPlan = new List<Wlst.iif.PlanBatchOperationInfo>();
    }
    }
    public partial class PlanBatchOperationInfoRq
    {
    public PlanBatchOperationInfoRq()
    {
    Op = 0;
    PlanIds = new List<int>();
    }
    }
    public partial class PlanRtuTimetableInfo
    {
    public PlanRtuTimetableInfo()
    {
    ItemsSelect = new List<Wlst.iif.PlanRtuTimetableInfo.PlanTimeTableInfo>();
    }
    }
    public partial class PlanRtuTimetableInfoBk
    {
    public PlanRtuTimetableInfoBk()
    {
    Head = new Wlst.iif.Head();
    ItemsPlan = new List<Wlst.iif.PlanRtuTimetableInfoBk.PlanGrpInfo>();
    }
    }
    public partial class PlanGrpTransRtu
    {
    public PlanGrpTransRtu()
    {
    ItemsPlan = new List<Wlst.iif.PlanGrpTransRtu.PlanGrpInfo>();
    }
    }
    public partial class PlanGrpTransRtuBk
    {
    public PlanGrpTransRtuBk()
    {
    Head = new Wlst.iif.Head();
    PlanRtuItems = new List<Wlst.iif.PlanGrpTransRtuBk.PlanGrpTransRtuBkItem>();
    }
    }
    public partial class QueryRtuBiref
    {
    public QueryRtuBiref()
    {
    RtuLikeQyert = string .Empty ;
    AreaId = 0;
    GroupId = 0;
    OpRtuType = 0;
    Op = 0;
    }
    }
    public partial class QueryRtuBirefBk
    {
    public QueryRtuBirefBk()
    {
    Head = new Wlst.iif.Head();
    QueryInfo = new Wlst.iif.QueryRtuBiref();
    ItemsArea = new List<Wlst.iif.QueryRtuBirefBk.AreaInfoBrief>();
    ItemsGroup = new List<Wlst.iif.QueryRtuBirefBk.GroupInfoBreif>();
    ItemsRtu = new List<Wlst.iif.QueryRtuBirefBk.RtuInfoBiref>();
    }
    }
    public partial class RtuInfo
    {
    public RtuInfo()
    {
    RtuId = 0;
    RtuPhyId = 0;
    RtuName = string .Empty ;
    RtuModel = 0;
    RtuUsed = 0;
    RtuFid = 0;
    RtuType = 0;
    Right = 0;
    CurrentSum = 0;
    ErrorCount = 0;
    ImageType = 0;
    RtuFname = string .Empty ;
    RtuFphyId = 0;
    AreaId = 0;
    AreaName = string .Empty ;
    }
    }
    public partial class AreaInfo
    {
    public AreaInfo()
    {
    Head = new Wlst.iif.Head();
    AreaId = 0;
    OpRtuType = new List<int>();
    AreaName = string .Empty ;
    ItemsRtu = new List<Wlst.iif.RtuInfo>();
    }
    }
    public partial class GroupInfo
    {
    public GroupInfo()
    {
    Head = new Wlst.iif.Head();
    AreaId = 0;
    GroupId = 0;
    OpRtuType = new List<int>();
    GroupName = string .Empty ;
    ItemsRtu = new List<Wlst.iif.RtuInfo>();
    AreaName = string .Empty ;
    }
    }
    public partial class AreasInfo
    {
    public AreasInfo()
    {
    Head = new Wlst.iif.Head();
    Op = 0;
    ItemsArea = new List<Wlst.iif.AreaInfo>();
    OpRtuType = new List<int>();
    AreaRight = 0;
    }
    }
    public partial class GroupsInfo
    {
    public GroupsInfo()
    {
    Head = new Wlst.iif.Head();
    Op = 0;
    AreaId = new List<int>();
    ItemsGroup = new List<Wlst.iif.GroupInfo>();
    OpRtuType = new List<int>();
    }
    }
    public partial class SluCtrlAndGrpbriefInfo
    {
    public SluCtrlAndGrpbriefInfo()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    GroupIds = new List<int>();
    ItemsGroup = new List<Wlst.iif.SluCtrlAndGrpbriefInfo.SluGroupbriefInfoItem>();
    }
    }
    public partial class Search
    {
    public Search()
    {
    AreaId = new List<int>();
    OpRtuType = new List<int>();
    SearchKey = string .Empty ;
    }
    }
    public partial class SearchBk
    {
    public SearchBk()
    {
    Head = new Wlst.iif.Head();
    ItemsRtu = new List<Wlst.iif.RtuInfo>();
    }
    }
    public partial class AddToArea
    {
    public AddToArea()
    {
    AreaId = 0;
    ItemsRtu = new List<int>();
    }
    }
    public partial class AddToGroup
    {
    public AddToGroup()
    {
    AreaId = 0;
    GroupId = 0;
    ItemsRtu = new List<int>();
    }
    }
    public partial class UpdateAreaOrGrpName
    {
    public UpdateAreaOrGrpName()
    {
    AreaId = 0;
    GroupId = 0;
    NameNew = string .Empty ;
    }
    }
    public partial class UpdateAreaOrGrpNameBk
    {
    public UpdateAreaOrGrpNameBk()
    {
    Head = new Wlst.iif.Head();
    AreaId = 0;
    GroupId = 0;
    NameNew = string .Empty ;
    }
    }
    public partial class AddAreaOrGroupInfo
    {
    public AddAreaOrGroupInfo()
    {
    NameAdd = string .Empty ;
    AreaIdIfaddgroup = 0;
    }
    }
    public partial class AddAreaOrGroupInfoBk
    {
    public AddAreaOrGroupInfoBk()
    {
    Head = new Wlst.iif.Head();
    AreaId = 0;
    NameAdd = string .Empty ;
    GroupIdIfaddgroup = 0;
    }
    }
    public partial class DeleteAreaOrGroupInfo
    {
    public DeleteAreaOrGroupInfo()
    {
    AreaId = 0;
    GroupIdIfdeletegroup = 0;
    }
    }
    public partial class DeleteAreaOrGroupInfoBk
    {
    public DeleteAreaOrGroupInfoBk()
    {
    Head = new Wlst.iif.Head();
    AreaId = 0;
    GroupIdIfdeletegroup = 0;
    }
    }
    public partial class RtuOnlineInfoRq
    {
    public RtuOnlineInfoRq()
    {
    RtuModel = 0;
    RtuState = 0;
    RtuFault = 0;
    RtuOnline = 0;
    }
    }
    public partial class RtuOnlineInfo
    {
    public RtuOnlineInfo()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.RtuOnlineInfo.RtuOnlineInfoOne>();
    }
    }
    public partial class RtuMeasureInfo
    {
    public RtuMeasureInfo()
    {
    RtuId = 0;
    Items = new Wlst.iif.RtuMeasureInfo.RtuMeasureInfoOne();
    ItemsLoop = new List<Wlst.iif.RtuMeasureInfo.RtuLoopInfo>();
    ItemsSwitchOut = new List<Wlst.iif.RtuMeasureInfo.RtuSwitchOutInfo>();
    ShowColums = new List<int>();
    Right = 0;
    }
    }
    public partial class RtusMeasureInfo
    {
    public RtusMeasureInfo()
    {
    Head = new Wlst.iif.Head();
    LightOnCheck = 0;
    LightOffCheck = 0;
    GisFaultDisplay = 0;
    ItemsRtu = new List<Wlst.iif.RtuMeasureInfo>();
    ItemsNoAnsFromDateStart = new List<int>();
    }
    }
    public partial class GetRtusMeasureInfo
    {
    public GetRtusMeasureInfo()
    {
    Op = 0;
    PageIdx = 0;
    ItemsRtu = new List<int>();
    DateStart = 0;
    }
    }
    public partial class GetRtusHisdata
    {
    public GetRtusHisdata()
    {
    Head = new Wlst.iif.Head();
    ItemsRtu = new List<int>();
    DateStart = 0;
    DateEnd = 0;
    }
    }
    public partial class RtuMeasureDayInfo
    {
    public RtuMeasureDayInfo()
    {
    RtuId = 0;
    DateCreate = 0;
    }
    }
    public partial class RtuMeasureDayInfoBk
    {
    public RtuMeasureDayInfoBk()
    {
    Head = new Wlst.iif.Head();
    RtuId = 0;
    RtuPhyId = 0;
    Items = new List<Wlst.iif.RtuMeasureDayInfoBk.RtuInfo>();
    }
    }
    public partial class RtuOrderZc
    {
    public RtuOrderZc()
    {
    RtuId = new List<int>();
    }
    }
    public partial class AnsRtuOrderZc
    {
    public AnsRtuOrderZc()
    {
    RtuId = 0;
    TimeOrVersion = string .Empty ;
    }
    }
    public partial class AnsRtuOrderSetWeekSet
    {
    public AnsRtuOrderSetWeekSet()
    {
    RtuId = 0;
    WeeksetId = 0;
    OrderSum = 0;
    }
    }
    public partial class AnsRtuOrderRtuPara
    {
    public AnsRtuOrderRtuPara()
    {
    HeartBeatPeriod = 0;
    AlarmDelays = 0;
    ErrorDelays = 0;
    ReportDataPeriod = 0;
    VoRange = 0;
    VoUpper = 0;
    VoLower = 0;
    SwitchOutCount = 0;
    SwitchInCount = 0;
    SinCount = 0;
    SwitchOutInfo = new List<Wlst.iif.AnsRtuOrderRtuPara.ZcRtuSwitchOutInfo>();
    LoopInfo = new List<Wlst.iif.AnsRtuOrderRtuPara.ZcRtuLoopInfo>();
    }
    }
    public partial class AnsZcRtuWeekTimeSet
    {
    public AnsZcRtuWeekTimeSet()
    {
    RtuId = 0;
    Info = new List<Wlst.iif.AnsZcRtuWeekTimeSet.ZcOneLoopOneWeekTime>();
    }
    }
    public partial class RtuOpenCloseLight
    {
    public RtuOpenCloseLight()
    {
    RtuId = 0;
    OP = 0;
    SwitchoutLoops = new List<int>();
    }
    }
    public partial class RtuOpenCloseLightCenter
    {
    public RtuOpenCloseLightCenter()
    {
    OP = 0;
    Items = new List<Wlst.iif.RtuOpenCloseLightCenter.RtuOpenCloseLightCenterItem>();
    }
    }
    public partial class AnsRtuOpenCloseLight
    {
    public AnsRtuOpenCloseLight()
    {
    RtuId = 0;
    OP = 0;
    SwitchoutId = 0;
    }
    }
    public partial class GetRtusHisweekset
    {
    public GetRtusHisweekset()
    {
    Head = new Wlst.iif.Head();
    ItemsRtu = new List<int>();
    DateStart = 0;
    DateEnd = 0;
    }
    }
    public partial class GetRtusHisweeksetbk
    {
    public GetRtusHisweeksetbk()
    {
    Head = new Wlst.iif.Head();
    Itemsdata = new List<Wlst.iif.GetRtusHisweeksetbk.GetRtusHisweeksetbkItem>();
    }
    }
    public partial class CurErrorDelete
    {
    public CurErrorDelete()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.CurErrorDelete.CurErrorItem>();
    Op = 0;
    }
    }
    public partial class SetAlarmErrInfo
    {
    public SetAlarmErrInfo()
    {
    Head = new Wlst.iif.Head();
    ErrInfoView = new List<Wlst.iif.SetAlarmErrInfo.AlarmErrInfoView>();
    }
    }
    public partial class GtCurFault
    {
    public GtCurFault()
    {
    DateStart = 0;
    DateEnd = 0;
    RtuIds = new List<int>();
    AreaId = new List<int>();
    FaultCodes = new List<int>();
    FaultLevel = 0;
    EquType = 0;
    }
    }
    public partial class CurFault
    {
    public CurFault()
    {
    Head = new Wlst.iif.Head();
    Errs = new List<Wlst.iif.CurFault.CurFaultItem>();
    }
    }
    public partial class GtPreFault
    {
    public GtPreFault()
    {
    DateStart = 0;
    DateEnd = 0;
    RtuIds = new List<int>();
    AreaId = new List<int>();
    FaultCodes = new List<int>();
    FaultLevel = 0;
    EquType = 0;
    Head = new Wlst.iif.Head();
    }
    }
    public partial class PreFault
    {
    public PreFault()
    {
    Head = new Wlst.iif.Head();
    Errs = new List<Wlst.iif.PreFault.PreFaultItem>();
    }
    }
    public partial class SystemOptionInfo
    {
    public SystemOptionInfo()
    {
    Head = new Wlst.iif.Head();
    SysName = string .Empty ;
    GisLon = 0;
    GisLat = 0;
    RtuOrder = 0;
    AreaType = 0;
    LuxId = 0;
    PageNum = 0;
    LightOnCheck = 0;
    LightOffCheck = 0;
    RtuMeasureDelay = 0;
    RtuRunningDisplay = new List<int>();
    RtuTurnTime = 0;
    RtuInterval = 0;
    SluTurnTime = 0;
    SluInterval = 0;
    LduTurnTime = 0;
    LduInterval = 0;
    LeakTurnTime = 0;
    LeakInterval = 0;
    EsuTurnTime = 0;
    EsuInterval = 0;
    MruTurnTime = 0;
    MruInterval = 0;
    LuxTurnTime = 0;
    LuxInterval = 0;
    FaultTotalDay = 0;
    VLoseValue = 0;
    BrightRateLower = 0;
    GisFaultDisplay = 0;
    LuxReportTime = 0;
    }
    }
    public partial class TimetablePlanBandingInfo
    {
    public TimetablePlanBandingInfo()
    {
    Head = new Wlst.iif.Head();
    ItemsBanding = new List<Wlst.iif.TimetablePlanBandingInfo.TimePlanBandingInfo>();
    AreaId = 0;
    ItemsPlan = new List<Wlst.iif.TimetablePlanBandingInfo.TimePlanInfoBrief>();
    }
    }
    public partial class TimetablePlanBandingSpecialPlanInfoSave
    {
    public TimetablePlanBandingSpecialPlanInfoSave()
    {
    ItemsBanding = new List<Wlst.iif.TimetablePlanBandingSpecialPlanInfoSave.PlanInfoBandingSpecialPlan>();
    }
    }
    public partial class TimetablePlanInfoBrief
    {
    public TimetablePlanInfoBrief()
    {
    Head = new Wlst.iif.Head();
    AreaIds = new List<int>();
    ItemsTimePlan = new List<Wlst.iif.TimetablePlanInfoBrief.TimePlanInfoBrief>();
    ItemsSpecialPlan = new List<Wlst.iif.TimetablePlanInfoBrief.SpecialTimePlanInfoBrief>();
    }
    }
    public partial class RequestRtuPlanInfo
    {
    public RequestRtuPlanInfo()
    {
    AreaId = 0;
    TimePlanId = 0;
    }
    }
    public partial class OneRtuPlanInfo
    {
    public OneRtuPlanInfo()
    {
    AreaId = 0;
    TimePlanId = 0;
    TimePlanName = string .Empty ;
    TimePlanDesc = string .Empty ;
    LuxOnValue = 0;
    LuxOffValue = 0;
    LuxId = 0;
    LuxBackupId = 0;
    LightOnOffset = 0;
    LightOffOffset = 0;
    LuxEffective = 0;
    ItemsSet = new List<Wlst.iif.OneRtuPlanInfo.TimeTableOnedayPlan>();
    ItemsSunRaiseTimeThisWeek = new List<int>();
    ItemsSunSetTimeThisWeek = new List<int>();
    ItemsLux = new List<Wlst.iif.OneRtuPlanInfo.LuxParaInfoItem>();
    }
    }
    public partial class AddOrUpdateRtuPlanInfo
    {
    public AddOrUpdateRtuPlanInfo()
    {
    AreaId = 0;
    TimePlanId = 0;
    TimePlanName = string .Empty ;
    TimePlanDesc = string .Empty ;
    LuxOnValue = 0;
    LuxOffValue = 0;
    LuxId = 0;
    LuxBackupId = 0;
    LightOnOffset = 0;
    LightOffOffset = 0;
    LuxEffective = 0;
    ItemsSet = new List<Wlst.iif.AddOrUpdateRtuPlanInfo.TimeTableOnedayPlan>();
    }
    }
    public partial class OneSpecialRtuTimePlanInfo
    {
    public OneSpecialRtuTimePlanInfo()
    {
    AreaId = 0;
    SpecialPlanId = 0;
    SpecialPlanName = string .Empty ;
    SpecialPlanDesc = string .Empty ;
    LuxOnValue = 0;
    LuxOffValue = 0;
    LuxId = 0;
    LuxBackupId = 0;
    LightOnOffset = 0;
    LightOffOffset = 0;
    LuxEffective = 0;
    DateStart = 0;
    DateEnd = 0;
    ItemsSpecial = new List<Wlst.iif.OneSpecialRtuTimePlanInfo.SpecialTimePlanInfo>();
    ItemsBanding = new List<int>();
    ItemsSunRiseSet = new List<Wlst.iif.OneSpecialRtuTimePlanInfo.SunSetRiseInfoItem>();
    ItemsLux = new List<Wlst.iif.OneRtuPlanInfo.LuxParaInfoItem>();
    }
    }
    public partial class AddOrUpdateSpecialRtuTimePlanInfo
    {
    public AddOrUpdateSpecialRtuTimePlanInfo()
    {
    AreaId = 0;
    SpecialPlanId = 0;
    SpecialPlanName = string .Empty ;
    SpecialPlanDesc = string .Empty ;
    LuxOnValue = 0;
    LuxOffValue = 0;
    LuxId = 0;
    LuxBackupId = 0;
    LightOnOffset = 0;
    LightOffOffset = 0;
    LuxEffective = 0;
    DateStart = 0;
    DateEnd = 0;
    ItemsSpecial = new List<Wlst.iif.AddOrUpdateSpecialRtuTimePlanInfo.SpecialTimePlanInfo>();
    }
    }
    public partial class DeleteTimetablePlanInfo
    {
    public DeleteTimetablePlanInfo()
    {
    AreaId = 0;
    PlanId = 0;
    }
    }
    public partial class GetRecordTypes
    {
    public GetRecordTypes()
    {
    RecordsTypeIn = 0;
    }
    }
    public partial class GetRecordTypesBk
    {
    public GetRecordTypesBk()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.GetRecordTypesBk.RecordTypesItem>();
    }
    }
    public partial class GtOpRecord
    {
    public GtOpRecord()
    {
    Head = new Wlst.iif.Head();
    DateStart = 0;
    DateEnd = 0;
    RtuIds = new List<int>();
    AreaId = new List<int>();
    RecordTypes = new List<int>();
    }
    }
    public partial class OpRecord
    {
    public OpRecord()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.OpRecord.OpRecordItem>();
    }
    }
    public partial class SysInfo
    {
    public SysInfo()
    {
    Head = new Wlst.iif.Head();
    SysInfoField = new List<Wlst.iif.SysInfo.SysInfoView>();
    AppOursInfo = new List<Wlst.iif.SysInfo.AppInfoView>();
    AppAllInfo = new List<Wlst.iif.SysInfo.AppInfoView>();
    }
    }
    public partial class GtControlSuccessRate
    {
    public GtControlSuccessRate()
    {
    Head = new Wlst.iif.Head();
    DateStart = 0;
    DateEnd = 0;
    RtuIds = new List<int>();
    OperateType = 0;
    }
    }
    public partial class ControlSuccessRate
    {
    public ControlSuccessRate()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.ControlSuccessRate.ControlSuccessRateItem>();
    }
    }
    public partial class GtRtuElecDay
    {
    public GtRtuElecDay()
    {
    Head = new Wlst.iif.Head();
    DateStart = 0;
    DateEnd = 0;
    RtuIds = new List<int>();
    }
    }
    public partial class RtuElecDay
    {
    public RtuElecDay()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.RtuElecDay.RtuElecTotal>();
    }
    }
    public partial class GtRtuElecRtu
    {
    public GtRtuElecRtu()
    {
    Head = new Wlst.iif.Head();
    DateStart = 0;
    DateEnd = 0;
    RtuIds = new List<int>();
    }
    }
    public partial class RtuElecRtu
    {
    public RtuElecRtu()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.RtuElecRtu.RtuElecItem>();
    }
    }
    public partial class GtFaultStatisticsDay
    {
    public GtFaultStatisticsDay()
    {
    Head = new Wlst.iif.Head();
    DateStart = 0;
    DateEnd = 0;
    RtuIds = new List<int>();
    }
    }
    public partial class FaultStatisticsDay
    {
    public FaultStatisticsDay()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.FaultStatisticsDay.FaultStatistics>();
    }
    }
    public partial class GtFaultStatisticsRtu
    {
    public GtFaultStatisticsRtu()
    {
    Head = new Wlst.iif.Head();
    DateStart = 0;
    DateEnd = 0;
    RtuIds = new List<int>();
    }
    }
    public partial class FaultStatisticsRtu
    {
    public FaultStatisticsRtu()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.FaultStatisticsRtu.FaultStatisticsItem>();
    }
    }
    public partial class GtRtuDataStatistics
    {
    public GtRtuDataStatistics()
    {
    Head = new Wlst.iif.Head();
    DateStart = 0;
    DateEnd = 0;
    RtuIds = new List<int>();
    PeakType = 0;
    Diff = 0;
    }
    }
    public partial class RtuDataStatistics
    {
    public RtuDataStatistics()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.RtuDataStatistics.RtuDataStatisticsItem>();
    }
    }
    public partial class GtSluLightOnRate
    {
    public GtSluLightOnRate()
    {
    Head = new Wlst.iif.Head();
    DateStart = 0;
    DateEnd = 0;
    RtuIds = new List<int>();
    }
    }
    public partial class SluLightOnRate
    {
    public SluLightOnRate()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.SluLightOnRate.SluLightOnRateItem>();
    }
    }
    public partial class EquipmentInfoType
    {
    public EquipmentInfoType()
    {
    Head = new Wlst.iif.Head();
    EquIds = new List<int>();
    ItemType = new List<Wlst.iif.EquipmentInfoTypeItem>();
    }
    }
    public partial class EquipmentInfoTypeItem
    {
    public EquipmentInfoTypeItem()
    {
    EquId = 0;
    TypeId = 0;
    TypeName = string .Empty ;
    }
    }
    public partial class EquipmentInfoAdd
    {
    public EquipmentInfoAdd()
    {
    ThisEquimentAttachRtuId = 0;
    PhyId = 0;
    EquipmentMode = 0;
    AreaId = 0;
    GroupId = 0;
    EquName = string .Empty ;
    }
    }
    public partial class EquipmentInfoUpdate
    {
    public EquipmentInfoUpdate()
    {
    RtuId = 0;
    RtuPhyId = 0;
    EquipmentMode = 0;
    GroupId = 0;
    EquName = string .Empty ;
    }
    }
    public partial class EquipmentInfoBk
    {
    public EquipmentInfoBk()
    {
    Head = new Wlst.iif.Head();
    RtuId = 0;
    }
    }
    public partial class EquipmentInfoDelete
    {
    public EquipmentInfoDelete()
    {
    RtuId = 0;
    }
    }
    public partial class EquipmentInfoDeleteBk
    {
    public EquipmentInfoDeleteBk()
    {
    Head = new Wlst.iif.Head();
    RtuIds = new List<int>();
    }
    }
    public partial class RtuEquipmentBase
    {
    public RtuEquipmentBase()
    {
    RtuId = 0;
    RtuPhyId = 0;
    RtuName = string .Empty ;
    RtuStateCode = 0;
    RtuRealState = 0;
    RtuModel = 0;
    RtuInstallAddr = string .Empty ;
    RtuArgu = string .Empty ;
    RtuRemark = string .Empty ;
    DateCreate = 0;
    DateUpdate = 0;
    Idf = string .Empty ;
    VoltageAlarmUpperlimit = 0;
    VoltageAlarmLowerlimit = 0;
    IsSwitchinputJudgebyA = false;
    RtuUsedType = 0;
    Aoverload = 0;
    AoverAlittle = 0;
    StaticIp = string .Empty ;
    MobileNo = string .Empty ;
    ModuleSn = string .Empty ;
    IsHasElec = 0;
    PhaseRadio = new List<int>();
    }
    }
    public partial class RtuPara
    {
    public RtuPara()
    {
    Head = new Wlst.iif.Head();
    Baseinfo = new Wlst.iif.RtuEquipmentBase();
    LoopsInfo = new List<Wlst.iif.RtuPara.RtuAnalogParameter>();
    InputsInfo = new List<Wlst.iif.RtuPara.RtuSwitchInputParameter>();
    OutputsInfo = new List<Wlst.iif.RtuPara.RtuSwitchOutputParameter>();
    RtuId = 0;
    Right = 0;
    }
    }
    public partial class RqRtuPara
    {
    public RqRtuPara()
    {
    RtuId = 0;
    }
    }
    public partial class GtSludataCon
    {
    public GtSludataCon()
    {
    Head = new Wlst.iif.Head();
    DateStart = 0;
    DateEnd = 0;
    SluIds = new List<int>();
    }
    }
    public partial class ReplygtSludataCon
    {
    public ReplygtSludataCon()
    {
    Head = new Wlst.iif.Head();
    ItemsData = new List<Wlst.iif.ReplygtSludataCon.DataSluCon>();
    }
    }
    public partial class SluOrderZc
    {
    public SluOrderZc()
    {
    SluIds = new List<int>();
    }
    }
    public partial class GetSluOrderZcdata
    {
    public GetSluOrderZcdata()
    {
    SluIds = new List<int>();
    DateCtreatStart = 0;
    IsNeedPlaninfo = 0;
    }
    }
    public partial class GetSluOrderZcCtrldata
    {
    public GetSluOrderZcCtrldata()
    {
    SluId = 0;
    SluCtrlIds = new List<int>();
    DateCtreatStart = 0;
    IsNeedPlaninfo = 0;
    }
    }
    public partial class GetCtrldata
    {
    public GetCtrldata()
    {
    SluPhyId = 0;
    CtrlBarCode = 0;
    }
    }
    public partial class ReplygtSludataMeasure
    {
    public ReplygtSludataMeasure()
    {
    Head = new Wlst.iif.Head();
    LightOnCheck = 0;
    LightOffCheck = 0;
    GisFaultDisplay = 0;
    SluData = new List<Wlst.iif.ReplygtSludataCon.DataSluCon>();
    }
    }
    public partial class SluCtrlDataMeasure
    {
    public SluCtrlDataMeasure()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    SluPhyId = 0;
    RtuName = string .Empty ;
    CtrlId = 0;
    CtrlName = string .Empty ;
    BarCodeId = 0;
    Info = new Wlst.iif.SluCtrlDataMeasure.DataSluCtrl();
    Items = new List<Wlst.iif.SluCtrlDataMeasure.DataSluCtrlLamp>();
    ItemsPlan = new List<Wlst.iif.ReplygtSludataCon.DataSluConPlanInfo>();
    }
    }
    public partial class GtSludataLamp
    {
    public GtSludataLamp()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    SluCtrlIds = new List<int>();
    LampId = 0;
    DateStart = 0;
    DateEnd = 0;
    }
    }
    public partial class ReplygtSludataLamp
    {
    public ReplygtSludataLamp()
    {
    Head = new Wlst.iif.Head();
    Data = new List<Wlst.iif.SluCtrlDataMeasure>();
    }
    }
    public partial class ReplygtSludataLampMeasure
    {
    public ReplygtSludataLampMeasure()
    {
    Head = new Wlst.iif.Head();
    Data = new List<Wlst.iif.SluCtrlDataMeasure>();
    }
    }
    public partial class CtrlPhyinfo
    {
    public CtrlPhyinfo()
    {
    SignalStrength = 0;
    Routing = 0;
    Phase = 0;
    UsefulCommunicate = 0;
    AllCommunicate = 0;
    CtrlLoop = 0;
    PowerSaving = 0;
    HasLeakage = false;
    HasTemperature = false;
    HasTimer = false;
    Model = 0;
    DtCreate = 0;
    DateCreateStr = string .Empty ;
    SluId = 0;
    SluPhyId = 0;
    RtuName = string .Empty ;
    CtrlId = 0;
    CtrlName = string .Empty ;
    }
    }
    public partial class GtSludataCtrpPhyInfo
    {
    public GtSludataCtrpPhyInfo()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    SluCtrlIds = new List<int>();
    DateStart = 0;
    DateEnd = 0;
    }
    }
    public partial class ReplygtSludataCtrpPhyInfo
    {
    public ReplygtSludataCtrpPhyInfo()
    {
    Head = new Wlst.iif.Head();
    Data = new List<Wlst.iif.CtrlPhyinfo>();
    }
    }
    public partial class ReplygtSluPhydataLampMeasure
    {
    public ReplygtSluPhydataLampMeasure()
    {
    Head = new Wlst.iif.Head();
    Data = new List<Wlst.iif.CtrlPhyinfo>();
    }
    }
    public partial class AssistCtrlData
    {
    public AssistCtrlData()
    {
    SluId = 0;
    SluPhyId = 0;
    RtuName = string .Empty ;
    CtrlId = 0;
    CtrlName = string .Empty ;
    BarCodeId = 0;
    LampCode = string .Empty ;
    DateTime = 0;
    DateTimeStr = string .Empty ;
    LeakageCurrent = 0;
    LightDataField = new List<Wlst.iif.AssistCtrlData.LightData>();
    }
    }
    public partial class ReplygtSluAssistCtrlDataMeasure
    {
    public ReplygtSluAssistCtrlDataMeasure()
    {
    Head = new Wlst.iif.Head();
    Data = new List<Wlst.iif.AssistCtrlData>();
    }
    }
    public partial class SluCtrlParaRead
    {
    public SluCtrlParaRead()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    SluPhyId = 0;
    RtuName = string .Empty ;
    Items = new List<Wlst.iif.SluCtrlParaRead.SluCtrlParaReadItem>();
    }
    }
    public partial class SluRightOperators
    {
    public SluRightOperators()
    {
    OperatorItems = new List<Wlst.iif.SluRightOperators.SluRightOperator>();
    }
    }
    public partial class SluRightOperatorReply
    {
    public SluRightOperatorReply()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    SluPhyId = 0;
    RtuName = string .Empty ;
    Items = new List<Wlst.iif.SluRightOperatorReply.SluRightOperatorReplyItem>();
    }
    }
    public partial class SluTimeRead
    {
    public SluTimeRead()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    SluPhyId = 0;
    RtuName = string .Empty ;
    Iems = new List<Wlst.iif.SluTimeRead.SluTimeReadItem>();
    }
    }
    public partial class ResetAndInit
    {
    public ResetAndInit()
    {
    SluId = 0;
    ReConcentrator = false;
    HardReZigbee = false;
    SoftReZigbee = false;
    ReCarrier = false;
    InitAll = false;
    ClearData = false;
    ClearArgs = false;
    ClearTask = false;
    }
    }
    public partial class ResetAndInitReply
    {
    public ResetAndInitReply()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    IsSuccessfull = false;
    ZcSetResetAndInit = new Wlst.iif.ResetAndInit();
    }
    }
    public partial class SluSetAndRead
    {
    public SluSetAndRead()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    Op = 0;
    }
    }
    public partial class SluSetAndReadReplyOne
    {
    public SluSetAndReadReplyOne()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    Op = 0;
    IsSuccessfull = false;
    Info = string .Empty ;
    }
    }
    public partial class SluSetAndReadReplyTwo
    {
    public SluSetAndReadReplyTwo()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    Op = 0;
    IsSuccessfull = false;
    ZcJzqTime = new Wlst.iif.SluSetAndReadReplyTwo.JzqTimeInfo();
    Info = string .Empty ;
    }
    }
    public partial class SluSetAndReadReplyThree
    {
    public SluSetAndReadReplyThree()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    IsSuccessfull = false;
    ZcJzqPara = new Wlst.iif.SluSetAndReadReplyThree.JzqPara();
    Info = string .Empty ;
    }
    }
    public partial class SluSetAndReadReplyFour
    {
    public SluSetAndReadReplyFour()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    IsSuccessfull = false;
    ZcJzqAlarmPara = new Wlst.iif.SluSetAndReadReplyFour.JzqAlarmPara();
    Info = string .Empty ;
    }
    }
    public partial class SluSetAndReadReplyFive
    {
    public SluSetAndReadReplyFive()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    IsSuccessfull = false;
    ZcCtrlDomainChangeInfo = new List<bool>();
    Info = string .Empty ;
    }
    }
    public partial class SluSetAndReadReplySix
    {
    public SluSetAndReadReplySix()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    IsSuccessfull = false;
    ZcSoftVersion = string .Empty ;
    Info = string .Empty ;
    }
    }
    public partial class SluCtrlArgsRead
    {
    public SluCtrlArgsRead()
    {
    SluId = 0;
    CtrlId = 0;
    Op = 0;
    }
    }
    public partial class SluCtrlArgsReplyOne
    {
    public SluCtrlArgsReplyOne()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    SluPhyId = 0;
    RtuName = string .Empty ;
    CtrlId = 0;
    CtrlName = string .Empty ;
    CtrPhyId = 0;
    CtrlDataField = new Wlst.iif.SluCtrlArgsReplydata.CtrlData();
    CtrlDataNewField = new List<Wlst.iif.SluCtrlArgsReplydata.CtrlDataNew>();
    }
    }
    public partial class SluCtrlArgsReplyTwo
    {
    public SluCtrlArgsReplyTwo()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    SluPhyId = 0;
    RtuName = string .Empty ;
    CtrlId = 0;
    CtrlName = string .Empty ;
    CtrPhyId = 0;
    CtrlTime = 0;
    CtrlGroup = new List<int>();
    CtrlVerField = new Wlst.iif.SluCtrlArgsReplydata.CtrlVer();
    }
    }
    public partial class SluCtrlArgsReplyThree
    {
    public SluCtrlArgsReplyThree()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    SluPhyId = 0;
    RtuName = string .Empty ;
    CtrlId = 0;
    CtrlName = string .Empty ;
    CtrPhyId = 0;
    CtrlParaField = new Wlst.iif.SluCtrlArgsReplydata.CtrlPara();
    CtrlSunrisesetField = new Wlst.iif.SluCtrlArgsReplydata.CtrlSunriseset();
    }
    }
    public partial class SluCtrlArgsReplyFour
    {
    public SluCtrlArgsReplyFour()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    SluPhyId = 0;
    RtuName = string .Empty ;
    CtrlId = 0;
    CtrlName = string .Empty ;
    CtrPhyId = 0;
    CtrlRuntimeField = new List<Wlst.iif.SluCtrlArgsReplydata.CtrlRuntime>();
    }
    }
    public partial class SluCtrlArgsReplydata
    {
    public SluCtrlArgsReplydata()
    {
    }
    }
    public partial class LuxDataRq
    {
    public LuxDataRq()
    {
    LuxId = 0;
    DtStartTime = 0;
    DtEndTime = 0;
    }
    }
    public partial class LuxDataBk
    {
    public LuxDataBk()
    {
    Head = new Wlst.iif.Head();
    LuxId = 0;
    DtStartTime = 0;
    DtEndTime = 0;
    Info = new List<Wlst.iif.LuxDataBk.LuxDataItem>();
    LuxShowPhyId = 0;
    LuxName = string .Empty ;
    }
    }
    public partial class TimeTableReportRecordBk
    {
    public TimeTableReportRecordBk()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.TimeTableReportRecordBk.TimeTableReportRecordItem>();
    }
    }
    public partial class TimeTableReportRecordRq
    {
    public TimeTableReportRecordRq()
    {
    AreaId = 0;
    TimeTableId = 0;
    IsOpenLight = 0;
    DtStartTime = 0;
    DtEndTime = 0;
    }
    }
    public partial class TimeTableReportRecordDetailRq
    {
    public TimeTableReportRecordDetailRq()
    {
    AreaId = 0;
    TimeTableId = 0;
    IsOpenLight = 0;
    DateCreateCs = 0;
    }
    }
    public partial class TimeTableReportRecordDetailBk
    {
    public TimeTableReportRecordDetailBk()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.TimeTableReportRecordDetailBk.RtuOpInfo>();
    }
    }
    public partial class RecordWeekTimeBk
    {
    public RecordWeekTimeBk()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.RecordWeekTimeBk.RtuOpInfo>();
    }
    }
    public partial class RecordWeekTimeRq
    {
    public RecordWeekTimeRq()
    {
    DtStartTime = 0;
    DtEndTime = 0;
    RtuId = new List<int>();
    }
    }
    public partial class SunRiseSetInfo
    {
    public SunRiseSetInfo()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.SunRiseSetInfo.SunRiseItem>();
    }
    }
    public partial class MsgRecordRq
    {
    public MsgRecordRq()
    {
    DtStartTime = 0;
    DtEndTime = 0;
    UserPhoneNumber = 0;
    }
    }
    public partial class MsgRecordBk
    {
    public MsgRecordBk()
    {
    Items = new List<Wlst.iif.MsgRecordBk.MsgRecordItem>();
    }
    }
    public partial class InfoRq
    {
    public InfoRq()
    {
    AreaId = 0;
    OpArgu = 0;
    }
    }
    public partial class SluPlanGrpInfoBk
    {
    public SluPlanGrpInfoBk()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.SluPlanGrpInfoBk.SluPlanGrpItemInfo>();
    }
    }
    public partial class SluPlanGrpSave
    {
    public SluPlanGrpSave()
    {
    AreaId = 0;
    GrpId = 0;
    GrpName = string .Empty ;
    Items = new List<Wlst.iif.SluPlanGrpSave.SluGrpSaveSelectedItem>();
    }
    }
    public partial class SluPlanBriefInfo
    {
    public SluPlanBriefInfo()
    {
    Head = new Wlst.iif.Head();
    AreaId = 0;
    Items = new List<Wlst.iif.SluPlanBriefInfo.SluPlanItemBriefInfo>();
    }
    }
    public partial class SluPlanDetailInfoRq
    {
    public SluPlanDetailInfoRq()
    {
    AreaId = 0;
    PlanId = 0;
    }
    }
    public partial class SluPlanDetailInfo
    {
    public SluPlanDetailInfo()
    {
    Head = new Wlst.iif.Head();
    AreaId = 0;
    TimePlanId = 0;
    TimePlanName = string .Empty ;
    DateCreate = 0;
    TimePlanDesc = string .Empty ;
    TimePlanIsuesd = false;
    UsedWeekSet = new List<int>();
    LoopCanDo = new List<int>();
    CmdType = 0;
    Scale = 0;
    OpeMethod = 0;
    OpeArgu = 0;
    TimeOfLightStartEffect = 0;
    TimeOfLightEndEffect = 0;
    LuxStartEffect = 0;
    LuxEndEffect = 0;
    LightUsedRtuId = 0;
    Luxitems = new List<Wlst.iif.SluPlanDetailInfo.LuxInfo>();
    }
    }
    public partial class SluPlanBandingInfo
    {
    public SluPlanBandingInfo()
    {
    Head = new Wlst.iif.Head();
    AreaId = 0;
    Items = new List<Wlst.iif.SluPlanBandingInfo.SluPlanBandingBriefInfo>();
    }
    }
    public partial class SluPlanBandingDetailInfo
    {
    public SluPlanBandingDetailInfo()
    {
    Head = new Wlst.iif.Head();
    AreaId = 0;
    PlanId = 0;
    PlanName = string .Empty ;
    DateCreate = 0;
    PlanDesc = string .Empty ;
    IsUesd = false;
    Items = new List<Wlst.iif.SluPlanBandingDetailInfo.TimePlanBandingGrpItem>();
    }
    }
    public partial class SluTimePlanRecord
    {
    public SluTimePlanRecord()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.SluTimePlanRecord.SluSluTimePlanSendInfo>();
    }
    }
    public partial class GtSluTimePlanRecord
    {
    public GtSluTimePlanRecord()
    {
    Head = new Wlst.iif.Head();
    DateStart = 0;
    DateEnd = 0;
    SluIds = new List<int>();
    }
    }
    public partial class SluCtrlParaRq
    {
    public SluCtrlParaRq()
    {
    SluId = 0;
    }
    }
    public partial class SluPara
    {
    public SluPara()
    {
    Head = new Wlst.iif.Head();
    SluId = 0;
    Para = new Wlst.iif.SluPara.SluParameter();
    ItemsCtrls = new List<Wlst.iif.SluPara.SluCtrlPara>();
    ItemsGrp = new List<Wlst.iif.SluPara.SluCtrlGroupPara>();
    Right = 0;
    }
    }
    public partial class UserShowFontRq
    {
    public UserShowFontRq()
    {
    Head = new Wlst.iif.Head();
    UserId = string .Empty ;
    }
    }
    public partial class UserShowFontInfo
    {
    public UserShowFontInfo()
    {
    Head = new Wlst.iif.Head();
    UserSfo = new Wlst.iif.UserShowFontInfo.UserShowFont();
    }
    }
    public partial class DeleteUserShowFontInfo
    {
    public DeleteUserShowFontInfo()
    {
    Head = new Wlst.iif.Head();
    UserId = string .Empty ;
    }
    }
    public partial class GetEquRelated
    {
    public GetEquRelated()
    {
    RtuId = 0;
    }
    }
    public partial class RelatedRtus
    {
    public RelatedRtus()
    {
    Head = new Wlst.iif.Head();
    RtuItems = new List<Wlst.iif.RtuInfo>();
    }
    }
    public partial class GetRtusState
    {
    public GetRtusState()
    {
    Head = new Wlst.iif.Head();
    DateStart = 0;
    }
    }
    public partial class RtusState
    {
    public RtusState()
    {
    Head = new Wlst.iif.Head();
    RtuItems = new List<Wlst.iif.RtusState.RtuStateInfo>();
    }
    }
    public partial class SluStatusRead
    {
    public SluStatusRead()
    {
    SluId = new List<int>();
    Time = 0;
    }
    }
    public partial class SluStatusReturn
    {
    public SluStatusReturn()
    {
    Head = new Wlst.iif.Head();
    SluStatusField = new List<Wlst.iif.SluStatusReturn.SluStatus>();
    }
    }
    public partial class GetRtuLoopHistory
    {
    public GetRtuLoopHistory()
    {
    Head = new Wlst.iif.Head();
    RtuId = 0;
    LoopId = 0;
    DateStart = 0;
    DateEnd = 0;
    }
    }
    public partial class RtuLoopHistory
    {
    public RtuLoopHistory()
    {
    Head = new Wlst.iif.Head();
    RtuId = 0;
    RtuPhyId = 0;
    RtuName = string .Empty ;
    RtuLoopId = 0;
    RtuLoopName = string .Empty ;
    LoopItems = new List<Wlst.iif.RtuLoopHistory.RtuLoopData>();
    }
    }
    public partial class SunRiseSetTime
    {
    public SunRiseSetTime()
    {
    Head = new Wlst.iif.Head();
    SunRise = 0;
    SunSet = 0;
    SunRiseStr = string .Empty ;
    SunSetStr = string .Empty ;
    }
    }
    public partial class TopCurFaultInfo
    {
    public TopCurFaultInfo()
    {
    Head = new Wlst.iif.Head();
    CurErrs = new List<Wlst.iif.TopCurFaultInfo.TopCurFaultItem>();
    }
    }
    public partial class OpenCloseTimeInfo
    {
    public OpenCloseTimeInfo()
    {
    Head = new Wlst.iif.Head();
    SunRise = 0;
    SunSet = 0;
    SunRiseStr = string .Empty ;
    SunSetStr = string .Empty ;
    OpenTime = 0;
    CloseTime = 0;
    OpenTimeStr = string .Empty ;
    CloseTimeStr = string .Empty ;
    }
    }
    public partial class BrightRateTrendInfo
    {
    public BrightRateTrendInfo()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.BrightRateTrendInfo.BrightRateTrendItem>();
    }
    }
    public partial class EquOnlineInfo
    {
    public EquOnlineInfo()
    {
    Head = new Wlst.iif.Head();
    OnlineItems = new List<Wlst.iif.EquOnlineInfo.EquOnlineItem>();
    }
    }
    public partial class FaultResponseInfo
    {
    public FaultResponseInfo()
    {
    FaultResponseItems = new List<Wlst.iif.FaultResponseInfo.FaultResponseItem>();
    }
    }
    public partial class NewFaultInfo
    {
    public NewFaultInfo()
    {
    NewFaultItems = new List<Wlst.iif.NewFaultInfo.NewFaultItem>();
    }
    }
    public partial class EquActiveInfo
    {
    public EquActiveInfo()
    {
    EquActiveItems = new List<Wlst.iif.EquActiveInfo.EquActiveItem>();
    }
    }
    public partial class HomePageData
    {
    public HomePageData()
    {
    Head = new Wlst.iif.Head();
    OpenCloseTime = new Wlst.iif.OpenCloseTimeInfo();
    TopCurFault = new Wlst.iif.TopCurFaultInfo();
    FaultReponse = new Wlst.iif.FaultResponseInfo();
    NewFault = new Wlst.iif.NewFaultInfo();
    EquActive = new Wlst.iif.EquActiveInfo();
    }
    }
    public partial class SysUrlInfo
    {
    public SysUrlInfo()
    {
    Head = new Wlst.iif.Head();
    Urls = new List<string>();
    }
    }
    public partial class GisGetTableInfo
    {
    public GisGetTableInfo()
    {
    Head = new Wlst.iif.Head();
    TableNames = new List<string>();
    TableInfos = new List<Wlst.iif.GisGetTableInfo.GisTableInfo>();
    }
    }
    public partial class GisDeleteElement
    {
    public GisDeleteElement()
    {
    Head = new Wlst.iif.Head();
    TableName = string .Empty ;
    ColumNames = new List<string>();
    ColumValues = new List<string>();
    }
    }
    public partial class GisAddElement
    {
    public GisAddElement()
    {
    Head = new Wlst.iif.Head();
    TableName = string .Empty ;
    ColumNames = new List<string>();
    ColumValues = new List<string>();
    GisX = new List<double>();
    GisY = new List<double>();
    }
    }
    public partial class GisUpdateElement
    {
    public GisUpdateElement()
    {
    Head = new Wlst.iif.Head();
    TableName = string .Empty ;
    ColumNames = new List<string>();
    ColumValues = new List<string>();
    GisX = new List<double>();
    GisY = new List<double>();
    }
    }
    public partial class LeakPara
    {
    public LeakPara()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.LeakPara.LeakParaItem>();
    LeakId = 0;
    LeakName = string .Empty ;
    LeakPhyId = 0;
    LeakFid = 0;
    LeakFphyId = 0;
    LeakFname = string .Empty ;
    }
    }
    public partial class RequestLeakData
    {
    public RequestLeakData()
    {
    Head = new Wlst.iif.Head();
    DtStartTime = 0;
    DtEndTime = 0;
    RtuId = new List<int>();
    LeakA = 0;
    IsLeakAlarm = 0;
    LeakUppper = 0;
    Items = new List<Wlst.iif.RequestLeakData.LeakNewData>();
    }
    }
    public partial class LeakOrders
    {
    public LeakOrders()
    {
    RtuId = new List<int>();
    Op = 0;
    LeakLineId = new List<int>();
    OrderBreaktype = 0;
    }
    }
    public partial class LeakZcWorkArgs
    {
    public LeakZcWorkArgs()
    {
    Head = new Wlst.iif.Head();
    Items = new List<Wlst.iif.LeakZcWorkArgs.LeakZcWorkArgsItem>();
    LeakId = 0;
    LeakName = string .Empty ;
    LeakPhyId = 0;
    LeakFid = 0;
    LeakFphyId = 0;
    LeakFname = string .Empty ;
    }
    }
    public partial class LuxPara
    {
    public LuxPara()
    {
    Head = new Wlst.iif.Head();
    LuxId = 0;
    LuxName = string .Empty ;
    LuxPhyId = 0;
    LuxFid = 0;
    LuxFphyId = 0;
    LuxFname = string .Empty ;
    LuxCommTypeCode = 0;
    LuxWorkMode = 0;
    LuxRange = 0;
    }
    }
    public partial class RequestLuxData
    {
    public RequestLuxData()
    {
    Head = new Wlst.iif.Head();
    DtStartTime = 0;
    DtEndTime = 0;
    LuxId = new List<int>();
    Items = new List<Wlst.iif.RequestLuxData.LuxDataItem>();
    }
    }
    public partial class LuxOrders
    {
    public LuxOrders()
    {
    LuxId = 0;
    LuxPhyId = 0;
    Op = 0;
    }
    }
    public partial class MruPara
    {
    public MruPara()
    {
    Head = new Wlst.iif.Head();
    MruId = 0;
    MruName = string .Empty ;
    MruPhyId = 0;
    MruFid = 0;
    MruFphyId = 0;
    MruFname = string .Empty ;
    MruBaudrate = 0;
    MruRatio = 0;
    MruType = 0;
    MruAddr1 = 0;
    MruAddr2 = 0;
    MruAddr3 = 0;
    MruAddr4 = 0;
    MruAddr5 = 0;
    MruAddr6 = 0;
    }
    }
    public partial class RequestMruData
    {
    public RequestMruData()
    {
    Head = new Wlst.iif.Head();
    DtStartTime = 0;
    DtEndTime = 0;
    MruId = new List<int>();
    Items = new List<Wlst.iif.RequestMruData.MruDataItem>();
    }
    }
    public partial class RequestMruStatisticsData
    {
    public RequestMruStatisticsData()
    {
    Head = new Wlst.iif.Head();
    DtStartTime = 0;
    DtEndTime = 0;
    MruId = new List<int>();
    StatisticsType = string .Empty ;
    Items = new List<Wlst.iif.RequestMruStatisticsData.MruStatisticsDataItem>();
    }
    }
    public partial class RequestMruFailData
    {
    public RequestMruFailData()
    {
    Head = new Wlst.iif.Head();
    DtStartTime = 0;
    DtEndTime = 0;
    Items = new List<Wlst.iif.RequestMruFailData.MruFailDataItem>();
    }
    }
    public partial class MruOrders
    {
    public MruOrders()
    {
    MruId = 0;
    Op = 0;
    DataMruType = 0;
    DataTimeType = 0;
    }
    }
    public partial class ReplyReadMruData
    {
    public ReplyReadMruData()
    {
    DateCreate = 0;
    StrDateCreate = string .Empty ;
    RtuId = 0;
    DataMruType = 0;
    DataTimeType = 0;
    MruRatio = 0;
    MruData = 0;
    MruTotal = 0;
    }
    }
    public partial class MruMeasure
    {
    public MruMeasure()
    {
    MruId = new List<int>();
    }
    }
    public partial class LduPara
    {
    public LduPara()
    {
    Head = new Wlst.iif.Head();
    LduId = 0;
    LduName = string .Empty ;
    LduPhyId = 0;
    LduFid = 0;
    LduFphyId = 0;
    LduFname = string .Empty ;
    ItemsPara = new List<Wlst.iif.LduPara.LduLinePars>();
    }
    }
    public partial class LduZcPara
    {
    public LduZcPara()
    {
    Head = new Wlst.iif.Head();
    LduId = 0;
    LduPhyId = 0;
    LduFid = 0;
    ItemsPara = new List<Wlst.iif.LduZcPara.LduLinePars>();
    }
    }
    public partial class RequestLduData
    {
    public RequestLduData()
    {
    Head = new Wlst.iif.Head();
    DtStartTime = 0;
    DtEndTime = 0;
    LduId = new List<int>();
    LineId = 0;
    Items = new List<Wlst.iif.RequestLduData.LduLineDataItem>();
    }
    }
    public partial class LduOrders
    {
    public LduOrders()
    {
    LduId = 0;
    LineIds = new List<int>();
    Op = 0;
    }
    }
    public partial class LduBrightLightSetOrZc
    {
    public LduBrightLightSetOrZc()
    {
    LduId = 0;
    ItemsBrightLight = new List<Wlst.iif.LduBrightLightSetOrZc.LduLineBrightLightData>();
    }
    }
    public partial class EquimentRemarkRead
    {
    public EquimentRemarkRead()
    {
    RtuId = new List<int>();
    }
    }
    public partial class EquimentRemarkReturn
    {
    public EquimentRemarkReturn()
    {
    Head = new Wlst.iif.Head();
    EquRemark = new List<Wlst.iif.EquimentRemarkReturn.SingleEquimentRemark>();
    }
    }
    public partial class EquimentRemarkSave
    {
    public EquimentRemarkSave()
    {
    EquRemark = new List<Wlst.iif.EquimentRemarkSave.SingleEquimentRemark>();
    }
    }
    public partial class SluSglCtrlParaRead
    {
    public SluSglCtrlParaRead()
    {
    Imei = new List<long>();
    }
    }
    public partial class SluSglCtrlParaReturn
    {
    public SluSglCtrlParaReturn()
    {
    Head = new Wlst.iif.Head();
    SluSglCtrlPara = new List<Wlst.iif.SluSglCtrlParaReturn.SingleSluSglCtrlPara>();
    SluSglField = new List<Wlst.iif.SluSglCtrlParaReturn.SluSglFieldList>();
    }
    }
    public partial class SluSglCtrlParaSave
    {
    public SluSglCtrlParaSave()
    {
    SluSglCtrlPara = new List<Wlst.iif.SluSglCtrlParaSave.SingleSluSglCtrlPara>();
    FieldId = 0;
    }
    }
    public partial class MsgWithIif
    {
    public MsgWithIif()
    {
    Head = new Wlst.iif.Head();
    GetRtusWeekTimeInfoGet = new Wlst.iif.GetRtusWeekTimeInfo();
    RtuWeekTimeInfoBack = new List<Wlst.iif.RtuWeekTimeInfo>();
    NROrder1004 = new Wlst.iif.NRSetOrZcTudeAndDeviation();
    NROrder1010 = new Wlst.iif.NRSetOrZcLongitudeAndLatitude();
    NROrder1030 = new Wlst.iif.NRSetOrZcCtrlPara();
    NROrder1032 = new Wlst.iif.NRMeasure();
    NROrder1033 = new Wlst.iif.NROcLight();
    NROrder1034 = new Wlst.iif.NRGroupOcLight();
    NROrder1035 = new Wlst.iif.NRSetOrZcTimeTable();
    }
    }
    public partial class GetRtusWeekTimeInfo
    {
    public GetRtusWeekTimeInfo()
    {
    RtuIds = new List<int>();
    DtTimes = new List<long>();
    }
    }
    public partial class RtuWeekTimeInfo
    {
    public RtuWeekTimeInfo()
    {
    RtuId = 0;
    InfoItems = new List<Wlst.iif.RtuWeekTimeInfo.OneDayWeekTime>();
    }
    }
    public partial class NRSetOrZcTudeAndDeviation
    {
    public NRSetOrZcTudeAndDeviation()
    {
    NrSluId = 0;
    Op = 0;
    Longitude = 0;
    Latitude = 0;
    OnDeviation = 0;
    OffDeviation = 0;
    OnDo = new List<int>();
    OffDo = new List<int>();
    }
    }
    public partial class NRSetOrZcLongitudeAndLatitude
    {
    public NRSetOrZcLongitudeAndLatitude()
    {
    NrSluId = 0;
    Op = 0;
    Longitude = 0;
    Latitude = 0;
    }
    }
    public partial class NRSetOrZcCtrlPara
    {
    public NRSetOrZcCtrlPara()
    {
    NrSluId = 0;
    Op = 0;
    SluitemNum = 0;
    SluitemConfigField = new List<Wlst.iif.NRSetOrZcCtrlPara.SluitemConfig>();
    }
    }
    public partial class NRMeasure
    {
    public NRMeasure()
    {
    NrSluId = 0;
    SluitemNum = 0;
    CtrlId = 0;
    SluitemDataField = new List<Wlst.iif.NRMeasure.SluitemData>();
    }
    }
    public partial class NROcLight
    {
    public NROcLight()
    {
    NrSluId = 0;
    SluitemNum = 0;
    CtrlId = 0;
    SluitemCtrlField = new List<Wlst.iif.NROcLight.SluitemCtrl>();
    }
    }
    public partial class NRGroupOcLight
    {
    public NRGroupOcLight()
    {
    NrSluId = 0;
    GroupNum = 0;
    GroupCtrlField = new List<Wlst.iif.NRGroupOcLight.GroupCtrl>();
    }
    }
    public partial class NRSetOrZcTimeTable
    {
    public NRSetOrZcTimeTable()
    {
    NrSluId = 0;
    GroupNum = 0;
    Op = 0;
    GroupConfigField = new List<Wlst.iif.NRSetOrZcTimeTable.GroupConfig>();
    }
    }
}
