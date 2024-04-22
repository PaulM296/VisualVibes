using System.ComponentModel.DataAnnotations;

namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Conversation : BaseEntity
    {
        public Guid FirstParticipantId { get; set; }

        public User FirstParticipant { get; set; }

        public Guid SecondParticipantId { get; set; }

        public User SecondParticipant { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
