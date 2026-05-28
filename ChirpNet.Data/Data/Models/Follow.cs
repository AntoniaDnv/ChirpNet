using System;
using System.Collections.Generic;
using System.Text;

namespace ChirpNet.Data.Data.Models
{
    public class Follow
    {
        public int Id { get; set; } 
        public DateTime CreatedOn {  get; set; }
        public string FollowerId { get; set; } = null!;
        public ApplicationUser Follower { get; set; } = null!;

        public string FollowingId { get; set; } = null!;    
        public ApplicationUser Following { get; set; } = null!;
    }
}
