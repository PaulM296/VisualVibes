namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Comment : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
