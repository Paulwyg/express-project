using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.Cr.PPProtocolSvrCnt.Server.Logic;
using Wlst.mobile;

namespace Wlst.Cr.PPProtocolSvrCnt.Server
{

    /// <summary>
    /// 协议池
    /// </summary>
    public class ProtocolServer
    {
        //private static ProtocolMontorPool pv2 = new ProtocolMontorPool();


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instancesClassOfActionIn">获取的协议类型</param>
        /// <param name="action">等待的数据到达后需要处理的函数 第一参数为发送端的Session值，第二参数为数据协议类型</param>
        /// <param name="typeOfClassOfActionIn">处理函数所在的类的类名 </param>
        /// <param name="isRunInUiThread">是否要求在执行注册函数前检测函数是否注册 默认检测 </param>
        public static void RegisterProtocols(string cmd,
                                                Action<string, MsgWithMobile> action,
                                                Type typeOfClassOfActionIn, object instancesClassOfActionIn = null, bool isRunInUiThread = false) 
        {
            if (string .IsNullOrEmpty( cmd ) || action == null || typeOfClassOfActionIn == null) return;
            PriRegisterProtocols(cmd, action, typeOfClassOfActionIn, instancesClassOfActionIn, isRunInUiThread);
        }




        /// <summary>
        /// 删除协议
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="instancesClassOfActionIn"> </param>
        public static void DeleteProtocols(string cmd, object instancesClassOfActionIn = null)
        {
            ProtocolMontorPool.MySelf .DeleteProtocol(cmd, instancesClassOfActionIn);
        }

        /// <summary>
        /// 删除协议
        /// </summary>
        /// <param name="instancesClassOfActionIn"> </param>
        public static void DeleteProtocols(object instancesClassOfActionIn = null)
        {
            ProtocolMontorPool.MySelf.DeleteProtocol(instancesClassOfActionIn);
        }

        public static  Tuple<int, int> GetKeysValues()
        {
            return ProtocolMontorPool.MySelf.GetKeysValues();
        }

        /// <summary>
        /// 根据接收关键字获取 承载该数据data的类型 Func  并执行；
        /// 此函数未进行线程交换 在一个线程内执行
        /// </summary>
        /// <param name="key">查找关键字</param>
        /// <param name="session">客户标示 </param>
        /// <param name="data">数据 </param>
        /// <param name="runInUiOtherThread"> </param>
        /// <returns>是否执行成功</returns>
        public static bool ExcuteAction(string key, string session, MsgWithMobile data)
        {
            return PriExcuteAction(key, session, data);
        }

        /// <summary>
        /// 检测用户是否已经登录，在执行用户函数前执行，如果注册的时候要求检测登录的话，同时返回值为true则继续执行否则不继续执行
        /// </summary>
        public static  Func< string,bool > FunctionCheckSessionIsLogin;

        /// <summary>
        /// 如果本函数不为空  同时要求检测用户是否登录  并且该函数返回false则执行本函数
        /// </summary>
        public static Action<string> ActionIfSessionNotLogin;

        /// <summary>
        /// 根据接收关键字获取 承载该数据data的类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无法查找到则返回null</returns>
        public static Type GetType(string key)
        {
            return PriGetType(key);
        }

        public static bool GetIsRunInUiThread(string key)
        {
            return PriGetIsRunInUiThread(key);
        }


        #region  Private



        
        private static void PriRegisterProtocols(string cmd,
                                                    Action<string, MsgWithMobile> action,
                                                    Type classTypeOfActionIn, object instancesOfClassOfActionIn = null, bool isRunInUiThread = false) 
        {
            if (string .IsNullOrEmpty( cmd ) || action == null) return;
            if (string.IsNullOrEmpty(cmd )) return;
            var data = new PoolData()
                           {
                               ActionObj = action,
                               Cmd = cmd,
                               DataType = typeof (MsgWithMobile),
                               ActionName = action.Method.Name,
                               ClassTypeOfActionIn = classTypeOfActionIn,
                               InstancesClassOfActionIn = instancesOfClassOfActionIn,
                               IsRunInUiThread = isRunInUiThread,
                           };
            ProtocolMontorPool.MySelf.AddProtocol(data.Cmd, data);
        }




        /// <summary>
        /// 根据接收关键字获取 承载该数据data的类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns>无法查找到则返回null</returns>
        private static Type PriGetType(string key)
        {
            var info = ProtocolMontorPool.MySelf.GetAction(key);
            if (info == null) return null;
            if (info.Count < 0) return null ;
            return info[0].DataType;
        }


