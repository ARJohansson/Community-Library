using CommunityLibrary.Data;
using CommunityLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Repos
{
    public class ReviewRepo : IReviewRepo
    {
        private ApplicationDbContext context;

        public ReviewRepo(ApplicationDbContext c) => context = c;
        public IEnumerable<Review> Reviews => context.Reviews;

        public void AddReview(Review review)
        {
            context.Add(review);
            context.SaveChanges();
        }

        public IEnumerable<Review> GetReviewsByBook(Book book)
        {
            return (from r in context.Reviews
                    where r.BookTitle.Contains(book.Title)
                    select r).ToList();
        }

        public Review GetReviewById(int id)
        {
            Review review = context.Reviews.Find(id);
            return review;
        }
    }
}
