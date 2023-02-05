using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    internal class Book : INotifyPropertyChanged
    {
        private int id;
        private string title;
        private string description;
        private string author;
        private DateTime created;
        private int pages;
        private decimal cost;

        public string Author
        {
            get { return author; }
            set
            {
                author = value;
                OnPropertyChanged("Author");
            }
        }

        public int Id
        {
            get { return id; }
            set 
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }


        public string Description
        {
            get { return description; }
            set
            {
                description = value; 
                OnPropertyChanged("Description");
            }
        }
        public DateTime Created
        {
            get { return created; }
            set 
            { 
                created= value;
                OnPropertyChanged("Created");
            }
        }
        public int Pages
        {
            get { return pages; }
            set 
            {
                pages = value;
                OnPropertyChanged("Pages");
            }
        }



        public decimal Cost
        {
            get { return cost; }
            set { cost= value; OnPropertyChanged("Cost"); }
        }






        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
