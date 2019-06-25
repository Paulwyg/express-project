using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Sr.EquipmentInfoHolding.Services
{
    /// <summary>
    /// 其他信息
    /// </summary>
   public class Others
   {
       /// <summary>
       /// 手动开灯二次确认
       /// </summary>
       public static int OpenCloseLightSecondConfirm = 0;
       /// <summary>
       /// 手动关灯二次确认
       /// </summary>
       public static int CloseLightSecondConfirm = 0;
       /// <summary>
       /// 是否备份数据库到客户端
       /// </summary>
        public static bool CopyDataBaseFromSvr = true;
       /// <summary>
       /// 是否自动弹出故障报警界面
       /// </summary>
        public static bool IsShowThisViewOnNewErrArriveInfo = false;
        /// <summary>
        /// 是否显示故障报警数量
        /// </summary>
        public static bool IsShowNewErrArriveOnUi = false;
        /// <summary>
        /// 老终端支持软件下发二次开关灯(不稳定)
        /// </summary>
        public static bool IsOldUseTwoOpenLightSection = false;

        /// <summary>
        /// 支持批量终端停运
        /// </summary>
        public static bool IsShowLotStopRunning = false;

        /// <summary>
        /// 开关灯后立即显示时间表报表
        /// </summary>
        public static bool IsShowTimeTableOnTime = false;

        /// <summary>
        /// 开启语音报警
        /// </summary>
        public static bool IsAllowVoiceReport = false;
        /// <summary>
        /// 开启故障查询统计
        /// </summary>
        public static bool IsShowErrsCal = false;

        /// <summary>
        /// 控制中心屏蔽报警
        /// </summary>
        public static bool IsControlCenterNoErr = false;

        /// <summary>
        /// 故障查询界面，显示参数信息
        /// </summary>
        public static int IsShowArgsInErrInfo = 0;


        ///// <summary>
        ///// 在线数量仅显示终端
        ///// </summary>
        //public static bool IsOnlyShowTmlOnline = false;

       /// <summary>
       /// 终端历史数据查询 是否显示高级选项
       /// </summary>
        public static bool HsdataQueryShGjOp = false;

        /// <summary>
        /// 最新数据屏蔽回路显示电流
        /// </summary>
        public static bool NewdataShieldLoopShA = false;

        /// <summary>
        /// 最新数据屏蔽回路显示电压
        /// </summary>
        public static bool NewdataShieldLoopShV = false;

        ///// <summary>
        ///// 是否显示快速操作
        ///// </summary>
        //public static bool IsShowFastControl = true;
        ///// <summary>
        ///// 时候有电流时亮灯率为100%
        ///// </summary>
        //public static bool IsLdlAs100Per = false;

        /// <summary>
        /// 多终端勾选后，右击故障查询
        /// </summary>
        public static List<int> TmlTreeChked = new List<int>();

        /// <summary>
        /// 是否有火零检测功能
        /// </summary>
        public static bool IsShowHLbph = false;

        /// <summary>
        /// 终端火零不平衡报警电流差值上限
        /// </summary>
        public static double HLbphUpper = 0;

        /// <summary>
        /// 终端火零不平衡报警电流差值下限
        /// </summary>
        public static double HLbphLower = 0;

        /// <summary>
        /// 终端火零不平衡更新报警值
        /// </summary>
        public static double HlbphUpdateAlarm = 0;

        /// <summary>
        /// 终端火零不平衡检测报警次数
        /// </summary>
        public static double HLbphTimer = 0;


        /// <summary>
        /// 终端火零不平衡应急关灯屏蔽时间
        /// </summary>
        public static int HLbphShieldTimer = 0;


        /// <summary>
        /// 添加终端时自动载入参数的路径
        /// </summary>
        public static string LoadParaPath = string.Empty;

        /// <summary>
        /// 最新数据屏蔽回路显示电压
        /// </summary>
        public static string SystemName = "";

        /// <summary>
        /// 终端备注名称
        /// </summary>
        public static string RemarkName = "";


        /// <summary>
        /// 控制中心界面已经打开
        /// </summary>
        public static bool ControlCenterIsShow = false;


        /// <summary>
        /// 城市代号 记录在config下City.txt中   登录时 赋值 各处特殊化代码直接判断
        /// </summary>
        public static int CityNum = 0;



        /// <summary>
        /// 用户点击的设备Id  lvf  2018年4月16日13:12:26
        /// </summary>
        public static int CurrentSelectRtuId = 0;


        /// <summary>
        /// 是否选测失败 lvf  2018年8月6日15:21:13
        /// </summary>
        public static bool IsMeasureFail = false ;


        /// <summary>
        /// 服务器IP地址 lvf  2018年8月6日15:21:13
        /// </summary>
        public static string SeverIpAddr = "";


        /// <summary>
        /// 服务器端口号 lvf  2018年8月6日15:21:13
        /// </summary>
        public static string SeverPort = "";

        /// <summary>
        /// 记录召测终端 lvf  2018年8月7日15:21:13
        /// </summary>
        public static Dictionary<int ,DateTime> ZcRtus = new Dictionary<int, DateTime>();

        /// <summary>
        /// 终端类型动态配置 lvf  2018年12月11日09:26:11 
        /// key：index  Value：typeName，typeColor
        /// </summary>
        public static Dictionary<int, Tuple<string,string>> LocalRtuType = new Dictionary<int, Tuple<string,string>>();

        /// <summary>
        /// 终端备注动态配置 lvf  2019年1月15日09:39:03
        /// key：index  Value：RemarkName
        /// </summary>
        public static Dictionary<int, string> LocalRtuRemarks = new Dictionary<int, string>();


        /// <summary>
        /// 服务器http服务端口号 lvf  2018年8月6日15:21:13
        /// </summary>
        public static string SeverHttpPort = "0";

        ///// <summary>
        ///// 记录召测终端 lvf  2018年8月7日15:21:13
        ///// </summary>
        //public static double GlobalAShield = 0;

        ///// <summary>
        ///// 是否显示快速操作
        ///// </summary>
        //public static bool IsShowFastControl = true;
        ///// <summary>
        ///// 时候有电流时亮灯率为100%
        ///// </summary>
        //public static bool IsLdlAs100Per = false;

       /// <summary>
       /// 日出
       /// </summary>
        public static int Sunrise = 0;

       /// <summary>
       /// 日落
       /// </summary>
        public static int Sunset = 0;

       /// <summary>
       /// lvf 2019年4月28日14:16:42 地区设置
       /// </summary>
        public static List<Tuple<int, string>> RegionItems = new List<Tuple<int, string>>(); 
   }

   ///// <summary>
   ///// 终端最新数据回路附加显示信息
   ///// </summary>
   //public static class RtuLoopsAttachShowInfo
   //{
   //    /// <summary>
   //    /// 附加显示信息  终端地址-回路  显示信息-是否加重显示 
   //    /// </summary>
   //    public static Dictionary<Tuple<int, int>, Tuple<string, bool, long>> Info =
   //        new Dictionary<Tuple<int, int>, Tuple<string, bool, long>>();


   //    /// <summary>
   //    /// 更新回路附加显示信息 最新数据
   //    /// </summary>
   //    /// <param name="rtuId"></param>
   //    /// <param name="loopid"></param>
   //    /// <param name="info"></param>
   //    /// <param name="error"></param>
   //    /// <param name="dthappentime"> </param>
   //    public static void UpdateAttachInfo(int rtuId, int loopid, string info, bool error, long dthappentime)
   //    {
   //        var tmp = new Tuple<int, int>(rtuId, loopid);
   //        if (Info.ContainsKey(tmp)) Info.Remove(tmp);


   //        Info.Add(tmp, new Tuple<string, bool, long>(info, error, dthappentime));
   //    }


   //    /// <summary>
   //    /// 获取回路附加显示信息 最新数据
   //    /// </summary>
   //    /// <param name="rtuId"></param>
   //    /// <param name="loopId"></param>
   //    /// <returns></returns>
   //    public static Tuple<string, bool, long> GetRtuLoopAttachInfo(int rtuId, int loopId)
   //    {
   //        if (Services.EquipmentDataInfoHold.MySlef.Info.ContainsKey(rtuId) == false) return null;
   //        var attlst = Services.EquipmentDataInfoHold.MySlef.Info[rtuId ].EquipmentsThatAttachToThisRtu ;//.GetMainEquipmentAttachedLst(rtuId);
   //        if (attlst == null || attlst.Count == 0) return null;
   //        foreach (var t in attlst)
   //        {
   //            if (Services.EquipmentIdAssignRang.IsRtuIsLine(t))
   //            {
   //                var lineque = Services.EquipmentDataInfoHold.GetInfoById(t);// Services.ServicesEquipemntInfoHold.GetEquipmentInfo(t);
   //                if (lineque == null) continue;
   //                var tmpsg = lineque as Model .Wj1090Ldu ;
   //                if (tmpsg == null) continue;
   //                var binds =
   //                    (from g in tmpsg.WjLduLines.Values   where g.LduLoopId  == loopId && g.IsUsed select g.LduLineId).ToList();
   //                if (binds.Count == 0) continue;
   //                var tmps = new Tuple<int, int>(t, binds[0]);
   //                if (Info.ContainsKey(tmps)) return Info[tmps];
   //                else return new Tuple<string, bool, long>("正常", false, 0);
   //            }
   //        }
   //        return null;
   //    }
   //}
}
