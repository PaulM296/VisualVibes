using System.ComponentModel.DataAnnotations;

namespace VisualVibes.App.DTOs.PostDtos
{
    public class CreatePostDto
    {
        [Required]
        [StringLength(1000, ErrorMessage = "Caption must have 1000 characters or fewer!")]
        public string Caption { get; set; }

        [Required]
        public string Pictures { get; set; }
    }
}
