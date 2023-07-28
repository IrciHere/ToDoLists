using System.Text.Json;

namespace ToDoLists.Core;

public class ToDoListManager
{
    public List<ToDoItem> ToDoItemsList { get; private set; }

    public ToDoListManager()
    {
        ToDoItemsList = new List<ToDoItem>();
    }
    
    public ToDoItem GetItemFromList(string itemTitle)
    {
        ToDoItem? itemFound = ToDoItemsList.Find(findItem => findItem.Title == itemTitle);

        if (itemFound is null)
        {
            throw new Exception("Could not find selected item");
        }

        return itemFound;
    }
    
    public void AddNewItem(string title, string description)
    {
        var itemToAdd = new ToDoItem
        {
            Title = title,
            Description = description
        };
        
        ToDoItemsList.Add(itemToAdd);
    }

    public void DeleteItem(string itemTitle)
    {
        ToDoItem itemToRemove = GetItemFromList(itemTitle);

        ToDoItemsList.Remove(itemToRemove);
    }

    public void ChangeItemDoneStatus(string itemTitle)
    {
        ToDoItem itemToChange = GetItemFromList(itemTitle);

        itemToChange.Done = !itemToChange.Done;
    }

    public void ImportItemsList()
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
            ToDoItemsList = JsonSerializer.Deserialize<List<ToDoItem>>(json) ?? new List<ToDoItem>();
        }
        catch
        {
            throw new Exception("Invalid file format");
        }
    }

    public void SaveItemsList()
    {
        string json = JsonSerializer.Serialize(ToDoItemsList, new JsonSerializerOptions { WriteIndented = true });

        using StreamWriter file = File.CreateText("itemsList.json");

        file.Write(json);
    }
}