using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ToDoLists;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly ToDoListManager _itemsListManager;

    public MainWindow()
    {
        _itemsListManager = new ToDoListManager();
        _itemsListManager.ImportItemsList();
        InitializeComponent();
        UpdateListBox();
    }
    
    #region MethodsCalledFromWindow
    
    private void ShowItem(object sender, SelectionChangedEventArgs e)
    {
        ClearPreviewBox();

        string itemTitle = GetSelectedItemTitle();

        if (string.IsNullOrWhiteSpace(itemTitle))
        {
            return;
        }
        
        ToDoItem itemFound = _itemsListManager.GetItemFromList(itemTitle);

        DrawItemDetails(itemFound);
    }
    
    private void DeleteListItem(object sender, RoutedEventArgs e)
    {
        string itemTitle = GetSelectedItemTitle();
        
        if (string.IsNullOrWhiteSpace(itemTitle))
        {
            return;
        }
        
        _itemsListManager.DeleteItem(itemTitle);

        listBox.SelectedIndex = -1;
        
        UpdateListBox();
    }
    
    private void CheckDoneItem(object sender, RoutedEventArgs e)
    {
        string itemTitle = GetSelectedItemTitle();
        
        if (string.IsNullOrWhiteSpace(itemTitle))
        {
            return;
        }
        
        _itemsListManager.ChangeItemDoneStatus(itemTitle);
        
        UpdateListBox();
    }
    
    private void OpenAddWindow(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddItemWindow(parent: this);
        addWindow.Show();
    }

    public void AddNewItem(string title, string description)
    {
        _itemsListManager.AddNewItem(title, description);
        
        UpdateListBox();
    }

    private void SaveItemsList(object sender, RoutedEventArgs e)
    {
        _itemsListManager.SaveItemsList();
    }
    
    #endregion MethodsCalledFromWindow

    #region MethodsCalledFromCode
    
    private string GetSelectedItemTitle()
    {
        if (listBox.SelectedIndex < 0)
        {
            return string.Empty;
        }

        var listBoxItem = listBox.SelectedItem.ToString();

        return listBoxItem ?? string.Empty;
    }
    
    private void ClearPreviewBox()
    {
        titleTextBox.Clear();
        descriptionTextBox.Clear();
        doneTextBox.Clear();
    }
    
    private void UpdateListBox()
    {
        listBox.Items.Clear();
        
        if (_itemsListManager.ToDoItemsList.Count <= 0)
        {
            return;
        }

        foreach (ToDoItem item in _itemsListManager.ToDoItemsList)
        {
            listBox.Items.Add(item.Title);
        }
    }
    
    private void DrawItemDetails(ToDoItem toDoItem)
    {
        titleTextBox.Text = toDoItem.Title;
        descriptionTextBox.Text = toDoItem.Description;
        doneTextBox.Text = toDoItem.Done.ToString();

        doneTextBox.Foreground = toDoItem.Done ? Brushes.Green : Brushes.Red;
    }
    
    #endregion MethodsCalledFromCode
}