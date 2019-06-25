//using System;

//namespace Wlst.Cr.CoreOne.LogInfo
//{
//    /// <summary>
//    /// LogInfo interface
//    /// </summary>
//    public interface IILogInfo
//    {
//        /// <summary>
//        /// Replays the saved logs if the Callback has been set.
//        /// </summary>
//        void ReplaySavedLogs();

//        /// <summary>
//        /// Gets or sets the callback to receive logs.
//        /// </summary>
//        /// <value>An Action&lt;string, Category, Priority&gt; callback.</value>
//        Action<string, Category, Priority> Callback { get; set; }

//        /// <summary>
//        /// Write a new log entry with the specified category and priority.
//        /// </summary>
//        /// <param name="message">Message body to log.</param>
//        /// <param name="category">Category of the entry.</param>
//        /// <param name="priority">The priority of the entry.</param>
//        void Log(string message, Category category, Priority priority);
//    }
//}
