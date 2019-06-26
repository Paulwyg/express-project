using System;
using System.Windows.Input;
using System.Diagnostics;
using Wlst.Cr.Core.UtilityFunction;

namespace Wlst.Cr.Core.CommandCore
{
    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other objects by invoking delegates. The default return value for the CanExecute method is 'true'.
    /// </summary>
    [Serializable]
    public class CommandRelay<T> : ICommand
    {

        #region Declarations

        readonly Predicate<T> _canExecute;
        readonly Action<T> _execute;

        #endregion


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRelay&lt;T&gt;"/> class and the command can always be executed.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public CommandRelay(Action<T> execute)
            : this(execute, null )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRelay&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public CommandRelay(Action<T> execute, Predicate<T> canExecute)
        {

            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        [DebuggerStepThrough]
        public Boolean CanExecute(Object parameter)
        {
            // return _canExecute == null || _canExecute((T) parameter);
            if (_canExecute == null) return true;
            if (parameter != null) return _canExecute((T)parameter);
            return false; //参数需设置在command前
        }

        public void Execute(Object parameter)
        {
            try
            {
                _execute((T)parameter);
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Core RelayCommand Execute Error:" + ex.ToString());
            }
        }

        #endregion


    }

    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other objects by invoking delegates. The default return value for the CanExecute method is 'true'.
    /// </summary>
    [Serializable]
    public class CommandRelay : ICommand
    {

        #region Declarations

        readonly Func<Boolean> _canExecute;
        readonly Action _execute;

        #endregion


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRelay&lt;T&gt;"/> class and the command can always be executed.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public CommandRelay(Action execute)
            : this(execute, null  )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRelay&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public CommandRelay(Action execute, Func<Boolean> canExecute)
        {

            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        [DebuggerStepThrough]
        public Boolean CanExecute(Object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(Object parameter)
        {
            try
            {
                _execute();
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Core RelayCommand Execute Error:" + ex.ToString());
            }
        }

        #endregion
    }


}
