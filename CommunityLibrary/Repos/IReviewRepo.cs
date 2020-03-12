using CommunityLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityLibrary.Repos
{
    public interface IReviewRepo
    {
        IEnumerable<Review> Reviews { get; }
        void AddReview(Review review);
        IEnumerable<Review> GetReviewsByBook(Book book);
        Review GetReviewById(int id);
    }
}
