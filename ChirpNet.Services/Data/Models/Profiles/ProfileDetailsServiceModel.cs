using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Models.Profiles
{
    public class ProfileDetailsServiceModel
    {
        public string UserId { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public string? Bio { get; set; }

        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public int PostsCount { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }
        public IEnumerable<ProfilePostServiceModel> RecentPosts { get; set; }
        = new List<ProfilePostServiceModel>();
    }
}
