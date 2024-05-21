using System.ComponentModel.DataAnnotations;
using VisualVibes.Domain.Enum;

namespace VisualVibes.App.DTOs.ReactionDtos
{
    public class UpdateReactionDto
    {
        [Required]
        [Range(0, 4)]
        public ReactionType ReactionType { get; set; }
    }
}
