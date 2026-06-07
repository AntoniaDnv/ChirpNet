using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Services.Data.Models.Profiles
{
    public class ProfilePostServiceModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedOn { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

    }
}
