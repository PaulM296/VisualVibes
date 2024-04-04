using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        private static IList<Comment> _comments = new List<Comment>();
        private readonly FileSystemLogger _logger;
        public CommentRepository(FileSystemLogger logger) : base(logger)
        {

        }

        public async Task<ICollection<Comment>> GetAllAsync(Guid postId)
        {
            var commentsForPost = _comments.Where(c => c.PostId == postId).ToList();

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
