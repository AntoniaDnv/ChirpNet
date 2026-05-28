using System.ComponentModel.DataAnnotations;

namespace ChirpNet.Data.Data.Models;

public class Post
{
    public int Id { get; set; }

    [Required]
    [MaxLength(280)]
    public string Content { get; set; } = null!;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    [Required]
    public string AuthorId { get; set; } = null!;

    public ApplicationUser Author { get; set; } = null!;

    public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

    public ICollection<Like> Likes { get; set; } = new HashSet<Like>();
}