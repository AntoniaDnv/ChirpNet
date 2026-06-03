using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Models.Posts
{
    public class PostFeedServiceModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public string AuthorId { get; set; } = null!;
        public string AuthorDisplayName { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }
    }
}
