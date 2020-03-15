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

        // GET: Requests
        public async Task<IActionResult> Index(string? data)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            if (data == null)
            {
                if (requestRepo.CheckForOwnerName(user.UserName))
                {
                    ViewBag.requestsReceived = requestRepo.Requests.Where(e => e.Owner == user.UserName);
                }
                else if (requestRepo.CheckForRequesterName(user.UserName))
                {
                    ViewBag.requestsSent = requestRepo.Requests.Where(e => e.Requester == user.UserName);
                }
                return View(await _context.Requests.ToListAsync());
            }else if(data == "requests")
            {
                if (requestRepo.CheckForRequesterName(user.UserName))
                {
                    ViewBag.requestsSent = requestRepo.Requests.Where(e => e.Requester == user.UserName);
                    return View(ViewBag.requestsSent);
                }
                else 
                    return NotFound();
            }
            else
            {
                if (requestRepo.CheckForOwnerName(user.UserName))
                {
                    ViewBag.requestsReceived = requestRepo.Requests.Where(e => e.Owner == user.UserName);
                    return View(ViewBag.requestsReceived);
                }
                else
                    return NotFound();
            }
        }

        // GET: Requests/Create
        public IActionResult Create(int id)
        {
            Book book = _context.Books.Find(id);
            ViewBag.thisBook = book.Title;
            ViewBag.bookOwner = book.Owner;
            ViewBag.bookImg = book.ImgLink;
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookTitle,RequestID,Requester,Owner,Duration")] Request request)
        {
            if (ModelState.IsValid)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
