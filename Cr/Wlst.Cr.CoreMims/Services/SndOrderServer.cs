using System;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.mobile;

namespace Wlst.Cr.CoreMims.Services
{
    /// <summary>
    /// 数据发送入口
    /// </summary>
    public class SndOrderServer
    {

        /// <summary>
        /// 发送数据前
        /// </summary>
        public static  Action<MsgWithMobile>  BeforeSnd ;

        /// <summary>
        /// 发送数据到服务端
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instances"></param>
        /// <param name="waitTime">发送一次等待回应时间 </param>
        /// <param name="sndTimesWhileSndWithoutRrespons">如果发送失败 是否继续发送 继续发送的次数  额外发送的次数 </param>
        /// <param name="isUserOperarot">是否为用户手动的操作 </param>
        /// <returns></returns>
        public static bool OrderSnd(MsgWithMobile instances, int waitTime = 0,
                                       int sndTimesWhileSndWithoutRrespons = 0, bool isUserOperarot = false)
        {
            if (isUserOperarot && BeforeSnd != null)
            {
                try
                {
                    BeforeSnd(instances);
                }
                catch (Exception ex)
                {

                }
            }
            return Wlst.Sr.PPPandSocketSvr.Server.SocketClient.SndData(instances, waitTime,
                                                                       sndTimesWhileSndWithoutRrespons);
        }

    }
}
