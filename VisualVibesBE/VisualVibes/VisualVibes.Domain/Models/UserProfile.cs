using System.ComponentModel.DataAnnotations;

namespace VisualVibes.Domain.Models.BaseEntity
{
    public class UserProfile : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserId { get; set; }

        public AppUser User { get; set; }

        public string? ProfilePicture { get; set; }

        public string? Bio {  get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
