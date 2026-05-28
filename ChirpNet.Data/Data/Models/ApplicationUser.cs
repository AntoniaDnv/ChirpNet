using Microsoft.AspNetCore.Identity;

namespace ChirpNet.Data.Data.Models;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }

    public string? Bio { get; set; }

    public string? ProfileImageUrl { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public ICollection<Post> Posts { get; set; } = new HashSet<Post>();

    public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

    public ICollection<Like> Likes { get; set; } = new HashSet<Like>();

    public ICollection<Follow> Following { get; set; } = new HashSet<Follow>();

    public ICollection<Follow> Followers { get; set; } = new HashSet<Follow>();
}