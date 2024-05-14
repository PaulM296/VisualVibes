using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;
using Microsoft.EntityFrameworkCore;

namespace VisualVibes.Infrastructure.Repositories
{
    public class ReactionRepository : BaseRepository<Reaction>, IReactionRepository
    {
        public ReactionRepository(VisualVibesDbContext context) : base(context)
        {

        }

        public async Task<ICollection<Reaction>> GetAllAsync(Guid postId)
        {
            var reactionsForPost = await _context.Reactions.Where(r => r.PostId == postId).ToListAsync();

            return reactionsForPost;
        }
    }
}
