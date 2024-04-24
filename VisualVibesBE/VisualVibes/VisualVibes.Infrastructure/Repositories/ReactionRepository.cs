using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;
using Microsoft.EntityFrameworkCore;

namespace VisualVibes.Infrastructure.Repositories
{
    public class ReactionRepository : BaseRepository<Reaction>, IReactionRepository
    {
        public ReactionRepository(VisualVibesDbContext context, FileSystemLogger logger) : base(context, logger)
        {

        }

        public async Task<ICollection<Reaction>> GetAllAsync(Guid postId)
        {
            var reactionsForPost = await _context.Reactions.Where(r => r.PostId == postId).ToListAsync();

            if (reactionsForPost.Count == 0)
            {
                await _logger.LogAsync(nameof(GetAllAsync), isSuccess: false);
            }
            else
            {
                await _logger.LogAsync(nameof(GetAllAsync), isSuccess: true);
            }

            return reactionsForPost;
        }
    }
}
