using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChirpNet.Services.Data.Models.Posts
{
    public class PostCommentInputModel
    {
        public int PostId { get; set; }
        [Required(ErrorMessage = "Comment content is required.")]
        [StringLength(300, MinimumLength = 1, ErrorMessage = "Comment must be between 1 and 300 chars.")]
        public string Content { get; set; } = null!;
    }
}
