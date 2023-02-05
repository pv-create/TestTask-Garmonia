using BookStore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Security.Policy;
using System.Numerics;

namespace BookStore.ViewModels
{
    internal class BookstoreViewModel : INotifyPropertyChanged
    {

        private readonly HttpClient _httpClient;

        private Book selectedBook;
        public ObservableCollection<Book>? Books { get; set; }





        private RelayCommand showAll;
        public RelayCommand ShowAll
        {
            get
            {
                return showAll ??
                    (showAll = new RelayCommand(obj =>
                    {
                        Task.Run(async () => { await this.GetBooks(); }).Wait();
                    },
                    (obj) => Books.Count > 0
                    ));
            }
        }

       

   




        //команда обнавления обьекта
        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ??
                  (updateCommand = new RelayCommand(obj =>
                  {
                      Book book = obj as Book;
                      if (book != null)
                      {
                          Task.Run(async () => { await this.UpdateBook(); }).Wait();
                      }
                  },
                 (obj) => Books.Count > 0));
            }
        }




        // команда добавления нового объекта
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      Book book = new Book();
                      book.Created= DateTime.Now;
                     // book.Id = Books.Max(x => x.Id);
                      Books.Add(book); 
                      SelectedBook = book;
                      Task.Run(async () => { await this.InsertBook(); }).Wait();
                      //SelectedBook.Id = Books.Max(x => x.Id);
                      //Task.Run(async () => { await this.GetBooks(); }).Wait();
                  }));
            }
        }




        // команда удаления
        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
                  {
                      Book book = obj as Book;
                      if (book != null)
                      {
                          Task.Run(async () => { await this.DeleteBook(); }).Wait();
                          Books.Remove(book);
                      }
                  },
                 (obj) => Books.Count > 0));
            }
        }










        public BookstoreViewModel()
        {
            _httpClient = new HttpClient();

           // Books = new ObservableCollection<Book>();

            var baseUrl = new Uri("https://localhost:44375/api/");

            _httpClient.BaseAddress = baseUrl;

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            Task.Run(async () => {await this.GetBooks(); }).Wait();

        }


        public Book SelectedBook
        {
            get { return selectedBook; }
            set
            {
                selectedBook = value;
                OnPropertyChanged("SelectedBook");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        private async Task GetBooks()
        {
            
               
            var result = await _httpClient.GetStringAsync("Books");

            var books = JsonConvert.DeserializeObject<List<BookStore.Models.Book>>(result).ToList();


            Books = new ObservableCollection<Book>();


            foreach (var i in books)
            {
                Books.Add(i);
            }  
            
        }


        private async Task InsertBook()
        {
            var book = new
            {

                Author = "",
                Cost = 0,
                Created = selectedBook.Created,
                Description ="",
                Id = 0,
                Pages = 0,
                Title = ""

            };

            var result = await _httpClient.PostAsJsonAsync("Books", book);

            var  returnValue = await result.Content.ReadAsStringAsync();


            selectedBook.Id = Convert.ToInt32(returnValue);

            //await GetBooks();
        }

        private async Task DeleteBook()
        {
            var result = await _httpClient.DeleteAsync("Books/" + selectedBook.Id);
        }


        private async Task UpdateBook()
        {
            var book = new
            {

                Author = selectedBook.Author,
                Cost = selectedBook.Cost,
                Created = selectedBook.Created,
                Description = selectedBook.Description,
                //Id = selectedBook.Id==0? Books.Max(x=>x.Id): selectedBook.Id,
                Id= selectedBook.Id,
                Pages = selectedBook.Pages,
                Title = selectedBook.Title

            };
            var result = await _httpClient.PutAsJsonAsync("Books/" + selectedBook.Id, book);

           // await GetBooks();
        }


    }
}




