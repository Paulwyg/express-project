using System;

namespace Wlst.Cr.PPProtocolSvrCnt.Server.Logic
{


    internal class PoolData
    {
        /// <summary>
        /// 处理函数
        /// </summary>
        public object ActionObj;

        /// <summary>
        /// 处理函数中协议数据类型
        /// </summary>
        public Type DataType;

        /// <summary>
        /// 识别关键字
        /// </summary>
        public string Cmd;

        /// <summary>
        /// 处理函数的名称
        /// </summary>
        public string ActionName;

        /// <summary>
        /// 处理函数所在的类的类类型
        /// </summary>
        public Type ClassTypeOfActionIn;

        /// <summary>
        /// 是否在Ui线程运行
        /// </summary>
        public bool IsRunInUiThread;

        /// <summary>
        /// 处理函数所在类的类实例
        /// </summary>
        public object InstancesClassOfActionIn;
    }
}
