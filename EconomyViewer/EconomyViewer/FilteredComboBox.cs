using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;

namespace EconomyViewer;
public class FilteredComboBox : ComboBox
{
    public static readonly DependencyProperty MinimumSearchLengthProperty =
        DependencyProperty.Register(
            "MinimumSearchLength",
            typeof(int),
            typeof(FilteredComboBox),
            new UIPropertyMetadata(int.MaxValue));

    private string _currentFilter = "";
    private string _oldFilter = "";
    [Description("Length of the search string that triggers filtering.")]
    [Category("Filtered ComboBox")]
    [DefaultValue(int.MaxValue)]
    public int MinimumSearchLength
    {
        [DebuggerStepThrough]
        get => (int)GetValue(MinimumSearchLengthProperty);

        [DebuggerStepThrough]
        set => SetValue(MinimumSearchLengthProperty, value);
    }
    protected TextBox EditableTextBox => (TextBox)GetTemplateChild("PART_EditableTextBox");
    public FilteredComboBox() { }
    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
        if (newValue != null)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(newValue);
            view.Filter += FilterPredicate;
        }

        if (oldValue != null)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(oldValue);
            view.Filter -= FilterPredicate;
        }

        base.OnItemsSourceChanged(oldValue, newValue);
    }

    protected override void OnDropDownOpened(EventArgs e)
    {
        if (Text == "")
        {
            RefreshFilter();
        }
        base.OnDropDownOpened(e);
    }
    /// <summary>
    /// Modify and apply the filter.
    /// </summary>
    /// <param name="e">Key Event Args.</param>
    /// <remarks>
    /// Alternatively, you could react on 'OnTextChanged', but navigating through
    /// the DropDown will also change the text.
    /// </remarks>
    protected override void OnKeyUp(KeyEventArgs e)
    {
        if ((e.Key == Key.Up || e.Key == Key.Down) && SelectedIndex != -1)
            return;
        if (e.Key is Key.Tab or Key.Enter)
        {
            ClearFilter();
        }
        else
        {
            if (Text != _oldFilter)
            {
                if (Text.Length != 0)
                {
                    _oldFilter = Text;
                    RefreshFilter();
                    Text = _oldFilter;
                    IsDropDownOpen = true;
                    EditableTextBox.SelectionStart = int.MaxValue;
                }
                else
                {
                    IsDropDownOpen = false;
                    RefreshFilter();
                    SelectedIndex = -1;
                }
                MaxDropDownHeight = Items.Count == 0 ? 0 : 350;
            }
            base.OnKeyUp(e);
            _currentFilter = Text;
        }
    }
    /// <summary>
    /// Confirm or cancel the selection when Tab, Enter, or Escape are hit.
    /// Open the DropDown when the Down Arrow is hit.
    /// </summary>
    /// <param name="e">Key Event Args.</param>
    /// <remarks>
    /// The 'KeyDown' event is not raised for Arrows, Tab and Enter keys.
    /// It is swallowed by the DropDown if it's open.
    /// So use the Preview instead.
    /// </remarks>
    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        if (e.Key is Key.Tab or Key.Enter)
            IsDropDownOpen = false;
        else if (e.Key == Key.Escape)
        {
            IsDropDownOpen = false;
            SelectedIndex = -1;
            Text = _currentFilter;
        }
        else if (e.Key == Key.Back && EditableTextBox.SelectedText.Length == Text.Length)
        {
            RefreshFilter();
        }
        else
        {
            if (e.Key == Key.Down)
                IsDropDownOpen = true;
            base.OnPreviewKeyDown(e);
        }
        _oldFilter = Text;
    }
    /// <summary>
    /// Make sure the text corresponds to the selection when leaving the control.
    /// </summary>
    /// <param name="e">A KeyBoardFocusChangedEventArgs.</param>
    protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
    {
        ClearFilter();
        int temp = SelectedIndex;
        SelectedIndex = -1;
        Text = "";
        SelectedIndex = temp;
        base.OnPreviewLostKeyboardFocus(e);
    }
    /// <summary>
    /// Clear the Filter.
    /// </summary>
    private void ClearFilter()
    {
        _currentFilter = "";
        RefreshFilter();
    }
    /// <summary>
    /// The Filter predicate that will be applied to each row in the ItemsSource.
    /// </summary>
    /// <param name="value">A row in the ItemsSource.</param>
    /// <returns>Whether or not the item will appear in the DropDown.</returns>
    private bool FilterPredicate(object value)
    {
        return value != null && (Text.Length == 0 || (value.ToString() ?? "").ToLower().Contains(Text.ToLower()));
    }
    /// <summary>
    /// Re-apply the Filter.
    /// </summary>
    private void RefreshFilter()
    {
        if (ItemsSource != null)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ItemsSource);
            view.Refresh();
        }
    }
}