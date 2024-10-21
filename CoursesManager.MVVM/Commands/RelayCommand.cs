using CoursesManager.MVVM.Commands;
using System.Windows.Input;

namespace CoursesManager.MVVM.Commands
{
    public class RelayCommand(Action execute, Func<bool>? canExecute = null) : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute();
        }

        public void Execute(object? parameter)
        {
            execute.Invoke();
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}