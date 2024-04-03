namespace VisualVibes.App.DTOs
{
    public class ReactionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public DateTime Timestamp { get; set; }

        public static ReactionDto FromReaction(ReactionDto reaction)
        {
            return new ReactionDto
            {
                Id = reaction.Id,
                UserId = reaction.UserId,
                PostId = reaction.PostId,
                Timestamp = reaction.Timestamp
            };
        }
    }
}
