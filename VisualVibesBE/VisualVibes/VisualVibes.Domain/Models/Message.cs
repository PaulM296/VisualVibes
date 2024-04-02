namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Message : BaseEntity
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid ConversationId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
