using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.client;

namespace Wlst.Sr.EquipmentInfoHolding.Services
{

    /// <summary>
    /// 为数据持有提供基础服务
    /// </summary>
    public partial class AreaInfoHold : EventHandlerHelperExtendNotifyProperyChanged
    {
        private static AreaInfoHold _mySlef;

        public static AreaInfoHold MySlef
        {
            get
            {
                if (_mySlef == null) _mySlef=new AreaInfoHold();
                return _mySlef;
            }
        }



        /// <summary>
        /// 构造函数；执行事件注册
        /// </summary>
        protected AreaInfoHold()
        {
          //  this.InitAction();
        }

        /// <summary>
        /// 提供数据持有的数据结构
        /// </summary>
        protected ConcurrentDictionary<int, AreaInfo.AreaItem> Info = new ConcurrentDictionary<int, AreaInfo.AreaItem>();


        #region 提供外部对数据的操作Get Set

        /// <summary>
        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___ </para> 
        /// <para>任何修改会使原始数据被修改形成脏数据 </para>
        /// </summary>
        public ConcurrentDictionary<int, AreaInfo.AreaItem> AreaInfo
        {
            get { return Info; } //将原始数据返回  数据安全性无法保证
        }

        /// <summary>
        /// 根据设备逻辑地址获取设备信息；不存在返回 kong 
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public List<int> GetRtuInArea(int areaId)
        {
            if (!Info.ContainsKey(areaId)) return new List<int>();
            return Info[areaId].LstTml;
        }

        /// <summary>
        /// 获取终端属于哪一个区域 无法查阅则返回-1
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns></returns>
        public int GetAreaThatRtuIn(int rtuId)
        {
            foreach (var f in Info )
            {
                if (f.Value.LstTml.Contains(rtuId)) return f.Key;
            }
            return 0;
        }

        public AreaInfo.AreaItem GetAreaInfo(int areaId)
        {
            if (!Info.ContainsKey(areaId)) return null;
            return Info[areaId];
        }

        /// <summary>
        /// 返回-1 表示无法查阅,查询设备所属的区域
        /// </summary>
        /// <param name="rtuId"></param>
        /// <returns>-1 表示查阅不到</returns>
        public int GetRtuBelongArea(int rtuId)
        {
            foreach (var f in Info)
            {
                if (f.Value.LstTml.Contains(rtuId)) return f.Key;
            }
            return -1;
        }

        /// <summary>
        /// 获取所有区域控制的终端
        /// </summary>
        /// <returns></returns>
        public List<int> GetRtuInAllArea()
        {
            var rtn = new List<int>();
            foreach (var f in Info) rtn.AddRange(f.Value.LstTml);
            return rtn;
        }

        #endregion

    }

