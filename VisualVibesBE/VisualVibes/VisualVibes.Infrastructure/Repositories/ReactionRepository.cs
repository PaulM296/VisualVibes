using System.Xml.Linq;
using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Repositories
{
    public class ReactionRepository : BaseRepository<Reaction>, IReactionRepository
    {
        private readonly FileSystemLogger _logger;
        public ReactionRepository(FileSystemLogger logger) : base(logger)
        {
            _logger = logger;
        }

        public async Task<ICollection<Reaction>> GetAllAsync(Guid postId)
        {
            var reactionsForPost = GetAll().Where(r => r.PostId == postId).ToList();

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
