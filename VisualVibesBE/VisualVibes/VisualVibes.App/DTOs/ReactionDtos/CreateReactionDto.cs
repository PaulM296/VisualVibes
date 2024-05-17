using VisualVibes.Domain.Enum;

namespace VisualVibes.App.DTOs.ReactionDtos
{
    public class CreateReactionDto
    {
        public string UserId { get; set; }
        public Guid PostId { get; set; }
        public ReactionType ReactionType { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
