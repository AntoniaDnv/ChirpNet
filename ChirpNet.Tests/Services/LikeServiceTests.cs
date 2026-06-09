using ChirpNet.Data;
using ChirpNet.Data.Data;
using ChirpNet.Data.Data.Models;
using ChirpNet.Services.Data.Services;
using ChirpNet.Tests.Helpers;
using NUnit.Framework;

namespace ChirpNet.Tests.Services;

[TestFixture]
public class LikeServiceTests
{
    private ApplicationDbContext dbContext = null!;
    private LikeService likeService = null!;

    [SetUp]
    public void SetUp()
    {
        this.dbContext = TestDbContextFactory.CreateDbContext();
        this.likeService = new LikeService(this.dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        this.dbContext.Dispose();
    }

    [Test]
    public async Task ToggleLikeAsync_ShouldLikePost_WhenPostIsNotLiked()
    {
        ApplicationUser user = new ApplicationUser
        {
            Id = "user-1",
            UserName = "user@test.com",
            Email = "user@test.com",
            DisplayName = "Test User"
        };

        Post post = new Post
        {
            Id = 1,
            Content = "Test post",
            AuthorId = user.Id,
            CreatedOn = DateTime.UtcNow
        };

        await this.dbContext.Users.AddAsync(user);
        await this.dbContext.Posts.AddAsync(post);
        await this.dbContext.SaveChangesAsync();

        var result = await this.likeService.ToggleLikeAsync(post.Id, user.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.IsLiked, Is.True);
        Assert.That(result.LikesCount, Is.EqualTo(1));
        Assert.That(this.dbContext.Likes.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task ToggleLikeAsync_ShouldUnlikePost_WhenPostIsAlreadyLiked()
    {
        ApplicationUser user = new ApplicationUser
        {
            Id = "user-1",
            UserName = "user@test.com",
            Email = "user@test.com",
            DisplayName = "Test User"
        };

        Post post = new Post
        {
            Id = 1,
            Content = "Test post",
            AuthorId = user.Id,
            CreatedOn = DateTime.UtcNow
        };

        Like like = new Like
        {
            PostId = post.Id,
            UserId = user.Id,
            CreatedOn = DateTime.UtcNow
        };

        await this.dbContext.Users.AddAsync(user);
        await this.dbContext.Posts.AddAsync(post);
        await this.dbContext.Likes.AddAsync(like);
        await this.dbContext.SaveChangesAsync();

        var result = await this.likeService.ToggleLikeAsync(post.Id, user.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.IsLiked, Is.False);
        Assert.That(result.LikesCount, Is.EqualTo(0));
        Assert.That(this.dbContext.Likes.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task ToggleLikeAsync_ShouldReturnNull_WhenPostDoesNotExist()
    {
        var result = await this.likeService.ToggleLikeAsync(999, "user-1");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task ToggleLikeAsync_ShouldReturnNull_WhenPostIsDeleted()
    {
        ApplicationUser user = new ApplicationUser
        {
            Id = "user-1",
            UserName = "user@test.com",
            Email = "user@test.com",
            DisplayName = "Test User"
        };

        Post deletedPost = new Post
        {
            Id = 1,
            Content = "Deleted post",
            AuthorId = user.Id,
            CreatedOn = DateTime.UtcNow,
            IsDeleted = true
        };

        await this.dbContext.Users.AddAsync(user);
        await this.dbContext.Posts.AddAsync(deletedPost);
        await this.dbContext.SaveChangesAsync();

        var result = await this.likeService.ToggleLikeAsync(deletedPost.Id, user.Id);

        Assert.That(result, Is.Null);
        Assert.That(this.dbContext.Likes.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task IsPostLikedByUserAsync_ShouldReturnTrue_WhenUserLikedPost()
    {
        ApplicationUser user = new ApplicationUser
        {
            Id = "user-1",
            UserName = "user@test.com",
            Email = "user@test.com",
            DisplayName = "Test User"
        };

        Post post = new Post
        {
            Id = 1,
            Content = "Test post",
            AuthorId = user.Id,
            CreatedOn = DateTime.UtcNow
        };

        Like like = new Like
        {
            PostId = post.Id,
            UserId = user.Id,
            CreatedOn = DateTime.UtcNow
        };

        await this.dbContext.Users.AddAsync(user);
        await this.dbContext.Posts.AddAsync(post);
        await this.dbContext.Likes.AddAsync(like);
        await this.dbContext.SaveChangesAsync();

        bool result = await this.likeService.IsPostLikedByUserAsync(post.Id, user.Id);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task IsPostLikedByUserAsync_ShouldReturnFalse_WhenUserHasNotLikedPost()
    {
        bool result = await this.likeService.IsPostLikedByUserAsync(1, "user-1");

        Assert.That(result, Is.False);
    }
}