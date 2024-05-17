using VisualVibes.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Reaction : BaseEntity
    {
        public string UserId { get; set; }

        public AppUser User { get; set; }

        public Guid PostId { get; set; }

        public Post Post { get; set; }

        public ReactionType ReactionType { get; set; }

        public DateTime Timestamp { get; set; }

    }
}
