using System.Collections.Generic;
using System.Linq;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Sr.EquipmentInfoHolding.Services;

namespace Wlst.Sr.EquipmentInfoHolding.Other
{
    public class EquipmentRunningInfoHolding
    {
        /// <summary>
        /// 确认只加载一次
        /// </summary>
        protected static bool BolLoad;

        /// <summary>
        /// 确认数据是否是发给自己的  
        /// 只对发给自己的数据处理
        /// </summary>
        protected static bool BolRequestformServer = false;


        protected static Dictionary<int, TerminalRunningInfomation> DictionaryInfo =
            new Dictionary<int, TerminalRunningInfomation>();

        #region get basic terminal infomation

        /// <summary>
        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___ </para> 
        /// <para>任何修改会使原始数据被修改形成脏数据 </para>
        /// </summary>
        public static Dictionary<int, TerminalRunningInfomation> TmlRunningInfoDictionary
        {
            get { return DictionaryInfo; //将原始数据返回  数据安全性无法保证
            }
        }

        public static TerminalRunningInfomation GetTmlRunningInfo(int id)
        {
            if (DictionaryInfo.ContainsKey(id))
            {
                return DictionaryInfo[id];
            }
            return null;
        }

        /// <summary>
        /// <para>获取升序排列的终端列表</para>
        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___  </para>
        /// <para>任何修改会使原始数据被修改形成脏数据 </para>
        /// </summary>
        public static List<TerminalRunningInfomation> TmlRunningInfoList
        {
            get
            {
                var lstReturn = new List<TerminalRunningInfomation>();
                var result = from pair in DictionaryInfo orderby pair.Key select pair;
                foreach (var p in result)
                {
                    //将原始数据的地址赋给返回list 共享原始数据   数据安全性无法保证
                    lstReturn.Add(p.Value);
                }
                return lstReturn;
            }
        }

        #endregion

        /// <summary>
        /// 与服务器交互数据 触发点
        /// </summary>
        protected static void ExRequestFaultTypeInfofromServer()
        {
            //if (BolRequestformServer) return;
            //int waitId = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            ////CETC50_Core.UtilityFunction.ProtocolEvent.ProtocolEventTokener(52301, _waitId, 5, Ex, ExOverTime);
            //LogInfo .Log( "正在请求终端运行信息!!!");
            //SndOrderServer.OrderSnd(EventIdAssign.TmlRunningInfoChange, null, null, waitId, 10,
            //                                                 6);

            //var info=Wlst .Sr .ProtocolCnt .ServerPart .fau
        }
    }
}