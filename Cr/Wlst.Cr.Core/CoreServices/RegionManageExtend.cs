using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Wlst.Cr.Core.ComponentHold;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.UtilityFunction;

namespace Wlst.Cr.Core.CoreServices
{
    internal class RegionManageExtend
    {
        //module id  1
        private static Dictionary<int, Window> item = new Dictionary<int, Window>();
        private static Dictionary<int, string> setinfo = new Dictionary<int, string>();
        //  private static ConcurrentQueue<Window> viewitems = new ConcurrentQueue<Window>();



        internal static void AddAndShowView(string title, int id, object contend)
        {
            if (setinfo.Count == 0)
            {
                UtilityFunction.OptionXmlSvr.LoadSet(100);
                if (UtilityFunction.OptionXmlSvr.Data.ContainsKey(100))
                {
                    foreach (var f in UtilityFunction.OptionXmlSvr.Data[100])
                    {

                        setinfo.Add(f.Key, f.Value);
                    }
                }
            }


            Window win = null;
            if (RegionManage.IsNewViewInDocumentRegionPopupUseDefaultWin)
            {
                Window winx = new Window();
                winx.Title = title;
                //win.ToolTip = title;

                win = winx;
            }
            else
            {
                WindowForWlst.CustomChromeWindow winx = new WindowForWlst.CustomChromeWindow();
                winx.Title = title;
                //win.ToolTip = title;
                winx.TitleCetc = title;
                win = winx;

            }
            //WindowForWlst.CustomChromeWindow win = new WindowForWlst.CustomChromeWindow();
            //win.Title = title;
            ////win.ToolTip = title;
            //win.TitleCetc = title;
            win.ResizeMode = ResizeMode.CanResize;


            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //win.Left = UtilityFunction.OptionXmlSvr.GetOptionInt(100, id*10 + 1, 100);
            //win.Top = UtilityFunction.OptionXmlSvr.GetOptionInt(100, id*10 + 2, 100);
            win.Height = UtilityFunction.OptionXmlSvr.GetOptionInt(100, id*10 + 3, 700);
            win.Width = UtilityFunction.OptionXmlSvr.GetOptionInt(100, id*10 + 4, 950);

            win.Tag = id;
            win.Closing += new System.ComponentModel.CancelEventHandler(win_Closing);
            win.SizeChanged += new SizeChangedEventHandler(win_SizeChanged);
            win.LocationChanged += new EventHandler(win_LocationChanged);
            win.Content = contend;
            win.Show();
            if (item.ContainsKey(id) == false) item.Add(id, win);

            //viewitems.Enqueue(win);
            //InitS();
        }


        //  static void InitS( )
        //{


        //    Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
        //    newWindowThread.SetApartmentState(ApartmentState.STA);
        //    newWindowThread.IsBackground = true;
        //    newWindowThread.Start();
        //}


        //  private static void ThreadStartingPoint()
        //  {
        //      Window de = null;
        //      if (viewitems.Count > 0)
        //      {
        //          if (viewitems.TryDequeue(out de) && de != null)
        //          {
        //              de.Show();
        //              System.Windows.Threading.Dispatcher.Run();
        //          }
        //      }
        //  }

        /// <summary>
        /// viewid *10 + 1 left,+2 right
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void win_LocationChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            var win = sender as Window;
            if (win == null || win.Tag == null) return;

            int id = 0;
            Int32.TryParse(win.Tag.ToString(), out id);
            if (id == 0) return;

            int left = (int) win.Left;
            int top = (int) win.Top;

            if (setinfo.ContainsKey(id*10 + 1)) setinfo[id*10 + 1] = left + "";
            else setinfo.Add(id*10 + 1, left + "");

            if (setinfo.ContainsKey(id*10 + 2)) setinfo[id*10 + 2] = top + "";
            else setinfo.Add(id*10 + 2, left + "");
            //UtilityFunction.OptionXmlSvr.UpdateInfo(100, id*10 + 1, left+"",top +"");
            // UtilityFunction.OptionXmlSvr.UpdateInfo(100, id * 10 + 2, top + "");
            UtilityFunction.OptionXmlSvr.SaveXml(100, setinfo);
        }

