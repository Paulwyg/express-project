using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Cr.CoreMims.ShowMsgInfo
{
    /// <summary>
    /// 显示最新信息
    /// </summary>
    public class ShowNewMsg
    {
        /// <summary>
        /// 注册显示信息  操作地址-终端名称-操作-执行情况
        /// </summary>
        public static Action<int, string, OperatrType, string> ActionAddShowInfo;

        private delegate void ShowIinfo(int y, string yy, OperatrType yyy, string yyyy);

        /// <summary>
        /// 新增加新的用户操作信息
        /// </summary>
        /// <param name="rtuId">操作的终端地址</param>
        /// <param name="rtuName">终端 </param>
        /// <param name="operatr">用户操作还是服务器应答 </param>
        /// <param name="operatorContent">执行情况 如 完成或 等待 </param>
        public static  void AddNewShowMsg(int rtuId, string rtuName, OperatrType operatr, string operatorContent)
        {
            if (ActionAddShowInfo == null) return;
            try
            {
                Application.Current.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal, new ShowIinfo(ActionAddShowInfo),
                    rtuId, rtuName, operatr, operatorContent);
            }
            catch (Exception ex)
            {

            }
        }

    }

    /// <summary>
    /// 100+ 用于终端    200+ 
    /// </summary>

    public enum OpType
    {
        [Description("1选测")] RtuMeasure = 101,
        [Description("2开灯")] RtuOpen,
        [Description("3关灯")] RtuClose,
        [Description("4发送13周设置")] Snd13Week,
        [Description("5发送节假日")] SndHoliday,
        [Description("6召测时钟")] ZcTime,
        [Description("7召测终端参数")] ZcRtuPara,
        [Description("8召测周设置")] ZcWeek,
        [Description("9召测节假日")] ZcHoliday,
        [Description("10对时")] AsynTime,
        SluMeasure =201,
        SluOpen,
        SluAdjust,
        SluClose,
        CtrlMeasure,
        [Description("发送46周设置")]
        Snd46Week,
        [Description("发送78周设置")]
        Snd78Week,
        [Description("4发送全年周设置")]
        SndAllYearWeek,
    }

    public enum OperatrType
    {
        UserOperator = 1,
        ServerReply,
        SystemInfo
    }
}
