using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;
using Microsoft.EntityFrameworkCore;
using VisualVibes.App.DTOs.PaginationDtos;

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

        public async Task<int> GetPostTotalReactionNumber(Guid postId)
        {
            var reactionsForPost = await _context.Reactions.CountAsync(r => r.PostId == postId);

            return reactionsForPost;
        }

        public async Task<PaginationResponseDto<Reaction>> GetAllPagedReactionsAsync(Guid postId, int pageIndex, int pageSize)
        {
            var reactionsForPost = await _context.Reactions
                .Where(r => r.PostId == postId)
                .OrderByDescending(r => r.Timestamp)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.Reactions.Where(r => r.PostId == postId).CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginationResponseDto<Reaction>(reactionsForPost, pageIndex, totalPages);
        }
    }
}
