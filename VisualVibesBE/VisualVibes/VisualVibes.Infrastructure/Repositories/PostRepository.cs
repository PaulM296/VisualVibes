using Microsoft.EntityFrameworkCore;
using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(VisualVibesDbContext context, FileSystemLogger logger) : base(context, logger)
        {

        }

        public async Task<List<Post>> GetTopReactedPostsAsync(int count)
        {
            return await _context.Posts
                .OrderByDescending(p => p.Reactions.Count)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Post>> GetPostsByUserIdAsync(Guid userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }
    }
}
