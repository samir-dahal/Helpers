using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LibraryDesktopWPF.Commands
{
    public class Command : ICommand
    {
        private readonly Action _action;
        private readonly Action<object> _actionWithParam;
        private readonly Func<bool> _canExecute;
        private readonly Func<object, bool> _canExecuteWithParam;
        public Command(Action action)
        {
            _action = action;
        }
        public Command(Action<object> action)
        {
            _actionWithParam = action;
        }
        public Command(Action<object> action, Func<object, bool> canExecute)
        {
            _actionWithParam = action;
            _canExecuteWithParam = canExecute;
        }
        public Command(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? _canExecuteWithParam?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _action?.Invoke();
            _actionWithParam?.Invoke(parameter);
        }
        public void ChangeCanExecute()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
