using ChirpNet.Services.Data.Models.Posts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostFeedServiceModel>> GetPublicFeedAsync();
        Task CreateAsync(PostCreateInputModel inputModel, string authorId);

        Task<PostDetailsServiceModel?> GetDetailsAsync(int postId);
        Task<bool> AddCommentAsync(PostCommentInputModel inputModel, string authorId);

        Task<PostEditInputModel?> GetForEditAsync(int postId, string userId); 
        Task<bool> EditAsync(PostEditInputModel inputModel, string userId);
        Task<bool> DeleteAsync(int postId, string userId);
    }
}
