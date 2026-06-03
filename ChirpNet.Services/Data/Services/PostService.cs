using ChirpNet.Data.Data;
using ChirpNet.Services.Data.Interfaces;
using ChirpNet.Services.Data.Models.Posts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext dbContext;
        public PostService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            
        }
        public async Task<IEnumerable<PostFeedServiceModel>> GetPublicFeedAsync()
        {
            return await dbContext
                .Posts
                .AsNoTracking ()
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p=>p.CreatedOn)
                .Select( p=> new PostFeedServiceModel
                {
                    Id = p.Id,
                    Content = p.Content,
                    AuthorId = p.AuthorId,
                    AuthorDisplayName = p.Author.DisplayName ?? p.Author.UserName!,
                    CreatedOn = p.CreatedOn,
                    LikesCount = p.Likes.Count(),
                    CommentsCount = p.Comments.Count()


                }).ToListAsync();
        }
    }
}
