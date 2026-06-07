using ChirpNet.Data.Data;
using ChirpNet.Services.Data.Interfaces;
using ChirpNet.Services.Data.Models.Profiles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext dbContext;

        public ProfileService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<ProfilePostServiceModel>> GetMyPostsAsync(string userId)
        {
            return await this.dbContext
                .Posts.AsNoTracking()
                .Where(p=> p.AuthorId == userId && !p.IsDeleted)
                .Select(p => new ProfilePostServiceModel
                {
                    Id = p.Id,
                    Content = p.Content,
                    CreatedOn = p.CreatedOn,
                    LikesCount = p.Likes.Count,
                    CommentsCount = p.Comments.Count(c => !c.IsDeleted)
                })
            .ToListAsync();
        }

        public async Task<ProfileDetailsServiceModel?> GetProfileDetailsAsync(string userId)
        {
            return await this.dbContext.Users
                 .AsNoTracking()
                 .Where(u => u.Id == userId)
                 .Select(u => new ProfileDetailsServiceModel
                 {
                     UserId = u.Id,
                     DisplayName = u.DisplayName ?? u.UserName!,
                     Bio = u.Bio,
                     ProfileImageUrl = u.ProfileImageUrl,
                     CreatedOn = u.CreatedOn,
                     PostsCount = u.Posts.Count(p => !p.IsDeleted),
                     FollowersCount = u.Followers.Count,
                     FollowingCount = u.Following.Count,
                     RecentPosts = u.Posts
                     .Where(p => !p.IsDeleted)
                     .OrderByDescending(p => p.CreatedOn)
                     .Take(10)
                     .Select(p => new ProfilePostServiceModel
                     {
                         Id = p.Id,
                         Content = p.Content,
                         CreatedOn = p.CreatedOn,
                         LikesCount = p.Likes.Count,
                         CommentsCount = p.Comments.Count(c => !c.IsDeleted)
                     }).ToList()
                 }).FirstOrDefaultAsync();
        }
    }
}