    /// <summary>
    /// Action
    /// </summary>
    public partial class AreaInfoHold : EventHandlerHelperExtendNotifyProperyChanged
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
                Request() ;
                return;
            }
        }

        public void InitStartService()
        {
            InitEvent();
            InitAction();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(Request, 1);

        }


        protected  void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxAreaGrp.wls_area_info,
                //.ClientPart.wlst_Measures_server_ans_clinet_request_RtuOnLine,
                OrderRtuOnLine,
                typeof (AreaInfoHold), this);
            
        }

        private List<int> alreadyDel;
 
        public List<int> AlreadyDel
        {
            get
            {
                if (alreadyDel == null) alreadyDel = new List<int>();
                return alreadyDel;
            }
        } 

        protected void OrderRtuOnLine(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (infos.WstAreagrpAreaInfo == null) return;
            Info.Clear();
            foreach (var f in infos.WstAreagrpAreaInfo.AreaItems)
            {
                if (Info.ContainsKey(f.AreaId)) Info[f.AreaId] = f;
                else Info.TryAdd(f.AreaId, f);
            }
            if (infos.WstAreagrpAreaInfo.Op == 2)
            {
                EquipmentInfoHolding.Services.EquipmentDataInfoHold.MySlef.RequestEquipmentInfoLstfromServer();
            }

            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D == false)
            {
                var tmplst = new List<int>();
                
                tmplst.AddRange(Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR);
                tmplst.AddRange(Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW);
                tmplst.AddRange(Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaX);
                var originalLst = new List<int>();
                foreach(var f in tmplst)
                {
                    if (!originalLst.Contains(f) && f!=-1)
                    {
                        originalLst.Add(f);
                    }
                }
                #region Original

                //bool find = false;
                //foreach (var f in tmplst)
                //{
                //    if (Info.ContainsKey(f))
                //    {
                //        find = true;
                //        break;
                //    }
                //}
                //if (find == false)
                //{
                //    WlstMessageBox.Show("请下线", "您所管理的区域已经删除，您被迫下线!!!", WlstMessageBoxType.Ok);
                //    Environment.Exit(0);
                //}

                #endregion


                var delLst = new List<int>();
                string delLsts = "";
                foreach (var f in originalLst)
                {
                    if (!Info.ContainsKey(f)&&!delLst.Contains(f)&&!AlreadyDel.Contains(f))
                    {
                        delLst.Add(f);     
                    }
                }

                
                

                if (delLst.Count > 0)
                {
                    if (AlreadyDel.Count == originalLst.Count - 1)
                    {
                        MessageBox.Show( "您所管理的区域已经删除，您被迫下线!!!","请下线", MessageBoxButton.OK);
                        tmplst.Clear();
                        originalLst.Clear();
                        delLst.Clear();                       
                        Environment.Exit(0);
                    }
                    else
                    {
                        delLsts = string.Join(",", delLst.ToArray());                   
                        MessageBox.Show( "您所管理的区域中，id为" + delLsts + "的区域被管理员删除", "区域信息发生变动",MessageBoxButton.OK);
                        AlreadyDel.AddRange(delLst);
                        tmplst.Clear();
                        originalLst.Clear();
                        delLst.Clear(); 
                    }
                }


            }


            //发布事件  
            var args = new PublishEventArgs()
                           {
                               EventType = PublishEventType.Core,
                               EventId = EventIdAssign.AreaInfoChanged,
                           };
            EventPublish.PublishEvent(args);


           var   arg = new PublishEventArgs()
            {
                EventId = EventIdAssign.SingleInfoGroupAllNeedUpdate,
                EventType = PublishEventType.Core
            };
            EventPublish.PublishEvent(arg);

            var  aarg = new PublishEventArgs()
            {
                EventId = EventIdAssign.MulityInfoGroupAllNeedUpdate,
                EventType = PublishEventType.Core
            };
            EventPublish.PublishEvent(aarg);

            


        }

        /// <summary>
        /// 与服务器交互数据 触发点
        /// </summary>
        private void Request()
        {

            var info = Wlst.Sr.ProtocolPhone.LxAreaGrp.wls_area_info;
                //.wlst_sys_rtu_online;//.ServerPart.wlst_Measures_clinet_request_RtuOnLine;
            info.WstAreagrpAreaInfo.Op = 1;
            SndOrderServer.OrderSnd(info);
        }


        /// <summary>
        ///   请求服务器更新数据
        /// </summary>
        public void UpdateAreaInfo(List<AreaInfo.AreaItem> areainfo)
        {
            //var ntg = (from t in areainfo orderby t.Index ascending select t).ToList();
            var info = Wlst.Sr.ProtocolPhone.LxAreaGrp.wls_area_info;
            foreach (var t in areainfo)
            {
                info.WstAreagrpAreaInfo.AreaItems.Add(new AreaInfo.AreaItem()
                {
                    AreaName = t.AreaName,
                    AreaId = t.AreaId,
                    LstTml = t.LstTml,
                });

            }

            info.WstAreagrpAreaInfo.Op = 2;

            SndOrderServer.OrderSnd(info, 10, 6);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                0, "所有分组", Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "更新区域信息");


        }
    }

}
