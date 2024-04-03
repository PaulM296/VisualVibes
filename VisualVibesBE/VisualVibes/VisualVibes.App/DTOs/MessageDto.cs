using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid ConversationId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public static MessageDto FromMessage(Message message)
        {
            return new MessageDto
            {
                Id = message.Id,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                ConversationId = message.ConversationId,
                Content = message.Content
            };
        }
    }
}
