namespace VisualVibes.App.DTOs.UserProfileDtos
{
    public class UpdateUserProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
