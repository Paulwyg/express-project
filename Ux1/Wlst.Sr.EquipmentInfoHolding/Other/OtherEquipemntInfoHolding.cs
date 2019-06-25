//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Practices.Prism.MefExtensions.Event;
//using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
//using Wlst.Cr.Core.CoreServices;
//using Wlst.Cr.Core.Models;
//using Wlst.Cr.WjEquipmentBaseModels.Interface;

//namespace Wlst.Sr.EquipmentInfoHolding.DataHolding
//{

//    /// <summary>
//    /// 其他设备信息持有；执行与服务器端数据交互；
//    /// 保存其他设备信息的基本信息供外籍提取使用;
//    /// 由于其他设备包含的内容可能非常多，可以是单灯设备 可以是防盗设备，设备获取由各自的模块去识别~~~ 然后提交给此类来持有
//    /// </summary>
//    public partial  class OtherEquipemntInfoHolding
//    {
//        protected static Dictionary<int, SupperEquipmentInstanceContains> DictionaryOtherEquipmentInfo = new Dictionary<int, SupperEquipmentInstanceContains>();


//        /// <summary>
//        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___ </para> 
//        /// <para>任何修改会使原始数据被修改形成脏数据 </para>
//        /// <para>修改请用TerminalInfomation类的clone方法进行克隆副本使用</para>
//        /// </summary>
//        public static Dictionary<int, SupperEquipmentInstanceContains> OtherEquipmentSupperInfoDictionary
//        {
//            get
//            {
//                return DictionaryOtherEquipmentInfo; //将原始数据返回  数据安全性无法保证
//            }
//        }

//        /// <summary>
//        /// 根据设备逻辑地址获取主设备信息；不存在返回null
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns>不存在返回null</returns>
//        public static SupperEquipmentInstanceContains GetOtherEquipmentInfo(int id)
//        {
//            if (DictionaryOtherEquipmentInfo.ContainsKey(id))
//            {
//                return DictionaryOtherEquipmentInfo[id];
//            }
//            return null;
//        }

//        /// <summary>
//        /// <para>获取升序排列的其他设备列表</para>
//        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___  </para>
//        /// <para>任何修改会使原始数据被修改形成脏数据 </para>
//        /// <para>修改请用具体类的clone方法进行克隆副本使用</para>
//        /// </summary>
//        public static List<SupperEquipmentInstanceContains> GetOtherEquipmentsInfoList
//        {
//            get
//            {
//                var lstReturn = new List<SupperEquipmentInstanceContains>();
//                var result = from pair in DictionaryOtherEquipmentInfo orderby pair.Key select pair;
//                foreach (var p in result)
//                {
//                    //将原始数据的地址赋给返回list 共享原始数据   数据安全性无法保证
//                    lstReturn.Add(p.Value);
//                }
//                return lstReturn;
//            }
//        }

//    }

//    public partial class OtherEquipemntInfoHolding
//    {

//        #region add list

//        /// <summary>
//        /// 增加设备
//        /// 仅程序内数据新增  
//        /// </summary>
//        /// <param name="otherEquipmentInfo">设备信息</param>
//        /// <param name="publidshEvent">如果需要发布事件的话则发布事件参数为 一个终端地址列表 仅一个参数列表 </param>
//        /// <param name="addOtherEnventId"> </param>
//        public static void AddOhterEquipmentInfo(List<SupperEquipmentInstanceContains> otherEquipmentInfo, bool publidshEvent, int addOtherEnventId = 0)
//        {
//            //发布事件  update
//            var args = new PublishEventArgs()
//                           {
//                               EventType = PublishEventType.Core,
//                               EventId = addOtherEnventId,
//                           };
//            foreach (var t in otherEquipmentInfo)
//            {
//                if (DictionaryOtherEquipmentInfo.ContainsKey(t.Value))
//                {
//                    UpdateOtherEquipmentInfo(t, publidshEvent);
//                    continue;
//                }

