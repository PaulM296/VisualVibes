using Microsoft.EntityFrameworkCore;
using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Repositories
{
    public class FeedRepository : BaseRepository<Feed>, IFeedRepository
    {
        public FeedRepository(VisualVibesDbContext context) : base(context)
        {

        }

        public async Task<Feed> GetByUserIdAsync(string userId)
        {
            var feed = await _context.Feeds
                .Include(f => f.FeedPosts)
                .FirstOrDefaultAsync(f => f.UserID == userId);

            return feed;
        }

        public async Task<List<FeedPost>> GetFeedPostsByUserIdAsync(string userId)
        {
            var feed = await _context.Feeds
                .Include(f => f.FeedPosts)
                    .ThenInclude(fp => fp.Post)
                        .ThenInclude(p => p.User)
                            .ThenInclude(u => u.UserProfile)
                .Include(f => f.FeedPosts)
                    .ThenInclude(fp => fp.Post)
                        .ThenInclude(p => p.Reactions)
                            .ThenInclude(r => r.User)
                .Include(f => f.FeedPosts)
                    .ThenInclude(fp => fp.Post)
                        .ThenInclude(p => p.Comments)
                            .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(f => f.UserID == userId);

            if (feed != null)
            {
                return feed.FeedPosts.OrderBy(fp => fp.Post.CreatedAt).ToList();
            }

            return new List<FeedPost>();
        }
    }
}
