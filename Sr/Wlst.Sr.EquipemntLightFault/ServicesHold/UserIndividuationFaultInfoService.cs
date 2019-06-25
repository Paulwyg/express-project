//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Wlst.Sr.EquipemntLightFault.InfoHold;


//namespace Wlst.Sr.EquipemntLightFault.Services
//{
//    /// <summary>
//    /// 用户个性化报警信息设置
//    /// </summary>
//    public class UserIndividuationFaultInfoService
//    {
//        private static UserIndividuationFaultInfo _info =new UserIndividuationFaultInfo();

//        /// <summary>
//        /// 
//        /// </summary>
//        public UserIndividuationFaultInfoService()
//        {
//            _info.InitStartService();
//        }

//        /// <summary>
//        /// <para>获取原始的数据 </para> 
//        /// </summary>
//        public static Dictionary<int, Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem > InfoDictionary
//        {
//            get { return _info.InfoDictionary; } //将原始数据返回  数据安全性无法保证
//        }

//        /// <summary>
//        /// 获取该故障代码对本用户的设置信息，用户设置为空则报警
//        /// 根据故障代码获取用户自定义信息；不存在返回null；
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns>不存在返回null</returns>
//        public static Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem  GetInfoById(int id)
//        {
//            return _info.GetInfoById(id);
//        }

//        /// <summary>
//        /// <para>获取自定义故障列表，对本用户有效的列表</para>
//        /// </summary>
//        public static List<Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem > GetInfoList
//        {
//            get { return _info.GetInfoList; }
//        }

//        /// <summary>
//        /// 更新用户的自定义信息
//        /// </summary>
//        /// <param name="lst">自定义故障信息列表</param>
//        /// <param name="username">需要更新的用户名</param>
//        public static void ExUpdateFauleTypeInfoforServer(List<Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem > lst, string username)
//        {
//            _info.UpdateUserIndividuationFauleTypeInfo(lst, username);
//        }
//    }


//}
