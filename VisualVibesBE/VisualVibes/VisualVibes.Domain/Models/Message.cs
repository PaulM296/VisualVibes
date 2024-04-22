using System.ComponentModel.DataAnnotations;

namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Message : BaseEntity
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid ConversationId { get; set; }

        public Conversation Conversation { get; set; }

        public string Content { get; set; }

        public DateTime Timestamp { get; set; }

    }
}
