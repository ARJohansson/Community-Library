using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityLibrary.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using CommunityLibrary.Repos;

namespace CommunityLibrary.Data
{
    public class SeedData
    {
        public static async Task Seed(ApplicationDbContext context,
            IConfiguration configuration, IServiceProvider serviceProvider)
        {
            if (!context.Books.Any())
            {
                /********** Users to Create **********/
                string userName = "PerryJohnson";
                string email = "percy.jackson16@gmail.com";
                string name = "Percy Jackson";
                string password = "Son0fa-C-god";
                string role = configuration["Data:TestUser:Role"];

                await CreateUsers(context, configuration, serviceProvider, userName, email, name, password, role);

                userName = "robinhood";
                email = "HeroOfNottingham@gmail.com";
                name = "Robin Locksley";
                password = "Rich2p00r!";

                await CreateUsers(context, configuration, serviceProvider, userName, email, name, password, role);
                
                userName = "AvatarAang";
                email = "avatar100yrs@gmail.com";
                name = "Aang";
                password = "oldMan-100yrs";

                await CreateUsers(context, configuration, serviceProvider, userName, email, name, password, role);
                
                userName = "Rwby";
                email = "rwbyteamleader@gmail.com";
                name = "Ruby Rose";
                password = "huntr3ssGoals!Remnant";

                await CreateUsers(context, configuration, serviceProvider, userName, email, name, password, role);

                /********** Books to Create **********/

                //  Ender's Game
                string title = "Ender's Game";
                string author = "Orson Scott Card";
                string owner = "AvatarAang";
                DateTime written = new DateTime(1985, 01, 15);
                string imgLink = "endersgame";
                CreateBook(context, title, author, owner, written, imgLink);
                
                // Hitch Hiker's Guide to the Galaxy
                title = "Hitch Hiker's Guide to the Galaxy";
                author = "Douglas Adams";
                owner = "robinhood";
                written = new DateTime(1979, 10, 12);
                imgLink = "hitchhikersguide";
                CreateBook(context, title, author, owner, written, imgLink);

                // The Fellowship of the Ring
                title = "The Fellowship of the Ring";
                author = "J.R.R. Tolkien";
                owner = "robinhood";
                written = new DateTime(1954, 07, 29);
                imgLink = "lotrfellowship";
                CreateBook(context, title, author, owner, written, imgLink);

                // The Two Towers
                title = "The Two Towers";
                author = "J.R.R. Tolkien";
                owner = "robinhood";
                written = new DateTime(1954, 11, 11);
                imgLink = "lotrtowers";
                CreateBook(context, title, author, owner, written, imgLink);

                // Return of the King
                title = "Return of the King";
                author = "J.R.R. Tolkien";
                owner = "robinhood";
                written = new DateTime(1955, 10, 20);
                imgLink = "lotrking";
                CreateBook(context, title, author, owner, written, imgLink);

                // Lion the witch and the wardrobe
                title = "The Lion the Witch and the Wardrobe";
                author = "C.S. Lewis";
                owner = "Rwby";
                written = new DateTime(1950, 10, 16);
                imgLink = "narnialion";
                CreateBook(context, title, author, owner, written, imgLink);

                // The Hunger Games
                title = "The Hunger Games";
                author = "Suzanne Collins";
                owner = "TestUser";
                written = new DateTime(2008, 09, 14);
                imgLink = "thg";
                CreateBook(context, title, author, owner, written, imgLink);

                // Catching Fire
                title = "Catching Fire";
                author = "Suzanne Collins";
                owner = "robinhood";
                written = new DateTime(2009, 09, 01);
                imgLink = "thgfire";
                CreateBook(context, title, author, owner, written, imgLink);

                // Mockingjay
                title = "Mockingjay";
                author = "Suzanne Collins";
                owner = "TestUser";
                written = new DateTime(2010, 08, 24);
                imgLink = "thgmockingjay";
                CreateBook(context, title, author, owner, written, imgLink);

                // The Maze Runner
                title = "The Maze Runner";
                author = "James Dashner";
                owner = "PerryJohnson";
                written = new DateTime(2009, 10, 06);
                imgLink = "tmr";
                CreateBook(context, title, author, owner, written, imgLink);

                // The Scorch Trials
                title = "The Scorch Trials";
                author = "James Dashner";
                owner = "PerryJohnson";
                written = new DateTime(2010, 09, 18);
                imgLink = "tmrscorch";
                CreateBook(context, title, author, owner, written, imgLink);

                // The Death Cure
                title = "The Death Cure";
                author = "James Dashner";
                owner = "PerryJohnson";
                written = new DateTime(2011, 10, 11);
                imgLink = "tmrdeath";
                CreateBook(context, title, author, owner, written, imgLink);

                // Wee Free Men
                title = "Wee Free Men";
                author = "Terry Pratchett";
                owner = "Rwby";
                written = new DateTime(2003, 05, 01);
                imgLink = "weefreemen";
                CreateBook(context, title, author, owner, written, imgLink);

                context.SaveChanges();
            }
        }

        public static void CreateBook(ApplicationDbContext context, string title, string author, string owner, 
            DateTime Written, string imgLink)
        {
            if (context.Books.Count() == 0)
            {
                Book book = new Book()
                {
                    Title = title,
                    Author = author,
                    Owner = owner,
                    Written = Written,
                    ImgLink = imgLink,
                    AverageRating = 4,
                    Availability = true
                };
                context.Books.Add(book);
            } else if(context.Books.First(b => b.Title == title) == null)
            {
                Book book = new Book()
                {
                    Title = title,
                    Author = author,
                    Owner = owner,
                    Written = Written,
                    ImgLink = imgLink,
                    AverageRating = 4,
                    Availability = true
                };
                context.Books.Add(book);
            }
        }

        public static async Task CreateUsers(ApplicationDbContext context,
            IConfiguration configuration, IServiceProvider serviceProvider,
            string userName, string email, string name, string password, string role)
        {
            UserManager<AppUser> userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (await userManager.FindByNameAsync(userName) == null)
            {
                AppUser user = new AppUser()
                {
                    UserName = userName,
                    Email = email,
                    Name = name
                };

                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                    context.Users.Add(user);
                }
            }
        }

    }
}
