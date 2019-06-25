using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;

namespace Wlst.Sr.EquipmentInfoHolding.Services
{
    
    /// <summary>
    /// 为数据持有提供基础服务
    /// </summary>
    public partial class AreaEmeHold : EventHandlerHelperExtendNotifyProperyChanged
    {
        private static AreaEmeHold _mySlef;

        public static AreaEmeHold MySlef
        {
            get
            {
                if (_mySlef == null) _mySlef = new AreaEmeHold();
                return _mySlef;
            }
        }



        /// <summary>
        /// 构造函数；执行事件注册
        /// </summary>
        protected AreaEmeHold()
        {
            //  this.InitAction();
        }

        /// <summary>
        /// 提供数据持有的数据结构  emeid- rtuid-switchs
        /// </summary>
        protected ConcurrentDictionary<int, ConcurrentDictionary<int, List<int>>> Info =
            new ConcurrentDictionary<int, ConcurrentDictionary<int, List<int>>>();   


        #region 提供外部对数据的操作Get Set

        /// <summary>
        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___ </para> 
        /// <para>任何修改会使原始数据被修改形成脏数据 </para>
        /// </summary>
        public ConcurrentDictionary<int, ConcurrentDictionary<int, List<int>>> AreaInfo
        {
            get { return Info; } //将原始数据返回  数据安全性无法保证
        }

        /// <summary>
        /// 根据设备逻辑地址获取设备信息；不存在返回  rtuid-switchs 
        /// </summary>
        /// <param name="emdId"></param>
        /// <returns></returns>
        public ConcurrentDictionary<int, List<int>> GetEmeInfo(int emdId)
        {
            if (!Info.ContainsKey(emdId)) return new ConcurrentDictionary<int, List<int>>( );
            return Info[emdId];
        }

   

        #endregion

    }

    /// <summary>
    /// Action
    /// </summary>
    public partial class AreaEmeHold 
    {

        internal void InitEvent()
        {
            this.AddEventFilterInfo(100, PublishEventType.ReCn);

        }/// <summary>
        /// 事件数据处理
        /// </summary>
        /// <param name="args"></param>
        public override void ExPublishedEvent(PublishEventArgs args)
        {
            if (args.EventType == PublishEventType.ReCn)
            {
                Request();
                return;
            }
        }

        public void InitStartService()
        {
            InitEvent();
            InitAction();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(Request, 1);

        }


        protected void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSpe .wlst_spe_eme_class_info ,
                //.ClientPart.wlst_Measures_server_ans_clinet_request_RtuOnLine,
                OrderRtuOnLine,
                typeof(AreaEmeHold), this);

        }
 
        protected void OrderRtuOnLine(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (infos.WstSpeEmeClassInfo  == null) return;

            if (infos.WstSpeEmeClassInfo.Op == 2)
            {
                if (infos.WstSpeEmeClassInfo.Item.Count == 0) return;
                int emeid = infos.WstSpeEmeClassInfo.Item[0].EmeId;
                if (Info.ContainsKey(emeid)) Info[emeid].Clear();
                else Info.TryAdd(emeid, new ConcurrentDictionary<int, List<int>>());

                foreach (var f in infos.WstSpeEmeClassInfo.Item[0].RtuItems)
                {
                    if (Info[emeid].ContainsKey(f.RtuId) == false) Info[emeid].TryAdd(f.RtuId, f.RtuSwitchOuts);
                    else Info[emeid][f.RtuId] = f.RtuSwitchOuts;
                }
            }
            else if (infos.WstSpeEmeClassInfo.Op == 1)
            {
                Info.Clear();
                foreach (var fx in infos.WstSpeEmeClassInfo.Item)
                {

                    int emeid = fx.EmeId;
                    if (Info.ContainsKey(emeid)) Info[emeid].Clear();
                    else Info.TryAdd(emeid, new ConcurrentDictionary<int, List<int>>());

                    foreach (var f in fx.RtuItems)
                    {
                        if (Info[emeid].ContainsKey(f.RtuId) == false) Info[emeid].TryAdd(f.RtuId, f.RtuSwitchOuts);
                        else Info[emeid][f.RtuId] = f.RtuSwitchOuts;
                    }
                }
            }

            //发布事件  
            var args = new PublishEventArgs()
            {
                EventType = PublishEventType.Core,
                EventId = EventIdAssign.AreaEmedataUpdate,
            };
            args.AddParams(infos.WstSpeEmeClassInfo.Op); //1、请求，2、更新
            EventPublish.PublishEvent(args);
        }

        /// <summary>
        /// 与服务器交互数据 触发点
        /// </summary>
        private void Request()
        {

            var info = Wlst.Sr.ProtocolPhone.LxSpe .wlst_spe_eme_class_info ;
            info.WstSpeEmeClassInfo .Op = 1;
            SndOrderServer.OrderSnd(info);
        }

    }

}
