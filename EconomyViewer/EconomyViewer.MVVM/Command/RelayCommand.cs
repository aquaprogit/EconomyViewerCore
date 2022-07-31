using System;
using System.Windows.Input;

namespace EconomyViewer.MVVM.Command;

public class RelayCommand : ICommand
{
    private readonly Predicate<object?> _canExecute;
    private readonly Action<object?> _execute;

    public RelayCommand(Action<object?> execute, Predicate<object?> canExecute = )
    {
        _canExecute = canExecute;
        _execute = execute;
    }
    public RelayCommand(Action<object?> execute)
    {
        _canExecute = (_) => true;
        _execute = execute;
    }

    public event EventHandler? CanExecuteChanged {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute != null && _canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        if (CanExecute(parameter) && _execute != null)
            _execute(parameter!);
    }
}
