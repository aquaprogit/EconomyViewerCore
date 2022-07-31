using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

using EconomyViewer.MVVM.Helper;

namespace EconomyViewer.MVVM.ViewModel;

public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
    public event EventHandler<MVVMMessageBoxEventArgs>? MessageBoxRequest;
    protected void MessageBox_Show(Action<MessageBoxResult> resultAction, string messageBoxText, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None)
    {
        MessageBoxRequest?.Invoke(this, new MVVMMessageBoxEventArgs(resultAction, messageBoxText, caption, button, icon));
    }
}