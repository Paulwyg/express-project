using System;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;


namespace Wlst.Cr.CoreOne.Services
{
    /// <summary>
    /// 系统显示信息
    /// </summary>
    [Serializable]
    public class LogInfo
    {
   
        private static moduleloadunloadshow wl = new moduleloadunloadshow();

        internal class moduleloadunloadshow : Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged
        {
            public moduleloadunloadshow ()
            {
                this.AddEventFilterInfo(Wlst.Cr.Core.ModuleServices.ModuleAssemblyEvent.ModuleLoadedEvent,
                                        PublishEventType.Core);
                this.AddEventFilterInfo(Wlst.Cr.Core.ModuleServices.ModuleAssemblyEvent.ModulePreLoadEvent,
                                      PublishEventType.Core);
                this.AddEventFilterInfo(Wlst.Cr.Core.ModuleServices.ModuleAssemblyEvent.ModulePreUnLoadEvent,
                                      PublishEventType.Core);
                this.AddEventFilterInfo(Wlst.Cr.Core.ModuleServices.ModuleAssemblyEvent.ModuleUnLoadedEvent,
                                      PublishEventType.Core);
            }

            public override void ExPublishedEvent(PublishEventArgs args)
            {
                try
                {
                    //base.ExPublishedEvent(args);
                    var info = args.GetParams()[0] as Wlst.Cr.Core.Modularity.AssemblyConfig;
                    if (info != null)
                    {
                        if (args.EventId == Wlst.Cr.Core.ModuleServices.ModuleAssemblyEvent.ModuleLoadedEvent)
                        {
                            var bol = (bool) args.GetParams()[1];
                            if (bol)
                                Log("程序核心加载模块成功，加载模块为:" + info.ModuleName);
                            else
                                Log("程序核心加载模块失败，加载模块为:" + info.ModuleName);
                        }
                        else if (args.EventId == Wlst.Cr.Core.ModuleServices.ModuleAssemblyEvent.ModuleUnLoadedEvent)
                        {
                            var bol = (bool) args.GetParams()[1];
                            if (bol)
                                Log("程序核心卸载模块成功，卸载模块为:" + info.ModuleName);
                            else
                                Log("程序核心卸载模块失败，卸载模块为:" + info.ModuleName);
                        }
                        else if (args.EventId == Wlst.Cr.Core.ModuleServices.ModuleAssemblyEvent.ModulePreLoadEvent)
                        {

                            Log("程序核心正在加载模块，加载模块为:" + info.ModuleName);

                        }
                        else if (args.EventId == Wlst.Cr.Core.ModuleServices.ModuleAssemblyEvent.ModulePreUnLoadEvent)
                        {

                            Log("程序核心正在卸载模块，卸载模块为:" + info.ModuleName);

                        }

                    }
                }
                catch (Exception ex)
                {

                }

            }
        }


        ///// <summary>
        ///// Replays the saved logs if the Callback has been set.
        ///// </summary>
        //public void ReplaySavedLogs()
        //{
        //    ILogInfo.ReplaySavedLogs();
        //}


        ///// <summary>
        ///// Write a new log entry with the specified category and priority.
        ///// </summary>
        ///// <param name="message">Message body to log.</param>
        ///// <param name="category">Category of the entry.</param>
        ///// <param name="priority">The priority of the entry.</param>
        //public static void Log(string message, Category category, Priority priority)
        //{
        //    ILogInfo.Log(message, category, priority);
        //}

        /// <summary>
        /// Write a new log entry 
        /// </summary>
        /// <param name="message">Message body to log.</param>
        public static void Log(string message)
        {
            Wlst .Cr .Core .UtilityFunction .WriteLog .WriteLogDebug(  message );
        }
    }

}
