using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.ConversationDtos
{
    public class ResponseConversationDto
    {
        public Guid Id { get; set; }
        public Guid FirstParticipantId { get; set; }
        public Guid SecondParticipantId { get; set; }

        public static ResponseConversationDto FromConversation(Conversation conversation)
        {
            return new ResponseConversationDto
            {
                Id = conversation.Id,
                FirstParticipantId = conversation.FirstParticipantId,
                SecondParticipantId = conversation.SecondParticipantId,
            };
        }
    }
}
