using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs
{
    public class UserFollowerDto
    {
        Guid FollowerId { get; set; }
        Guid FollowingId { get; set; }

        public static UserFollowerDto FromUserFollower(UserFollower UserFollower)
        {
            return new UserFollowerDto
            {
                FollowerId = UserFollower.FollowerId,
                FollowingId = UserFollower.FollowerId
            };
        }
    }
}
