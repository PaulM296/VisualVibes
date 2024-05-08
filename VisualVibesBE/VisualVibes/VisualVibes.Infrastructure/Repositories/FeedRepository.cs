using Microsoft.EntityFrameworkCore;
using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Repositories
{
    public class FeedRepository : BaseRepository<Feed>, IFeedRepository
    {
        public FeedRepository(VisualVibesDbContext context,FileSystemLogger logger) : base(context, logger)
        {

        }

        public async Task<Feed> GetByUserIdAsync(Guid userId)
        {
            var feed = await _context.Feeds
                .Include(f => f.FeedPosts)
                .FirstOrDefaultAsync(f => f.UserID == userId);

            return feed;
        }
    }
}
