using System;
using System.Windows.Input;

namespace DnD35.SRD.Navigator8.MVVMLight
{
    /// <summary>
    /// This class comes courtesy of the MVVM Light Libs, which I did not create.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;

        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;

            if (canExecute != null)
            {
                _canExecute = canExecute;
            }
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null 
                || _canExecute();
        }

        public virtual void Execute(object parameter)
        {
            if (CanExecute(parameter)
                && _execute != null)
            {
                _execute();
            }
        }
    }
}