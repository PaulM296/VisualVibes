using VisualVibes.Domain.Models;

namespace VisualVibes.App.Interfaces
{
    public interface IUserFollowerRepository
    {
        Task AddFollowerAsync(string followerId, string followingId);
        Task RemoveFollowerAsync(string followerId, string followingId);
        Task<IEnumerable<UserFollower>> GetFollowersByUserIdAsync(string userId);
        Task<IEnumerable<UserFollower>> GetFollowingByUserIdAsync(string userId);
    }
}
