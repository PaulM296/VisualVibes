using Microsoft.EntityFrameworkCore;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models;
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

        public async Task AddFollowerAsync(Guid followerId, Guid followingId)
        {
            var userFollower = new UserFollower
            {
                FollowerId = followerId,
                FollowingId = followingId
            };

            _context.UserFollower.Add(userFollower);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFollowerAsync(Guid followerId, Guid followingId)
        {
            var userFollower = await _context.UserFollower
                .FirstOrDefaultAsync(uf => uf.FollowerId == followerId && uf.FollowingId == followingId);

            if(userFollower == null)
            {
                throw new EntityNotFoundException($"The follower does not exist, therefore it could not be removed.");
            }

            _context.UserFollower.Remove(userFollower);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserFollower>> GetFollowersByUserIdAsync(Guid userId)
        {
            return await _context.UserFollower
                .Include(uf => uf.Follower)
                .Where(uf => uf.FollowingId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserFollower>> GetFollowingByUserIdAsync(Guid userId)
        {
            return await _context.UserFollower
                .Include(uf => uf.Following)
                .Where(uf => uf.FollowerId == userId)
                .ToListAsync();
        }
    }
}
