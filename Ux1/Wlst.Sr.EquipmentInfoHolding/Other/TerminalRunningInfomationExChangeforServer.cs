using System.Collections.Generic;

namespace Wlst.Sr.EquipmentInfoHolding.Other
{
    public class TerminalRunningInfomationExChangeforServer
    {
        /// <summary>
        /// 本地发送到服务器则为所有终端的最新更新时间列表；
        /// 服务器传回到本地则为本地需要更新的数据， 
        /// 包括程序运行数据以及数据库数据
        /// </summary>
        public List<TerminalRunningInfomation> LstInfo;

        /// <summary>
        /// 
        /// </summary>
        public TerminalRunningInfomationExChangeforServer()
        {
            LstInfo = new List<TerminalRunningInfomation>();
        }
    }
}