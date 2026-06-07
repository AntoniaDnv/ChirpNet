using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Interfaces
{
    public interface IFollowService
    {
        Task<bool> IsFollowingAsync(string followerId, string followingId);
        Task<bool> FollowAsync(string followerId, string followingId);
        Task<bool> UnfollowAsync (string followerId, string followingId);
    }
}
