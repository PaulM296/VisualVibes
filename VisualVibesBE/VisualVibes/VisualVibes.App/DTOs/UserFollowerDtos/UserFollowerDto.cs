using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.UserFollowerDtos
{
    public class UserFollowerDto
    {
        public Guid FollowerId { get; set; }
        public Guid FollowingId { get; set; }
    }
}
