using System.ComponentModel.DataAnnotations;
using VisualVibes.Domain.Enum;

namespace VisualVibes.App.DTOs.UserDtos
{
    public class RegisterUser
    {
        [Required]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Your username must be between 6 abd 30 characters long!")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format!")]
        public string Email { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Your password must be between 8 and 25 characters long!")]
        public string Password { get; set; }

        [Required]
        [Range(0, 1)]
        public Role Role { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [StringLength(200, ErrorMessage = "Bio must have 200 characters or fewer!")]
        public string? Bio {  get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        public string? ProfilePicture { get; set; }
    }
}
