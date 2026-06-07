using ChirpNet.Data;
using ChirpNet.Data.Data;
using ChirpNet.Data.Data.Models;
using ChirpNet.Services.Data.Interfaces;
using ChirpNet.Services.Data.Models.Likes;
using Microsoft.EntityFrameworkCore;

namespace ChirpNet.Services.Data.Services;

public class LikeService : ILikeService
{
    private readonly ApplicationDbContext dbContext;

    public LikeService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<LikeToggleServiceModel?> ToggleLikeAsync(int postId, string userId)
    {
        bool postExists = await this.dbContext
            .Posts
            .AnyAsync(p => p.Id == postId && !p.IsDeleted);

        if (!postExists)
        {
            return null;
        }

        Like? existingLike = await this.dbContext
            .Likes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

        bool isLiked;

        if (existingLike == null)
        {
            Like like = new Like
            {
                PostId = postId,
                UserId = userId,
                CreatedOn = DateTime.UtcNow
            };

            await this.dbContext.Likes.AddAsync(like);
            isLiked = true;
        }
        else
        {
            this.dbContext.Likes.Remove(existingLike);
            isLiked = false;
        }

        await this.dbContext.SaveChangesAsync();

        int likesCount = await this.dbContext
            .Likes
            .CountAsync(l => l.PostId == postId);

        return new LikeToggleServiceModel
        {
            IsLiked = isLiked,
            LikesCount = likesCount
        };
    }

    public async Task<bool> IsPostLikedByUserAsync(int postId, string userId)
    {
        return await this.dbContext
            .Likes
            .AnyAsync(l => l.PostId == postId && l.UserId == userId);
    }
}