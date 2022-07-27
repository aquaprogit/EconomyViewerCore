using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EconomyViewer.Control
{
    /// <summary>
    /// Interaction logic for IntegerUpDown.xaml
    /// </summary>
    public partial class IntegerUpDown : UserControl
    {
        public IntegerUpDown()
        {
            InitializeComponent();
            DataContext = this;
        }
        public int Value {
            get => (int)GetValue(ValueProperty);
            set {
                if (value >= MinValue && value <= MaxValue)
                {
                    SetValue(ValueProperty, value);
                    Debug.WriteLine(Value.ToString());
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
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(IntegerUpDown), new PropertyMetadata(5));
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(IntegerUpDown), new PropertyMetadata(int.MaxValue));
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(IntegerUpDown), new PropertyMetadata(int.MinValue));

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            Value++;
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            Value--;
        }
    }
}
