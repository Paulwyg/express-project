using System;
using System.Windows.Input;
using System.Diagnostics;
using Wlst.Cr.Core.UtilityFunction;

namespace Wlst.Cr.CoreMims.Commands
{
    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other objects by invoking delegates. The default return value for the CanExecute method is 'true'.
    /// </summary>
    [Serializable]
    public class RelayCommand<T> : ICommand
    {

        #region Declarations

        readonly Predicate<T> _canExecute;
        readonly Action<T> _execute;

        #endregion
        /// <summary>
        /// 是否检查socket状态
        /// </summary>
        private bool _chechSocket=false ;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class and the command can always be executed.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<T> execute)
            : this(execute, null, false )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <param name="isCheckSocketConnect">canExecute whether add a para withe check socket conect </param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute,bool isCheckSocketConnect)
        {

            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
            _chechSocket = isCheckSocketConnect;
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
            if (_chechSocket && !Wlst .Cr .SuperSocketSvrCnt .Services .SuperSocketClnt .IsConnected ) return false;
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
    public class RelayCommand : ICommand
    {

        #region Declarations

        readonly Func<Boolean> _canExecute;
        readonly Action _execute;

        #endregion

        /// <summary>
        /// 是否检查socket状态
        /// </summary>
        private bool _chechSocket = false;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class and the command can always be executed.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action execute)
            : this(execute, null,false  )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// /// <param name="isCheckSocketConnect">canExecute whether add a para withe check socket conect </param>
        public RelayCommand(Action execute, Func<Boolean> canExecute, bool isCheckSocketConnect)
        {

            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
            _chechSocket = isCheckSocketConnect;
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
            if (_chechSocket && !Wlst.Cr.SuperSocketSvrCnt.Services.SuperSocketClnt.IsConnected) return false;
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


    ///// <summary>
    ///// A command whose sole purpose is to relay its functionality to other objects by invoking delegates. The default return value for the CanExecute method is 'true'.
    ///// </summary>
    //public class RelayCommand<T> : ICommand
    //{

    //    #region Declarations

    //    readonly Predicate<T> _canExecute;
    //    readonly Action<T> _execute;

    //    #endregion

    //    #region Constructors

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class and the command can always be executed.
    //    /// </summary>
    //    /// <param name="execute">The execution logic.</param>
    //    public RelayCommand(Action<T> execute)
    //        : this(execute, null)
    //    {
    //    }

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class.
    //    /// </summary>
    //    /// <param name="execute">The execution logic.</param>
    //    /// <param name="canExecute">The execution status logic.</param>
    //    public RelayCommand(Action<T> execute, Predicate<T> canExecute)
    //    {

    //        if (execute == null)
    //            throw new ArgumentNullException("execute");
    //        _execute = execute;
    //        _canExecute = canExecute;
    //    }

    //    #endregion

    //    #region ICommand Members

    //    public event EventHandler CanExecuteChanged
    //    {
    //        add
    //        {

    //            if (_canExecute != null)
    //                CommandManager.RequerySuggested += value;
    //        }
    //        remove
    //        {

    //            if (_canExecute != null)
    //                CommandManager.RequerySuggested -= value;
    //        }
    //    }

    //    [DebuggerStepThrough]
    //    public Boolean CanExecute(Object parameter)
    //    {
    //       // return _canExecute == null || _canExecute((T) parameter);
    //        if (_canExecute == null) return true;
    //        if(parameter !=null )return _canExecute((T)parameter);
    //        return false; //参数需设置在command前
    //    }

    //    public void Execute(Object parameter)
    //    {
    //        try
    //        {
    //            _execute((T) parameter);
    //        }
    //        catch (Exception ex)
    //        {
    //            WriteLog.WriteLogError("Core RelayCommand Execute Error:" + ex.ToString());
    //        }
    //    }

    //    #endregion


    //}

    ///// <summary>
    ///// A command whose sole purpose is to relay its functionality to other objects by invoking delegates. The default return value for the CanExecute method is 'true'.
    ///// </summary>
    //public class RelayCommand : ICommand
    //{

    //    #region Declarations

    //    readonly Func<Boolean> _canExecute;
    //    readonly Action _execute;

    //    #endregion

    //    #region Constructors

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class and the command can always be executed.
    //    /// </summary>
    //    /// <param name="execute">The execution logic.</param>
    //    public RelayCommand(Action execute)
    //        : this(execute, null)
    //    {
    //    }

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class.
    //    /// </summary>
    //    /// <param name="execute">The execution logic.</param>
    //    /// <param name="canExecute">The execution status logic.</param>
    //    public RelayCommand(Action execute, Func<Boolean> canExecute)
    //    {

    //        if (execute == null)
    //            throw new ArgumentNullException("execute");
    //        _execute = execute;
    //        _canExecute = canExecute;
    //    }

    //    #endregion

    //    #region ICommand Members

    //    public event EventHandler CanExecuteChanged
    //    {
    //        add
    //        {

    //            if (_canExecute != null)
    //                CommandManager.RequerySuggested += value;
    //        }
    //        remove
    //        {

    //            if (_canExecute != null)
    //                CommandManager.RequerySuggested -= value;
    //        }
    //    }

    //    [DebuggerStepThrough]
    //    public Boolean CanExecute(Object parameter)
    //    {
    //        return _canExecute == null || _canExecute();
    //    }

    //    public void Execute(Object parameter)
    //    {
    //        try
    //        {
    //            _execute();
    //        }
    //        catch (Exception ex)
    //        {
    //            WriteLog.WriteLogError("Core RelayCommand Execute Error:" + ex.ToString());
    //        }
    //    }

    //    #endregion
    //}
}
