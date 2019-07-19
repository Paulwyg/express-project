using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.mobile;

namespace Wlst.Cr.PPProtocolSvrCnt.Server.Logic
{
    internal class ThreadPoolLogic
    {

        #region Thread Pool

        private static WaitCallback _callBack = new WaitCallback(PooledFunc);
        //private static long _currentRunningTask = 0;

        private static void PooledFunc(object state)
        {

            var info = state as TmpClass;
            if (info == null) return;
            //Interlocked.Increment(ref _currentRunningTask);
            // Common.WriteError.WriteLogError("-----------------------------------------------------" + DeCoding.CurrentRunningThread);
            try
            {
                if (info.ProtocolMbl != null)
                {
                    if (info.ProtocolMbl.Head == null) return;

                    var cmd = info.ProtocolMbl.Head.Cmd;
                    if (string.IsNullOrEmpty(cmd)) return;
                    Server.ProtocolServer.ExcuteAction(info.ProtocolMbl.Head.Cmd, info.Session, info.ProtocolMbl);
                }

            }
            catch (Exception ex)
            {
                Common.WriteError.WriteLogError("收到底层协议数据，当将底层协议数据解析为上底层协议数据时出现异常:" + ex);
            }
            //Interlocked.Decrement(ref _currentRunningTask);
            //if (DateTime.Now.Hour % 5 == 1 && DateTime.Now.Minute % 10 == 1 && DateTime.Now.Second % 10 == 1)
            //{
            //    try
            //    {
            //        Common.WriteError.WriteLogError("Not Error ,Jst print CurrentRunningTask:" + _currentRunningTask);
            //    }
            //    catch (Exception e)
            //    {
                    
            //    }
                
            //}
        }



        protected class TmpClass
        {

            public string Session;
            public MsgWithMobile ProtocolMbl;


            public TmpClass(string session, MsgWithMobile protocol)
            {
                this.Session = session;
                this.ProtocolMbl = protocol;


            }
        }

        #endregion

        //public static System.Collections.Concurrent.ConcurrentDictionary<long, WeakReference> Tr =
        //    new ConcurrentDictionary<long, WeakReference>();

        //private static long r = 1;

        public static void RunDispatch(string userSessionId, MsgWithMobile protocol)
        {


            ThreadPool.QueueUserWorkItem(_callBack, new TmpClass(userSessionId, protocol));
            
            //var rtr = Interlocked.Increment(ref r);
            //if (Tr.ContainsKey(rtr) == false) Tr.TryAdd(rtr, new WeakReference(protocol));
            //if (DateTime.Now.Second%20 == 1)
            //{
            //    var ntg = (from t in Tr where t.Value.Target == null select t.Key).ToList();
            //    WeakReference ob;
            //    foreach (var f in ntg)
            //    {
            //        if (Tr.ContainsKey(f)) Tr.TryRemove(f, out ob);
            //    }
            //    Common.WriteError.WriteLogError("WeakReference Shows  " + Tr.Count);
            //}
        }

      

    }
}
