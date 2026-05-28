using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Data.Data.Models
{
    public class Like
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }= DateTime.UtcNow;
        public int PostId { get; set; } 
        public Post Post { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
