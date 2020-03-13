using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityLibrary.Models;
using CommunityLibrary.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommunityLibrary.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        static IBookRepo bookRepo;
        static IReportRepo reportRepo;
        static IReviewRepo reviewRepo;
        static IRequestRepo requestRepo;

        public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signinMgr,
                RoleManager<IdentityRole> roleMgr, IBookRepo bookR, IReportRepo reportR, 
                IReviewRepo reviewR, IRequestRepo requestR)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            roleManager = roleMgr;
            bookRepo = bookR;
            reportRepo = reportR;
            reviewRepo = reviewR;
            requestRepo = requestR;
        }

        // Returns the view of the Login Page, annotation allows anyone to 
        // access the page
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        // Verifies details from the Login Page, annotations allow anyone to 
        // access the page, and validates user information
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Finds the appuser based on the entered username
                AppUser user = await userManager.FindByNameAsync(details.UserName);
                // if the user exists then verify password
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result =
                            await signInManager.PasswordSignInAsync(
                                user, details.Password, false, false);
                    // if the password is correct allow to proceed to requested URL
                    if (result.Succeeded && returnUrl != null && !returnUrl.Contains("/Account"))
                    {
                        return RedirectToAction(returnUrl ?? "/");
                    }
                    else if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                        ModelState.AddModelError(nameof(LoginViewModel.UserName),
                            "Invalid user or password");
                }
                ModelState.AddModelError(nameof(LoginViewModel.UserName),
                    "Invalid user or password");
            }
            return View(details);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }


        // Returns the view from the Sign Up page
        [AllowAnonymous]
        public ViewResult SignUp() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // creates a new appuser using the information provided 
                // in the singup form
                AppUser user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                if (model.Name != null)
                {
                    user.Name = model.Name;
                }

                IdentityResult result =
                    await userManager.CreateAsync(user, model.Password);
                await userManager.AddToRoleAsync(user, "Users");

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Users,Admins")]
        public async Task<ActionResult> Index()
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                ViewBag.currentBooks = bookRepo.Books.Where(e => e.Owner == user.UserName).ToList();
                ViewBag.currentReviews = reviewRepo.Reviews.Where(e => e.Reviewer == user.UserName).ToList();
                ViewBag.currentRequests = requestRepo.Requests.Where(e => e.Requester == user.UserName).ToList();
                ViewBag.currentReceived = requestRepo.Requests.Where(e => e.Owner == user.UserName).ToList();
                return View("Index", user);
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }

    }
}
