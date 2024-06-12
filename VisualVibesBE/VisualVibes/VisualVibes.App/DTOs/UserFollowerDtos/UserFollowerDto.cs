using System.ComponentModel.DataAnnotations;
using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.UserFollowerDtos
{
    public class UserFollowerDto
    {
        [Required]
        public string FollowerId { get; set; }

        [Required]
        public string FollowingId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public Guid ImageId { get; set; }
    }
}
