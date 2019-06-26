using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Wlst.Cr.Core.ComponentHold;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Cr.Core.CoreServices
{
    /// <summary>
    /// 管理Prism的所有Region区域的管理 ，页面的获取与导航
    /// </summary>
    public class RegionManage
    {

        ///// <summary>
        ///// 手动设置屏蔽自动加载页面
        ///// </summary>
        //public static void SetLoadInner()
        //{
        //    Wlst.Cr.Core.Behavior.AutoPopulateExportedViewsBehavior.SetLoadInner();// = true;
        //}
        ///// <summary>
        ///// 手动加载页面
        ///// </summary>
        //public static void RunningLoadViewToWindows()
        //{
        //    Wlst.Cr.Core.Behavior.AutoPopulateExportedViewsBehavior.RunningLoadViewToWindows() ;
        //}

        private    delegate void  CmbdelegateMbl(object  data);

        /// <summary>
        /// 
        /// </summary>
        public static void DispatcherInvoke(Action<object> ac, object  data)
        {
            Application.Current.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.DataBind,
                new CmbdelegateMbl(ac), data);
        }

        /// <summary>
        /// 主显示区域的界面是否 在新窗口中显示
        /// </summary>
        public static bool IsNewViewInDocumentRegionPopup = false    ;
        /// <summary>
        /// 弹出窗口使用默认的系统窗口
        /// </summary>
        public static bool IsNewViewInDocumentRegionPopupUseDefaultWin = true;
        private static IRegionManager _regionManager;

        /// <summary>
        /// 获取IRegionManager实例
        /// </summary>
        public static IRegionManager RegionManagerInstances
        {
            get
            {
                if (_regionManager == null)
                {
                    _regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
                }
                return _regionManager;
            }

        }

        #region 主界面

        private static bool _issysyteacti = false;
        private static bool _isfirstset = true;

        /// <summary>
        /// 主界面是否已经激活  指主界面是否已经按照设计者要求已经显示 login界面手动赋值
        /// </summary>
        public static bool IsSystemMainViewActive
        {
            get { return _issysyteacti; }
            internal set
            {
                _issysyteacti = value;

                if (value && _isfirstset)
                {
                    _issysyteacti = false;
                    EventPublish.PublishEvent(new PublishEventArgs()
                                                    {
                                                        EventType = PublishEventType.SvAv,
                                                        EventId =101
                                                    });
                }
            }
        }
       
        #endregion

        #region NavToViewWithNoArgu

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tuple"></param>
        private static void DoTaskA(Tuple<string,  bool> tuple)
        {
            // int viewId, string viewAttachRegionName, bool show


            try
            {

                var attachRegion = ViewComponentHolding.GetViewAttachRegionById(tuple.Item1);

                if (attachRegion.Equals("DocumentRegion") && IsNewViewInDocumentRegionPopup)
                {
                    RegionManageExtend.DoTaskA(tuple);
                    return;
                }

                if (!RegionManagerInstances.Regions.ContainsRegionWithName(attachRegion))
                {
                    WriteLog.WriteLogInfo(
                        "Core ShowViewByIdAttachRegion Can not find Region :" + attachRegion);
                    return;
                }
                var view = ViewComponentHolding.GetViewById(tuple.Item1);
                if (view == null)
                {
                    WriteLog.WriteLogInfo(
                        "Core ShowViewByIdAttachRegion Can not find View IN ViewComponentHolding By id :" + tuple.Item1);
                    return;
                }


                if (tuple.Item2 )
                {
                    if (!RegionManagerInstances.Regions[attachRegion].Views.Contains(view))
                    {

                        try
                        {
                            var userControlMvvm = view as ContentControl;
                            if (userControlMvvm != null)
                            {
                                var mvvm = userControlMvvm.DataContext as IINavInitBeforShow;
                                //导航页面必须实现此接口方可使用此函数进行页面导航数据初始化，否则需要自设定导航初始化
                                if (mvvm != null)
                                {
                                   // mvvm.NavInitBeforShow();

                                    var dt = DateTime.Now.Ticks;
                                    mvvm.NavInitBeforShow();
                                    var ds = DateTime.Now.Ticks - dt;
                                    RegionManageExtend.CalInitTime(tuple.Item1, ds);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog.WriteLogInfo(
                              "Core ShowViewByIdAttachRegion NavInitBeforShow Error IN ViewComponentHolding By id :" + tuple.Item1);
                        }

                        RegionManagerInstances.Regions[attachRegion].
                            Add(view);

                    }
                    RegionManagerInstances.Regions[attachRegion].Activate(view);
                }
                else
                {
                    if (RegionManagerInstances.Regions[attachRegion].Views.Contains(view))
                    {
                        RegionManagerInstances.Regions[attachRegion].Remove(view);
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Core ShowViewByIdAttachRegion Error:" + ex);
            }
        }


        //定义委托
        private delegate void DoTask(Tuple<string, bool> tuple);


        /// <summary>
        /// Show View By ViewId and ViewAttachRegion
        /// </summary>
        /// <param name="show">是显示页面 或 关闭页面</param>
        /// <param name="viewId">页面Id值 </param>
        public static void ShowViewByIdAttachRegion(int viewId,  bool show)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal, new DoTask(DoTaskA),
                    new Tuple<string, bool>(viewId + "", show));
            }
            catch (Exception e)
            {
                WriteLog.WriteLogError("Core ShowViewByIdAttachRegion Error in Invoke :" + e);
            }
        }

        #endregion


        #region NavWithArgu

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argu"></param>
        private static void DoTaskB(params object[] argu)
        {
            // int viewId, string viewAttachRegionName, bool show


            try
            {
                if (argu.Length < 2) return;
                if (argu[0] == null) return;

                string viewId = argu[0].ToString();
                string viewAttachRegion = ViewComponentHolding.GetViewAttachRegionById(viewId);
             //   string viewAttachRegion = argu[1].ToString();
                object[] arus = new object[argu.Length - 1];
                for (int i = 1; i < argu.Length; i++)
                {
                    arus[i - 1] = argu[i];
                }

                if (viewAttachRegion.Equals("DocumentRegion") && IsNewViewInDocumentRegionPopup)
                {
                    RegionManageExtend .DoTaskB(argu );
                    return;
                }

                if (!RegionManagerInstances.Regions.ContainsRegionWithName(viewAttachRegion))
                {
                    WriteLog.WriteLogInfo(
                        "Core ShowViewByIdAttachRegion Can not find Region :" + viewAttachRegion);
                    return;
                }
                var view = ViewComponentHolding.GetViewById(viewId);
                if (view == null)
                {
                    WriteLog.WriteLogInfo(
                        "Core ShowViewByIdAttachRegion Can not find View IN ViewComponentHolding By id :" + viewId);
                    return;
                }


                try
                {
                    var userControlMvvm = view as ContentControl;
                    if (userControlMvvm != null)
                    {
                        var mvvm = userControlMvvm.DataContext as IINavInitBeforShow;
                            //导航页面必须实现此接口方可使用此函数进行页面导航数据初始化，否则需要自设定导航初始化
                        if (mvvm != null)
                        {
                            //mvvm.NavInitBeforShow(arus);

                            var dt = DateTime.Now.Ticks;
                            mvvm.NavInitBeforShow(arus);
                            var ds = DateTime.Now.Ticks - dt;
                            RegionManageExtend.CalInitTime(viewId, ds);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogInfo(
                      "Core ShowViewByIdAttachRegion NavInitBeforShow Error IN ViewComponentHolding By id :" + viewId);
                }

                if (!RegionManagerInstances.Regions[viewAttachRegion].Views.Contains(view))
                {
                    RegionManagerInstances.Regions[viewAttachRegion].Add(view);
                }
                RegionManagerInstances.Regions[viewAttachRegion].Activate(view);


                //bool bolNavOnLoad = false;
                //var userControlMvvm = view as ContentControl;
                //if (userControlMvvm != null)
                //{
                //    var mvvm = userControlMvvm.DataContext as IINavOnLoad; //导航页面必须实现此接口方可使用此函数进行页面导航数据初始化，否则需要自设定导航初始化
                //    if (mvvm != null)
                //    {
                //        mvvm.NavOnLoad(arus);
                //        bolNavOnLoad = true;
                //    }
                //}

                //if (!bolNavOnLoad)
                //{
                //    WriteLog.WriteLogInfo("ShowViewByIdAttachRegion  viewId :" + viewId +
                //                          " Can not find Mvvm and can not be  NavOnLoaded");
                //}


                Task t = new Task(() =>
                                      {
                                          Thread.Sleep(500);
                                          try
                                          {
                                              Application.Current.Dispatcher.Invoke(
                                                  System.Windows.Threading.DispatcherPriority.Normal,
                                                  new DoTaskFuck(Fucksssss),
                                                  new Tuple<string, object, object[]>(viewId, view, arus));
                                          }
                                          catch (Exception e)
                                          {
                                              WriteLog.WriteLogError(
                                                  "Core ShowViewByIdAttachRegion Error in Invoke Task:" + e);
                                          }


                                      });
                t.Start();

            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Core ShowViewByIdAttachRegion Error:" + ex);
            }
        }

        private delegate void DoTaskFuck(object view);

        private static void Fucksssss(object abc)
        {
            try
            {
                var args = abc as Tuple<string , object, object[]>;
                if (args == null) return;
                bool bolNavOnLoad = false;
                var userControlMvvm = args.Item2 as ContentControl;
                if (userControlMvvm != null)
                {
                    var mvvm = userControlMvvm.DataContext as IINavOnLoad; //导航页面必须实现此接口方可使用此函数进行页面导航数据初始化，否则需要自设定导航初始化
                    if (mvvm != null)
                    {


                        var dt = DateTime.Now.Ticks;
                        mvvm.NavOnLoad(args.Item3);
                        var ds = DateTime.Now.Ticks - dt;
                        RegionManageExtend.CalInitTime(args .Item1 , ds);

                     //   mvvm.NavOnLoad(args.Item3);
                        bolNavOnLoad = true;
                    }
                }

                if (!bolNavOnLoad)
                {
                    WriteLog.WriteLogInfo("ShowViewByIdAttachRegion  Can not  be  NavOnLoaded,viewid is " + args.Item1);
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Core ShowViewByIdAttachRegion Error while NavOnLoad:" + ex);
            }

        }

        //定义委托
        private delegate void DoTaskTwo(params object[] argu);


        /// <summary>
        /// Show View By ViewId and ViewAttachRegion
        /// </summary>
        /// <param name="argu">参数</param>
        /// <param name="viewId">页面Id值 </param>
        public static void ShowViewByIdAttachRegionWithArgu(int viewId,   params object[] argu)
        {
            try
            {
                object[] arg = new object[argu.Length + 1];
                arg[0] = viewId+"";
                for (int i = 0; i < argu.Length; i++)
                {
                    arg[1 + i] = argu[i];
                }
                Application.Current.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal, new DoTaskTwo(DoTaskB), arg);
            }
            catch (Exception e)
            {
                WriteLog.WriteLogError("Core ShowViewByIdAttachRegion Error in Invoke :" + e);
            }
        }


        /// <summary>
        /// Show View By ViewId and ViewAttachRegion
        /// </summary>
        /// <param name="argu">参数</param>
        /// <param name="viewType">页面类型 </param>
        public static void ShowViewByIdAttachRegionWithArgu(Type viewType,   params object[] argu)
        {
            try
            {
                object[] arg = new object[argu.Length + 1];
                arg[0] = viewType .GUID  + "";
                for (int i = 0; i < argu.Length; i++)
                {
                    arg[1 + i] = argu[i];
                }
                Application.Current.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal, new DoTaskTwo(DoTaskB), arg);
            }
            catch (Exception e)
            {
                WriteLog.WriteLogError("Core ShowViewByIdAttachRegion Error in Invoke :" + e);
            }
        }
        #endregion


        /// <summary>
        /// 通过页面Id获取页面实例; 页面必须继承自 ContentControl ；如果页面需要执行初始化则必须实现IINavOnLoad 接口 NavOnLoad方法实现参数初始化 ；
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="args"></param>
        /// <returns>无法查阅则返回 null</returns>
        public static object GetViewById(int viewId, params object[] args)
        {
            var obs = ViewComponentHolding.GetViewById(viewId + "");

            if (obs != null)
            {
                //View = obs; 
                var userControlMvvm = obs as ContentControl; // UserControl; ContentControl
                if (userControlMvvm != null)
                {
                    var mvvm = userControlMvvm.DataContext as IINavOnLoad; //导航页面必须实现此接口方可使用此函数进行页面导航数据初始化，否则需要自设定导航初始化
                    if (mvvm != null)
                    {
                        mvvm.NavOnLoad(args);
                    }

                }
            }
            return obs;
        }

        /// <summary>
        /// 通过页面Id获取页面实例; 页面必须继承自 ContentControl ；如果页面需要执行初始化则必须实现IINavOnLoad 接口 NavOnLoad方法实现参数初始化 ；
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="args"></param>
        /// <returns>无法查阅则返回 null</returns>
        public static object GetViewByIdDisConnectFather(int viewId, params object[] args)
        {
            var info = GetViewById(viewId, args);
            var userControl = info as ContentControl; // UserControl;
            if (userControl != null)
            {
                var parent = userControl.Parent;
                if (parent != null)
                {
                    parent.SetValue(ContentPresenter.ContentProperty, null);
                }
            }
            return info;
        }

        /// <summary>
        /// 指定Id页面是否已经显示于指定的区域
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public static bool RegionWetherHasThisIdView(int viewId, string regionName)
        {
            if (!RegionManagerInstances.Regions.ContainsRegionWithName(regionName))
            {
                return false;
            }

            var view = ViewComponentHolding.GetViewById(viewId + "");
            if (view == null)
            {
                return false;
            }
            if (RegionManagerInstances.Regions[regionName].Views.Contains(view))
            {
                return true;
            }
            return false;
        }
    }
}
