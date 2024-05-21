using MediatR;
using System.ComponentModel.DataAnnotations;

namespace VisualVibes.App.DTOs.CommentDtos
{
    public record CreateCommentDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public Guid PostId { get; set; }

        [Required]
        [StringLength(1500, ErrorMessage = "The comment must have 1500 characters or fewer!")]
        public string Text { get; set; }
    }
}
