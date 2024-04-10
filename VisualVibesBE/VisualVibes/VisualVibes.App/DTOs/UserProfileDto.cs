using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public string Bio { get; set; }
        public DateTime DateOfBirth { get; set; }

        public static UserProfileDto FromUserProfile(UserProfile UserProfile)
        {
            return new UserProfileDto
            {
                Id = UserProfile.Id,
                FirstName = UserProfile.FirstName,
                LastName = UserProfile.LastName,
                Email = UserProfile.Email,
                ProfilePicture = UserProfile.ProfilePicture,
                DateOfBirth = UserProfile.DateOfBirth,
                Bio = UserProfile.Bio
            };
        }
    }
}
