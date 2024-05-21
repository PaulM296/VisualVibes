using System.ComponentModel.DataAnnotations;

namespace VisualVibes.App.DTOs.MessageDtos
{
    public class CreateMessageDto
    {
        [Required]
        public Guid ConversationId { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "Caption must have 5000 characters or fewer!")]
        public string Content { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}