        private static bool PriGetIsRunInUiThread(string key)
        {
            var info = ProtocolMontorPool.MySelf.GetAction(key);
            if (info == null) return false ;
            if (info.Count < 0) return false;
            return info[0].IsRunInUiThread  ;
        }


        private delegate bool CmbdelegateMbl(string session, PoolData t, MsgWithMobile data);


        //private delegate void  CmbdelegateMblUi( string data);

        //private static  bool _isRunning = false;
        //private static object _obj = 1;

        //static void StartUiThread()
        //{
        //    if (_isRunning) return;

        //    lock (_obj)
        //    {
        //        try
        //        {
        //            if (_isRunning) return;
        //            _isRunning = true;

        //            Application.Current.Dispatcher.Invoke(
        //                System.Windows.Threading.DispatcherPriority.Normal,
        //                new CmbdelegateMblUi(PriExcuteActionsssMblRuninUi), "123");
        //            System.Windows.Forms.Application.DoEvents();
        //        }
        //        catch (Exception ex)
        //        {
                   
        //        }
        //    }
        //}

        //static void DoEvents()
        //{
        //    DispatcherFrame frame = new DispatcherFrame(true);
        //    Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate(object arg)
        //    {
        //        DispatcherFrame fr = arg as DispatcherFrame;
        //        fr.Continue = false;
        //    }, frame);
        //    Dispatcher.PushFrame(frame);
        //}  

        //private static void PriExcuteActionsssMblRuninUi(string data)
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            while (_queue .Count >0)
        //            {
        //                Tuple<string, PoolData, MsgWithMobile> tmp;
        //                if(_queue.TryDequeue( out tmp ) )
        //                {
        //                    PriExcuteActionsssMbl(tmp .Item1 , tmp .Item2 , tmp .Item3 );
        //                    System.Windows.Forms.Application.DoEvents();
        //                  //  DoEvents();

        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }

        //        try
        //        {
        //            System.Windows.Forms.Application.DoEvents();
        //            System.Windows.Forms.Application.DoEvents();
        //            System.Windows.Forms.Application.DoEvents();
        //            System.Windows.Forms.Application.DoEvents();
        //            System.Windows.Forms.Application.DoEvents();
        //            System.Windows.Forms.Application.DoEvents();
        //            System.Windows.Forms.Application.DoEvents();
        //            System.Windows.Forms.Application.DoEvents();
        //            System.Windows.Forms.Application.DoEvents();
        //            System.Windows.Forms.Application.DoEvents();
        //            System.Windows.Forms.Application.DoEvents();
        //            System.Windows.Forms.Application.DoEvents();
        //        }catch (Exception ex)
        //        {
                    
        //        }
        //    }
        //}

        //private static  ConcurrentQueue<Tuple<string, PoolData , MsgWithMobile>> _queue =
        //    new ConcurrentQueue<Tuple<string, PoolData, MsgWithMobile>>();
        private static  ConcurrentDictionary<string, Tuple<long, long>> _tmp = new ConcurrentDictionary<string, Tuple<long, long>>();
        private static  ConcurrentDictionary<string, Tuple<long, long>> _tmp1 = new ConcurrentDictionary<string, Tuple<long, long>>();
        //public static bool RunInUi = false  ;
   
        private static List<string> _errorOnRunInThread = new List<string>();

        private static bool PriExcuteAction(string key, string session, MsgWithMobile data)
        {
            // StartUiThread();
            var info = ProtocolMontorPool.MySelf.GetAction(key);
            if (info == null || info.Count < 1)
            {
                Common.WriteError.WriteLogError("Unablt to get the aciton of key:" + key);
                return false;
            }

            foreach (var t in info)
            {
                if (Application.Current != null && t.IsRunInUiThread)
                {

                    Application.Current.Dispatcher.Invoke(
                        System.Windows.Threading.DispatcherPriority.DataBind,
                        new CmbdelegateMbl(PriExcuteActionsssMblxxx),
                        session,
                        t, data);

                }
                else
                {

                    var sp = t.Cmd + t.ActionName;
                   // if (_errorOnRunInThread.Contains(sp) == false) _errorOnRunInThread.Add(sp);

                    if (_errorOnRunInThread.Contains(sp ) && Application.Current != null)
                    {
                        Application.Current.Dispatcher.Invoke(
                            System.Windows.Threading.DispatcherPriority.DataBind,
                            new CmbdelegateMbl(PriExcuteActionsssMblxxx),
                            session,
                            t, data);
                    }
                    else
                    {
                        //try
                        //{

                            PriExcuteActionsssMblxxx(session, t, data);
                        //}
                        //catch (Exception ex)
                        //{
                        //    if (_errorOnRunInThread.Contains(key) == false) _errorOnRunInThread.Add(key);
                        //    Common.WriteError.WriteLogError("NoErrRRR RunInBackthread error :" + key);
                        //    throw;
                        //}
                    }
                }
            }

            return true;
        }

