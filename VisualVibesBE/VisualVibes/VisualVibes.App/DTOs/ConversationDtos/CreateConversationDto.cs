using System.ComponentModel.DataAnnotations;

namespace VisualVibes.App.DTOs.ConversationDtos
{
    public class CreateConversationDto
    {
        [Required]
        public string FirstParticipantId { get; set; }

        [Required]
        public string SecondParticipantId { get; set; }
    }
}
