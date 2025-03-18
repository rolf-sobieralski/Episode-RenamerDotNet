using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Episode_RenamerDotNet.Classes
{
    public class CommandAction : ICommand
    {
        private bool _canExecute;
        private Action<object> _executeWO;
        private Action _execute;
        public event EventHandler CanExecuteChanged;

        public CommandAction(Action<object> execute)
        {
            _canExecute = true;
            _executeWO = execute;
        }
        public CommandAction(Action execute)
        {
            _canExecute = true;
            _execute = execute;
        }

        bool ICommand.CanExecute(object parameter)
        {
            return _canExecute;
        }

        void ICommand.Execute(object parameter)
        {
            if (_executeWO != null)
            {
                _executeWO(parameter);
            }
            else
            {
                _execute();
            }
        }
    }
}
