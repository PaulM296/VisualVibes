using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.UserFollowerDtos
{
    public class UserFollowerDto
    {
        public string FollowerId { get; set; }
        public string FollowingId { get; set; }
    }
}
