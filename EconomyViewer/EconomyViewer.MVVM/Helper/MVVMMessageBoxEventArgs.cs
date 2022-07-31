using System;
using System.Windows;

using EconomyViewer.Utility;

namespace EconomyViewer.MVVM.Helper;

public class MVVMMessageBoxEventArgs
{
    private readonly Action _resultAction;
    private readonly string _messageBoxText;
    private readonly Action? _cancelAction;
    private readonly MessageBoxResult _expected;

    private readonly string _caption;
    private readonly MessageBoxButton _button;
    private readonly MessageBoxImage _icon;

    private readonly MessageBoxType? _type;
    public MVVMMessageBoxEventArgs(Action resultAction, MessageBoxResult expected, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
    {
        _resultAction = resultAction;
        _expected = expected;
        _messageBoxText = messageBoxText;
        _caption = caption;
        _button = button;
        _icon = icon;
    }
    public MVVMMessageBoxEventArgs(Action resultAction, string messageBoxText, MessageBoxType type, Action? cancelAction = null)
    {
        _resultAction = resultAction;
        _messageBoxText = messageBoxText;
        _cancelAction = cancelAction;
        _caption = GetCaption(type);
        _button = GetButtons(type);
        _icon = GetIcon(type);
        _type = type;
    }
    private static MessageBoxImage GetIcon(MessageBoxType? type)
    {
        return type switch {
            MessageBoxType.Confirmation => MessageBoxImage.Question,
            MessageBoxType.Error => MessageBoxImage.Error,
            MessageBoxType.Notifing or null => MessageBoxImage.Information,
            _ => throw new NotImplementedException(),
        };
    }
    private static MessageBoxButton GetButtons(MessageBoxType? type)
    {
        return type switch {
            MessageBoxType.Confirmation => MessageBoxButton.YesNo,
            MessageBoxType.Error => MessageBoxButton.OK,
            MessageBoxType.Notifing or null => MessageBoxButton.OK,
            _ => throw new NotImplementedException(),
        };
    }
    private static string GetCaption(MessageBoxType? type)
    {
        return type switch {
            MessageBoxType.Confirmation => "Подтверждение",
            MessageBoxType.Error => "Ошибка",
            MessageBoxType.Notifing or null => "Уведомление",
            _ => throw new NotImplementedException(),
        };
    }

    public void Show()
    {
        if (_type == MessageBoxType.Confirmation)
        {
            if (ShowReady() == MessageBoxResult.Yes)
                _resultAction?.Invoke();
            else
                _cancelAction?.Invoke();
        }
        else
        {
            if (_type == MessageBoxType.Notifing)
                _resultAction?.Invoke();
            ShowReady();
        }
        MessageBoxResult ShowReady()
        {
            return MyMessageBox.Show(_messageBoxText, GetCaption(_type), GetButtons(_type), GetIcon(_type));
        }
    }

}
public enum MessageBoxType
{
    Confirmation,
    Error,
    Notifing
}
