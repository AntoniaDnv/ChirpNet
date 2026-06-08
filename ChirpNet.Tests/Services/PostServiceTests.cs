using ChirpNet.Data.Data;
using ChirpNet.Data.Data.Models;
using ChirpNet.Services.Data.Models.Posts;
using ChirpNet.Services.Data.Services;
using ChirpNet.Tests.Helpers;
using NUnit.Framework;

using System;
using System.Linq;
using System.Threading.Tasks;

using Assert = NUnit.Framework.Assert;

namespace ChirpNet.Tests.Services
{
    [TestFixture]
    public class PostServiceTests
    {
        private ApplicationDbContext dbContext = null!;
        private PostService postService = null!;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = TestDbContextFactory.CreateDbContext();
            this.postService = new PostService(this.dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        [Test]
        public async Task CreateAsync_ShouldAddPostToDatabase()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "user-1",
                UserName = "test@test.com",
                Email = "test@test.com",
                DisplayName = "Test User"
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            PostCreateInputModel inputModel = new PostCreateInputModel
            {
                Content = "This is a test post."
            };

            await this.postService.CreateAsync(inputModel, user.Id);

            Post? post = this.dbContext.Posts.FirstOrDefault();

            Assert.That(post, Is.Not.Null);
            Assert.That(post!.Content, Is.EqualTo("This is a test post."));
            Assert.That(post.AuthorId, Is.EqualTo(user.Id));
            Assert.That(post.IsDeleted, Is.False);
        }

        [Test]
        public async Task GetPublicFeedAsync_ShouldReturnOnlyNotDeletedPosts()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "user-1",
                UserName = "test@test.com",
                Email = "test@test.com",
                DisplayName = "Test User"
            };

            await this.dbContext.Users.AddAsync(user);

            await this.dbContext.Posts.AddRangeAsync(
                new Post
                {
                    Id = 1,
                    Content = "Visible post",
                    AuthorId = user.Id,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Post
                {
                    Id = 2,
                    Content = "Deleted post",
                    AuthorId = user.Id,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = true
                });

            await this.dbContext.SaveChangesAsync();

            var result = await this.postService.GetPublicFeedAsync();

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Content, Is.EqualTo("Visible post"));
        }

        [Test]
        public async Task GetForEditAsync_ShouldReturnPost_WhenUserIsOwner()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "owner-id",
                UserName = "owner@test.com",
                Email = "owner@test.com",
                DisplayName = "Owner"
            };

            await this.dbContext.Users.AddAsync(user);

            Post post = new Post
            {
                Id = 1,
                Content = "Original content",
                AuthorId = user.Id,
                CreatedOn = DateTime.UtcNow
            };

            await this.dbContext.Posts.AddAsync(post);
            await this.dbContext.SaveChangesAsync();

            PostEditInputModel? result = await this.postService.GetForEditAsync(post.Id, user.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(post.Id));
            Assert.That(result.Content, Is.EqualTo("Original content"));
        }

        [Test]
        public async Task GetForEditAsync_ShouldReturnNull_WhenUserIsNotOwner()
        {
            ApplicationUser owner = new ApplicationUser
            {
                Id = "owner-id",
                UserName = "owner@test.com",
                Email = "owner@test.com",
                DisplayName = "Owner"
            };

            ApplicationUser otherUser = new ApplicationUser
            {
                Id = "other-id",
                UserName = "other@test.com",
                Email = "other@test.com",
                DisplayName = "Other"
            };

            await this.dbContext.Users.AddRangeAsync(owner, otherUser);

            Post post = new Post
            {
                Id = 1,
                Content = "Original content",
                AuthorId = owner.Id,
                CreatedOn = DateTime.UtcNow
            };

            await this.dbContext.Posts.AddAsync(post);
            await this.dbContext.SaveChangesAsync();

            PostEditInputModel? result = await this.postService.GetForEditAsync(post.Id, otherUser.Id);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task EditAsync_ShouldUpdatePost_WhenUserIsOwner()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "owner-id",
                UserName = "owner@test.com",
                Email = "owner@test.com",
                DisplayName = "Owner"
            };

            await this.dbContext.Users.AddAsync(user);

            Post post = new Post
            {
                Id = 1,
                Content = "Old content",
                AuthorId = user.Id,
                CreatedOn = DateTime.UtcNow
            };

            await this.dbContext.Posts.AddAsync(post);
            await this.dbContext.SaveChangesAsync();

            PostEditInputModel inputModel = new PostEditInputModel
            {
                Id = post.Id,
                Content = "New content"
            };

            bool result = await this.postService.EditAsync(inputModel, user.Id);

            Post updatedPost = this.dbContext.Posts.First(p => p.Id == post.Id);

            Assert.That(result, Is.True);
            Assert.That(updatedPost.Content, Is.EqualTo("New content"));
            Assert.That(updatedPost.ModifiedOn, Is.Not.Null);
        }

        [Test]
        public async Task EditAsync_ShouldReturnFalse_WhenUserIsNotOwner()
        {
            ApplicationUser owner = new ApplicationUser
            {
                Id = "owner-id",
                UserName = "owner@test.com",
                Email = "owner@test.com",
                DisplayName = "Owner"
            };

            ApplicationUser otherUser = new ApplicationUser
            {
                Id = "other-id",
                UserName = "other@test.com",
                Email = "other@test.com",
                DisplayName = "Other"
            };

            await this.dbContext.Users.AddRangeAsync(owner, otherUser);

            Post post = new Post
            {
                Id = 1,
                Content = "Original content",
                AuthorId = owner.Id,
                CreatedOn = DateTime.UtcNow
            };

            await this.dbContext.Posts.AddAsync(post);
            await this.dbContext.SaveChangesAsync();

            PostEditInputModel inputModel = new PostEditInputModel
            {
                Id = post.Id,
                Content = "Hacked content"
            };

            bool result = await this.postService.EditAsync(inputModel, otherUser.Id);

            Post unchangedPost = this.dbContext.Posts.First(p => p.Id == post.Id);

            Assert.That(result, Is.False);
            Assert.That(unchangedPost.Content, Is.EqualTo("Original content"));
        }

        [Test]
        public async Task DeleteAsync_ShouldSoftDeletePost_WhenUserIsOwner()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "owner-id",
                UserName = "owner@test.com",
                Email = "owner@test.com",
                DisplayName = "Owner"
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

            bool result = await this.postService.DeleteAsync(post.Id, user.Id);

            Post deletedPost = this.dbContext.Posts.First(p => p.Id == post.Id);

            Assert.That(result, Is.True);
            Assert.That(deletedPost.IsDeleted, Is.True);
            Assert.That(deletedPost.ModifiedOn, Is.Not.Null);
        }
    }
}