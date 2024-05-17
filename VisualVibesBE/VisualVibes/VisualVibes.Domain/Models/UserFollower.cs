using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Domain.Models
{
    public class UserFollower
    {
        public string FollowerId { get; set; }

        public AppUser Follower { get; set; }

        public string FollowingId { get; set; }

        public AppUser Following { get; set; }
    }
}
