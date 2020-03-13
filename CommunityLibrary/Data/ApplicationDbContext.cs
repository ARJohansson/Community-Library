using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommunityLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityLibrary.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options){ }

        public DbSet<Book> Books { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Review> Reviews { get; set; }


        public static async Task CreateAdminAccount(IServiceProvider serviceProvider, 
            IConfiguration configuration)
        {
            UserManager<AppUser> userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string userName = configuration["Data:AdminUser:Name"];
            string email = configuration["Data:AdminUser:Email"];
            string password = configuration["Data:AdminUser:Password"];
            string role = configuration["Data:AdminUser:Role"];

            if(await userManager.FindByNameAsync(userName) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                AppUser user = new AppUser { UserName = userName, Email = email };
                IdentityResult result = await userManager.CreateAsync(user, password);

                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }

        public static async Task CreateTestUserAccount(IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            UserManager<AppUser> userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string userName = configuration["Data:TestUser:Name"];
            string email = configuration["Data:TestUser:Email"];
            string password = configuration["Data:TestUser:Password"];
            string role = configuration["Data:TestUser:Role"];

            if (await userManager.FindByNameAsync(userName) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                AppUser user = new AppUser { UserName = userName, Email = email };
                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }


    }
}
