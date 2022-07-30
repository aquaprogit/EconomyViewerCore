using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EconomyViewer.MVVM.Helper;

public class CheckBoxData : INotifyPropertyChanged
{
    private string _header;
    private bool _isChecked;

    public string Header {
        get => _header;
        set {
            _header = value;
            OnPropertyChanged();
        }
    }
    public bool IsChecked {
        get => _isChecked; set {
            _isChecked = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string propName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
    public override string ToString()
    {
        return $"{Header} - {IsChecked.ToString()[0]}";
    }
}
