using Microsoft.EntityFrameworkCore;
using VisualVibes.App;
using VisualVibes.App.DTOs.PaginationDtos;
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

        public async Task<PaginationResponseDto<FeedPost>> GetPagedFeedPostsByUserIdAsync(string userId, int pageIndex, int pageSize)
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
                var totalPosts = feed.FeedPosts.Count;
                var totalPages = (int)Math.Ceiling(totalPosts / (double)pageSize);

                var pagedFeedPosts = feed.FeedPosts
                    .OrderByDescending(fp => fp.Post.CreatedAt)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return new PaginationResponseDto<FeedPost>(pagedFeedPosts, pageIndex, totalPages);
            }

            return new PaginationResponseDto<FeedPost>(new List<FeedPost>(), pageIndex, 0);
        }
    }
}
