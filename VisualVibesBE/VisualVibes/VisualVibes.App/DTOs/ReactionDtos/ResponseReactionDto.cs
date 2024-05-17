using VisualVibes.Domain.Enum;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.ReactionDtos
{
    public class ResponseReactionDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid PostId { get; set; }
        public ReactionType ReactionType { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
