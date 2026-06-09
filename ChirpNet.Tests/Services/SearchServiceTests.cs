using ChirpNet.Data.Data;
using ChirpNet.Data.Data.Models;
using ChirpNet.Services.Data.Services;
using ChirpNet.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Tests.Services
{
    [TestFixture]
    public class SearchServiceTests
    {
        private ApplicationDbContext dbContext = null!;
        private SearchService searchService = null!;

        [SetUp]
        public void SetUp()
        {
            this.dbContext =TestDbContextFactory.CreateDbContext();
            this.searchService = new SearchService(this.dbContext); 

        }
        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        [Test]
        public async Task SearchAsync_ShouldReturnEmptyResult_WhenKeywordIsNull()
        {
            var result = await this.searchService.SearchAsync(null);
            Assert.That(result.Posts.Count, Is.EqualTo(0));
            Assert.That(result.Users.Count, Is.EqualTo(0));
        }
        [Test]

        public async Task SearchAsync_ShouldFindPostByContent()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "user-1",
                UserName = "maria@test.com",
                Email = "maria@test.com",
                DisplayName = "Maria"
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Posts.AddRangeAsync(
            new Post
            {
                Id = 1,
                Content = "Learning ASP.NET MVC is fun",
                AuthorId = user.Id,
                CreatedOn = DateTime.UtcNow
            },
            new Post
            {
                Id = 2,
                Content = "Completely unrelated text",
                AuthorId = user.Id,
                CreatedOn = DateTime.UtcNow
            });
            await this.dbContext.SaveChangesAsync();
            var result = await this.searchService.SearchAsync("mvc");

            Assert.That(result.Posts.Count(), Is.EqualTo(1));
            Assert.That(result.Posts.First().Content, Does.Contain("MVC"));
        }
        [Test]
        public async Task SearchAsync_ShouldNotReturnDeletedPosts()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "user-1",
                UserName = "maria@test.com",
                Email = "maria@test.com",
                DisplayName = "Maria"
            };
            await this.dbContext.AddAsync(user);
            await this.dbContext.Posts.AddAsync(new Post
            {
                Id = 1,
                Content = "Hidden MVC post",
                AuthorId = user.Id,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = true
            });
            await this.dbContext.SaveChangesAsync();
            var result = await this.searchService.SearchAsync("mvc");
            Assert.That(result.Posts.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task SearchAsync_ShouldFindUsersByDisplayName()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "user-1",
                UserName = "maria@test.com",
                Email = "maria@test.com",
                DisplayName = "Maria"
            };
            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var result = await this.searchService.SearchAsync("maria");
            Assert.That(result.Users.Count(), Is.EqualTo(1));
            Assert.That(result.Users.First().DisplayName, Is.EqualTo("Maria"));
        }
    }
}
