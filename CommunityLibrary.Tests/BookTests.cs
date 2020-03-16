using System;
using Xunit;
using CommunityLibrary.Repos;
using CommunityLibrary.Repos.FakeRepos;
using CommunityLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace CommunityLibrary.Tests
{
    public class BookTests
    {
        private List<AppUser> users = new List<AppUser>();
        private List<Book> books = new List<Book>();
        private IBookRepo repo;
        // Test Add
        [Fact]
        public void TestAddBook()
        {
            //Arrange
            repo = new FakeBookRepo();
            SeedData();
            Book book = new Book()
            {
                Title = "The Lightning Thief",
                Author = "Rick Riordan",
                Written = new DateTime(2005, 06, 28),
                Owner = users[1].UserName,
                Availability = true,
                AverageRating = 5,
                ImgLink = "genericblack"
            };

            // Act
            repo.AddBook(book);

            // Assert
            Assert.Single(repo.Books);
            Assert.Equal("The Lightning Thief",
                repo.Books.ElementAt(repo.Books.Count() - 1).Title);
            Assert.Equal(users[1].UserName,
                repo.Books.ElementAt(repo.Books.Count() - 1).Owner);
        }

        [Fact]
        public void TestCheckForBookByTitle()
        {
            //Arrange
            repo = new FakeBookRepo();
            SeedBookData();
            foreach (Book b in books)
            {
                repo.AddBook(b);
            }

            //Act
            bool hp = repo.CheckForBookByTitle("Harry Potter and the Sorceror's Stone");
            bool pj = repo.CheckForBookByTitle("The Heroes of Olympus");

            //Assert
            Assert.True(hp);
            Assert.False(pj);
        }

        [Fact]
        public void TestGetBookByTitle()
        {
            //Arrange
            repo = new FakeBookRepo();
            SeedBookData();
            foreach(Book b in books)
            {
                repo.AddBook(b);
            }

            //Act
            Book hp = repo.GetBookByTitle("Harry Potter and the Sorceror's Stone");
            Book pj = repo.GetBookByTitle("The Lightning Thief");

            //Assert
            Assert.Equal(books[0], pj);
            Assert.NotEqual(books[1], pj);
            Assert.Equal(books[1], hp);
            Assert.NotEqual(books[0], hp);
        }

        [Fact]
        public void SeedData()
        {
            AppUser user = new AppUser()
            {
                UserName = "TeenWolf",
                Name = "Scott McCall",
                Email = "justwerewolfthings@gmail.com"
            };

            users.Add(user);

            user = new AppUser()
            {
                UserName = "Stiles",
                Name = "Stiles Stilinski",
                Email = "mybestfriendsawerewolf@gmail.com"
            };

            users.Add(user);
        }

        [Fact]
        public void SeedBookData()
        {
            SeedData();
            Book book = new Book()
            {
                Title = "The Lightning Thief",
                Author = "Rick Riordan",
                Written = new DateTime(2005, 06, 28),
                Owner = users[1].UserName,
                Availability = true,
                AverageRating = 5,
                ImgLink = "genericblack"
            };

            books.Add(book);

            book = new Book()
            {
                Title = "Harry Potter and the Sorceror's Stone",
                Author = "J.K. Rowling",
                Written = new DateTime(1997, 06, 26),
                Owner = users[0].UserName,
                Availability = false,
                Borrower = users[1].UserName,
                AverageRating = 4,
                ImgLink = "genericred"
            };

            books.Add(book);
        }
    }
}
