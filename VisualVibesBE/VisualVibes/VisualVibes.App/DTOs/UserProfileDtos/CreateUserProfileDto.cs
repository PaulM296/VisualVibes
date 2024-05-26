using System.ComponentModel.DataAnnotations;
using VisualVibes.App.DTOs.ImageDtos;

namespace VisualVibes.App.DTOs.UserProfileDtos
{
    public class CreateUserProfileDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public CreateImageDto ProfilePicture { get; set; }

        [StringLength(200, ErrorMessage = "Bio must have 200 characters or fewer!")]
        public string? Bio { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
