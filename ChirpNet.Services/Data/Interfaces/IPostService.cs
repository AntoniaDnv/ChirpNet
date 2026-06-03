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
    }
}
