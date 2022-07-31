using System;
using System.Windows;

using EconomyViewer.Utility;

namespace EconomyViewer.MVVM.Helper;

public class MVVMMessageBoxEventArgs
{
    private readonly Action<MessageBoxResult> _resultAction;
    private readonly string _messageBoxText;
    private readonly string _caption;
    private readonly MessageBoxButton _button;
    private readonly MessageBoxImage _icon;

    public MVVMMessageBoxEventArgs(Action<MessageBoxResult> resultAction, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
    {
        _resultAction = resultAction;
        _messageBoxText = messageBoxText;
        _caption = caption;
        _button = button;
        _icon = icon;
    }
    public void Show()
    {
        _resultAction?.Invoke(MyMessageBox.Show(_messageBoxText, _caption, _button, _icon));
    }
}
