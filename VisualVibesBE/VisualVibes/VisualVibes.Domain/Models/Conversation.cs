namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Conversation : BaseEntity
    {
        public Guid FirstParticipantId { get; set; }
        public Guid SecondParticipantId { get; set;}
        public List<Message> ConversationMessages { get; set; }
    }
}
