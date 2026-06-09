using ChirpNet.Data;
using ChirpNet.Data.Data;
using ChirpNet.Data.Data.Models;
using ChirpNet.Services.Data.Services;
using ChirpNet.Tests.Helpers;
using NUnit.Framework;

namespace ChirpNet.Tests.Services;

[TestFixture]
public class FollowServiceTests
{
    private ApplicationDbContext dbContext = null!;
    private FollowService followService = null!;

    [SetUp]
    public void SetUp()
    {
        this.dbContext = TestDbContextFactory.CreateDbContext();
        this.followService = new FollowService(this.dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        this.dbContext.Dispose();
    }

    [Test]
    public async Task FollowAsync_ShouldCreateFollow_WhenUsersAreValid()
    {
        ApplicationUser follower = new ApplicationUser
        {
            Id = "follower-id",
            UserName = "follower@test.com",
            Email = "follower@test.com",
            DisplayName = "Follower"
        };

        ApplicationUser following = new ApplicationUser
        {
            Id = "following-id",
            UserName = "following@test.com",
            Email = "following@test.com",
            DisplayName = "Following"
        };

        await this.dbContext.Users.AddRangeAsync(follower, following);
        await this.dbContext.SaveChangesAsync();

        bool result = await this.followService.FollowAsync(follower.Id, following.Id);

        Assert.That(result, Is.True);
        Assert.That(this.dbContext.Follows.Count(), Is.EqualTo(1));

        Follow follow = this.dbContext.Follows.First();

        Assert.That(follow.FollowerId, Is.EqualTo(follower.Id));
        Assert.That(follow.FollowingId, Is.EqualTo(following.Id));
    }

    [Test]
    public async Task FollowAsync_ShouldReturnFalse_WhenUserTriesToFollowSelf()
    {
        ApplicationUser user = new ApplicationUser
        {
            Id = "user-id",
            UserName = "user@test.com",
            Email = "user@test.com",
            DisplayName = "User"
        };

        await this.dbContext.Users.AddAsync(user);
        await this.dbContext.SaveChangesAsync();

        bool result = await this.followService.FollowAsync(user.Id, user.Id);

        Assert.That(result, Is.False);
        Assert.That(this.dbContext.Follows.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task FollowAsync_ShouldReturnFalse_WhenFollowingUserDoesNotExist()
    {
        ApplicationUser follower = new ApplicationUser
        {
            Id = "follower-id",
            UserName = "follower@test.com",
            Email = "follower@test.com",
            DisplayName = "Follower"
        };

        await this.dbContext.Users.AddAsync(follower);
        await this.dbContext.SaveChangesAsync();

        bool result = await this.followService.FollowAsync(follower.Id, "missing-user-id");

        Assert.That(result, Is.False);
        Assert.That(this.dbContext.Follows.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task FollowAsync_ShouldReturnFalse_WhenAlreadyFollowing()
    {
        ApplicationUser follower = new ApplicationUser
        {
            Id = "follower-id",
            UserName = "follower@test.com",
            Email = "follower@test.com",
            DisplayName = "Follower"
        };

        ApplicationUser following = new ApplicationUser
        {
            Id = "following-id",
            UserName = "following@test.com",
            Email = "following@test.com",
            DisplayName = "Following"
        };

        Follow existingFollow = new Follow
        {
            FollowerId = follower.Id,
            FollowingId = following.Id,
            CreatedOn = DateTime.UtcNow
        };

        await this.dbContext.Users.AddRangeAsync(follower, following);
        await this.dbContext.Follows.AddAsync(existingFollow);
        await this.dbContext.SaveChangesAsync();

        bool result = await this.followService.FollowAsync(follower.Id, following.Id);

        Assert.That(result, Is.False);
        Assert.That(this.dbContext.Follows.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task IsFollowingAsync_ShouldReturnTrue_WhenFollowExists()
    {
        ApplicationUser follower = new ApplicationUser
        {
            Id = "follower-id",
            UserName = "follower@test.com",
            Email = "follower@test.com",
            DisplayName = "Follower"
        };

        ApplicationUser following = new ApplicationUser
        {
            Id = "following-id",
            UserName = "following@test.com",
            Email = "following@test.com",
            DisplayName = "Following"
        };

        Follow follow = new Follow
        {
            FollowerId = follower.Id,
            FollowingId = following.Id,
            CreatedOn = DateTime.UtcNow
        };

        await this.dbContext.Users.AddRangeAsync(follower, following);
        await this.dbContext.Follows.AddAsync(follow);
        await this.dbContext.SaveChangesAsync();

        bool result = await this.followService.IsFollowingAsync(follower.Id, following.Id);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task IsFollowingAsync_ShouldReturnFalse_WhenFollowDoesNotExist()
    {
        bool result = await this.followService.IsFollowingAsync("user-1", "user-2");

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task UnfollowAsync_ShouldRemoveFollow_WhenFollowExists()
    {
        ApplicationUser follower = new ApplicationUser
        {
            Id = "follower-id",
            UserName = "follower@test.com",
            Email = "follower@test.com",
            DisplayName = "Follower"
        };

        ApplicationUser following = new ApplicationUser
        {
            Id = "following-id",
            UserName = "following@test.com",
            Email = "following@test.com",
            DisplayName = "Following"
        };

        Follow follow = new Follow
        {
            FollowerId = follower.Id,
            FollowingId = following.Id,
            CreatedOn = DateTime.UtcNow
        };

        await this.dbContext.Users.AddRangeAsync(follower, following);
        await this.dbContext.Follows.AddAsync(follow);
        await this.dbContext.SaveChangesAsync();

        bool result = await this.followService.UnfollowAsync(follower.Id, following.Id);

        Assert.That(result, Is.True);
        Assert.That(this.dbContext.Follows.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task UnfollowAsync_ShouldReturnFalse_WhenFollowDoesNotExist()
    {
        bool result = await this.followService.UnfollowAsync("user-1", "user-2");

        Assert.That(result, Is.False);
    }
}