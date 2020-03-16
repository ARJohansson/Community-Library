using System;
using Xunit;
using CommunityLibrary.Repos;
using CommunityLibrary.Repos.FakeRepos;
using CommunityLibrary.Models;
using System.Collections.Generic;
using System.Linq;


namespace CommunityLibrary.Tests
{
    public class ReviewTests
    {
        private IReviewRepo repo;
        private List<AppUser> users = new List<AppUser>();
        private List<Book> books = new List<Book>();
        private List<Review> reviews = new List<Review>();

        [Fact]
        public void TestAddReview()
        {
            //Arrange
            repo = new FakeReviewRepo();
            SeedData();
            Review review = new Review()
            {
                Reviewer = users[0].UserName,
                BookTitle = books[0].Title,
                BookRating = 5,
                ReviewID = 0,
                Text = "This book was really good! I can't believe I hadn't read it before!"
            };

            //Act
            repo.AddReview(review);

            //Assert
            Assert.Single(repo.Reviews);
            Assert.Equal("TeenWolf", repo.Reviews.ElementAt(0).Reviewer);
            Assert.Equal("The Lightning Thief", repo.Reviews.ElementAt(0).BookTitle);
        }

        [Fact]
        public void TestGetReviewById()
        {
            //Arrange
            repo = new FakeReviewRepo();
            SeedReviewData();
            foreach(Review r in reviews)
            {
                repo.AddReview(r);
            }

            //Act
            Review stilesHP = repo.GetReviewById(2);

            //Assert
            Assert.Equal("Harry Potter and the Sorceror's Stone",
                stilesHP.BookTitle);
            Assert.Equal("Stiles", stilesHP.Reviewer);

            Assert.NotEqual("TeenWolf", stilesHP.Reviewer);
        }

        [Fact]
        public void TestGetReviewsByBook()
        {
            //Arrange
            repo = new FakeReviewRepo();
            SeedReviewData();
            foreach(Review r in reviews)
            {
                repo.AddReview(r);
            }

            //Act
            IEnumerable<Review> Pj = repo.GetReviewsByBook(books[0]);

            //Assert
            Assert.Equal(2, Pj.Count());
            Assert.Equal("Stiles", Pj.ElementAt(1).Reviewer);
            Assert.Equal("The Lightning Thief", Pj.ElementAt(1).BookTitle);
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
        public void SeedReviewData()
        {
            SeedData();
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
