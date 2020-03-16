using System;
using Xunit;
using CommunityLibrary.Repos;
using CommunityLibrary.Repos.FakeRepos;
using CommunityLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace CommunityLibrary.Tests
{
    public class RequestTests
    {
        private IRequestRepo repo;
        private List<AppUser> users = new List<AppUser>();
        private List<Book> books = new List<Book>();
        private List<Request> requests = new List<Request>();

        [Fact]
        public void TestAddRequest()
        {
            //Arrange
            repo = new FakeRequestRepo();
            SeedData();
            Request request = new Request()
            {
                Requester = users[0].UserName,
                Owner = users[1].UserName,
                BookTitle = books[0].Title,
                Duration = "1 week"
            };

            //Act
            repo.AddRequest(request);

            //Assert
            Assert.Single(repo.Requests);
            Assert.Equal("Stiles", repo.Requests.ElementAt(0).Owner);
            Assert.Equal("The Lightning Thief", repo.Requests.ElementAt(0).BookTitle);
        }

        [Fact]
        public void TestCheckForOwnerName()
        {
            //Arrange
            repo = new FakeRequestRepo();
            SeedRequestData();
            foreach(Request r in requests)
            {
                repo.AddRequest(r);
            }

            //Act
            bool stiles = repo.CheckForOwnerName("Stiles");
            bool robin = repo.CheckForOwnerName("robinhood");

            //Assert
            Assert.True(stiles);
            Assert.False(robin);
        }

        [Fact]
        public void TestCheckForRequesterName()
        {
            //Arrange
            repo = new FakeRequestRepo();
            SeedRequestData();
            foreach(Request r in requests)
            {
                repo.AddRequest(r);
            }

            //Act
            bool stiles = repo.CheckForRequesterName("Stiles");
            bool robin = repo.CheckForRequesterName("robinhood");


            //Assert
            Assert.True(stiles);
            Assert.False(robin);
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

        [Fact]
        public void SeedRequestData()
        {
            SeedData();
            Request request = new Request()
            {
                Requester = users[0].UserName,
                Owner = users[1].UserName,
                BookTitle = books[0].Title,
                Duration = "1 week"
            };

            requests.Add(request);

            request = new Request()
            {
                Requester = users[1].UserName,
                Owner = users[0].UserName,
                BookTitle = books[1].Title,
                Duration = "2 week"
            };

            requests.Add(request);
        }
    }
}
