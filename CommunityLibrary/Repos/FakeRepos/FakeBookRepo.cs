using CommunityLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Repos.FakeRepos
{
    public class FakeBookRepo : IBookRepo
    {
        private List<Book> books = new List<Book>();
        public IEnumerable<Book> Books => books;

        public void AddBook(Book book)
        {
            books.Add(book);
        }

        public bool CheckForBookByTitle(string title)
        {
            for(int i = 0; i < books.Count(); i++)
            {
                if(books[i].Title == title)
                {
                    return true;
                }
            }
            return false;
        }

        public Book GetBookByTitle(string title)
        {
            Book book = books.Find(b => b.Title == title);
            return book;
        }
    }
}
