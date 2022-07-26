using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using EconomyViewer.DAL.EF;
using EconomyViewer.DAL.ViewModel;

using Microsoft.EntityFrameworkCore;

namespace EconomyViewer;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        ServerViewModel = new ServerViewModel();
        DataContext = this;
        InitializeComponent();
    }
    public ServerViewModel ServerViewModel { get; set; }
    private void WorkingPanel_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {

    }

    private void ToAdd_Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void MainRemoveFromSumUp_Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void MinimizeWindow_Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void ListViewItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {

    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void MainCopyValue_Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void MainCheckAllFilter_CheckBox_Click(object sender, RoutedEventArgs e)
    {

    }

    private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
    {

    }

    private void CheckBox_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Edit_Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Delete_Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void CloseWindow_Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void MainClearToSumUp_Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void MainAddToSumUp_Button_Click(object sender, RoutedEventArgs e)
    {

    }
}
