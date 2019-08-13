using System;
using System.Collections.Generic;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.Cr.PPProtocolSvrCnt.Server.Logic;
using Wlst.mobile;

namespace Wlst.Cr.PPProtocolSvrCnt.Server
{
    /// <summary>
    /// 解码
    /// </summary>
    public class DeCoding
    {
        //public static int CurrentRunningThread = 0;
        /// <summary>
        /// 默认不屏蔽 屏蔽时间设置为程序启动后的2分钟
        /// </summary>
        /// <param name="isShield"></param>
        /// <param name="cmds"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void Sheiled(bool isShield, List<string> cmds, long start=0, long end=0)
        {
            Shielding = isShield;
            LstCmds = cmds;
          if(start !=0)  StartUp = start;
          if (end != 0) StartEndUp = end;
        }

        private  static bool Shielding = false;
        private static long StartUp = 0;
        private static long StartEndUp = 0;
        private static List<string> LstCmds = new List<string>(); 


        /// <summary>
        /// 当客户端或服务端接收到协议数据时 处理协议数据;如果协议数据携带有Rcv=2标示的则此处解析后将不再继续执行
        /// </summary>
        /// <param name="session">接收到数据的套接字标示 </param>
        /// <param name="data">按照协议解析出来的数据 目前为``之间的数据</param>
        /// <returns>返回解析后的协议携带的 关键字信息 以及数据Guid值 如果解析错处返回null</returns>
        public static GuidWithCmd OnReceiveCntOrSvrData(string session, string data)
        {
            try
            {
                if (StartUp == 0)
                {
                    StartUp = DateTime.Now.Ticks;
                    StartEndUp = DateTime.Now.AddSeconds(120).Ticks;
                }



                if (string.IsNullOrEmpty(data)) return null;
                if (Shielding && DateTime.Now.Ticks > StartUp && DateTime.Now.Ticks < StartEndUp)
                {
                    if (LstCmds != null)
                    {
                        foreach (var g in LstCmds)
                        {
                            if (data.Contains(g)) return null;
                        }
                    }
                }

                var tmps = MsgWithMobile.Deserialize(System.Convert.FromBase64String(data));
                if (tmps != null && tmps.Head != null && !string.IsNullOrEmpty(tmps.Head.Cmd))
                {
                    if (tmps.Head.Rcv != 2)
                        ThreadPoolLogic.RunDispatch(session, tmps); //处理数据
                    return new GuidWithCmd()
                               {
                                   Cmd = tmps.Head.Cmd,
                                   Guid = tmps.Head.Gid,
                                   RcvDataAndDoNotSndAgain = tmps.Head.Rcv
                               };
                }



            }
            catch (Exception ex)
            {
                Common.WriteError.WriteLogError("收到数据，当将数据解析为底层协议数据时出现异常:" + ex);
            }
            return null;
        }

    }



    /// <summary>
    /// 发送协议的关键性信息
    /// </summary>
    public class GuidWithCmd
    {
        /// <summary>
        /// 对方返回值的Guid
        /// </summary>
        public long Guid;

        /// <summary>
        /// 协议头信息
        /// </summary>
        public string Cmd;

        /// <summary>
        /// 如果中间层返回数据值为2 则表示中间层收到客户端数据 要求客户端放弃再次下发
        /// </summary>
        public int RcvDataAndDoNotSndAgain;
    }
}
