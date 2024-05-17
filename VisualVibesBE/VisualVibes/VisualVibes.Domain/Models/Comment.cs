using System.ComponentModel.DataAnnotations;

namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Comment : BaseEntity
    {
        public string UserId { get; set; }

        public AppUser User { get; set; }

        public Guid PostId { get; set; }

        public Post Post { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
