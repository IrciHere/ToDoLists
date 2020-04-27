using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ToDoLists
{
    class ToDoItem
    {
        public string title, description;
        public bool done;
        public ToDoItem(string _title, string _description)
        {
            if (_title.Length <= 0)
            {
                throw new Exception("Title can not be empty");
            }
            title = _title;
            description = _description;
            done = false;
        }

        public ToDoItem()
        {

        }
    }
}