        public static bool InitDebugTest = false;
        private static bool PriExcuteActionsssMblxxx(string session, PoolData t, MsgWithMobile data)
        {
            long d1 = DateTime.Now.Ticks;
            PriExcuteActionsssMbl(session, t, data);

            if (InitDebugTest)
            {
                var d2 = DateTime.Now.Ticks - d1;
                string title = t.ActionName + "-" + t.ClassTypeOfActionIn.FullName;
                if (_tmp.ContainsKey(title))
                {
                    _tmp[title] = new Tuple<long, long>(_tmp[title].Item1 + d2, _tmp[title].Item2 + 1);
                }
                else
                {
                    _tmp.TryAdd(title, new Tuple<long, long>(d2, 1));
                }

                var tti = t.ActionName;
                if (_tmp1.ContainsKey(tti))
                {
                    _tmp1[tti] = new Tuple<long, long>(_tmp1[tti].Item1 + d2, _tmp1[tti].Item2 + 1);
                }
                else
                {
                    _tmp1.TryAdd(tti, new Tuple<long, long>(d2, 1));
                }

                Wrlog();
            }
            return true;
        }


        private static  long _dtls = 0;
        static void Wrlog()
        {
            if (DateTime.Now.Ticks - _dtls > 60*50000000L)
            {
                _dtls = DateTime.Now.Ticks;
                var ntg =
                    (from t in _tmp where t.Value.Item1 > 10000000 orderby t.Value.Item1 descending select t).ToArray();
                foreach (var f in ntg)
                {
                    Common.WriteError.WriteLogError("NoErrRRR :" + (f.Value.Item1/1000000).ToString("d4") + "   " +
                                                    f.Value.Item2.ToString("d4") + "  " + f.Key);
                }

                ntg =
                    (from t in _tmp1 where t.Value.Item1 > 10000000 orderby t.Value.Item1 descending select t).ToArray();
                foreach (var f in ntg)
                {
                    Common.WriteError.WriteLogError("NoErrRRR :" + (f.Value.Item1/1000000).ToString("d4") + "   " +
                                                    f.Value.Item2.ToString("d4") + "  " + f.Key);
                }
            }
        }

        private static bool PriExcuteActionsssMbl(string session, PoolData t, MsgWithMobile data)
        {
            try
            {
                //var info = pv2.GetAction(key);
                //if (info == null || info.Count < 1)
                //{
                //    Common.WriteError.WriteLogError("Unablt to get the aciton of key:" + key);
                //    return false;
                //}




                //foreach (var t in info)
                //{

                    var action = t.ClassTypeOfActionIn.GetMethod(t.ActionName,
                                                                 BindingFlags.Instance | BindingFlags.Static |
                                                                 BindingFlags.Public |
                                                                 BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                    ;
                    if (action != null)
                    {
                        object[] par = new object[2];
                        par[0] = session;
                        par[1] = data;
                        if (action.IsStatic)
                        {
                            action.Invoke(null, par);
                        }
                        else
                        {
                            object obj = t.InstancesClassOfActionIn;
                            if (obj == null)
                                obj = Activator.CreateInstance(t.ClassTypeOfActionIn);
                            {

                                //Application.Current.Dispatcher.Invoke(
                                //    System.Windows.Threading.DispatcherPriority.Normal, new Cmbdelegate(action.Invoke), obj, par);

                                //Application.Current.Dispatcher.Invoke()

                                action.Invoke(obj, par);
                                if (Application.Current != null && t.IsRunInUiThread )
                                    System.Windows.Forms.Application.DoEvents();
                                //System.Windows.Forms.Application.DoEvents();
                                //System .Reflection 
                            }
                        }
                   // }
                }
                return true;
            }


            catch (Exception ex)
            {
                var sp = t.Cmd + t.ActionName;
                if (_errorOnRunInThread.Contains(sp) == false) _errorOnRunInThread.Add(sp);

                Common.WriteError.WriteLogError("ExcuteAction Error:" + ex);
            }
            return false;
        }
        #endregion
    }

}
