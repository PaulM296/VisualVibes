
namespace VisualVibes.App.DTOs.MessageDtos
{
    public class ResponseMessageDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ConversationId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
