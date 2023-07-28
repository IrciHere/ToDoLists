using System.Windows;

namespace ToDoLists;

public partial class AddItemWindow
{
    private readonly MainWindow _parent;

    public AddItemWindow(MainWindow parent)
    {
        InitializeComponent();
        _parent = parent;
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void AddItem(object sender, RoutedEventArgs e)
    {
        string itemTitle = titleBox.Text;
        string itemDescription = descriptionBox.Text;

        if (string.IsNullOrWhiteSpace(itemTitle))
        {
            MessageBox.Show("Title can't be empty!");
            return;
        }
        
        _parent.AddNewItem(itemTitle, itemDescription);
        
        Close();  
    }
}