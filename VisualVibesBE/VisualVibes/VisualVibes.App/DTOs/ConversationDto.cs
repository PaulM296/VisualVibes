using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs
{
    public class ConversationDto
    {
        public Guid Id { get; set; }
        public Guid FirstParticipantId { get; set; }
        public Guid SecondParticipantId { get; set; }

        public static ConversationDto FromConversation(Conversation conversation)
        {
            return new ConversationDto
            {
                Id = conversation.Id,
                FirstParticipantId = conversation.FirstParticipantId,
                SecondParticipantId = conversation.SecondParticipantId,
            };
        }
    }
}
