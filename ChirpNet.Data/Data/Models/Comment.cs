using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChirpNet.Data.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(300)]
        public string Content { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public bool isIsDeleted { get; set; }   
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
        public string AuthorId { get; set; } = null!;
        public ApplicationUser Author { get; set; } = null!;

    }
}
