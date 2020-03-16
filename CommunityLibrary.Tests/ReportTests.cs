using System;
using Xunit;
using CommunityLibrary.Repos;
using CommunityLibrary.Repos.FakeRepos;
using CommunityLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace CommunityLibrary.Tests
{
    public class ReportTests
    {
        private IReportRepo repo;
        private List<AppUser> users = new List<AppUser>();
        private List<Book> books = new List<Book>();
        private List<Review> reviews = new List<Review>();

        [Fact]
        public void TestAddReport()
        {
            //Arrange
            repo = new FakeReportRepo();
            SeedData();
            Report report = new Report()
            {
                Reporter = users[1],
                ReportedUserName = users[0].UserName,
                Reason = "Troll",
                Details = "Sorry, dude, it's just for the test, I promise."
            };

            //Act
            repo.AddReport(report);
            AppUser stiles = users[1];

            //Assert
            Assert.Single(repo.Reports);
            Assert.Equal("Stiles", repo.Reports.ElementAt(0).Reporter.UserName);
            Assert.Equal(stiles, repo.Reports.ElementAt(0).Reporter);
            Assert.Equal("TeenWolf", repo.Reports.ElementAt(0).ReportedUserName);
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

            Review review = new Review()
            {
                Reviewer = users[0].UserName,
                BookTitle = books[0].Title,
                BookRating = 5,
                ReviewID = 0,
                Text = "This book was really good! I can't believe I hadn't read it before!"
            };
            reviews.Add(review);

            review = new Review()
            {
                Reviewer = users[1].UserName,
                BookTitle = books[0].Title,
                BookRating = 5,
                ReviewID = 1,
                Text = "This is literally my favorite book. I mean, I thought I was going through some" +
                "crazy stuff!"
            };
            reviews.Add(review);

            review = new Review()
            {
                Reviewer = users[1].UserName,
                BookTitle = books[1].Title,
                BookRating = 5,
                ReviewID = 2,
                Text = "I wish I was a wizard in England! Hogwarts is so much cooler than Beacon Hills High"
            };
            reviews.Add(review);
        }
    }
}
