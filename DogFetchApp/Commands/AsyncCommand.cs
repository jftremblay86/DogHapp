using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DogFetchApp.Commands
{
    /// <summary>
    /// Async Command
    /// 
    /// Based on John Thiriet AsynCommand
    /// Source : https://johnthiriet.com/mvvm-going-async-with-async-command/
    /// </summary>
    public interface IAsyncCommand<T> : ICommand
    {
        Task ExecuteAsync(T parameter);
        bool CanExecute(T parameter);
    }

    public class AsyncCommand<T> : IAsyncCommand<T>
    {
        private bool _isExecuting;
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;

        public AsyncCommand(
            Func<T, Task> execute,
            Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(T parameter)
        {
            return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
        }

        public async Task ExecuteAsync(T parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    await _execute(parameter);
                }
                finally
                {
                    _isExecuting = false;
                }
            }
            RaiseCanExecuteChanged();
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region ICommand methods implementation
        public bool CanExecute(object parameter)
        {

            return CanExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            /// We ignore the warning, since this way of executing
            /// is the sync way
        #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            ExecuteAsync((T)parameter);
        #pragma warning restore CS4014 
            // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
        }
        #endregion
    }
}