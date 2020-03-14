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
        public async Task<IActionResult> Index(string? data, int? id)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if(user != null)
            {
                ViewBag.userReviews = reviewRepo.Reviews.Where(e => e.Reviewer == user.UserName).ToList();
                if(data != null)
                {
                    return (ViewBag.userReviews);
                }
                else if (data is null)
                {
                    return View(await _context.Reviews.ToListAsync());
                }

                if (id != null)
                {
                    Book book = await _context.Books.FirstOrDefaultAsync(b => b.BookID == id);
                    ViewBag.bookReviews = reviewRepo.Reviews.Where(e => e.BookTitle == book.Title).ToList();
                    return (ViewBag.bookRevies);
                }
                else
                {
                    return View(await _context.Reviews.ToListAsync());
                }
            }

            return View(await _context.Reviews.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public async Task<IActionResult> Create(int? id)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            if (id == null)
            {
                return View();
            }
            else
            {
                Book book = await _context.Books.FindAsync(id);
                ViewBag.bookTitle = book.Title;
                ViewBag.bookImg = book.ImgLink;
                ViewBag.userName = user.UserName;
                return View();
            }
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int reviewID, string text, string bookTitle, int bookRating)
        {
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
                
                if (bookRepo.CheckForBookByTitle(review.BookTitle))
                {
                    Book book = bookRepo.GetBookByTitle(review.BookTitle);
                    book.Reviews.Add(review);
                    _context.Add(review);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }else
                {
                    return NotFound();
                }
            }
            return NotFound();
        }

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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                try
                {
                    if (review.ReviewRating == 0)
                    {
                        review.ReviewRating = reviewRating;
                        _context.Update(review);
                    }
                    else
                    {
                        int avg = (review.ReviewRating + reviewRating) / 2;
                        review.ReviewRating = avg;
                        _context.Update(review);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            Book book = bookRepo.GetBookByTitle(review.BookTitle);
            if (review == null)
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
