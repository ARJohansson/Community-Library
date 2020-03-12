using CommunityLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Repos
{
    public interface IBookRepo
    {
        IEnumerable<Book> Books { get; }
        void AddBook(Book book);
        bool CheckForBookByTitle(string title);
        Book GetBookByTitle(string title);
    }
}
