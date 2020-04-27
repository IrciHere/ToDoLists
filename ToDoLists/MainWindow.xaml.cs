using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

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
            titleTextBox.Document.Blocks.Clear();
            descriptionTextBox.Document.Blocks.Clear();
            doneTextBox.Document.Blocks.Clear();
            if (listBox.SelectedIndex == -1)
            {
                return;
            } 
            var item = (ListBox) sender;
            var listBoxItem = item.SelectedItem;
            var itemFound = itemsList.Find(findItem => findItem.title == listBoxItem.ToString());

            TextRange rangeTitle = new TextRange(titleTextBox.Document.ContentEnd, titleTextBox.Document.ContentEnd);
            rangeTitle.Text = itemFound.title + "\n\n";
            rangeTitle.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.ExtraBold);
            rangeTitle.ApplyPropertyValue(TextElement.FontSizeProperty, 20.0);

            TextRange rangeDescription = new TextRange(descriptionTextBox.Document.ContentEnd, descriptionTextBox.Document.ContentEnd);
            rangeDescription.Text = itemFound.description;
            rangeDescription.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            rangeDescription.ApplyPropertyValue(TextElement.FontSizeProperty, 16.0);
            rangeDescription.ApplyPropertyValue(TextElement.FontFamilyProperty, "Times New Roman");

            TextRange rangeDone = new TextRange(doneTextBox.Document.ContentEnd, doneTextBox.Document.ContentEnd);
            rangeDone.Text = "Task done: " + itemFound.done;
            if (itemFound.done) rangeDone.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
            else rangeDone.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            rangeDone.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            rangeDone.ApplyPropertyValue(TextElement.FontSizeProperty, 20.0);
        }


        private void updateListBox()
        {
            listBox.Items.Clear();
            if (itemsList.Count <= 0) return;

            foreach (var item in itemsList)
            {
                listBox.Items.Add(item.title);
            }
        }


        private void deleteListItem(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex < 0) return;
            
            var item = listBox.SelectedItem.ToString();
            var itemToRemove = itemsList.Single(x => x.title == item);
            listBox.SelectedIndex = -1;
            itemsList.Remove(itemToRemove);
            updateListBox();    
        }


        private void checkDoneItem(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex < 0) return;

            var item = listBox.SelectedItem.ToString();
            var itemToCheck = itemsList.Single(x => x.title == item);
            itemToCheck.done = !itemToCheck.done;
            updateListBox();
            listBox.SelectedItem = item;
        }

        private void openAddWindow(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow(this);
            addWindow.Show();
        }

        public void addNewItem(string _title, string _description)
        {
            itemsList.Add(new ToDoItem(_title, _description));
            updateListBox();
        }

        private void importItemsList()
        {
            try
            {
                using (StreamReader r = new StreamReader("itemsList.json"))
                {
                    json = r.ReadToEnd();
                }
            }
            catch
            {
                return;
            }

            itemsList = JsonConvert.DeserializeObject<List<ToDoItem>>(json);
        }

        private void saveItemsList(object sender, RoutedEventArgs e)
        {
            using (StreamWriter file = File.CreateText("itemsList.json"))
            {
                json = JsonConvert.SerializeObject(itemsList, Formatting.Indented);
                file.Write(json);
            }
        }
    }
}
