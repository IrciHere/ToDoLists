using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ToDoLists;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private List<ToDoItem> _itemsList;

    public MainWindow()
    {
        _itemsList = new List<ToDoItem>();
        ImportItemsList();
        InitializeComponent();
        UpdateListBox();
    }
    
    private void ShowItem(object sender, SelectionChangedEventArgs e)
    {
        ClearPreviewBox();

        if (listBox.SelectedIndex == -1)
        {
            return;
        }

        var item = (ListBox)sender;
        var listBoxItem = item.SelectedItem?.ToString();

        if (listBoxItem == null)
        {
            return;
        }
        
        ToDoItem itemFound = GetItemFromList(listBoxItem);

        titleTextBox.Text = itemFound.Title;
        descriptionTextBox.Text = itemFound.Description;
        doneTextBox.Text = itemFound.Done.ToString();

        doneTextBox.Foreground = itemFound.Done ? Brushes.Green : Brushes.Red;
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
        
        if (_itemsList.Count <= 0)
        {
            return;
        }

        foreach (ToDoItem item in _itemsList)
        {
            listBox.Items.Add(item.Title);
        }
    }


    private void DeleteListItem(object sender, RoutedEventArgs e)
    {
        if (listBox.SelectedIndex < 0)
        {
            return;
        }

        var itemTitle = listBox.SelectedItem.ToString();
        
        ToDoItem itemToRemove = GetItemFromList(itemTitle);

        listBox.SelectedIndex = -1;
        _itemsList.Remove(itemToRemove);
        UpdateListBox();
    }


    private void CheckDoneItem(object sender, RoutedEventArgs e)
    {
        if (listBox.SelectedIndex < 0) return;

        var itemTitle = listBox.SelectedItem.ToString();
        
        ToDoItem itemToCheck = GetItemFromList(itemTitle);
        
        itemToCheck.Done = !itemToCheck.Done;
        UpdateListBox();
    }

    private ToDoItem GetItemFromList(string itemTitle)
    {
        ToDoItem? itemFound = _itemsList.Find(findItem => findItem.Title == itemTitle);

        if (itemFound is null)
        {
            throw new Exception("Could not find selected item");
        }

        return itemFound;
    }

    private void OpenAddWindow(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddItemWindow(parent: this);
        addWindow.Show();
    }

    public void AddNewItem(string _title, string _description)
    {
        var itemToAdd = new ToDoItem
        {
            Title = _title,
            Description = _description
        };
        _itemsList.Add(itemToAdd);
        UpdateListBox();
    }

    private void ImportItemsList()
    {
        string json;

        if (!File.Exists("itemsList.json"))
        {
            return;
        }

        using (var r = new StreamReader("itemsList.json"))
        {
            json = r.ReadToEnd();
        }

        try
        {
            _itemsList = JsonSerializer.Deserialize<List<ToDoItem>>(json) ?? new List<ToDoItem>();
        }
        catch
        {
            throw new Exception("Invalid file format");
        }
    }

    private void SaveItemsList(object sender, RoutedEventArgs e)
    {
        string json = JsonSerializer.Serialize(_itemsList, new JsonSerializerOptions { WriteIndented = true });

        using StreamWriter file = File.CreateText("itemsList.json");

        file.Write(json);
    }
}