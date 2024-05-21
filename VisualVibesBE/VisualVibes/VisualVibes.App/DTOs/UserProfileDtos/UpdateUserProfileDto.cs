using System.ComponentModel.DataAnnotations;

namespace VisualVibes.App.DTOs.UserProfileDtos
{
    public class UpdateUserProfileDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        public string? ProfilePicture { get; set; }

        [StringLength(200, ErrorMessage = "Bio must have 200 characters or fewer!")]
        public string? Bio { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
