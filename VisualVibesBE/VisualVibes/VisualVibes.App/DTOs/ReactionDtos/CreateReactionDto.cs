using System.ComponentModel.DataAnnotations;
using VisualVibes.Domain.Enum;

namespace VisualVibes.App.DTOs.ReactionDtos
{
    public class CreateReactionDto
    {
        [Required]
        public Guid PostId { get; set; }

        [Required]
        [Range(0, 4)]
        public ReactionType ReactionType { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}
