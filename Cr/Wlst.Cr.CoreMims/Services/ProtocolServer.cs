using System;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.mobile;

namespace Wlst.Cr.CoreMims.Services
{
    /// <summary>
    /// 注册协议
    /// </summary>
    public class ProtocolServer
    {
        /// <summary>
        /// 注册指定协议结构的数据的处理方法，方法要求最好为静态方法，如果不为静态方法要求类必须有一个无参构造函数，否则注册的方法永不执行
        /// </summary>
        /// <param name="instancesClassOfActionIn">如果注册的处理函数为静态函数则为null  如果不为静态函数 希望返回的数据在本类实例中处理则为 this  否则新建类处理 </param>
        /// <param name="cmd"> </param>
        /// <param name="action">此方法要求必须为静态方法， 等待的数据到达后需要处理的函数 第一参数为发送端的Session值，第二参数为数据协议类型</param>
        /// <param name="typeOfClassOfActionIn">处理函数所在的类的类名 </param>
        /// <param name="runInOtherThread">是否在非UI线程运行 默认false既在UI线程运行 </param>
        public static void RegistProtocol(string cmd,
                                             Action<string, MsgWithMobile> action,
                                             Type typeOfClassOfActionIn, object instancesClassOfActionIn, bool runInOtherThread = false)
        {
            Wlst.Sr.PPPandSocketSvr.Server.ProtocolServices.RegisterProtocols(cmd, action, typeOfClassOfActionIn, instancesClassOfActionIn, runInOtherThread 
                                                                              );//runInOtherThread
        }


        /// <summary>
        /// 注册指定协议结构的数据的处理方法，方法要求最好为静态方法，如果不为静态方法要求类必须有一个无参构造函数，否则注册的方法永不执行
        /// </summary>
        /// <param name="instancesClassOfActionIn">如果注册的处理函数为静态函数则为null  如果不为静态函数 希望返回的数据在本类实例中处理则为 this  否则新建类处理 </param>
        /// <param name="cmd"> </param>
        /// <param name="action">此方法要求必须为静态方法， 等待的数据到达后需要处理的函数 第一参数为发送端的Session值，第二参数为数据协议类型</param>
        /// <param name="typeOfClassOfActionIn">处理函数所在的类的类名 </param>
        /// <param name="runInOtherThread">是否在非UI线程运行 默认false既 不在UI线程运行 </param>
        public static void RegistProtocol(MsgWithMobile cmd,
                                             Action<string, MsgWithMobile> action,
                                             Type typeOfClassOfActionIn, object instancesClassOfActionIn, bool runInOtherThread = false)
        {
            if (cmd == null || cmd.Head == null || string.IsNullOrEmpty(cmd.Head.Cmd)) return;
            Wlst.Sr.PPPandSocketSvr.Server.ProtocolServices.RegisterProtocols(cmd.Head.Cmd, action, typeOfClassOfActionIn, instancesClassOfActionIn, runInOtherThread 
                                                                              );//runInOtherThread
        }




        /// <summary>
        /// 删除协议
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="instancesClassOfActionIn">如果为静态函数则为null  如果不为静态函数 希望返回的数据在本类实例中处理则为 this  否则新建类处理 </param>
        public static void UnsubscribeProtocols(string cmd, object instancesClassOfActionIn)
        {
            if (cmd == null || instancesClassOfActionIn == null) return;
            Wlst.Sr.PPPandSocketSvr.Server.ProtocolServices.DeleteProtocols(cmd, instancesClassOfActionIn);
        }

        /// <summary>
        /// 删除协议
        /// </summary>
        /// <param name="instancesClassOfActionIn">如果为静态函数则为null  如果不为静态函数 希望返回的数据在本类实例中处理则为 this  否则新建类处理 </param>
        public static void UnsubscribeProtocols(object instancesClassOfActionIn)
        {
            if (instancesClassOfActionIn == null) return;
            Wlst.Sr.PPPandSocketSvr.Server.ProtocolServices.DeleteProtocols(instancesClassOfActionIn);
        }
    }

}
