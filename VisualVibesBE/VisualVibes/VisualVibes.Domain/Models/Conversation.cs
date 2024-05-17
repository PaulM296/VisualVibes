using System.ComponentModel.DataAnnotations;

namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Conversation : BaseEntity
    {
        public string FirstParticipantId { get; set; }

        public AppUser FirstParticipant { get; set; }

        public string SecondParticipantId { get; set; }

        public AppUser SecondParticipant { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
