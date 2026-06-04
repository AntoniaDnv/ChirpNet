using ChirpNet.Data.Data;
using ChirpNet.Data.Data.Models;
using ChirpNet.Services.Data.Interfaces;
using ChirpNet.Services.Data.Models.Posts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ChirpNet.Services.Data.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext dbContext;
        public PostService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            
        }

        public async Task<bool> AddCommentAsync(PostCommentInputModel inputModel, string authorId)
        {
            bool postExists = await this.dbContext
                .Posts
                .AnyAsync(p => p.Id == inputModel.PostId && !p.IsDeleted);

            if (!postExists)
            {
                return false;
            }

            Comment comment = new Comment
            {
                Content = inputModel.Content,
                PostId = inputModel.PostId,
                AuthorId = authorId,
                CreatedOn = DateTime.UtcNow
            };

            await this.dbContext.Comments.AddAsync(comment);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task CreateAsync(PostCreateInputModel inputModel, string authorId)
        {
            Post post = new Post
            {
                Content = inputModel.Content,
                AuthorId = authorId,
                CreatedOn = DateTime.UtcNow
            };
            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();
        }

        public async Task<PostDetailsServiceModel?> GetDetailsAsync(int postId)
        {
            return await this.dbContext
                 .Posts
                 .AsNoTracking()
                 .Where(p => p.Id == postId && !p.IsDeleted)
                 .Select(p => new PostDetailsServiceModel
                 {
                     Id = p.Id,
                     Content = p.Content,
                     AuthorId = p.AuthorId,
                     AuthorDisplayName = p.Author.DisplayName ?? p.Author.UserName!,
                     CreatedOn = p.CreatedOn,
                     LikesCount = p.Likes.Count,
                     Comments = p.Comments
                        .Where(c => !c.IsDeleted)
                        .OrderBy(c => c.CreatedOn)
                        .Select(c => new PostCommentServiceModel
                        {
                            Id = c.Id,
                            Content = c.Content,
                            AuthorDisplayName = c.Author.DisplayName ?? c.Author.UserName!,
                            CreatedOn = c.CreatedOn
                        })
                        .ToList(),
                     CommentInput = new PostCommentInputModel
                     {
                         PostId = p.Id                   
                     }

                 }).FirstOrDefaultAsync();
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
