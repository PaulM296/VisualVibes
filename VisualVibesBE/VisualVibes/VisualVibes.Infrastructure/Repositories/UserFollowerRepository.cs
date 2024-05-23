using Microsoft.EntityFrameworkCore;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;
using VisualVibes.Infrastructure.Exceptions;

namespace VisualVibes.Infrastructure.Repositories
{
    public class UserFollowerRepository : IUserFollowerRepository
    {
        private readonly VisualVibesDbContext _context;

        public UserFollowerRepository(VisualVibesDbContext context)
        {
            _context = context;
        }

        public async Task AddFollowerAsync(string followerId, string followingId)
        {
            var userFollower = new UserFollower
            {
                FollowerId = followerId,
                FollowingId = followingId
            };

            _context.UserFollower.Add(userFollower);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFollowerAsync(string followerId, string followingId)
        {
            var userFollower = await _context.UserFollower
                .FirstOrDefaultAsync(uf => uf.FollowerId == followerId && uf.FollowingId == followingId);

            if (userFollower == null)
            {
                throw new EntityNotFoundException($"The follower does not exist, therefore it could not be removed.");
            }

            var feed = await _context.Feeds.FirstOrDefaultAsync(f => f.UserID == followerId);
            if (feed != null)
            {
                var feedPostsToRemove = await _context.FeedPost
                    .Where(fp => fp.FeedId == feed.Id && fp.Post.UserId == followingId)
                    .ToListAsync();

                _context.FeedPost.RemoveRange(feedPostsToRemove);
            }

            _context.UserFollower.Remove(userFollower);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserFollower>> GetFollowersByUserIdAsync(string userId)
        {
            return await _context.UserFollower
                .Include(uf => uf.Follower)
                .Where(uf => uf.FollowingId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserFollower>> GetFollowingByUserIdAsync(string userId)
        {
            return await _context.UserFollower
                .Include(uf => uf.Following)
                .Where(uf => uf.FollowerId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<FeedPost>> GetFeedPostsByUserIdAsync(Guid feedId, string userId)
        {
            return await _context.FeedPost
            .Include(fp => fp.Post)
            .Where(fp => fp.FeedId == feedId && fp.Post.UserId == userId)
            .ToListAsync();
        }
    }
}
