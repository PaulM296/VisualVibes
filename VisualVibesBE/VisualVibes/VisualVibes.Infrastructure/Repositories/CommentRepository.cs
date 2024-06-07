using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;
using Microsoft.EntityFrameworkCore;
using VisualVibes.App.DTOs.PaginationDtos;

namespace VisualVibes.Infrastructure.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(VisualVibesDbContext context) : base(context)
        {

        }

        public async Task<PaginationResponseDto<Comment>> GetAllPagedCommentsAsync(Guid postId, int pageIndex, int pageSize)
        {
            var commentsForPost = await _context.Comments
                .Where(c => c.PostId == postId)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.Comments.Where(c => c.PostId == postId).CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginationResponseDto<Comment>(commentsForPost, pageIndex, totalPages);
        }

        public async Task<ICollection<Comment>> GetAllAsync(Guid postId)
        {
            var commentsForPost = await _context.Comments.Where(c => c.PostId == postId).ToListAsync();

            return commentsForPost;
        }

        public async Task<int> GetPostTotalCommentNumber(Guid postId)
        {
            var reactionsForPost = await _context.Comments.CountAsync(r => r.PostId == postId);

            return reactionsForPost;
        }
    }
}
