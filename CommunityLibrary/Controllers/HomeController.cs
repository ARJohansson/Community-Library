using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CommunityLibrary.Models;
using CommunityLibrary.Repos;

namespace CommunityLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static IBookRepo bookRepo;
        static IReportRepo reportRepo;
        static IReviewRepo reviewRepo;
        static IRequestRepo requestRepo;

        public HomeController(ILogger<HomeController> logger, IBookRepo bookR, 
            IReportRepo reportR, IReviewRepo reviewR, IRequestRepo requestR)
        {
            _logger = logger;
            bookRepo = bookR;
            reportRepo = reportR;
            reviewRepo = reviewR;
            requestRepo = requestR;
        }

        // Returns the View for the Index
        [HttpGet]
        public IActionResult Index()
        {
            // Calculates the most popular books
            ViewBag.currentBooks = bookRepo.Books.Where(e => e.AverageRating >= 4).ToList();
            // Selects five of them
            if(ViewBag.currentBooks.Count > 5)
            {
                List<Book> books = ViewBag.currentBooks;
                ViewBag.currentBooks = books.Take(5);
            }
            return View();
        }

        // Checks which search bar is being used and passes the correct List to the Books View
        [HttpPost]
        public IActionResult Index(string? searchAuthor, string? searchTitle)
        {
            if (searchAuthor == null && searchTitle == null)
            {
                return View("Index");
            }
            else
            {
                if (searchAuthor != null)
                {
                    // Makes sure the search text doesn't have anything harmful before checking the repository
                    if (searchAuthor.Contains("<") || searchAuthor.Contains("=") || searchAuthor.Contains("$")
                       || searchAuthor.Contains("(") || searchAuthor.Contains("{"))
                        return View("Index");
                    else
                    {
                        var possibleAuthors = (from Book b in bookRepo.Books
                                               where b.Author.Contains(searchAuthor)
                                               select b).ToList();
                        return View("Books", possibleAuthors);
                    }
                }
                else
                {
                    // Makes sure the search text doesn't have anything harmful before checking the repository
                    if (searchTitle.Contains("<") || searchTitle.Contains("=") || searchTitle.Contains("$")
                       || searchTitle.Contains("(") || searchTitle.Contains("{"))
                        return View("Index");
                    else
                    {
                        var possibleTitles = (from Book b in bookRepo.Books
                                              where b.Title.Contains(searchTitle)
                                              select b).ToList();
                        return View("Books", possibleTitles);
                    }
                }
            }
        }

        // Returns Privacy View - Currently Empty
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
