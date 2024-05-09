using VisualVibes.Domain.Enum;

namespace VisualVibes.App.DTOs.ReactionDtos
{
    public class CreateReactionDto
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public ReactionType ReactionType { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
