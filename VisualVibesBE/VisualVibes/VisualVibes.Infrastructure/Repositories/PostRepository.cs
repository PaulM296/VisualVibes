using Microsoft.EntityFrameworkCore;
using VisualVibes.App;
using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(VisualVibesDbContext context) : base(context)
        {

        }

        public async Task<List<Post>> GetTopReactedPostsAsync(int count)
        {
            return await _context.Posts
                .OrderByDescending(p => p.Reactions.Count)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Post>> GetPostsByUserIdAsync(string userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                    .ThenInclude(u => u.UserProfile)
                .Include(p => p.Reactions)
                    .ThenInclude(r => r.User)
                    .ThenInclude(u => u.UserProfile)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<PaginationResponseDto<Post>> GetPaginatedPostsByUserIdAsync(string userId, int pageIndex, int pageSize)
        {
            var query = _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                    .ThenInclude(u => u.UserProfile)
                .Include(p => p.Reactions)
                    .ThenInclude(r => r.User)
                    .ThenInclude(u => u.UserProfile)
                .OrderByDescending(p => p.CreatedAt);

            var totalItems = await query.CountAsync();
            var posts = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return new PaginationResponseDto<Post>(posts, pageIndex, totalPages);
        }
    }
}
