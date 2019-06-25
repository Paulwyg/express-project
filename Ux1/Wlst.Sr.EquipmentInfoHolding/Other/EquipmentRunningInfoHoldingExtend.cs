using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.AsyncTask;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Sr.EquipmentInfoHolding.Services;

namespace Wlst.Sr.EquipmentInfoHolding.Other
{
    public partial class EquipmentRunningInfoHoldingExtend : EquipmentRunningInfoHolding
    {
        /// <summary>
        /// 增加或更新数据信息
        /// </summary>
        /// <param name="info">信息</param>
        protected void AddOrUpdateInfo(TerminalRunningInfomation info)
        {
            if (info == null) return;

            if (DictionaryInfo.ContainsKey(info.RtuId))
            {
                DictionaryInfo[info.RtuId] = info;
            }
            else
            {
                DictionaryInfo.Add(info.RtuId, info);
            }
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="rtuId"> </param>
        protected void DeleteInfo(int rtuId)
        {
            if (DictionaryInfo.ContainsKey(rtuId))
            {
                DictionaryInfo.Remove(rtuId);
            }
        }

        ///// <summary>
        ///// 删除数据
        ///// </summary>
        ///// <param name="rtuId"> </param>
        //protected void DeleteFaultTypeInfo(int rtuId)
        //{

        //    if (DictionaryInfo.ContainsKey(rtuId))
        //    {
        //        DictionaryInfo.Remove(rtuId);
        //    }
        //}
    };

    public partial class EquipmentRunningInfoHoldingExtend
    {

        public EquipmentRunningInfoHoldingExtend()
        {
            EventPublisher.AddEventSubScriptionTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);


        }




        /// <summary>
        /// 执行数据初始化并注册事件
        /// </summary>
        public void InitLoad()
        {
            if (BolRequestformServer) return;
            BolRequestformServer = true;
            ExRequestFaultTypeInfofromServer();
        }

        #region IEventAggregator Subscription

        /// <summary>
        /// 事件过滤
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private bool FundOrderFilter(PublishEventArgs args)
        {
            if (args.EventType == PublishEventType.Sevr)
            {
                switch (args.EventId)
                {
                    case EventIdAssign.TmlRunningInfoChange:
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        private void FundEventHandler(PublishEventArgs args)
        {
            try
            {
                Async.Run(new Action<object>(ExExecuteEvent), args);
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }

        /// <summary>
        /// 事件执行服务器数据到达  更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private void ExExecuteEvent(object obj)
        {
            var args = obj as PublishEventArgs;
            if (args == null) return;
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                                                  new Action<PublishEventArgs>(ExExecuteEventIns),
                                                  args);
            return;
        }

        /// <summary>
        /// 线程执行 具体执行
        /// </summary>
        private void ExExecuteEventIns(PublishEventArgs args)
        {
            var tmlInfoExchangeforServer = args.GetParams()[1] as TerminalRunningInfomationExChangeforServer;
            if (tmlInfoExchangeforServer == null) return;
            if (tmlInfoExchangeforServer.LstInfo == null) return;
            switch (args.EventId)
            {
                case EventIdAssign.TmlRunningInfoChange: //tml
                    UpdateTmlCoreData(tmlInfoExchangeforServer);
                    break;
            }
        }

        #endregion

        /// <summary>
        /// 线程执行数据更新   需要预先赋值  _tmlInfoExchangeforUpdatetmlinfo
        /// </summary>
        private void UpdateTmlCoreData(TerminalRunningInfomationExChangeforServer tmlInfoExchangeforUpdatetmlinfo)
        {
            if (tmlInfoExchangeforUpdatetmlinfo == null) return;
            //如果型号发生变化 需要进一步修改 
            if (tmlInfoExchangeforUpdatetmlinfo.LstInfo == null || tmlInfoExchangeforUpdatetmlinfo.LstInfo.Count == 0)
                return;
            try
            {
                var tmp = new PublishEventArgs()
                              {
                                  EventId =
                                      EventIdAssign.
                                      TmlRunningInfoChange,
                                  EventType = PublishEventType.Core
                              };
                DictionaryInfo.Clear();
                foreach (var t in tmlInfoExchangeforUpdatetmlinfo.LstInfo)
                {
                    try
                    {
                        this.AddOrUpdateInfo(t);
                        tmp.AddParams(t.RtuId);
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLogError("Add tml faults info error:" +
                                                                           ex.ToString());
                    }
                }

                EventPublisher.EventPublish(tmp);
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error to update tml faults ,ex:" + ex);
            }
        }
    }
}