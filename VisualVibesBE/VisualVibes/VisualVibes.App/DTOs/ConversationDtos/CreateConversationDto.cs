namespace VisualVibes.App.DTOs.ConversationDtos
{
    public class CreateConversationDto
    {
        public Guid FirstParticipantId { get; set; }
        public Guid SecondParticipantId { get; set; }
    }
}