//                var iibas = t.Instances as IIEquipmentInfo;
//                if (iibas == null) return;
//                DictionaryOtherEquipmentInfo.Add(t.Value, t);
//                args.AddParams(t.Value);
//            }
//            if (publidshEvent && args.GetParams().Count > 0)
//            {
//                EventPublisher.EventPublish(args);
//            }
//        }

//        #endregion

//        #region add

//        /// <summary>
//        /// 增加设备
//        /// 仅程序内数据新增  
//        /// </summary>
//        /// <param name="otherEquipmentInfo">设备信息</param>
//        /// <param name="publidshEvent"> </param>
//        /// <param name="addOtherEnventId"> </param>
//        public static  void AddOhterEquipmentInfo(SupperEquipmentInstanceContains otherEquipmentInfo, bool publidshEvent, int addOtherEnventId = 0)
//        {
//            if (DictionaryOtherEquipmentInfo.ContainsKey(otherEquipmentInfo.Value))
//            {
//                UpdateOtherEquipmentInfo(otherEquipmentInfo, publidshEvent);
//                return;
//            }
            
//            var iibas = otherEquipmentInfo.Instances as IIEquipmentInfo;
//            if (iibas == null) return;

//            DictionaryOtherEquipmentInfo.Add(otherEquipmentInfo.Value, otherEquipmentInfo);
//            if (publidshEvent)
//            {
//                //发布事件  update
//                var args = new PublishEventArgs()
//                               {
//                                   EventType = PublishEventType.Core,
//                                   EventId = addOtherEnventId,
//                               };
//                args.AddParams(otherEquipmentInfo.Value);
//                EventPublisher.EventPublish(args);
//            }
//        }

//        #endregion

//        #region update

//        /// <summary>
//        /// 更设备信息  
//        /// 并发布事件
//        /// </summary>
//        /// <param name="otherEquipmentInfo">设备信息</param>
//        /// <param name="publidshEvent"> </param>
//        /// <param name="updateOtherEnventId"> </param>
//        public static void UpdateOtherEquipmentInfo(SupperEquipmentInstanceContains otherEquipmentInfo, bool publidshEvent, int updateOtherEnventId = 0)
//        {
//            var iibas = otherEquipmentInfo.Instances as IIEquipmentInfo;
//            if (iibas == null) return;

//            if (!DictionaryOtherEquipmentInfo.ContainsKey(otherEquipmentInfo.Value))
//            {
//                AddOhterEquipmentInfo(otherEquipmentInfo, publidshEvent);
//                return;
//            }
//            DictionaryOtherEquipmentInfo[otherEquipmentInfo.Value] = otherEquipmentInfo;
//            if (publidshEvent)
//            {
//                //发布事件  update
//                var args = new PublishEventArgs()
//                {
//                    EventType = PublishEventType.Core,
//                    EventId = updateOtherEnventId,
//                };
//                args.AddParams(otherEquipmentInfo.Value);
//                EventPublisher.EventPublish(args);
//            }
//        }

//        #endregion

//        #region delete

//        /// <summary>
//        /// 删除终端 
//        /// 仅删除程序内数据
//        /// </summary>
//        /// <param name="id">设备地址</param>
//        /// <param name="publidshEvent"> </param>
//        /// <param name="deleteOtherEnventId"> </param>
//        public static void DeleteOtherEquipmentInfo(int id, bool publidshEvent, int deleteOtherEnventId = 0)
//        {
//            if (DictionaryOtherEquipmentInfo.ContainsKey(id))
//            {
//                DictionaryOtherEquipmentInfo.Remove(id);
//                if (publidshEvent)
//                {
//                    //发布事件  update
//                    var args = new PublishEventArgs()
//                    {
//                        EventType = PublishEventType.Core,
//                        EventId = deleteOtherEnventId,
//                    };
//                    args.AddParams(id);
//                    EventPublisher.EventPublish(args);
//                }
//            }
//        }

//        #endregion

//    };
//}
