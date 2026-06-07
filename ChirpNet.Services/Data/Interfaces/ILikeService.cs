using ChirpNet.Services.Data.Models.Likes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Interfaces
{
    public interface ILikeService
    {
        Task<LikeToggleServiceModel?> ToggleLikeAsync(int postId, string userId);

        Task<bool> IsPostLikedByUserAsync(int postId, string userId);
    }
}
