using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.ConversationDtos
{
    public class ResponseConversationDto
    {
        public Guid Id { get; set; }
        public string FirstParticipantId { get; set; }
        public string SecondParticipantId { get; set; }
    }
}
