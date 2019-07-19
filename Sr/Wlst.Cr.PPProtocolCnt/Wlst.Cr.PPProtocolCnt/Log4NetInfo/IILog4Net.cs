namespace Wlst.Cr.PPProtocolSvrCnt.Log4NetInfo
{
    public interface IILog4Net
    {
        /// <summary>
        /// 记录程序运行的一般信息
        /// </summary>
        /// <param name="msg"></param>
        void WriteLogInfo(string msg);


        /// <summary>
        /// 记录程序的出错信息
        /// </summary>
        /// <param name="msg"></param>
        void WriteLogError(string msg);


        /// <summary>
        /// 记录程序的调试信息 如Exception信息等
        /// </summary>
        /// <param name="msg"></param>
        void WriteLogDebug(string msg);

    }
}
