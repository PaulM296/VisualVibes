using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Post : BaseEntity
    {
        public string UserId { get; set; }

        public AppUser User { get; set; }

        public string? Caption { get; set; }

        public string? Pictures { get; set; }

        public ICollection<Reaction> Reactions { get; set; }

        public ICollection<Comment> Comments {  get; set; }

        public ICollection<FeedPost> FeedPosts { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid ImageId { get; set; }
        public Image Image { get; set; }
        public bool isModerated { get; set; }
    }
}
