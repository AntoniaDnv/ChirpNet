using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Admin
{
    public class AdminPostServiceModel
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public string AuthorDisplayName { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }
    }
}
