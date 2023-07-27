using System.Windows;

namespace ToDoLists;

public partial class AddItemWindow : Window
{
    private MainWindow parent;
    private string title, description;
    public AddItemWindow(MainWindow _parent)
    {
        InitializeComponent();
        parent = _parent;
    }

    private void closeWindow(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void addItem(object sender, RoutedEventArgs e)
    {
        if (titleBox.Text == "")
        {
            MessageBox.Show("Title can't be empty!");
            return;
        }
        title = titleBox.Text;
        description = descriptionBox.Text;
        parent.addNewItem(title, description);
        Close();  
    }
}