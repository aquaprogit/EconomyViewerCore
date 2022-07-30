using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EconomyViewer.Control;

/// <summary>
/// Interaction logic for IntegerUpDown.xaml
/// </summary>
public partial class IntegerUpDown : UserControl, INotifyPropertyChanged
{
    public IntegerUpDown()
    {
        InitializeComponent();
    }
    public int Value {
        get => (int)GetValue(ValueProperty);
        set {
            if (value >= MinValue && value <= MaxValue)
            {
                SetValue(ValueProperty, value);
                OnPropertyChanged(nameof(IsIncreasable));
                OnPropertyChanged(nameof(IsDecreasable));
            }
        }
    }
    public int MaxValue {
        get => (int)GetValue(MaxValueProperty);
        set {
            if (value > MinValue)
                SetValue(MaxValueProperty, value);
            else
                throw new ArgumentOutOfRangeException("MaxValue can not be greater than MinValue");
        }
    }
    public int MinValue {
        get => (int)GetValue(MinValueProperty);
        set {
            if (value < MaxValue)
                SetValue(MinValueProperty, value);
            else
                throw new ArgumentOutOfRangeException("MinValue can not be less than MinValue");
        }
    }
    public bool IsIncreasable => Value < MaxValue;
    public bool IsDecreasable => Value > MinValue;

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(int), typeof(IntegerUpDown), new PropertyMetadata(0));
    public static readonly DependencyProperty MaxValueProperty =
        DependencyProperty.Register("MaxValue", typeof(int), typeof(IntegerUpDown), new PropertyMetadata(int.MaxValue));
    public static readonly DependencyProperty MinValueProperty =
        DependencyProperty.Register("MinValue", typeof(int), typeof(IntegerUpDown), new PropertyMetadata(int.MinValue));

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    private void UpButton_Click(object sender, RoutedEventArgs e) => Value++;

    private void DownButton_Click(object sender, RoutedEventArgs e) => Value--;

    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        bool isAnyNonDigit = e.Text.Any(c => char.IsDigit(c) == false);
        e.Handled = isAnyNonDigit;
        if (isAnyNonDigit == false)
        {
            int integer = int.Parse(e.Text);
            e.Handled = integer < 0 || integer > int.MaxValue;
        }
    }

    private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(typeof(string)))
        {
            string text = (string)e.DataObject.GetData(typeof(string));
            if (text.Any(c => char.IsDigit(c) == false))
            {
                e.CancelCommand();
            }
        }
        else
        {
            e.CancelCommand();
        }
    }
}
