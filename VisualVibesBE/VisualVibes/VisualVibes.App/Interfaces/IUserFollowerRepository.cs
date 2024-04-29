using VisualVibes.Domain.Models;

namespace VisualVibes.App.Interfaces
{
    public interface IUserFollowerRepository
    {
        Task AddFollowerAsync(Guid followerId, Guid followingId);
        Task RemoveFollowerAsync(Guid followerId, Guid followingId);
        Task<IEnumerable<UserFollower>> GetFollowersByUserIdAsync(Guid userId);
        Task<IEnumerable<UserFollower>> GetFollowingByUserIdAsync(Guid userId);
    }
}
