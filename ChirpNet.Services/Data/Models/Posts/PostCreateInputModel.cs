using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChirpNet.Services.Data.Models.Posts
{
    public class PostCreateInputModel
    {
        [Required(ErrorMessage = "Post content is required.")]
        [StringLength(280, MinimumLength = 1, ErrorMessage = "Post content must be between 1 and 280 characters.")]
        public string Content { get; set; } = null!;
    }
}
