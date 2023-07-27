using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace ToDoLists
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ToDoItem> itemsList;
        private string json;

        public MainWindow()
        {
            itemsList = new List<ToDoItem>();
            importItemsList();
            InitializeComponent();
            updateListBox();
        }


        private void showItem(object sender, SelectionChangedEventArgs e)
        {
            titleTextBox.Clear();
            descriptionTextBox.Clear();
            doneTextBox.Clear();
            if (listBox.SelectedIndex == -1) return;

            var item = (ListBox)sender;
            var listBoxItem = item.SelectedItem;
            var itemFound = itemsList.Find(findItem => findItem.Title == listBoxItem.ToString());

            titleTextBox.Text = itemFound.Title;
            descriptionTextBox.Text = itemFound.Description;
            doneTextBox.Text = itemFound.Done.ToString();

            if (itemFound.Done) doneTextBox.Foreground = Brushes.Green;
            else doneTextBox.Foreground = Brushes.Red;
        }


        private void updateListBox()
        {
            listBox.Items.Clear();
            if (itemsList.Count <= 0) return;

            foreach (var item in itemsList)
            {
                listBox.Items.Add(item.Title);
            }
        }


        private void deleteListItem(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex < 0) return;

            var item = listBox.SelectedItem.ToString();
            var itemToRemove = itemsList.FirstOrDefault(x => x.Title == item);
            if (itemToRemove is null) return;

            listBox.SelectedIndex = -1;
            itemsList.Remove(itemToRemove);
            updateListBox();
        }


        private void checkDoneItem(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex < 0) return;

            var item = listBox.SelectedItem.ToString();
            var itemToCheck = itemsList.Single(x => x.Title == item);
            itemToCheck.Done = !itemToCheck.Done;
            updateListBox();
            listBox.SelectedItem = item;
        }

        private void openAddWindow(object sender, RoutedEventArgs e)
        {
            AddItemWindow addWindow = new AddItemWindow(this);
            addWindow.Show();
        }

        public void addNewItem(string _title, string _description)
        {
            var itemToAdd = new ToDoItem()
            {
                Title = _title,
                Description = _description
            };
            itemsList.Add(itemToAdd);
            updateListBox();
        }

        private void importItemsList()
        {
            if (!File.Exists("itemsList.json")) return;

            using (StreamReader r = new StreamReader("itemsList.json"))
            {
                json = r.ReadToEnd();
            }

            try
            {
                itemsList = JsonSerializer.Deserialize<List<ToDoItem>>(json);
            }
            catch
            {
                throw new Exception("Invalid file format");
            }
        }

        private void saveItemsList(object sender, RoutedEventArgs e)
        {
            using (StreamWriter file = File.CreateText("itemsList.json"))
            {
                json = JsonSerializer.Serialize(itemsList, new JsonSerializerOptions() { WriteIndented = true });
                file.Write(json);
            }
        }
    }
}