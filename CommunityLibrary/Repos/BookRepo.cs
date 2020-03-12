using CommunityLibrary.Data;
using CommunityLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Repos
{
    public class BookRepo : IBookRepo
    {
        private ApplicationDbContext context;

        public BookRepo(ApplicationDbContext c) => context = c;
        public IEnumerable<Book> Books => context.Books;

        public void AddBook(Book book)
        {
            context.Add(book);
            context.SaveChanges();
        }

        public bool CheckForBookByTitle(string title)
        {
            for(int i = 0; i < context.Books.Count(); i++)
            {
                List <Book> books = Books.ToList();
                if(books[i].Title == title)
                {
                    return true;
                }
            }
            return false;
        }

        public Book GetBookByTitle(string title)
        {
            Book book;
            book = context.Books.First(b => b.Title == title);
            return book;
        }
    }
}
