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
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<AppUser> userManager;
        static IBookRepo bookRepo;
        static IReportRepo reportRepo;
        static IReviewRepo reviewRepo;
        static IRequestRepo requestRepo;

        public BookController(ApplicationDbContext context, UserManager<AppUser> userMgr,
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

        // GET: Books
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? data)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                ViewBag.userBooks = bookRepo.Books.Where(e => e.Owner == user.UserName).ToList();
                if (data == "true")
                {
                    return View(ViewBag.userBooks);
                }
                return View(await _context.Books.ToListAsync());
            }
            return View(await _context.Books.ToListAsync());
        }

        [AllowAnonymous]
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id, string? data)
        {
            if (bookRepo.CheckForBookByTitle(data))
            {
                ViewBag.book = bookRepo.GetBookByTitle(data);
                id = ViewBag.book.BookID;
                AppUser user = await userManager.GetUserAsync(HttpContext.User);
                if(ViewBag.book.Owner == user.UserName)
                {
                    ViewBag.userBooks = ViewBag.book;
                }
            }

            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Users,Admins")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Users,Admins")]
        public async Task<IActionResult> Create([Bind("BookID,Title,ImgLink,Author,Owner,Borrower,Written,Availability,AverageRating")] Book book)
        {
            string link;
            if (book.ImgLink == "1")
                link = "genericblack";
            else if (book.ImgLink == "1")
                link = "genericblue";
            else if (book.ImgLink == "1")
                link = "genericred";
            else
                link = "genericgreen";
            book.ImgLink = link;

            if (ModelState.IsValid)
            {
                AppUser user = await userManager.GetUserAsync(HttpContext.User);
                book.Owner = user.UserName;
                bookRepo.AddBook(book);
                _context.Add(book);
                _context.Update(user);
                //await _context.SaveChangesAsync();
                return RedirectToAction("Index", user);
            }
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Users,Admins")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Users,Admins")]
        public async Task<IActionResult> Edit(int id, [Bind("BookID,Title,ImgLink,Author,Owner,Borrower,Written,Availability,AverageRating")] Book book)
        {
            if (id != book.BookID)
            {
                return NotFound();
            }

            string link;
            if (book.ImgLink == "1")
                link = "genericblack";
            else if (book.ImgLink == "1")
                link = "genericblue";
            else if (book.ImgLink == "1")
                link = "genericred";
            else
                link = "genericgreen";
            book.ImgLink = link;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookID))
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
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Users,Admins")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Users,Admins")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }

        public static int AverageBookRating(Book currentBook)
        {
            int sum = 0;
            if(currentBook.Reviews.Count == 0)
            {
                return currentBook.AverageRating;
            }else
            {
                foreach(Review review in currentBook.Reviews)
                {
                    sum += review.BookRating;
                }
                sum += currentBook.AverageRating;
            }
            currentBook.AverageRating = sum / (currentBook.Reviews.Count + 1);
            return currentBook.AverageRating;           
        }
    }
}
