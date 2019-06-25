//using System;
//using System.Collections.Generic;
//using System.Windows;

//namespace Wlst.Cr.CoreOne.LogInfo
//{

//    /// <summary>
//    /// A logger that holds on to log entries until a callback delegate is set, then plays back log entries and sends new log entries.
//    /// </summary>
//    public class LogInfo : IILogInfo
//    {
     
//        private readonly Queue<Tuple<string, Category, Priority>> savedLogs =
//            new Queue<Tuple<string, Category, Priority>>();

//        private Action<string, Category, Priority> callback;


//        //定义委托
//        public delegate void DoTask(string msg, Category cate, Priority pri);
//        /// <summary>
//        /// Gets or sets the callback to receive logs.
//        /// </summary>
//        /// <value>An Action&lt;string, Category, Priority&gt; callback.</value>
//        public Action<string, Category, Priority> Callback
//        {
//            get { return this.callback; }
//            set { this.callback = value; }
//        }

//        /// <summary>
//        /// Write a new log entry with the specified category and priority.
//        /// </summary>
//        /// <param name="message">Message body to log.</param>
//        /// <param name="category">Category of the entry.</param>
//        /// <param name="priority">The priority of the entry.</param>
//        public void Log(string message, Category category, Priority priority)
//        {
//            if (this.Callback != null)
//            {
//                //this.Callback(message, category, priority);
//                Application.Current.Dispatcher.Invoke(
//                    System.Windows.Threading.DispatcherPriority.Normal, new DoTask(Callback), message, category,
//                    priority);
//            }
//            else
//            {
//                this.savedLogs.Enqueue(new Tuple<string, Category, Priority>(message, category, priority));
//                if (this.savedLogs.Count > 50) this.savedLogs.Clear();
//            }
//        }

//        /// <summary>
//        /// Replays the saved logs if the Callback has been set.
//        /// </summary>
//        public void ReplaySavedLogs()
//        {
//            if (this.Callback != null)
//            {
//                while (this.savedLogs.Count > 0)
//                {
//                    var log = this.savedLogs.Dequeue();
//                    //this.Callback(log.Item1, log.Item2, log.Item3);
//                    Application.Current.Dispatcher.Invoke(
//                        System.Windows.Threading.DispatcherPriority.Normal, new DoTask(Callback), log.Item1, log.Item2,
//                        log.Item3);
//                }
//            }
//        }
//    };

//    public enum Category
//    {
//        Info = 1,
//        Debug,
//        Error
//    };

//    public enum Priority
//    {
//        Low,
//        Normal,
//        High
//    };

//}
