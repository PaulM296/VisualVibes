namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Reaction : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
