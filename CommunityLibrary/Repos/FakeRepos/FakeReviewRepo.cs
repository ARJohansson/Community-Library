using CommunityLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Repos.FakeRepos
{
    public class FakeReviewRepo : IReviewRepo
    {
        private static List<Review> reviews = new List<Review>();
        public IEnumerable<Review> Reviews => reviews;

        public void AddReview(Review review)
        {
            reviews.Add(review);
        }

        public Review GetReviewById(int id)
        {
            Review review = reviews.Find(r => r.ReviewID == id);
            return review;
        }

        public IEnumerable<Review> GetReviewsByBook(Book book)
        {
            IEnumerable<Review> revs = reviews.Where(r => r.BookTitle == book.Title);
            return revs;
        }
    }
}
