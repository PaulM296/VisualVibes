using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Domain.Models
{
    public class UserFollower
    {
        public Guid FollowerId { get; set; }

        public User Follower { get; set; }

        public Guid FollowingId { get; set; }

        public User Following { get; set; }
    }
}
