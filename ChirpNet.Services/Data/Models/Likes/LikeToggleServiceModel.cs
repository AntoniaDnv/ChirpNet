using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Models.Likes
{
    public class LikeToggleServiceModel
    {
        public bool IsLiked { get; set; }
        public int LikesCount { get; set; }
    }
}
