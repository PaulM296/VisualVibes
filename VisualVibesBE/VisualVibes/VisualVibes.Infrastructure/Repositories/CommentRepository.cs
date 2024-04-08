using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        private readonly FileSystemLogger _logger;
        public CommentRepository(FileSystemLogger logger) : base(logger)
        {
            _logger = logger;
        }

        public async Task<ICollection<Comment>> GetAllAsync(Guid postId)
        {
            var commentsForPost = GetAll().Where(c => c.PostId == postId).ToList();

            if (commentsForPost.Count == 0)
            {
                await _logger.LogAsync(nameof(GetAllAsync), isSuccess: false);
            }
            else
            {
                await _logger.LogAsync(nameof(GetAllAsync), isSuccess: true);
            }

            return commentsForPost;
        }
    }
}
