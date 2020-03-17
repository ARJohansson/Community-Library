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
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<AppUser> userManager;
        private IRequestRepo requestRepo;

        public RequestController(ApplicationDbContext context, UserManager<AppUser> userMgr,
                IRequestRepo requestR)
        {
            _context = context;
            userManager = userMgr;
            requestRepo = requestR;
        }

        // Returns a list of requests depending on paramater variable data.
        // GET: Requests
        public async Task<IActionResult> Index(string data)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            List<Request> received = new List<Request>();
            List<Request> requests = new List<Request>();
            // If data == null returns the list of all requests that are related to the user
            if (data == null)
            {
                List<Request> userRequests = new List<Request>();
                if (requestRepo.CheckForOwnerName(user.UserName))
                {
                    received = requestRepo.Requests.Where(e => e.Owner == user.UserName).ToList();
                    foreach(Request r in received)
                    {
                        userRequests.Add(r);
                    }
                }
                if (requestRepo.CheckForRequesterName(user.UserName))
                {
                    requests = requestRepo.Requests.Where(e => e.Requester == user.UserName).ToList();
                    foreach(Request r in requests)
                    {
                       userRequests.Add(r);
                    }
                }
                userRequests = requestRepo.Requests.ToList();
                return View(userRequests);

            } //If data == "requests" returns a list of the current user's created requests. 
            else if(data == "requests")
            {
                if (requestRepo.CheckForRequesterName(user.UserName))
                {
                    requests = requestRepo.Requests.Where(e => e.Requester == user.UserName).ToList();
                    return View(requests);
                }
                else 
                    return NotFound();
            } // If data == "received" returns a list of the current user's received requests. 
            else
            {
                if (requestRepo.CheckForOwnerName(user.UserName))
                {
                    received = requestRepo.Requests.Where(e => e.Owner == user.UserName).ToList();
                    return View(received);
                }
                else
                    return NotFound();
            }
        }

        // Returns view of request create view, only viewable if passed a valid book id
        // GET: Requests/Create
        public IActionResult Create(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            Book book = _context.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.thisBook = book.Title;
            ViewBag.bookOwner = book.Owner;
            ViewBag.bookImg = book.ImgLink;
            return View();
        }

        // Creates a new book request
        // POST: Requests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookTitle,RequestID,Requester,Owner,Duration")] Request request)
        {
            if(request.Duration != "1 week" && request.Duration != "2 weeks" && request.Duration != "3 weeks" ||
                _context.Books.Find(request.BookTitle) == null || _context.Users.Find(request.Requester) == null ||
                _context.Users.Find(request.Owner) == null)
            {
                return View(request);
            }

            if (ModelState.IsValid)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // Returns the Edit View, only the owner of the book can edit the request
        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        // POST: Requests/Edit/5
        // Edits the request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestID,Requester,Owner,BookTitle,Duration,Accepted")] Request request)
        {
            if (id != request.RequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.RequestID))
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
            return View(request);
        }

        // Deletes the request. Only the owner of the book can delete the request
        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.RequestID == id);
        }
    }
}
