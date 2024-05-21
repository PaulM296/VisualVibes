using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;
using Microsoft.EntityFrameworkCore;
using VisualVibes.App.DTOs.PaginationDtos;


namespace VisualVibes.Infrastructure.Repositories
{
    public class ConversationRepository : BaseRepository<Conversation>, IConversationRepository
    {
        public ConversationRepository(VisualVibesDbContext context) : base(context)
        {

        }

        public async Task<List<Conversation>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Conversations
            .Where(c => c.FirstParticipantId == userId || c.SecondParticipantId == userId)
            .ToListAsync();
        }

        public async Task<PaginationResponseDto<Conversation>> GetAllPagedConversationsByUserIdAsync(string userId, int pageIndex, int pageSize)
        {
            var conversations = await _context.Conversations
                .Where(u => u.FirstParticipantId == userId || u.SecondParticipantId == userId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.Conversations.Where(u => u.FirstParticipantId == userId ||
            u.SecondParticipantId == userId).CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginationResponseDto<Conversation>(conversations, pageIndex, totalPages);
        }
    }
}
