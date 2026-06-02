using ChirpNet.Data.Common;
using ChirpNet.Data.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Data.Data.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedAdminAsync(userManager);
            await SeedDemoUsersAndPostsAsync(dbContext, userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager) 
        {
            bool adminRoleExists = await roleManager.RoleExistsAsync(ApplicationConstants.AdministratorRoleName);

            if (!adminRoleExists)
            {
                IdentityRole adminRole = new IdentityRole(ApplicationConstants.AdministratorRoleName);

                await roleManager.CreateAsync(adminRole);
            }
        }

        private static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager) 
        {
            ApplicationUser? adminUser = await userManager.FindByEmailAsync(ApplicationConstants.AdminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = ApplicationConstants.AdminEmail,
                    Email = ApplicationConstants.AdminEmail,
                    EmailConfirmed = true,
                    DisplayName = "ChirpNet Admin",
                    Bio = "Administrator account for managing ChirpNet.",
                    CreatedOn = DateTime.UtcNow
                };

                IdentityResult createResult = await userManager.CreateAsync(
                    adminUser,
                    ApplicationConstants.AdminPassword);

                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(
                        adminUser,
                        ApplicationConstants.AdministratorRoleName);
                }
            }
        }
        private static async Task SeedDemoUsersAndPostsAsync(
       ApplicationDbContext dbContext,
       UserManager<ApplicationUser> userManager)
        {
            bool hasPosts = await dbContext.Posts.AnyAsync();

            if (hasPosts)
            {
                return;
            }

            ApplicationUser? maria = await userManager.FindByEmailAsync("maria@chirpnet.com");

            if (maria == null)
            {
                maria = new ApplicationUser
                {
                    UserName = "maria@chirpnet.com",
                    Email = "maria@chirpnet.com",
                    EmailConfirmed = true,
                    DisplayName = "Maria",
                    Bio = "I love coding and coffee.",
                    CreatedOn = DateTime.UtcNow
                };

                await userManager.CreateAsync(maria, "Maria123!");
            }

            ApplicationUser? ivan = await userManager.FindByEmailAsync("ivan@chirpnet.com");

            if (ivan == null)
            {
                ivan = new ApplicationUser
                {
                    UserName = "ivan@chirpnet.com",
                    Email = "ivan@chirpnet.com",
                    EmailConfirmed = true,
                    DisplayName = "Ivan",
                    Bio = "Student developer building MVC projects.",
                    CreatedOn = DateTime.UtcNow
                };

                await userManager.CreateAsync(ivan, "Ivan123!");
            }

            List<Post> posts = new List<Post>
        {
            new Post
            {
                Content = "Welcome to ChirpNet! This is our first seeded post.",
                AuthorId = maria.Id,
                CreatedOn = DateTime.UtcNow
            },
            new Post
            {
                Content = "Learning ASP.NET Core MVC step by step.",
                AuthorId = ivan.Id,
                CreatedOn = DateTime.UtcNow
            },
            new Post
            {
                Content = "Entity Framework Core migrations are starting to make sense!",
                AuthorId = maria.Id,
                CreatedOn = DateTime.UtcNow
            }
        };

            await dbContext.Posts.AddRangeAsync(posts);
            await dbContext.SaveChangesAsync();
        }
    }
}
