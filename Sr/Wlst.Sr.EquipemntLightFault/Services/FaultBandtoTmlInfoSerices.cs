using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Sr.EquipemntLightFault.InfoHold;

namespace Wlst.Sr.EquipemntLightFault.Services
{
    /// <summary>
    /// 终端与故障信息绑定服务;仅终端的绑定信息部包含单灯的
    /// </summary>
    public class FaultBandtoTmlInfoSerices
    {
      //  private static TmlFaultBindingInfo _info = new TmlFaultBindingInfo(EventIdAssign .FaultBandtoEquipmentRequest ,EventIdAssign .FaultBandtoEquipmentUpdate );


        private static TmlFaultBindingInfo _info = new TmlFaultBindingInfo();
        public FaultBandtoTmlInfoSerices()
        {
            _info.InitStartService();
        }

        /// <summary>
        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___ </para> 
        /// <para>任何修改会使原始数据被修改形成脏数据 </para>
        /// <para>地址 故障编码地址</para>
        /// </summary>
        public static ConcurrentDictionary<int , List< int>> InfoDictionary
        {
            get { return _info.InfoDictionary; } //将原始数据返回  数据安全性无法保证
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rtuId">地址</param>
        /// <param name="faultCodeId">故障编码地址</param>
        /// <returns>不存在返回false</returns>
        public static bool GetFaultBandtoTmlInfo(int rtuId, int faultCodeId)
        {
            return _info.GetIsAlarmByIdAndFaultId(rtuId, faultCodeId);
        }


        /// <summary>
        /// 更新信息到服务器
        /// </summary>
        /// <param name="info">需要更新的信息</param>
        public static void UpdateFaultBandTmlInfo(List<Tuple<int, int>> info,int areaId)
        {
            _info.UpdateFaultBandTmlInfo(info,areaId );
        }

        /// <summary>
        /// 与服务器交互数据 触发点
        /// </summary>
        public static  void RequestFaultBandTmlInfo()
        {
            _info.RequestFaultBandTmlInfo();
        }
    }
}