        /// <summary>
        /// viewid*10+3 : height  viewid*10+4 : winth 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void win_SizeChanged(object sender, SizeChangedEventArgs e)
        {


            var win = sender as Window;
            if (win == null || win.Tag == null) return;

            int id = 0;
            Int32.TryParse(win.Tag.ToString(), out id);
            if (id == 0) return;

            int left = (int) win.Height;
            int top = (int) win.Width;


            if (setinfo.ContainsKey(id*10 + 3)) setinfo[id*10 + 3] = left + "";
            else setinfo.Add(id*10 + 3, left + "");

            if (setinfo.ContainsKey(id*10 + 4)) setinfo[id*10 + 4] = top + "";
            else setinfo.Add(id*10 + 4, left + "");

            UtilityFunction.OptionXmlSvr.SaveXml(100, setinfo);
        }



        private static void win_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var dlt = sender as Window;
            if (dlt == null) return;
            var userControlMvvm = dlt.Content as ContentControl;
            if (userControlMvvm == null) return;


            var mvvm = userControlMvvm.DataContext as IIOnHideOrClose;
            //导航页面必须实现此接口方可使用此函数进行页面导航数据初始化，否则需要自设定导航初始化
            if (mvvm != null)
            {
                mvvm.OnUserHideOrClosing();
            }
            e.Cancel = true;
            dlt.Hide();
            //throw new NotImplementedException();
        }

        #region NavToViewWithNoArgu



        internal static void DoTaskA(Tuple<string, bool> tuple)
        {
            string viewId = tuple.Item1;
            bool show = tuple.Item2;
            int viewIdx = Convert.ToInt32(viewId);

            try
            {

                var attachRegion = ViewComponentHolding.GetViewAttachRegionById(viewId);


                var view = ViewComponentHolding.GetViewById(viewId);
                if (view == null)
                {
                    WriteLog.WriteLogInfo(
                        "Core ShowViewByIdAttachRegion Can not find View IN ViewComponentHolding By id :" + viewId);
                    return;
                }


                if (show)
                {
                    if (item.ContainsKey(viewIdx))
                    {

                        item[viewIdx].Show();
                        item[viewIdx].Focus();
                        item[viewIdx].WindowState = WindowState.Normal;
                        return;
                    }

                    try
                    {
                        var userControlMvvm = view as ContentControl;
                        string title = string.Empty;
                        if (userControlMvvm != null)
                        {
                            userControlMvvm.HorizontalAlignment = HorizontalAlignment.Center;
                            userControlMvvm.VerticalAlignment = VerticalAlignment.Center;
                            //userControlMvvm.HorizontalContentAlignment = HorizontalAlignment.Center;
                            //userControlMvvm.VerticalContentAlignment  = VerticalAlignment.Center;
                            var mvvm = userControlMvvm.DataContext as IINavInitBeforShow;
                            //导航页面必须实现此接口方可使用此函数进行页面导航数据初始化，否则需要自设定导航初始化
                            if (mvvm != null)
                            {

                                var dt = DateTime.Now.Ticks;
                                mvvm.NavInitBeforShow();
                                var ds = DateTime.Now.Ticks - dt;
                                CalInitTime(viewIdx, ds);

                                // mvvm.NavInitBeforShow();
                            }

                            var mv = userControlMvvm.DataContext as IITab;
                            if (mv != null) title = mv.Title;
                        }
                        AddAndShowView(title, viewIdx, userControlMvvm);

                        //Window win = new Window();
                        //win.Title = title;
                        //win.Closing += new System.ComponentModel.CancelEventHandler(win_Closing);

                        //win.Content = userControlMvvm;
                        //win.Show();
                        //if (item.ContainsKey(viewIdx) == false) item.Add(viewIdx, win);
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLogInfo(
                            "Core ShowViewByIdAttachRegion NavInitBeforShow Error IN ViewComponentHolding By id :" +
                            view);
                    }


                }
                //else
                //{
                //    if (RegionManagerInstances.Regions[attachRegion].Views.Contains(view))
                //    {
                //        RegionManagerInstances.Regions[attachRegion].Remove(view);
                //    }
                //}

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
        public static void ShowViewByIdAttachRegion(int viewId, bool show)
        {
            try
            {
                DoTaskA(new Tuple<string, bool>(viewId + "", show));
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
        internal static void DoTaskB(params object[] argu)
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
                            //  mvvm.NavInitBeforShow(arus);

                            var dt = DateTime.Now.Ticks;
                            mvvm.NavInitBeforShow(arus);
                            var ds = DateTime.Now.Ticks - dt;
                            CalInitTime(viewId, ds);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogInfo(
                        "Core ShowViewByIdAttachRegion NavInitBeforShow Error IN ViewComponentHolding By id :" + viewId);
                }

                int vid = Convert.ToInt32(viewId);
                if (item.ContainsKey(vid))
                {

                    item[vid].Show();
                    item[vid].Focus();
                    item[vid].WindowState = WindowState.Normal;


                    var userControlMvvm = item[vid].Content as ContentControl;
                    if (userControlMvvm != null)
                    {
                        var mvvm = userControlMvvm.DataContext as IINavOnLoad;
                        //导航页面必须实现此接口方可使用此函数进行页面导航数据初始化，否则需要自设定导航初始化
                        if (mvvm != null)
                        {
                            //mvvm.NavOnLoad(arus);

                            var dt = DateTime.Now.Ticks;

                            mvvm.NavOnLoad(arus);
                            var ds = DateTime.Now.Ticks - dt;
                            CalInitTime(vid, ds);
                        }

                    }
                    return;
                }



                try
                {

                    bool bolNavOnLoad = false;
                    var userControlMvvm = view as ContentControl;
                    string title = string.Empty;
                    if (userControlMvvm != null)
                    {

                        var mvvm = userControlMvvm.DataContext as IINavOnLoad;
                        //导航页面必须实现此接口方可使用此函数进行页面导航数据初始化，否则需要自设定导航初始化
                        if (mvvm != null)
                        {
                            var dt = DateTime.Now.Ticks;

                            mvvm.NavOnLoad(arus);
                            bolNavOnLoad = true;
                            var ds = DateTime.Now.Ticks - dt;
                            CalInitTime(vid, ds);
                        }
                        var mv = userControlMvvm.DataContext as IITab;
                        if (mv != null) title = mv.Title;


                    }
                    AddAndShowView(title, vid, userControlMvvm);

                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError("Core ShowViewByIdAttachRegion Error while NavOnLoad:" + ex);
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Core ShowViewByIdAttachRegion Error:" + ex);
            }
        }



        /// <summary>
        /// Show View By ViewId and ViewAttachRegion
        /// </summary>
        /// <param name="argu">参数</param>
        /// <param name="viewId">页面Id值 </param>
        public static void ShowViewByIdAttachRegionWithArgu(int viewId, params object[] argu)
        {
            try
            {
                object[] arg = new object[argu.Length + 1];
                arg[0] = viewId + "";
                for (int i = 0; i < argu.Length; i++)
                {
                    arg[1 + i] = argu[i];
                }
                DoTaskB(arg);
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
        public static void ShowViewByIdAttachRegionWithArgu(Type viewType, params object[] argu)
        {
            try
            {
                object[] arg = new object[argu.Length + 1];
                arg[0] = viewType.GUID + "";
                for (int i = 0; i < argu.Length; i++)
                {
                    arg[1 + i] = argu[i];
                }
                DoTaskB(arg);

            }
            catch (Exception e)
            {
                WriteLog.WriteLogError("Core ShowViewByIdAttachRegion Error in Invoke :" + e);
            }
        }

        #endregion


        private static ConcurrentDictionary<int, long> _dic = new ConcurrentDictionary<int, long>();


        internal static void CalInitTime(string title, long time)
        {
            int vid = 0;
            if (Int32.TryParse(title, out vid))
            {
                CalInitTime(vid, time);
            }
        }

        internal static void CalInitTime(int title, long time)
        {
            if (_dic.ContainsKey(title)) _dic[title] += time;
            else _dic.TryAdd(title, time);

            if (_dic[title] > 10000000)
            {
                UtilityFunction.WriteLog.WriteLogError("NavonLoad ：" + title + "  - " + _dic[title]/1000000);
            }
        }
    }
}
