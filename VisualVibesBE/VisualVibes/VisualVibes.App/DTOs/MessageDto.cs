using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ConversationId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public static MessageDto FromMessage(Message message)
        {
            return new MessageDto
            {
                Id = message.Id,
                UserId = message.UserId,
                ConversationId = message.ConversationId,
                Content = message.Content,
                Timestamp = message.Timestamp
            };
        }
    }
}
