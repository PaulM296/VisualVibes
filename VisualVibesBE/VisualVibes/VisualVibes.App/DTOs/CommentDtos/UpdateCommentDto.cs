using System.ComponentModel.DataAnnotations;

namespace VisualVibes.App.DTOs.CommentDtos
{
    public class UpdateCommentDto
    {
        [Required]
        [StringLength(1500, ErrorMessage = "The comment must have 1500 characters or fewer!")]
        public string Text { get; set; }
    }
}
