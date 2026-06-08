using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Admin
{
    public class AdminDashboardServiceModel
    {
        public int UsersCount { get; set; }

        public int PostsCount { get; set; }

        public int CommentsCount { get; set; }

        public int LikesCount { get; set; }

        public int FollowsCount { get; set; }
    }
}
