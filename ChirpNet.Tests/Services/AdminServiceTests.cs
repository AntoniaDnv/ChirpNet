using ChirpNet.Data.Data;
using ChirpNet.Data.Data.Models;
using ChirpNet.Services.Data.Services;
using ChirpNet.Tests.Helpers;
using NUnit.Framework;

namespace ChirpNet.Tests.Services
{

    [TestFixture]
    public class AdminServiceTests
    {
        private ApplicationDbContext dbContext = null!;
        private AdminService adminService = null!;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = TestDbContextFactory.CreateDbContext();
            this.adminService = new AdminService(this.dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        [Test]
        public async Task GetDashboardAsync_ShouldReturnCorrectCounts()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "user-1",
                UserName = "user@test.com",
                Email = "user@test.com",
                DisplayName = "User"
            };

            await this.dbContext.Users.AddAsync(user);

            Post post = new Post
            {
                Id = 1,
                Content = "Test post",
                AuthorId = user.Id,
                CreatedOn = DateTime.UtcNow
            };

            await this.dbContext.Posts.AddAsync(post);

            await this.dbContext.Comments.AddAsync(new Comment
            {
                Id = 1,
                Content = "Test comment",
                PostId = post.Id,
                AuthorId = user.Id,
                CreatedOn = DateTime.UtcNow
            });

            await this.dbContext.Likes.AddAsync(new Like
            {
                PostId = post.Id,
                UserId = user.Id,
                CreatedOn = DateTime.UtcNow
            });

            await this.dbContext.SaveChangesAsync();

            var result = await this.adminService.GetDashboardAsync();

            Assert.That(result.UsersCount, Is.EqualTo(1));
            Assert.That(result.PostsCount, Is.EqualTo(1));
            Assert.That(result.CommentsCount, Is.EqualTo(1));
            Assert.That(result.LikesCount, Is.EqualTo(1));
        }

        [Test]
        public async Task DeletePostAsync_ShouldSoftDeletePost_WhenPostExists()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "user-1",
                UserName = "user@test.com",
                Email = "user@test.com",
                DisplayName = "User"
            };

            await this.dbContext.Users.AddAsync(user);

            Post post = new Post
            {
                Id = 1,
                Content = "Post to delete",
                AuthorId = user.Id,
                CreatedOn = DateTime.UtcNow
            };

            await this.dbContext.Posts.AddAsync(post);
            await this.dbContext.SaveChangesAsync();

            bool result = await this.adminService.DeletePostAsync(post.Id);

            Post deletedPost = this.dbContext.Posts.First(p => p.Id == post.Id);

            Assert.That(result, Is.True);
            Assert.That(deletedPost.IsDeleted, Is.True);
            Assert.That(deletedPost.ModifiedOn, Is.Not.Null);
        }

        [Test]
        public async Task DeletePostAsync_ShouldReturnFalse_WhenPostDoesNotExist()
        {
            bool result = await this.adminService.DeletePostAsync(999);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task RestorePostAsync_ShouldRestoreDeletedPost_WhenPostExists()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "user-1",
                UserName = "user@test.com",
                Email = "user@test.com",
                DisplayName = "User"
            };

            await this.dbContext.Users.AddAsync(user);

            Post post = new Post
            {
                Id = 1,
                Content = "Deleted post",
                AuthorId = user.Id,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = true
            };

            await this.dbContext.Posts.AddAsync(post);
            await this.dbContext.SaveChangesAsync();

            bool result = await this.adminService.RestorePostAsync(post.Id);

            Post restoredPost = this.dbContext.Posts.First(p => p.Id == post.Id);

            Assert.That(result, Is.True);
            Assert.That(restoredPost.IsDeleted, Is.False);
            Assert.That(restoredPost.ModifiedOn, Is.Not.Null);
        }

        [Test]
        public async Task RestorePostAsync_ShouldReturnFalse_WhenPostDoesNotExist()
        {
            bool result = await this.adminService.RestorePostAsync(999);

            Assert.That(result, Is.False);
        }
    }
}