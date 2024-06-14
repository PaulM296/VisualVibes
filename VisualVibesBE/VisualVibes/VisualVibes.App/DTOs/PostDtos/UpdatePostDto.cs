using System.ComponentModel.DataAnnotations;

namespace VisualVibes.App.DTOs.PostDtos
{
    public class UpdatePostDto
    {
        [Required]
        [StringLength(1000, ErrorMessage = "Caption must have 1000 characters or fewer!")]
        public string Caption { get; set; }

    }
}
