using VisualVibes.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Reaction : BaseEntity
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid PostId { get; set; }

        public Post Post { get; set; }

        public ReactionType ReactionType { get; set; }

        public DateTime Timestamp { get; set; }

    }
}
