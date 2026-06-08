using ChirpNet.Data;
using ChirpNet.Data.Data;
using ChirpNet.Services.Data.Admin;
using ChirpNet.Services.Data.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace ChirpNet.Services.Data.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext dbContext;

    public AdminService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<AdminDashboardServiceModel> GetDashboardAsync()
    {
        return new AdminDashboardServiceModel
        {
            UsersCount = await this.dbContext.Users.CountAsync(),
            PostsCount = await this.dbContext.Posts.CountAsync(p => !p.IsDeleted),
            CommentsCount = await this.dbContext.Comments.CountAsync(c => !c.IsDeleted),
            LikesCount = await this.dbContext.Likes.CountAsync(),
            FollowsCount = await this.dbContext.Follows.CountAsync()
        };
    }

    public async Task<IEnumerable<AdminPostServiceModel>> GetAllPostsAsync()
    {
        return await this.dbContext
            .Posts
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedOn)
            .Select(p => new AdminPostServiceModel
            {
                Id = p.Id,
                Content = p.Content,
                AuthorDisplayName = p.Author.DisplayName ?? p.Author.UserName!,
                CreatedOn = p.CreatedOn,
                IsDeleted = p.IsDeleted,
                LikesCount = p.Likes.Count,
                CommentsCount = p.Comments.Count(c => !c.IsDeleted)
            })
            .ToListAsync();
    }

    public async Task<bool> DeletePostAsync(int postId)
    {
        var post = await this.dbContext
            .Posts
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post == null)
        {
            return false;
        }

        post.IsDeleted = true;
        post.ModifiedOn = DateTime.UtcNow;

        await this.dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RestorePostAsync(int postId)
    {
        var post = await this.dbContext
            .Posts
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post == null)
        {
            return false;
        }

        post.IsDeleted = false;
        post.ModifiedOn = DateTime.UtcNow;

        await this.dbContext.SaveChangesAsync();

        return true;
    }
}