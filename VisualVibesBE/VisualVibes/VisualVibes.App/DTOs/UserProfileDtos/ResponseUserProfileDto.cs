using MediatR;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.UserProfileDtos
{
    public class ResponseUserProfileDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public string Bio { get; set; }
        public DateTime DateOfBirth { get; set; }

        //public static ResponseUserProfileDto FromUserProfile(UserProfile UserProfile)
        //{
        //    return new ResponseUserProfileDto
        //    {
        //        Id = UserProfile.Id,
        //        UserId = UserProfile.UserId,
        //        FirstName = UserProfile.FirstName,
        //        LastName = UserProfile.LastName,
        //        Email = UserProfile.Email,
        //        ProfilePicture = UserProfile.ProfilePicture,
        //        DateOfBirth = UserProfile.DateOfBirth,
        //        Bio = UserProfile.Bio
        //    };
        //}
    }
}
