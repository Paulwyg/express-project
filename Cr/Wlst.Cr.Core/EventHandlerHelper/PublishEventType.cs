 

namespace Wlst.Cr.Core.EventHandlerHelper
{
    /// <summary>
    /// 事件发布 Core预定义的几个字段 使用者可自定义字段
    /// </summary>
    public class PublishEventType
    {
        /// <summary>
        /// Core.CoreServices.PublishEventArgs.None
        /// </summary>
        public const string None = "Core.CoreServices.PublishEventArgs.None";

        /// <summary>
        /// 程序内部发布事件 Core.CoreServices.PublishEventArgs.Core
        /// 参数未知 
        /// </summary>
        public const string Core = "Core.CoreServices.PublishEventArgs.Core";

        /// <summary>
        /// 程序内部发布时间  Core.CoreServices.PublishEventArgs.Sevr
        /// </summary>
        public const string Sevr = "Core.CoreServices.PublishEventArgs.Sevr";

        /// <summary>
        /// 系统断线后重新连接到服务器后，发布成功连接事件；可能连接断开已经很久了，系统保留的缓存数据需要重新请求;
        /// 事件发布Id默认 100  Core.CoreServices.PublishEventArgs.ReCn
        /// </summary>
        public const string ReCn = "Core.CoreServices.PublishEventArgs.ReCn";


        /// <summary>
        /// 系统主界面激活 事件发布Id默认 101  Core.CoreServices.SystemMainViewActive
        /// </summary>
        public const string SvAv = "Core.CoreServices.SystemMainViewActive";




    }
}
