using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using EconomyViewer.MVVM.ViewModel;

namespace EconomyViewer;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly List<Grid> _contentGrids;
    public MainWindow()
    {
        ServerViewModel = new ServerViewModel();
        DataContext = this;
        InitializeComponent();
        _contentGrids = new() { Main_Grid, Add_Grid, Edit_Grid };
    }
    public ServerViewModel ServerViewModel { get; set; }
    #region Navigation Handlers
    private void MinimizeWindow_Button_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
    private void CloseWindow_Button_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
    private void ListViewItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        string menuItem = ((ListViewItem)sender).Tag.ToString()!;
        _contentGrids.ForEach(contentGrid => contentGrid.Visibility = contentGrid.Name.StartsWith(menuItem) ? Visibility.Visible : Visibility.Hidden);
    }
    #endregion
    private void WorkingPanel_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        ToggleNavBar_ToggleButton.IsChecked = false;
    }

    private void ToAdd_Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void MainRemoveFromSumUp_Button_Click(object sender, RoutedEventArgs e)
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

    private void MainClearToSumUp_Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void MainAddToSumUp_Button_Click(object sender, RoutedEventArgs e)
    {

    }


}
