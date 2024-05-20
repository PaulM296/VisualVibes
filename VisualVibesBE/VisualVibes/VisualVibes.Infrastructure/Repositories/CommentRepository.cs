using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;
using Microsoft.EntityFrameworkCore;
using VisualVibes.Infrastructure.Exceptions;

namespace VisualVibes.Infrastructure.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(VisualVibesDbContext context) : base(context)
        {

        }

        public async Task<ICollection<Comment>> GetAllAsync(Guid postId)
        {
            var commentsForPost = await _context.Comments.Where(c => c.PostId == postId).ToListAsync();

            return commentsForPost;
        }
    }
}
