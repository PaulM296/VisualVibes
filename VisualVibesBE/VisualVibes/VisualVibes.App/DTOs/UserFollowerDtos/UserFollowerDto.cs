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
    }
}
