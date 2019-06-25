using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wlst.Cr.CoreMims.Services
{
    public class KeepAliveSocket
    {
        /// <summary>
        /// 客户端向中间层发送数据的间隔时间 单位秒
        /// </summary>
        public int SndClientBeatTime = 30;


        public static KeepAliveSocket Myself
        {
            get
            {
                if (_mysqlf == null) new KeepAliveSocket();
                return _mysqlf;
            }
        }

        private static KeepAliveSocket _mysqlf;

        private KeepAliveSocket()
        {
            if (_mysqlf == null) _mysqlf = this;
        }

        private System.Timers.Timer _schedule;
        private bool _running = false;

        public void StartHeatBeat()
        {
            if (_running) return;
            _running = true;

            var path = Directory.GetCurrentDirectory() + "\\Config" + "\\" + "ping.txt";
            if (System.IO.File.Exists(path))
            {
                var file = Wlst.Cr.Coreb.Servers.WriteFile.ReadFiles(path);
                foreach (var f in file )
                {
                    var sps = f.Split('-');
                    if (sps.Length < 2) continue;
                    int x = 0;
                    if(Int32 .TryParse( sps [1],out x ))
                    {
                        if(x >0 && x<60)
                        {
                            SndClientBeatTime = x;
                            break;
                        }
                    }
                }
            }

            _schedule = new System.Timers.Timer(SndClientBeatTime*1000); //实例化Timer类，设置间隔时间为1000毫秒；
            _schedule.Elapsed += new System.Timers.ElapsedEventHandler(Schedule); //到达时间的时候执行事件；
            _schedule.AutoReset = true; //设置是执行一次（false）还是一直执行(true)；
            _schedule.Enabled = true; //是否执行System.Timers.Timer.Elapsed事件；
            _schedule.Start();


            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxSys  .wst_heart_beating  ,//.ClientPart.wlst_server_ans_clinet_heat_beating,
                action,
                typeof (KeepAliveSocket), this);
            DtClientHeatBeatingLastTime = new DateTime(1999, 1, 1);
        }

        /// <summary>
        /// 客户端收到服务段心跳包应答
        /// </summary>
        public event EventHandler ClientHeatBeating;

        public DateTime DtClientHeatBeatingLastTime;

        private void action(string session, Wlst .mobile .MsgWithMobile  infos)
        {
            DtClientHeatBeatingLastTime = DateTime.Now;

            if (ClientHeatBeating != null) ClientHeatBeating(null, EventArgs.Empty);

        }

        private void Schedule(object source, System.Timers.ElapsedEventArgs e)
        {
            var xxxinfo = Wlst.Sr.ProtocolPhone .LxSys  .wst_heart_beating  ;//.ServerPart.wlst_clinet_heat_beating;
            SndOrderServer.OrderSnd(xxxinfo);
        }
    }
}
