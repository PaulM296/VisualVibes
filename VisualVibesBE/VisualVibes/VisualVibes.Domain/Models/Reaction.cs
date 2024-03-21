namespace VisualVibes.Domain.Models
{
    public class Reaction
    {
        public Guid ReactionId { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
