using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Wlst.Cr.Core.WindowsShowHelper
{
    /// <summary>
    /// 控制窗口显示的 基类函数
    /// </summary>
    public class ControlWinBase
    {
        protected Window View = null;


        protected  static Dictionary<long, WeakReference> Weak = new Dictionary<long, WeakReference>();

        private void _view_OnUserClosing(object sender, EventArgs e)
        {
            var weakr = new WeakReference(View);
            Weak.Add(DateTime.Now.Ticks, weakr);

            View.Closing -= _view_OnUserClosing;

            var sp = View.DataContext as Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged;
            if (sp != null)
            {
                sp.UnsubscribeEvent();
            }
            //else
            //{
            //    var sps = View.DataContext as Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelper;
            //    if (sps != null) sps.UnsubscribeEvent();
            //}

            OnUserClosingIng();
            View = null;

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnUserClosingIng()
        {

        }


        private List<int> _para = new List<int>();

        /// <summary>
        /// 显示界面调用的 显示函数  参数最终传递到  IINavOnWindow 接口中 如果不需要传递参数 则空即可
        /// </summary>
        /// <param name="para"></param>
        public void ShowView(List<int> para)
        {
            _para.Clear();
            if (para != null)
                _para.AddRange(para);
            if (View == null)
            {
                Initializzze();
            }
            else
            {
                View.Dispatcher.BeginInvoke((Action)delegate()
                {
                    if (View.WindowState == WindowState.Minimized)
                    {
                        View.WindowState = WindowState.Normal;
                    }
                });
            }
        }

        private void Initializzze()
        {
            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
            Weak.Add(DateTime.Now.Ticks, new WeakReference(newWindowThread));


            var ntg = (from t in Weak where t.Value.Target == null select t.Key).ToList();
            foreach (var f in ntg)
            {
                if (Weak.ContainsKey(f)) Weak.Remove(f);
            }

        }

        /// <summary>
        /// 必须实现的  获取窗口的函数
        /// </summary>
        /// <returns></returns>
        public virtual Window GetInstansView()
        {
            return null;
        }

        private void ThreadStartingPoint()
        {

            View = GetInstansView();
            if (View == null) return;
            View.Closing += _view_OnUserClosing;
            var iinav = View.DataContext as Wlst.Cr.Core.CoreInterface.IINavOnLoad;
            if (iinav != null)
            {
                try
                {
                    iinav.NavOnLoad(_para);
                }
                catch (Exception ex)
                {
                    Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("线程显示windows界面执行NavOnWindow时出错:" + ex);
                }
            }

            View.Show();
            View.Focus();
            System.Windows.Threading.Dispatcher.Run();
        }
    }
}
