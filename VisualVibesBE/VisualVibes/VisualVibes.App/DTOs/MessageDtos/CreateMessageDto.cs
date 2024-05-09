namespace VisualVibes.App.DTOs.MessageDtos
{
    public class CreateMessageDto
    {
        public Guid UserId { get; set; }
        public Guid ConversationId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
