using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Sr.EquipemntLightFault.InfoHold;


namespace Wlst.Sr.EquipemntLightFault.Services
{
    /// <summary>
    /// 所有设备的故障类别信息
    /// </summary>
    public  class TmlFaultTypeInfoServices
    {
        private static FaultTypeInfo _info = new FaultTypeInfo();

        /// <summary>
        /// 
        /// </summary>
        public TmlFaultTypeInfoServices()
        {
            _info.InitStartService();
        }

        /// <summary>
        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___ </para> 
        /// </summary>
        public static ConcurrentDictionary<int, Wlst.client.FaultTypes.FaultTypeItem> InfoDictionary
        {
            get { return _info.InfoDictionary; } //将原始数据返回  数据安全性无法保证
        }

        /// <summary>
        /// 获取 设定的自定义规则
        /// </summary>
        public static ConcurrentDictionary<int, client.FaultTypes.FaultSettingRuleOne> Ruls
        {
            get { return _info.Ruls; } //将原始数据返回  数据安全性无法保证
        }

        /// <summary>
        /// 根据设备逻辑地址获取设备信息；不存在返回null
        /// </summary>
        /// <param name="id"></param>
        /// <returns>不存在返回null</returns>
        public static Wlst.client.FaultTypes.FaultTypeItem  GetInfoById(int id)
        {
            return _info.GetInfoById(id);
        }

        /// <summary>
        /// <para>获取报警故障列表</para>
        /// </summary>
        public static List<Wlst.client.FaultTypes.FaultTypeItem > GetInfoList
        {
            get { return _info.GetInfoList; }
        }

        /// <summary>
        /// <para>获取是否屏蔽手动报警</para>
        /// </summary>
        public static bool GetIsShieldAlarmsThatUserOcLightCause
        {
            get { return UserDisplayErrorSelfSetInfoHold.IsShieldAlarmsThatUserOcLightCause; }
        }

        /// <summary>
        /// 更新系统报警
        /// </summary>
        /// <param name="lst">自定义报警内容</param>
        /// <param name="hour">统计的报警时间段长度</param>
        /// <param name="volbelow">电压缺相下限值</param>
        /// <param name="ldlBelow"> 亮灯率报警下限值</param>
        public static void ExUpdateFauleTypeInfoforServer(List<Wlst.client.FaultTypes.FaultTypeItem> lst, int hour, int volbelow, double ldlBelow, List<client.FaultTypes.FaultSettingRuleOne> rules)
        {
            _info.UpdateFauleTypeInfo(lst,hour ,volbelow ,ldlBelow,rules );
        }


        /// <summary>
        /// 获取故障报警统计时间段
        /// </summary>
        public static  int GetTimeAlarmLong
        {
            get { return _info.TimeLongAlarm; }
        }


        /// <summary>
        /// 检测电压 认为电压为0的 最低条件
        /// </summary>
        public static int GetVolBelow
        {
            get { return _info.VolBelow ; }
        }


        public static double  GetLdlBelow
        {
            get { return _info.LdlBelow ; }
        }
    }
}
