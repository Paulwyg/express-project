using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

using JetBrains.Annotations;

namespace Elysium.Controls
{
    [PublicAPI]
    public static class WindowCommands
    {
        [PublicAPI]
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
            MessageId = "System.Windows.Input.RoutedUICommand.#ctor(System.String,System.String,System.Type)",
            Justification = "Minimize is the name of command.")]
        public static RoutedUICommand Minimize
        {
            get
            {
                return _minimize ?? (_minimize = new RoutedUICommand("Minimize", "Minimize", typeof(Window)));
            }
        }

        private static RoutedUICommand _minimize;

        [PublicAPI]
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
            MessageId = "System.Windows.Input.RoutedUICommand.#ctor(System.String,System.String,System.Type)",
            Justification = "Maximize is the name of command.")]
        public static RoutedUICommand Maximize
        {
            get
            {
                return _maximize ?? (_maximize = new RoutedUICommand("Maximize", "Miaximize", typeof(Window)));
            }
        }

        private static RoutedUICommand _maximize;

        [PublicAPI]
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
            MessageId = "System.Windows.Input.RoutedUICommand.#ctor(System.String,System.String,System.Type)",
            Justification = "Restore is the name of command.")]
        public static RoutedUICommand Restore
        {
            get
            {
                return _restore ?? (_restore = new RoutedUICommand("Restore", "Restore", typeof(Window)));
            }
        }

        private static RoutedUICommand _restore;

        [PublicAPI]
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
            MessageId = "System.Windows.Input.RoutedUICommand.#ctor(System.String,System.String,System.Type)",
            Justification = "Close is the name of command.")]
        public static RoutedUICommand Close
        {
            get
            {
                return _close ?? (_close = new RoutedUICommand("Close", "Close", typeof(Window)));
            }
        }

        private static RoutedUICommand _close;
    }
}