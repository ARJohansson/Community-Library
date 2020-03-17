using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CommunityLibrary.Data;
using CommunityLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CommunityLibrary.Repos;

namespace CommunityLibrary.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<AppUser> userManager;
        static IBookRepo bookRepo;
        static IReportRepo reportRepo;
        static IReviewRepo reviewRepo;
        static IRequestRepo requestRepo;

        public ReportController(ApplicationDbContext context, UserManager<AppUser> userMgr,
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

        // Returns List of all Reports, only Admins can see this list
        [Authorize(Roles = "Admins")]
        // GET: Reports
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reports.ToListAsync());
        }

        // Returns the details for a reported user, only Admins can see this view
        [Authorize(Roles = "Admins")]
        // GET: Reports/Details/5
        public async Task<IActionResult> UserDetails(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // ViewBags store the specific user's information
            ViewBag.userReports = reportRepo.Reports.Where(e => e.ReportedUserName == user.UserName).ToList();
            ViewBag.userBooks = bookRepo.Books.Where(e => e.Owner == user.UserName).ToList();
            ViewBag.userReviews = reviewRepo.Reviews.Where(e => e.Reviewer == user.UserName).ToList();
            ViewBag.userRequests = requestRepo.Requests.Where(e => e.Requester == user.UserName).ToList();
            ViewBag.userReceived = requestRepo.Requests.Where(e => e.Owner == user.UserName).ToList();
            
            return View(user);
        }

        // Returns Create View based on a review id, cannot create a 
        // report without a review -- for now
        // GET: Reports/Create
        public async Task<IActionResult> Create(int id)
        {

            Review review = reviewRepo.GetReviewById(id);

            AppUser user = await userManager.FindByNameAsync(review.Reviewer);
            ViewBag.reportedUser = user.UserName;

            return View();
        }

        // Creates a new Report
        // POST: Reports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportedUserName,ReportID,Reason,Details")] Report report)
        {
            string reason;
            if (report.Reason == "1")
                reason = "Troll";
            else if (report.Reason == "2")
                reason = "Explicit Language";
            else if (report.Reason == "3")
                reason = "Cyber Bullying";
            else
                reason = "Failure To Return";

            report.Reason = reason;

            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            report.Reporter = user;

            if (ModelState.IsValid)
            {
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Review");
            }
            return View(report);
        }

        // Returns Edit page for report, only Admins can access this page
        // GET: Reports/Edit/5
        [Authorize(Roles = "Admins")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        // Edits a report, only Admins can access this page
        // POST: Reports/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admins")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportID,Reason,Details")] Report report)
        {
            if (id != report.ReportID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.ReportID))
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
            return View(report);
        }

        /********* Not Currently Implemented *********/
        //// GET: Reports/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var report = await _context.Reports
        //        .FirstOrDefaultAsync(m => m.ReportID == id);
        //    if (report == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(report);
        //}

        //// POST: Reports/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var report = await _context.Reports.FindAsync(id);
        //    _context.Reports.Remove(report);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.ReportID == id);
        }
    }
}
