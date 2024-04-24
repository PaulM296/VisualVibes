using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;
using Microsoft.EntityFrameworkCore;

namespace VisualVibes.Infrastructure.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(VisualVibesDbContext context, FileSystemLogger logger) : base(context, logger)
        {

        }

        public async Task<ICollection<Comment>> GetAllAsync(Guid postId)
        {
            var commentsForPost = await _context.Comments.Where(c => c.PostId == postId).ToListAsync();

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
