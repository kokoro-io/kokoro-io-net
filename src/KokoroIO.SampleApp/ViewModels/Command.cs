using System;
using System.Windows.Input;

namespace KokoroIO.SampleApp.ViewModels
{
    public sealed class Command : ICommand
    {
        private readonly Action _Action;

        internal Command(Action action)
        {
            _Action = action;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        bool ICommand.CanExecute(object parameter)
            => true;

        public void Execute(object parameter)
            => _Action();
    }
}