using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ClientWPF.Utils.Wpf
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action<object> command;

        public RelayCommand(Action<object> command)
        {
            this.command = command;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            command(parameter);
        }
    }
}
