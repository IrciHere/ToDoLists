using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ToDoLists
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private MainWindow parent;
        private string title, description;
        public AddWindow(MainWindow _parent)
        {
            InitializeComponent();
            parent = _parent;
        }

        private void addItem(object sender, RoutedEventArgs e)
        {
            if (titleBox.Text == "")
            {
                Close(); return;
            }

            title = titleBox.Text;
            description = descriptionBox.Text;
            parent.addNewItem(title, description);
            Close();
        }
    }
}
