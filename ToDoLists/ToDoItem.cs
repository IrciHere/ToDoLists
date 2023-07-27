using System;

namespace ToDoLists;

public class ToDoItem
{
    private readonly string _title = string.Empty;

    public required string Title
    {
        get => _title;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception("Title can not be empty!");
            }

            _title = value;
        }
    }

    public string Description { get; init; } = string.Empty;
    public bool Done { get; set; }
}