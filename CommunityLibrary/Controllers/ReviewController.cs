using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CommunityLibrary.Data;
using CommunityLibrary.Models;
using Microsoft.AspNetCore.Identity;
using CommunityLibrary.Repos;
using Microsoft.AspNetCore.Authorization;

namespace CommunityLibrary.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<AppUser> userManager;
        static IBookRepo bookRepo;
        static IReportRepo reportRepo;
        static IReviewRepo reviewRepo;
        static IRequestRepo requestRepo;

        public ReviewController(ApplicationDbContext context, UserManager<AppUser> userMgr,
                IBookRepo bookR, IReportRepo reportR,
                IReviewRepo reviewR, IRequestRepo requestR)
        {
            _context = context;
            userManager = userMgr;
            bookRepo = bookR;
            reportRepo = reportR;
            reviewRepo = reviewR;
            requestRepo = requestR;
        }

        [AllowAnonymous]
        // GET: Reviews
        // Returns a list of reviews depending on the paramaters data and id
        public async Task<IActionResult> Index(string? data, int? id)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if(user != null)
            {
                ViewBag.userReviews = reviewRepo.Reviews.Where(e => e.Reviewer == user.UserName).ToList();
                // If data is not null then return reviews that the current user wrote
                if(data != null)
                {
                    return View(ViewBag.userReviews);
                }
                else if (id != null)
                {
                    Book book = await _context.Books.FirstOrDefaultAsync(b => b.BookID == id);
                    if (book != null)
                    {
                        ViewBag.bookReviews = reviewRepo.Reviews.Where(e => e.BookTitle == book.Title).ToList();
                        return View(ViewBag.bookReviews);
                    }
                    else
                        return NotFound();
                }
            }
            // if id is not null then return reviews based on the selected book
            else if (id != null)
            {
                Book book = await _context.Books.FirstOrDefaultAsync(b => b.BookID == id);
                if (book != null)
                {
                    ViewBag.bookReviews = reviewRepo.Reviews.Where(e => e.BookTitle == book.Title).ToList();
                    return View(ViewBag.bookReviews);
                }
                else
                    return NotFound();
            }
            // otherwise return all reviews

            return View(await _context.Reviews.ToListAsync());
        }

        // Returns the Create View but only if the book id is valid
        // GET: Reviews/Create
        public async Task<IActionResult> Create(int? id)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Book book = await _context.Books.FindAsync(id);
                if(book == null)
                {
                    return NotFound();
                }
                ViewBag.bookTitle = book.Title;
                ViewBag.bookImg = book.ImgLink;
                ViewBag.userName = user.UserName;
                return View();
            }
        }

        // POST: Reviews/Create
        // Creats a new review
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int reviewID, string text, string bookTitle, int bookRating)
        {
            if(bookRating < 1 || bookRating > 5)
            {
                ModelState.AddModelError(nameof(LoginViewModel.UserName),
                       "Invalid bookRating");
            } else if(text.Contains(":/") || text.Contains("\"") || text.Contains(".."+ "\\") || (text == null) ||
                text.Contains("{") || text.Contains("<") || text.Contains("}") || text.Contains(">")){
                ModelState.AddModelError(nameof(LoginViewModel.UserName),
                        "Invalid Text");
            }

            if(bookRepo.CheckForBookByTitle(bookTitle) == false)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                AppUser user = await userManager.GetUserAsync(HttpContext.User);
                Review review = new Review()
                {
                    Reviewer = user.UserName,
                    ReviewID = reviewID,
                    Text = text,
                    BookTitle = bookTitle,
                    BookRating = bookRating
                };

                Book book = bookRepo.GetBookByTitle(review.BookTitle);
                book.Reviews.Add(review);
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return NotFound();
        }

        // Returns the Rate Review Page if the id is valid, the reviewer cannot rate
        // their own review
        // GET: Reviews/RateReview/5
        public async Task<IActionResult> RateReview(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Reviews/RateReview/5
        // rates a user's review 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RateReview(int id, int reviewRating)
        {
            Review review = _context.Reviews.Find(id);
            if (id != review.ReviewID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // if the review has not been rated before then reset the review rating to the input
                if (review.ReviewRating == 0)
                {
                    review.ReviewRating = reviewRating;
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                } // if the review has been rated find the average between the two numbers
                else
                {
                    int avg = (review.ReviewRating + reviewRating) / 2;
                    review.ReviewRating = avg;
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                // Return to the review index
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        // Deletes a review. Only the reviewer can delete the review.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            Book book = bookRepo.GetBookByTitle(review.BookTitle);
            if (review == null || book == null)
            {
                return NotFound();
            }
            book.Reviews.Remove(review);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewID == id);
        }

    }
}
