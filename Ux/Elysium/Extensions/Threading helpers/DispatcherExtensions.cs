using System;
using System.Windows.Threading;

using JetBrains.Annotations;

namespace Elysium.Extensions
{
    [PublicAPI]
    public static class DispatcherExtensions
    {
        [PublicAPI]
        public static DispatcherOperation BeginInvoke(this Dispatcher dispatcher, Action callback)
        {
            return dispatcher.BeginInvoke(callback);
        }

        [PublicAPI]
        public static DispatcherOperation BeginInvoke(this Dispatcher dispatcher, Action callback, DispatcherPriority priority)
        {
            return dispatcher.BeginInvoke(callback, priority);
        }

        [PublicAPI]
        public static object Invoke(this Dispatcher dispatcher, Action callback)
        {
            return dispatcher.Invoke(callback);
        }

        [PublicAPI]
        public static object Invoke(this Dispatcher dispatcher, Action callback, DispatcherPriority priority)
        {
            return dispatcher.Invoke(callback, priority);
        }
    }
}