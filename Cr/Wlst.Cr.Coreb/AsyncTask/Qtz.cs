using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using Wlst.Cr.Coreb.Servers;

namespace Wlst.Cr.Coreb.AsyncTask
{
    /// <summary>
    /// 定时或间隔调度
    /// </summary>
    public class Qtz
    {

        private static volatile Qtz _instance = null;
        private static readonly object LockHelper = new object();

        private static Qtz MySelf
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockHelper)
                    {
                        if (_instance == null)
                        {
                            _instance = new Qtz();
                            _instance.InitStart();
                        }
                    }
                }
                return _instance;
            }
        }


        private Qtz()
        {
        }

        protected void InitStart()
        {
            Threads.Add(new Thread(RunMx));
            Threads.Add(new Thread(RunMx)); //50
            // Threads.Add(new Thread(RunMx));
            Threads.Add(new Thread(RunDis0)); //50
            Threads.Add(new Thread(RunDis1)); //100
            Threads.Add(new Thread(RunDis1)); //100
            Threads.Add(new Thread(RunDis2)); //500
            Threads.Add(new Thread(RunDis2)); //500
            Threads.Add(new Thread(RunDis2)); //500
            Threads.Add(new Thread(RunDis3)); //1000
            Threads.Add(new Thread(RunDis3));
            Threads.Add(new Thread(RunDis3));
            Threads.Add(new Thread(RunDis3));
            Threads.Add(new Thread(RunDis3));
            Threads.Add(new Thread(RunDis3));
            Threads.Add(new Thread(RunDis3));
            Threads.Add(new Thread(RunDis4));
            foreach (var f in Threads) f.Start();
        }

        /// <summary>
        /// 添加调度函数
        /// </summary>
        /// <param name="portInf"></param>
        /// <param name="firstExecutetime">第一次执行时间</param>
        /// <param name="intervalSeconds">下一次执行的间隔时间 如果只执行一次可任意 秒</param>
        /// <param name="function">到达时间需要执行的函数</param>
        /// <param name="functionArgu">函数执行需要携带的参数 </param>
        /// <param name="repecttimes">需要执行的次数：0或-1为永久执行，大于0则表示具体的执行次数</param>
        /// <param name="funOnTaskOverExecuted"> 当任务执行结束时候的执行函数  即Repecttimes变为0的时候 通知用户结束了</param>
        /// <param name="canExecute">执行任务前的检测函数  当检测函数无法通过时 任务将自动终止执行，第一个参数为传入的functionArgu，第二参数为传入的taskName 如果返回为false则系统将该任务剔除 </param>
        /// <param name="taskName">任务名称 当系统删除时候可能会记录使用 </param>
        /// <param name="runInUithread">如果为客户端 是否在UI线程运行 </param>
        public static void AddQtz( string taskName,int portInf, long firstExecutetime, int intervalSeconds, Action<object> function,
                                  object functionArgu = null, int repecttimes = -1, Action<object> funOnTaskOverExecuted = null, Func<object, string, bool> canExecute = null,bool runInUithread=false )
        {
            MySelf.Items.Enqueue(new QtzItem(portInf, taskName, firstExecutetime, intervalSeconds * 10000000L, function,
                                             functionArgu,
                                             repecttimes,funOnTaskOverExecuted, canExecute,runInUithread));

            WriteLog.WriteDebug( "Qtz Add :" + taskName );
        }


        /// <summary>
        /// 添加调度函数
        /// </summary>
        /// <param name="portInf"></param>
        /// <param name="firstExecutetime">第一次执行时间</param>
        /// <param name="intervalmillisecondes">下一次执行的间隔时间 如果只执行一次可任意  毫秒 1000为1秒</param>
        /// <param name="function">到达时间需要执行的函数</param>
        /// <param name="functionArgu">函数执行需要携带的参数 </param>
        /// <param name="repecttimes">需要执行的次数：0或-1为永久执行，大于0则表示具体的执行次数</param>
        /// <param name="funOnTaskOverExecuted">当任务执行结束时候的执行函数  即Repecttimes变为0的时候 通知用户结束了 </param>
        /// <param name="canExecute">执行任务前的检测函数  当检测函数无法通过时 任务将自动终止执行，第一个参数为传入的functionArgu，第二参数为传入的taskName 如果返回为false则系统将该任务剔除  </param>
        /// <param name="taskName">任务名称 当系统删除时候可能会记录使用 </param>
        /// <param name="runInUithread">如果为客户端 是否在UI线程运行 </param>
        public static void AddQtz( string taskName,int portInf, long firstExecutetime, Action<object> function, long intervalmillisecondes,
                                  object functionArgu = null, int repecttimes = -1, Action<object> funOnTaskOverExecuted = null, Func<object, string, bool> canExecute = null, bool runInUithread = false)
        {
            MySelf.Items.Enqueue(new QtzItem(portInf, taskName, firstExecutetime, intervalmillisecondes * 10000, function,
                                             functionArgu,
                                             repecttimes,funOnTaskOverExecuted, canExecute,runInUithread));

            WriteLog.WriteDebug("Qtz Add :" + taskName);
        }

        ConcurrentQueue<QtzItem> Items = new ConcurrentQueue<QtzItem>();
         ConcurrentQueue<QtzItem> ItemsQtz = new ConcurrentQueue<QtzItem>();
         List<Thread> Threads = new List<Thread>();

        /// <summary>
        /// 主分配线程
        /// </summary>
        private void RunMx()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(50);
                    MainRun();
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError( "Qtz main Error:" + ex);
                }
            }
        }


        /// <summary>
        /// 处理现场0  间隔100
        /// </summary>
        private void RunDis0()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(50);
                    DisRun();
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError("Qtz RunDis Error:" + ex);
                }
            }
        }

        /// <summary>
        /// 处理现场1  间隔100
        /// </summary>
        private void RunDis1()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(100);
                    DisRun();
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError( "Qtz RunDis Error:" + ex);
                }
            }
        }

        /// <summary>
        /// 处理现场2  间隔500
        /// </summary>
        private void RunDis2()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(500);
                    DisRun();
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError( "Qtz RunDis Error:" + ex);
                }
            }
        }

        /// <summary>
        /// 处理现场3  间隔1000
        /// </summary>
        private void RunDis3()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(1000);
                    DisRun();
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError( "Qtz RunDis Error:" + ex);
                }
            }
        }
        
        /// <summary>
        /// 处理现场4  间隔100  并实现打印*
        /// </summary>
        private void RunDis4()
        {
            while (true)
            {
                try
                {
                    Console .Write("*");
                    Thread.Sleep(1000);
                    DisRun();
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError( "Qtz RunDis Error:" + ex);
                }
            }
        }

        /// <summary>
        /// 主分配线程 
        /// </summary>
        private void MainRun()
        {
            //遍历 列表
            if (Items.Count > 0)
            {
                //临时存放
                List<QtzItem> lst = new List<QtzItem>();
                QtzItem item;
                //遍历
                while (Items.TryDequeue(out item))
                {
                    //如果执行时间小于当前时间 即需要立即执行的任务
                    if (item.NextExecutetime < DateTime.Now.Ticks)
                    {
                        //添加到 执行队列里面
                        ItemsQtz.Enqueue(item);
                    }
                    else
                    {
                        //放入临时队列
                        lst.Add(item);
                    }
                }
                //将临时队列里面的任务 还原回 主队列里面
                foreach (var f in lst) Items.Enqueue(f);
            }
        }

        /// <summary>
        /// 任务执行
        /// </summary>
        private void DisRun()
        {
            if (ItemsQtz.Count > 0)
            {
                QtzItem item;
                //遍历 需要执行的任务队列
                while (ItemsQtz.TryDequeue(out item))
                {
                    //检测判断函数 如果无法通过判断函数 则删除任务
                    if (item.CanExecute != null)
                    {
                        bool canExecute = item.CanExecute(item.Argu, item.Name);
                        if (canExecute == false)
                        {
                            WriteLog.WriteDebug( 
                                "Qtz Stop Execute for Not get through FunContinueExecute,Task Name is  " + item.Name);
                            continue;
                        }
                    }

                    ////比较时间 检测执行时间是否合法 如果
                    //while (item.NextExecutetime < DateTime.Now.AddHours(-1).Ticks)
                    //{
                    //    //item.ExecuteTimes++; //执行次数
                    //    //如果设置了执行次数 则执行次数递减 直到0
                    //    if (item.Repecttimes > 0) item.Repecttimes -= 1;
                    //    if (item.Repecttimes != 0) //如果执行次数变为0  则该任务将失效 ，只有执行次数大于0 或为-1（永久执行的）才会自动更新时间
                    //    {
                    //        item.NextExecutetime = DateTime.Now.Ticks + item.IntervalMs; //变更下次执行时间
                    //    }
                    //}

                    //比较时间 是否应该执行
                    if (item.NextExecutetime < DateTime.Now.Ticks)
                    {
                        //如果执行次数为0了 则停止执行
                        if (item.Repecttimes == 0)
                        {
                            if (item.FunOnTaskOverExecuted != null) item.FunOnTaskOverExecuted(item.Argu);
                            WriteLog.WriteDebug( 
                                  "Qtz Stop Execute UNNORMAL for Repecttimes = 0,Task Name is " + item.Name);
                            continue;
                        }

                        item.ExecuteTimes++; //执行次数
                        try
                        {
                            // WriteSystemLog.WriteSystemLogInfo("Qtz  Executing,Task Name is  " + item.Name);
                            if (item.RunInUithread &&  Application.Current != null)
                            {

                                Application.Current.Dispatcher.BeginInvoke(new Action(
                                                                               () =>
                                                                                   {
                                                                                       item.FunExecuted(item.Argu);
                                                                                       //执行任务
                                                                                   }));
                            }
                            else item.FunExecuted(item.Argu); //执行任务
                        }
                        catch (Exception ex)
                        {
                            WriteLog.WriteDebug( 
                                "Qtz Execute Function error:" + ex);
                        }
                        //如果设置了执行次数 则执行次数递减 直到0
                        if (item.Repecttimes > 0) item.Repecttimes -= 1;
                        if (item.Repecttimes != 0) //如果执行次数变为0  则该任务将失效 ，只有执行次数大于0 或为-1（永久执行的）才会自动更新时间
                        {
                            var tmpNext = item.NextExecutetime + item.IntervalMs;
                            if (tmpNext > DateTime.Now.Ticks) item.NextExecutetime = tmpNext; //变更下次执行时间 使用NextExecutetime + item.IntervalMs
                            else
                                item.NextExecutetime = DateTime.Now.Ticks + item.IntervalMs; //变更下次执行时间  使用当前执行时间后间隔指定时间
                            Items.Enqueue(item); //执行完毕后 归入主队列
                        }
                        else
                        {
                            if (item.FunOnTaskOverExecuted != null) item.FunOnTaskOverExecuted(item.Argu);
                            WriteLog.WriteDebug( 
                              "Qtz Stop Execute normal for Repecttimes = 0,Task Name is " + item.Name);
                        }
                    }
                    else
                    {
                        //加入主队列
                        Items.Enqueue(item);
                    }
                    Thread.Sleep(2);
                }
            }
        }
    }

     class QtzItem
    {
        public int PortInf;
        public string Name;

        /// <summary>
        /// 执行间隔
        /// </summary>
        public long IntervalMs;

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public long NextExecutetime;

        public Action<object> FunExecuted;
         /// <summary>
        /// 当任务执行结束时候的执行函数  即Repecttimes变为0的时候 通知用户结束了
         /// </summary>
        public Action<object> FunOnTaskOverExecuted;

         /// <summary>
         /// 判断函数是否继续执行，默认执行
         /// </summary>
        public Func<object, string, bool> CanExecute;

        /// <summary>
        /// 已经执行次数
        /// </summary>
        public long ExecuteTimes;

        //internal bool IsLocked = false;

        /// <summary>
        /// -1、0 永久重复  其他为执行次数 最少一次 
        /// </summary>
        public long Repecttimes;

        /// <summary>
        /// 函数携带的参数
        /// </summary>
        public object Argu;


         /// <summary>
         /// 如果为客户端 是否在UI线程运行
         /// </summary>
         public bool RunInUithread;

         /// <summary>
         /// 
         /// </summary>
         /// <param name="portInf"></param>
         /// <param name="name">任务名称 暂时未启用</param>
         /// <param name="firstExecutetime">第一次执行时间</param>
         /// <param name="intervalmillisecondes">任务执行间隔时间 微妙  10000000为一秒</param>
         /// <param name="function">执行函数</param>
         /// <param name="functionArgu">执行参数</param>
         /// <param name="repecttimes">执行次数 不设置则永久执行 -1也为永久执行</param>
         /// <param name="funOnTaskOverExecuted">当任务执行结束时候的执行函数  即Repecttimes变为0的时候 通知用户结束了 </param>
         /// <param name="canExecute">此函数通过后才会执行 执行函数 对执行次数的一个补充，如果返回为false则系统将该任务剔除，系统将先判断此函数  </param>
         /// <param name="runInUithread">如果为客户端 是否在UI线程运行 </param>
         public QtzItem(int portInf, string name, long firstExecutetime, long intervalmillisecondes, Action<object> function,
                      object functionArgu, int repecttimes = -1,Action<object> funOnTaskOverExecuted=null, Func<object,string , bool> canExecute = null,bool runInUithread=false )
         {
             if (repecttimes == 0) repecttimes = -1;
             PortInf = portInf;
             Name = name;
             IntervalMs = intervalmillisecondes;
             NextExecutetime = firstExecutetime;
             FunExecuted = function;
             Repecttimes = repecttimes;
             Argu = functionArgu;
             CanExecute = canExecute;
             FunOnTaskOverExecuted = funOnTaskOverExecuted;
             RunInUithread = runInUithread;

             //比较时间 检测执行时间是否合法 如果
             while (NextExecutetime < DateTime.Now.Ticks)
             {
                 //item.ExecuteTimes++; //执行次数
                 //如果设置了执行次数 则执行次数递减 直到0
                 if (Repecttimes > 0) Repecttimes -= 1;
                 if (Repecttimes != 0) //如果执行次数变为0  则该任务将失效 ，只有执行次数大于0 或为-1（永久执行的）才会自动更新时间
                 {
                     NextExecutetime = NextExecutetime + IntervalMs; //变更下次执行时间
                 }
             }
         }
    }
}











